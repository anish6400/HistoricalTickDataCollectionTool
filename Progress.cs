using System;
using System.Windows.Forms;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class Progress
    {

        Label lblProgress;
        float progressVal;
        ProgressBar progressBar;

        public Progress(Label lblProgress, ProgressBar progressBar)
        {
            progressVal = 0;
            this.lblProgress = lblProgress;
            this.progressBar = progressBar;
        }

        public void updateProgress(float progressVal)
        {
            this.progressVal = progressVal;
            lblProgress.Text = progressVal.ToString("0.00") + "%";
            progressBar.Value = (int)Math.Floor(progressVal);
        }

    }
}
