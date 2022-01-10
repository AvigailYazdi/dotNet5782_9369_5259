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
        private void showCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CustomersListPage(bl));
        }

        private void showParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ParcelsListPage(bl));
        }

        private void showStationsButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StationsListPage(bl));
        }
        private void showDronesButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DronesListPage(bl));
        }
    }
}
