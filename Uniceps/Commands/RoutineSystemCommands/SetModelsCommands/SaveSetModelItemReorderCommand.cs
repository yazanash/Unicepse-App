using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.SetModelsCommands
{
    public class SaveSetModelItemReorderCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _dataStore;
        private readonly SetModelItemsListViewModel  _setModelItemsList;

        public SaveSetModelItemReorderCommand(SetsModelDataStore dataStore, SetModelItemsListViewModel setModelItemsList)
        {
            _dataStore = dataStore;
            _setModelItemsList = setModelItemsList;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            List<SetModel>? reorderedList = _setModelItemsList.SetModelItems.Select(x => x.SetModel!).ToList();
            foreach (SetModel? setModel in reorderedList)
            {
                if (setModel != null)
                {
                    setModel.RoundIndex = reorderedList.IndexOf(setModel);
                    _setModelItemsList.SetModelItems.SingleOrDefault(x => x.SetModel!.Id == setModel.Id)!.RoundIndex = setModel.RoundIndex;
                }
            }
            await _dataStore.UpdateRange(reorderedList);
        }
    }
}
