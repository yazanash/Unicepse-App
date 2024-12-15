using Unicepse.Core.Models.Player;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.ViewModels.PlayersViewModels;
using Unicepse.navigation.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.SubscriptionViewModel;
using Unicepse.Core.Models.Subscription;
using Unicepse.Commands.SubscriptionCommand;
using Unicepse.navigation;
using Unicepse.Stores.RoutineStores;

namespace Unicepse.ViewModels.PlayersAttendenceViewModels
{
    public class LogPlayerAttendenceViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<PlayerListItemViewModel> _playerListItemViewModels;
        private readonly PlayersDataStore _playerStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly NavigationStore _navigationStore;

        private readonly EmployeeStore _employeeStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly MetricDataStore _metricDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly LicenseDataStore _licenseDataStore;
        private readonly NavigationService<PlayerListViewModel> _navigationService;
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly RoutineTemplatesDataStore _routineTemplatesDataStore;
        private readonly HomeViewModel  _homeViewModel;
        public SearchBoxViewModel SearchBox { get; set; }

        public IEnumerable<PlayerListItemViewModel> PlayersList => _playerListItemViewModels;

        public PlayerListItemViewModel? SelectedPlayer
        {
            get
            {
                return PlayersList
                    .FirstOrDefault(y => y?.Player == _playerStore.SelectedPlayer);
            }
            set
            {
                _playerStore.SelectedPlayer = value?.Player;

            }
        }
        public ICommand LoadPlayersCommand { get; }
        public ICommand CancelCommand { get; }
        public LogPlayerAttendenceViewModel(PlayersDataStore playerStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore, HomeViewModel homeViewModel, SubscriptionDataStore subscriptionDataStore, EmployeeStore employeeStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService, ExercisesDataStore exercisesDataStore, RoutineTemplatesDataStore routineTemplatesDataStore)
        {
            _playerStore = playerStore;
            _subscriptionDataStore = subscriptionDataStore;
            LoadPlayersCommand = new LoadPlayersCommand(this,_playerStore);
            _playerListItemViewModels = new ObservableCollection<PlayerListItemViewModel>();
            _playerStore.Players_loaded += PlayerStore_PlayersLoaded;
            //_subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore = playersAttendenceStore;
            _navigationStore = navigationStore;
            _homeViewModel = homeViewModel;
            _employeeStore = employeeStore;
            _sportDataStore = sportDataStore;
            _paymentDataStore = paymentDataStore;
            _metricDataStore = metricDataStore;
            _routineDataStore = routineDataStore;
            _licenseDataStore = licenseDataStore;
            _navigationService = navigationService;
            _exercisesDataStore = exercisesDataStore;
            _routineTemplatesDataStore = routineTemplatesDataStore;
            CancelCommand = new NavaigateCommand<HomeViewModel>(new NavigationService<HomeViewModel>(_navigationStore, () => homeViewModel));
           
        }
        private void SearchBox_SearchedText(string? obj)
        {
            _playerListItemViewModels.Clear();
            if (!string.IsNullOrEmpty(obj))
            {
                LoadPlayers(_playerStore.Players, obj);
            }
            else
                LoadPlayers(_playerStore.Players);
        }




        public override void Dispose()
        {
            _playerStore.Players_loaded -= PlayerStore_PlayersLoaded;
            base.Dispose();
        }


        private void PlayerStore_PlayersLoaded()
        {
            _playerListItemViewModels.Clear();

            foreach (Player player in _playerStore.Players)
            {
                AddPlayer(player);
            }
        }
        void LoadPlayers(IEnumerable<Player> players, string query)
        {
            _playerListItemViewModels.Clear();

            foreach (Player player in players.Where(x => x.FullName!.ToLower().Contains(query.ToLower())))
            {
                AddPlayer(player);
            }


        }
        void LoadPlayers(IEnumerable<Player> players)
        {

            _playerListItemViewModels.Clear();

            foreach (Player player in players)
            {
                AddPlayer(player);
            }

        }
        private void AddPlayer(Player player)
        {
            PlayerListItemViewModel itemViewModel =
                new(player, _navigationStore, _subscriptionDataStore, _playerStore, _sportDataStore, _paymentDataStore,
                _metricDataStore, _routineDataStore, _playersAttendenceStore, _licenseDataStore, _navigationService,
                _exercisesDataStore, _routineTemplatesDataStore,_homeViewModel);
            _playerListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _playerListItemViewModels.Count;
        }
        public static LogPlayerAttendenceViewModel LoadViewModel(PlayersDataStore playersStore, PlayersAttendenceStore playersAttendenceStore, NavigationStore navigationStore , HomeViewModel homeViewModel, SubscriptionDataStore subscriptionDataStore, EmployeeStore employeeStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, MetricDataStore metricDataStore, RoutineDataStore routineDataStore, LicenseDataStore licenseDataStore, NavigationService<PlayerListViewModel> navigationService, ExercisesDataStore exercisesDataStore, RoutineTemplatesDataStore routineTemplatesDataStore)
        {
            LogPlayerAttendenceViewModel viewModel = new(playersStore, playersAttendenceStore, navigationStore,homeViewModel, subscriptionDataStore, employeeStore,  sportDataStore,  paymentDataStore,  metricDataStore,  routineDataStore,  licenseDataStore,  navigationService,  exercisesDataStore,  routineTemplatesDataStore);

            viewModel.LoadPlayersCommand.Execute(null);

            return viewModel;
        }

    }

}
