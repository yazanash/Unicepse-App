using PlatinumGym.Core.Models.Subscription;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.Employee.TrainersViewModels
{
    public class TrainerDausesListViewModel : ListingViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private ObservableCollection<TrainerDausesListItemViewModel> _trainerDausesListItemViewModels;

        public TrainerDausesListViewModel(EmployeeStore employeeStore, SubscriptionDataStore subscriptionDataStore)
        {
            _employeeStore = employeeStore;
            _subscriptionDataStore = subscriptionDataStore;
            _subscriptionDataStore.Loaded += _subscriptionDataStore_Loaded;
            _trainerDausesListItemViewModels = new ObservableCollection<TrainerDausesListItemViewModel>();
        }

        private void _subscriptionDataStore_Loaded()
        {
          foreach(Subscription subscription in _subscriptionDataStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }

        private void AddSubscription(Subscription subscription)
        {
            TrainerDausesListItemViewModel trainerdause = new TrainerDausesListItemViewModel(subscription);
            _trainerDausesListItemViewModels.Add(trainerdause);
        }
        public IEnumerable<TrainerDausesListItemViewModel> Dauses => _trainerDausesListItemViewModels;
    }
}
