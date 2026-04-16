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
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.SportsViewModels;

namespace Uniceps.Views.EmployeeViews.CreditViews
{
    /// <summary>
    /// Interaction logic for CreateCreditWindowView.xaml
    /// </summary>
    public partial class CreateCreditWindowView : Window
    {
        public CreateCreditWindowView()
        {
            InitializeComponent();
            this.DataContextChanged += CreateCreditWindowView_DataContextChanged;
        }

        private void CreateCreditWindowView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as CreateCreditViewModelWindow;
            if (vm != null)
                vm.CreditCreated += () => this.Close();
            var editVm = DataContext as EditCreditDetailsViewModel;
            if (editVm != null)
                editVm.CreditUpdated += () => this.Close();
        }

    }
}
