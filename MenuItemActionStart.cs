#region Using declarations

using System;
using System.Windows;
using System.Windows.Controls;
using NinjaTrader.Core;
using NinjaTrader.Gui;
using NinjaTrader.Gui.Tools;
using NinjaTrader.NinjaScript;

#endregion

namespace NinjaTrader.Custom.AddOns.HistoricalTickDataCollectionTool
{
    public class MenuItemActionStart : AddOnBase
    {
        private NTMenuItem menuItemsController;
        private NTMenuItem toolMenuItem;

        protected override void OnWindowCreated(Window window)
        {
            var controlCenter = window as ControlCenter;
            if (controlCenter == null) return;

            menuItemsController = controlCenter.FindFirst("ControlCenterMenuItemTools") as NTMenuItem;
            if (menuItemsController == null) return;

            toolMenuItem = new NTMenuItem
            {
                Header = "Historical Tick Data Collection Tool",
                Style = (Style)Application.Current.TryFindResource("MainMenuItem")
            };
            toolMenuItem.Click += OpenCollectorWindow;

            menuItemsController.Items.Add(toolMenuItem);
        }

        protected override void OnWindowDestroyed(Window window)
        {
            if (toolMenuItem != null && window is ControlCenter)
            {
                if (menuItemsController != null && menuItemsController.Items.Contains(toolMenuItem))
                {
                    menuItemsController.Items.Remove(toolMenuItem);
                }
                toolMenuItem = null;
            }
            toolMenuItem.Click -= OpenCollectorWindow;
        }

        private void OpenCollectorWindow(object sender, RoutedEventArgs e)
        {
            Globals.RandomDispatcher.BeginInvoke(new Action(() => new CollectorWindow().Show()));
        }
    }
}
