namespace Turing.CoinMarket.Test.UI.Models
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            this.GlobalTab = new GlobalTab();
        }

        public GlobalTab GlobalTab { get; set; }
    }
}