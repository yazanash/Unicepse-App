using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.ExpensesCommands;
using Uniceps.Commands.RevenueCommands;
using Uniceps.Core.Models;
using Uniceps.navigation.Stores;
using Uniceps.Stores;
using Uniceps.ViewModels.Expenses;
using Uniceps.ViewModels.SubscriptionViewModel;
using Uniceps.Views.Expenses;
using Uniceps.Views.OtherRevenueViews;
using Uniceps.Views.SubscriptionView;

namespace Uniceps.ViewModels.OtherRevenueViewModels
{
    public class RevenueListViewModel : ListingViewModelBase
    {
        private readonly OtherRevenuesDataStore _otherRevenuesDataStore;


        private readonly ObservableCollection<RevenueListItemViewModel> _revenueListItemViewModels;
        public IEnumerable<RevenueListItemViewModel> RevenueList => _revenueListItemViewModels;
        public ICommand? AddExpensesCommand { get; }
        public bool HasData => _revenueListItemViewModels.Count > 0;

        public RevenueListViewModel(OtherRevenuesDataStore otherRevenuesDataStore)
        {
            _otherRevenuesDataStore = otherRevenuesDataStore;
            LoadRevenuesCommand = new LoadRevenueCommand(_otherRevenuesDataStore, this);
            _revenueListItemViewModels = new ObservableCollection<RevenueListItemViewModel>();
            DeleteRevenueCommand = new DeleteRevenueCommand(_otherRevenuesDataStore);

            _otherRevenuesDataStore.Loaded += _otherRevenuesDataStore_Loaded;
            _otherRevenuesDataStore.Created += _otherRevenuesDataStore_Created;
            _otherRevenuesDataStore.Updated += _otherRevenuesDataStore_Updated;
            _otherRevenuesDataStore.Deleted += _otherRevenuesDataStore_Deleted;
            AddExpensesCommand = new RelayCommand(ExecuteAddRevenueCommand);
            LoadRevenuesCommand.Execute(null);
        }
        private void ExecuteAddRevenueCommand()
        {
            AddRevenueViewModel addRevenueViewModel = new AddRevenueViewModel(_otherRevenuesDataStore);
            RevenueDetailViewWindow revenueDetailViewWindow = new RevenueDetailViewWindow();
            revenueDetailViewWindow.DataContext = addRevenueViewModel;
            revenueDetailViewWindow.ShowDialog();
        }
        private void _otherRevenuesDataStore_Deleted(int obj)
        {
            RevenueListItemViewModel? itemViewModel = _revenueListItemViewModels.FirstOrDefault(y => y.Revenue?.Id == obj);

            if (itemViewModel != null)
            {
                _revenueListItemViewModels.Remove(itemViewModel);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _otherRevenuesDataStore_Updated(OtherRevenue obj)
        {
            RevenueListItemViewModel? revenueListItemViewModel =
                    _revenueListItemViewModels.FirstOrDefault(y => y.Revenue!.Id == obj.Id);

            if (revenueListItemViewModel != null)
            {
                revenueListItemViewModel.Update(obj);
            }
            OnPropertyChanged(nameof(HasData));
        }

        private void _otherRevenuesDataStore_Created(OtherRevenue obj)
        {
            AddRevenue(obj);
        }

        private void _otherRevenuesDataStore_Loaded()
        {
            _revenueListItemViewModels.Clear();
            foreach (OtherRevenue revenue in _otherRevenuesDataStore.Revenues)
            {
                AddRevenue(revenue);
            }

        }
        private void AddRevenue(OtherRevenue revenue)
        {
            RevenueListItemViewModel itemViewModel =
                new RevenueListItemViewModel(revenue);

            _revenueListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _revenueListItemViewModels.Count();
            OnPropertyChanged(nameof(HasData));
        }

        public ICommand DeleteRevenueCommand { get; private set; }

        public ICommand LoadRevenuesCommand { get; private set; }

        public override void Dispose()
        {
            _otherRevenuesDataStore.Loaded -= _otherRevenuesDataStore_Loaded;
            _otherRevenuesDataStore.Created -= _otherRevenuesDataStore_Created;
            _otherRevenuesDataStore.Updated -= _otherRevenuesDataStore_Updated;
            _otherRevenuesDataStore.Deleted -= _otherRevenuesDataStore_Deleted;
            base.Dispose();
        }
        public ICommand DeleteCommand => new RelayCommand<RevenueListItemViewModel>(ExecuteDeleteCommand);
        private void ExecuteDeleteCommand(RevenueListItemViewModel revenueListItemViewModel)
        {
            DeleteRevenueCommand.Execute(revenueListItemViewModel.Revenue.Id);
        }
        public ICommand EditCommand => new RelayCommand<RevenueListItemViewModel>(ExecuteEditRevenueCommand);
        public void ExecuteEditRevenueCommand(RevenueListItemViewModel revenueListItemViewModel)
        {
            AddRevenueViewModel editRevenueViewModel = new AddRevenueViewModel(_otherRevenuesDataStore, revenueListItemViewModel.Revenue);
            RevenueDetailViewWindow revenueDetailViewWindow = new RevenueDetailViewWindow();
            revenueDetailViewWindow.DataContext = editRevenueViewModel;
            revenueDetailViewWindow.ShowDialog();
        }
    }
}
