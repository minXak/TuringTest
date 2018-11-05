namespace Turing.CoinMarket.Test.UI.Models
{
    public class TrackCryptoModel
    {
        public string Symbol { get; set; }
        public decimal Threshold { get; set; }
        public bool IsUpper { get; set; }

        public decimal LastPrice { get; set; }
    }
}