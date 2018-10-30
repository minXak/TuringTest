using System;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Turing.CoinMarket.Repositories;

namespace Turing.CoinMarket.Query.Tests
{
    [TestFixture]
    public class CoinMarketCapQueryTests
    {
        [Test]
        public async Task BaseTest()
        {
            var query = new CoinMarketCapQuery();

            var result = await query.GetAll();
            Assert.IsNotNull(result);
            CollectionAssert.IsNotEmpty(result);
        }

        [TestCase("")]
        [TestCase("USD", 0, 100)]
        [TestCase("EUR")]
        [TestCase("BTC")]
        public async Task BaseTest_Currency(string currency, int start = 1, int limit = 10)
        {
            var query = new CoinMarketCapQuery();

            var result = await query.GetAll(new CoinMarketRequest { Currency = currency, Limit = limit, Start = start });
            Assert.IsNotNull(result);
        }
    }
}
