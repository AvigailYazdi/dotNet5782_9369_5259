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
    /// Interaction logic for ManagerAreaPage.xaml
    /// </summary>
    public partial class ManagerAreaPage : Page
    {
        IBL bl;
        public ManagerAreaPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }



        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image i = sender as Image;
            i.Width += 10;
            i.Height += 10;
            if (i.Name == "DroneImage")
                DroneLabel.FontSize += 5;
            else if (i.Name == "ParcelImage")
                ParcelLabel.FontSize += 5;
            else if (i.Name == "CustomerImage")
                CustomerLabel.FontSize += 5;
            else
                StationLabel.FontSize += 5;

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image i = sender as Image;
            i.Width -= 10;
            i.Height -= 10;
            if (i.Name == "DroneImage")
                DroneLabel.FontSize -= 5;
            else if (i.Name == "ParcelImage")
                ParcelLabel.FontSize -= 5;
            else if (i.Name == "CustomerImage")
                CustomerLabel.FontSize -= 5;
            else
                StationLabel.FontSize -= 5;
        }

        private void DroneImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new DronesListPage(bl));
        }

        private void ParcelImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new ParcelsListPage(bl));
        }

        private void CustomerImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new CustomersListPage(bl));
        }

        private void StationImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new StationsListPage(bl));
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
