using PlatinumGymPro.Commands;
using PlatinumGymPro.Commands.PlayersCommands;
using PlatinumGymPro.Models;
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
        private PlayerStore _playerStore;
        private TrainerStore _trainerStore;
        private SportStore _sportStore;
        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public IEnumerable<OrderByItemViewModel> OrderByList => OrderByItemViewModel;
        public ICommand AddPlayerCommand { get; }
       
        public FiltersItemViewModel? SelectedFilter
        {
            get
            {
                return filtersItemViewModel
                    .FirstOrDefault(y => y?.Id == _playerStore.SelectedFilters?.Id);
            }
            set
            {
                _playerStore.SelectedFilters = value?.Filter;

            }
        }
        public OrderByItemViewModel? SelectedOrderBy
        {
            get
            {
                return OrderByItemViewModel
                    .FirstOrDefault(y => y?.Id == _playerStore.SelectedOrderBy?.Id);
            }
            set
            {
                _playerStore.SelectedOrderBy = value?.OrderBy;

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
        public PlayerListViewModel(NavigationStore navigatorStore, PlayerStore playerStore, TrainerStore trainerStore, SportStore sportStore)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _trainerStore = trainerStore;
            _sportStore = sportStore;
            LoadPlayersCommand = new LoadPlayersCommand(_playerStore, this);
           
            AddPlayerCommand = new NavaigateCommand<AddPlayerViewModel>(new NavigationService<AddPlayerViewModel>(_navigatorStore, () => new AddPlayerViewModel(navigatorStore, _playerStore, this)));
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();


            _playerStore.PlayersLoaded += _playerStore_PlayersLoaded;
            _playerStore.PlayerAdded += _playerStore_PlayerAdded;
            _playerStore.PlayerUpdated += _playerStore_PlayerUpdated;
            _playerStore.PlayerDeleted += _playerStore_PlayerDeleted;
            _playerStore.SelectedFilterChanged += _playerStore_SelectedFilterChanged;
            _playerStore.SelectedOrderByChanged += _playerStore_SelectedOrderByChanged;

            OrderByItemViewModel = new();

            OrderByItemViewModel.Add(new OrderByItemViewModel(new OrderBy { Id = 1, Content = "الاسم" }));
            OrderByItemViewModel.Add(new OrderByItemViewModel(new OrderBy { Id = 2, Content = "الديون" }));
            OrderByItemViewModel.Add(new OrderByItemViewModel(new OrderBy { Id = 3, Content = "منتهي الاشتراك" }));

            SelectedOrderBy = OrderByItemViewModel.FirstOrDefault(x => x.OrderBy.Id ==1);


            filtersItemViewModel = new();

            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 1, Content = "ذكور" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 2, Content = "اناث" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 3, Content = "بدون مدرب" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 4, Content = "غير فعال" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 5, Content = "الكل" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 6, Content = "فعال" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 7, Content = "منتهي الاشتراك" }));
            filtersItemViewModel.Add(new FiltersItemViewModel(new Filter { Id = 8, Content = "ديون" }));
            SelectedFilter = filtersItemViewModel.FirstOrDefault(x => x.Filter.Id == 4);
            

        }

        private void _playerStore_SelectedOrderByChanged()
        {
            switch (_playerStore.SelectedOrderBy?.Id)
            {
                case 1:
                    playerListItemViewModels.OrderBy(x => x.Player.FullName).ToList();
                    break;
                case 2:
                    playerListItemViewModels.OrderBy(x => x.Player.Balance).ToList();
                    break;
                case 3:
                    playerListItemViewModels.OrderBy(x => x.Player.SubscribeEndDate).ToList();
                    break;
                default:
                    playerListItemViewModels.OrderBy(x => x.Player.FullName);
                    break;
            }
        }

        private void _playerStore_SelectedFilterChanged()
        {
            LoadPlayersCommand.Execute(null);
        }


        protected override void Dispose()
        {
            _playerStore.PlayersLoaded -= _playerStore_PlayersLoaded;
            _playerStore.PlayerAdded -= _playerStore_PlayerAdded;
            _playerStore.PlayerUpdated -= _playerStore_PlayerUpdated;
            _playerStore.PlayerDeleted -= _playerStore_PlayerDeleted;
            _playerStore.SelectedFilterChanged -= _playerStore_SelectedFilterChanged;
            _playerStore.SelectedOrderByChanged -= _playerStore_SelectedOrderByChanged;
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
            playerListItemViewModels.Clear();

            foreach (Player player in _playerStore.Players)
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
                new PlayerListItemViewModel(player, _playerStore, _navigatorStore, _trainerStore, _sportStore, this);
            playerListItemViewModels.Add(itemViewModel);
        }
        public static PlayerListViewModel LoadViewModel(PlayerStore playerStore, NavigationStore navigatorStore, TrainerStore trainerStore, SportStore sportStore)
        {
            PlayerListViewModel viewModel = new PlayerListViewModel(navigatorStore, playerStore, trainerStore, sportStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
