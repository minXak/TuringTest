﻿namespace Turing.CoinMarket.Repositories
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