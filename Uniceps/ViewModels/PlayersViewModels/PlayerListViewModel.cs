using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Player;
using Uniceps.Core.Models.Player;
using Uniceps.navigation;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.Views.PlayerViews;

namespace Uniceps.ViewModels.PlayersViewModels
{

    public class PlayerListViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> playerListItemViewModels;
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playerStore;
        private readonly Func<int,PlayerProfileViewModel> _playerProfileFactory;
        private readonly ILogger _logger;
        private readonly string Flags = "[PL] ";
        public SearchBoxViewModel SearchBox { get; set; }
        public ICollectionView PlayerList { get; set; }
        public List<PlayerFilter> FiltersList { get; set; } = new();
        public List<Order> OrderByList { get; set; } = new();
        public ICommand AddPlayerCommand { get; }
        public bool HasData => playerListItemViewModels.Count > 0;

        private PlayerFilter? _selectedFilter;
        public PlayerFilter? SelectedFilter
        {
            get
            {
                return _selectedFilter;
            }
            set
            {
                _selectedFilter = value;
                PlayerList.Refresh();

            }
        }
        private Order? _selectedOrderBy;
        public Order? SelectedOrderBy
        {
            get
            {
                return _selectedOrderBy;
            }
            set
            {
                _selectedOrderBy = value;
                ApplySorting();
            }
        }
        public int PlayersCount => playerListItemViewModels.Count();
        public int PlayersFemaleCount => playerListItemViewModels.Count(x => !x.GenderMale);
        public int PlayersMaleCount => playerListItemViewModels.Count(x => x.GenderMale);
        public ICommand LoadPlayersCommand { get; }
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
          Func<int, PlayerProfileViewModel> playerProfileFactory)
        {
            _navigatorStore = navigatorStore;
            _playerStore = playerStore;
            _playerProfileFactory = playerProfileFactory;
            _logger = logger;

            LoadPlayersCommand = new LoadPlayersCommand(this, _playerStore);
            AddPlayerCommand = new RelayCommand(ExecuteAddPlayerCommand);
            playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            _playerStore.Player_created += PlayerStore_PlayerAdded;
            _playerStore.Player_update += PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted += PlayerStore_PlayerDeleted;
            PlayerList = CollectionViewSource.GetDefaultView(playerListItemViewModels);
            PlayerList.Filter = FilterPlayers;
            foreach(var item in Enum.GetValues<PlayerFilter>())
            {
                FiltersList.Add(item);
            }
            foreach (var item in Enum.GetValues<Order>())
            {
                OrderByList.Add(item);
            }
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
          
            _logger.LogInformation("{Flags} view model loaded", Flags);
            LoadPlayersCommand.Execute(null);
        }
        private bool FilterPlayers(object item)
        {
            if (item is PlayerListItemViewModel playerVM)
            {
                // أولاً: فحص نص البحث بـ SearchBox
                if (!string.IsNullOrWhiteSpace(SearchBox.SearchText))
                {
                    bool matchesSearch = playerVM.Player?.FullName != null &&
                                         playerVM.Player.FullName.Contains(SearchBox.SearchText, StringComparison.OrdinalIgnoreCase);
                    
                    if (!matchesSearch) return false;
                }

                if (SelectedFilter != null)
                {
                     if (SelectedFilter.Value == PlayerFilter.GenderMale && !playerVM.GenderMale) return false;
                    if (SelectedFilter.Value == PlayerFilter.GenderFemale && playerVM.GenderMale) return false;
                    if (SelectedFilter.Value == PlayerFilter.Active && !playerVM.IsActive) return false;
                    if (SelectedFilter.Value == PlayerFilter.HaveDebt && playerVM.Balance<=0) return false;
                }

                return true;
            }
            return false;
        }
        private void ApplySorting()
        {
            if (PlayerList == null) return;

            PlayerList.SortDescriptions.Clear();

            if (SelectedOrderBy != null)
            {
                switch (_selectedOrderBy)
                {
                    case Order.ByName:
                        PlayerList.SortDescriptions.Add(new SortDescription(nameof(PlayerListItemViewModel.FullName), ListSortDirection.Ascending));
                        break;
                    case Order.BySubscribeEnd:
                        PlayerList.SortDescriptions.Add(new SortDescription(nameof(PlayerListItemViewModel.SubscribeEndDate), ListSortDirection.Descending));
                        break;
                    case Order.ByDebt:
                        PlayerList.SortDescriptions.Add(new SortDescription(nameof(PlayerListItemViewModel.Balance), ListSortDirection.Descending));
                        break;
                }
              
            }
            else
            {
                // ترتيب افتراضي (مثلاً حسب حقل الترتيب Order)
                PlayerList.SortDescriptions.Add(new SortDescription(nameof(PlayerListItemViewModel.Order), ListSortDirection.Ascending));
            }
        }
        private void SearchBox_SearchedText(string? obj)
        {
            PlayerList.Refresh();
        }

        public override void Dispose()
        {
            _logger.LogInformation("{Flags}dispose", Flags);
            _playerStore.Players_loaded -= PlayerStore_PlayersLoaded;
            _playerStore.Player_created -= PlayerStore_PlayerAdded;
            _playerStore.Player_update -= PlayerStore_PlayerUpdated;
            _playerStore.Player_deleted -= PlayerStore_PlayerDeleted;
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
        }

        private void PlayerStore_PlayersLoaded()
        {
            playerListItemViewModels.Clear();
            foreach (Player player in _playerStore.Players)
            {
                AddPlayer(player);
            }
        }
       
        private void AddPlayer(Player player)
        {
            _logger.LogInformation("{Flags}add Player list item model", Flags);

            PlayerListItemViewModel itemViewModel =
                new(player, _playerProfileFactory);
            playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = playerListItemViewModels.Count;
            OnPropertyChanged(nameof(HasData));
        }
    }
}
