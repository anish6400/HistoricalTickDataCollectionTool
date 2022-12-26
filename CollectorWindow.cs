using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public partial class CollectorWindow : Form
    {
        public CollectorWindow()
        {
            InitializeComponent();
        }

        private void CollectorWindow_Load(object sender, EventArgs e)
        {
            PopulateAssets();
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

            for (int year = 2018; year < currYear; year++)
            {
                for (int month = 3; month <= 12; month += 3)
                {
                    Contract contract = new Contract(cBoxAssets.Text, month, year);
                    contractsList.Items.Add(contract);
                }
            }

            for (int month = 3; month < currMonth; month += 3)
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

            DataCollectionLogic dcThread = new DataCollectionLogic(selectedContracts, progressObj);

            Thread dataCollectionThread = new Thread(() => dcThread.StartCollection());
            dataCollectionThread.IsBackground = true;
            dataCollectionThread.Start();
        }
    }
}
