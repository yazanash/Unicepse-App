using Unicepse.Commands;
using Unicepse.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using emp = Unicepse.Core.Models.Employee;
using Unicepse.Commands.Employee;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.Stores.EmployeeStores;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
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
        private readonly LicenseDataStore _licenseDataStore;
        private readonly EmployeeSubscriptionDataStore? _employeeSubscriptionDataStore;
        public IEnumerable<TrainerListItemViewModel> TrainerList => trainerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public ICommand AddTrainerCommand { get; }
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
        public TrainersListViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, LicenseDataStore licenseDataStore, EmployeeSubscriptionDataStore? employeeSubscriptionDataStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _licenseDataStore = licenseDataStore;
            _employeeSubscriptionDataStore = employeeSubscriptionDataStore;

            LoadTrainerCommand = new LoadTrainersCommand(_employeeStore, this);
            AddTrainerCommand = new NavaigateCommand<AddTrainerViewModel>(new NavigationService<AddTrainerViewModel>(_navigatorStore, () => CreateAddTrainerViewModel(navigatorStore, this, _sportDataStore, _employeeStore)));
            AddEmployeeCommand = new NavaigateCommand<AddEmployeeViewModel>(new NavigationService<AddEmployeeViewModel>(_navigatorStore, () => new AddEmployeeViewModel(navigatorStore, this, _employeeStore)));
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
        void LoadEmployees(IEnumerable<emp.Employee> employees)
        {
            trainerListItemViewModels.Clear();

            foreach (emp.Employee employee in employees)
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

            foreach (emp.Employee employee in _employeeStore.Employees.Where(x => x.FullName!.ToLower().Contains(obj!.ToLower())))
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
        }

        private void _trainerStore_TrainerUpdated(emp.Employee trainer)
        {
            TrainerListItemViewModel? sportViewModel =
                  trainerListItemViewModels.FirstOrDefault(y => y.Trainer.Id == trainer.Id);

            if (sportViewModel != null)
            {
                sportViewModel.Update(trainer);
            }
        }

        private void _trainerStore_TrainerAdded(emp.Employee trainer)
        {
            AddTrainer(trainer);
        }

        private void _trainerStore_TrainersLoaded()
        {
            trainerListItemViewModels.Clear();

            foreach (emp.Employee trainer in _employeeStore.Employees)
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





        private void AddTrainer(emp.Employee trainer)
        {
            TrainerListItemViewModel itemViewModel =
                new TrainerListItemViewModel(trainer, _navigatorStore, _employeeStore, _sportDataStore, this, _dausesDataStore, _creditsDataStore,_licenseDataStore, _employeeSubscriptionDataStore);
            trainerListItemViewModels.Add(itemViewModel);
        }
        public static TrainersListViewModel LoadViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore,LicenseDataStore licenseDataStore,EmployeeSubscriptionDataStore employeeSubscriptionDataStore)
        {
            TrainersListViewModel viewModel = new TrainersListViewModel(navigatorStore, employeeStore, sportDataStore, dausesDataStore, creditsDataStore,licenseDataStore, employeeSubscriptionDataStore);

            viewModel.LoadTrainerCommand.Execute(null);

            return viewModel;
        }
        private AddTrainerViewModel CreateAddTrainerViewModel(NavigationStore navigatorStore, TrainersListViewModel trainersListViewModel, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            return AddTrainerViewModel.LoadViewModel(navigatorStore, trainersListViewModel, sportDataStore, employeeStore);
        }
    }
}
