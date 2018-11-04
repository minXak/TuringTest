namespace Turing.CoinMarket.Test.UI
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