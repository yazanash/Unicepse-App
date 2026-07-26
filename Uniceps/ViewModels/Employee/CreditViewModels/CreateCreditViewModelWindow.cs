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
using Emp = Uniceps.Core.Models.Employee;
namespace Uniceps.ViewModels.Employee.CreditViewModels
{
    public class CreateCreditViewModelWindow : ErrorNotifyViewModelBase
    {
        private readonly CreditsDataStore _creditDataStore;
        public Emp.Employee? SelectedEmployee;
        public Emp.Credit? Credit;
        public bool IsEditMode = false;
        public CreateCreditViewModelWindow(CreditsDataStore creditDataStore, Emp.Employee selectedEmployee)
        {
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand(_creditDataStore, this);
            SelectedEmployee = selectedEmployee;
        }
        public CreateCreditViewModelWindow( CreditsDataStore creditDataStore, Emp.Credit credit)
        {
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand(_creditDataStore, this);
            Credit = credit;
            IsEditMode = true;
            CreditValue = Credit.CreditValue;
            CreditDate = credit.Date;
            Description = credit.Description;
        }
        public CreateCreditViewModelWindow(CreditsDataStore creditDataStore, double amount, Emp.Employee selectedEmployee)
        {
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand(_creditDataStore, this);
            CreditValue = amount;
            SelectedEmployee = selectedEmployee;
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
