using NinjaTrader.Cbi;
using System;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class Contract
    {
        private string instrumentName;

        // both startDate and endDate are inclusive
        private int startMonth;
        private int startYear;
        private DateTime startDate;

        private int endMonth;
        private int endYear;
        private DateTime endDate;

        // Sets the startDate to the sunday after 3rd friday of startMonth
        private void SetStartDate()
        {
            DateTime sampleStartDate = new DateTime(endYear, endMonth, 1).AddMonths(-3);

            DateTime thirdFridayOfMonth = Utils.FindDay(sampleStartDate.Year, sampleStartDate.Month, DayOfWeek.Friday, 3);

            startDate = thirdFridayOfMonth.AddDays(2);
        }

        private void SetEndDate()
        {
            endDate = Utils.FindDay(endYear, endMonth, DayOfWeek.Friday, 3);
        }

        public Contract(string instrumentName, int endMonth, int endYear)
        {
            this.instrumentName = instrumentName;
            this.endMonth = endMonth;
            this.endYear = endYear;

            SetStartDate();
            SetEndDate();
        }

        public string InstrumentName { get { return instrumentName; } }
        public int EndMonth { get { return endMonth; } }
        public int EndYear { get { return endYear; } }
        public DateTime StartDate { get { return startDate; } }
        public DateTime EndDate { get { return endDate; } }

        public Instrument GetNTInstrument()
        {
            return Instrument.GetInstrument(ToString());
        }

        public override string ToString()
        {
            return instrumentName + " " + endMonth.ToString("D2") + "-" + (endYear % 100).ToString();
        }
    }
}
