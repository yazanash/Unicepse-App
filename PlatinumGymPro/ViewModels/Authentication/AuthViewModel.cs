using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using PlatinumGymPro.Stores.PlayerStores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;

        public AuthViewModel(NavigationStore navigatorStore, PlayerStore playerStore, SportStore sportStore, TrainerStore trainerStore)
        {
            _navigatorStore= navigatorStore;
            AuthCommand = new AuthCommand(new NavigationService<MainViewModel>(_navigatorStore, () => new MainViewModel(playerStore, sportStore, trainerStore,_navigatorStore)),this);
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        public ICommand AuthCommand { get; }
    }
}
