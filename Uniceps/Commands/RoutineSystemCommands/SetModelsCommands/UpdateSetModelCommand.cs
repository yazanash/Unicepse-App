using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    public class UpdateSetModelCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _dataStore;
        private readonly SetModelListItemViewModel _setModelListItemViewModel;

        public UpdateSetModelCommand(SetsModelDataStore dataStore, SetModelListItemViewModel setModelListItemViewModel)
        {
            _dataStore = dataStore;
            _setModelListItemViewModel = setModelListItemViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            SetModel setModel = _setModelListItemViewModel.SetModel!;
            setModel.Repetition = _setModelListItemViewModel.Repetition;
            await _dataStore.Update(setModel);
        }
    }
}
