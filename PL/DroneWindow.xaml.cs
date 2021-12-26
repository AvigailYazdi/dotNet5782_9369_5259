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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL bl;
        enum op { Add, Update };
        op option;
        BO.DroneToL currentDroneToL;
        bool flag = true;
        MessageBoxButton b = MessageBoxButton.OK;
        MessageBoxImage i = MessageBoxImage.Information;
        public DroneWindow(IBL _bl)//ctor for add
        {
            InitializeComponent();
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatus));
            StationIdCBox.ItemsSource = bl.AvaliableStationList();
            StationIdCBox.DisplayMemberPath = "Name";
            StationIdCBox.SelectedValuePath = "Id";

            currentDroneToL = new BO.DroneToL();

            option = op.Add;
            this.Title = "Add Drone";
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            StationIdCBox.Visibility = StationIdLabel.Visibility = Visibility.Visible;
            ChargingButton.Visibility = DelieveryButton.Visibility=Visibility.Collapsed;
            batteryTextBox.Visibility = BatteryLabel.Visibility = Visibility.Collapsed;
            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Collapsed;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Collapsed;
            OpButton.IsEnabled = false;

        }
        public DroneWindow(IBL _bl, BO.DroneToL _d)//ctor for update
        {
            InitializeComponent();
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            statusComboBox.ItemsSource = Enum.GetValues(typeof(BO.DroneStatus));
            currentDroneToL = _d;
            gridOneDrone.DataContext = currentDroneToL;

            option = op.Update;
            this.Title = "Update Drone";
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";

            StationIdCBox.Visibility = StationIdLabel.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = DelieveryButton.Visibility = Visibility.Visible;
            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Visible;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Visible;

            idTextBox.IsEnabled = weightComboBox.IsEnabled = statusComboBox.IsEnabled = false;

            weightComboBox.SelectedItem = currentDroneToL.Weight;
            statusComboBox.SelectedItem = currentDroneToL.Status;
        }

        private void OpButton_Click(object sender, RoutedEventArgs e)
        {
            currentDroneToL = gridOneDrone.DataContext as BO.DroneToL;
            try
            {
                if (option == op.Add)
                {
                    BO.Drone d = new BO.Drone();
                    d.Id = int.Parse(idTextBox.Text);//currentDroneToL.Id;
                    d.Model = modelTextBox.Text;//currentDroneToL.Model;
                    d.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;//currentDroneToL.Weight;
                    bl.AddDrone(d, int.Parse(StationIdCBox.SelectedValue.ToString()));
                    MessageBox.Show("The drone is added successfully!", "Add", b, i);
                    this.Close();
                }
                else 
                {
                    bl.UpdateDroneName(currentDroneToL.Id, currentDroneToL.Model);
                    MessageBox.Show("The drone Model is updated successfully!", "Update", b, i);
                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }

        private void CancelOrCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ChargingButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentDroneToL.Status == BO.DroneStatus.Avaliable)
                {
                    bl.UpdateDroneToCharge(currentDroneToL.Id);
                    statusComboBox.SelectedItem = BO.DroneStatus.Maintenance;
                    MessageBox.Show("The drone is in charge", "Charging", b, i);
                }
                else if (currentDroneToL.Status == BO.DroneStatus.Maintenance)
                {
                    bl.UpdateDisChargeDrone(currentDroneToL.Id);
                    statusComboBox.SelectedItem = BO.DroneStatus.Avaliable;
                    batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                    MessageBox.Show("The drone discharged", "Discharging", b, i);

                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }

        private void DelieveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentDroneToL.Status == BO.DroneStatus.Avaliable)
                {
                    bl.UpdateParcelToDrone(currentDroneToL.Id);
                    if (currentDroneToL.ParcelId != -1)
                    {
                        parcelIdTextBox.Text = Convert.ToString(currentDroneToL.ParcelId);
                        statusComboBox.SelectedItem = BO.DroneStatus.Delivery;
                        batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                        MessageBox.Show("The drone is connected to a parcel", "Connection", b, i);
                    }
                    else
                    {
                        i = MessageBoxImage.Error;
                        MessageBox.Show("The connection failed", "ERROR", b, i);
                        i = MessageBoxImage.Information;
                    }
                }
                else if (currentDroneToL.Status == BO.DroneStatus.Delivery)
                {
                    BO.Parcel p = bl.GetParcel(currentDroneToL.ParcelId);
                    if (p.PickedUp == null)
                    {
                        bl.UpdateParcelCollect(currentDroneToL.Id);
                        batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                        MessageBox.Show("The parcel was picked up", "Picking up", b, i);
                    }
                    else if (p.PickedUp != null && p.Delivered == null)
                    {
                        bl.UpdateParcelProvide(currentDroneToL.Id);
                        batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                        parcelIdTextBox.Text = "-1";
                        statusComboBox.SelectedItem = BO.DroneStatus.Avaliable;
                        MessageBox.Show("The parcel was provided", "Providing", b, i);
                    }
                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }
        private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
                return;

            //allow list of system keys (add other key here if you want to allow)
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox

            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (idTextBox.Text.Length < 3 || idTextBox.Text.Length > 4)
                {
                    idTextBox.BorderBrush = Brushes.Red;
                    OpButton.IsEnabled = false;
                    flag = false;
                    IntegrityIdLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    IntegrityIdLabel.Visibility = Visibility.Collapsed;
                    idTextBox.BorderBrush = modelTextBox.BorderBrush;
                    flag = true;
                    if (idTextBox.Text != "" && modelTextBox.Text != "" && weightComboBox.SelectedIndex != -1 && StationIdCBox.SelectedIndex != -1)
                        OpButton.IsEnabled = true;
                    else
                        OpButton.IsEnabled = false;
                }
            }
            else
            {
                if (modelTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        private void modelTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (option == op.Add)
            {

                if (idTextBox.Text != "" && modelTextBox.Text != "" && weightComboBox.SelectedIndex != -1 && flag && StationIdCBox.SelectedIndex != -1)
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            else
            {
                if (modelTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (idTextBox.Text != "" && modelTextBox.Text != "" && weightComboBox.SelectedIndex != -1 && flag && StationIdCBox.SelectedIndex != -1)
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            else
            {
                if (modelTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        //private void ClosingWindow(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    Button b = sender as Button;
        //    if (b == null)
        //        e.Cancel = false;
        //    else
        //        e.Cancel = false;
        //}
    }
}
