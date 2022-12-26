using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class Logger
    {
        private static TextBox txtBoxLogs;
        private static StringBuilder logStr;

        public static void Init(TextBox txtBoxLogs)
        {
            Logger.txtBoxLogs = txtBoxLogs;
            logStr = new StringBuilder();
        }

        public static void LogMessage(string message)
        {
            logStr.Append(message);
            logStr.AppendLine();
            UpdateTextBox();
        }

        public static void LogException(string message, Exception e)
        {
            LogMessage(message);
            logStr.Append(e.ToString());
            logStr.AppendLine();
            UpdateTextBox();
        }

        public static void ClearLogs()
        {
            logStr.Clear();
            UpdateTextBox();
        }

        private static void ValidateTextBoxInit()
        {
            if (txtBoxLogs == null)
            {
                throw new NotImplementedException("Logs text box not initialized. Make sure to have called init method before any others.");
            }
        }

        private static void UpdateTextBox()
        {
            ValidateTextBoxInit();
            txtBoxLogs.Text = logStr.ToString();
        }
    }
}
