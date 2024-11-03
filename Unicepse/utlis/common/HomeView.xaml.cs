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
using Unicepse.ViewModels.PlayersAttendenceViewModels;

namespace Unicepse.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
        }

        private void PlayerDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Calendar_LostFocus(object sender, RoutedEventArgs e)
        {
            calender.SelectedDate = DateTime.Now;
        }
       
    }
}
