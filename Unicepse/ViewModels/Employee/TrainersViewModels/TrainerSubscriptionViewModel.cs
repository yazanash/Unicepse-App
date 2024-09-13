using Unicepse.Core.Models.Subscription;
using Unicepse.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.ViewModels.SportsViewModels;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class TrainerSubscriptionViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly ObservableCollection<SportListItemViewModel> _sportListItemViewModels;

        private readonly EmployeeStore _employeeStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public IEnumerable<SportListItemViewModel> SportList => _sportListItemViewModels;
        public SearchBoxViewModel SearchBox { get; set; }
        public TrainerSubscriptionViewModel(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            _employeeStore = employeeStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _sportListItemViewModels = new ObservableCollection<SportListItemViewModel>();
            foreach (var sport in _employeeStore.Sports)
            {
                SportListItemViewModel listItemViewModel = new SportListItemViewModel(sport);
                _sportListItemViewModels.Add(listItemViewModel);
            }
            SelectedSport = _sportListItemViewModels.FirstOrDefault();
            _employeeStore.SportChanged += _employeeStore_SportChanged;
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;

            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            LoadSubscriptionsForTrainer = new LoadSubscriptionnForTrainer(_employeeStore, _subscriptionDataStore);
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _subscriptionListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                SearchSubscriptionsBySport(SelectedSport!.Sport, obj);

            }
            else
                LoadSubscriptionsBySport(SelectedSport!.Sport);

        }
        public void LoadSubscriptionsBySport(Core.Models.Sport.Sport? obj)
        {
            if (obj!.Id == -1)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions);
            }
            else if (obj != null)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x => x.Sport != null && x.Sport!.Id == obj.Id));
            }
        }
        public void SearchSubscriptionsBySport(Core.Models.Sport.Sport? obj, string query)
        {
            if (SelectedSport!.Sport.Id == -1)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions, query);
            }
            else if (SelectedSport!.Sport != null)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x => x.Sport != null && x.Sport!.Id == SelectedSport!.Sport.Id), query);
            }
        }
        public SportListItemViewModel? SelectedSport
        {
            get
            {
                return _sportListItemViewModels
                    .FirstOrDefault(y => y.Sport.Id == _employeeStore.SelectedSport!.Id);
            }
            set
            {
                _employeeStore.SelectedSport = value?.Sport;

            }
        }
        private void _employeeStore_SportChanged(Core.Models.Sport.Sport? obj)
        {
            LoadSubscriptionsBySport(obj);
        }
        void LoadSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions)
            {
                AddSubscription(subscription);
            }
            PlayersCount = _subscriptionListItemViewModels.Count;
            PlayersFemaleCount = _subscriptionListItemViewModels.Where(x => !x.Subscription.Player!.GenderMale).Count();
            PlayersMaleCount = _subscriptionListItemViewModels.Where(x => x.Subscription.Player!.GenderMale).Count();

        }
        void LoadSubscriptions(IEnumerable<Subscription> subscriptions, string query)
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions.Where(x => x.Player!.FullName!.Contains(query)))
            {
                AddSubscription(subscription);
            }
            PlayersCount = _subscriptionListItemViewModels.Count;
            PlayersFemaleCount = _subscriptionListItemViewModels.Where(x => !x.Subscription.Player!.GenderMale).Count();
            PlayersMaleCount = _subscriptionListItemViewModels.Where(x => x.Subscription.Player!.GenderMale).Count();

        }
        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();
            foreach (var subs in _subscriptionDataStore.Subscriptions)
                AddSubscription(subs);
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel subscriptionListItemViewModel = new(subscription);
            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
            subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsForTrainer { get; }
        public static TrainerSubscriptionViewModel LoadViewModel(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            TrainerSubscriptionViewModel viewModel = new TrainerSubscriptionViewModel(employeeStore, subscriptionDataStore);

            viewModel.LoadSubscriptionsForTrainer.Execute(null);

            return viewModel;
        }
        private int _playersCount;
        public int PlayersCount
        {
            get
            {
                return _playersCount;
            }
            set
            {
                _playersCount = value;
                OnPropertyChanged(nameof(PlayersCount));
            }
        }
        private int _playersFemaleCount;
        public int PlayersFemaleCount
        {
            get
            {
                return _playersFemaleCount;
            }
            set
            {
                _playersFemaleCount = value;
                OnPropertyChanged(nameof(PlayersFemaleCount));
            }
        }
        private int _playersMaleCount;
        public int PlayersMaleCount
        {
            get
            {
                return _playersMaleCount;
            }
            set
            {
                _playersMaleCount = value;
                OnPropertyChanged(nameof(PlayersMaleCount));
            }
        }
    }
}
