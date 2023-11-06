using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels
{
    public class SportsViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        //private readonly SportStore _sportStore;
        //private readonly TrainerStore _trainerStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public SportsViewModel(NavigationStore navigatorStore)
        {
            _navigatorStore = navigatorStore;
            //_sportStore = sportStore;
            //_trainerStore = trainerStore;
            navigatorStore.CurrentViewModel = CreateSportViewModel(_navigatorStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private SportListViewModel CreateSportViewModel(NavigationStore navigatorStore)
        {
            return SportListViewModel.LoadViewModel( navigatorStore);
        }
    }
}
