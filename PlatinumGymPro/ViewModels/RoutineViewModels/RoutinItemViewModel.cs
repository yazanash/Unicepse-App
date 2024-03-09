using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Commands;
using PlatinumGymPro.Services;
using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlatinumGymPro.ViewModels.RoutineViewModels
{
    public class RoutinItemViewModel : ViewModelBase
    {
        public PlayerRoutine playerRoutine { get; set; }
        private readonly PlayersDataStore _playersDataStore;
        private readonly RoutineDataStore _routineDataStore;
        private readonly NavigationStore _navigationStore;
        private readonly RoutinePlayerViewModels _routinePlayerViewModels;
        public RoutinItemViewModel(PlayerRoutine playerRoutine, PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationStore navigationStore, RoutinePlayerViewModels routinePlayerViewModels)
        {
            this.playerRoutine = playerRoutine;
            _playersDataStore = playersDataStore;
            _routineDataStore = routineDataStore;
            _navigationStore = navigationStore;
            _routinePlayerViewModels = routinePlayerViewModels;

            EditCommand = new NavaigateCommand<EditRoutineViewModel>(new NavigationService<EditRoutineViewModel>(_navigationStore, () => LoadAddRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore,()=> _routinePlayerViewModels))));
        }

        private EditRoutineViewModel LoadAddRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService)
        {
            return EditRoutineViewModel.LoadViewModel(playersDataStore, routineDataStore, navigationService);
        }

        public ICommand EditCommand { get; }
        public int Id => playerRoutine.Id;
        public int RoutineNo => playerRoutine.RoutineNo;
        public DateTime routineDate => playerRoutine.RoutineData;

        public void Update(PlayerRoutine obj)
        {
            this.playerRoutine = obj;
        }
    }
}
