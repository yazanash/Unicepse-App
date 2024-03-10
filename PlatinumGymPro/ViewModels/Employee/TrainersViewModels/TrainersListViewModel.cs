using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.TrainersCommands;
using PlatinumGym.Core.Models.Employee;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.TrainersViewModels
{
    public class TrainersListViewModel : ViewModelBase
    {

        private readonly ObservableCollection<TrainerListItemViewModel> trainerListItemViewModels;

        private NavigationStore _navigatorStore;
        private EmployeeStore _employeeStore;

        public IEnumerable<TrainerListItemViewModel> TrainerList => trainerListItemViewModels;

        public ICommand AddTrainerCommand { get; }
        public ICommand LoadTrainerCommand { get; }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        public TrainersListViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            LoadTrainerCommand = new LoadTrainersCommand(_employeeStore, this);
            AddTrainerCommand = new NavaigateCommand<AddTrainerViewModel>(new NavigationService<AddTrainerViewModel>(_navigatorStore, () => new AddTrainerViewModel(navigatorStore, this)));
            trainerListItemViewModels = new ObservableCollection<TrainerListItemViewModel>();



            _employeeStore.Loaded += _trainerStore_TrainersLoaded;
            _employeeStore.Created += _trainerStore_TrainerAdded;
            _employeeStore.Updated += _trainerStore_TrainerUpdated;
            _employeeStore.Deleted += _trainerStore_TrainerDeleted;

        }

        private void _trainerStore_TrainerDeleted(int id)
        {
            TrainerListItemViewModel? itemViewModel = trainerListItemViewModels.FirstOrDefault(y => y.Trainer?.Id == id);

            if (itemViewModel != null)
            {
                trainerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _trainerStore_TrainerUpdated(Employee trainer)
        {
            TrainerListItemViewModel? sportViewModel =
                  trainerListItemViewModels.FirstOrDefault(y => y.Trainer.Id == trainer.Id);

            if (sportViewModel != null)
            {
                sportViewModel.Update(trainer);
            }
        }

        private void _trainerStore_TrainerAdded(Employee trainer)
        {
            AddTrainer(trainer);
        }

        private void _trainerStore_TrainersLoaded()
        {
            trainerListItemViewModels.Clear();

            foreach (Employee trainer in _employeeStore.Employees)
            {
                AddTrainer(trainer);
            }
        }

        public override void Dispose()
        {

            _employeeStore.Loaded -= _trainerStore_TrainersLoaded;
            _employeeStore.Created -= _trainerStore_TrainerAdded;
            _employeeStore.Updated -= _trainerStore_TrainerUpdated;
            _employeeStore.Deleted -= _trainerStore_TrainerDeleted;
            base.Dispose();
        }





        private void AddTrainer(Employee trainer)
        {
            TrainerListItemViewModel itemViewModel =
                new TrainerListItemViewModel(trainer, _navigatorStore);
            trainerListItemViewModels.Add(itemViewModel);
        }
        public static TrainersListViewModel LoadViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore)
        {
            TrainersListViewModel viewModel = new TrainersListViewModel(navigatorStore, employeeStore);

            viewModel.LoadTrainerCommand.Execute(null);

            return viewModel;
        }

    }
}
