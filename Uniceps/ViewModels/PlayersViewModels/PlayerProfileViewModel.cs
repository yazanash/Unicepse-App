using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Core.Models.Player;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.PaymentsViewModels;
using Uniceps.ViewModels.PlayersAttendenceViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerProfileViewModel : ViewModelBase,IQueryAttributable
    {
        private readonly PlayersDataStore _playersDataStore;
        private readonly LicenseStore _licenseStore;

        private readonly IDialogService _dialogService;

        public int PlayerId { get; set; }
        public PlayerListItemViewModel? Player { get; set; }

        public PlayerMainPageViewModel PlayerMainPageViewModel { get; }
        public PaymentListViewModel PaymentListViewModel { get; }
        public MetricReportViewModel MetricReportViewModel { get; }
        public PlayerAttendenceViewModel PlayerAttendenceViewModel { get; }

        private int _selectedTabIndex = 0;
        public int SelectedTabIndex
        {
            get => _selectedTabIndex;
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
               
            }
        }

        public bool IsFullVersion => _licenseStore.Current.IsFullVersion;
       
        public PlayerProfileViewModel(
            PlayersDataStore playersDataStore,
            LicenseStore licenseStore,
            PlayerMainPageViewModel playerMainPageViewModel,
            PaymentListViewModel paymentListViewModel,
            MetricReportViewModel metricReportViewModel,
            PlayerAttendenceViewModel playerAttendenceViewModel,
            PremiumViewModel premiumViewModel,
            IDialogService dialogService)
        {
            _playersDataStore = playersDataStore;
            _licenseStore = licenseStore;
            _playersDataStore.Player_update += _playersDataStore_Player_update;
            PlayerMainPageViewModel = playerMainPageViewModel;
            PaymentListViewModel = paymentListViewModel;
            MetricReportViewModel = metricReportViewModel;
            PlayerAttendenceViewModel = playerAttendenceViewModel;
         

            //if (IsFullVersion)
            //{
            //    MetricReportViewModel = metricReportViewModel;
            //    PlayerAttendenceViewModel = playerAttendenceViewModel;
            //    // metricReportViewModel.Initialize(PlayerId);
            //    // playerAttendenceViewModel.Initialize(PlayerId);
            //}
            //else
            //{
            //    //MetricReportViewModel = premiumViewModel;
            //    //PlayerAttendenceViewModel = premiumViewModel;
            //}

            _dialogService = dialogService;
        }

        public ICommand GetPlayerByIdCommand => new AsyncRelayCommand<int>(GetPlayerById);

        private async Task GetPlayerById(int id)
        {
            Player player = await _playersDataStore.GetPlayerById(id);
            Player = new PlayerListItemViewModel(player, _playersDataStore,_dialogService);
            OnPropertyChanged(nameof(Player));
        }

        private void _playersDataStore_Player_update(Player obj)
        {
            if (Player != null && Player.Player.Id == obj.Id)
                Player.Update(obj);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<int>("PlayerId", out var playerId))
            {
                PlayerId = playerId;
                GetPlayerByIdCommand.Execute(PlayerId);
                var navParams = new Dictionary<string, object>
            {
                { "PlayerId", PlayerId },
            };
                PlayerMainPageViewModel.ApplyQueryAttributes(navParams);
                PaymentListViewModel.ApplyQueryAttributes(navParams);
                MetricReportViewModel.ApplyQueryAttributes(navParams);
                PlayerAttendenceViewModel.ApplyQueryAttributes(navParams);
            }
        }
    }
}
