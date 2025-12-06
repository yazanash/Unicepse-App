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
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PlayersViewModels;
using Uniceps.ViewModels.SportsViewModels;
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
        public ICollectionView SubscriptionList { get; set; }
        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPlayerLogCommand { get; }
        public ICommand AddCommand => new RelayCommand(OpenCreateSubscription);
        public ICommand LoginCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteLoginCommand);
        public SearchBoxViewModel SearchBox { get; set; }
        public ObservableCollection<SubscriptionStatus> SubscriptionStatuses { get; set; } = new();
        public void OpenCreateSubscription()
        {
            CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = CreateSubscriptionWindowViewModel.LoadViewModel(_sportDataStore, _dataStore, _playersDataStore, _paymentDataStore, _employeeStore);
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            subscriptionCreationViewWindow.DataContext = createSubscriptionWindowViewModel;
            subscriptionCreationViewWindow.Show();
        }
        public ICommand RenewCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteRenewCommand);
        private void ExecuteRenewCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (!subscriptionListItemViewModel.Subscription.IsRenewed)
            {
                CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = CreateSubscriptionWindowViewModel.LoadViewModel(_sportDataStore, _dataStore, _playersDataStore, _paymentDataStore, _employeeStore);
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
        private async void ExecuteLoginCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (_accountStore.SystemSubscription != null)
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
            else
            {
                MessageBox.Show("عذرا هذه الميزة مخصصة لنسخة المدفوعة");
            }

        }
        public SubscriptionMainViewModel(SubscriptionDataStore dataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore, PlayersAttendenceStore playersAttendenceStore, AccountStore accountStore)
        {
            _dataStore = dataStore;
            _paymentDataStore = paymentDataStore;
            _playersAttendenceStore = playersAttendenceStore;

            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            SubscriptionList = CollectionViewSource.GetDefaultView(_subscriptionListItemViewModels);
            SubscriptionList.Filter = CheckSubscriptionFilter;
            _dataStore.AllLoaded += _dataStore_Loaded;
            _dataStore.Created += _subscriptionStore_Created;
            _dataStore.Updated += _subscriptionStore_Updated;
            _dataStore.Deleted += _subscriptionStore_Deleted;
            SearchBox = new SearchBoxViewModel();
            SearchBox.SearchedText += SearchBox_SearchedText;
            _playersAttendenceStore.Loaded += _playersAttendenceStore_Loaded;
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
        }

        private void _playersAttendenceStore_Loaded()
        {
            foreach (var subscription in _subscriptionListItemViewModels)
            {
                subscription.IsLoggedIn = _playersAttendenceStore.PlayersAttendence.Any(x => x.Code == subscription.Code && x.IsLogged == true);
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

                bool matchStatus =
        SelectedSubscriptionStatus == SubscriptionStatus.None || // إذا اختار None يعني الكل
        subscriptionListItemViewModel.SubscriptionStatus == SelectedSubscriptionStatus;



                return matchText && matchStatus;
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
            itemViewModel.IsLoggedIn = _playersAttendenceStore.PlayersAttendence.Any(x => x.Code == itemViewModel.Code && x.IsLogged == true);
            OnPropertyChanged(nameof(HasData));
        }
        public static SubscriptionMainViewModel LoadViewModel(SubscriptionDataStore dataStore, PlayersDataStore playersDataStore, SportDataStore sportDataStore, PaymentDataStore paymentDataStore, EmployeeStore employeeStore, PlayersAttendenceStore playersAttendenceStore, AccountStore accountStore)
        {
            SubscriptionMainViewModel viewModel = new(dataStore, playersDataStore, sportDataStore, paymentDataStore, employeeStore, playersAttendenceStore, accountStore);

            viewModel.LoadSubscriptionCommand.Execute(null);
            viewModel.LoadPlayerLogCommand.Execute(null);

            return viewModel;
        }

    }
}
