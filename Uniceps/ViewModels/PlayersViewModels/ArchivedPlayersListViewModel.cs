using Uniceps.Core.Models;
using Uniceps.Core.Models.Player;
using Uniceps.Commands;
using Uniceps.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands.Player;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;
using Uniceps.utlis.common;
using Uniceps.Stores;
using Uniceps.navigation.Stores;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class ArchivedPlayersListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly ObservableCollection<FiltersItemViewModel> filtersItemViewModel;
        private readonly ObservableCollection<OrderByItemViewModel> OrderByItemViewModel;
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayerProfileViewModel _playerProfileViewModel;

        public SearchBoxViewModel SearchBox { get; set; }
        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public IEnumerable<OrderByItemViewModel> OrderByList => OrderByItemViewModel;
        public ICommand BackCommand { get; }
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
                    .FirstOrDefault(x => x.Player.Id == _playerStore.SelectedPlayer!.Id);
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
        public ArchivedPlayersListViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, PlayerListViewModel playerListViewModel,
             PlayerProfileViewModel playerProfileViewModel)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            LoadPlayersCommand = new LoadArchivedPlayersCommand(this, _playerStore);

            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();


            _playerStore.ArchivedPlayers_loaded += PlayerStore_PlayersLoaded;
            _playerStore.ArchivedPlayer_created += PlayerStore_PlayerAdded;
            _playerStore.Player_update += PlayerStore_PlayerUpdated;
            _playerStore.ArchivedPlayer_restored += _playerStore_ArchivedPlayer_restored;
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
            BackCommand = new NavaigateCommand<PlayerListViewModel>(new NavigationService<PlayerListViewModel>(_navigatorStore, () => playerListViewModel));
            _playerProfileViewModel = playerProfileViewModel;
        }

        private void _playerStore_ArchivedPlayer_restored(Player obj)
        {
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == obj.Id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void SearchBox_SearchedText(string? obj)
        {
            playerListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadPlayers(_playerStore.ArchivedPlayers, obj);
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
            _playerStore.ArchivedPlayers_loaded -= PlayerStore_PlayersLoaded;
            _playerStore.ArchivedPlayer_created -= PlayerStore_PlayerAdded;
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
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.GenderMale == true), order);
                    break;
                case utlis.common.Filter.Active:
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.SubscribeEndDate >= DateTime.Now.AddDays(-1)), order);
                    break;
                case utlis.common.Filter.InActive:
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.SubscribeEndDate < DateTime.Now), order);
                    break;
                case utlis.common.Filter.GenderFemale:
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.GenderMale == false), order);
                    break;
                case utlis.common.Filter.SubscribeEnd:
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.SubscribeEndDate < DateTime.Now), order);
                    break;
                case utlis.common.Filter.HaveDebt:
                    LoadPlayers(_playerStore.ArchivedPlayers.Where(x => x.Balance < 0), order);
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
                 new(player, _navigatorStore, _playerProfileViewModel);
            playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = playerListItemViewModels.Count();
        }
        public static ArchivedPlayersListViewModel LoadViewModel(NavigationStore navigatorStore,
            PlayersDataStore playersStore, PlayerListViewModel playerListViewModel,
            PlayerProfileViewModel playerProfileViewModel)
        {
            ArchivedPlayersListViewModel viewModel = new(navigatorStore, playersStore, playerListViewModel, playerProfileViewModel);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
