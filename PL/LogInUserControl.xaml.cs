﻿using System;
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
    /// Interaction logic for LogInUserControl.xaml
    /// </summary>
    public partial class LogInUserControl : UserControl
    {
        IBL bl;
        public LogInUserControl(IBL _bl)
        {
            InitializeComponent();
            bl = _bl;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            UserOptionsUserControl myUser = new UserOptionsUserControl(bl,((sender as Button).Content).ToString());
            signInGrid.Children.Clear();
            signInGrid.Children.Add(myUser);
        }
    }
}
