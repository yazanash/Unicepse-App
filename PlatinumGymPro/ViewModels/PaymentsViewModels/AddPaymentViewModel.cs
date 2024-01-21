using PlatinumGymPro.Commands.SubscriptionCommand;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.PaymentsViewModels
{
    public class AddPaymentViewModel : ListingViewModelBase
    {
        private readonly PaymentDataStore _paymentDataStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PlayersDataStore _playersDataStore;
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListViewModel;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListViewModel;
        public AddPaymentViewModel(PaymentDataStore paymentDataStore, SubscriptionDataStore subscriptionDataStore, PlayersDataStore playersDataStore)
        {
            _paymentDataStore = paymentDataStore;
            _subscriptionDataStore = subscriptionDataStore;
            _playersDataStore = playersDataStore;
            _subscriptionListViewModel = new ObservableCollection<SubscriptionListItemViewModel>();
            LoadSubscriptionCommand = new LoadSubscriptions(this, _subscriptionDataStore, _playersDataStore.SelectedPlayer!);
        }

        ICommand LoadSubscriptionCommand;
    }
}
