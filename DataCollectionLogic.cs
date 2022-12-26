using System.Collections.Generic;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class DataCollectionLogic
    {
        private List<Contract> selectedContracts;
        private Progress progressObj;

        public DataCollectionLogic(List<Contract> selectedContracts, Progress progressObj)
        {
            this.selectedContracts = selectedContracts;
            this.progressObj = progressObj;
        }

        public void StartCollection()
        {
            //TODO
        }
    }
}
