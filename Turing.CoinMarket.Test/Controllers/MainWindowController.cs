using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Test.UI.Models;

namespace Turing.CoinMarket.Test.UI.Controllers
{
    public class MainWindowController
    {
        private readonly MainWindow _window;
        private readonly ThresholdWatcherController _tresholdWatcherController;

        public List<CryptoCurrencyModel> LocalItems { get; set; }

        public event EventHandler ThresholdCheckerEvent;

        public MainWindowController(MainWindow window, 
            ThresholdWatcherController tresholdWatcherController)
        {
            _window = window;
            _tresholdWatcherController = tresholdWatcherController;
            this.ThresholdCheckerEvent += ThresholdChecker;
        }

        public void Global_Click(object sender, RoutedEventArgs e)
        {
            _window.GlobalPanel.Visibility = Visibility.Visible;
            _window.CoinPanel.Visibility = Visibility.Collapsed;
            _window.GlobalMenuItem.Template = _window.Resources["MenuTopSelected"] as ControlTemplate;
            _window.CoinMarketMenuItem.Template = _window.Resources["MenuTop"] as ControlTemplate;
        }

        public void CoinMarket_Click(object sender, RoutedEventArgs e)
        {
            _window.GlobalPanel.Visibility = Visibility.Collapsed;
            _window.CoinPanel.Visibility = Visibility.Visible;
            _window.GlobalMenuItem.Template = _window.Resources["MenuTop"] as ControlTemplate;
            _window.CoinMarketMenuItem.Template = _window.Resources["MenuTopSelected"] as ControlTemplate;
        }

        public void InitTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
            dispatcherTimer.Start();
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            await _window.RefreshAll();
        }

        private void ThresholdChecker(object sender, EventArgs e)
        {
            var reachedThresholdList = new List<TrackCryptoModel>();
            foreach (var trackCryptoModel in _tresholdWatcherController.CryptoTracker)
            {
                var currency = this.LocalItems.FirstOrDefault(s => s.Symbol == trackCryptoModel.Value.Symbol);
                if (currency != null)
                {
                    if (trackCryptoModel.Value.IsUpper)
                    {
                        if (currency.Price > trackCryptoModel.Value.Threshold &&
                            trackCryptoModel.Value.LastPrice < trackCryptoModel.Value.Threshold)
                        {
                            reachedThresholdList.Add(trackCryptoModel.Value);
                        }
                    }
                    if (currency.Price < trackCryptoModel.Value.Threshold &&
                        trackCryptoModel.Value.LastPrice > trackCryptoModel.Value.Threshold)
                    {
                        reachedThresholdList.Add(trackCryptoModel.Value);
                    }
                }
            }

            if (reachedThresholdList.Any())
            {
                var stringBuilder = new StringBuilder();
                foreach (var cryptoModel in reachedThresholdList)
                {
                    stringBuilder.Append($"{cryptoModel.Symbol} - reached threshold {cryptoModel.Threshold} \n");
                }
                MessageBox.Show(stringBuilder.ToString(),
                    "Threshold alert",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }

        public virtual void InvokeThresholdCheckerEvent()
        {
            ThresholdCheckerEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}