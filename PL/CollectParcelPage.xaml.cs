using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CollectParcelPage.xaml
    /// </summary>
    public partial class CollectParcelPage : Page
    {
        IBL bl;
        string UserName;
        ObservableCollection<BO.ParcelToL> oParcel;
        public CollectParcelPage(IBL _bl, string _UserName)
        {
            InitializeComponent();
            bl = _bl;
            UserName = _UserName;
            oParcel = new ObservableCollection<BO.ParcelToL>(bl.GetParcelByPredicate(p => p.ReceiverName == UserName && p.Status == BO.ParcelStatus.Provided));
            parcelToLDataGrid.DataContext = oParcel;
            parcelToLDataGrid.IsReadOnly = true;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            BO.ParcelToL currentParcel = parcelToLDataGrid.SelectedItem as BO.ParcelToL;
            oParcel.Remove(currentParcel);
        }
    }
}
