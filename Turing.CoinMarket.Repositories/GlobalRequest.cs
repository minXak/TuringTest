namespace Turing.CoinMarket.Repositories
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