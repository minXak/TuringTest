using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            InitializeComponent();

            CbxCurrency.ItemsSource = new List<string> { "USD", "EUR", "BTC" };
            _pager = new Pager();
            this._coinMarketCapQuery = new CoinMarketCapQuery();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.cryptoCurrencyGrid.ItemsSource = await _coinMarketCapQuery.GetAll();
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
            var request = RequesetFactory.New(_pager, CbxCurrency.Text);
            this.cryptoCurrencyGrid.ItemsSource = await _coinMarketCapQuery.GetAll(request);
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
            await LoadPage();
        }
    }
}

