/*
NINAD DESHPANDE
U50461301
 */

using System.Windows.Forms;

namespace Project1
{
    partial class Form_StockView
    {
        /// <summary>
        /// Manages the disposal of components utilized by the designer.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Releases any resources used by the form to free up memory, depending on whether 
        /// they were created by the form or passed in as external objects.
        /// </summary>
        /// <param name="disposing">Tells if managed resources should be disposed of i.e, true or left intact i.e, false)</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Automatically generating the methods required for initializing the form components. 
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.comboBox_PatternSelector = new System.Windows.Forms.ComboBox();
            this.button_loadData = new System.Windows.Forms.Button();
            this.openFileDialog_TicketChooser = new System.Windows.Forms.OpenFileDialog();
            this.label_startDate = new System.Windows.Forms.Label();
            this.dateTimePicker_startDate = new System.Windows.Forms.DateTimePicker();
            this.label_endDate = new System.Windows.Forms.Label();
            this.dateTimePicker_endDate = new System.Windows.Forms.DateTimePicker();
            this.chart_candlesticks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.CandlestickBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.button_ToggleSelection = new System.Windows.Forms.Button();
            this.numericUpDownLeeway = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CandlestickBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeeway)).BeginInit();
            this.SuspendLayout();
            // 
            // comboBox_PatternSelector
            // 
            this.comboBox_PatternSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_PatternSelector.Location = new System.Drawing.Point(1362, 617);
            this.comboBox_PatternSelector.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBox_PatternSelector.Name = "comboBox_PatternSelector";
            this.comboBox_PatternSelector.Size = new System.Drawing.Size(244, 28);
            this.comboBox_PatternSelector.TabIndex = 0;
            // 
            // button_loadData
            // 
            this.button_loadData.Location = new System.Drawing.Point(1360, 524);
            this.button_loadData.Name = "button_loadData";
            this.button_loadData.Size = new System.Drawing.Size(246, 72);
            this.button_loadData.TabIndex = 0;
            this.button_loadData.Text = "Load Stock";
            this.button_loadData.UseVisualStyleBackColor = true;
            this.button_loadData.Click += new System.EventHandler(this.button_loadData_Click);
            // 
            // openFileDialog_TicketChooser
            // 
            this.openFileDialog_TicketChooser.FileName = "openFileDialog1";
            this.openFileDialog_TicketChooser.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_TicketChooser_FileOk);
            // 
            // label_startDate
            // 
            this.label_startDate.AutoSize = true;
            this.label_startDate.Location = new System.Drawing.Point(1356, 312);
            this.label_startDate.Name = "label_startDate";
            this.label_startDate.Size = new System.Drawing.Size(83, 20);
            this.label_startDate.TabIndex = 2;
            this.label_startDate.Text = "Start Date";
            // 
            // dateTimePicker_startDate
            // 
            this.dateTimePicker_startDate.Location = new System.Drawing.Point(1360, 335);
            this.dateTimePicker_startDate.Name = "dateTimePicker_startDate";
            this.dateTimePicker_startDate.Size = new System.Drawing.Size(296, 26);
            this.dateTimePicker_startDate.TabIndex = 3;
            this.dateTimePicker_startDate.Value = new System.DateTime(2024, 2, 16, 0, 0, 0, 0);
            // 
            // label_endDate
            // 
            this.label_endDate.AutoSize = true;
            this.label_endDate.Location = new System.Drawing.Point(1356, 420);
            this.label_endDate.Name = "label_endDate";
            this.label_endDate.Size = new System.Drawing.Size(77, 20);
            this.label_endDate.TabIndex = 4;
            this.label_endDate.Text = "End Date";
            this.label_endDate.Click += new System.EventHandler(this.label_endDate_Click);
            // 
            // dateTimePicker_endDate
            // 
            this.dateTimePicker_endDate.Location = new System.Drawing.Point(1360, 443);
            this.dateTimePicker_endDate.Name = "dateTimePicker_endDate";
            this.dateTimePicker_endDate.Size = new System.Drawing.Size(296, 26);
            this.dateTimePicker_endDate.TabIndex = 5;
            // 
            // chart_candlesticks
            // 
            this.chart_candlesticks.BackColor = System.Drawing.Color.PaleGreen;
            chartArea1.Name = "ChartArea_OHLC";
            chartArea2.AlignWithChartArea = "ChartArea_OHLC";
            chartArea2.Name = "ChartArea_Beauty";
            this.chart_candlesticks.ChartAreas.Add(chartArea1);
            this.chart_candlesticks.ChartAreas.Add(chartArea2);
            this.chart_candlesticks.DataSource = this.CandlestickBindingSource;
            this.chart_candlesticks.Location = new System.Drawing.Point(14, 66);
            this.chart_candlesticks.Name = "chart_candlesticks";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Green, LabelValueType=Low";
            series1.Name = "Series_OHLC";
            series1.XValueMember = "date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Date;
            series1.YValueMembers = "high, low, open, close";
            series1.YValuesPerPoint = 4;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.ChartArea = "ChartArea_Beauty";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Name = "Series_Beauty";
            series2.XValueMember = "date";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueMembers = "beauty";
            series2.YValuesPerPoint = 4;
            this.chart_candlesticks.Series.Add(series1);
            this.chart_candlesticks.Series.Add(series2);
            this.chart_candlesticks.Size = new System.Drawing.Size(1336, 898);
            this.chart_candlesticks.TabIndex = 7;
            this.chart_candlesticks.Text = "chart1";
            this.chart_candlesticks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.chart_candlesticks_MouseClick_1);
            // 
            // CandlestickBindingSource
            // 
            this.CandlestickBindingSource.DataSource = typeof(Project1.Candlestick);
            // 
            // button_ToggleSelection
            // 
            this.button_ToggleSelection.Location = new System.Drawing.Point(1362, 675);
            this.button_ToggleSelection.Name = "button_ToggleSelection";
            this.button_ToggleSelection.Size = new System.Drawing.Size(244, 50);
            this.button_ToggleSelection.TabIndex = 8;
            this.button_ToggleSelection.Text = "Toggle Selection";
            this.button_ToggleSelection.UseVisualStyleBackColor = true;
            this.button_ToggleSelection.Click += new System.EventHandler(this.button_ToggleSelection_Click_1);
            // 
            // numericUpDownLeeway
            // 
            this.numericUpDownLeeway.DecimalPlaces = 1;
            this.numericUpDownLeeway.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownLeeway.Location = new System.Drawing.Point(1372, 760);
            this.numericUpDownLeeway.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numericUpDownLeeway.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDownLeeway.Name = "numericUpDownLeeway";
            this.numericUpDownLeeway.Size = new System.Drawing.Size(234, 26);
            this.numericUpDownLeeway.TabIndex = 9;
            this.numericUpDownLeeway.Value = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            this.numericUpDownLeeway.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // Form_StockView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Info;
            this.ClientSize = new System.Drawing.Size(1677, 829);
            this.Controls.Add(this.numericUpDownLeeway);
            this.Controls.Add(this.button_ToggleSelection);
            this.Controls.Add(this.comboBox_PatternSelector);
            this.Controls.Add(this.chart_candlesticks);
            this.Controls.Add(this.dateTimePicker_endDate);
            this.Controls.Add(this.label_endDate);
            this.Controls.Add(this.dateTimePicker_startDate);
            this.Controls.Add(this.label_startDate);
            this.Controls.Add(this.button_loadData);
            this.Name = "Form_StockView";
            this.Text = "Pick a Stock";
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CandlestickBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeeway)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_loadData;
        private System.Windows.Forms.OpenFileDialog openFileDialog_TicketChooser;
        private System.Windows.Forms.BindingSource CandlestickBindingSource;
        private System.Windows.Forms.ComboBox comboBox_PatternSelector;

        private System.Windows.Forms.Label label_startDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_startDate;
        private System.Windows.Forms.Label label_endDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_endDate;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_candlesticks;
        private Button button_ToggleSelection;
        private NumericUpDown numericUpDownLeeway;
    }
}