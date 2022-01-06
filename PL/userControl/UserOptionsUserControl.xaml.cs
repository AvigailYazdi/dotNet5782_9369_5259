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
    /// Interaction logic for UserOptionsUserControl.xaml
    /// </summary>
    public partial class UserOptionsUserControl : UserControl
    {
        IBL bl;
        enum op { Manager,Customer};
        op option;
        public UserOptionsUserControl(IBL _bl, string role)
        {
            InitializeComponent();
            bl = _bl;
            if (role == "Manager")
            {
                option = op.Manager;
                welcomeLabel.Content = "Welcome Dear Manager";
                parcelLabel.Content = "Parcels";
                customerLabel.Content = "Customers";
            }
            else
            {
                option = op.Customer;
                welcomeLabel.Content = "Welcome Dear Customer";
                parcelLabel.Content = "Add a new parcel";
                customerLabel.Content = "Update profile";
                //customerImage.Source = new BitmapImage(new Uri("images/profile.png"));
            }
        }
    }
}
