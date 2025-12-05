using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Core.Models.Employee;
using Uniceps.ViewModels;
using Uniceps.ViewModels.Employee.CreditViewModels;
using Uniceps.Commands.AccountantCommand;
using Uniceps.Stores;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.Accountant
{
    public class CreditsCardViewModel : ListingViewModelBase
    {
        private readonly DailyReportStore _dailyReportStore;
        private readonly AccountingStateViewModel _accountingStateViewModel;
        public CreditsCardViewModel(DailyReportStore dailyReportStore,AccountingStateViewModel accountingStateViewModel)
        {
            _dailyReportStore = dailyReportStore;
            _accountingStateViewModel = accountingStateViewModel;

            _creditListItemViewModels = new ObservableCollection<CreditListItemViewModel>();
            LoadCreditsCommand = new LoadDailyCredits(_dailyReportStore, _accountingStateViewModel);
            _dailyReportStore.CreditsLoaded += _dailyReportStore_CreditsLoaded;
        }
        private void _dailyReportStore_CreditsLoaded()
        {
            _creditListItemViewModels.Clear();
            foreach (Credit credit in _dailyReportStore.Credits)
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
        public static CreditsCardViewModel LoadViewModel(DailyReportStore dailyReportStore, AccountingStateViewModel accountingStateViewModel)
        {
            CreditsCardViewModel viewModel = new CreditsCardViewModel(dailyReportStore, accountingStateViewModel);

            viewModel.LoadCreditsCommand.Execute(null);

            return viewModel;
        }
    }
}
