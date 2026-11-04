using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Subscription;
using Uniceps.ViewModels;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.Accountant
{
    public class SubscriptionStateCardViewModel : ListingViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;
        public SubscriptionStateCardViewModel(DailyReportStore dailyReportStore)
        {
            _dailyReportStore = dailyReportStore;
            _incomeListItemViewModels = new ObservableCollection<SubscriptionListItemViewModel>();
            LoadSubscriptionsCommand = new LoadDailySubscriptions(_dailyReportStore);
            _dailyReportStore.SubscriptionsLoaded += __dailyReportStore_SubscriptionsLoaded;
        }

        private void __dailyReportStore_SubscriptionsLoaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (Subscription subscription in _dailyReportStore.Subscriptions)
            {
                AddSubscription(subscription);
            }
        }

        private readonly ObservableCollection<SubscriptionListItemViewModel> _incomeListItemViewModels;
        public IEnumerable<SubscriptionListItemViewModel> SubcriptionsList => _incomeListItemViewModels;

        private void AddSubscription(Subscription subscription)
        {
            SubscriptionListItemViewModel itemViewModel =
                 new SubscriptionListItemViewModel(subscription);
            _incomeListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _incomeListItemViewModels.Count();
        }
        public ICommand LoadSubscriptionsCommand;
    }
}
