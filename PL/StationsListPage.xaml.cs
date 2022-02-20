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
    /// Interaction logic for StationsListPage.xaml
    /// </summary>
    public partial class StationsListPage : Page
    {
        IBL bl;
        ObservableCollection<BO.StationToL> os;
        public StationsListPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            os= new ObservableCollection<BO.StationToL>(bl.StationList());
            stationToLDataGrid.DataContext = os;
            stationToLDataGrid.IsReadOnly = true;
        }
        private void addStationButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new StationPage(bl,os));
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void doubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            BO.StationToL curStationToL = stationToLDataGrid.SelectedItem as BO.StationToL;
            if (curStationToL != null)
            {
                BO.BaseStation s = bl.GetStation(curStationToL.Id);
                this.NavigationService.Navigate(new StationPage(bl, s));
            }
        }

        private void GroupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupCheckBox.IsChecked == true)
            {
                List<IGrouping<int, BO.StationToL>> GroupingData = bl.StationList()
                    .GroupBy(b => b.AvaliableSlots)
                    .ToList();
                stationToLDataGrid2.DataContext = GroupingData;
                stationToLDataGrid2.Visibility = Visibility.Visible;
                stationToLDataGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                stationToLDataGrid.Visibility = Visibility.Visible;
                stationToLDataGrid2.Visibility = Visibility.Collapsed;
            }
        }

        private void avaliableCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (avaliableCheckBox.IsChecked == false)
            {
                os = new ObservableCollection<BO.StationToL>(bl.StationList());
                stationToLDataGrid.DataContext = os;
                group(bl.StationList());
            }
            else
            {
                os = new ObservableCollection<BO.StationToL>(bl.AvaliableStationList());
                stationToLDataGrid.DataContext = os;
                group(bl.AvaliableStationList());
            }
        }
        private void group(IEnumerable<BO.StationToL> stations)
        {
            List<IGrouping<int, BO.StationToL>> GroupingData = stations
                    .GroupBy(b => b.AvaliableSlots)
                    .ToList();
            stationToLDataGrid2.DataContext = GroupingData;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            os = new ObservableCollection<BO.StationToL>(bl.StationList());
            stationToLDataGrid.DataContext = os;
        }
    }
}
