using ModalControl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.PlayerAttendenceCommands;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Helpers;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.NavigationSystem;
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
        private readonly SportDataStore _sportDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly PlayersAttendenceStore _playersAttendenceStore;
        private readonly IDialogService _dialogService;
        public ObservableCollection<SportListItemViewModel> SportsList { get; set; } = new();
        public ObservableCollection<SubscriptionTrainerListItem> TrainersList { get; set; } = new();
        private readonly List<SubscriptionListItemViewModel> _subscriptionListItemViewModels = new();
        public RangeObservableCollection<SubscriptionListItemViewModel> SubscriptionList { get; set; } = new();

        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPlayerLogCommand { get; }
        public ICommand LoadSportsCommand { get; }
        public ICommand LoadTrainersCommand { get; }

        public ICommand AddCommand => new RelayCommand(OpenCreateSubscription);
        public ICommand LoginCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteLoginCommand);
        public ICommand OpenScanCommand { get; }
        public ICommand ClearFiltersCommand => new RelayCommand(ExecuteClearFilters);
        private bool _isClearingFilters = false;
        private void ExecuteClearFilters()
        {
            _isClearingFilters = true;
            SelectedSport = null;
            SelectedTrainer = null;
            SelectedSubscriptionStatus = SubscriptionStatus.None;
            _isClearingFilters = false;
            FilterCommand.Execute(null);
        }

        public SearchBoxViewModel SearchBox { get; set; }
        public ObservableCollection<SubscriptionStatus> SubscriptionStatuses { get; set; } = new();
        public void OpenCreateSubscription()
        {
            _dialogService.ShowDialog<SubscriptionCreationViewWindow, CreateSubscriptionWindowViewModel>();
        }
        public ICommand RenewCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteRenewCommand);
        private void ExecuteRenewCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            if (!subscriptionListItemViewModel.Subscription.IsRenewed)
            {
                var navParams = new Dictionary<string, object>
                {
                    { "PlayerId", subscriptionListItemViewModel.Subscription.PlayerId },
                    {"Subscription",subscriptionListItemViewModel.Subscription },
                    {"Mode",QueryMode.Renewal},

                };
                _dialogService.ShowDialog<SubscriptionCreationViewWindow, CreateSubscriptionWindowViewModel>(navParams);
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

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


        }
        private bool _loggedPlayers;
        public bool LoggedPlayers
        {
            get => _loggedPlayers;
            set
            {
                _loggedPlayers = value;
                if (!_isClearingFilters)
                {
                    FilterCommand.Execute(null);
                }
            }
        }
        public SubscriptionMainViewModel(SubscriptionDataStore dataStore, SportDataStore sportDataStore, EmployeeStore employeeStore, PlayersAttendenceStore playersAttendenceStore, IDialogService dialogService)
        {
            _dataStore = dataStore;
            _playersAttendenceStore = playersAttendenceStore;

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
            _sportDataStore = sportDataStore;
            _sportDataStore.Loaded += _sportDataStore_Loaded;
            LoadSportsCommand = new AsyncRelayCommand(ExecuteLoadSports);
            LoadTrainersCommand = new AsyncRelayCommand(ExecuteLoadTrainers);
            foreach (var item in Enum.GetValues(typeof(SubscriptionStatus)))
            {
                SubscriptionStatuses.Add((SubscriptionStatus)item);
            }

            _employeeStore = employeeStore;
            _employeeStore.Loaded += _employeeStore_Loaded;
            LoadSubscriptionCommand.Execute(null);
            LoadPlayerLogCommand.Execute(null);
            LoadSportsCommand.Execute(null);
            LoadTrainersCommand.Execute(null);
            OpenScanCommand = new LoginPlayerScanCommand(new ReadPlayerQrCodeViewModel(), _playersAttendenceStore);
            _dialogService = dialogService;
        }
        private SubscriptionTrainerListItem? _selectedTrainer;
        public SubscriptionTrainerListItem? SelectedTrainer
        {
            get { return _selectedTrainer; }
            set
            {
                _selectedTrainer = value; OnPropertyChanged(nameof(SelectedTrainer));
                if (!_isClearingFilters)
                {
                    FilterCommand.Execute(null);
                }
            }
        }
        private SportListItemViewModel? _selectedSport;
        public SportListItemViewModel? SelectedSport
        {
            get { return _selectedSport; }
            set
            {
                _selectedSport = value; OnPropertyChanged(nameof(SelectedSport));
                if (!_isClearingFilters)
                {
                    FilterCommand.Execute(null);
                }
            }
        }
        private void _employeeStore_Loaded()
        {
            TrainersList.Clear();
            foreach (var emp in _employeeStore.Employees.Where(x => x.IsTrainer))
            {
                TrainersList.Add(new SubscriptionTrainerListItem(emp));
            }
        }

        private void _sportDataStore_Loaded()
        {
            SportsList.Clear();
            foreach (Sport sport in _sportDataStore.Sports)
            {
                SportsList.Add(new SportListItemViewModel(sport));
            }
        }

        private async Task ExecuteLoadTrainers()
        {
            await _employeeStore.GetAll();
        }

        private async Task ExecuteLoadSports()
        {
            await _sportDataStore.GetAll();
        }

        private void _playersAttendenceStore_LoggedOut(DailyPlayerReport obj)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var masterItem = _subscriptionListItemViewModels.FirstOrDefault(x => x.Id == obj.SubscriptionId);
                if (masterItem != null && obj.Date.Date == DateTime.Now.Date)
                {
                    masterItem.IsLoggedIn = false;

                    var viewItem = SubscriptionList.FirstOrDefault(x => x.Id == obj.SubscriptionId);
                    if (viewItem != null)
                    {
                        viewItem.IsLoggedIn = false;

                        if (LoggedPlayers)
                        {
                            SubscriptionList.Remove(viewItem);
                        }
                        else
                        {
                            int currentIndex = SubscriptionList.IndexOf(viewItem);

                            int targetIndex = GetFirstLoggedOutIndexExcluding(viewItem);

                            if (currentIndex != -1 && currentIndex != targetIndex)
                            {
                                if (currentIndex < targetIndex)
                                {
                                    SubscriptionList.Move(currentIndex, targetIndex - 1);
                                }
                                else
                                {
                                    SubscriptionList.Move(currentIndex, targetIndex);
                                }
                            }
                        }
                    }
                }
            }));
        }
        private int GetFirstLoggedOutIndexExcluding(SubscriptionListItemViewModel currentItem)
        {
            for (int i = 0; i < SubscriptionList.Count; i++)
            {
                if (!SubscriptionList[i].IsLoggedIn && SubscriptionList[i] != currentItem)
                {
                    return i;
                }
            }
            return SubscriptionList.Count;
        }
        private void _playersAttendenceStore_LoggedIn(DailyPlayerReport obj)
        {
            Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                SubscriptionListItemViewModel? subscriptionListItemViewModel = _subscriptionListItemViewModels.FirstOrDefault(x => x.Id == obj.SubscriptionId);
                if (subscriptionListItemViewModel != null &&
                    obj.Date.Date == DateTime.Now.Date)
                {
                    subscriptionListItemViewModel.IsLoggedIn = true;
                    var viewItem = SubscriptionList.FirstOrDefault(x => x.Id == obj.SubscriptionId);
                    if (viewItem != null)
                    {
                        viewItem.IsLoggedIn = true;

                        int currentIndex = SubscriptionList.IndexOf(viewItem);
                        if (currentIndex > 0)
                        {
                            SubscriptionList.Move(currentIndex, 0);
                        }
                    }
                    else
                    {
                        FilterCommand.Execute(null);
                    }

                }
            }));
        }

        private void _playersAttendenceStore_Loaded()
        {
            foreach (var subscription in _subscriptionListItemViewModels)
            {
                subscription.IsLoggedIn = _playersAttendenceStore.PlayersAttendence.Any(x => x.Code == subscription.Code && x.IsLogged == true &&
                x.Date.Date == DateTime.Now.Date);
            }
            FilterCommand.Execute(null);
        }

        private async Task GetLoggedPlayers()
        {
            await _playersAttendenceStore.GetLoggedPlayers(DateTime.Now);

        }
        private void SearchBox_SearchedText(string? obj)
        {
            SubscriptionFilter = obj!;
            if (!_isClearingFilters)
            {
                FilterCommand.Execute(null);
            }
        }

        public ICommand FilterCommand => new AsyncRelayCommand(ApplyFilterAndSortAsync);

        private CancellationTokenSource? _filterCancellationTokenSource;
        public async Task ApplyFilterAndSortAsync()
        {
            _filterCancellationTokenSource?.Cancel();
            _filterCancellationTokenSource = new CancellationTokenSource();
            var token = _filterCancellationTokenSource.Token;

            try
            {
                IsLoading = true;
                await Task.Yield();
                var filteredList = await Task.Run(() =>
                {
                    token.ThrowIfCancellationRequested();

                    return _subscriptionListItemViewModels.Where(item =>
                    {
                        if (token.IsCancellationRequested) return false;

                        bool matchText = string.IsNullOrEmpty(SubscriptionFilter) ||
                            (!string.IsNullOrEmpty(item.PlayerName) && item.PlayerName.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(item.SportName) && item.SportName.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(item.Trainer) && item.Trainer.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(item.Code) && item.Code.Contains(SubscriptionFilter, StringComparison.OrdinalIgnoreCase));

                        bool hasDebt = SelectedSubscriptionStatus != SubscriptionStatus.HasDebt || item.RestValue > 0;
                        bool matchStatus = SelectedSubscriptionStatus == SubscriptionStatus.None || SelectedSubscriptionStatus == SubscriptionStatus.HasDebt || item.SubscriptionStatus == SelectedSubscriptionStatus;
                        bool matchTrainer = SelectedTrainer == null || item.Subscription.TrainerId == SelectedTrainer.Id;
                        bool matchSport = SelectedSport == null || item.Subscription.SportId == SelectedSport.Id;
                        bool matchLogged = !LoggedPlayers || item.IsLoggedIn;

                        return matchText && matchStatus && hasDebt && matchTrainer && matchSport && matchLogged;
                    })
                    .OrderByDescending(x => x.IsLoggedIn)
                    .ThenByDescending(x => x.RollDateFull)
                    .ThenBy(x => x.Subscription?.Id)
                    .ToList();
                }, token);

                if (!token.IsCancellationRequested)
                {
                    SubscriptionList.ReplaceRange(filteredList);
                }
            }
            catch (OperationCanceledException)
            {
                // تم إلغاء المهمة لصالح بحث أحدث - إجراء طبيعي
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Filter Exception: {ex.Message}");
            }
            finally
            {
                if (!token.IsCancellationRequested)
                {
                    IsLoading = false;
                    OnPropertyChanged(nameof(HasData));
                }
            }
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
                if (!_isClearingFilters)
                {
                    FilterCommand.Execute(null);
                }
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
            _subscriptionListItemViewModels.RemoveAll(y => y.Subscription?.Id == id);
            var viewItem = SubscriptionList.FirstOrDefault(y => y.Subscription?.Id == id);
            if (viewItem != null)
            {
                SubscriptionList.Remove(viewItem);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            var masterItem = _subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);
            masterItem?.Update(subscription);

            var viewItem = SubscriptionList.FirstOrDefault(y => y.Subscription.Id == subscription.Id);
            viewItem?.Update(subscription);
            FilterCommand.Execute(null);
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
            FilterCommand.Execute(null);
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
            var navParams = new Dictionary<string, object>
                {
                    { "PlayerId", subscriptionListItemViewModel.Subscription.PlayerId }
                };
            _dialogService.ShowDialog<PlayerProfileWindowView, PlayerProfileViewModel>(navParams);
        }

    }
}
