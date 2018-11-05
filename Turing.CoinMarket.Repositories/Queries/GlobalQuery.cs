using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Repositories.CoinMarketModels;
using Turing.CoinMarket.Repositories.Requests;

namespace Turing.CoinMarket.Repositories.Queries
{
    public class GlobalQuery : IGlobalQuery
    {
        public async Task<CryptoGlobalModel> Get()
        {
            var request = new GlobalRequest();
            return await Get(request);
        }

        public async Task<CryptoGlobalModel> Get(GlobalRequest request)
        {
            var urlParams = PrepareQuery(request);

            var result = await ApiConnecter.GetApiResult(urlParams);
            var parsedResult = ParseResult(result);
            var mappedResult = MapResult(parsedResult, request.Currency);
            return mappedResult;
        }

        private string PrepareQuery(GlobalRequest request)
        {
            return string.Format(ApiConnecter.GlobalUrl, request.Currency);
        }

        private CryptoGlobalModel MapResult(GlobalModel parsedResult, string currency)
        {
            var output = new CryptoGlobalModel
            {
                MarketCount = parsedResult.data.active_markets,
                CryptoCurrenciesCount = parsedResult.data.active_cryptocurrencies,
                BtcAmountPercentage = parsedResult.data.bitcoin_percentage_of_market_cap
            };

            if (!parsedResult.data.quotes.ContainsKey(currency))
            {
                Trace.TraceError("Currency for Global tab was not found");
                return output;
            }

            var quote = parsedResult.data.quotes.FirstOrDefault(s => s.Key == currency);
            output.TotalMarketCap = quote.Value.total_market_cap;

            return output;
        }

        private GlobalModel ParseResult(string result)
        {
            var parsedResult = JsonConvert.DeserializeObject<GlobalModel>(result);
            return parsedResult;
        }
    }

    public interface IGlobalQuery
    {
        Task<CryptoGlobalModel> Get();
    }    
}
