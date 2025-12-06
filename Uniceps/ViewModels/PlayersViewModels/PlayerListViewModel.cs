using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.utlis.common;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Employee.TrainersViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.EmployeeViews;
using Uniceps.Views.PlayerViews;

namespace Uniceps.ViewModels.PlayersViewModels
{

    public class PlayerListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly ObservableCollection<FiltersItemViewModel> filtersItemViewModel;
        private readonly ObservableCollection<OrderByItemViewModel> OrderByItemViewModel;
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly PlayerProfileViewModel _playerProfileViewModel;
        private readonly ILogger _logger;
        private readonly string Flags = "[PL] ";
        public SearchBoxViewModel SearchBox { get; set; }
        public IEnumerable<PlayerListItemViewModel> PlayerList => playerListItemViewModels;
        public IEnumerable<FiltersItemViewModel> FiltersList => filtersItemViewModel;
        public IEnumerable<OrderByItemViewModel> OrderByList => OrderByItemViewModel;
        public ICommand AddPlayerCommand { get; }
        public bool HasData => playerListItemViewModels.Count > 0;


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
        public ICommand ImportCommand => new RelayCommand(ImportExcel);

        private void ImportExcel()
        {
            //ImporterProgressViewModel importerProgressViewModel = new ImporterProgressViewModel(_dataStore);
            //ImportProgressWindow importProgressWindow = new ImportProgressWindow
            //{
            //    DataContext = importerProgressViewModel
            //};
            //importProgressWindow.ShowDialog();
        }
        public ICommand ExportToExcelCommand => new RelayCommand(ExecuteExportToExcelCommand);
        private void ExecuteAddPlayerCommand()
        {
            AddPlayerViewModel addPlayerViewModel= new AddPlayerViewModel(_playerStore);
            PlayerDetailWindowView playerDetailWindowView = new PlayerDetailWindowView();
            playerDetailWindowView.DataContext = addPlayerViewModel;
            playerDetailWindowView.ShowDialog();
        }
        private void ExecuteExportToExcelCommand()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "احفظ الملف",
                FileName = "players_" + DateTime.Now.ToString("dd-MM-yyyy _ HH-mm") + ".xlsx"
            };

            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                if (string.IsNullOrWhiteSpace(filePath)) return;
                _playerStore.ExportToExcel(filePath);
            }
        }
        public PlayerListViewModel(NavigationStore navigatorStore, PlayersDataStore playerStore, ILogger logger,
           PlayerProfileViewModel playerProfileViewModel)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;

            _logger = logger;
            _playerProfileViewModel = playerProfileViewModel;

            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);
            AddPlayerCommand =new RelayCommand(ExecuteAddPlayerCommand);
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            ArchivedPlayerCommand = new NavaigateCommand<ArchivedPlayersListViewModel>(new NavigationService<ArchivedPlayersListViewModel>(_navigatorStore, () => ArchivedPlayersViewModel(navigatorStore, _playerStore, this, _playerProfileViewModel)));
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            _playerStore.Player_created += PlayerStore_PlayerAdded;
            _playerStore.Player_update += PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted += PlayerStore_PlayerDeleted;
            _playerStore.ArchivedPlayer_restored += PlayerStore_ArchivedPlayer_restored;
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
            _logger.LogInformation("{Flags} view model loaded", Flags);
        }

        private void PlayerStore_ArchivedPlayer_restored(Player obj)
        {
            _logger.LogInformation("{Flags}archived player resored event", Flags);

            AddPlayer(obj);
        }

        private static ArchivedPlayersListViewModel ArchivedPlayersViewModel(NavigationStore navigatorStore,
            PlayersDataStore playerStore, PlayerListViewModel playerListViewModel, PlayerProfileViewModel playerProfileViewModel)
        {
            return ArchivedPlayersListViewModel.LoadViewModel(navigatorStore, playerStore,
                playerListViewModel, playerProfileViewModel);
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _logger.LogInformation("{Flags}search text changed", Flags);

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
            _logger.LogInformation("{Flags}order changed", Flags);
            FilterPlayers(order, _playerStore.SelectedFilter);
        }

        private void PlayerStore_FilterChanged(utlis.common.Filter? filter)
        {
            _logger.LogInformation("{Flags}filter changed", Flags);
            FilterPlayers(_playerStore.SelectedOrder, filter);
        }
        public override void Dispose()
        {
            _logger.LogInformation("{Flags}dispose", Flags);
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
            _logger.LogInformation("{Flags}player deleted", Flags);
            PlayerListItemViewModel? itemViewModel = playerListItemViewModels.FirstOrDefault(y => y.Player?.Id == id);

            if (itemViewModel != null)
            {
                playerListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void PlayerStore_PlayerUpdated(Player player)
        {
            _logger.LogInformation("{Flags}player updated", Flags);
            PlayerListItemViewModel? playerViewModel =
                   playerListItemViewModels.FirstOrDefault(y => y.Player.Id == player.Id);

            if (playerViewModel != null)
            {
                playerViewModel.Update(player);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void PlayerStore_PlayerAdded(Player player)
        {
            _logger.LogInformation("{Flags}player added", Flags);
            AddPlayer(player);
            PlayersCount++;
            if (!player.GenderMale)
                PlayersFemaleCount++;
            else
                PlayersMaleCount++;

        }

        private void PlayerStore_PlayersLoaded()
        {
            _logger.LogInformation("{Flags}players loaded event", Flags);
            FilterPlayers(_playerStore.SelectedOrder, _playerStore.SelectedFilter);
        }
        void FilterPlayers(Order? order, utlis.common.Filter? filter)
        {
            _logger.LogInformation("{Flags}filter players", Flags);
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
            _logger.LogInformation("{Flags}load players", Flags);
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
            _logger.LogInformation("{Flags}search player loaded", Flags);
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
            _logger.LogInformation("{Flags}add Player list item model", Flags);

            PlayerListItemViewModel itemViewModel =
                new(player, _navigatorStore, _playerProfileViewModel);
            playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = playerListItemViewModels.Count;
            OnPropertyChanged(nameof(HasData));
        }
        public static PlayerListViewModel LoadViewModel(NavigationStore navigatorStore, PlayersDataStore playersStore,
           ILogger<PlayerListViewModel> logger, PlayerProfileViewModel playerProfileViewModel)
        {
            logger.LogInformation("[PL] view model loaded");


            PlayerListViewModel viewModel = new(navigatorStore, playersStore, logger, playerProfileViewModel);
            logger.LogInformation("[PL] execute command");
            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }
    }
}
