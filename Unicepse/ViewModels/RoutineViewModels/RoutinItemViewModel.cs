﻿using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicepse.Commands.Player;
using Unicepse.Stores;
using Unicepse.utlis.common;
using Unicepse.navigation.Stores;
using Unicepse.navigation;

namespace Unicepse.ViewModels.RoutineViewModels
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

            EditCommand = new NavaigateCommand<EditRoutineViewModel>(new NavigationService<EditRoutineViewModel>(_navigationStore, () => LoadEditRoutineViewModel(_playersDataStore, _routineDataStore, new NavigationService<RoutinePlayerViewModels>(_navigationStore, () => _routinePlayerViewModels), _navigationStore)));

        }


        private EditRoutineViewModel LoadEditRoutineViewModel(PlayersDataStore playersDataStore, RoutineDataStore routineDataStore, NavigationService<RoutinePlayerViewModels> navigationService, NavigationStore navigationStore)
        {
            return EditRoutineViewModel.LoadViewModel(playersDataStore, routineDataStore, navigationService, navigationStore);
        }
        public ICommand EditCommand { get; }
        public int Id => playerRoutine.Id;
        public string? RoutineNo => playerRoutine.RoutineNo;
        public string? routineDate => playerRoutine.RoutineData.ToShortDateString();

        public void Update(PlayerRoutine obj)
        {
            playerRoutine = obj;
        }
    }
}
