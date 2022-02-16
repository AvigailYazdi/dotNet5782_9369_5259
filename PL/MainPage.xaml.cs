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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        IBL bl;
        public MainPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void logInButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new LogInPage(bl));
        }

        private void signUpButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SignUpPage(bl));
        }
    }
}
