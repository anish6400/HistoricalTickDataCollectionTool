using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public partial class CollectorWindow : Form
    {
        private Thread dataCollectionThread;
        private DataCollectionLogic dcLogic;
        public CollectorWindow()
        {
            InitializeComponent();
        }

        private void CollectorWindow_Load(object sender, EventArgs e)
        {
            PopulateAssets();
            Logger.Init(txtBoxLogs);
        }

        private void PopulateAssets()
        {
            cBoxAssets.Items.Clear();
            foreach (string asset in Assets.GetAll())
            {
                cBoxAssets.Items.Add(asset);
            }
            cBoxAssets.SelectedIndex = 0;
        }

        private void cBoxContracts_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateContracts();
        }

        private void PopulateContracts()
        {
            contractsList.Items.Clear();

            int currMonth = DateTime.Now.Month;
            int currYear = DateTime.Now.Year;

            for (int year = 2020; year < currYear; year++)
            {
                for (int month = 3; month <= 12; month += 3)
                {
                    Contract contract = new Contract(cBoxAssets.Text, month, year);
                    contractsList.Items.Add(contract);
                }
            }

            for (int month = 3; month <= currMonth; month += 3)
            {
                Contract contract = new Contract(cBoxAssets.Text, month, currYear);
                contractsList.Items.Add(contract);
            }
        }

        private void startCollectionBtn_Click(object sender, EventArgs e)
        {
            List<Contract> selectedContracts = new List<Contract>();
            foreach (object item in contractsList.CheckedItems)
            {
                selectedContracts.Add(item as Contract);
            }

            Progress progressObj = new Progress(lblProgress, progressBar);

            dcLogic = new DataCollectionLogic(selectedContracts, progressObj);

            dataCollectionThread = new Thread(() => dcLogic.StartCollection());
            dataCollectionThread.IsBackground = true;
            dataCollectionThread.Start();

            startCollectionBtn.Enabled = false;
        }

        private void CollectorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dcLogic != null && dataCollectionThread != null)
            {
                dcLogic.KillBarsRequest();
                dataCollectionThread.Abort();
            }
        }
    }
}
