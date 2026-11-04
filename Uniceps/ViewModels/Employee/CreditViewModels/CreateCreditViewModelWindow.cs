using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Employee.CreditsCommands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Metric;
using Uniceps.navigation;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Emp = Uniceps.Core.Models.Employee;
namespace Uniceps.ViewModels.Employee.CreditViewModels
{
    public class CreateCreditViewModelWindow : ErrorNotifyViewModelBase, IQueryAttributable
    {
        private readonly CreditsDataStore _creditDataStore;
        public Emp.Employee? SelectedEmployee;
        public Emp.Credit? Credit;
        public bool IsEditMode = false;
        public CreateCreditViewModelWindow(CreditsDataStore creditDataStore)
        {
            _creditDataStore = creditDataStore;
            SubmitCommand = new SubmitCreditCommand(_creditDataStore, this);
        }
        private void ApplyEditMode(Emp.Credit credit)
        {
            Credit = credit;
            IsEditMode = true;
            CreditValue = Credit.CreditValue;
            CreditDate = credit.Date;
            Description = credit.Description;
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
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<Emp.Employee>("Employee", out var emp))
            {
                if (emp != null)
                {
                    SelectedEmployee = emp;
                }
            }
            if (query.TryGetValue<Credit>("Credit", out var credit))
            {
                if (credit != null)
                    ApplyEditMode(credit);
            }
            if (query.TryGetValue<double>("Amount", out var amount))
            {
                CreditValue = amount;
            }
          
        }
    }
}
