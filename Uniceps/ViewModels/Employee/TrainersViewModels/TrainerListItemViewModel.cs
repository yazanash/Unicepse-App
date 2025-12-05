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
using Uniceps.navigation.Stores;
using Uniceps.Stores.EmployeeStores;
using Uniceps.Views.EmployeeViews;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainerListItemViewModel : ViewModelBase
    {
        public Core.Models.Employee.Employee Trainer;
        private readonly EmployeeStore? _employeeStore;
        private readonly DausesDataStore? _dausesDataStore;
        private readonly SportDataStore? _sportDataStore;
        private readonly TrainersListViewModel? _trainersListViewModel;
        private readonly NavigationStore? _navigationStore;
        private readonly CreditsDataStore? _creditsDataStore;
        private readonly EmployeeSubscriptionDataStore? _employeeSubscriptionDataStore;
        private readonly AccountStore?   _accountStore;
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

        public TrainerListItemViewModel(Core.Models.Employee.Employee trainer, NavigationStore navigationStore, EmployeeStore employeeStore, SportDataStore sportDataStore, TrainersListViewModel trainersListViewModel, DausesDataStore? dausesDataStore, CreditsDataStore? creditsDataStore, EmployeeSubscriptionDataStore? employeeSubscriptionDataStore, AccountStore accountStore)
        {
            Trainer = trainer;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _trainersListViewModel = trainersListViewModel;
            _dausesDataStore = dausesDataStore;
            _navigationStore = navigationStore;
            _creditsDataStore = creditsDataStore;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;
            _accountStore = accountStore;

            NavigationStore EmployeeAccountPageNavigation = new NavigationStore();
            if (trainer.IsTrainer)
                EditCommand = new RelayCommand(ExecuteEditTrainerCommand);
            else
                EditCommand = new RelayCommand(ExecuteEditEmployeeCommand);
            DeleteCommand = new DeleteEmployeeCommand(_employeeStore, new NavigationService<TrainersListViewModel>(_navigationStore, () => _trainersListViewModel));

            OpenAccountCommand = new NavaigateCommand<EmployeeAccountViewModel>(new NavigationService<EmployeeAccountViewModel>(_navigationStore, () => new EmployeeAccountViewModel(EmployeeAccountPageNavigation, _employeeStore, _dausesDataStore!, _creditsDataStore!, this, _employeeSubscriptionDataStore!, _accountStore)));
        }
        public void ExecuteEditTrainerCommand()
        {
            EditTrainerViewModel editTrainerViewModel = EditTrainerViewModel.LoadViewModel(_sportDataStore!, _employeeStore!);
            TrainerDetailsWindowView trainerDetailsWindowView = new TrainerDetailsWindowView();
            trainerDetailsWindowView.DataContext = editTrainerViewModel;
            trainerDetailsWindowView.ShowDialog();
        }
        public void ExecuteEditEmployeeCommand()
        {
            EditEmployeeViewModel editEmployeeViewModel = new EditEmployeeViewModel(_employeeStore!);
            EmployeeDetailsWindowView employeeDetailsWindowView = new EmployeeDetailsWindowView();
            employeeDetailsWindowView.DataContext = editEmployeeViewModel;
            employeeDetailsWindowView.ShowDialog();
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
