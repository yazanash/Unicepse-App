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
using Uniceps.ViewModels.OtherRevenueViewModels;

namespace Uniceps.Views.OtherRevenueViews
{
    /// <summary>
    /// Interaction logic for RevenueDetailViewWindow.xaml
    /// </summary>
    public partial class RevenueDetailViewWindow : Window
    {
        public RevenueDetailViewWindow()
        {
            InitializeComponent();
            this.DataContextChanged += RevenueDetailViewWindow_DataContextChanged;
        }

        private void RevenueDetailViewWindow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddRevenueViewModel;
            if (vm != null)
                vm.RevenueCreated += () => this.Close();
        }
    }
}
