using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Employee;
using Uniceps.Stores.AccountantStores;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.Accountant
{
    public class CreditsCardViewModel : ListingViewModelBase
    {
        private readonly CreditsDailyAccountantStore _gymStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public CreditsCardViewModel(CreditsDailyAccountantStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            _gymStore = gymStore;
            _accountingStateViewModel = accountingStateViewModel;

            _creditListItemViewModels = new ObservableCollection<CreditListItemViewModel>();
            LoadCreditsCommand = new LoadDailyCredits(_gymStore, _accountingStateViewModel);
            _gymStore.EmployeeCreditsLoaded += _gymStore_CreditsLoaded;
        }
        private void _gymStore_CreditsLoaded()
        {
            _creditListItemViewModels.Clear();
            foreach (Credit credit in _gymStore.EmployeeCredits)
            {
                AddCredit(credit);
            }
        }

        private readonly ObservableCollection<CreditListItemViewModel> _creditListItemViewModels;
        public IEnumerable<CreditListItemViewModel> CreditsList => _creditListItemViewModels;

        private void AddCredit(Credit credit)
        {
            CreditListItemViewModel itemViewModel =
                 new CreditListItemViewModel(credit);
            _creditListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _creditListItemViewModels.Count();

        }
        public ICommand LoadCreditsCommand;
        public static CreditsCardViewModel LoadViewModel(CreditsDailyAccountantStore gymStore, AccountingStateViewModel accountingStateViewModel)
        {
            CreditsCardViewModel viewModel = new CreditsCardViewModel(gymStore, accountingStateViewModel);

            viewModel.LoadCreditsCommand.Execute(null);

            return viewModel;
        }
    }
}
