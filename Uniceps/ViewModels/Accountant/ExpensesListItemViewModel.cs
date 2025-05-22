using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.utlis.common;
using exp = Uniceps.Core.Models.Expenses;

namespace Uniceps.ViewModels.Accountant
{
    public class ExpensesListItemViewModel : ViewModelBase
    {
        public Core.Models.Expenses.Expenses Expenses;
        public int Id => Expenses.Id;
        public string? Description => Expenses.Description;
        public double? Value => Expenses.Value;
        public string date => Expenses.date.ToShortDateString();

        public ExpensesListItemViewModel(Core.Models.Expenses.Expenses expenses)
        {
            Expenses = expenses;
        }
    }
}
