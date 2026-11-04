using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.Payments;
using Uniceps.Commands.SubscriptionCommand;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Subscription;
using Uniceps.navigation.DialogService;
using Uniceps.navigation.Extensions;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerMainPageViewModel : ListingViewModelBase,IQueryAttributable
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> subscriptionListItemViewModels;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly IDialogService _dialogService;

        public int PlayerId;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => subscriptionListItemViewModels;

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

            }
        }
        public PlayerMainPageViewModel( SubscriptionDataStore subscriptionStore, IDialogService dialogService)
        {
            _subscriptionStore = subscriptionStore;
            _dialogService = dialogService;

            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
        }

        public ICommand LoadSubscriptionCommand { get; }

        public ICommand PrintSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecutePrintSubscriptionCommand);

        public void ExecutePrintSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            string filename = subscriptionListItemViewModel.SportName + "_" + subscriptionListItemViewModel.RollDate;
            PrintWindowDialog printWindowDialog = new PrintWindowDialog(filename);
            printWindowDialog.DataContext = new PrintWindowViewModel(new SubscriptionPrintViewModel(subscriptionListItemViewModel.Subscription), new NavigationStore());
            printWindowDialog.ShowDialog();
        }
        public ICommand AddSubscriptionCommand => new RelayCommand(ExecuteAddSubscriptionCommand);

        public void ExecuteAddSubscriptionCommand()
        {
            var navParams = new Dictionary<string, object>
            {
                { "PlayerId", PlayerId }
            };
            _dialogService.ShowDialog<SubscriptionCreationViewWindow, CreateSubscriptionWindowViewModel>(navParams);
        }
        public ICommand EditSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteEditSubscriptionCommand);

        public void ExecuteEditSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            var navParams = new Dictionary<string, object>
            {
                { "PlayerId", PlayerId },
                {"Subscription",subscriptionListItemViewModel.Subscription },
                {"Mode",QueryMode.Edit},

            };
            _dialogService.ShowDialog<SubscriptionCreationViewWindow, CreateSubscriptionWindowViewModel>(navParams);
        }
        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                subscriptionListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _subscriptionStore_Updated(Subscription subscription)
        {
            SubscriptionListItemViewModel? subscriptionViewModel =
                  subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription.Id == subscription.Id);

            if (subscriptionViewModel != null)
            {
                subscriptionViewModel.Update(subscription);
            }
        }

        private void _subscriptionStore_Created(Subscription subscription)
        {
            LoadData();
        }

        private void _subscriptionStore_Loaded()
        {
            LoadData();
        }
        void LoadData()
        {
            subscriptionListItemViewModels.Clear();

            foreach (Subscription subscription in _subscriptionStore.Subscriptions.OrderByDescending(x => x.RollDate))
            {
                AddSubscription(subscription);
            }
        }
        public override void Dispose()
        {
            _subscriptionStore.Loaded -= _subscriptionStore_Loaded;
            _subscriptionStore.Created -= _subscriptionStore_Created;
            _subscriptionStore.Updated -= _subscriptionStore_Updated;
            _subscriptionStore.Deleted -= _subscriptionStore_Deleted;
            base.Dispose();
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                new SubscriptionListItemViewModel(subscription, _subscriptionStore);
            subscriptionListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = subscriptionListItemViewModels.Count();
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.TryGetValue<int>("PlayerId", out var playerId))
            {
                PlayerId = playerId;
                LoadSubscriptionCommand.Execute(PlayerId);
            }
           
        }
    }
}
