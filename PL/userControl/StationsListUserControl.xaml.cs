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
    /// Interaction logic for StationsListUserControl.xaml
    /// </summary>
    public partial class StationsListUserControl : UserControl
    {
        IBL bl;

        public StationsListUserControl(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            for (int i = 1; i < 41; i++)
            {
               numSlotsSelector.Items.Add(i);
            }
            numSlotsSelector.Items.Add("Clear");
            stationToLDataGrid.DataContext = bl.StationList();
            stationToLDataGrid.IsReadOnly = true;
        }

        private void avaliableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (avaliableCheckBox.IsChecked == false && (numSlotsSelector.SelectedIndex == -1 || numSlotsSelector.SelectedIndex == 40))
                stationToLDataGrid.DataContext = bl.StationList();
            else if (avaliableCheckBox.IsChecked == true && (numSlotsSelector.SelectedIndex==-1 || numSlotsSelector.SelectedIndex == 40))
                stationToLDataGrid.DataContext = bl.AvaliableStationList();
            else
                stationToLDataGrid.DataContext = bl.GetStationToLByPredicate(s => s.AvaliableSlots == numSlotsSelector.SelectedIndex + 1);
        }

        private void numSlotsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (avaliableCheckBox.IsChecked == false && (numSlotsSelector.SelectedIndex == -1 || numSlotsSelector.SelectedIndex == 40))
                stationToLDataGrid.DataContext = bl.StationList();
            else if (avaliableCheckBox.IsChecked == true && (numSlotsSelector.SelectedIndex == -1 || numSlotsSelector.SelectedIndex == 40))
                stationToLDataGrid.DataContext = bl.AvaliableStationList();
            else
                stationToLDataGrid.DataContext = bl.GetStationToLByPredicate(s => s.AvaliableSlots == numSlotsSelector.SelectedIndex + 1);
        }

        private void addStationButton_Click(object sender, RoutedEventArgs e)
        {
            StationUserControl myUser = new StationUserControl(bl);
            StationListGrid.Children.Clear();
            StationListGrid.Children.Add(myUser);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            StationListGrid.Visibility = Visibility.Hidden;
            
        }
        private void doubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            BO.StationToL curStationToL = stationToLDataGrid.SelectedItem as BO.StationToL;
            if (curStationToL != null)
            {
                StationUserControl myUser = new StationUserControl(bl, curStationToL);
                StationListGrid.Children.Clear();
                StationListGrid.Children.Add(myUser);
            }
        }
    }
}
