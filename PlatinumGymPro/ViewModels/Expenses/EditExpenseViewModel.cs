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
    public class EditExpenseViewModel : ViewModelBase
    {
        private readonly ExpensesDataStore _expensesDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly ExpensesListViewModel _expensesListViewModel;

        public EditExpenseViewModel(ExpensesDataStore expensesDataStore, NavigationStore navigationStore, ExpensesListViewModel expensesListViewModel)
        {
            _expensesDataStore = expensesDataStore;
            _expensesListViewModel = expensesListViewModel;
            _navigationStore = navigationStore;
            SubmitCommand = new EditExpensesCommand(_expensesDataStore, new NavigationService<ExpensesListViewModel>(_navigationStore, () => _expensesListViewModel), this);

            ExpensesValue = _expensesDataStore.SelectedExpenses!.Value;
            Descriptiones = _expensesDataStore.SelectedExpenses!.Description;
            ExpensesDate = _expensesDataStore.SelectedExpenses!.date;

        }
        public ICommand SubmitCommand { get; }

        #region Properties
        private double _expensesValue;
        public double ExpensesValue
        {
            get { return _expensesValue; }
            set { _expensesValue = value; OnPropertyChanged(nameof(ExpensesValue)); }
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
