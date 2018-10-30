using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Turing.CoinMarket.DataModels;
using Turing.CoinMarket.Repositories.CoinMarketModels;

namespace Turing.CoinMarket.Repositories
{
    public class CoinMarketCapQuery : ICoinMarketCapRepository
    {
        private const string TickerUrl = "/v2/ticker/?sort=id&structure=array&convert={0}&start={1}&limit={2}";
        private const string CoinMarketBase = "https://api.coinmarketcap.com";

        public async Task<List<CryptoCurrencyModel>> GetAll()
        {
            var request = new CoinMarketRequest();
            return await GetAll(request);
        }

        public async Task<List<CryptoCurrencyModel>> GetAll(CoinMarketRequest request)
        {
            var urlParams = PrepareQuery(request);
            var result = await GetApiResult(urlParams);

            var parsedResult = ParseResult(result);
            var mappedResult = MapResult(parsedResult, request.Currency);
            return mappedResult;
        }

        private string PrepareQuery(CoinMarketRequest request)
        {
            return string.Format(TickerUrl, request.Currency, request.Start, request.Limit);
        }

        private static async Task<string> GetApiResult(string urlParams)
        {
            using (var client = new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }))
            {
                client.BaseAddress = new Uri(CoinMarketBase);
                HttpResponseMessage response = await client.GetAsync(urlParams);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
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
                
                mappedResult.Add(new CryptoCurrencyModel
                {
                    MarketCap = quote.Value.market_cap.GetValueOrDefault(0),
                    Name = tickerModelItem.name,
                    Symbol = tickerModelItem.symbol,
                    Price = quote.Value.price.GetValueOrDefault(0),
                    TokenAmount = tickerModelItem.total_supply.GetValueOrDefault(0),
                    PercentageMoveLast24H = quote.Value.percent_change_24h.GetValueOrDefault(0),
                    TransactionTotalAmountLast24H = quote.Value.voume_24h.GetValueOrDefault(0)
                });
            }

            return mappedResult;
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
        Task<List<CryptoCurrencyModel>> GetAll(CoinMarketRequest request);
    }

    public class CoinMarketRequest
    {
        public CoinMarketRequest()
        {
            Currency = "USD";
            Limit = 100;
            Start = 0;
        }

        public string Currency { get; set; }
        public int Limit { get; set; }
        public int Start { get; set; }
    }
}
