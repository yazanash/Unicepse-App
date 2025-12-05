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
using Uniceps.ViewModels.Employee.TrainersViewModels;

namespace Uniceps.Views.EmployeeViews
{
    /// <summary>
    /// Interaction logic for EmployeeDetailsWindowView.xaml
    /// </summary>
    public partial class EmployeeDetailsWindowView : Window
    {
        public EmployeeDetailsWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += EmployeeDetailsWindowView_DataContextChanged;
        }

        private void EmployeeDetailsWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddEmployeeViewModel;
            if (vm != null)
                vm.EmployeeCreated += () => this.Close();
            var editVm = DataContext as EditEmployeeViewModel;
            if (editVm != null)
                editVm.EmployeeUpdated += () => this.Close();
        }
    }
}
