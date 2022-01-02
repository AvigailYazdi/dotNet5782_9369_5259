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
    /// Interaction logic for CustomerUserControl.xaml
    /// </summary>
    public partial class CustomerUserControl : UserControl
    {
        IBL bl;
        bool flag = true;
        enum op { Add, Update }; 
        op option; 
        BO.CustomerToL currentCustomerToL;  
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        public CustomerUserControl(IBL _bl)// ctor for add
        {
            InitializeComponent();
            bl = _bl;
            currentCustomerToL = new BO.CustomerToL();
            option = op.Add;
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            numArrivedTextBox.Visibility = numGotTextBox.Visibility = numOnWayTextBox.Visibility = numSendTextBox.Visibility = Visibility.Collapsed;
            numSentLabel.Visibility = numOnWayLabel.Visibility = numGotLabel.Visibility = numArrivedLabel.Visibility = Visibility.Collapsed;
            OpButton.IsEnabled = false;
        }
        public CustomerUserControl(IBL _bl, BO.CustomerToL _c)// ctor for update
        {
            InitializeComponent();
            bl = _bl;
            currentCustomerToL = _c;
            gridOneCustomer.DataContext = currentCustomerToL;
            option = op.Update;
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";
            idTextBox.IsEnabled = numArrivedTextBox.IsEnabled = numGotTextBox.IsEnabled = numOnWayTextBox.IsEnabled = numSendTextBox.IsEnabled = false;
            longitudeLabel.Visibility = longitudeTextBox.Visibility = latitudeLabel.Visibility = latitudeTextBox.Visibility = Visibility.Collapsed;
        }

        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentCustomerToL = gridOneCustomer.DataContext as BO.CustomerToL;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.Customer c = new BO.Customer();
                    c.Id = int.Parse(idTextBox.Text);
                    c.Name = nameTextBox.Text;
                    c.PhoneNum = phoneNumTextBox.Text;
                    c.Place.Longitude = Convert.ToDouble(longitudeTextBox.Text);
                    c.Place.Latitude = Convert.ToDouble(latitudeTextBox.Text);
                    bl.AddCustomer(c);
                    MessageBox.Show("The customer is added successfully!", "Add", b, i);
                    this.Visibility = Visibility.Hidden;
                }
                else // The button is update
                {
                    nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    phoneNumTextBox.GetBindingExpression(ComboBox.TextProperty).UpdateSource();
                    MessageBox.Show("The customer is updated successfully!", "Update", b, i);
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
                if (idTextBox.Text != "" && nameTextBox.Text != "" && phoneNumTextBox.Text!="" && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            else
            {
                if (nameTextBox.Text != "" && phoneNumTextBox.Text!="")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (idTextBox.Text.Length != 9)
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
                    if (idTextBox.Text != "" && nameTextBox.Text != "" && phoneNumTextBox.Text != "" && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                        OpButton.IsEnabled = true;
                    else
                        OpButton.IsEnabled = false;
                }
            }
            else
            {
                if (nameTextBox.Text != "" && phoneNumTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
        }
    }
}
