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
    /// Interaction logic for DronesListPage.xaml
    /// </summary>
    public partial class DronesListPage : Page
    {
        IBL bl;
        enum Weight { Light, Medium, Heavy, Clear };
        enum Status { Avaliable, Maintenance, Delivery, Clear }
        ObservableCollection<BO.DroneToL> od;
        /// <summary>
        /// ctor 
        /// </summary>
        /// <param name="_bl"> An accses to bl functions</param>
        public DronesListPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(Status));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(Weight));
            od= new ObservableCollection<BO.DroneToL>(bl.DroneList());
            dronesDataGrid.DataContext = od;
            dronesDataGrid.IsReadOnly = true;
        }
        /// <summary>
        /// A function that filters the view by selectors
        /// </summary>
        /// <param name="sender"> The sent object </param>
        /// <param name="e"> Selection changed event args</param>
        private void selector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
            {
                group(bl.DroneList());
                od= new ObservableCollection<BO.DroneToL>(bl.DroneList());
                dronesDataGrid.DataContext = od;
            }
            else if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && StatusSelector.SelectedIndex != 3)
            {
                group(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem));
                od= new ObservableCollection<BO.DroneToL>(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem));
                dronesDataGrid.DataContext = od;
            }
            else if (WeightSelector.SelectedIndex != 3 && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
            {
                group(bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                od= new ObservableCollection<BO.DroneToL>(bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                dronesDataGrid.DataContext = od;
            }
            else
            {
                group(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                od= new ObservableCollection<BO.DroneToL>(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                dronesDataGrid.DataContext = od;
            }
        }
        private void group(IEnumerable<BO.DroneToL> drones)
        {
            List<IGrouping<BO.DroneStatus, BO.DroneToL>> GroupingData = drones
                    .GroupBy(d => d.Status)
                    .ToList();
            dronesDataGrid2.DataContext = GroupingData;
        }
        /// <summary>
        /// A function that opens the drone window for add
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Routed event args</param>
        private void addDroneButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DronePage(bl,od));
        }
        /// <summary>
        /// A function that keeps the window updated
        /// </summary>
        /// <param name="sender"> The sent object </param>
        /// <param name="e"> Event args</param>
        private void activatedWindow(object sender, EventArgs e)
        {
            if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
            {
                group(bl.DroneList());
                dronesDataGrid.DataContext = bl.DroneList();
            }
            else if ((WeightSelector.SelectedIndex == -1 || WeightSelector.SelectedIndex == 3) && StatusSelector.SelectedIndex != 3)
            {
                group(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem));
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem);
            }
            else if (WeightSelector.SelectedIndex != 3 && (StatusSelector.SelectedIndex == 3 || StatusSelector.SelectedIndex == -1))
            {
                group(bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            }
            else
            {
                group(bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem));
                dronesDataGrid.DataContext = bl.GetDronesByPerdicate(d => d.Status == (BO.DroneStatus)StatusSelector.SelectedItem && d.Weight == (BO.WeightCategories)WeightSelector.SelectedItem);
            }
        }
        /// <summary>
        /// A function that opens the drone window for update the selected drone
        /// </summary>
        /// <param name="sender">The sent object </param>
        /// <param name="e"> Mouse button event args</param>
        private void doubleClickDataGrid(object sender, MouseButtonEventArgs e)
        {
            BO.DroneToL curDroneToL = dronesDataGrid.SelectedItem as BO.DroneToL;
            if (curDroneToL != null)
                this.NavigationService.Navigate(new DronePage(bl, curDroneToL));
                //new DroneWindow(bl, curDroneToL).ShowDialog();
        }
        /// <summary>
        /// A function that closes the window
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Routed event args</param>
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void showParcelButton_Click(object sender, RoutedEventArgs e)
        {
            BO.DroneToL curDroneToL = dronesDataGrid.SelectedItem as BO.DroneToL;
            if (curDroneToL != null && curDroneToL.ParcelId != -1)
                this.NavigationService.Navigate(new ParcelPage(bl, bl.GetParcel(curDroneToL.ParcelId)));
        }

        private void GroupCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (GroupCheckBox.IsChecked == true)
            {
                List<IGrouping<BO.DroneStatus, BO.DroneToL>> GroupingData = bl.DroneList()
                    .GroupBy(d => d.Status)
                    .ToList();
                dronesDataGrid2.DataContext = GroupingData;
                dronesDataGrid2.Visibility = Visibility.Visible;
                dronesDataGrid.Visibility = Visibility.Collapsed;
                StatusSelector.IsEnabled = false;
            }
            else
            {
                dronesDataGrid.Visibility = Visibility.Visible;
                dronesDataGrid2.Visibility = Visibility.Collapsed;
                StatusSelector.IsEnabled = true;
            }
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            od = new ObservableCollection<BO.DroneToL>(bl.DroneList());
            dronesDataGrid.DataContext = od;
        }
    }
}

