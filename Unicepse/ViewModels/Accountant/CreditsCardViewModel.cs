using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.AccountantCommand;
using Unicepse.Core.Models.Employee;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.ViewModels.Employee.CreditViewModels;

namespace Unicepse.ViewModels.Accountant
{
    public class CreditsCardViewModel : ListingViewModelBase
    {
        private readonly GymStore _gymStore;

        public CreditsCardViewModel(GymStore gymStore)
        {
            _gymStore = gymStore;
            _creditListItemViewModels = new ObservableCollection<CreditListItemViewModel>();
            LoadCreditsCommand = new LoadDailyCredits(_gymStore);
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
        public static CreditsCardViewModel LoadViewModel(GymStore gymStore)
        {
            CreditsCardViewModel viewModel = new CreditsCardViewModel(gymStore);

            viewModel.LoadCreditsCommand.Execute(null);

            return viewModel;
        }
    }
}
