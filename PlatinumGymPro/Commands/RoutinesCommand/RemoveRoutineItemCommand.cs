using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.RoutinesCommand
{
    public class RemoveRoutineItemCommand : CommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly RoutineDataStore _routineDataStore;
        RoutineItems _routineItems;
        public RemoveRoutineItemCommand(PlayersDataStore playerStore, RoutineDataStore routineDataStore, RoutineItems routineItems)
        {
            _playerStore = playerStore;
            _routineDataStore = routineDataStore;
            _routineItems = routineItems;
        }

        public override void Execute(object? parameter)
        {
          
            _routineDataStore.DeleteRoutineItem(_routineItems);
        }

    }
}
