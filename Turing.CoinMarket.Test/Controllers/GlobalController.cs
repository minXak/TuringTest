using System;
using System.Threading.Tasks;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Repositories;
using Turing.CoinMarket.Repositories.Queries;
using Turing.CoinMarket.Test.UI.Factories;

namespace Turing.CoinMarket.Test.UI.Controllers
{
    public class GlobalController
    {
        private readonly MainWindow _mainWindow;
        private GlobalQuery _globalQuery;

        public GlobalController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            this._globalQuery = new GlobalQuery();
        }

        public async Task LoadGlobal()
        {
            try
            {
                var request = RequesetFactory.NewGlobal(_mainWindow.GetCurrency());
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
            _mainWindow.GlobalTab.BtcPercentage = result.BtcAmountPercentage;
            _mainWindow.GlobalTab.CryptoCurrencyCount = result.CryptoCurrenciesCount;
            _mainWindow.GlobalTab.MarketCount = result.MarketCount;
            _mainWindow.GlobalTab.TotalMarketCap = result.TotalMarketCap;
        }
    }
}