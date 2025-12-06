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
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.PrintViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class PlayerMainPageViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> subscriptionListItemViewModels;
        private NavigationStore _navigatorStore;
        private readonly SubscriptionDataStore _subscriptionStore;
        private readonly PaymentDataStore _paymentStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly SportDataStore _sportDataStore;
        private readonly EmployeeStore _employeeStore;
        private readonly AccountStore _accountStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => subscriptionListItemViewModels;
        public SubscriptionListItemViewModel? SelectedSubscription
        {
            get
            {
                return SubscriptionList
                    .FirstOrDefault(y => y?.Subscription == _subscriptionStore.SelectedSubscription);
            }
            set
            {
                _subscriptionStore.SelectedSubscription = value?.Subscription;

            }
        }


        public PlayerMainPageViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playersDataStore, PaymentDataStore paymentStore, SportDataStore sportDataStore, AccountStore accountStore, EmployeeStore employeeStore)
        {
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _paymentStore = paymentStore;
            _sportDataStore = sportDataStore;
            _accountStore = accountStore;
            _employeeStore = employeeStore;

            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore, _playersDataStore);
            LoadPaymentCommand = new LoadPaymentsCommand(_playersDataStore, _paymentStore);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
        }

        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPaymentCommand { get; }

        public ICommand PrintSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecutePrintSubscriptionCommand);

        public void ExecutePrintSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            string filename = subscriptionListItemViewModel.SportName + "_" + subscriptionListItemViewModel.RollDate;
            PrintWindowDialog printWindowDialog = new PrintWindowDialog(filename);
            printWindowDialog.DataContext = new PrintWindowViewModel(new SubscriptionPrintViewModel(subscriptionListItemViewModel.Subscription,_accountStore),new NavigationStore());
            printWindowDialog.ShowDialog();
        }
        public ICommand AddSubscriptionCommand => new RelayCommand(ExecuteAddSubscriptionCommand);

        public void ExecuteAddSubscriptionCommand()
        {
            CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = CreateSubscriptionWindowViewModel.LoadViewModel(_sportDataStore, _subscriptionStore, _playersDataStore, _paymentStore, _employeeStore);
            createSubscriptionWindowViewModel.SetPlayer(_playersDataStore.SelectedPlayer!);
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            subscriptionCreationViewWindow.DataContext = createSubscriptionWindowViewModel;
            subscriptionCreationViewWindow.Show();
        }
        private void _subscriptionStore_Deleted(int id)
        {
            SubscriptionListItemViewModel? itemViewModel = subscriptionListItemViewModels.FirstOrDefault(y => y.Subscription?.Id == id);

            if (itemViewModel != null)
            {
                double value = itemViewModel.Subscription.TotalPaid - itemViewModel.Subscription.PriceAfterOffer;
                _playersDataStore.UpdatePlayerBalance(itemViewModel.Subscription.PlayerId, value);
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
                new SubscriptionListItemViewModel(subscription, _navigatorStore, _subscriptionStore, _sportDataStore, _playersDataStore, this, _paymentStore);
            subscriptionListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = subscriptionListItemViewModels.Count();
        }

        public static PlayerMainPageViewModel LoadViewModel(NavigationStore navigatorStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore, PaymentDataStore paymentDataStore, SportDataStore sportDataStore,AccountStore accountStore,EmployeeStore employeeStore)
        {
            PlayerMainPageViewModel viewModel = new PlayerMainPageViewModel(navigatorStore, subscriptionDataStore, playersDataStore, paymentDataStore, sportDataStore, accountStore, employeeStore);
            viewModel.LoadSubscriptionCommand.Execute(null);
            viewModel.LoadPaymentCommand.Execute(null);
            return viewModel;
        }
    }
}
