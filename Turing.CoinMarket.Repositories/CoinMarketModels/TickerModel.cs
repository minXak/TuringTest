using System;
using System.Collections.Generic;

namespace Turing.CoinMarket.Repositories.CoinMarketModels
{
    public class TickerModel
    {
        public List<TickerModelItem> data { get; set; }
    }

    public class TickerModelItem
    {
        public string id { get; set; }
        public string name { get; set; }
        public string symbol { get; set; }
        public decimal? total_supply { get; set; }

        public Dictionary<string, TickerQuote> quotes { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class TickerQuote
    {
        public decimal? price { get; set; }
        public decimal? voume_24h { get; set; }
        public decimal? market_cap { get; set; }
        public decimal? percent_change_24h { get; set; }
    }
}