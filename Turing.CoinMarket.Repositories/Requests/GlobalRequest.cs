namespace Turing.CoinMarket.Repositories.Requests
{
    public class CoinMarketRequest : PageableRequest
    {
        public CoinMarketRequest()
        {
            Currency = "USD";
        }

        public string Currency { get; set; }
    }
}