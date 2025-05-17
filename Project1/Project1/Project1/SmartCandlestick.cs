using System;

namespace Project1
{
    // SmartCandlestick class inherits from the Candlestick class
    public class SmartCandlestick : Candlestick
    {
        // Property to calculate the range of the candlestick (high - low)
        public decimal Range => high - low;

        // Property to calculate the body range of the candlestick (absolute difference between close and open)
        public decimal BodyRange => Math.Abs(close - open);

        // Property to determine the top price (maximum of open and close)
        public decimal TopPrice => Math.Max(open, close);

        // Property to determine the bottom price (minimum of open and close)
        public decimal BottomPrice => Math.Min(open, close);

        // Property to calculate the upper tail (distance from TopPrice to high)
        public decimal UpperTail => high - TopPrice;

        // Property to calculate the lower tail (distance from low to BottomPrice)
        public decimal LowerTail => BottomPrice - low;

        // Constructor to create a SmartCandlestick from a CSV row
        public SmartCandlestick(string csvRow) : base(csvRow)
        {
        }

        // Constructor to create a SmartCandlestick from individual parameters
        public SmartCandlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, long volume)
            : base(date, open, high, low, close, volume)
        {
        }

        // Property to indicate if this candlestick is a peak
        public bool IsPeak { get; set; } = false;

        // Property to indicate if this candlestick is a valley
        public bool IsValley { get; set; } = false;

        // Method to detect if this candlestick is a peak or valley
        public void DetectPeakOrValley(SmartCandlestick previous, SmartCandlestick next)
        {
            // Ensure previous and next candlesticks are not null
            if (previous == null || next == null)
                return;

            // A peak is higher than its neighbors
            IsPeak = high > previous.high && high > next.high;

            // A valley is lower than its neighbors
            IsValley = low < previous.low && low < next.low;
        }

        // Pattern Detection Properties

        // Property to determine if the candlestick is bullish (close > open)
        public bool IsBullish => close > open;

        // Property to determine if the candlestick is bearish (close < open)
        public bool IsBearish => close < open;

        // Property to determine if the candlestick is neutral (close == open)
        public bool IsNeutral => close == open;

        // Property to check if the candlestick is a Marubozu (no upper or lower tail)
        public bool IsMarubozu => UpperTail == 0 && LowerTail == 0;

        // Property to check if the candlestick is a Hammer (long lower tail)
        public bool IsHammer => LowerTail > (2 * BodyRange) && UpperTail < BodyRange;

        // Property to check if the candlestick is a Doji (very small or no body)
        public bool IsDoji => BodyRange <= 0.001m;

        // Property to check if the candlestick is a Dragonfly Doji (no upper tail, long lower tail)
        public bool IsDragonflyDoji => IsDoji && UpperTail <= 0 && LowerTail > 0;

        // Property to check if the candlestick is a Gravestone Doji (no lower tail, long upper tail)
        public bool IsGravestoneDoji => IsDoji && UpperTail > 0 && LowerTail <= 0;

        // Method to get the name of the candlestick pattern
        public string GetPatternName()
        {
            if (IsMarubozu) return "Marubozu"; // Check if it's a Marubozu
            if (IsHammer) return "Hammer"; // Check if it's a Hammer
            if (IsDragonflyDoji) return "Dragonfly Doji"; // Check if it's a Dragonfly Doji
            if (IsGravestoneDoji) return "Gravestone Doji"; // Check if it's a Gravestone Doji
            if (IsDoji) return "Doji"; // Check if it's a Doji
            if (IsBullish) return "Bullish"; // Check if it's Bullish
            if (IsBearish) return "Bearish"; // Check if it's Bearish
            return "Neutral"; // Default to Neutral if no pattern matches
        }
        public int Beauty { get; set; } = 0;

    }
}
