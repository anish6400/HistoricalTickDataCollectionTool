using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

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
