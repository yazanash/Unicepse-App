using System.Collections.Generic;
using System.Windows.Input;
using Uniceps.Core.Models.Sport;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Emp = Uniceps.Core.Models.Employee;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class EmployeeAccountViewModel : ViewModelBase , IQueryAttributable
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private EmployeeStore _employeeStore;
        public TrainerListItemViewModel? Employee { get; set; }
        public Emp.Employee? SelectedEmployee { get; set; }
        public EmployeeAccountantPageViewModel EmployeeAccountantPageViewModel { get; }
        public CreditListViewModel CreditListViewModel { get; }
        public PremiumViewModel PremiumViewModel { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }
        public EmployeeAccountViewModel(EmployeeAccountantPageViewModel employeeAccountantPageViewModel, CreditListViewModel creditListViewModel, PremiumViewModel premiumViewModel, INavigationService navigationService, IDialogService dialogService, EmployeeStore employeeStore)
        {
            EmployeeAccountantPageViewModel = employeeAccountantPageViewModel;
            CreditListViewModel = creditListViewModel;
            PremiumViewModel = premiumViewModel;
            _navigationService = navigationService;
            _dialogService = dialogService;
            _employeeStore = employeeStore;
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<Emp.Employee>("Employee", out var emp))
            {
                if (emp != null)
                {
                    SelectedEmployee = emp;
                    Employee = new TrainerListItemViewModel(emp, _employeeStore, _navigationService, _dialogService);
                }
                EmployeeAccountantPageViewModel.ApplyQueryAttributes(query);
                CreditListViewModel.ApplyQueryAttributes(query);
            }
        }
    }
}
