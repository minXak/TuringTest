namespace Turing.CoinMarket.DataModels
{
    public class CryptoGlobalModel
    {
        public int MarketCount { get; set; }
        public int CryptoCurrenciesCount { get; set; }
        public decimal TotalMarketAmount { get; set; }
        public decimal BtcAmountPercentage { get; set; }
    }
}