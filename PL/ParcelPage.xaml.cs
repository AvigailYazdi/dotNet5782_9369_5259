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
    /// Interaction logic for ParcelPage.xaml
    /// </summary>
    public partial class ParcelPage : Page
    {
        IBL bl;
        enum op { Add, Remove, Pick, Supply }; // Enum of the options
        op option; // The selected option- add/ update
        BO.Parcel currentParcel; // The selected station
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        ObservableCollection<BO.ParcelToL> oParcel;
        public ParcelPage(IBL _bl, ObservableCollection<BO.ParcelToL> _op)//for add
        {
            InitializeComponent();
            bl = _bl;
            oParcel = _op;
            currentParcel = new BO.Parcel();
            option = op.Add;
            OpButton.Content = "Add";
            CancelOrCloseButton.Content = "Cancel";
            idTextBox.Visibility = droneIdTextBox.Visibility = scheduledDatePicker.Visibility = requestedDatePicker.Visibility = pickedUpDatePicker.Visibility = deliveredDatePicker.Visibility = Visibility.Collapsed;
            idLabel.Visibility = droneIdLabel.Visibility = scheduledLabel.Visibility = connectedLabel.Visibility = pickedUpLabel.Visibility = deliveredLabel.Visibility = Visibility.Collapsed;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            senderComboBox.ItemsSource = bl.CustomerList();
            receiverComboBox.ItemsSource = bl.CustomerList();
            OpButton.IsEnabled = false;
        }
        public ParcelPage(IBL _bl, BO.Parcel _p)//for update
        {
            InitializeComponent();
            bl = _bl;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            senderComboBox.ItemsSource = bl.CustomerList();
            receiverComboBox.ItemsSource = bl.CustomerList();
            currentParcel = _p;
            OpButton.IsEnabled = true;
            oneParcelGrid.DataContext = currentParcel;
            int i = 0;
            foreach (var item in bl.CustomerList())
            {
                if (item.Id == _p.Sender.Id)
                    break;
                i++;
            }
            senderComboBox.SelectedIndex = i;
            i = 0;
            foreach (var item in bl.CustomerList())
            {
                if (item.Id != _p.Receiver.Id)
                    i++;
                else
                    break;
            }
            receiverComboBox.SelectedIndex = i;
            weightComboBox.SelectedItem = currentParcel.Weight;
            priorityComboBox.SelectedItem = currentParcel.Priority;
            CancelOrCloseButton.Content = "Close";
            idTextBox.IsEnabled = weightComboBox.IsEnabled = priorityComboBox.IsEnabled = receiverComboBox.IsEnabled = senderComboBox.IsEnabled = requestedDatePicker.IsEnabled = false;
            status(bl.GetParcelStatus(_p.Id));
        }

        private void status(BO.ParcelStatus _ps)
        {
            if (_ps == BO.ParcelStatus.Created)
            {
                OpButton.Content = "Remove";
                option = op.Remove;
                droneIdTextBox.Visibility = scheduledDatePicker.Visibility = pickedUpDatePicker.Visibility = deliveredDatePicker.Visibility = Visibility.Collapsed;
                droneIdLabel.Visibility = scheduledLabel.Visibility = pickedUpLabel.Visibility = deliveredLabel.Visibility = Visibility.Collapsed;

            }
            else if (_ps == BO.ParcelStatus.Connected)
            {
                OpButton.Content = "Pick Up";
                option = op.Pick;
                showReceiverButton.Visibility = showSenderButton.Visibility = showDroneButton.Visibility = Visibility.Visible;
                pickedUpDatePicker.Visibility = deliveredDatePicker.Visibility = Visibility.Collapsed;
                pickedUpLabel.Visibility = deliveredLabel.Visibility = Visibility.Collapsed;
            }
            else if (_ps == BO.ParcelStatus.PickedUp)
            {
                OpButton.Content = "Supply";
                option = op.Supply;
                showReceiverButton.Visibility = showSenderButton.Visibility = showDroneButton.Visibility = Visibility.Visible;
                deliveredDatePicker.Visibility = deliveredLabel.Visibility = Visibility.Collapsed;
            }
            else
            {
                OpButton.Visibility = Visibility.Collapsed;
            }
        }
        private void opButton_Click(object sender, RoutedEventArgs e)
        {
            currentParcel = oneParcelGrid.DataContext as BO.Parcel;
            try
            {
                if (option == op.Add)// The button is add
                {
                    BO.CustomerToL c1 = (BO.CustomerToL)senderComboBox.SelectedItem;
                    BO.CustomerToL c2 = (BO.CustomerToL)receiverComboBox.SelectedItem;
                    BO.Parcel p = new BO.Parcel();
                    p.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;
                    p.Priority = (BO.Priorities)priorityComboBox.SelectedItem;
                    p.Sender = new BO.CustomerInP { Id = c1.Id, Name = c1.Name };
                    p.Receiver = new BO.CustomerInP { Id = c2.Id, Name = c2.Name };
                    bl.AddParcel(p);
                    oParcel.Add(bl.GetParcelToL(bl.GetParcelId()-1));
                    MessageBox.Show("The parcel is added successfully!", "Add", this.b, i);
                    this.NavigationService.GoBack();
                }
                else if (option == op.Remove)  // The button is update
                {
                    bl.DeleteParcel(currentParcel.Id);
                    this.NavigationService.GoBack();
                }
                else if (option == op.Pick)
                {
                    bl.UpdateParcelCollect(currentParcel.MyDrone.Id);
                    BO.Parcel p = bl.GetParcel(currentParcel.Id);
                    pickedUpDatePicker.SelectedDate = p.PickedUp;
                    pickedUpDatePicker.Visibility = pickedUpLabel.Visibility = Visibility.Visible;
                    MessageBox.Show("The parcel was picked up", "Picking up", b, i);
                    status(BO.ParcelStatus.PickedUp);
                }
                else//provided
                {
                    bl.UpdateParcelProvide(currentParcel.MyDrone.Id);
                    BO.Parcel p = bl.GetParcel(currentParcel.Id);
                    deliveredDatePicker.SelectedDate = p.Delivered;
                    deliveredDatePicker.Visibility = deliveredLabel.Visibility = Visibility.Visible;
                    MessageBox.Show("The parcel was provided", "Providing", b, i);
                    status(BO.ParcelStatus.Provided);
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (option == op.Add)
            {
                if (weightComboBox.SelectedIndex == -1 || priorityComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == -1 || receiverComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == receiverComboBox.SelectedIndex)
                    OpButton.IsEnabled = false;
                else
                    OpButton.IsEnabled = true;
            }
        }

        private void showReceiverButton_Click(object sender, RoutedEventArgs e)
        {
            BO.CustomerToL c = (BO.CustomerToL)receiverComboBox.SelectedItem;
            this.NavigationService.Navigate(new CustomerPage(bl, bl.GetCustomer(c.Id)));
        }

        private void showSenderButton_Click(object sender, RoutedEventArgs e)
        {
            BO.CustomerToL c = (BO.CustomerToL)senderComboBox.SelectedItem;
            this.NavigationService.Navigate(new CustomerPage(bl, bl.GetCustomer(c.Id)));
        }

        private void showDroneButton_Click(object sender, RoutedEventArgs e)
        {
            BO.DroneToL d = bl.GetDroneToL(int.Parse(droneIdTextBox.Text));
            this.NavigationService.Navigate(new DronePage(bl, d));
        }
    }
}
