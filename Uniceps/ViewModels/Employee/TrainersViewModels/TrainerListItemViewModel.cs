using Uniceps.Commands;
using Uniceps.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using emp = Uniceps.Core.Models.Employee;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.navigation.Stores;
using Uniceps.Stores.EmployeeStores;

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
        private readonly LicenseDataStore? _licenseDataStore;
        private readonly EmployeeSubscriptionDataStore? _employeeSubscriptionDataStore;
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

        public TrainerListItemViewModel(Core.Models.Employee.Employee trainer, NavigationStore navigationStore, EmployeeStore employeeStore, SportDataStore sportDataStore, TrainersListViewModel trainersListViewModel, DausesDataStore? dausesDataStore, CreditsDataStore? creditsDataStore, LicenseDataStore licenseDataStore, EmployeeSubscriptionDataStore? employeeSubscriptionDataStore)
        {
            Trainer = trainer;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _trainersListViewModel = trainersListViewModel;
            _dausesDataStore = dausesDataStore;
            _navigationStore = navigationStore;
            _creditsDataStore = creditsDataStore;
            _licenseDataStore = licenseDataStore;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;

            NavigationStore EmployeeAccountPageNavigation = new NavigationStore();
            if (trainer.IsTrainer)
                EditCommand = new NavaigateCommand<EditTrainerViewModel>(new NavigationService<EditTrainerViewModel>(_navigationStore, () => CreateEditTrainerViewModel(navigationStore, _trainersListViewModel, _sportDataStore, _employeeStore)));
            else
                EditCommand = new NavaigateCommand<EditEmployeeViewModel>(new NavigationService<EditEmployeeViewModel>(_navigationStore, () => new EditEmployeeViewModel(navigationStore, _trainersListViewModel, _employeeStore)));
            DeleteCommand = new DeleteEmployeeCommand(_employeeStore, new NavigationService<TrainersListViewModel>(_navigationStore, () => _trainersListViewModel));

            OpenAccountCommand = new NavaigateCommand<EmployeeAccountViewModel>(new NavigationService<EmployeeAccountViewModel>(_navigationStore, () => new EmployeeAccountViewModel(EmployeeAccountPageNavigation, _employeeStore, _dausesDataStore!, _creditsDataStore!, this, _licenseDataStore, _employeeSubscriptionDataStore!)));
        }
        public TrainerListItemViewModel(Core.Models.Employee.Employee trainer)
        {
            Trainer = trainer;

        }
        private EditTrainerViewModel CreateEditTrainerViewModel(NavigationStore navigationStore, TrainersListViewModel trainersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            return EditTrainerViewModel.LoadViewModel(navigationStore, trainersListViewModel, sportDataStore, employeeStore);
        }

        public void Update(Core.Models.Employee.Employee trainer)
        {
            Trainer = trainer;
            OnPropertyChanged(nameof(trainer));
        }
    }
}
