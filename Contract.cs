using System;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class Contract
    {
        private string instrumentName;
        private int month;
        private int year;

        public Contract(string instrumentName, int month, int year)
        {
            this.instrumentName = instrumentName;
            this.month = month;
            this.year = year;
        }

        public string InstrumentName { get { return instrumentName; } }
        public int Month { get { return month; } }
        public int Year { get { return year; } }

        public DateTime GetStartDate()
        {
            // TODO
            return DateTime.Now;
        }
        public DateTime GetEndDate()
        {
            // TODO
            return DateTime.Now;
        }

        public override string ToString()
        {
            return instrumentName + " " + month.ToString("D2") + "-" + (year % 100).ToString();
        }
    }
}
