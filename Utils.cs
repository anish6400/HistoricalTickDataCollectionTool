using System;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public static class Utils
    {

        /*
         Thanks to: https://stackoverflow.com/a/20691099/16867234
         */
        public static DateTime FindDay(int year, int month, DayOfWeek Day, int occurance)
        {
            if (occurance <= 0 || occurance > 5) throw new Exception("Occurance is invalid");

            DateTime firstDayOfMonth = new DateTime(year, month, 1);
            //Substract first day of the month with the required day of the week 
            var daysneeded = (int)Day - (int)firstDayOfMonth.DayOfWeek;
            //if it is less than zero we need to get the next week day (add 7 days)
            if (daysneeded < 0) daysneeded = daysneeded + 7;
            //DayOfWeek is zero index based; multiply by the Occurance to get the day
            var resultedDay = (daysneeded + 1) + (7 * (occurance - 1));

            if (resultedDay > (firstDayOfMonth.AddMonths(1) - firstDayOfMonth).Days)
                throw new Exception(String.Format("No {0} occurance(s) of {1} in the required month", occurance, Day.ToString()));

            return new DateTime(year, month, resultedDay);
        }
    }
}
