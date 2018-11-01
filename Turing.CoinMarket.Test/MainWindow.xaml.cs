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
        private readonly Pager _pager;

        public MainWindow()
        {
            Loaded += Window_Loaded;

            _pager = new Pager();
            this._coinMarketCapQuery = new CoinMarketCapQuery();

            InitializeComponent();            
        }

        private async void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            await LoadPage();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadPage();
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
                var request = RequesetFactory.New(_pager, (CbxCurrency.SelectedItem as ComboBoxItem).Content.ToString());
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
            await LoadPage();
        }

        private async void CbxCurrency_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsLoaded)
            {
                return;
            }
            await LoadPage();
        }
    }
}

