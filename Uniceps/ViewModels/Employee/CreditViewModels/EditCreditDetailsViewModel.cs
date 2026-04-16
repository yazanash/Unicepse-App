using Uniceps.Commands;
using Uniceps.Commands.Employee.CreditsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.navigation;
using Uniceps.Commands.Player;
using Uniceps.Stores;
using Uniceps.ViewModels;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.Employee.CreditViewModels
{
    public class EditCreditDetailsViewModel : ErrorNotifyViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditDataStore;
        public EditCreditDetailsViewModel(EmployeeStore employeeStore, CreditsDataStore creditDataStore)
        {
            _employeeStore = employeeStore;
            _creditDataStore = creditDataStore;
            CreditDate = _creditDataStore.SelectedCredit!.Date;
            CreditValue = _creditDataStore.SelectedCredit!.CreditValue;
            Description = _creditDataStore.SelectedCredit!.Description;
            SubmitCommand = new EditCreditCommand( _employeeStore, _creditDataStore, this);
        }
        public Action? CreditUpdated;
        internal void OnCreditUpdated()
        {
            CreditUpdated?.Invoke();
        }
        private double _creditValue;
        public double CreditValue
        {
            get { return _creditValue; }
            set
            {
                _creditValue = value; OnPropertyChanged(nameof(CreditValue));
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
            set
            {
                _creditDate = value;
                OnPropertyChanged(nameof(CreditDate));

            }
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
