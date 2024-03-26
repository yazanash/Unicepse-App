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
using emp = PlatinumGym.Core.Models.Employee;

namespace PlatinumGymPro.ViewModels.TrainersViewModels
{
    public class TrainerListItemViewModel : ViewModelBase
    {
        public emp.Employee Trainer;
        private readonly EmployeeStore employeeStore;
        private readonly SportDataStore sportDataStore;
        private readonly TrainersListViewModel  _trainersListViewModel;
        private readonly NavigationStore _navigationStore;
        public int Id => Trainer.Id;
        public string? FullName => Trainer.FullName;
        public double SalaryValue => Trainer.SalaryValue;
        public double ParcentValue => Trainer.ParcentValue;
        public string? Phone => Trainer.Phone;
        public int BirthDate => Trainer.BirthDate;
        public string? Position => Trainer.Position;

        public ICommand? EditCommand { get; }
        public ICommand? DeleteCommand { get; }

        public TrainerListItemViewModel(emp.Employee trainer, NavigationStore navigationStore, EmployeeStore employeeStore, SportDataStore sportDataStore, TrainersListViewModel trainersListViewModel)
        {
            Trainer = trainer;
            this.employeeStore = employeeStore;
            this.sportDataStore = sportDataStore;
            _trainersListViewModel = trainersListViewModel;
            _navigationStore = navigationStore;
            EditCommand = new NavaigateCommand<EditTrainerViewModel>(new NavigationService<EditTrainerViewModel>(_navigationStore, () => CreateEditTrainerViewModel(_navigationStore, _trainersListViewModel, sportDataStore, employeeStore)));
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
