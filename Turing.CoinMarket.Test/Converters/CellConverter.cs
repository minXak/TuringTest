using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Turing.CoinMarket.DataModels;

namespace Turing.CoinMarket.Test.UI.Converters
{
    public class CellConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is CryptoCurrencyModel))
            {
                return null;
            }

            if (((CryptoCurrencyModel) value).PercentageMoveLast24H > 0)
            {
                return new SolidColorBrush(Colors.ForestGreen);
            }

            if (((CryptoCurrencyModel)value).PercentageMoveLast24H < 0)
            {
                return new SolidColorBrush(Colors.OrangeRed);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(Colors.Black);
        }
    }
}