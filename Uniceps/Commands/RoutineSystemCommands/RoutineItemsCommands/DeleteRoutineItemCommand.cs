using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class DeleteRoutineItemCommand : AsyncCommandBase
    {
        private readonly RoutineItemDataStore _routineItemDataStore;

        public DeleteRoutineItemCommand(RoutineItemDataStore routineItemDataStore)
        {
            _routineItemDataStore = routineItemDataStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if(MessageBox.Show("سيتم حذف التمرين ... هل انت متاكد","",MessageBoxButton.YesNo,MessageBoxImage.Warning)== MessageBoxResult.Yes)
            {
                await _routineItemDataStore.Delete(_routineItemDataStore.SelectedRoutineItem!.Id);

            }
        }
    }
}
