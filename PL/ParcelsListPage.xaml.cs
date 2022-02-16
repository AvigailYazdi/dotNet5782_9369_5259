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
        ObservableCollection<BO.ParcelToL> op;
        enum parcelStatus { Created, Connected, PickedUp, Provided, Clear };

        public ParcelsListPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            op = new ObservableCollection<BO.ParcelToL>(bl.ParcelList());
            parcelToLDataGrid.DataContext = op; 
            parcelToLDataGrid.IsReadOnly = true;
            dateButton.Content = "choose dates";
            StatusSelector.ItemsSource = Enum.GetValues(typeof(parcelStatus));
        }

        private void addParcelButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ParcelPage(bl,op));
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
                StatusSelector.IsEnabled = false;
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
                StatusSelector.IsEnabled = true;
                GroupReceiverCheckBox.IsEnabled = true;
                parcelToLDataGrid.Visibility = Visibility.Visible;
                parcelToLDataGrid2.Visibility = Visibility.Collapsed;
            }
        }

        private void GroupReceiverCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupReceiverCheckBox.IsChecked == true)
            {
                StatusSelector.IsEnabled = false;
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
                StatusSelector.IsEnabled = true;
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

        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex == 4)
            {
                GroupReceiverCheckBox.IsEnabled = GroupSenderCheckBox.IsEnabled = true;
                op = new ObservableCollection<BO.ParcelToL>(bl.ParcelList());
                parcelToLDataGrid.DataContext = op;
            }
            else
            {
                GroupReceiverCheckBox.IsEnabled = GroupSenderCheckBox.IsEnabled = false;
                op= new ObservableCollection<BO.ParcelToL>(bl.GetParcelByPredicate(p => p.Status == (BO.ParcelStatus)StatusSelector.SelectedItem));
                parcelToLDataGrid.DataContext = op;
            }
        }

        private void dateButton_Click(object sender, RoutedEventArgs e)
        {
            if (dateButton.Content == "choose dates")
            {
                dateButton.Content = "Verification";
                dateCalenderSent.Visibility = Visibility.Visible;
                dateCalenderReceived.Visibility = Visibility.Visible;
            }
            else
            {
                DateTime? dStart =dateCalenderSent.SelectedDate;
                DateTime? dEnd = dateCalenderReceived.SelectedDate;
                dateChoice(dStart, dEnd);
                dateButton.Content = "choose dates";
                dateCalenderSent.Visibility = Visibility.Hidden;
                dateCalenderReceived.Visibility = Visibility.Hidden;
            }
        }
        private void dateChoice(DateTime? dStart, DateTime? dEnd)
        {
            if (dStart != null && dEnd != null)
            {
                if (dEnd.HasValue)
                    dEnd = dEnd.Value.AddDays(1);
                if (dStart <= dEnd)
                {
                    op = new ObservableCollection<BO.ParcelToL>(bl.GetParcelByPredicate(p => bl.GetParcel(p.Id).Requested >= dStart && bl.GetParcel(p.Id).Requested <= dEnd));
                    parcelToLDataGrid.DataContext = op;
                }
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}

