using Uniceps.Commands.ExpensesCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.ViewModels;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.Expenses
{
    public class EditExpenseViewModel : ErrorNotifyViewModelBase
    {
        private readonly ExpensesDataStore _expensesDataStore;
        public ExpensesListItemViewModel SelectedExpensesListItemViewModel;
        public EditExpenseViewModel(ExpensesDataStore expensesDataStore, ExpensesListItemViewModel selectedExpensesListItemViewModel)
        {
            _expensesDataStore = expensesDataStore;
            SelectedExpensesListItemViewModel = selectedExpensesListItemViewModel;
            SubmitCommand = new EditExpensesCommand(_expensesDataStore, this);
            ExpensesValue = SelectedExpensesListItemViewModel.Expenses!.Value;
            Descriptiones = SelectedExpensesListItemViewModel.Expenses!.Description;
            ExpensesDate = SelectedExpensesListItemViewModel.Expenses!.date;
        }
        public ICommand SubmitCommand { get; }
        public Action? ExpensesUpdated;
        public void OnExpensesUpdated()
        {
            ExpensesUpdated?.Invoke();
        }
        #region Properties
        private double _expensesValue;
        public double ExpensesValue
        {
            get { return _expensesValue; }
            set
            {
                _expensesValue = value;
                OnPropertyChanged(nameof(ExpensesValue));
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
