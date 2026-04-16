using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Employee.CreditsCommands;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;

namespace Uniceps.ViewModels.Employee.CreditViewModels
{
    public class CreateCreditViewModelWindow : ErrorNotifyViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditDataStore;
        public CreateCreditViewModelWindow(EmployeeStore employeeStore, CreditsDataStore creditDataStore)
        {
            _employeeStore = employeeStore;
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand(_employeeStore, _creditDataStore, this);
        }
        public CreateCreditViewModelWindow(EmployeeStore employeeStore, CreditsDataStore creditDataStore, double amount)
        {
            _employeeStore = employeeStore;
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand( _employeeStore, _creditDataStore, this);
            CreditValue = amount;
        }
        public Action? CreditCreated;
        internal void OnCreditCreated()
        {
            CreditCreated?.Invoke();
        }
        private double _creditValue;
        public double CreditValue
        {
            get { return _creditValue; }
            set
            {
                _creditValue = value;
                OnPropertyChanged(nameof(CreditValue));
                ClearError(nameof(CreditValue));
                if (CreditValue < 0)
                {
                    AddError("لايمكن الدفع بقيمة اقل من 0", nameof(CreditValue));
                    OnErrorChanged(nameof(CreditValue));
                }
            }
        }
        private DateTime _creditDate = DateTime.Now;
        public DateTime CreditDate
        {
            get { return _creditDate; }
            set { _creditDate = value; OnPropertyChanged(nameof(CreditDate)); }
        }
        private string? _description;
        public string? Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged(nameof(Description)); }
        }
        public ICommand SubmitCommand { get; }
    }
}
