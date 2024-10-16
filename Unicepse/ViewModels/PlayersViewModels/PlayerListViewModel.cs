using Unicepse.Core.Models;
using Unicepse.Core.Models.Player;
using Unicepse.Commands;
using Unicepse.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.PlayersViewModels
{

    public class PlayerListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly ObservableCollection<FiltersItemViewModel> filtersItemViewModel;
        private readonly ObservableCollection<OrderByItemViewModel> OrderByItemViewModel;
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly SportDataStore _sportStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly LicenseDataStore _licenseDataStore;
        public SearchBoxViewModel SearchBox { get; set; }

        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public IEnumerable<OrderByItemViewModel> OrderByList => OrderByItemViewModel;
        public ICommand AddPlayerCommand { get; }

        public FiltersItemViewModel? SelectedFilter
        {
            get
            {
                return filtersItemViewModel
                    .FirstOrDefault(y => y?.Filter == _playerStore.SelectedFilter);
            }
            set
            {
                _playerStore.SelectedFilter = value?.Filter;

            }
        }
        public OrderByItemViewModel? SelectedOrderBy
        {
            get
            {
                return OrderByItemViewModel
                    .FirstOrDefault(y => y?.OrderBy == _playerStore.SelectedOrder);
            }
            set
            {
                _playerStore.SelectedOrder = value?.OrderBy;

            }
        }


        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return _playerStore.SelectedPlayer;
            }
            set
            {
                _playerStore.SelectedPlayer = value;

            }
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

        public ICommand LoadPlayersCommand { get; }
        public ICommand ArchivedPlayerCommand { get; }
        public PlayerListViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionStore, SportDataStore sportStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, LicenseDataStore licenseDataStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _subscriptionStore = subscriptionStore;
            _sportStore = sportStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _licenseDataStore = licenseDataStore;
            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);

            AddPlayerCommand = new NavaigateCommand<AddPlayerViewModel>(new NavigationService<AddPlayerViewModel>(_navigatorStore, () => new AddPlayerViewModel(navigatorStore, this, _playerStore, _subscriptionStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore,_licenseDataStore)));
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            ArchivedPlayerCommand = new NavaigateCommand<ArchivedPlayersListViewModel>(new NavigationService<ArchivedPlayersListViewModel>(_navigatorStore, () => ArchivedPlayersViewModel(navigatorStore, _playerStore, _subscriptionStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, this,_licenseDataStore)));


            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            _playerStore.Player_created += PlayerStore_PlayerAdded;
            _playerStore.Player_update += PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted += PlayerStore_PlayerDeleted;
            _playerStore.FilterChanged += PlayerStore_FilterChanged;
            _playerStore.OrderChanged += PlayerStore_OrderChanged;


            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;

            OrderByItemViewModel = new();

            OrderByItemViewModel.Add(new OrderByItemViewModel(Order.ByName, 1, "الاسم"));
            OrderByItemViewModel.Add(new OrderByItemViewModel(Order.ByDebt, 2, "الديون"));
            OrderByItemViewModel.Add(new OrderByItemViewModel(Order.BySubscribeEnd, 3, "منتهي الاشتراك"));

            SelectedOrderBy = OrderByItemViewModel.FirstOrDefault(x => x.Id == 1);


            filtersItemViewModel = new();

            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.GenderMale, 1, "ذكور"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.GenderFemale, 2, "اناث"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.WithoutTrainer, 3, "بدون مدرب"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.InActive, 4, "غير فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.Active, 6, "فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.SubscribeEnd, 7, "منتهي الاشتراك"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.HaveDebt, 8, "ديون"));

            SelectedFilter = filtersItemViewModel.FirstOrDefault(x => x.Id == 6);
           
        }
        private ArchivedPlayersListViewModel ArchivedPlayersViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore _metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore,PlayerListViewModel playerListViewModel,LicenseDataStore licenseDataStore)
        {
            return ArchivedPlayersListViewModel.LoadViewModel(navigatorStore, playerStore, subscriptionDataStore, sportDataStore, paymentDataStore, _metricDataStore, routineDataStore, playersAttendenceStore, playerListViewModel,licenseDataStore);
        }
        private void SearchBox_SearchedText(string? obj)
        {
            playerListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadPlayers(_playerStore.Players, obj);
            }
            else
                FilterPlayers(_playerStore.SelectedOrder, _playerStore.SelectedFilter);
        }

        private void PlayerStore_OrderChanged(Order? order)
        {
            FilterPlayers(order, _playerStore.SelectedFilter);
        }

        private void PlayerStore_FilterChanged(utlis.common.Filter? filter)
        {
            FilterPlayers(_playerStore.SelectedOrder, filter);
        }
        public override void Dispose()
        {
            _playerStore.Players_loaded -= PlayerStore_PlayersLoaded;
            _playerStore.Player_created -= PlayerStore_PlayerAdded;
            _playerStore.Player_update -= PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted -= PlayerStore_PlayerDeleted;
            _playerStore.FilterChanged -= PlayerStore_FilterChanged;
            _playerStore.OrderChanged -= PlayerStore_OrderChanged;
            base.Dispose();
        }
        private void PlayerStore_PlayerDeleted(int id)
        {
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void PlayerStore_PlayerUpdated(Player player)
        {
            PlayerListItemViewModel? playerViewModel =
                   playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
        }

        private void PlayerStore_PlayerAdded(Player player)
        {
            AddPlayer(player);
            PlayersCount++;
            if (!player.GenderMale)
                PlayersFemaleCount++;
            else
                PlayersMaleCount++;
        }

        private void PlayerStore_PlayersLoaded()
        {

            FilterPlayers(_playerStore.SelectedOrder, _playerStore.SelectedFilter);
        }
        void FilterPlayers(Order? order, utlis.common.Filter? filter)
        {
            playerListItemViewModels.Clear();
            switch (filter)
            {
                case utlis.common.Filter.GenderMale:
                    LoadPlayers(_playerStore.Players.Where(x => x.GenderMale == true), order);
                    break;
                case utlis.common.Filter.Active:
                    LoadPlayers(_playerStore.Players.Where(x => x.SubscribeEndDate > DateTime.Now), order);
                    break;
                case utlis.common.Filter.InActive:
                    LoadPlayers(_playerStore.Players.Where(x => x.SubscribeEndDate < DateTime.Now), order);
                    break;
                case utlis.common.Filter.GenderFemale:
                    LoadPlayers(_playerStore.Players.Where(x => x.GenderMale == false), order);
                    break;
                case utlis.common.Filter.SubscribeEnd:
                    LoadPlayers(_playerStore.Players.Where(x => x.SubscribeEndDate < DateTime.Now), order);
                    break;
                case utlis.common.Filter.HaveDebt:
                    LoadPlayers(_playerStore.Players.Where(x => x.Balance < 0), order);
                    break;

            }
        }
        void LoadPlayers(IEnumerable<Player> players, Order? order)
        {
            playerListItemViewModels.Clear();
            switch (order)
            {
                case Order.ByName:
                    players = players.OrderBy(x => x.FullName);
                    break;
                case Order.ByDebt:
                    players = players.OrderByDescending(x => x.Balance);
                    break;
                case Order.BySubscribeEnd:
                    players = players.OrderByDescending(x => x.SubscribeEndDate);
                    break;
            }
            foreach (Player player in players)
            {
                AddPlayer(player);
            }
            PlayersCount = playerListItemViewModels.Count;
            PlayersFemaleCount = playerListItemViewModels.Where(x => !x.GenderMale).Count();
            PlayersMaleCount = playerListItemViewModels.Where(x => x.GenderMale).Count();

        }
        void LoadPlayers(IEnumerable<Player> players, string query)
        {
            playerListItemViewModels.Clear();

            foreach (Player player in players.Where(x => x.FullName!.ToLower().Contains(query.ToLower())))
            {
                AddPlayer(player);
            }
            PlayersCount = playerListItemViewModels.Count;
            PlayersFemaleCount = playerListItemViewModels.Where(x => !x.GenderMale).Count();
            PlayersMaleCount = playerListItemViewModels.Where(x => x.GenderMale).Count();

        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new(player, _navigatorStore, _subscriptionStore, _playerStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, this, _playersAttendenceStore,_licenseDataStore);
            playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = playerListItemViewModels.Count();
        }
        public static PlayerListViewModel LoadViewModel(NavigationStore navigatorStore, PlayersDataStore playersStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore,LicenseDataStore licenseDataStore)
        {
            PlayerListViewModel viewModel = new(navigatorStore, playersStore, subscriptionDataStore, sportDataStore, paymentDataStore, metricDataStore, routineDataStore, playersAttendenceStore,licenseDataStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
