using PlatinumGym.Core.Models;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.State.Navigator
{
    public class Navigator : ObservableObject, INavigator 
    {
        private readonly NavigationStore _navigatorStore;
        private readonly PlayersDataStore _playersStore;
        private readonly SportDataStore _sportStore;
        private readonly EmployeeStore _employeeStore;
        private readonly ExpensesDataStore _expensesStore;
        private readonly SubscriptionDataStore _subscriptionDataStore;
        private readonly PaymentDataStore _paymentDataStore ;
        public Navigator(NavigationStore navigatorStore, PlayersDataStore playersStore,
            SportDataStore sportStore, EmployeeStore employeeStore, ExpensesDataStore expensesStore, SubscriptionDataStore subscriptionDataStore, PaymentDataStore paymentDataStore)
        {
            _navigatorStore = navigatorStore;
            _playersStore = playersStore;
            _sportStore = sportStore;
            _employeeStore = employeeStore;
            _expensesStore = expensesStore;
            _subscriptionDataStore = subscriptionDataStore;
            _paymentDataStore = paymentDataStore;

            //LogoutCommand = new NavaigateCommand<AuthViewModel>(new NavigationService<AuthViewModel>(_navigatorStore, () => new AuthViewModel(_navigatorStore)));

        }

        private ViewModelBase? _CurrentViewModel;
     

        public ViewModelBase CurrentViewModel 
        {
            get { return _CurrentViewModel!; }
            set { _CurrentViewModel = value; OnPropertChanged(nameof(CurrentViewModel)); }
            
        }
         private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value; OnPropertChanged(nameof(IsOpen)); }
            
        }
        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this, _playersStore,_sportStore,_employeeStore, _expensesStore, _subscriptionDataStore,_paymentDataStore);
        //public ICommand LogoutCommand { get; }

    }
}
