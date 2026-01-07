using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineItemsCommands
{
    public class ApplySetsToAllCommand : AsyncCommandBase
    {
        private readonly SetsModelDataStore _dataStore;
        private readonly RoutineItemListViewModel _routineItemListViewModel;

        public ApplySetsToAllCommand(SetsModelDataStore dataStore, RoutineItemListViewModel routineItemListViewModel)
        {
            _dataStore = dataStore;
            _routineItemListViewModel = routineItemListViewModel;
        }

        public async override Task ExecuteAsync(object? parameter)
        {
            var selected = _routineItemListViewModel.SelectedRoutineItem;
            if (selected != null && selected.RoutineItemModel.Sets.Count > 0)
            {
                List<RoutineItemModel>? reorderedList = _routineItemListViewModel.RoutineItems.Where(x => x.Id != selected.Id).Select(x => x.RoutineItemModel!).ToList();

                foreach (RoutineItemModel itemModel in reorderedList)
                {
                    if (itemModel != null)
                    {
                        List<SetModel> setModels = new List<SetModel>();
                        foreach (var set in selected.RoutineItemModel.Sets)
                        {
                            SetModel setModel = new SetModel()
                            {
                                Repetition = set.Repetition,
                                RoundIndex = set.RoundIndex,
                                RoutineItemId = itemModel.Id,
                            };
                            setModels.Add(setModel);
                        }
                        await _dataStore.ApplySetsToAll(setModels, itemModel.Id);
                    }
                }
            }
            else
                MessageBox.Show("يجب تحديد العنصر المراد تطبيق جولاته وتاكد من انه يحتوي على جولات");
        }
    }
}
