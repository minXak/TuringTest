namespace Turing.CoinMarket.Test.UI
{
    public class Pager
    {
        public const int DefaultPageSize = 10;

        public Pager() : this(DefaultPageSize)
        {
        }

        public Pager(int pageSize)
        {
            CurrentPage = 1;
            PageSize = pageSize;
        }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public void NextPage()
        {
            CurrentPage += 1;
        }

        public void PreviousPage()
        {
            CurrentPage -= 1;
            if (CurrentPage < 1)
            {
                CurrentPage = 1;
            }
        }
    }
}