﻿namespace Turing.CoinMarket.Repositories.Requests
{
    public class PageableRequest
    {
        public PageableRequest()
        {
            Limit = 10;
            Start = 0;
        }

        public int Limit { get; set; }
        public int Start { get; set; }
    }
}