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
using System.Windows.Shapes;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationsListWindow.xaml
    /// </summary>
    public partial class StationsListWindow : Window
    {
        IBL bl;
        public StationsListWindow( IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            stationToLDataGrid.DataContext = bl.StationList();
            stationToLDataGrid.IsReadOnly = true;
        }
        private void addStationButton_Click(object sender, RoutedEventArgs e)
        {
            new StationWindow(bl).ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void doubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            BO.StationToL curStationToL = stationToLDataGrid.SelectedItem as BO.StationToL;
            if (curStationToL != null)
            {
                BO.BaseStation s = bl.GetStation(curStationToL.Id);
                new StationWindow(bl, s).ShowDialog();
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            if (avaliableCheckBox.IsChecked == false)
            {
                stationToLDataGrid.DataContext = bl.StationList();
                group(bl.StationList());           
            }
            else
            {
                stationToLDataGrid.DataContext = bl.AvaliableStationList();
                group(bl.AvaliableStationList());
            }

        }

        private void GroupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupCheckBox.IsChecked == true)
            {
                List<IGrouping<int, BO.StationToL>> GroupingData = bl.StationList()
                    .GroupBy(b =>b.AvaliableSlots)
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
                stationToLDataGrid.DataContext = bl.StationList();
                group(bl.StationList());
            }
            else
            {
                stationToLDataGrid.DataContext = bl.AvaliableStationList();
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
    }
}
