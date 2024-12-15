using Unicepse.Core.Models.Employee;
using Unicepse.Commands;
using Unicepse.Commands.Employee.CreditsCommands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.navigation;
using Unicepse.ViewModels;
using Unicepse.Stores;
using Unicepse.navigation.Stores;

namespace Unicepse.ViewModels.Employee.CreditViewModels
{
    public class CreditListViewModel : ListingViewModelBase
    {
        private readonly EmployeeStore _employeeStore;
        private readonly CreditsDataStore _creditDataStore;
        private NavigationStore _navigatorStore;
        private readonly ObservableCollection<CreditListItemViewModel> _creditListItemViewModels;
        public IEnumerable<CreditListItemViewModel> Credits => _creditListItemViewModels;
        public CreditListItemViewModel? SelectedCredit
        {
            get
            {
                return Credits
                    .FirstOrDefault(y => y?.credit == _creditDataStore.SelectedCredit);
            }
            set
            {
                _creditDataStore.SelectedCredit = value?.credit;

            }
        }
        public CreditListViewModel(EmployeeStore employeeStore, CreditsDataStore creditDataStore, NavigationStore navigatorStore)
        {
            _employeeStore = employeeStore;
            _creditDataStore = creditDataStore;
            _navigatorStore = navigatorStore;
            _creditDataStore.Created += _creditDataStore_Created;
            _creditDataStore.Updated += _creditDataStore_Updated;
            _creditDataStore.Loaded += _creditDataStore_Loaded;
            _creditDataStore.Deleted += _creditDataStore_Deleted;
            _creditListItemViewModels = new ObservableCollection<CreditListItemViewModel>();
            LoadCreditsCommand = new LoadCreditsCommand(_employeeStore, _creditDataStore);
            CreateCreditsCommand = new NavaigateCommand<CreditDetailsViewModel>(new NavigationService<CreditDetailsViewModel>(_navigatorStore, () => new CreditDetailsViewModel(_employeeStore, _creditDataStore, _navigatorStore, this)));
        }

        private void _creditDataStore_Deleted(int obj)
        {
            CreditListItemViewModel? itemViewModel = _creditListItemViewModels.FirstOrDefault(y => y.credit?.Id == obj);

            if (itemViewModel != null)
            {
                _creditListItemViewModels.Remove(itemViewModel);
            }
        }

        private void _creditDataStore_Loaded()
        {
            loadData();
        }
        void loadData()
        {
            _creditListItemViewModels.Clear();

            foreach (Credit credit in _creditDataStore.Credits.OrderByDescending(x => x.Date))
            {
                AddCredit(credit);
            }
        }
        private void _creditDataStore_Updated(Credit obj)
        {
            CreditListItemViewModel? creditViewModel =
                   _creditListItemViewModels.FirstOrDefault(y => y.credit.Id == obj.Id);

            if (creditViewModel != null)
            {
                creditViewModel.Update(obj);
            }
        }

        private void _creditDataStore_Created(Credit obj)
        {
            loadData();
        }
        private void AddCredit(Credit credit)
        {
            CreditListItemViewModel itemViewModel =
                new CreditListItemViewModel(credit, _employeeStore, _creditDataStore, _navigatorStore, this);
            _creditListItemViewModels.Add(itemViewModel);
            itemViewModel.Order = _creditListItemViewModels.Count();
        }
        public override void Dispose()
        {
            _creditDataStore.Created -= _creditDataStore_Created;
            _creditDataStore.Updated -= _creditDataStore_Updated;
            _creditDataStore.Loaded -= _creditDataStore_Loaded;
            _creditDataStore.Deleted -= _creditDataStore_Deleted;
            base.Dispose();

        }
        public ICommand LoadCreditsCommand { get; }
        public ICommand CreateCreditsCommand { get; }

        public static CreditListViewModel LoadViewModel(EmployeeStore employeeStore, CreditsDataStore creditsDataStore, NavigationStore navigatorStore)
        {
            CreditListViewModel viewModel = new CreditListViewModel(employeeStore, creditsDataStore, navigatorStore);

            viewModel.LoadCreditsCommand.Execute(null);

            return viewModel;
        }
    }
}
