using System.ComponentModel;
using System.Runtime.CompilerServices;
using Turing.CoinMarket.Test.UI.Annotations;

namespace Turing.CoinMarket.Test.UI
{
    public class GlobalTab : INotifyPropertyChanged
    {
        private int _marketCount;
        private int _cryptoCurrencyCount;
        private decimal? _totalMarketCap;
        private decimal _btcPercentage;

        public int MarketCount
        {
            get => _marketCount;
            set
            {
                _marketCount = value;
                OnPropertyChanged(nameof(MarketCount));
            }
        }

        public int CryptoCurrencyCount
        {
            get => _cryptoCurrencyCount;
            set
            {
                _cryptoCurrencyCount = value;
                OnPropertyChanged(nameof(CryptoCurrencyCount));
            }
        }

        public decimal? TotalMarketCap
        {
            get => _totalMarketCap;
            set
            {
                _totalMarketCap = value;
                OnPropertyChanged(nameof(TotalMarketCap));
            }
        }

        public decimal BtcPercentage
        {
            get => _btcPercentage;
            set
            {
                _btcPercentage = value;
                OnPropertyChanged(nameof(BtcPercentage));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}