using NinjaTrader.Cbi;
using NinjaTrader.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class DataCollectionLogic
    {
        private List<Contract> selectedContracts;
        private Progress progressObj;

        private BarsRequest barsRequestObj;

        public DataCollectionLogic(List<Contract> selectedContracts, Progress progressObj)
        {
            this.selectedContracts = selectedContracts;
            this.progressObj = progressObj;
        }

        public void StartCollection()
        {
            foreach (Contract contract in selectedContracts)
            {
                CollectContractData(contract);
            }
        }

        private void CollectContractData(Contract contract)
        {
            Logger.LogMessage("Starting data collection for contract name: " + contract.ToString());

            string dir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\HistoricalTickData\" + contract.ToString();

            for (DateTime dt = contract.StartDate; dt <= contract.EndDate; dt = dt.AddDays(1))
            {
                Logger.LogMessage(dt.ToString());

                barsRequestObj = new BarsRequest(contract.GetNTInstrument(), dt, dt);
                barsRequestObj.BarsPeriod = new BarsPeriod() { BarsPeriodType = BarsPeriodType.Tick, Value = 1 };
                barsRequestObj.TradingHours = TradingHours.Get("CME US Index Futures ETH");

                var isProcessing = true;
                barsRequestObj.Request((bars, errorCode, errorMessage) =>
                {
                    if (errorCode != ErrorCode.NoError)
                    {
                        Logger.LogMessage(string.Format("Error on requesting data: {1}, {2}", errorCode, errorMessage));
                        isProcessing = false;
                        return;
                    }

                    StringBuilder preMarketContent = new StringBuilder("time;bid;ask;price;volume" + Environment.NewLine);
                    StringBuilder marketContent = new StringBuilder("time;bid;ask;price;volume" + Environment.NewLine);

                    for (var i = 0; i < bars.Bars.Count; i++)
                    {
                        // Times are considered in pst

                        // open,high,low,close are same as we are requesting tick data
                        string tickDataStr = bars.Bars.GetTime(i).ToString("dd MMM yyy HH:mm:ss.fff") + ";" +
                                             bars.Bars.GetBid(i) + ";" + 
                                             bars.Bars.GetAsk(i) + ";" +
                                             bars.Bars.GetOpen(i) + ";" +
                                             bars.Bars.GetVolume(i);
                        
                        // between 6:30am and 2pm
                        var isMarketTime = bars.Bars.GetTime(i).TimeOfDay >= new TimeSpan(6, 30, 0) &&
                                           bars.Bars.GetTime(i).TimeOfDay < new TimeSpan(14, 0, 0);

                        if (isMarketTime)
                        {
                           marketContent.AppendLine(tickDataStr);
                        }
                        else
                        {
                            preMarketContent.AppendLine(tickDataStr);
                        }

                    }

                    if(bars.Bars.Count != 0) WriteToFIles(preMarketContent.ToString(), marketContent.ToString(), dt, contract);

                    isProcessing = false;
                });

                SpinWait.SpinUntil(() => !isProcessing);

                progressObj.updateProgress(Utils.GetPercentageElapsed(contract.StartDate, contract.EndDate, dt));
            }


            Logger.LogMessage("Finished data collection for contract name: " + contract.ToString());
        }

        private void WriteToFIles(string preMarketContent, string marketContent, DateTime dt, Contract contract)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                                        @"\HistoricalTickData\" +
                                                                        contract.ToString() +
                                                                        @"\" + dt.ToString("dd_MMM_yyy ddd");

            System.IO.FileInfo preMarketFile = new System.IO.FileInfo(System.IO.Path.Combine(folderPath, "ETH"));
            System.IO.FileInfo marketFile = new System.IO.FileInfo(System.IO.Path.Combine(folderPath, "RTH"));

            preMarketFile.Directory.Create();
            marketFile.Directory.Create();

            System.IO.File.WriteAllText(preMarketFile.FullName, preMarketContent);
            System.IO.File.WriteAllText(marketFile.FullName, marketContent);
        }

        public void KillBarsRequest()
        {
            if (barsRequestObj == null) return;
            barsRequestObj.Dispose();
            barsRequestObj = null;
        }
    }
}
