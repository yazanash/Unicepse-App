using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Stores;

namespace Unicepse.Commands.RoutinesCommand
{
    public class RepeateItemCommand : CommandBase
    {
        private readonly PlayersDataStore _playerStore;
        private readonly RoutineDataStore _routineDataStore;
        RoutineItems _routineItems;
        public RepeateItemCommand(PlayersDataStore playerStore, RoutineDataStore routineDataStore, RoutineItems routineItems)
        {
            _playerStore = playerStore;
            _routineDataStore = routineDataStore;
            _routineItems = routineItems;
        }

        public override void Execute(object? parameter)
        {
            RoutineItems routineItems = new RoutineItems();
            routineItems.Exercises = _routineItems.Exercises;
            routineItems.ItemOrder = _routineDataStore.RoutineItems.Where(x => x.Exercises!.GroupId == _routineDataStore.SelectedMuscle!.Id).Count() + 1;
            _routineDataStore.AddRoutineItem(routineItems);
        }
    }
}
