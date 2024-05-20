using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Commands.Employee;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SubscriptionViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.Employee.TrainersViewModels
{
    public class TrainerSubscriptionViewModel :ListingViewModelBase
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
        }
        public ICommand LoadSubscriptionsForTrainer { get; }
        public static TrainerSubscriptionViewModel LoadViewModel( EmployeeStore employeeStore,  SubscriptionDataStore subscriptionDataStore )
        {
            TrainerSubscriptionViewModel viewModel = new TrainerSubscriptionViewModel( employeeStore, subscriptionDataStore);

            viewModel.LoadSubscriptionsForTrainer.Execute(null);

            return viewModel;
        }
    }
}
