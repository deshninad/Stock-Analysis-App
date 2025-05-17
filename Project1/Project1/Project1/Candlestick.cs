/*
NINAD DESHPANDE
U50461301
*/

// The `Candlestick` class is designed to represent financial candlestick data, including 
// properties such as open, high, low, close prices, volume, and date. The class includes 
// constructors for default and parameterized initialization, as well as a CSV-parsing 
// constructor to populate properties from CSV data. This design supports easy handling 
// of financial data in C# applications.

// Import necessary libraries for the class's functionality.
using System;
using System.Globalization;

namespace Project1
{
    public class Candlestick
    {
        // Public property to represent the opening price of the candlestick.
        public Decimal open { get; set; }

        // Public property to represent the highest price of the candlestick.
        public Decimal high { get; set; }

        // Public property to represent the lowest price of the candlestick.
        public Decimal low { get; set; }

        // Public property to represent the closing price of the candlestick.
        public Decimal close { get; set; }

        // Public property to represent the trading volume of the candlestick.
        public long volume { get; set; }

        // Public property to store the date associated with the candlestick data.
        public DateTime date { get; set; }

        /// <summary>
        /// Default constructor for the Candlestick class.
        /// </summary>
        public Candlestick() { }

        /// <summary>
        /// Constructor that allows setting date, open, high, low, close, and volume values.
        /// </summary>
        /// <param name="date">The date associated with this candlestick.</param>
        /// <param name="open">The opening price of the candlestick.</param>
        /// <param name="high">The highest price of the candlestick.</param>
        /// <param name="low">The lowest price of the candlestick.</param>
        /// <param name="close">The closing price of the candlestick.</param>
        /// <param name="volume">The trading volume of the candlestick.</param>
        public Candlestick(DateTime date, decimal open = 0, decimal high = 0, decimal low = 0, decimal close = 0, long volume = 0)
        {
            this.date = date;      // Sets the candlestick date.
            this.open = open;      // Sets the open price.
            this.high = high;      // Sets the high price.
            this.low = low;        // Sets the low price.
            this.close = close;    // Sets the closing price.
            this.volume = volume;  // Sets the trading volume.
        }

        /// <summary>
        /// Constructor that initializes candlestick properties by parsing a CSV string.
        /// </summary>
        /// <param name="rowofData">CSV string representing a row of candlestick data.</param>
        public Candlestick(string rowofData)
        {
            // Define delimiters for parsing the CSV data.
            char[] separators = new char[] { ',', ' ', '"' };

            // Split the CSV string into an array of substrings, excluding empty entries.
            string[] subs = rowofData.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            // Ensure there are enough elements to avoid index issues.
            if (subs.Length >= 6)
            {
                // Temporary variable for parsing decimal values.
                decimal temp;

                // Attempt to parse the date (first column).
                if (DateTime.TryParse(subs[0], out DateTime parsedDate))
                {
                    date = parsedDate;
                }

                // Parse and set the open price (second column).
                if (Decimal.TryParse(subs[1], NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
                {
                    open = temp;
                }

                // Parse and set the high price (third column).
                if (Decimal.TryParse(subs[2], NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
                {
                    high = temp;
                }

                // Parse and set the low price (fourth column).
                if (Decimal.TryParse(subs[3], NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
                {
                    low = temp;
                }

                // Parse and set the close price (fifth column).
                if (Decimal.TryParse(subs[4], NumberStyles.Any, CultureInfo.InvariantCulture, out temp))
                {
                    close = temp;
                }

                // Parse and set the volume (sixth column).
                if (subs.Length > 5 && long.TryParse(subs[5], NumberStyles.Any, CultureInfo.InvariantCulture, out long parsedVolume))
                {
                    volume = parsedVolume;
                }
            }
            else
            {
                // Throws an exception if there are not enough columns for a valid candlestick.
                throw new ArgumentException("Insufficient data in CSV row to create a Candlestick object.");
            }
        }
    }
}
