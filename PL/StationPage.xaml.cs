using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for StationPage.xaml
    /// </summary>
    public partial class StationPage : Page
    {
        IBL bl;
        bool flagId = true;
        bool flagSlots = true;
        enum op { Add, Update }; // Enum of the options
        op option; // The selected option- add/ update
        BO.BaseStation currentStation; // The selected station
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        ObservableCollection<BO.StationToL> os;
        public StationPage(IBL _bl, ObservableCollection<BO.StationToL> _os)
        {
            InitializeComponent();
            bl = _bl;
            os = _os;
            currentStation = new BO.BaseStation();
            option = op.Add;
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            dronesDataGrid.Visibility = Visibility.Collapsed;
            OpButton.IsEnabled = false;
        }

        public StationPage(IBL _bl, BO.BaseStation _s)
        {
            InitializeComponent();
            bl = _bl;
            currentStation = _s;
            gridOneStation.DataContext = currentStation;
            dronesDataGrid.Visibility = Visibility.Visible;
            IEnumerable<BO.DroneToL> drones = from item in currentStation.DroneSlots
                                              select bl.GetDroneToL(item.Id);
            dronesDataGrid.DataContext = drones;
            dronesDataGrid.IsReadOnly = true;
            option = op.Update;
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";
            idTextBox.IsEnabled = longitudeTextBox.IsEnabled = latitudeTextBox.IsEnabled = false;
        }
        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentStation = gridOneStation.DataContext as BO.BaseStation;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.BaseStation bs = new BO.BaseStation();
                    bs.Id = int.Parse(idTextBox.Text);
                    bs.Name = nameTextBox.Text;
                    bs.AvaliableSlots = int.Parse(avaliableSlotsTextBox.Text);
                    bs.Place = new BO.Location();
                    bs.Place.Longitude = double.Parse(longitudeTextBox.Text);
                    bs.Place.Latitude = double.Parse(latitudeTextBox.Text);
                    bl.AddStation(bs);
                    os.Add(bl.GetStationToL(bs.Id));
                    MessageBox.Show("The station is added successfully!", "Add", this.b, i);
                    this.NavigationService.GoBack();
                }
                else // The button is update
                {
                    nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    avaliableSlotsTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    bl.UpdateStation(currentStation.Id, currentStation.Name, currentStation.AvaliableSlots);
                    MessageBox.Show("The station is updated successfully!", "Update", b, i);
                }
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }
        private void cancelOrCloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (idTextBox.Text != "" && nameTextBox.Text != "" && flagSlots && flagId && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            else
            {
                if (nameTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (idTextBox.Text.Length != 5)
                {
                    idTextBox.BorderBrush = Brushes.Red;
                    OpButton.IsEnabled = false;
                    flagId = false;
                    IntegrityIdLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    IntegrityIdLabel.Visibility = Visibility.Collapsed;
                    idTextBox.BorderBrush = nameTextBox.BorderBrush;
                    flagId = true;
                    if (idTextBox.Text != "" && nameTextBox.Text != "" && flagSlots && flagId && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                        OpButton.IsEnabled = true;
                    else
                        OpButton.IsEnabled = false;
                }
            }
            else
            {
                if (nameTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        private void dronesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.DroneToL cur = dronesDataGrid.SelectedItem as BO.DroneToL;
            if (cur != null)
                this.NavigationService.Navigate(new DronePage(bl, cur));
        }

        private void avaliableSlotsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //if (option == op.Add)
            //{
            if (avaliableSlotsTextBox.Text == "" || int.Parse(avaliableSlotsTextBox.Text) <= 0 || int.Parse(avaliableSlotsTextBox.Text) >= 41)
            {
                avaliableSlotsTextBox.BorderBrush = Brushes.Red;
                OpButton.IsEnabled = false;
                flagSlots = false;
                IntegritySlotsLabel.Visibility = Visibility.Visible;
            }
            else
            {
                IntegritySlotsLabel.Visibility = Visibility.Collapsed;
                avaliableSlotsTextBox.BorderBrush = nameTextBox.BorderBrush;
                flagSlots = true;
                if (idTextBox.Text != "" && nameTextBox.Text != "" && flagSlots && flagId && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            //}
            //else
            //{
            //    if (nameTextBox.Text != "")
            //        OpButton.IsEnabled = true;
            //    else
            //        OpButton.IsEnabled = false;
            //}
        }

        private void StationWindow_Activated(object sender, EventArgs e)
        {
            if (option == op.Update)
            {
                currentStation = bl.GetStation(currentStation.Id);
                IEnumerable<BO.DroneToL> drones = from item in currentStation.DroneSlots
                                                  select bl.GetDroneToL(item.Id);
                dronesDataGrid.DataContext = drones;
            }
        }
    }
}
