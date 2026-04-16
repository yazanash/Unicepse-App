using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Employee.CreditsCommands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Employee;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Views.EmployeeViews.CreditViews;

namespace Uniceps.ViewModels.Employee.CreditViewModels
{
    public class CreditListItemViewModel : ViewModelBase
    {
        private readonly EmployeeStore? _employeeStore;
        private readonly CreditsDataStore? _creditsDataStore;
        public Credit credit;
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public int Id => credit.Id;
        public string? EmployeeName => credit.EmpPerson!.FullName;
        public double CreditValue => credit.CreditValue;
        public string? Date => credit.Date.ToShortDateString();
        public string? Description => credit.Description;
        public CreditListItemViewModel(Credit credit, EmployeeStore employeeStore, CreditsDataStore creditsDataStore)
        {
            this.credit = credit;
            _employeeStore = employeeStore;
            _creditsDataStore = creditsDataStore;
            DeleteCommand = new DeleteCreditsCommand(_creditsDataStore);
        }
        public CreditListItemViewModel(Credit credit)
        {
            this.credit = credit;

        }
        public ICommand EditCommand => new RelayCommand(ExecuteCreateCreditsCommand);

        private void ExecuteCreateCreditsCommand()
        {
            if (_employeeStore != null && _creditsDataStore != null)
            {
                EditCreditDetailsViewModel editCreditDetailsViewModel = new EditCreditDetailsViewModel(_employeeStore, _creditsDataStore);
                CreateCreditWindowView createCreditWindowView = new CreateCreditWindowView();
                createCreditWindowView.DataContext = editCreditDetailsViewModel;
                createCreditWindowView.ShowDialog();
            }
        
        }

        public ICommand? DeleteCommand { get; }
        public void Update(Credit obj)
        {
            credit = obj;
        }
    }
}
