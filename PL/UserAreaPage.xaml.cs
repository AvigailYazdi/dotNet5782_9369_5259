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
    /// Interaction logic for UserAreaPage.xaml
    /// </summary>
    public partial class UserAreaPage : Page
    {
        IBL bl;
        string UserName;
        public UserAreaPage(IBL _bl, string _UserName)
        {
            InitializeComponent();
            bl = _bl;
            UserName = _UserName;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image i = sender as Image;
            i.Width += 10;
            i.Height += 10;
            if (i.Name == "MyParcelImage")
                MyParcelsLabel.FontSize += 5;
            else if (i.Name == "CollectImage")
                CollectLabel.FontSize += 5;
            else
                AddParcelLabel.FontSize += 5;

        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image i = sender as Image;
            i.Width -= 10;
            i.Height -= 10;
            if (i.Name == "MyParcelImage")
                MyParcelsLabel.FontSize -= 5;
            else if (i.Name == "CollectImage")
                CollectLabel.FontSize -= 5;
            else
                AddParcelLabel.FontSize -= 5;
        }

        private void MyParcelImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new MyParcelsPage(bl, UserName));
        }

        private void AddParcelImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new AddParcelPage(bl));
        }

        private void CollectImage_MouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new CollectParcelPage(bl));
        }
    }
}
