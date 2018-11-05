using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using FontAwesome.WPF;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Repositories;
using Turing.CoinMarket.Test.UI.Controllers;

namespace Turing.CoinMarket.Test.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CoinMarketCapQuery _coinMarketCapQuery;
        private GlobalQuery _globalQuery;
        private Pager _pager;
        private readonly MainWindowController _controller;
        private readonly Dictionary<string, TrackCryptoModel> _cryptoTracker = new Dictionary<string, TrackCryptoModel>();
        private readonly DataGridController _dataGridController;        

        private GlobalTab GlobalTab
        {
            get => ((MainViewModel)this.DataContext).GlobalTab;
            set => ((MainViewModel)this.DataContext).GlobalTab = value;
        }

        public MainWindow()
        {
            InitDependencies();
            InitializeComponent();
            InitEvents();

            _dataGridController = new DataGridController(this);
            _controller = new MainWindowController(this, _dataGridController);
        }
        
        private void InitEvents()
        {
            this.Loaded += Window_Loaded;            
            this.CryptoCurrencyGrid.MouseDoubleClick += CryptoCurrencyGridOnMouseDoubleClick;

            this.CoinMarketMenuItem.Click += _controller.CoinMarket_Click;
            this.GlobalMenuItem.Click += _controller.Global_Click;
        }

        private void InitDependencies()
        {
            this._pager = new Pager();
            this._coinMarketCapQuery = new CoinMarketCapQuery();
            this._globalQuery = new GlobalQuery();
            this.DataContext = new MainViewModel();
        }

        private void CryptoCurrencyGridOnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = (this.CryptoCurrencyGrid.SelectedItem as CryptoCurrencyModel);
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
                this._cryptoTracker.Add(popup.PopupSymbol.Text, new TrackCryptoModel
                {
                    Symbol = popupData.Symbol,
                    Threshold = popupData.Threshold,
                    IsUpper = popupData.IsUpper
                });
            }
        }

        private void LoadPopupWindow(CryptoCurrencyModel item, PopupViewModel popupDataContext)
        {
            if (_cryptoTracker.ContainsKey(item.Symbol))
            {
                var tracker = _cryptoTracker[item.Symbol];
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

        private async Task LoadGlobal()
        {
            try
            {
                var request = RequesetFactory.NewGlobal(GetCurrency());
                var result = await _globalQuery.Get(request);

                SetGlobalView(result);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void SetGlobalView(CryptoGlobalModel result)
        {
            GlobalTab.BtcPercentage = result.BtcAmountPercentage;
            GlobalTab.CryptoCurrencyCount = result.CryptoCurrenciesCount;
            GlobalTab.MarketCount = result.MarketCount;
            GlobalTab.TotalMarketCap = result.TotalMarketCap;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAll();
            _controller.InitTimer();
        }

        private async void Next_Click(object sender, RoutedEventArgs e)
        {
            _pager.NextPage();
            EnablePreviousIfNeeded();
            await LoadPage();
        }

        private void EnablePreviousIfNeeded()
        {
            if (_pager.CurrentPage > 1)
            {
                PreviousPage.IsEnabled = true;
            }
        }

        private async Task LoadPage()
        {
            LoadingSpinner.Icon = FontAwesomeIcon.Spinner;
            LoadingSpinner.Spin = true;
            try
            {
                var request = RequesetFactory.New(_pager, GetCurrency());
                this._controller.LocalItems = await _coinMarketCapQuery.GetAll(request);
                this.CryptoCurrencyGrid.ItemsSource = _controller.LocalItems;
                _controller.InvokeThresholdCheckerEvent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                LoadingSpinner.Icon = FontAwesomeIcon.Check;
                LoadingSpinner.Spin = false;
            }
        }

        private string GetCurrency()
        {
            return (CbxCurrency.SelectedItem as ComboBoxItem).Content.ToString();
        }

        private void DisablePreviousIfNeeded()
        {
            if (_pager.CurrentPage == 1)
            {
                PreviousPage.IsEnabled = false;
            }
        }

        private async void Prevoius_Click(object sender, RoutedEventArgs e)
        {
            _pager.PreviousPage();
            DisablePreviousIfNeeded();

            await LoadPage();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await RefreshAll();        
        }

        private async void CbxCurrency_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }

            await RefreshAll();
        }

        private async Task RefreshAll()
        {
            await LoadPage();
            await LoadGlobal();
        }       
    }
}

