using PlatinumGym.Core.Models;
using PlatinumGym.Core.Models.Player;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.PlayersCommands;
//using PlatinumGymPro.Models;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using PlatinumGymPro.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
  
    public class PlayerListViewModel : ViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly ObservableCollection<FiltersItemViewModel> filtersItemViewModel;
        private readonly ObservableCollection<OrderByItemViewModel> OrderByItemViewModel;
        private NavigationStore _navigatorStore;
        private PlayersDataStore _playerStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        //private TrainerStore _trainerStore;
        //private SportStore _sportStore;
        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public IEnumerable<OrderByItemViewModel> OrderByList => OrderByItemViewModel;
        public ICommand AddPlayerCommand { get; }

        public FiltersItemViewModel? SelectedFilter
        {
            get
            {
                return filtersItemViewModel
                    .FirstOrDefault(y => y?.Filter== _playerStore.SelectedFilter);
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

        public ICommand LoadPlayersCommand { get; }
        public PlayerListViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, SubscriptionDataStore subscriptionStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _subscriptionStore = subscriptionStore;
            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);

            AddPlayerCommand = new NavaigateCommand<AddPlayerViewModel>(new NavigationService<AddPlayerViewModel>(_navigatorStore, () => new AddPlayerViewModel(navigatorStore, this)));
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();


            _playerStore.players_loaded += _playerStore_PlayersLoaded;
            _playerStore.player_created += _playerStore_PlayerAdded;
            _playerStore.player_update += _playerStore_PlayerUpdated;
            _playerStore.player_deleted += _playerStore_PlayerDeleted;
            _playerStore.FilterChanged += _playerStore_FilterChanged;
            _playerStore.OrderChanged += _playerStore_OrderChanged;
            //_playerStore.SelectedFilterChanged += _playerStore_SelectedFilterChanged;
            //_playerStore.SelectedOrderByChanged += _playerStore_SelectedOrderByChanged;

            OrderByItemViewModel = new();

            OrderByItemViewModel.Add(new OrderByItemViewModel( Enums.Order.ByName, 1,  "الاسم" ));
            OrderByItemViewModel.Add(new OrderByItemViewModel( Enums.Order.ByDebt, 2,  "الديون" ));
            OrderByItemViewModel.Add(new OrderByItemViewModel( Enums.Order.BySubscribeEnd, 3,  "منتهي الاشتراك" ));

            SelectedOrderBy = OrderByItemViewModel.FirstOrDefault(x => x.Id == 1);


            filtersItemViewModel = new();

            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.GenderMale, 1, "ذكور"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.GenderFemale, 2, "اناث"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.WithoutTrainer, 3, "بدون مدرب"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.InActive, 4, "غير فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.Active, 6, "فعال"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.SubscribeEnd, 7, "منتهي الاشتراك"));
            filtersItemViewModel.Add(new FiltersItemViewModel(Enums.Filter.HaveDebt, 8, "ديون"));

            SelectedFilter = filtersItemViewModel.FirstOrDefault(x => x.Id == 6);


        }

        private void _playerStore_OrderChanged(Enums.Order? order)
        {
            FilterPlayers(order, _playerStore.SelectedFilter);
        }

        private void _playerStore_FilterChanged(Enums.Filter? filter)
        {
            FilterPlayers(_playerStore.SelectedOrder, filter);
        }
        protected override void Dispose()
        {
            _playerStore.players_loaded -= _playerStore_PlayersLoaded;
            _playerStore.player_created -= _playerStore_PlayerAdded;
            _playerStore.player_update -= _playerStore_PlayerUpdated;
            _playerStore.player_deleted -= _playerStore_PlayerDeleted;
            _playerStore.FilterChanged -= _playerStore_FilterChanged;
            _playerStore.OrderChanged -= _playerStore_OrderChanged;
            base.Dispose();
        }
        private void _playerStore_PlayerDeleted(int id)
        {
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _playerStore_PlayerUpdated(Player player)
        {
            PlayerListItemViewModel? playerViewModel =
                   playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
        }

        private void _playerStore_PlayerAdded(Player player)
        {
            AddPlayer(player);
            PlayersCount++;
            if (!player.GenderMale)
                PlayersFemaleCount++;
            else
                PlayersMaleCount++;
        }

        private void _playerStore_PlayersLoaded()
        {

            FilterPlayers(_playerStore.SelectedOrder, _playerStore.SelectedFilter);
        }
        void FilterPlayers(Enums.Order? order,Enums.Filter? filter)
        {
            playerListItemViewModels.Clear();
            switch (filter)
            {
                case Enums.Filter.GenderMale:
                    LoadPlayers(_playerStore.Players.Where(x => x.GenderMale == true), order);
                    break;
                case Enums.Filter.Active:
                    LoadPlayers(_playerStore.Players.Where(x => x.IsSubscribed == true), order);
                    break;
                case Enums.Filter.InActive:
                    LoadPlayers(_playerStore.Players.Where(x => x.IsSubscribed == false), order);
                    break;
                case Enums.Filter.GenderFemale:
                    LoadPlayers(_playerStore.Players.Where(x => x.GenderMale == false), order);
                    break;
                case Enums.Filter.SubscribeEnd:
                    LoadPlayers(_playerStore.Players.Where(x => x.SubscribeEndDate < DateTime.Now),order);
                    break;
                case Enums.Filter.HaveDebt:
                    LoadPlayers(_playerStore.Players.Where(x => x.Balance < 0), order);
                    break;

            }
        }
        void LoadPlayers(IEnumerable<Player> players,Enums.Order? order)
        {
            playerListItemViewModels.Clear();
            switch (order)
            {
                case Enums.Order.ByName:
                    players = players.OrderBy(x => x.FullName);
                    break;
                case Enums.Order.ByDebt:
                    players = players.OrderByDescending(x => x.Balance);
                    break;
                case Enums.Order.BySubscribeEnd:
                    players = players.OrderByDescending(x => x.SubscribeEndDate);
                    break;
            }
            foreach (Player player in players)
            {
                AddPlayer(player);
            }
            PlayersCount = playerListItemViewModels.Count();
            PlayersFemaleCount = playerListItemViewModels.Where(x => !x.GenderMale).Count();
            PlayersMaleCount = playerListItemViewModels.Where(x => x.GenderMale).Count();

        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new PlayerListItemViewModel(player, _navigatorStore, this, _subscriptionStore);
            playerListItemViewModels.Add(itemViewModel);
        }
        public static PlayerListViewModel LoadViewModel(NavigationStore navigatorStore, PlayersDataStore playersStore, SubscriptionDataStore subscriptionDataStore)
        {
            PlayerListViewModel viewModel = new PlayerListViewModel(navigatorStore, playersStore, subscriptionDataStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
