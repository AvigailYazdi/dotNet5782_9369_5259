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
    /// Interaction logic for StationUserControl.xaml
    /// </summary>
    public partial class StationUserControl : UserControl
    {
        IBL bl;
        bool flag = true;
        enum op { Add, Update }; // Enum of the options 
        op option; // The selected option- add/ update
        BO.StationToL currentStationToL; // The selected station 
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        public StationUserControl(IBL _bl)// ctor for add
        {
            InitializeComponent();
            bl = _bl;
            for (int i = 1; i < 41; i++)
            {
                avaliableSlotsComboBox.Items.Add(i);
            }
            currentStationToL = new BO.StationToL();
            option = op.Add;
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";

            OpButton.IsEnabled = false;
        }
        public StationUserControl(IBL _bl, BO.StationToL _s)
        {
            InitializeComponent();
            bl = _bl;
            for (int i = 1; i < 41; i++)
            {
                avaliableSlotsComboBox.Items.Add(i);
            }
            currentStationToL = _s;
            gridOneStation.DataContext = currentStationToL;
            option = op.Update;
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";
            idTextBox.IsEnabled = longitudeTextBox.IsEnabled = latitudeTextBox.IsEnabled = false;
            avaliableSlotsComboBox.SelectedItem = currentStationToL.AvaliableSlots;
        }
        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentStationToL = gridOneStation.DataContext as BO.StationToL;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.BaseStation bs = new BO.BaseStation();
                    bs.Id = int.Parse(idTextBox.Text);
                    bs.Name = nameTextBox.Text;
                    bs.AvaliableSlots = avaliableSlotsComboBox.SelectedIndex+1;
                    bs.Place.Longitude = Convert.ToDouble(longitudeTextBox.Text);
                    bs.Place.Latitude = Convert.ToDouble(latitudeTextBox.Text);
                    bl.AddStation(bs);
                    MessageBox.Show("The station is added successfully!", "Add", this.b, i);
                    this.Visibility=Visibility.Hidden;
                }
                else // The button is update
                {
                    nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    avaliableSlotsComboBox.GetBindingExpression(ComboBox.SelectedItemProperty).UpdateSource();
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
            this.Visibility = Visibility.Collapsed;
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
                if (idTextBox.Text != "" && nameTextBox.Text != "" && avaliableSlotsComboBox.SelectedIndex != -1 && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "") 
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
                if (idTextBox.Text.Length !=5)
                {
                    idTextBox.BorderBrush = Brushes.Red;
                    OpButton.IsEnabled = false;
                    flag = false;
                    IntegrityIdLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    IntegrityIdLabel.Visibility = Visibility.Collapsed;
                    idTextBox.BorderBrush = nameTextBox.BorderBrush;
                    flag = true;
                    if (idTextBox.Text != "" && nameTextBox.Text != "" && avaliableSlotsComboBox.SelectedIndex != -1 && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
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

        private void avaliableSlotsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (option == op.Add)
            {

                if (idTextBox.Text != "" && nameTextBox.Text != "" && avaliableSlotsComboBox.SelectedIndex != -1 && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
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
    }
}
