using PlatinumGym.Core.Models.Employee;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.Employee.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using emp = PlatinumGym.Core.Models.Employee;

namespace PlatinumGymPro.ViewModels.TrainersViewModels
{
    public class TrainerListItemViewModel : ViewModelBase
    {
        public emp.Employee Trainer;
        private readonly EmployeeStore? _employeeStore;
        private readonly DausesDataStore?  _dausesDataStore;
        private readonly SportDataStore? _sportDataStore;
        private readonly SubscriptionDataStore? _subscriptionDataStore;
        private readonly TrainersListViewModel? _trainersListViewModel;
        private readonly NavigationStore? _navigationStore;
        private readonly CreditsDataStore? _creditsDataStore;
        public int Id => Trainer.Id;
        public string? FullName => Trainer.FullName;
        public double SalaryValue => Trainer.SalaryValue;
        public double ParcentValue => Trainer.ParcentValue;
        public string? Phone => Trainer.Phone;
        public int BirthDate => Trainer.BirthDate;
        public string? Position => Trainer.Position;
        public Brush IsSubscribed => Trainer.IsActive ? Brushes.Green : Brushes.Red;
        public ICommand? EditCommand { get; }
        public ICommand? OpenAccountCommand { get; }

        public TrainerListItemViewModel(emp.Employee trainer, NavigationStore navigationStore, EmployeeStore employeeStore, SportDataStore sportDataStore, TrainersListViewModel trainersListViewModel, SubscriptionDataStore subscriptionDataStore, DausesDataStore? dausesDataStore, CreditsDataStore? creditsDataStore)
        {
            Trainer = trainer;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _trainersListViewModel = trainersListViewModel;
            _subscriptionDataStore = subscriptionDataStore;
            _dausesDataStore = dausesDataStore;
            _navigationStore = navigationStore;
            _creditsDataStore = creditsDataStore;

            if (trainer.IsTrainer)
                EditCommand = new NavaigateCommand<EditTrainerViewModel>(new NavigationService<EditTrainerViewModel>(_navigationStore, () => CreateEditTrainerViewModel(_navigationStore, _trainersListViewModel, sportDataStore, employeeStore)));
            else if (trainer.IsSecrtaria)
                EditCommand = new NavaigateCommand<EditEmployeeViewModel>(new NavigationService<EditEmployeeViewModel>(_navigationStore, () => new EditEmployeeViewModel(_navigationStore, _trainersListViewModel, employeeStore)));
            NavigationStore EmployeeAccountPageNavigation = new NavigationStore();
            OpenAccountCommand = new NavaigateCommand<EmployeeAccountViewModel>(new NavigationService<EmployeeAccountViewModel>(_navigationStore, () => new EmployeeAccountViewModel(EmployeeAccountPageNavigation, _employeeStore, _dausesDataStore!,_creditsDataStore!)));
        }
        public TrainerListItemViewModel(emp.Employee trainer)
        {
            Trainer = trainer;
        }

        private EditTrainerViewModel CreateEditTrainerViewModel(NavigationStore navigationStore, TrainersListViewModel trainersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            return EditTrainerViewModel.LoadViewModel(navigationStore, trainersListViewModel, sportDataStore, employeeStore);
        }

        public void Update(emp.Employee trainer)
        {
            this.Trainer = trainer;

            OnPropertyChanged(nameof(trainer));
        }
    }
}
