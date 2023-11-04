using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels
{
    public class TrainersViewModel : ViewModelBase
    {
        public NavigationStore _navigatorStore;
        private readonly TrainerStore _trainerStore;
        public ViewModelBase? CurrentViewModel => _navigatorStore.CurrentViewModel;
        public TrainersViewModel(NavigationStore navigatorStore, TrainerStore trainerStore)
        {
            _navigatorStore = navigatorStore;
            _trainerStore = trainerStore;
            navigatorStore.CurrentViewModel = CreateTrainerViewModel(_navigatorStore, _trainerStore);
            navigatorStore.CurrentViewModelChanged += NavigatorStore_CurrentViewModelChanged;
        }
        private void NavigatorStore_CurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }
        private TrainersListViewModel CreateTrainerViewModel(NavigationStore navigatorStore, TrainerStore _trainerStore)
        {
            return TrainersListViewModel.LoadViewModel(_trainerStore, navigatorStore);
        }
    }
}
