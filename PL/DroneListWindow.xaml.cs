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
        IBL.BL bL;
        public DroneListWindow(IBL.BL bl)
        {
            InitializeComponent();
            bL = bl;
            DronesListView.ItemsSource = bl.DroneList();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (WeightSelector.SelectedIndex == -1)
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
            else
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusSelector.SelectedIndex == -1)
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            else
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem && d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
        }

        private void AddDroneButton_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(bL).Show();
        }

        private void ActivatedWindow(object sender, EventArgs e)
        {
            if (WeightSelector.SelectedIndex == -1 && StatusSelector.SelectedIndex == -1)
                DronesListView.ItemsSource = bL.DroneList();
            else if(WeightSelector.SelectedIndex == -1)
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
            else if(StatusSelector.SelectedIndex == -1)
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
            else
                DronesListView.ItemsSource = bL.GetDronesByPerdicate(d => d.Weight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem && d.Status == (IBL.BO.DroneStatus)StatusSelector.SelectedItem);
        }

        private void DoubleClick(object sender, MouseButtonEventArgs e)
        {
           // new DroneWindow(bL,DronesListView.);
        }
    }
}
