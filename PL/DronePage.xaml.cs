using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for DronePage.xaml
    /// </summary>
    public partial class DronePage : Page
    {
        IBL bl;
        BackgroundWorker bwDrone;
        enum op { Add, Update }; // Enum of the options 
        op option; // The selected option- add/ update
        BO.DroneToL currentDroneToL; // The selected drone 
        bool flag = true; // A flag for id integrity
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        ObservableCollection<BO.DroneToL> od;
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="_bl"> An accses for bl functions</param>
        public DronePage(IBL _bl, ObservableCollection<BO.DroneToL> _od)
        {
            InitializeComponent();
            bl = _bl;
            od = _od;
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
            ChargingButton.Visibility = DelieveryButton.Visibility = Visibility.Collapsed;
            //batteryTextBox.Visibility = BatteryLabel.Visibility = Visibility.Collapsed;
            BatteryProgressBar.Visibility= BatteryLabel.Visibility= batteryTextBox.Visibility = Visibility.Collapsed;
            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Collapsed;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Collapsed;
            OpButton.IsEnabled = false;

        }
        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="_bl">An accses for bl functions </param>
        /// <param name="_d"> The selected drone</param>
        public DronePage(IBL _bl, BO.DroneToL _d)
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

            bwDrone = new BackgroundWorker();
            bwDrone.DoWork += BwDrone_DoWork;
            bwDrone.ProgressChanged += BwDrone_ProgressChanged;
            bwDrone.RunWorkerCompleted += BwDrone_RunWorkerCompleted;

            bwDrone.WorkerReportsProgress = true;
            bwDrone.WorkerSupportsCancellation = true;

            StationIdCBox.Visibility = StationIdLabel.Visibility = Visibility.Hidden;
            ChargingButton.Visibility = DelieveryButton.Visibility = Visibility.Visible;
            statusComboBox.Visibility = parcelIdTextBox.Visibility = Visibility.Visible;
            StatusLabel.Visibility = ParcelIdLabel.Visibility = Visibility.Visible;

            idTextBox.IsEnabled = weightComboBox.IsEnabled = statusComboBox.IsEnabled = false;

            weightComboBox.SelectedItem = currentDroneToL.Weight;
            statusComboBox.SelectedItem = currentDroneToL.Status;
        }
        private void AutomaticButton_Click(object sender, RoutedEventArgs e)
        {
            if (AutomaticButton.Content.ToString() == "Automatic")
            {
                if (bwDrone.IsBusy != true)
                {
                    this.Cursor = Cursors.Wait;
                    AutomaticButton.Content = "Release";
                    bwDrone.RunWorkerAsync();
                }
            }
            else
            {
                AutomaticButton.Content = "Automatic";
                if (bwDrone.WorkerSupportsCancellation == true)
                    bwDrone.CancelAsync();
            }
        }

        private void BwDrone_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.Arrow;
        }

        private void BwDrone_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            batteryTextBox.Text = currentDroneToL.Battery.ToString();
            BatteryProgressBar.Value = currentDroneToL.Battery;
            statusComboBox.SelectedItem = currentDroneToL.Status;
            parcelIdTextBox.Text = currentDroneToL.ParcelId.ToString();
        }

        private void BwDrone_DoWork(object sender, DoWorkEventArgs e)
        {
            while(bwDrone.CancellationPending!=true)
            {
                Thread.Sleep(1000);
                int i = currentDroneToL.Id;
                bl.NextState(i);
                currentDroneToL = bl.GetDroneToL(i);
                if (bwDrone.WorkerReportsProgress == true)
                    bwDrone.ReportProgress(1);
            }
            if (bwDrone.CancellationPending == true)
                e.Cancel = true;
        }

        /// <summary>
        /// A function that adds/ updates a drone
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e">Routed event args </param>
        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentDroneToL = gridOneDrone.DataContext as BO.DroneToL;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.Drone d = new BO.Drone();
                    d.Id = int.Parse(idTextBox.Text);//currentDroneToL.Id;
                    d.Model = modelTextBox.Text;//currentDroneToL.Model;
                    d.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;//currentDroneToL.Weight;
                    bl.AddDrone(d, int.Parse(StationIdCBox.SelectedValue.ToString()));
                    od.Add(bl.GetDroneToL(d.Id));
                    MessageBox.Show("The drone is added successfully!", "Add", b, i);
                    this.NavigationService.GoBack();
                }
                else // The button is update
                {
                    modelTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    bl.UpdateDroneName(currentDroneToL.Id, modelTextBox.Text);
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
        /// <summary>
        /// A function that closes the current window
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Routed event args</param>
        private void cancelOrCloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (bwDrone.WorkerSupportsCancellation == true)
                bwDrone.CancelAsync();
            Thread.Sleep(1000);
            this.NavigationService.GoBack();
        }
        /// <summary>
        /// A function that charge/ dischage the drone
        /// </summary>
        /// <param name="sender">The sent object</param>
        /// <param name="e">Routed event args</param>
        private void chargingButton_Click(object sender, RoutedEventArgs e)
        {
            currentDroneToL = gridOneDrone.DataContext as BO.DroneToL;
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
                    BatteryProgressBar.Value = currentDroneToL.Battery;
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
        /// <summary>
        /// A function that promotes the delivery process 
        /// </summary>
        /// <param name="sender">The sent object</param>
        /// <param name="e"> Routed event args</param>
        private void delieveryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (currentDroneToL.Status == BO.DroneStatus.Avaliable)
                {
                    bl.UpdateParcelToDrone(currentDroneToL.Id);
                    if (currentDroneToL.ParcelId != -1)//////////////
                    {
                        parcelIdTextBox.Text = Convert.ToString(currentDroneToL.ParcelId);
                        statusComboBox.SelectedItem = BO.DroneStatus.Delivery;
                        batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                        BatteryProgressBar.Value = currentDroneToL.Battery;
                        MessageBox.Show("The drone is connected to a parcel", "Connection", b, i);
                    }
                    else
                    {
                        statusComboBox.SelectedItem = BO.DroneStatus.Maintenance;
                        i = MessageBoxImage.Error;
                        MessageBox.Show("There is not enough battery, go to charging", "ERROR", b, i);
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
                        BatteryProgressBar.Value = currentDroneToL.Battery;
                        MessageBox.Show("The parcel was picked up", "Picking up", b, i);
                    }
                    else if (p.PickedUp != null && p.Delivered == null)
                    {
                        bl.UpdateParcelProvide(currentDroneToL.Id);
                        batteryTextBox.Text = Convert.ToString(currentDroneToL.Battery);
                        BatteryProgressBar.Value = currentDroneToL.Battery;
                        parcelIdTextBox.Text = "-1";
                        statusComboBox.SelectedItem = BO.DroneStatus.Avaliable;
                        MessageBox.Show("The parcel was provided", "Providing", b, i);
                    }
                }
                else
                {
                    i = MessageBoxImage.Error;
                    MessageBox.Show("The drone is in maintenance", "ERROR", b, i);
                    i = MessageBoxImage.Information;
                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }
        /// <summary>
        /// A function that ensures that the input of id contains just numbers
        /// </summary>
        /// <param name="sender">The sent object </param>
        /// <param name="e">Key event args </param>
        private void textBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
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
        /// <summary>
        /// A function that checks the id integrity
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Text changed event args</param>
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
        /// <summary>
        ///  A function that checks the model integrity
        /// </summary>
        /// <param name="sender">The sent object</param>
        /// <param name="e"> Text changed event args</param>
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
        /// <summary>
        /// A function that checks if the selection changed
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Selection changed event args</param>
        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
    }
}

