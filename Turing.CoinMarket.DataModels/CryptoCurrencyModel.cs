namespace Turing.CoinMarket.DataModels
{
    public class CryptoCurrencyModel
    {
        public string Name { get; set; }
        public string Symbol { get; set; }

        public decimal MarketCap { get; set; }
        public decimal Price { get; set; }
        public decimal TransactionTotalAmountLast24H { get; set; }
        public decimal PercentageMoveLast24H { get; set; }
        public decimal TokenAmount { get; set; }
    }
}
