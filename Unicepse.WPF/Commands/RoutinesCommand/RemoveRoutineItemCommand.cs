using Unicepse.Core.Models.TrainingProgram;
using Unicepse.WPF.Stores;
using Unicepse.WPF.ViewModels.RoutineViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.WPF.Commands.RoutinesCommand
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
