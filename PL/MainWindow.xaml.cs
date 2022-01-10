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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal readonly IBL bl = BlFactory.GetBl();
        /// <summary>
        /// A ctor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }
        private void MyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new MainPage(bl));
        }
        /// <summary>
        /// A function that opens the drone list window
        /// </summary>
        /// <param name="sender"> The sent object</param>
        /// <param name="e"> Routed event args</param>
        private void showDronesButton_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new DronesListPage(bl));
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    StationsListUserControl myUser = new StationsListUserControl(bl);
        //    mainGrid.Opacity = 0.9;
        //    //mainGrid.Children.Clear();
        //    //mainGrid.Children.Add(myUser);
        //}
        //private void back_hostUserControl(object sender, MouseButtonEventArgs e)
        //{
        //    mainGrid.Children.Clear();
        //    mainGrid.Children.Add(wingsGrid);
        //}
        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    StationsListUserControl myUser = new StationsListUserControl(bl);
        //    myUser.CloseButton.MouseLeftButtonDown += back_hostUserControl;
        //    mainGrid.Opacity = 0.9;
        //    mainGrid.Children.Clear();
        //    mainGrid.Children.Add(myUser);
        //}

        //private void logInButton_Click(object sender, RoutedEventArgs e)
        //{
        //    LogInUserControl myUser = new LogInUserControl(bl);
        //   // myUser.CloseButton.MouseLeftButtonDown += back_hostUserControl;
        //    mainGrid.Opacity = 0.9;
        //    mainGrid.Children.Clear();
        //    mainGrid.Children.Add(myUser);
        //}

        private void showCustomersButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomersListWindow(bl).ShowDialog();
        }

        private void showParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(bl).ShowDialog();
        }

        private void showStationsButton_Click(object sender, RoutedEventArgs e)
        {
            new StationsListWindow(bl).ShowDialog();
        }
    }
}
