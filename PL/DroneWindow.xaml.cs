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

namespace PL
{
    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL.BL bL;
        public DroneWindow(IBL.BL bl)
        {
            InitializeComponent();
            bL = bl;
            WeightCBox.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }
        public DroneWindow(IBL.BL bl, IBL.BO.Drone d)
        {
            InitializeComponent();
            bL = bl;
        }

        private void AddButon_Click(object sender, RoutedEventArgs e)
        {
            IBL.BO.Drone d = new IBL.BO.Drone();
            d.Id = int.Parse(IdTxt.Text);
            d.Model = ModelTxt.Text;
            d.Weight = (IBL.BO.WeightCategories)WeightCBox.SelectedItem;
            try
            {
                bL.AddDrone(d, int.Parse(IdStationTxt.Text));
                MessageBox.Show("The drone is added successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
