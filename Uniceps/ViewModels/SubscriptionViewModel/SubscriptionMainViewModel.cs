using ModalControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.PlayerAttendenceCommands;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.ViewModels.SportsViewModels;
using Uniceps.Views;
using Uniceps.Views.PlayerViews;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.SubscriptionViewModel
{
    public class SubscriptionMainViewModel : ListingViewModelBase
    {
        private readonly SubscriptionDataStore _dataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly PaymentDataStore _paymentDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly AccountStore _accountStore;
        private readonly Func<int,PlayerProfileViewModel>? _playerProfileFactory;
        private readonly Func<int, CreateSubscriptionWindowViewModel> _createSubscriptionFactory;
        public ICollectionView SubscriptionList { get; set; }
        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPlayerLogCommand { get; }
        public ICommand AddCommand => new RelayCommand(OpenCreateSubscription);
        public ICommand LoginCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteLoginCommand);
        public ICommand OpenScanCommand { get; }
        public SearchBoxViewModel SearchBox { get; set; }
        public ObservableCollection<SubscriptionStatus> SubscriptionStatuses { get; set; } = new();
        public void OpenCreateSubscription()
        {
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            subscriptionCreationViewWindow.DataContext = _createSubscriptionFactory(0);
            subscriptionCreationViewWindow.Show();
        }
        public ICommand RenewCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteRenewCommand);
        private void ExecuteRenewCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (!subscriptionListItemViewModel.Subscription.IsRenewed)
            {
                CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = _createSubscriptionFactory(0);
                createSubscriptionWindowViewModel.ApplySubscriptionRenew(subscriptionListItemViewModel.Subscription);
                SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
                subscriptionCreationViewWindow.DataContext = createSubscriptionWindowViewModel;
                subscriptionCreationViewWindow.Show();
            }
            else
            {
                MessageBox.Show("لا يمكن تجديد هذا الاشتراك .. الاشتراك مجدد مسبقا");
            }
        }
        public ICommand PrintSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecutePrintSubscriptionCommand);

        public void ExecutePrintSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            string filename = subscriptionListItemViewModel.SportName + "_" + subscriptionListItemViewModel.RollDate;
            PrintWindowDialog printWindowDialog = new PrintWindowDialog(filename);
            printWindowDialog.DataContext = new PrintWindowViewModel(new SubscriptionPrintViewModel(subscriptionListItemViewModel.Subscription), new NavigationStore());
            printWindowDialog.ShowDialog();
        }
        private async void ExecuteLoginCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (_accountStore != null)
            {
                try
                {

                DailyPlayerReport dailyPlayerReport = new DailyPlayerReport()
                {
                    loginTime = DateTime.Now,
                    logoutTime = DateTime.Now,
                    Date = DateTime.Now,
                    IsLogged = true,
                    Code = subscriptionListItemViewModel.Code!,

                };
                DailyPlayerReport? existed = await _playersAttendenceStore.GetLoggedPlayer(dailyPlayerReport);
                if (existed != null)
                {
                    existed.logoutTime = DateTime.Now;
                    existed.IsLogged = false;
                    await _playersAttendenceStore.LogOutPlayer(existed);
                    subscriptionListItemViewModel.IsLoggedIn = existed.IsLogged;
                }
                else
                {
                    await _playersAttendenceStore.LogInPlayer(dailyPlayerReport);
                    subscriptionListItemViewModel.IsLoggedIn = dailyPlayerReport.IsLogged;
                }
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }   

            }
            else
            {
                MessageBox.Show("عذرا هذه الميزة مخصصة لنسخة المدفوعة");
            }

        }
        public SubscriptionMainViewModel(SubscriptionDataStore dataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore, PlayersAttendenceStore playersAttendenceStore, AccountStore accountStore, Func<int, PlayerProfileViewModel>? playerProfileFactory, Func<int, CreateSubscriptionWindowViewModel> createSubscriptionFactory)
        {
            _dataStore = dataStore;
            _paymentDataStore = paymentDataStore;
            _playersAttendenceStore = playersAttendenceStore;
            _createSubscriptionFactory = createSubscriptionFactory;

            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            SubscriptionList = CollectionViewSource.GetDefaultView(_subscriptionListItemViewModels);
            SubscriptionList.Filter = CheckSubscriptionFilter;
            SubscriptionList.SortDescriptions.Add(new SortDescription("RollDateFull", ListSortDirection.Descending));
            _dataStore.AllLoaded += _dataStore_Loaded;
            _dataStore.Created += _subscriptionStore_Created;
            _dataStore.Updated += _subscriptionStore_Updated;
            _dataStore.Deleted += _subscriptionStore_Deleted;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
            _playersAttendenceStore.LoggedIn += _playersAttendenceStore_LoggedIn;
            _playersAttendenceStore.LoggedOut += _playersAttendenceStore_LoggedOut;
            LoadSubscriptionCommand = new LoadActiveSubscriptionCommand(_dataStore, this);
            LoadPlayerLogCommand = new AsyncRelayCommand(GetLoggedPlayers);
            _playersDataStore = playersDataStore;
            _sportDataStore = sportDataStore;
            _accountStore = accountStore;

            foreach (var item in Enum.GetValues(typeof(SubscriptionStatus)))
            {
                SubscriptionStatuses.Add((SubscriptionStatus)item);
            }

            _employeeStore = employeeStore;
            _playerProfileFactory = playerProfileFactory;
            LoadSubscriptionCommand.Execute(null);
            LoadPlayerLogCommand.Execute(null);
            OpenScanCommand = new LoginPlayerScanCommand(new ReadPlayerQrCodeViewModel(), _playersAttendenceStore);
        }

        private void _playersAttendenceStore_LoggedOut(DailyPlayerReport obj)
        {
            SubscriptionListItemViewModel? subscriptionListItemViewModel = _subscriptionListItemViewModels.FirstOrDefault(x => x.Id == obj.SubscriptionId);
            if (subscriptionListItemViewModel != null &&
                obj.Date.Date == DateTime.Now.Date)
                subscriptionListItemViewModel.IsLoggedIn = false;
        }

        private void _playersAttendenceStore_LoggedIn(DailyPlayerReport obj)
        {
            SubscriptionListItemViewModel? subscriptionListItemViewModel = _subscriptionListItemViewModels.FirstOrDefault(x => x.Id == obj.SubscriptionId);
            if (subscriptionListItemViewModel != null &&
                obj.Date.Date == DateTime.Now.Date)
                subscriptionListItemViewModel.IsLoggedIn = true;
        }

        private void _playersAttendenceStore_Loaded()
        {
            foreach (var subscription in _subscriptionListItemViewModels)
            {
                subscription.IsLoggedIn = _playersAttendenceStore.PlayersAttendence.Any(x => x.Code == subscription.Code && x.IsLogged == true&&
                x.Date.Date == DateTime.Now.Date);
            }
        }

        private async Task GetLoggedPlayers()
        {
            await _playersAttendenceStore.GetLoggedPlayers(DateTime.Now);

        }
        private void SearchBox_SearchedText(string? obj)
        {
            SubscriptionFilter = obj!;
            SubscriptionList.Refresh();
        }

        private bool CheckSubscriptionFilter(object obj)
        {
            if (obj is SubscriptionListItemViewModel subscriptionListItemViewModel)
            {
                bool matchText =
                    string.IsNullOrEmpty(SubscriptionFilter) ||
                    subscriptionListItemViewModel.PlayerName!.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase) ||
                    subscriptionListItemViewModel.SportName!.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase) ||
                    subscriptionListItemViewModel.Trainer!.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase) ||
                    subscriptionListItemViewModel.Code!.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase);

                bool hasDebt = SelectedSubscriptionStatus != SubscriptionStatus.HasDebt ||
         subscriptionListItemViewModel.RestValue >0;

                bool matchStatus =
        SelectedSubscriptionStatus == SubscriptionStatus.None || 
        subscriptionListItemViewModel.SubscriptionStatus == SelectedSubscriptionStatus;



                return matchText && matchStatus&& hasDebt;
            }
            return false;
        }
        public bool HasData => _subscriptionListItemViewModels.Count > 0;
        private string _subscriptionFilter = string.Empty;
        public string SubscriptionFilter
        {
            get { return _subscriptionFilter; }
            set { _subscriptionFilter = value; OnPropertyChanged(nameof(SubscriptionFilter)); }
        }
        private SubscriptionStatus _selectedSubscriptionStatus;
        public SubscriptionStatus SelectedSubscriptionStatus
        {
            get { return _selectedSubscriptionStatus; }
            set
            {
                _selectedSubscriptionStatus = value; OnPropertyChanged(nameof(SelectedSubscriptionStatus));
                SubscriptionList.Refresh();
            }
        }
        private SubscriptionListItemViewModel? _selectedSubscription;
        public SubscriptionListItemViewModel? SelectedSubscription
        {
            get
            {
                return _selectedSubscription;
            }
            set
            {
                _selectedSubscription = value;

                OnPropertyChanged(nameof(SelectedSubscription));
            }
        }
        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = _subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                _subscriptionListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            SubscriptionListItemViewModel? subscriptionViewModel =
                  _subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(subscription);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _subscriptionStore_Created(Subscription subscription)
        {
            AddSubscription(subscription);
        }
        private void _dataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();
            foreach (Subscription subscription in _dataStore.AllSubscriptions)
            {
                AddSubscription(subscription);
            }
            OnPropertyChanged(nameof(HasData));
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new SubscriptionListItemViewModel(subscription);
            _subscriptionListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _subscriptionListItemViewModels.Count();
            itemViewModel.IsLoggedIn = _playersAttendenceStore.PlayersAttendence.Any(x => x.Code == itemViewModel.Code && x.IsLogged == true &&
                x.Date.Date == DateTime.Now.Date);
            OnPropertyChanged(nameof(HasData));
        }
        public ICommand OpenProfileCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteOpenPlayerProfile);

        public void ExecuteOpenPlayerProfile(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (_playerProfileFactory != null)
            {
                PlayerProfileWindowView playerProfileWindowView = new PlayerProfileWindowView();
                playerProfileWindowView.DataContext = _playerProfileFactory(subscriptionListItemViewModel.Subscription.PlayerId);
                playerProfileWindowView.ShowDialog();
            }
        }

    }
}
