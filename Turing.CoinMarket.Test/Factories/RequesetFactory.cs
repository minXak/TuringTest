﻿using Turing.CoinMarket.Repositories;
using Turing.CoinMarket.Repositories.Requests;
using Turing.CoinMarket.Test.UI.Models;

namespace Turing.CoinMarket.Test.UI.Factories
{
    public class RequesetFactory
    {
        public static CoinMarketRequest New(Pager pager, string currency)
        {
            return new CoinMarketRequest
            {
                Start = pager.CurrentPage * pager.PageSize,
                Limit = pager.PageSize,
                Currency = currency
            };
        }

        public static GlobalRequest NewGlobal( string currency)
        {
            return new GlobalRequest
            {
                Currency = currency
            };
        }
    }
}