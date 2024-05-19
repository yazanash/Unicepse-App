using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using exp = PlatinumGym.Core.Models.Expenses;
namespace PlatinumGymPro.ViewModels.Accountant
{
    public class ExpensesListItemViewModel : ViewModelBase
    {
        public exp.Expenses Expenses;
        public int Id => Expenses.Id;
        public string? Description => Expenses.Description;
        public double? Value => Expenses.Value;
        public string date => Expenses.date.ToShortDateString();

        public ExpensesListItemViewModel(exp.Expenses expenses)
        {
            Expenses = expenses;
        }
    }
}
