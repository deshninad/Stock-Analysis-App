//NINAD DESHPANDE
//U50461301
//This comprehensive code defines a stock analysis tool implemented in a 
//    Windows Forms application using C#. It allows users to load candlestick data 
//    from CSV files, visualize it on a candlestick chart (OHLC), and analyze selected
//    ranges for specific patterns and trends. Users can filter data by date range, 
//    select patterns to highlight, and toggle a "selection mode" to analyze specific 
//    candlesticks. The tool calculates and displays Fibonacci retracement levels, 
//    wave beauty (a metric for Fibonacci confirmations within the selected range), 
//    and plots a "Beauty vs. Price" graph in a secondary chart area. It dynamically 
//    normalizes chart axes for better visualization and provides interactive elements,
//    such as annotations for peaks, valleys, and Fibonacci levels with confirmation points. The code also includes robust error handling, allowing users to handle missing or malformed data gracefully. Overall, this tool combines interactive charting with analytical capabilities to assist in technical stock analysis.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.AxHost;

namespace Project1
{
    public partial class Form_StockView : Form
    {
        // Binding list to store and bind SmartCandlestick objects to the UI
        private BindingList<SmartCandlestick> candlestickList { get; set; }
        // Temporary list to store candlesticks read from the file
        private List<SmartCandlestick> tempList;



        public Form_StockView()
        {
            InitializeComponent();

            // Allow multiple file selection in the OpenFileDialog
            openFileDialog_TicketChooser.Multiselect = true;

            // Set default values for the date pickers
            dateTimePicker_startDate.Value = DateTime.Now.AddYears(-1);
            dateTimePicker_endDate.Value = DateTime.Now;

            // Attach event handlers to update data on date change
            dateTimePicker_startDate.ValueChanged += DateTimePicker_ValueChanged;
            dateTimePicker_endDate.ValueChanged += DateTimePicker_ValueChanged;

            // Populate the dropdown menu with candlestick pattern options
            comboBox_PatternSelector.Items.AddRange(new string[]
            {
                "Bullish", "Bearish", "Neutral", "Marubozu", "Hammer", "Doji", "Dragonfly Doji", "Gravestone Doji"
            });

            // Attach event handler for pattern selection
            comboBox_PatternSelector.SelectedIndexChanged += ComboBox_PatternSelector_SelectedIndexChanged;

            // Attach event handler for mouse clicks on the chart
            chart_candlesticks.MouseClick += chart_candlesticks_MouseClick_1;

            chart_candlesticks.PostPaint += Chart_candlesticks_PostPaint;

        }

        // Event handler for loading data from files
        private void button_loadData_Click(object sender, EventArgs e)
        {
            if (openFileDialog_TicketChooser.ShowDialog() == DialogResult.OK) // Show the file dialog
            {
                var files = openFileDialog_TicketChooser.FileNames; // Get selected files

                if (files.Length > 0)
                {
                    bool isFirstFile = true; // Track the first file to load data into the current form

                    foreach (var filePath in files)
                    {
                        if (isFirstFile)
                        {
                            // Read data from the first file and display it
                            tempList = ReadSmartCandlestickDataFromFile(filePath);
                            filterCandlesticks();
                            CreateChartAreas();
                            normalizeChart();
                            displayCandlestick();
                            isFirstFile = false;
                        }
                        else
                        {
                            // Open additional files in new forms
                            var newForm = new Form_StockView();
                            newForm.LoadSmartCandlestickData(filePath);
                            newForm.Show();
                        }
                    }
                }
            }
        }

