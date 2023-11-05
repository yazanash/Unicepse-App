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
        //private readonly PlayerStore _playerStore;
        //private readonly SportStore _sportStore;
        //private readonly TrainerStore _trainerStore;
        private readonly NavigationStore _navigatorStore;
        public Navigator( NavigationStore navigatorStore)
        {
            //_playerStore = playerStore;
            //_sportStore = sportStore;
            //_trainerStore = trainerStore;
            _navigatorStore = navigatorStore;
            LogoutCommand = new NavaigateCommand<AuthViewModel>(new NavigationService<AuthViewModel>(_navigatorStore, () => new AuthViewModel(_navigatorStore)));

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
        //public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this, _playerStore, _sportStore, _trainerStore);
        public ICommand LogoutCommand { get; }

        public ICommand UpdateCurrentViewModelCommand => throw new NotImplementedException();
    }
}
