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

        public PaymentsCardViewModel(GymStore gymStore)
        {
            _gymStore = gymStore;
            _incomeListItemViewModels = new ObservableCollection<IncomeListItemViewModel>();
            LoadPaymentsCommand = new LoadDailyPayments(_gymStore);
            _gymStore.PaymentsLoaded += _gymStore_PaymentsLoaded; ;
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
        public static PaymentsCardViewModel LoadViewModel(GymStore gymStore)
        {
            PaymentsCardViewModel viewModel = new PaymentsCardViewModel(gymStore);

            viewModel.LoadPaymentsCommand.Execute(null);

            return viewModel;
        }
    }
}
