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
using System.Collections.ObjectModel;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelsListPage.xaml
    /// </summary>
    public partial class ParcelsListPage : Page
    {
        IBL bl;
        ObservableCollection<BO.ParcelToL> oc;
        enum parcelStatus { Created, Connected, PickedUp, Provided, Clear };

        public ParcelsListPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            oc = new ObservableCollection<BO.ParcelToL>(bl.ParcelList());
            parcelToLDataGrid.DataContext = oc; 
            parcelToLDataGrid.IsReadOnly = true;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(parcelStatus));
        }

        private void addParcelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ParcelPage(bl));
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void parcelToLDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelToL curParcelToL = parcelToLDataGrid.SelectedItem as BO.ParcelToL;
            if (curParcelToL != null)
            {
                BO.Parcel p = bl.GetParcel(curParcelToL.Id);
                this.NavigationService.Navigate(new  ParcelPage(bl, p));
            }
        }

        private void GroupSenderCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupSenderCheckBox.IsChecked == true)
            {
                GroupReceiverCheckBox.IsEnabled = false;
                List<IGrouping<string, BO.ParcelToL>> GroupingData = bl.ParcelList()
                    .GroupBy(b => b.SenderName)
                    .ToList();
                parcelToLDataGrid2.DataContext = GroupingData;
                parcelToLDataGrid2.Visibility = Visibility.Visible;
                parcelToLDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                GroupReceiverCheckBox.IsEnabled = true;
                parcelToLDataGrid.Visibility = Visibility.Visible;
                parcelToLDataGrid2.Visibility = Visibility.Collapsed;
            }
        }

        private void GroupReceiverCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupReceiverCheckBox.IsChecked == true)
            {
                GroupSenderCheckBox.IsEnabled = false;
                List<IGrouping<string, BO.ParcelToL>> GroupingData = bl.ParcelList()
                    .GroupBy(b => b.ReceiverName)
                    .ToList();
                parcelToLDataGrid2.DataContext = GroupingData;
                parcelToLDataGrid2.Visibility = Visibility.Visible;
                parcelToLDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                GroupSenderCheckBox.IsEnabled = true;
                parcelToLDataGrid.Visibility = Visibility.Visible;
                parcelToLDataGrid2.Visibility = Visibility.Collapsed;
            }
        }

        private void group(IEnumerable<BO.ParcelToL> parcels)
        {
            if (GroupSenderCheckBox.IsChecked == true)
            {
                List<IGrouping<string, BO.ParcelToL>> GroupingData = parcels
                        .GroupBy(b => b.SenderName)
                        .ToList();
                parcelToLDataGrid2.DataContext = GroupingData;
            }
            else
            {
                List<IGrouping<string, BO.ParcelToL>> GroupingData = parcels
                        .GroupBy(b => b.ReceiverName)
                        .ToList();
                parcelToLDataGrid2.DataContext = GroupingData;
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            // parcelToLDataGrid.DataContext = bl.ParcelList();

        }

        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dateButton_Click(object sender, RoutedEventArgs e)
        {
            //dateCalender.Visibility = Visibility.Visible;
        }
    }
}

