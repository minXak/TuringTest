using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Repositories.CoinMarketModels;
using Turing.CoinMarket.Repositories.Requests;

namespace Turing.CoinMarket.Repositories.Queries
{
    public class CoinMarketCapQuery : ICoinMarketCapRepository
    {
        public async Task<List<CryptoCurrencyModel>> GetAll()
        {
            var request = new CoinMarketRequest();
            return await GetAll(request);
        }

        public async Task<List<CryptoCurrencyModel>> GetAll(CoinMarketRequest request)
        {
            var urlParams = PrepareQuery(request);

            var result = await ApiConnecter.GetApiResult(urlParams);
            var parsedResult = ParseResult(result);
            var mappedResult = MapResult(parsedResult, request.Currency);
            return mappedResult;
        }

        private string PrepareQuery(CoinMarketRequest request)
        {
            return string.Format(ApiConnecter.TickerUrl, request.Currency, request.Start, request.Limit);
        }

        private List<CryptoCurrencyModel> MapResult(TickerModel parsedResult, string currency)
        {
            var mappedResult = new List<CryptoCurrencyModel>();
            foreach (var tickerModelItem in parsedResult.data)
            {
                if (!tickerModelItem.quotes.ContainsKey(currency))
                {
                    continue;
                }

                var quote = tickerModelItem.quotes.FirstOrDefault(s => s.Key == currency);

                mappedResult.Add(MapCryptoCurrencyModel(quote, tickerModelItem));
            }

            return mappedResult;
        }

        private static CryptoCurrencyModel MapCryptoCurrencyModel(KeyValuePair<string, TickerQuote> quote, TickerModelItem tickerModelItem)
        {
            return new CryptoCurrencyModel
            {
                MarketCap = quote.Value.market_cap.GetValueOrDefault(0),
                Name = tickerModelItem.name,
                Symbol = tickerModelItem.symbol,
                Price = quote.Value.price.GetValueOrDefault(0),
                TokenAmount = tickerModelItem.total_supply.GetValueOrDefault(0),
                PercentageMoveLast24H = quote.Value.percent_change_24h.GetValueOrDefault(0),
                TransactionTotalAmountLast24H = quote.Value.volume_24h.GetValueOrDefault(0)
            };
        }

        private TickerModel ParseResult(string result)
        {
            var parsedResult = JsonConvert.DeserializeObject<TickerModel>(result);
            return parsedResult;
        }
    }

    public interface ICoinMarketCapRepository
    {
        Task<List<CryptoCurrencyModel>> GetAll();
    }
}
