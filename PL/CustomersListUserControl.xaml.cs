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
    /// Interaction logic for CustomersListUserControl.xaml
    /// </summary>
    public partial class CustomersListUserControl : UserControl
    {
        IBL bl;
        public CustomersListUserControl(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            customerToLDataGrid.DataContext = bl.CustomerList();
            customerToLDataGrid.IsReadOnly = true;
        }

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            CustomerUserControl myUser = new CustomerUserControl(bl);
            customersListGrid.Children.Clear();
            customersListGrid.Children.Add(myUser);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            customersListGrid.Visibility = Visibility.Hidden;
        }

        private void customerToLDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.CustomerToL curCustomerToL = customerToLDataGrid.SelectedItem as BO.CustomerToL;
            if (curCustomerToL != null)
            {
                CustomerUserControl myUser = new CustomerUserControl(bl, curCustomerToL);
                customersListGrid.Children.Clear();
                customersListGrid.Children.Add(myUser);
            }
        }
    }
}
