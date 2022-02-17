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
    /// Interaction logic for SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        IBL bl;
        bool flag = true;
        BO.Customer currentCustomer;
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box

        public SignUpPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            addImage.IsEnabled = false;
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
            if (idTextBox.Text != "" && nameTextBox.Text != "" && phoneNumTextBox.Text != "" && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                addImage.IsEnabled = true;
            else
                addImage.IsEnabled = false;
        }

        private void idTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (idTextBox.Text.Length != 9)
            {
                idTextBox.BorderBrush = Brushes.Red;
                addImage.IsEnabled = false;
                flag = false;
                IntegrityIdLabel.Content = "Id must have 9 digits";
                IntegrityIdLabel.Visibility = Visibility.Visible;
            }
            else if (idTextBox.Text.Length == 9 && !bl.CheckId(int.Parse(idTextBox.Text)))
            {
                idTextBox.BorderBrush = Brushes.Red;
                addImage.IsEnabled = false;
                flag = false;
                IntegrityIdLabel.Content = "Invalid Id";
                IntegrityIdLabel.Visibility = Visibility.Visible;
            }
            else
            {
                IntegrityIdLabel.Visibility = Visibility.Collapsed;
                idTextBox.BorderBrush = nameTextBox.BorderBrush;
                flag = true;
                if (idTextBox.Text != "" && nameTextBox.Text != "" && phoneNumTextBox.Text != "" && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                    addImage.IsEnabled = true;
                else
                    addImage.IsEnabled = false;
            }

        }


        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void addUser(object sender, MouseButtonEventArgs e)
        {
            currentCustomer = gridOneCustomer.DataContext as BO.Customer;
            try
            {
                BO.Customer u = new BO.Customer();
                u.Id = int.Parse(idTextBox.Text);
                u.Name = nameTextBox.Text;
                u.PhoneNum = phoneNumTextBox.Text;
                u.Place = new BO.Location();
                u.Place.Longitude = double.Parse(longitudeTextBox.Text);
                u.Place.Latitude = double.Parse(latitudeTextBox.Text);
                u.Password = passwordTextBox.Text;
                bl.AddCustomer(u);
                MessageBox.Show("The customer is added successfully!", "Add", b, i);
                this.NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                i = MessageBoxImage.Error;
                MessageBox.Show(ex.ToString(), "ERROR", b, i);
                i = MessageBoxImage.Information;
            }
        }
    }

}

