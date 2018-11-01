using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Turing.CoinMarket.Repositories
{
    public class ApiConnecter
    {
        public const string CoinMarketBase = "https://api.coinmarketcap.com";
        public const string TickerUrl = "/v2/ticker/?sort=symbol&structure=array&convert={0}&start={1}&limit={2}";

        public static async Task<string> GetApiResult(string urlParams)
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
    }
}