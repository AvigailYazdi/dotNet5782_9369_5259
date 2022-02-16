using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInPage.xaml
    /// </summary>
    public partial class LogInPage : Page
    {
        IBL bl;
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        public LogInPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BO.User u = bl.GetUser(PasswordTextBox.Text);
                if (u.UserRole == BO.Role.Manager)
                    this.NavigationService.Navigate(new ManagerAreaPage(bl));
                else
                {
                    //לשלוח פרטים על הלקוח
                    this.NavigationService.Navigate(new UserAreaPage(bl));
                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (NameTextBox.Text != "" && PasswordTextBox.Text != "")
            {
                SignButton.IsEnabled = true;
            }
            else
                SignButton.IsEnabled = false;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
