namespace Turing.CoinMarket.Repositories.Requests
{
    public class GlobalRequest
    {
        public GlobalRequest()
        {
            Currency = "USD";
        }

        public string Currency { get; set; }
    }
}