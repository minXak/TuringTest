using System.Threading.Tasks;

namespace Turing.CoinMarket.Test.UI.Controllers
{
    public class DataGridController
    {
        private readonly MainWindow _mainWindow;


        public DataGridController(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }
        public async Task RefreshAll()
        {
        }
    }
}