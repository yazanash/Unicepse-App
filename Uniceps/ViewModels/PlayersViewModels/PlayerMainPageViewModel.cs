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
        public int PlayerId;
        private readonly Func<int, CreateSubscriptionWindowViewModel> _createSubscriptionFactory ;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
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
        public PlayerMainPageViewModel(int playerId, NavigationStore navigatorStore, SubscriptionDataStore subscriptionStore, PlayersDataStore playersDataStore, PaymentDataStore paymentStore, SportDataStore sportDataStore, EmployeeStore employeeStore, Func<int, CreateSubscriptionWindowViewModel> createSubscriptionFactory)
        {
            PlayerId = playerId;
            _navigatorStore = navigatorStore;
            _subscriptionStore = subscriptionStore;
            _playersDataStore = playersDataStore;
            _paymentStore = paymentStore;
            _sportDataStore = sportDataStore;
            _employeeStore = employeeStore;
            _createSubscriptionFactory = createSubscriptionFactory;
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionStore);
            LoadPaymentCommand = new LoadPaymentsCommand(_paymentStore);
            subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionStore.Loaded += _subscriptionStore_Loaded;
            _subscriptionStore.Created += _subscriptionStore_Created;
            _subscriptionStore.Updated += _subscriptionStore_Updated;
            _subscriptionStore.Deleted += _subscriptionStore_Deleted;
            LoadSubscriptionCommand.Execute(PlayerId);
            LoadPaymentCommand.Execute(PlayerId);
        }

        public ICommand LoadSubscriptionCommand { get; }
        public ICommand LoadPaymentCommand { get; }

        public ICommand PrintSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecutePrintSubscriptionCommand);

        public void ExecutePrintSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            string filename = subscriptionListItemViewModel.SportName + "_" + subscriptionListItemViewModel.RollDate;
            PrintWindowDialog printWindowDialog = new PrintWindowDialog(filename);
            printWindowDialog.DataContext = new PrintWindowViewModel(new SubscriptionPrintViewModel(subscriptionListItemViewModel.Subscription),new NavigationStore());
            printWindowDialog.ShowDialog();
        }
        public ICommand AddSubscriptionCommand => new RelayCommand(ExecuteAddSubscriptionCommand);

        public void ExecuteAddSubscriptionCommand()
        {
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            subscriptionCreationViewWindow.DataContext = _createSubscriptionFactory(PlayerId);
            subscriptionCreationViewWindow.Show();
        }
        public ICommand EditSubscriptionCommand => new RelayCommand<SubscriptionListItemViewModel>(ExecuteEditSubscriptionCommand);

        public void ExecuteEditSubscriptionCommand(SubscriptionListItemViewModel subscriptionListItemViewModel)
        {
            SubscriptionCreationViewWindow subscriptionCreationViewWindow = new SubscriptionCreationViewWindow();
            CreateSubscriptionWindowViewModel createSubscriptionWindowViewModel = _createSubscriptionFactory(PlayerId);
            createSubscriptionWindowViewModel.ApplySubscriptionEdit(subscriptionListItemViewModel.Subscription);
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
                new SubscriptionListItemViewModel(subscription, _navigatorStore, _subscriptionStore, _sportDataStore, _playersDataStore, this, _paymentStore, _employeeStore);
            subscriptionListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = subscriptionListItemViewModels.Count();
        }
     
    }
}
