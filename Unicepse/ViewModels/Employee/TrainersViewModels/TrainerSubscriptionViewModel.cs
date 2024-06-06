using Unicepse.Core.Models.Subscription;
using Unicepse.Commands.Employee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Stores;
using Unicepse.ViewModels.SubscriptionViewModel;

namespace Unicepse.ViewModels.Employee.TrainersViewModels
{
    public class TrainerSubscriptionViewModel : ListingViewModelBase
    {
        private readonly ObservableCollection<SubscriptionListItemViewModel> _subscriptionListItemViewModels;
        private readonly EmployeeStore _employeeStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        public IEnumerable<SubscriptionListItemViewModel> SubscriptionList => _subscriptionListItemViewModels;
        public TrainerSubscriptionViewModel(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            _employeeStore = employeeStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            LoadSubscriptionsForTrainer = new LoadSubscriptionnForTrainer(_employeeStore, _subscriptionDataStore);
        }

        private void _subscriptionDataStore_Loaded()
        {
            _subscriptionListItemViewModels.Clear();
            foreach (var subs in _subscriptionDataStore.Subscriptions)
                AddSubscription(subs);
        }
        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel subscriptionListItemViewModel = new(subscription);
            _subscriptionListItemViewModels.Add(subscriptionListItemViewModel);
            subscriptionListItemViewModel.Order = _subscriptionListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsForTrainer { get; }
        public static TrainerSubscriptionViewModel LoadViewModel(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            TrainerSubscriptionViewModel viewModel = new TrainerSubscriptionViewModel(employeeStore, subscriptionDataStore);

            viewModel.LoadSubscriptionsForTrainer.Execute(null);

            return viewModel;
        }
    }
}
