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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.BL bl;
        enum op { Add, Update };
        op option;
        IBL.BO.DroneToL currentDroneToL;
        public DroneWindow(IBL.BL _bl)//ctor for add
        {
            InitializeComponent();
            bl = _bl;

            weightComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));


            currentDroneToL = new IBL.BO.DroneToL();

            option = op.Add;
            this.Title = "Add Drone";
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            ChargingButton.Visibility = Visibility.Collapsed;
            DelieveryButton.Visibility = Visibility.Collapsed;

            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Collapsed;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Collapsed;
        }
        public DroneWindow(IBL.BL _bl, IBL.BO.DroneToL _d)//ctor for update
        {
            InitializeComponent();
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatus));

            currentDroneToL = _d;
            gridOneDrone.DataContext = currentDroneToL;

            option = op.Update;
            this.Title = "Update Drone";
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";

            ChargingButton.Visibility = DelieveryButton.Visibility = Visibility.Visible;
            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Visible;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Visible;

            idTextBox.IsEnabled = weightComboBox.IsEnabled = statusComboBox.IsEnabled = false;

            weightComboBox.SelectedItem = currentDroneToL.Weight;
            statusComboBox.SelectedItem = currentDroneToL.Status;
        }

        private void OpButton_Click(object sender, RoutedEventArgs e)
        {
            currentDroneToL = gridOneDrone.DataContext as IBL.BO.DroneToL;
            try
            {
                if (option == op.Add)
                {
                    IBL.BO.Drone d = new IBL.BO.Drone();
                    d.Id = int.Parse(idTextBox.Text);//currentDroneToL.Id;
                    d.Model = modelTextBox.Text;//currentDroneToL.Model;
                    d.Weight = (IBL.BO.WeightCategories)weightComboBox.SelectedItem;//currentDroneToL.Weight;
                    bl.AddDrone(d);
                    MessageBox.Show("The drone is added successfully!");
                    this.Close();
                }
                else
                {
                    //currentDroneToL = gridOneDrone.DataContext as IBL.BO.DroneToL;
                    bl.UpdateDroneName(currentDroneToL.Id, currentDroneToL.Model);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void CancelOrCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChargingButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDroneToL.Status == IBL.BO.DroneStatus.Avaliable)
            {
                bl.UpdateDroneToCharge(currentDroneToL.Id);
                statusComboBox.SelectedItem = IBL.BO.DroneStatus.Maintenance;
            }
            else if (currentDroneToL.Status == IBL.BO.DroneStatus.Maintenance)
            {
                bl.UpdateDisChargeDrone(currentDroneToL.Id);
                statusComboBox.SelectedItem = IBL.BO.DroneStatus.Avaliable;
                batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
            }
        }

        private void DelieveryButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentDroneToL.Status == IBL.BO.DroneStatus.Avaliable)
            {
                bl.UpdateParcelToDrone(currentDroneToL.Id);
                if (currentDroneToL.ParcelId != -1)
                {
                    parcelIdTextBox.Text = Convert.ToString(currentDroneToL.ParcelId);
                    statusComboBox.SelectedItem = IBL.BO.DroneStatus.Delivery;
                    batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                }
            }
            else
            {
                IBL.BO.Parcel p = bl.GetParcel(currentDroneToL.ParcelId);
                if (currentDroneToL.Status == IBL.BO.DroneStatus.Delivery && p.PickedUp == null)
                {
                    bl.UpdateParcelCollect(currentDroneToL.Id);
                    batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                }
                else if (currentDroneToL.Status == IBL.BO.DroneStatus.Delivery && p.PickedUp != null && p.Delivered == null)
                {
                    bl.UpdateParcelProvide(currentDroneToL.Id);
                    batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                    parcelIdTextBox.Text = "-1";
                    statusComboBox.SelectedItem = IBL.BO.DroneStatus.Avaliable;
                }
            }

        }
    }
}
