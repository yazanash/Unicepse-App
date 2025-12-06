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
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.PlayersViewModels;

namespace Uniceps.Views.Expenses
{
    /// <summary>
    /// Interaction logic for ExpenseDetailViewWinow.xaml
    /// </summary>
    public partial class ExpenseDetailViewWinow : Window
    {
        public ExpenseDetailViewWinow()
        {
            InitializeComponent();
            this.DataContextChanged += ExpenseDetailViewWinow_DataContextChanged; ;
        }

        private void ExpenseDetailViewWinow_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var vm = DataContext as AddExpenseViewModel;
            if (vm != null)
                vm.ExpensesCreated += () => this.Close();
            var editVm = DataContext as EditExpenseViewModel;
            if (editVm != null)
                editVm.ExpensesUpdated += () => this.Close();
        }
    }
}
