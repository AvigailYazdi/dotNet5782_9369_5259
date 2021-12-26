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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL bl;
        enum Weight { Light, Medium, Heavy, Clear};
        enum Status { Avaliable, Maintenance, Delivery, Clear }
        public DroneListWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            
            StatusSelector.ItemsSource = Enum.GetValues(typeof(Status));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(Weight));

            dronesDataGrid.DataContext = bl.DroneList();
            dronesDataGrid.IsReadOnly = true;
        }

        private void Selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((WeightSelector.SelectedIndex == -1||WeightSelector.SelectedIndex == 3) && StatusSelector.SelectedIndex != 3)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem);
            else if((WeightSelector.SelectedIndex == -1||WeightSelector.SelectedIndex == 3) && (StatusSelector.SelectedIndex == 3|| StatusSelector.SelectedIndex == -1))
                dronesDataGrid.DataContext = bl.DroneList();
            else if(WeightSelector.SelectedIndex != 3 && (StatusSelector.SelectedIndex == 3|| StatusSelector.SelectedIndex == -1))
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            else
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).ShowDialog();
        }

        private void ActivatedWindow(object sender, EventArgs e)
        {

            if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
                dronesDataGrid.DataContext = bl.DroneList();
            else if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && StatusSelector.SelectedIndex != 3)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem);
            else if (WeightSelector.SelectedIndex != 3 && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            else
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
        }

        private void DoubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            BO.DroneToL curDroneToL = dronesDataGrid.SelectedItem as BO.DroneToL;
            if (curDroneToL != null)
                new DroneWindow(bl, curDroneToL).ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
