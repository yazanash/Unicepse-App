using PlatinumGym.Core.Models.Expenses;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Exp = PlatinumGym.Core.Models.Expenses;

namespace PlatinumGymPro.ViewModels.Expenses
{
    public class ExpensesListItemViewModel : ViewModelBase
    {
        public Exp.Expenses Expenses { get; set; }

        public ExpensesListItemViewModel(Exp.Expenses expenses, NavigationStore _navigatorStore)
        {
            Expenses = expenses;
            EditExpensesCommand = new NavaigateCommand<EditExpenseViewModel>(new NavigationService<EditExpenseViewModel>(_navigatorStore, () => new EditExpenseViewModel()));
        }
        public int Id => Expenses.Id;
        public string? Description => Expenses.Description;
        public double? Value => Expenses.Value;
        public string date => Expenses.date.ToLongDateString();
     
        public ICommand EditExpensesCommand { get; }
        public ICommand DeleteExpensesCommand { get; }
        public void Update(Exp.Expenses obj)
        {
            throw new NotImplementedException();
        }
    }
}