        // Method to read candlestick data from a CSV file and return it as a list
        private List<SmartCandlestick> ReadSmartCandlestickDataFromFile(string filePath)
        {
            var candlesticks = new List<SmartCandlestick>();

            if (File.Exists(filePath)) // Check if the file exists
            {
                using (var reader = new StreamReader(filePath))
                {
                    reader.ReadLine(); // Skip the header line
                    string line;
                    while ((line = reader.ReadLine()) != null) // Read each line
                    {
                        try
                        {
                            candlesticks.Add(new SmartCandlestick(line)); // Create SmartCandlestick objects
                        }
                        catch
                        {
                            // Show error message if line parsing fails
                            MessageBox.Show($"Error processing line in file: {filePath}", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return null;
                        }
                    }
                }
            }
            else
            {
                // Show error message if file does not exist
                MessageBox.Show("Error! File does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return candlesticks;
        }

        // Method to load candlestick data into the form
        public void LoadSmartCandlestickData(string filePath)
        {
            tempList = ReadSmartCandlestickDataFromFile(filePath);
            filterCandlesticks();
            CreateChartAreas();
            normalizeChart();
            displayCandlestick();
        }

        // Method to filter candlesticks based on the selected date range
        private void filterCandlesticks()
        {
            if (tempList == null)
            {
                MessageBox.Show("No data loaded. Please load a file first.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Filter candlesticks within the selected date range
            candlestickList = new BindingList<SmartCandlestick>(
                tempList.Where(cs => cs.date >= dateTimePicker_startDate.Value && cs.date <= dateTimePicker_endDate.Value).ToList());
        }

        // Method to create chart areas for OHLC and volume charts
        private void CreateChartAreas()
        {
            chart_candlesticks.ChartAreas.Clear();

            var chartAreaOHLC = new ChartArea("ChartArea_OHLC")
            {
                AxisX = { Title = "Date", IntervalType = DateTimeIntervalType.Auto, LabelStyle = { Format = "MM/dd/yyyy" } },
                AxisY = { Title = "Price" },
                Position = new ElementPosition(5, 5, 90, 40) // Top 40% of the chart control
            };

            var chartAreaBeauty = new ChartArea("ChartArea_Beauty")
            {
                AxisX = { Title = "Price" },
                AxisY = { Title = "Beauty" },
                AlignWithChartArea = "ChartArea_OHLC",
                Position = new ElementPosition(5, 55, 90, 40), // Increase the height allocated
                InnerPlotPosition = new ElementPosition(10, 10, 80, 80) // Adjust to provide more space for the graph
            };


            chart_candlesticks.ChartAreas.Add(chartAreaOHLC);
            chart_candlesticks.ChartAreas.Add(chartAreaBeauty);
        }


        private void normalizeChart()
        {
            if (candlestickList == null || candlestickList.Count == 0)
            {
                MessageBox.Show("No data available for chart normalization.", "Chart Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Normalize the Y-axis for the OHLC chart
            var minPrice = (double)(candlestickList.Min(c => c.low) * 0.98m);
            var maxPrice = (double)(candlestickList.Max(c => c.high) * 1.02m);
            chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = minPrice;
            chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = maxPrice;

            // Normalize the Y-axis for the Beauty chart
            var beautySeries = chart_candlesticks.Series.FirstOrDefault(s => s.Name == "Series_Beauty");
            if (beautySeries != null && beautySeries.Points.Count > 0)
            {
                double maxBeauty = beautySeries.Points.Max(p => p.YValues[0]);
                double minBeauty = beautySeries.Points.Min(p => p.YValues[0]);

                // Ensure a buffer to make the line visible
                chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisY.Minimum = minBeauty - 1;
                chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisY.Maximum = maxBeauty + 1;

                // Log for debugging
                //MessageBox.Show($"Normalized Beauty Chart Y-Axis: Min={Math.Min(0, minBeauty) - 1}, Max={maxBeauty + 1}");
            }
            else
            {
                // Default range for Beauty chart if no data is available
                chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisY.Minimum = 0;
                chart_candlesticks.ChartAreas["ChartArea_Beauty"].AxisY.Maximum = 50;
                //MessageBox.Show("Beauty chart is empty. Using default range.");
            }
        }






        // Method to display candlestick and volume data on the chart
        private void displayCandlestick()
        {
            if (candlestickList == null || candlestickList.Count == 0)
            {
                MessageBox.Show("No data to display.", "Display Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            chart_candlesticks.Series.Clear(); // Clear existing series

            // Define the series for OHLC chart
            var seriesOHLC = new Series("Series_OHLC")
            {
                ChartType = SeriesChartType.Candlestick,
                XValueMember = "date",
                YValueMembers = "high, low, open, close",
                ChartArea = "ChartArea_OHLC",
                XValueType = ChartValueType.DateTime // Ensure X-axis binds to DateTime
            };
            seriesOHLC["PriceUpColor"] = "Green";
            seriesOHLC["PriceDownColor"] = "Red";

            // Bind data to the chart
            chart_candlesticks.DataSource = candlestickList;
            chart_candlesticks.Series.Add(seriesOHLC);
            chart_candlesticks.DataBind();

            // Compute and plot Beauty vs Price
            ComputeAndPlotBeautyVsPrice();

            AddPeakValleyAnnotations(); // Add peak and valley annotations
        }



        // Method to add annotations for peaks and valleys
        private void AddPeakValleyAnnotations()
        {
            chart_candlesticks.Annotations.Clear(); // Clear existing annotations

            for (int i = 1; i < candlestickList.Count - 1; i++)
            {
                var current = candlestickList[i];
                var previous = candlestickList[i - 1];
                var next = candlestickList[i + 1];

                // Extract the high and low values
                decimal currentHigh = current.high;
                decimal currentLow = current.low;

                decimal previousHigh = previous.high;
                decimal nextHigh = next.high;

                decimal previousLow = previous.low;
                decimal nextLow = next.low;

                // Normalize high and low values to chart coordinates
                double normalizedHigh = (double)currentHigh;
                double normalizedLow = (double)currentLow;

                // Add a green line for peaks
                if (currentHigh > previousHigh && currentHigh > nextHigh)
                {
                    AddHorizontalLine(chart_candlesticks, normalizedHigh, System.Drawing.Color.Green);
                }

                // Add a red line for valleys
                if (currentLow < previousLow && currentLow < nextLow)
                {
                    AddHorizontalLine(chart_candlesticks, normalizedLow, System.Drawing.Color.Red);
                }
            }
        }

        /// <summary>
        /// Draws a horizontal line at the specified price on the chart.
        /// </summary>
        private void AddHorizontalLine(Chart chart, double price, System.Drawing.Color color)
        {
            var line = new HorizontalLineAnnotation
            {
                AxisX = chart.ChartAreas["ChartArea_OHLC"].AxisX,
                AxisY = chart.ChartAreas["ChartArea_OHLC"].AxisY,
                ClipToChartArea = chart.ChartAreas["ChartArea_OHLC"].Name,
                IsInfinitive = true,
                Y = price,
                LineColor = color,
                LineWidth = 1
            };

            chart.Annotations.Add(line);
        }


        // Event handler for pattern selection
        private void ComboBox_PatternSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedPattern = comboBox_PatternSelector.SelectedItem.ToString();
            HighlightPattern(selectedPattern);
        }

        // Method to highlight candlesticks matching the selected pattern
        private void HighlightPattern(string patternName)
        {
            chart_candlesticks.Annotations.Clear(); // Clear existing annotations

            if (!patternCheckers.ContainsKey(patternName))
            {
                MessageBox.Show("Invalid pattern selected.", "Pattern Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            foreach (var candlestick in candlestickList)
            {
                if (patternCheckers[patternName](candlestick))
                {
                    var boxAnnotation = new RectangleAnnotation
                    {
                        AxisX = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX,
                        AxisY = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY,
                        X = candlestick.date.ToOADate(),
                        Y = (double)candlestick.high,
                        Width = 1,
                        Height = (double)(candlestick.high - candlestick.low),
                        LineColor = System.Drawing.Color.Black,
                        BackColor = System.Drawing.Color.Transparent,
                        Text = patternName
                    };
                    chart_candlesticks.Annotations.Add(boxAnnotation);
                }
            }
        }

        // Dictionary of pattern checkers mapping pattern names to corresponding functions
        private readonly Dictionary<string, Func<SmartCandlestick, bool>> patternCheckers = new Dictionary<string, Func<SmartCandlestick, bool>>
        {
            { "Bullish", candlestick => candlestick.IsBullish },
            { "Bearish", candlestick => candlestick.IsBearish },
            { "Neutral", candlestick => candlestick.IsNeutral },
            { "Marubozu", candlestick => candlestick.IsMarubozu },
            { "Hammer", candlestick => candlestick.IsHammer },
            { "Doji", candlestick => candlestick.IsDoji },
            { "Dragonfly Doji", candlestick => candlestick.IsDragonflyDoji },
            { "Gravestone Doji", candlestick => candlestick.IsGravestoneDoji }
        };

        // Event handler for the update button
        private void button_Update_Click(object sender, EventArgs e)
        {
            chart_candlesticks.Series.Clear();
            chart_candlesticks.Annotations.Clear();
            filterCandlesticks();
            normalizeChart();
            displayCandlestick();
        }

        // Event handler for date pickers
        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (tempList != null)
            {
                filterCandlesticks();
                normalizeChart();
                displayCandlestick();
            }
            else
            {
                MessageBox.Show("No data loaded. Please load a file first.", "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Placeholder event handler for the end date label click
        private void label_endDate_Click(object sender, EventArgs e)
        {
            // No specific functionality needed
        }

        // Placeholder event handler for the FileOk event of the OpenFileDialog
        private void openFileDialog_TicketChooser_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // No specific functionality needed
        }

        //Project 3 Additions from this point
        //*********************************************************************************************************
        //*********************************************************************************************************
        //*********************************************************************************************************
        //*********************************************************************************************************


        private SmartCandlestick selectedCandlestick1 = null;
        private SmartCandlestick selectedCandlestick2 = null;
        private bool isSelecting = false; // Toggles selection mode
        private double rectXStart = 0;
        private double rectXEnd = 0;
        private double rectYTop = 0;
        private double rectYBottom = 0;


        private void HighlightSelectedCandlestick(SmartCandlestick candlestick)
        {
            var annotation = new VerticalLineAnnotation
            {
                AxisX = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX,
                AxisY = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY,
                X = candlestick.date.ToOADate(),
                IsInfinitive = true,
                LineColor = System.Drawing.Color.Blue,
                LineWidth = 2,
                ClipToChartArea = "ChartArea_OHLC"  // This ensures the annotation is clipped to the specified chart area
            };

            chart_candlesticks.Annotations.Add(annotation);
        }


        private void DrawHorizontalLinesBetweenCandlesticks()
        {
            if (selectedCandlestick1 == null || selectedCandlestick2 == null) return;

            // Calculate X-axis range (dates)
            double startX = Math.Min(selectedCandlestick1.date.ToOADate(), selectedCandlestick2.date.ToOADate());
            double endX = Math.Max(selectedCandlestick1.date.ToOADate(), selectedCandlestick2.date.ToOADate());

            // Find the highest high and lowest low between the selected candlesticks
            double highestHigh = Math.Max((double)selectedCandlestick1.high, (double)selectedCandlestick2.high);
            double lowestLow = Math.Min((double)selectedCandlestick1.low, (double)selectedCandlestick2.low);


            // Store the rectangle coordinates
            rectXStart = startX;
            rectXEnd = endX;
            rectYTop = highestHigh;
            rectYBottom = lowestLow;

            // Force the chart to redraw and trigger the PostPaint event
            chart_candlesticks.Invalidate();

            // Compute and plot Beauty vs Price after all calculations are complete
            ComputeAndPlotBeautyVsPrice();

            // Compute and display wave beauty
            ComputeAndDisplayWaveBeauty();

            // Reset selection
            selectedCandlestick1 = null;
            selectedCandlestick2 = null;
            isSelecting = false; // Disable selection mode
            MessageBox.Show("Wave selection completed, and Beauty computed.", "Wave Processed");
        }



        // Function to add horizontal lines for Fibonacci levels
        private void AddFibonacciLine(double level, string label, Chart chart)
        {
            // Create a HorizontalLineAnnotation for the Fibonacci level
            var fibonacciLine = new HorizontalLineAnnotation
            {
                AxisX = chart.ChartAreas["ChartArea_OHLC"].AxisX,
                AxisY = chart.ChartAreas["ChartArea_OHLC"].AxisY,
                ClipToChartArea = chart.ChartAreas["ChartArea_OHLC"].Name,
                IsInfinitive = true,
                Y = level,
                LineColor = System.Drawing.Color.Red, // Set line color
                LineWidth = 1
            };

            // Add the Fibonacci line to the chart
            chart.Annotations.Add(fibonacciLine);

            double startX = Math.Min(selectedCandlestick1.date.ToOADate(), selectedCandlestick2.date.ToOADate());


            // Optionally, add a label to the Fibonacci line to indicate its level
            var labelAnnotation = new TextAnnotation
            {
                AxisX = chart.ChartAreas["ChartArea_OHLC"].AxisX,
                AxisY = chart.ChartAreas["ChartArea_OHLC"].AxisY,
                X = startX + 0.01, // Adjust the X position for the label (near the start of the box)
                Y = level,
                Text = label,
                ForeColor = System.Drawing.Color.Red, // Label color matching the Fibonacci line color
                Font = new Font("Arial", 8),
                Alignment = ContentAlignment.MiddleLeft
            };

            // Add the label to the chart
            chart.Annotations.Add(labelAnnotation);
        }



        private void button_ToggleSelection_Click_1(object sender, EventArgs e)
        {
            // Toggle selection mode
            isSelecting = !isSelecting;

            if (isSelecting)
            {
                // Clear annotations (including box and diagonal line)
                chart_candlesticks.Annotations.Clear();

                // Reset the Beauty vs Price chart (second chart only)
                chart_candlesticks.Series.Remove(chart_candlesticks.Series.FirstOrDefault(s => s.Name == "Series_Beauty"));
                var beautyChartArea = chart_candlesticks.ChartAreas.FirstOrDefault(a => a.Name == "ChartArea_Beauty");

                if (beautyChartArea != null)
                {
                    // Reset the Beauty chart's axis ranges
                    beautyChartArea.AxisY.Minimum = 0;
                    beautyChartArea.AxisY.Maximum = 10; // Default range
                    beautyChartArea.AxisX.Minimum = double.NaN; // Auto reset X-axis
                    beautyChartArea.AxisX.Maximum = double.NaN;
                }

                // Reset variables related to Beauty vs Price
                overallBeauty = 0;
                confirmationPoints.Clear();

                // Reset selected candlesticks for wave computation
                selectedCandlestick1 = null;
                selectedCandlestick2 = null;

                // Reset rectangle and diagonal line coordinates
                rectXStart = 0;
                rectXEnd = 0;
                rectYTop = 0;
                rectYBottom = 0;

                MessageBox.Show("Selection mode activated. Select two candlesticks to process the wave.", "Selection Mode");
            }
            else
            {
                MessageBox.Show("Selection mode deactivated.", "Selection Mode");
            }
        }






        private void chart_candlesticks_MouseClick_1(object sender, MouseEventArgs e)
        {
            if (!isSelecting) return;

            HitTestResult result = chart_candlesticks.HitTest(e.X, e.Y);

            if (result.ChartElementType == ChartElementType.DataPoint)
            {
                int selectedIndex = result.PointIndex;
                SmartCandlestick selectedCandlestick = candlestickList[selectedIndex];

                if (selectedCandlestick1 == null)
                {
                    selectedCandlestick1 = selectedCandlestick;

                    // Get the previous and next candlesticks for comparison
                    SmartCandlestick previous = selectedIndex > 0 ? candlestickList[selectedIndex - 1] : null;
                    SmartCandlestick next = selectedIndex < candlestickList.Count - 1 ? candlestickList[selectedIndex + 1] : null;

                    // Detect if the selected candlestick is a peak or valley
                    selectedCandlestick1.DetectPeakOrValley(previous, next);

                    // Check if the first candlestick is a peak or a valley
                    if (!(selectedCandlestick1.IsPeak || selectedCandlestick1.IsValley))
                    {
                        MessageBox.Show("The first candlestick must be a peak or a valley.");
                        selectedCandlestick1 = null; // Reset the selection
                        return;
                    }

                    // Highlight the selected candlestick
                    HighlightSelectedCandlestick(selectedCandlestick);
                    MessageBox.Show("First candlestick selected.");
                }
                else if (selectedCandlestick2 == null)
                {
                    // Ensure the second selected candlestick is different from the first one
                    if (selectedCandlestick == selectedCandlestick1)
                    {
                        MessageBox.Show("Cannot select the same candlestick twice.", "Selection Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    selectedCandlestick2 = selectedCandlestick;

                    // Highlight the second selected candlestick
                    HighlightSelectedCandlestick(selectedCandlestick);
                    MessageBox.Show("Second candlestick selected. Drawing box...");

                    // Draw the horizontal lines and process the wave
                    DrawHorizontalLinesBetweenCandlesticks();

                    // Disable selection mode after processing the wave
                    isSelecting = false;
                    MessageBox.Show("Selection mode deactivated.");
                }
            }
        }




        private void Chart_candlesticks_PostPaint(object sender, ChartPaintEventArgs e)
        {
            if (rectXStart == 0 && rectXEnd == 0) return; // No rectangle to draw

            var chartArea = chart_candlesticks.ChartAreas["ChartArea_OHLC"];
            if (chartArea == null) return;

            // Map the rectangle's coordinates to pixel positions
            double xStartPixel = chartArea.AxisX.ValueToPixelPosition(rectXStart);
            double xEndPixel = chartArea.AxisX.ValueToPixelPosition(rectXEnd);
            double yTopPixel = chartArea.AxisY.ValueToPixelPosition(rectYTop);
            double yBottomPixel = chartArea.AxisY.ValueToPixelPosition(rectYBottom);

            // Create a rectangle from the calculated pixel positions
            var rect = new Rectangle(
                (int)xStartPixel,
                (int)yTopPixel,
                (int)(xEndPixel - xStartPixel),
                (int)(yBottomPixel - yTopPixel)
            );

            // Draw the rectangle
            using (var pen = new Pen(Color.Blue, 2))
            {
                e.ChartGraphics.Graphics.DrawRectangle(pen, rect);
            }

            // Ensure candlesticks are selected before proceeding
            if (selectedCandlestick1 == null || selectedCandlestick2 == null)
            {
                return; // Exit early if candlesticks are not selected
            }

            // Ensure the rectangle coordinates are properly initialized
            if (rectXStart == 0 && rectXEnd == 0 && rectYTop == 0 && rectYBottom == 0)
            {
                return; // Exit early if the rectangle has not been drawn
            }

            // Ensure the ChartGraphics object is valid
            if (e.ChartGraphics == null || e.ChartGraphics.Graphics == null)
            {
                return; // Exit early if the graphics object is unavailable
            }

            // Draw the diagonal line based on the trend direction
            using (var dashedPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 1))
            {
                dashedPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

                // Determine the trend direction using the selected candlesticks
                double startPrice = (double)selectedCandlestick1.close;
                double endPrice = (double)selectedCandlestick2.close;

                if (startPrice > endPrice) // Downward trend
                {
                    e.ChartGraphics.Graphics.DrawLine(
                        dashedPen,
                        (float)xStartPixel, (float)yTopPixel,     // Top-left
                        (float)xEndPixel, (float)yBottomPixel     // Bottom-right
                    );
                }
                else // Upward trend
                {
                    e.ChartGraphics.Graphics.DrawLine(
                        dashedPen,
                        (float)xStartPixel, (float)yBottomPixel,  // Bottom-left
                        (float)xEndPixel, (float)yTopPixel        // Top-right
                    );
                }
            }



            // Pass the pixel coordinates to DrawFibonacciLevelsAndConfirmationDots
            DrawFibonacciLevelsAndConfirmationDots(e, chartArea, xStartPixel, xEndPixel);
        }



        private void DrawFibonacciLevelsAndConfirmationDots(ChartPaintEventArgs e, ChartArea chartArea, double xStartPixel, double xEndPixel)
        {
            if (selectedCandlestick1 == null || selectedCandlestick2 == null) return;

            // Calculate the highest high and lowest low between the two candlesticks
            double highestHigh = Math.Max((double)selectedCandlestick1.high, (double)selectedCandlestick2.high);
            double lowestLow = Math.Min((double)selectedCandlestick1.low, (double)selectedCandlestick2.low);

            // Calculate Fibonacci levels using the highest high and lowest low
            double[] fibLevels = new double[]
            {
        lowestLow,                                  // 0%
        lowestLow + (highestHigh - lowestLow) * 0.236, // 23.6%
        lowestLow + (highestHigh - lowestLow) * 0.382, // 38.2%
        lowestLow + (highestHigh - lowestLow) * 0.5,   // 50%
        lowestLow + (highestHigh - lowestLow) * 0.618, // 61.8%
        highestHigh                                // 100%
            };

            string[] fibLabels = new string[] { "0", "23.6", "38.2", "50", "61.8", "100" };

            // Retrieve the user-defined leeway percentage
            double leeway = GetLeewayPercentage();

            // Variable to track overall beauty
            int overallBeauty = 0;

            // Draw Fibonacci levels and calculate confirmations
            for (int i = 0; i < fibLevels.Length; i++)
            {
                double level = fibLevels[i];
                string label = fibLabels[i];

                // Draw Fibonacci level line
                double yPixel = chartArea.AxisY.ValueToPixelPosition(level);
                using (var pen = new Pen(Color.Green, 1))
                {
                    e.ChartGraphics.Graphics.DrawLine(pen, (float)xStartPixel, (float)yPixel, (float)xEndPixel, (float)yPixel);
                }

                // Add label for the Fibonacci level
                using (var font = new Font("Arial", 8, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.Blue))
                {
                    // Place the label near the left edge of the rectangle
                    e.ChartGraphics.Graphics.DrawString(
                        $"{label}%",
                        font,
                        brush,
                        new PointF((float)xStartPixel - 40, (float)yPixel - 8) // Adjust position for readability
                    );
                }

                // Iterate through each candlestick in the wave for confirmations
                foreach (var candlestick in candlestickList)
                {
                    if (candlestick.date < selectedCandlestick1.date || candlestick.date > selectedCandlestick2.date)
                        continue;

                    double margin = level * leeway; // Calculate margin based on leeway

                    // Check if any OHLC value matches the Fibonacci level within the margin
                    if (Math.Abs((double)candlestick.open - level) <= margin ||
                        Math.Abs((double)candlestick.high - level) <= margin ||
                        Math.Abs((double)candlestick.low - level) <= margin ||
                        Math.Abs((double)candlestick.close - level) <= margin)
                    {
                        overallBeauty++; // Increment the beauty count for each confirmation point

                        // Add confirmation point (dot)
                        double xPixel = chartArea.AxisX.ValueToPixelPosition(candlestick.date.ToOADate());
                        using (var brush = new SolidBrush(Color.Orange))
                        {
                            e.ChartGraphics.Graphics.FillEllipse(brush, (float)xPixel - 5, (float)yPixel - 5, 10, 10); // Larger dots
                        }
                    }
                }
            }

            // Display the overall beauty value
            using (var font = new Font("Arial", 10, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.Black))
            {
                e.ChartGraphics.Graphics.DrawString($"Wave Beauty: {overallBeauty}",
                    font, brush, new PointF(10, 10)); // Display beauty at the top-left of the chart
            }
        }










        //I WANNA CRY. NOTHING WORKS.    



        private List<(double x, double y)> confirmationPoints = new List<(double x, double y)>();
        private double overallBeauty = 0;

        private void ComputeAndDisplayWaveBeauty()
        {
            if (selectedCandlestick1 == null || selectedCandlestick2 == null) return;

            var chartArea = chart_candlesticks.ChartAreas["ChartArea_OHLC"];
            if (chartArea == null)
            {
                MessageBox.Show("Chart Area not found.");
                return;
            }

            double startY = Math.Max(
                selectedCandlestick1.IsBullish ? (double)selectedCandlestick1.high : (double)selectedCandlestick1.low,
                selectedCandlestick2.IsBullish ? (double)selectedCandlestick2.high : (double)selectedCandlestick2.low
            );
            double endY = Math.Min(
                selectedCandlestick1.IsBullish ? (double)selectedCandlestick1.high : (double)selectedCandlestick1.low,
                selectedCandlestick2.IsBullish ? (double)selectedCandlestick2.high : (double)selectedCandlestick2.low
            );

            double[] fibLevels = new double[]
            {
                endY,
                endY + (startY - endY) * 0.236,
                endY + (startY - endY) * 0.382,
                endY + (startY - endY) * 0.5,
                endY + (startY - endY) * 0.618,
                startY
            };

            overallBeauty = 0;
            confirmationPoints.Clear(); // Clear previous points

            foreach (var candlestick in candlestickList)
            {
                if (candlestick.date < selectedCandlestick1.date || candlestick.date > selectedCandlestick2.date)
                    continue; // Only consider candlesticks within the wave

                int confirmations = 0;

                foreach (var level in fibLevels)
                {
                    if (Math.Abs((double)candlestick.open - level) <= 0.01)
                    {
                        confirmations++;
                        confirmationPoints.Add((candlestick.date.ToOADate(), level));
                    }
                    if (Math.Abs((double)candlestick.high - level) <= 0.01)
                    {
                        confirmations++;
                        confirmationPoints.Add((candlestick.date.ToOADate(), level));
                    }
                    if (Math.Abs((double)candlestick.low - level) <= 0.01)
                    {
                        confirmations++;
                        confirmationPoints.Add((candlestick.date.ToOADate(), level));
                    }
                    if (Math.Abs((double)candlestick.close - level) <= 0.01)
                    {
                        confirmations++;
                        confirmationPoints.Add((candlestick.date.ToOADate(), level));
                    }
                }

                candlestick.Beauty = confirmations;
                overallBeauty += confirmations;
            }

            MessageBox.Show($"Selected: {overallBeauty}", "Wave Beauty");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private double GetLeewayPercentage()
        {
            // Get the user-specified leeway percentage
            return (double)numericUpDownLeeway.Value / 100; // Convert percentage to decimal
        }

        private void ComputeAndPlotBeautyVsPrice()
        {
            if (selectedCandlestick1 == null || selectedCandlestick2 == null)
            {
                MessageBox.Show("Please select two candlesticks before computing Beauty vs Price.", "Error");
                return;
            }

            // Determine the start and end prices of the wave
            double startPrice = Math.Min((double)selectedCandlestick1.close, (double)selectedCandlestick2.close);
            double endPrice = Math.Max((double)selectedCandlestick1.close, (double)selectedCandlestick2.close);

            double waveHeight = endPrice - startPrice;
            double extension = waveHeight * 0.25; // Extend by 25% of wave height

            // Generate prices for the X-axis
            List<double> prices = new List<double>();
            for (double price = startPrice; price <= endPrice + extension; price += waveHeight / 20)
            {
                prices.Add(price);
            }

            // Create a series for Beauty vs Price
            var beautySeries = new Series("Series_Beauty")
            {
                ChartType = SeriesChartType.Line, // Line chart for smooth curves
                ChartArea = "ChartArea_Beauty",
                XValueType = ChartValueType.Double // X-axis represents price
            };

            // Compute beauty for each price
            foreach (var price in prices)
            {
                double beautyValue = ComputeBeautyForWave(selectedCandlestick1, selectedCandlestick2, price);
                beautySeries.Points.AddXY(price, beautyValue);
            }

            // Add the beauty series to the chart
            chart_candlesticks.Series.Add(beautySeries);

            // Normalize the Y-axis
            normalizeChart();
        }



        private double ComputeBeautyForWave(SmartCandlestick c1, SmartCandlestick c2, double? overrideEndPrice = null)
        {
            // Determine the direction of the wave
            double startPrice = (double)c1.close;
            double endPrice = overrideEndPrice ?? (double)c2.close;

            double highPrice = Math.Max(startPrice, endPrice);
            double lowPrice = Math.Min(startPrice, endPrice);

            double[] fibLevels = new double[]
            {
        lowPrice,
        lowPrice + (highPrice - lowPrice) * 0.236,
        lowPrice + (highPrice - lowPrice) * 0.382,
        lowPrice + (highPrice - lowPrice) * 0.5,
        lowPrice + (highPrice - lowPrice) * 0.618,
        highPrice
            };

            double leeway = GetLeewayPercentage();
            double beauty = 0;

            // Consider all candlesticks in the wave
            foreach (var candlestick in candlestickList)
            {
                if (candlestick.date < c1.date || candlestick.date > c2.date) continue;

                double[] ohlcValues = new double[] { (double)candlestick.open, (double)candlestick.high, (double)candlestick.low, (double)candlestick.close };
                foreach (var fibLevel in fibLevels)
                {
                    foreach (var ohlcValue in ohlcValues)
                    {
                        if (Math.Abs(ohlcValue - fibLevel) <= fibLevel * leeway)
                        {
                            beauty++;
                        }
                    }
                }
            }

            return beauty;
        }



        //End of code here
    }
}
