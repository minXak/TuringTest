using System.ComponentModel;
using System.Runtime.CompilerServices;
using Turing.CoinMarket.Test.UI.Annotations;

namespace Turing.CoinMarket.Test.UI
{
    public class PopupViewModel : INotifyPropertyChanged
    {
        private string _symbol;
        private decimal _threshold;
        private bool _isUpper;
        private DirectionEnum _directionEnum;

        public string Symbol
        {
            get => _symbol;
            set
            {
                _symbol = value;
                OnPropertyChanged(nameof(Symbol));
            }
        }

        public decimal Threshold
        {
            get => _threshold;
            set
            {
                _threshold = value;
                OnPropertyChanged(nameof(Threshold));
            }
        }

        public bool IsUpper
        {
            get => Direction == DirectionEnum.Up;           
        }

        public DirectionEnum Direction
        {
            get => _directionEnum;
            set
            {
                _directionEnum = value;
                OnPropertyChanged(nameof(Direction));

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum DirectionEnum
    {
        Up = 0,
        Down
    }
}