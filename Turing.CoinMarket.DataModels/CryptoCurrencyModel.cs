namespace Turing.CoinMarket.DataModels
{
    public class CryptoCurrencyModel
    {
        public decimal MarketCap { get; set; }
        public decimal Price { get; set; }
        public long TransactionCountLast24H{ get; set; }
        public decimal PercentageMoveLast24H{ get; set; }
        public long TokenAmount { get; set; }
    }
}
