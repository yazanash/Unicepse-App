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
using Microsoft.Extensions.Logging;

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
        private readonly NavigationService<PlayerListViewModel> _navigationService;
        private readonly ILogger _logger;
        string Flags = "[PL] ";
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
                return PlayerList
                   .FirstOrDefault(y => y?.Player == _playerStore.SelectedPlayer);
            }
            set
            {
                _playerStore.SelectedPlayer = value?.Player;

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
        public PlayerListViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionStore, SportDataStore sportStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService, ILogger logger)
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
            _navigationService = navigationService;
            _logger = logger;
            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore,_logger);
            AddPlayerCommand = new NavaigateCommand<AddPlayerViewModel>(new NavigationService<AddPlayerViewModel>(_navigatorStore, () => new AddPlayerViewModel(navigatorStore, this, _playerStore, _subscriptionStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, _licenseDataStore, _navigationService)));
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            ArchivedPlayerCommand = new NavaigateCommand<ArchivedPlayersListViewModel>(new NavigationService<ArchivedPlayersListViewModel>(_navigatorStore, () => ArchivedPlayersViewModel(navigatorStore, _playerStore, _subscriptionStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore, this, _licenseDataStore, _navigationService)));
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            _playerStore.Player_created += PlayerStore_PlayerAdded;
            _playerStore.Player_update += PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted += PlayerStore_PlayerDeleted;
            _playerStore.ArchivedPlayer_restored += playerStore_ArchivedPlayer_restored;
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
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.InActive, 4, "غير فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.Active, 6, "فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.SubscribeEnd, 7, "منتهي الاشتراك"));
            filtersItemViewModel.Add(new FiltersItemViewModel(utlis.common.Filter.HaveDebt, 8, "ديون"));
            SelectedFilter = filtersItemViewModel.FirstOrDefault(x => x.Id == 6);
            _logger.LogInformation(Flags + "view model loaded");

        }

        private void playerStore_ArchivedPlayer_restored(Player obj)
        {
            _logger.LogInformation(Flags + "archived player resored event");

            AddPlayer(obj);
        }

        private ArchivedPlayersListViewModel ArchivedPlayersViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore _metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore,PlayerListViewModel playerListViewModel,LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService)
        {
            return ArchivedPlayersListViewModel.LoadViewModel(navigatorStore, playerStore, subscriptionDataStore, sportDataStore, paymentDataStore, _metricDataStore, routineDataStore, playersAttendenceStore, playerListViewModel,licenseDataStore,navigationService);
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _logger.LogInformation(Flags+"search text changed");

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
            _logger.LogInformation(Flags + "order changed");
            FilterPlayers(order, _playerStore.SelectedFilter);
        }

        private void PlayerStore_FilterChanged(utlis.common.Filter? filter)
        {
            _logger.LogInformation(Flags + "filter changed");
            FilterPlayers(_playerStore.SelectedOrder, filter);
        }
        public override void Dispose()
        {
            _logger.LogInformation(Flags + "dispose");
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
            _logger.LogInformation(Flags + "player deleted");
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void PlayerStore_PlayerUpdated(Player player)
        {
            _logger.LogInformation(Flags + "player updated");
            PlayerListItemViewModel? playerViewModel =
                   playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
        }

        private void PlayerStore_PlayerAdded(Player player)
        {
            _logger.LogInformation(Flags + "player added");
            AddPlayer(player);
            PlayersCount++;
            if (!player.GenderMale)
                PlayersFemaleCount++;
            else
                PlayersMaleCount++;
        }

        private void PlayerStore_PlayersLoaded()
        {
            _logger.LogInformation(Flags + "players loaded event");
            FilterPlayers(_playerStore.SelectedOrder, _playerStore.SelectedFilter);
        }
        void FilterPlayers(Order? order, utlis.common.Filter? filter)
        {
            _logger.LogInformation(Flags + "filter players");
            playerListItemViewModels.Clear();
            switch (filter)
            {
                case utlis.common.Filter.GenderMale:
                    LoadPlayers(_playerStore.Players.Where(x => x.GenderMale == true), order);
                    break;
                case utlis.common.Filter.Active:
                    LoadPlayers(_playerStore.Players.Where(x => x.SubscribeEndDate >= DateTime.Now.AddDays(-1)), order);
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
            _logger.LogInformation(Flags + "load players");
            playerListItemViewModels.Clear();
            switch (order)
            {
                case Order.ByName:
                    players = players.OrderBy(x => x.FullName);
                    break;
                case Order.ByDebt:
                    players = players.OrderBy(x => x.Balance);
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
            _logger.LogInformation(Flags + "search player loaded");
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
            _logger.LogInformation(Flags + "add Player list item model");

            PlayerListItemViewModel itemViewModel =
                new(player, _navigatorStore, _subscriptionStore, _playerStore, _sportStore, _paymentDataStore, _metricDataStore, _routineDataStore, _playersAttendenceStore,_licenseDataStore,_navigationService);
            playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = playerListItemViewModels.Count();
        }
        public static PlayerListViewModel LoadViewModel(NavigationStore navigatorStore, PlayersDataStore playersStore, SubscriptionDataStore subscriptionDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, PlayersAttendenceStore playersAttendenceStore,LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService,ILogger<PlayerListViewModel> logger)
        {
            logger.LogInformation("[PL] view model loaded");


            PlayerListViewModel viewModel = new(navigatorStore, playersStore, subscriptionDataStore, sportDataStore, paymentDataStore, metricDataStore, routineDataStore, playersAttendenceStore,licenseDataStore,navigationService, logger);
            logger.LogInformation("[PL] execute command");
            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
