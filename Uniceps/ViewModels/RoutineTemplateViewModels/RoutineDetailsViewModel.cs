using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.ViewModels;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels.RoutineDataViewModels;

namespace Uniceps.ViewModels.RoutineTemplateViewModels
{
    public class RoutineDetailsViewModel : ListingViewModelBase
    {
        public RoutineDayGroupViewModel RoutineDayGroupViewModel { get; set; }
        public RoutineItemListViewModel RoutineItemListViewModel { get; set; }
        public SetModelItemsListViewModel SetModelListItemViewModel { get; set; }
        public RoutineDetailsViewModel(RoutineDayGroupViewModel routineDayGroupViewModel, RoutineItemListViewModel routineItemListViewModel, SetModelItemsListViewModel setModelListItemViewModel)
        {
            RoutineDayGroupViewModel = routineDayGroupViewModel;
            RoutineItemListViewModel = routineItemListViewModel;
            SetModelListItemViewModel = setModelListItemViewModel;
        }

    }
}
