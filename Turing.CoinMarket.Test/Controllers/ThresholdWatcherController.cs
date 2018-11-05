using System.Collections.Generic;
using System.Windows.Input;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Test.UI.Models;

namespace Turing.CoinMarket.Test.UI.Controllers
{
    public class ThresholdWatcherController
    {
        private readonly MainWindow _mainWindow;
        public readonly Dictionary<string, TrackCryptoModel> CryptoTracker = new Dictionary<string, TrackCryptoModel>();


        public ThresholdWatcherController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void CryptoCurrencyGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (_mainWindow.CryptoCurrencyGrid.SelectedItem as CryptoCurrencyModel);
            var popup = new PopupWindow();

            LoadPopupWindow(item, popup.GetTypedContext());

            var result = popup.ShowDialog();

            ProcessDialogResult(result, popup);
        }

        private void ProcessDialogResult(bool? result, PopupWindow popup)
        {
            if (result.GetValueOrDefault(false))
            {
                var popupData = popup.GetTypedContext();
                this.CryptoTracker.Add(popup.PopupSymbol.Text, new TrackCryptoModel
                {
                    Symbol = popupData.Symbol,
                    Threshold = popupData.Threshold,
                    IsUpper = popupData.IsUpper
                });
            }
        }

        private void LoadPopupWindow(CryptoCurrencyModel item, PopupViewModel popupDataContext)
        {
            if (CryptoTracker.ContainsKey(item.Symbol))
            {
                var tracker = CryptoTracker[item.Symbol];
                popupDataContext.Direction = tracker.IsUpper ? DirectionEnum.Up : DirectionEnum.Down;
                popupDataContext.Symbol = tracker.Symbol;
                popupDataContext.Threshold = tracker.Threshold;
            }
            else
            {
                popupDataContext.Direction = DirectionEnum.Up;
                popupDataContext.Symbol = item.Symbol;
                popupDataContext.Threshold = item.Price;
            }
        }
    }
}