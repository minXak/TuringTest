using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FontAwesome.WPF;
using Turing.CoinMarket.Repositories.Queries;
using Turing.CoinMarket.Test.UI.Controllers;
using Turing.CoinMarket.Test.UI.Factories;
using Turing.CoinMarket.Test.UI.Models;

namespace Turing.CoinMarket.Test.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CoinMarketCapQuery _coinMarketCapQuery;
        private Pager _pager;
        private MainWindowController _controller;
        private GlobalController _globalController;
        private ThresholdWatcherController _tresholdWatcherController;

        public GlobalTab GlobalTab
        {
            get => ((MainViewModel)this.DataContext).GlobalTab;
            set => ((MainViewModel)this.DataContext).GlobalTab = value;
        }

        public MainWindow()
        {
            InitDependencies();
            InitializeComponent();
            InitEvents();
        }

        private void InitEvents()
        {
            this.Loaded += Window_Loaded;            
            this.CryptoCurrencyGrid.MouseDoubleClick += _tresholdWatcherController.CryptoCurrencyGridOnMouseDoubleClick;

            this.CoinMarketMenuItem.Click += _controller.CoinMarket_Click;
            this.GlobalMenuItem.Click += _controller.Global_Click;
        }

        private void InitDependencies()
        {
            this._pager = new Pager();
            this._coinMarketCapQuery = new CoinMarketCapQuery();
            this.DataContext = new MainViewModel();

            this._globalController = new GlobalController(this);
            this._tresholdWatcherController = new ThresholdWatcherController(this);
            this._controller = new MainWindowController(this, _tresholdWatcherController);
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

        public string GetCurrency()
        {
            return (CbxCurrency.SelectedItem as ComboBoxItem).Content.ToString();
        }

        private async void Prevoius_Click(object sender, RoutedEventArgs e)
        {
            _pager.PreviousPage();
            DisablePreviousIfNeeded();

            await LoadPage();
        }

        private void DisablePreviousIfNeeded()
        {
            if (_pager.CurrentPage == 1)
            {
                PreviousPage.IsEnabled = false;
            }
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

        public async Task RefreshAll()
        {
            await LoadPage();
            await _globalController.LoadGlobal();
        }       
    }
}

