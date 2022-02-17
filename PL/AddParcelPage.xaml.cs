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
        public AddParcelPage(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;
        }

        private void GoBack(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (weightComboBox.SelectedIndex == -1 || priorityComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == -1 || receiverComboBox.SelectedIndex == -1 || senderComboBox.SelectedIndex == receiverComboBox.SelectedIndex)
                OpButton.IsEnabled = false;
            else
                OpButton.IsEnabled = true;

        }
    
    }
}
