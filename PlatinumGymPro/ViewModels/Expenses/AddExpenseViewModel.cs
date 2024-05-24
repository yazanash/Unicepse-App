using PlatinumGymPro.Commands.ExpensesCommands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Expenses
{
    public class AddExpenseViewModel : ErrorNotifyViewModelBase
    {
        private readonly ExpensesDataStore _expensesDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesListViewModel _expensesListViewModel;

        public AddExpenseViewModel(ExpensesDataStore expensesDataStore, NavigationStore navigationStore, ExpensesListViewModel expensesListViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _expensesListViewModel = expensesListViewModel;
            _navigationStore = navigationStore;
            SubmitCommand = new SubmitExpensesCommand(_expensesDataStore, new NavigationService<ExpensesListViewModel>(_navigationStore, () => _expensesListViewModel), this);
        }
        public ICommand SubmitCommand { get; }

        #region Properties
        private double _expensesValue;
        public double ExpensesValue
        {
            get { return _expensesValue; }
            set
            {
                _expensesValue = value; OnPropertyChanged(nameof(ExpensesValue));
                ClearError(nameof(ExpensesValue));
                if (ExpensesValue < 0)
                {
                    AddError("لايمكن الدفع بقيمة اقل من 0", nameof(ExpensesValue));
                    OnErrorChanged(nameof(ExpensesValue));
                }
            }
        }
        private string? _descriptiones;
        public string? Descriptiones
        {
            get { return _descriptiones; }
            set { _descriptiones = value; OnPropertyChanged(nameof(Descriptiones)); }
        }
        private DateTime _expensesDate = DateTime.Now;
        public DateTime ExpensesDate
        {
            get { return _expensesDate; }
            set { _expensesDate = value; OnPropertyChanged(nameof(ExpensesDate)); }
        }
        #endregion
    }
}
