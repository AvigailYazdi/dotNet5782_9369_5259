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
    /// Interaction logic for CustomerPage.xaml
    /// </summary>
    public partial class CustomerPage : Page
    {
        IBL bl;
        bool flag = true;
        enum op { Add, Update };
        op option;
        BO.Customer currentCustomer;
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        ObservableCollection<BO.CustomerToL> oc;
        public CustomerPage(IBL _bl, ObservableCollection<BO.CustomerToL> _oc )// ctor for add
        {
            InitializeComponent();
            bl = _bl;
            oc = _oc;
            currentCustomer = new BO.Customer();
            option = op.Add;
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            OpButton.IsEnabled = false;
            sentParcelsDataGrid.Visibility = suppliedParcelsDataGrid.Visibility = Visibility.Collapsed;
        }
        public CustomerPage(IBL _bl, BO.Customer _c)// ctor for update
        {
            InitializeComponent();
            bl = _bl;
            currentCustomer = _c;
            gridOneCustomer.DataContext = currentCustomer;
            option = op.Update;
            OpButton.Content = "Update";
            CancelOrCloseButton.Content = "Close";
            idTextBox.IsEnabled = longitudeTextBox.IsEnabled = latitudeTextBox.IsEnabled = false;
            sentParcelsDataGrid.Visibility = Visibility.Visible;
            sentParcelsDataGrid.DataContext = currentCustomer.SendParcel;
            sentParcelsDataGrid.IsReadOnly = true;
            suppliedParcelsDataGrid.Visibility = Visibility.Visible;
            suppliedParcelsDataGrid.DataContext = currentCustomer.GetParcel;
            suppliedParcelsDataGrid.IsReadOnly = true;
        }

        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentCustomer = gridOneCustomer.DataContext as BO.Customer;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.Customer c = new BO.Customer();
                    c.Id = int.Parse(idTextBox.Text);
                    c.Name = nameTextBox.Text;
                    c.PhoneNum = phoneNumTextBox.Text;
                    c.Place = new BO.Location();
                    c.Place.Longitude = double.Parse(longitudeTextBox.Text);
                    c.Place.Latitude = double.Parse(latitudeTextBox.Text);
                    bl.AddCustomer(c);
                    oc.Add(bl.getCustomerToL(c.Id));
                    MessageBox.Show("The customer is added successfully!", "Add", b, i);
                    this.NavigationService.GoBack();
                }
                else // The button is update
                {
                    nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    phoneNumTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                    bl.UpdateCustomer(int.Parse(idTextBox.Text), nameTextBox.Text, phoneNumTextBox.Text);
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
                if (idTextBox.Text != "" && nameTextBox.Text != "" && phoneNumTextBox.Text != "" && flag && longitudeTextBox.Text != "" && latitudeTextBox.Text != "")
                    OpButton.IsEnabled = true;
                else
                    OpButton.IsEnabled = false;
            }
            else
            {
                if (nameTextBox.Text != "" && phoneNumTextBox.Text != "")
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

        private void suppliedParcelsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelAtC cur = suppliedParcelsDataGrid.SelectedItem as BO.ParcelAtC;
            if (cur != null)
            {
                BO.Parcel p = bl.GetParcel(cur.Id);
                this.NavigationService.Navigate(new ParcelPage(bl, p));
            }
        }

        private void sentParcelsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ParcelAtC cur = sentParcelsDataGrid.SelectedItem as BO.ParcelAtC;
            if (cur != null)
            {
                BO.Parcel p = bl.GetParcel(cur.Id);
                this.NavigationService.Navigate(new ParcelPage(bl, p));
            }
        }

        private void CustomerWindow_Activated(object sender, EventArgs e)
        {
            if (option == op.Update)
            {
                currentCustomer = bl.GetCustomer(currentCustomer.Id);
                sentParcelsDataGrid.DataContext = currentCustomer.SendParcel;
                suppliedParcelsDataGrid.DataContext = currentCustomer.GetParcel;
            }
        }
    }
}

