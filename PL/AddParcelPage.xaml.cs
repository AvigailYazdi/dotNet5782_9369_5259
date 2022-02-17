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
    /// Interaction logic for AddParcelPage.xaml
    /// </summary>
    public partial class AddParcelPage : Page
    {
        IBL bl;
        string UserName;
        MessageBoxButton b = MessageBoxButton.OK; // A button of the message box
        MessageBoxImage i = MessageBoxImage.Information; // An icon of the message box
        public AddParcelPage(IBL _bl, string _UserName)
        {
            InitializeComponent();
            bl = _bl;
            UserName = _UserName;
            weightComboBox.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            priorityComboBox.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            receiverComboBox.ItemsSource = bl.CustomerList();
            int i = 0;
            senderComboBox.ItemsSource = bl.CustomerList();
            foreach (var item in bl.CustomerList())
            {
                if (item.Name == UserName)
                    break;
                i++;
            }
            senderComboBox.SelectedIndex = i;
            senderComboBox.IsEnabled = false;
            addImage.IsEnabled = false;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weightComboBox.SelectedIndex == -1 || priorityComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == -1 || receiverComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == receiverComboBox.SelectedIndex)
                addImage.IsEnabled = false;
            else
                addImage.IsEnabled = true;
        }

        private void addUser(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BO.CustomerToL c1 = (BO.CustomerToL)senderComboBox.SelectedItem;
                BO.CustomerToL c2 = (BO.CustomerToL)receiverComboBox.SelectedItem;
                BO.Parcel p = new BO.Parcel();
                p.Weight = (BO.WeightCategories)weightComboBox.SelectedItem;
                p.Priority = (BO.Priorities)priorityComboBox.SelectedItem;
                p.Sender = new BO.CustomerInP
                {
                    Id = c1.Id,
                    Name = c1.Name
                };
                p.Receiver = new BO.CustomerInP
                {
                    Id = c2.Id,
                    Name = c2.Name
                };
                bl.AddParcel(p);
                MessageBox.Show("The parcel is added successfully!", "Add", this.b, i);
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
