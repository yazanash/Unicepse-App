using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.AccountantCommand;
using Unicepse.Core.Models.Payment;
using Unicepse.Stores;
using Unicepse.utlis.common;

namespace Unicepse.ViewModels.Accountant
{
    public class PaymentsCardViewModel : ListingViewModelBase
    {
        private readonly GymStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public PaymentsCardViewModel(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPaymentsCommand = new LoadDailyPayments(_gymStore,_accountingStateViewModel);
            _gymStore.PaymentsLoaded += _gymStore_PaymentsLoaded;
            
        }

        private void _gymStore_PaymentsLoaded()
        {
            _incomeListItemViewModels.Clear();
            foreach (PlayerPayment payment in _gymStore.Payments)
            {
                AddPayment(payment);
            }
        }

        private readonly ObservableCollection<IncomeListItemViewModel>  _incomeListItemViewModels;
        public IEnumerable<IncomeListItemViewModel> PaymentsList => _incomeListItemViewModels;

        private void AddPayment(PlayerPayment payments)
        {
            IncomeListItemViewModel itemViewModel =
                 new IncomeListItemViewModel(payments);
            _incomeListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _incomeListItemViewModels.Count();
        }
        public ICommand LoadPaymentsCommand;
        public static PaymentsCardViewModel LoadViewModel(GymStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            PaymentsCardViewModel viewModel = new PaymentsCardViewModel(gymStore, accountingStateViewModel);

            viewModel.LoadPaymentsCommand.Execute(null);

            return viewModel;
        }
    }
}
