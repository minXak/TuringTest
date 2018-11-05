using System.Threading.Tasks;
using NUnit.Framework;
using Turing.CoinMarket.Repositories;
using Turing.CoinMarket.Repositories.Queries;
using Turing.CoinMarket.Repositories.Requests;

namespace Turing.CoinMarket.Query.Tests
{
    [TestFixture]
    public class GlobalQueryTests
    {
        [Test]
        public async Task BaseTest()
        {
            var query = new GlobalQuery();

            var result = await query.Get();
            Assert.IsNotNull(result);
        }

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("BTC")]
        public async Task BaseTest_Currency(string currency)
        {
            var query = new GlobalQuery();

            var result = await query.Get(new GlobalRequest { Currency = currency });
            Assert.IsNotNull(result);
        }
    }
}
