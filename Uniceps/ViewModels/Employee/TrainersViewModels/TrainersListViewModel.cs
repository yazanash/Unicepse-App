using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Employee;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.Views.EmployeeViews;

namespace Uniceps.ViewModels.Employee.TrainersViewModels
{
    public class TrainersListViewModel : ListingViewModelBase
    {

        private readonly ObservableCollection<TrainerListItemViewModel> trainerListItemViewModels;
        private NavigationStore _navigatorStore;
        private EmployeeStore _employeeStore;
        private SportDataStore _sportDataStore;
        private DausesDataStore _dausesDataStore;
        private readonly CreditsDataStore _creditsDataStore;
        private readonly AccountStore _accountStore;
        private readonly LicenseStore _licenseStore;
        public bool HasData => trainerListItemViewModels.Count > 0;
        public ICollectionView TrainerList { get; set; }
        public List<Filter> FiltersList { get; set; } = new();
        public ICommand AddTrainerCommand { get; }
        private void ExecuteAddTrainerCommand()
        {
            AddTrainerViewModel addTrainerViewModel = new AddTrainerViewModel(_employeeStore);
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
        public Filter? _selectedFilter;
        public Filter? SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                TrainerList.Refresh();
            }
        }
        public SearchBoxViewModel SearchBox { get; set; }
        public TrainersListViewModel(NavigationStore navigatorStore, EmployeeStore employeeStore, SportDataStore sportDataStore, DausesDataStore dausesDataStore, CreditsDataStore creditsDataStore, AccountStore accountStore, LicenseStore licenseStore)
        {
            _navigatorStore = navigatorStore;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _dausesDataStore = dausesDataStore;
            _creditsDataStore = creditsDataStore;
            _accountStore = accountStore;

            LoadTrainerCommand = new LoadTrainersCommand(_employeeStore, this);
            AddTrainerCommand = new RelayCommand(ExecuteAddTrainerCommand);
            AddEmployeeCommand = new RelayCommand(ExecuteAddEmployeeCommand);
            trainerListItemViewModels = new ObservableCollection<TrainerListItemViewModel>();
            TrainerList = CollectionViewSource.GetDefaultView(trainerListItemViewModels);
            TrainerList.Filter = FilterTrainers;
            foreach (var item in Enum.GetValues<Filter>())
            {
                FiltersList.Add(item);
            }
            _employeeStore.Loaded += _trainerStore_TrainersLoaded;
            _employeeStore.Created += _trainerStore_TrainerAdded;
            _employeeStore.Updated += _trainerStore_TrainerUpdated;
            _employeeStore.Deleted += _trainerStore_TrainerDeleted;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;

            _licenseStore = licenseStore;
            LoadTrainerCommand.Execute(null);
        }
        private bool FilterTrainers(object item)
        {
            if (item is TrainerListItemViewModel playerVM)
            {
                // أولاً: فحص نص البحث بـ SearchBox
                if (!string.IsNullOrWhiteSpace(SearchBox.SearchText))
                {
                    bool matchesSearch = playerVM.FullName != null &&
                                         playerVM.FullName.Contains(SearchBox.SearchText, StringComparison.OrdinalIgnoreCase);

                    if (!matchesSearch) return false;
                }

                if (SelectedFilter != null)
                {
                    if (playerVM.Trainer == null) return false;

                    var filterType = SelectedFilter.Value;

                    if (filterType == Filter.Employee && (playerVM.Trainer.IsSecrtaria || playerVM.Trainer.IsTrainer))
                        return false;

                    if (filterType == Filter.Trainer && !playerVM.Trainer.IsTrainer)
                        return false;

                    if (filterType == Filter.Secretary && !playerVM.Trainer.IsSecrtaria)
                        return false;
                }
                return true;
            }
            return false;
        }
       
        private void SearchBox_SearchedText(string? obj)
        {
            TrainerList.Refresh();
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
                new TrainerListItemViewModel(trainer, _navigatorStore, _employeeStore, this, _dausesDataStore, _creditsDataStore, _accountStore, _licenseStore);
            trainerListItemViewModels.Add(itemViewModel);
            OnPropertyChanged(nameof(HasData));
        }
       
    }
}
