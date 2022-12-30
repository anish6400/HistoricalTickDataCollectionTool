using NinjaTrader.Cbi;
using NinjaTrader.Data;
using System;
using System.Collections.Generic;
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
                DBUitl.CreateTable(contract.ToString());
                CollectContractData(contract);
            }
        }

        private void CollectContractData(Contract contract)
        {
            Logger.LogMessage("Starting data collection for contract name: " + contract.ToString());

            barsRequestObj = new BarsRequest(contract.GetNTInstrument(), contract.StartDate, contract.EndDate);
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

				float prevPer = 0;
                for (var i = 0; i < bars.Bars.Count; i++)
                {
                    // open,high,low,close are same as we are requesting tick data
                    DBUitl.AddRecord(contract.ToString(), new DateTimeOffset(bars.Bars.GetTime(i)).ToUnixTimeMilliseconds(),
                                    bars.Bars.GetBid(i), bars.Bars.GetAsk(i),
                                    bars.Bars.GetOpen(i), bars.Bars.GetVolume(i));

					float currPer = (i*100)/bars.Bars.Count;
                    if(i%10000 == 0)
					{
						progressObj.updateProgress(currPer);
						prevPer = currPer;
					}
                }

				progressObj.updateProgress(100F);
                isProcessing = false;
            }
            );

            // Waits for bars request finish processing
            SpinWait.SpinUntil(() => !isProcessing);

            Logger.LogMessage("Finished data collection for contract name: " + contract.ToString());
        }

        public void KillBarsRequest()
        {
            if (barsRequestObj == null) return;
            barsRequestObj.Dispose();
            barsRequestObj = null;
        }
    }
}
