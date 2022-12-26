using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public static class Assets
    {
        public static List<string> GetAll()
        {
            List<string> result = new List<string>();

            result.Add("ES");
            result.Add("NQ");
            result.Add("ZB");
            result.Add("ZN");

            return result;
        }
    }
}
