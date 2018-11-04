using System;
using System.Collections.Generic;

namespace Turing.CoinMarket.Repositories.CoinMarketModels
{
    public class GlobalModel
    {
        public GlobalModelData data { get; set; }      
    }

    public class GlobalModelData
    {
        public int active_cryptocurrencies { get; set; }
        public int active_markets { get; set; }
        public decimal bitcoin_percentage_of_market_cap { get; set; }
        public Dictionary<string, GlobalQuote> quotes { get; set; }

    }

    public class GlobalQuote
    {
        public decimal? total_market_cap { get; set; }
        public decimal? total_volume_24h { get; set; }       
    }
}