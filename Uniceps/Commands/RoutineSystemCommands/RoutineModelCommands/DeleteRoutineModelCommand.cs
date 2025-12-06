using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.navigation;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class DeleteRoutineModelCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly NavigationService<RoutineListViewModel> _navigationService;

        public DeleteRoutineModelCommand(RoutineTempDataStore routineTempDataStore, NavigationService<RoutineListViewModel> navigationService)
        {
            _routineTempDataStore = routineTempDataStore;
            _navigationService = navigationService;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_routineTempDataStore.SelectedRoutine != null)
            {
                if(MessageBox.Show("سيتم حذف هذا البرنامج مع جميع التمارين والجولات ... هل انت متاكد","تحذير",MessageBoxButton.YesNo,MessageBoxImage.Warning)==MessageBoxResult.Yes)
                {
                    await _routineTempDataStore.Delete(_routineTempDataStore.SelectedRoutine.Id);
                    _navigationService.ReNavigate();
                }
            }
        }
    }
}
