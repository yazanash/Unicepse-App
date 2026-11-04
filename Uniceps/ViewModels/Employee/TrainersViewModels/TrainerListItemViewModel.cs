using Uniceps.Commands;
using Uniceps.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.Views.EmployeeViews;
using Uniceps.navigation.NavigationSystem;
using Uniceps.navigation.DialogService;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerListItemViewModel : ViewModelBase
    {
        public Core.Models.Employee.Employee Trainer;
        private readonly EmployeeStore? _employeeStore;
        private readonly INavigationService? _navigationService;
        private readonly IDialogService? _dialogService;
        public int Id => Trainer.Id;
        public string? FullName => Trainer.FullName;
        public double SalaryValue => Trainer.SalaryValue;
        public double ParcentValue => Trainer.ParcentValue;
        public string? Phone => Trainer.Phone;
        public int BirthDate => Trainer.BirthDate;
        public string? Position => Trainer.Position;
        public string? Gendertext => Trainer.GenderMale ? "ذكر" : "انثى";
        public string? SubscribeDate => Trainer.StartDate.ToShortDateString();
        public Brush IsSubscribed => Trainer.IsActive ? Brushes.Green : Brushes.Red;
        public ICommand? EditCommand { get; }
        public ICommand? OpenAccountCommand { get; }
        public ICommand? DeleteCommand { get; }

        public TrainerListItemViewModel(Core.Models.Employee.Employee trainer, EmployeeStore employeeStore, INavigationService navigationService, IDialogService dialogService)
        {
            Trainer = trainer;
            _employeeStore = employeeStore;
            _navigationService = navigationService;
            _dialogService = dialogService;


            EditCommand = new RelayCommand(ExecuteEditEmployeeCommand);
            DeleteCommand = new DeleteEmployeeCommand(_employeeStore, _navigationService);
            var navParams = new Dictionary<string, object>()
            {
                { "Employee",Trainer}
            };
            OpenAccountCommand = new RelayCommand(() => _navigationService.Navigate<EmployeeAccountViewModel>(navParams));
        }
        public void ExecuteEditEmployeeCommand()
        {
            if (_dialogService != null)
            {
                var navParams = new Dictionary<string, object>()
            {
                { "Employee",Trainer}
            };
                _dialogService.ShowDialog<EmployeeDetailsWindowView, AddEmployeeViewModel>(navParams);
            }
        }
        public TrainerListItemViewModel(Core.Models.Employee.Employee trainer)
        {
            Trainer = trainer;
        }
       
        public void Update(Core.Models.Employee.Employee trainer)
        {
            Trainer = trainer;
            OnPropertyChanged(nameof(trainer));
        }
    }
}
