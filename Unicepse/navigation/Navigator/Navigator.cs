using Unicepse.Core.Models;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;

namespace Unicepse.navigation.Navigator
{
    public class Navigator : ObservableObject, INavigator
    {
        private readonly NavigationStore _navigationStore;
        public Navigator(UpdateCurrentViewModelCommand updateCurrentViewModelCommand, NavigationStore navigationStore)
        { 
            _navigationStore = navigationStore;
            _navigationStore.CurrentViewModelChanged += _navigationStore_CurrentViewModelChanged;
            UpdateCurrentViewModelCommand = updateCurrentViewModelCommand;
           
        }

        private void _navigationStore_CurrentViewModelChanged()
        {
            CurrentViewModel = _navigationStore.CurrentViewModel;
            OnPropertChanged(nameof(CurrentViewModel));
        }

        private ViewModelBase? _CurrentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get { return _CurrentViewModel!; }
            set { _CurrentViewModel = value; OnPropertChanged(nameof(CurrentViewModel)); }

        }
        public ICommand UpdateCurrentViewModelCommand { get; }
        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { _isOpen = value;OnPropertChanged(nameof(IsOpen)); }
        }

    }
}
