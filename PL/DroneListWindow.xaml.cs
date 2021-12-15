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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.BL bl;
        public DroneListWindow(IBL.BL _bl)
        {
            InitializeComponent();
            bl = _bl;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));

            dronesDataGrid.DataContext = bl.DroneList();
            dronesDataGrid.IsReadOnly = true;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedIndex == -1)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
            else
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex == -1)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            else
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem && d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bl).ShowDialog();
        }

        private void ActivatedWindow(object sender, EventArgs e)
        {
            //dronesDataGrid.DataContext = bl.DroneList();

            if (WeightSelector.SelectedIndex == -1 && StatusSelector.SelectedIndex == -1)
                dronesDataGrid.DataContext = bl.DroneList();
            else if (WeightSelector.SelectedIndex == -1)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
            else if (StatusSelector.SelectedIndex == -1)
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            else
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem && d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
        }

        private void DoubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            IBL.BO.DroneToL curDroneToL = dronesDataGrid.SelectedItem as IBL.BO.DroneToL;
            if (curDroneToL != null)
                new DroneWindow(bl, curDroneToL).ShowDialog();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
