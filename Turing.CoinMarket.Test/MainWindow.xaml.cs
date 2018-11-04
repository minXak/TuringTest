using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using FontAwesome.WPF;
using Turing.CoinMarket.Repositories;

namespace Turing.CoinMarket.Test.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CoinMarketCapQuery _coinMarketCapQuery;
        private readonly GlobalQuery _globalQuery;
        private readonly Pager _pager;

        private GlobalTab GlobalTab
        {
            get => ((MainViewModel) this.DataContext).GlobalTab;
            set => ((MainViewModel) this.DataContext).GlobalTab = value;
        }

        public MainWindow()
        {
            Loaded += Window_Loaded;

            _pager = new Pager();
            this._coinMarketCapQuery = new CoinMarketCapQuery();
            this._globalQuery = new GlobalQuery();

            this.DataContext = new MainViewModel();
            InitializeComponent();            
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            await RefreshAll();
        }

        private async Task LoadGlobal()
        {
            try
            {
                var request = RequesetFactory.NewGlobal(GetCurrency());
                var result = await _globalQuery.Get(request);
                GlobalTab.BtcPercentage = result.BtcAmountPercentage;
                GlobalTab.CryptoCurrencyCount = result.CryptoCurrenciesCount;
                GlobalTab.MarketCount = result.MarketCount;
                GlobalTab.TotalMarketCap = result.TotalMarketCap;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await RefreshAll();
            InitTimer();
        }

        private void InitTimer()
        {
            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(10);            
            dispatcherTimer.Start();
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
                this.CryptoCurrencyGrid.ItemsSource = await _coinMarketCapQuery.GetAll(request);
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

        private void Global_Click(object sender, RoutedEventArgs e)
        {
            GlobalPanel.Visibility = Visibility.Visible;
            CoinPanel.Visibility = Visibility.Collapsed;     
            GlobalMenuItem.Template = this.Resources["MenuTopSelected"] as ControlTemplate;
            CoinMarketMenuItem.Template = this.Resources["MenuTop"] as ControlTemplate;
        }

        private void CoinMarket_Click(object sender, RoutedEventArgs e)
        {
            GlobalPanel.Visibility = Visibility.Collapsed;
            CoinPanel.Visibility = Visibility.Visible;
            GlobalMenuItem.Template = this.Resources["MenuTop"] as ControlTemplate;
            CoinMarketMenuItem.Template = this.Resources["MenuTopSelected"] as ControlTemplate;
        }
    }
}

