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
    /// Interaction logic for CustomersListWindow.xaml
    /// </summary>
    public partial class CustomersListWindow : Window
    {
        IBL bl;
        public CustomersListWindow(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            customerToLDataGrid.DataContext = bl.CustomerList();
            customerToLDataGrid.IsReadOnly = true;
            
        }

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl).ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void customerToLDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.CustomerToL curCustomerToL = customerToLDataGrid.SelectedItem as BO.CustomerToL;
            if (curCustomerToL != null)
            {
                BO.Customer c = bl.GetCustomer(curCustomerToL.Id);
                new CustomerWindow(bl, c).ShowDialog();
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            customerToLDataGrid.DataContext = bl.CustomerList();
        }
    }
}
