using Uniceps.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.navigation.Stores;
using Uniceps.Commands.Player;
using Uniceps.Core.Models;
using Uniceps.ViewModels;

namespace Uniceps.navigation.Navigator
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
            set { _isOpen = value; OnPropertChanged(nameof(IsOpen)); }
        }

    }
}
