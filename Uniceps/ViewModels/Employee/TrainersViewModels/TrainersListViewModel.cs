using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Employee;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.EmployeeStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.EmployeeViews;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainersListViewModel : ListingViewModelBase
    {

        private readonly ObservableCollection<TrainerListItemViewModel> trainerListItemViewModels;
        private readonly ObservableCollection<FiltersItemViewModel> filtersItemViewModel;
        private NavigationStore _navigatorStore;
        private EmployeeStore _employeeStore;
        private SportDataStore _sportDataStore;
        private DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly EmployeeSubscriptionDataStore? _employeeSubscriptionDataStore;
        private readonly AccountStore _accountStore;
        public bool HasData => trainerListItemViewModels.Count > 0;
        public IEnumerable<TrainerListItemViewModel> TrainerList => trainerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public ICommand AddTrainerCommand { get; }
        private void ExecuteAddTrainerCommand()
        {
            AddTrainerViewModel addTrainerViewModel = AddTrainerViewModel.LoadViewModel(_sportDataStore, _employeeStore);
            TrainerDetailsWindowView trainerDetailsWindow = new TrainerDetailsWindowView();
            trainerDetailsWindow.DataContext = addTrainerViewModel;
            trainerDetailsWindow.ShowDialog();
        }
        private void ExecuteAddEmployeeCommand()
        {
            AddEmployeeViewModel employeeViewModel = new AddEmployeeViewModel(_employeeStore);
            EmployeeDetailsWindowView employeeDetailsWindowView = new EmployeeDetailsWindowView();
            employeeDetailsWindowView.DataContext = employeeViewModel;
            employeeDetailsWindowView.ShowDialog();
        }
        public ICommand AddEmployeeCommand { get; }
        public ICommand LoadTrainerCommand { get; }
        public FiltersItemViewModel? SelectedFilter
        {
            get
            {
                return filtersItemViewModel
                    .FirstOrDefault(y => y?.Filter == _employeeStore.SelectedFilter);
            }
            set
            {
                _employeeStore.SelectedFilter = value?.Filter;

            }
        }
        public SearchBoxViewModel SearchBox { get; set; }
        public TrainersListViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, EmployeeSubscriptionDataStore? employeeSubscriptionDataStore, AccountStore accountStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;
            _accountStore = accountStore;

            LoadTrainerCommand = new LoadTrainersCommand(_employeeStore, this);
            AddTrainerCommand = new RelayCommand(ExecuteAddTrainerCommand);
            AddEmployeeCommand = new RelayCommand(ExecuteAddEmployeeCommand);
            trainerListItemViewModels = new ObservableCollection<TrainerListItemViewModel>();



            _employeeStore.Loaded += _trainerStore_TrainersLoaded;
            _employeeStore.Created += _trainerStore_TrainerAdded;
            _employeeStore.Updated += _trainerStore_TrainerUpdated;
            _employeeStore.Deleted += _trainerStore_TrainerDeleted;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;


            filtersItemViewModel = new();

            filtersItemViewModel.Add(new FiltersItemViewModel(Filter.All, 1, "الكل"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Filter.Trainer, 2, "المدربين"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Filter.Secretary, 3, "السكرتارية"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Filter.Employee, 3, "الموظفين"));

            _employeeStore.FilterChanged += _employeeStore_FilterChanged;
        }

        private void _employeeStore_FilterChanged(Filter? filter)
        {

            switch (filter)
            {
                case Filter.All:
                    LoadEmployees(_employeeStore.Employees);
                    break;
                case Filter.Trainer:
                    LoadEmployees(_employeeStore.Employees.Where(x => x.IsTrainer == true));
                    break;
                case Filter.Secretary:
                    LoadEmployees(_employeeStore.Employees.Where(x => x.IsSecrtaria == true));
                    break;
                case Filter.Employee:
                    LoadEmployees(_employeeStore.Employees.Where(x => x.IsSecrtaria == false && x.IsTrainer == false));
                    break;


            }
        }
        void LoadEmployees(IEnumerable<Core.Models.Employee.Employee> employees)
        {
            trainerListItemViewModels.Clear();

            foreach (Core.Models.Employee.Employee employee in employees)
            {
                AddTrainer(employee);
            }


        }
        public TrainerListItemViewModel? SelectedEmployee
        {
            get
            {
                return TrainerList
                    .FirstOrDefault(y => y?.Trainer == _employeeStore.SelectedEmployee);
            }
            set
            {
                _employeeStore.SelectedEmployee = value?.Trainer;

            }
        }
        private void SearchBox_SearchedText(string? obj)
        {
            trainerListItemViewModels.Clear();

            foreach (Core.Models.Employee.Employee employee in _employeeStore.Employees.Where(x => x.FullName!.ToLower().Contains(obj!.ToLower())))
            {
                AddTrainer(employee);
            }
        }

        private void _trainerStore_TrainerDeleted(int id)
        {
            TrainerListItemViewModel? itemViewModel = trainerListItemViewModels.FirstOrDefault(y => y.Trainer?.Id == id);

            if (itemViewModel != null)
            {
                trainerListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _trainerStore_TrainerUpdated(Core.Models.Employee.Employee trainer)
        {
            TrainerListItemViewModel? sportViewModel =
                  trainerListItemViewModels.FirstOrDefault(y => y.Trainer.Id == trainer.Id);

            if (sportViewModel != null)
            {
                sportViewModel.Update(trainer);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _trainerStore_TrainerAdded(Core.Models.Employee.Employee trainer)
        {
            AddTrainer(trainer);
        }

        private void _trainerStore_TrainersLoaded()
        {
            trainerListItemViewModels.Clear();

            foreach (Core.Models.Employee.Employee trainer in _employeeStore.Employees)
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





        private void AddTrainer(Core.Models.Employee.Employee trainer)
        {
            TrainerListItemViewModel itemViewModel =
                new TrainerListItemViewModel(trainer, _navigatorStore, _employeeStore, _sportDataStore, this, _dausesDataStore, _creditsDataStore, _employeeSubscriptionDataStore, _accountStore);
            trainerListItemViewModels.Add(itemViewModel);
            OnPropertyChanged(nameof(HasData));
        }
        public static TrainersListViewModel LoadViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, EmployeeSubscriptionDataStore employeeSubscriptionDataStore,AccountStore accountStore)
        {
            TrainersListViewModel viewModel = new TrainersListViewModel(navigatorStore, employeeStore, sportDataStore, dausesDataStore, creditsDataStore, employeeSubscriptionDataStore, accountStore);

            viewModel.LoadTrainerCommand.Execute(null);

            return viewModel;
        }
        private AddTrainerViewModel CreateAddTrainerViewModel( SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            return AddTrainerViewModel.LoadViewModel(sportDataStore, employeeStore);
        }
    }
}
