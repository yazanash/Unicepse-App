using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class SportSubscriptionsViewModel : ListingViewModelBase
    {
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionList = new();
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionsList => _subscriptionList;

        public ObservableCollection<SportListItemViewModel> SportsList { get; set; } = new();
        public ObservableCollection<SubscriptionTrainerListItem> TrainersList { get; set; } = new();

        public SportSubscriptionsViewModel(SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, EmployeeStore employeeStore)
        {
            _subscriptionDataStore = subscriptionDataStore;
            _sportDataStore = sportDataStore;
            _employeeStore = employeeStore;
            _subscriptionDataStore.ReportLoaded += _subscriptionDataStore_ReportLoaded;
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            _employeeStore.Loaded += _employeeStore_Loaded;
            LoadSportsCommand = new AsyncRelayCommand(ExecuteLoadSports);
            LoadTrainersCommand = new AsyncRelayCommand(ExecuteLoadTrainers);
            LoadSubscriptionQuery = new AsyncRelayCommand(ExecuteLoadReport);
            LoadTrainersCommand.Execute(null);
            LoadSportsCommand.Execute(null);
            LoadSubscriptionQuery.Execute(null);
        }

        private void _subscriptionDataStore_ReportLoaded()
        {
            foreach (Subscription subscription in _subscriptionDataStore.SubscriptionReport)
            {
                AddSubscription(subscription);
            }
            OnPropertyChanged(nameof(HasData));
        }
        public bool HasData => _subscriptionList.Count > 0;
        private void _employeeStore_Loaded()
        {
            TrainersList.Clear();
            foreach (var emp in _employeeStore.Employees.Where(x => x.IsTrainer))
            {
                TrainersList.Add(new SubscriptionTrainerListItem(emp));
            }
        }

        private void _sportDataStore_Loaded()
        {
            SportsList.Clear();
            foreach (Sport sport in _sportDataStore.Sports)
            {
                SportsList.Add(new SportListItemViewModel(sport));
            }
        }
        private async Task ExecuteLoadTrainers()
        {
            await _employeeStore.GetAll();
        }

        private async Task ExecuteLoadSports()
        {
            await _sportDataStore.GetAll();
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new SubscriptionListItemViewModel(subscription);
            _subscriptionList.Add(itemViewModel);
            itemViewModel.Order = _subscriptionList.Count();
            OnPropertyChanged(nameof(HasData));
        }
        private DateTime _date =DateTime.Now;
        public DateTime Date
        {
            get => _date;
            set { _date = value; OnPropertyChanged(nameof(Date)); LoadSubscriptionQuery.Execute(null); }
        }
        public ICommand ClearFiltersCommand => new RelayCommand(ExecuteClearFilters);
        private bool _isClearingFilters = false;
        private void ExecuteClearFilters()
        {
            _isClearingFilters = true;
            SelectedSport = null;
            SelectedTrainer = null;
            _isClearingFilters = false;
            LoadSubscriptionQuery.Execute(null);
        }
        private SubscriptionTrainerListItem? _selectedTrainer;
        public SubscriptionTrainerListItem? SelectedTrainer
        {
            get { return _selectedTrainer; }
            set
            {
                _selectedTrainer = value; OnPropertyChanged(nameof(SelectedTrainer));
                if (!_isClearingFilters)
                {
                    LoadSubscriptionQuery.Execute(null);
                }
            }
        }
        private SportListItemViewModel? _selectedSport;
        public SportListItemViewModel? SelectedSport
        {
            get { return _selectedSport; }
            set
            {
                _selectedSport = value; OnPropertyChanged(nameof(SelectedSport));
                if (!_isClearingFilters)
                {
                    LoadSubscriptionQuery.Execute(null);
                }
            }
        }
        private async Task ExecuteLoadReport()
        {
            int? trainerId = SelectedTrainer?.Id;
            int? sportId = SelectedSport?.Id;
            await _subscriptionDataStore.GetAllByDate(Date, sportId, trainerId);
        }

        public ICommand LoadSubscriptionQuery { get; }
        public ICommand LoadSportsCommand { get; }
        public ICommand LoadTrainersCommand { get; }

    }
}
