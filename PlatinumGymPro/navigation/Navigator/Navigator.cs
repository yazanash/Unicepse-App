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
        public Navigator(NavigationStore navigatorStore, PlayersDataStore playersStore)
        {
            _navigatorStore = navigatorStore;
            _playersStore = playersStore;
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
        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this, _playersStore);
        //public ICommand LogoutCommand { get; }

    }
}
