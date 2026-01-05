using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class SaveRoutineItemReorderCommand : AsyncCommandBase
    {
        private readonly RoutineItemDataStore _dataStore;
        private readonly RoutineItemListViewModel _routineItemListViewModel;

        public SaveRoutineItemReorderCommand(RoutineItemDataStore dataStore, RoutineItemListViewModel routineItemListViewModel)
        {
            _dataStore = dataStore;
            _routineItemListViewModel = routineItemListViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            List<RoutineItemModel>? reorderedList = _routineItemListViewModel.RoutineItems.Select(x => x.RoutineItemModel!).ToList();
            foreach (RoutineItemModel itemModel in reorderedList)
            {
                if (itemModel != null)
                {
                    itemModel.Order = reorderedList.IndexOf(itemModel);
                }
            }
            await _dataStore.UpdateRange(reorderedList);
            _routineItemListViewModel.HasOrderChanged = false;
        }
    }
}
