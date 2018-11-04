using System.Collections.Generic;
using System.Threading.Tasks;
using Turing.CoinMarket.DataModels;

namespace Turing.CoinMarket.Repositories
{
    public interface IPagableRepository<in T> where T : PageableRequest
    {
        Task<List<CryptoCurrencyModel>> GetAll(T request);
    }
}