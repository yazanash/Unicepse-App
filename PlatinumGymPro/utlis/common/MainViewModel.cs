﻿//using PlatinumGymPro.Models;
using PlatinumGymPro.State.Navigator;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
       
        public INavigator Navigator { get; set; }
        //private readonly NavigationStore _navigationStore;
        public NavigationStore _navigatorStore;

        public MainViewModel( NavigationStore navigatorStore)
        {
            _navigatorStore = navigatorStore;

            Navigator = new Navigator( _navigatorStore);
            Navigator.CurrentViewModel = new HomeViewModel();
            //_navigationStore.CurrentViewModelChanged += NavigationStore_CurrentViewModelChanged;
        }

        //private void NavigationStore_CurrentViewModelChanged()
        //{
        //    OnPropertyChanged(nameof(CurrentViewModel));
        //}

        //public ViewModelBase? CurrentViewModel =>_navigationStore.CurrentViewModel;

    }
}
