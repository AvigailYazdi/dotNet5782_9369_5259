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
    /// Interaction logic for MyParcelsPage.xaml
    /// </summary>
    public partial class MyParcelsPage : Page
    {
        IBL bl;
        string UserName;
        public MyParcelsPage(IBL _bl, string _UserName)
        {
            InitializeComponent();
            bl = _bl;
            UserName = _UserName;
            parcelToLDataGrid.DataContext = bl.GetParcelByPredicate(p=>p.SenderName==UserName);
            parcelToLDataGrid.IsReadOnly = true;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void parcelToLDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelToL curParcelToL = parcelToLDataGrid.SelectedItem as BO.ParcelToL;
            if (curParcelToL != null)
            {
                BO.Parcel p = bl.GetParcel(curParcelToL.Id);
                this.NavigationService.Navigate(new ParcelPage(bl, p));
            }
        }
    }
}
