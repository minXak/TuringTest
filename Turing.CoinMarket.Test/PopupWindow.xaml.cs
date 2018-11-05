using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Turing.CoinMarket.Test.UI
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        public PopupWindow()
        {
            this.DataContext = new PopupViewModel();
            InitializeComponent();   
        }

        public PopupViewModel GetTypedContext()
        {
            return this.DataContext as PopupViewModel;;
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Visibility = Visibility.Hidden;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            this.Visibility = Visibility.Hidden;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[\\d \\.]");

            if (e.Text.Contains(".") && (e.Source as TextBox).Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            e.Handled = !regex.IsMatch(e.Text);            
        }
    }
}
