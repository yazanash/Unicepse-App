using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Subscription;
using Uniceps.Stores;
using Uniceps.Stores.SportStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Commands.Sport;

namespace Uniceps.ViewModels.SportsViewModels
{
    public class SportSubscriptionsViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly ObservableCollection<TrainersListItemViewModel> _trainersListItemViewModels;
        private readonly SportDataStore _sportDataStore;
        private readonly SportSubscriptionDataStore _subscriptionDataStore;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public IEnumerable<TrainersListItemViewModel> TrainersList => _trainersListItemViewModels;
        public SearchBoxViewModel SearchBox { get; set; }

        public SportSubscriptionsViewModel(SportDataStore sportDataStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            _sportDataStore = sportDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _trainersListItemViewModels = new ObservableCollection<TrainersListItemViewModel>();
            foreach (var trainer in _sportDataStore.Trainers)
            {
                TrainersListItemViewModel listItemViewModel = new TrainersListItemViewModel(trainer);
                _trainersListItemViewModels.Add(listItemViewModel);
            }
            SelectedTrainer = _trainersListItemViewModels.FirstOrDefault();
            _sportDataStore.TrainerChanged += _sportDataStore_TrainerChanged;
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            LoadSubscriptionsForSport = new LoadSubscriptionsForSport(_sportDataStore, _subscriptionDataStore);
        }

        private void SearchBox_SearchedText(string? obj)
        {
            _subscriptionListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                SearchSubscriptionsByTrainer(SelectedTrainer!.trainer, obj);

            }
            else
                LoadSubscriptionsByTrainer(SelectedTrainer!.trainer);

        }

        public TrainersListItemViewModel? SelectedTrainer
        {
            get
            {
                return _trainersListItemViewModels
                    .FirstOrDefault(y => y.trainer.Id == _sportDataStore.SelectedTrainer!.Id);
            }
            set
            {
                _sportDataStore.SelectedTrainer = value?.trainer;

            }
        }
        public void LoadSubscriptionsByTrainer(Core.Models.Employee.Employee? obj)
        {
            if (obj!.Id == -1)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions);
            }
            else if (obj!.Id == -2)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x => x.TrainerId == null));
            }
            else if (obj != null)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x =>  x.TrainerId == obj.Id));
            }
        }
        public void SearchSubscriptionsByTrainer(Core.Models.Employee.Employee? obj, string query)
        {
            if (SelectedTrainer!.trainer.Id == -1)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions, query);
            }
            else if (SelectedTrainer!.trainer!.Id == -2)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x => x.TrainerId == null), query);
            }
            else if (SelectedTrainer!.trainer != null)
            {
                LoadSubscriptions(_subscriptionDataStore.Subscriptions.Where(x => x.TrainerId == SelectedTrainer!.trainer.Id), query);
            }
        }
        private void _sportDataStore_TrainerChanged(Core.Models.Employee.Employee? obj)
        {
            LoadSubscriptionsByTrainer(obj);

        }

        private void _subscriptionDataStore_Loaded()
        {
            LoadSubscriptions(_subscriptionDataStore.Subscriptions);
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel subscriptionListItemViewModel = new(subscription);
            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
            subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsForSport { get; }
        public static SportSubscriptionsViewModel LoadViewModel(SportDataStore sportDataStore, SportSubscriptionDataStore subscriptionDataStore)
        {
            SportSubscriptionsViewModel viewModel = new SportSubscriptionsViewModel(sportDataStore, subscriptionDataStore);

            viewModel.LoadSubscriptionsForSport.Execute(null);

            return viewModel;
        }
        void LoadSubscriptions(IEnumerable<Subscription> subscriptions)
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions)
            {
                AddSubscription(subscription);
            }
            //PlayersCount = _subscriptionListItemViewModels.Count;
            //PlayersFemaleCount = _subscriptionListItemViewModels.Where(x => !x.Subscription.Player!.GenderMale).Count();
            //PlayersMaleCount = _subscriptionListItemViewModels.Where(x => x.Subscription.Player!.GenderMale).Count();

        }
        void LoadSubscriptions(IEnumerable<Subscription> subscriptions, string query)
        {
            _subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in subscriptions.Where(x => x.PlayerName!.Contains(query)))
            {
                AddSubscription(subscription);
            }
            //PlayersCount = _subscriptionListItemViewModels.Count;
            //PlayersFemaleCount = _subscriptionListItemViewModels.Where(x => !x.Subscription.Player!.GenderMale).Count();
            //PlayersMaleCount = _subscriptionListItemViewModels.Where(x => x.Subscription.Player!.GenderMale).Count();

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
