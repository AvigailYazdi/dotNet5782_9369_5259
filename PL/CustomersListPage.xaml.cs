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
    /// Interaction logic for CustomersListPage.xaml
    /// </summary>
    public partial class CustomersListPage : Page
    {
        IBL bl;
        ObservableCollection<BO.CustomerToL> oc;
        public CustomersListPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
            oc = new ObservableCollection<BO.CustomerToL>(bl.CustomerList());
            customerToLDataGrid.DataContext = oc;
            customerToLDataGrid.IsReadOnly = true;
        }

        private void addCustomerButton_Click(object sender, RoutedEventArgs e)
        {

            this.NavigationService.Navigate(new CustomerPage(bl,oc));
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void customerToLDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.CustomerToL curCustomerToL = customerToLDataGrid.SelectedItem as BO.CustomerToL;
            if (curCustomerToL != null)
            {
                BO.Customer c = bl.GetCustomer(curCustomerToL.Id);
               this.NavigationService.Navigate(new CustomerPage(bl, c));
            }
        }
        private void Window_Activated(object sender, EventArgs e)
        {
            customerToLDataGrid.DataContext = bl.CustomerList();
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            oc = new ObservableCollection<BO.CustomerToL>(bl.CustomerList());
            customerToLDataGrid.DataContext = oc;
        }
    }
}

