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
        enum NumSlots {}
        IBL bl;
        public StationsListUserControl(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            for (int i = 1; i < 101; i++)
            {
               numSlotsSelector.Items.Add(i);
            }
        }

        private void avaliableCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (avaliableCheckBox.IsChecked == true)
            {
                stationToLDataGrid.DataContext = bl.AvaliableStationList();
            }
        }

        private void numSlotsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (numSlotsSelector.SelectedIndex >= 0 || numSlotsSelector.SelectedIndex < 100) 
            {
                stationToLDataGrid.DataContext = bl.GetStationToLByPredicate(s => s.AvaliableSlots == numSlotsSelector.SelectedIndex + 1);   
            }
        }
    }
}
