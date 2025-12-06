using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    public class DeleteSetModelCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _dataStore;

        public DeleteSetModelCommand(SetsModelDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            if (parameter != null)
            {
                int id = Convert.ToInt32(parameter);
                await _dataStore.Delete(id);
            }
              
        }
    }
}
