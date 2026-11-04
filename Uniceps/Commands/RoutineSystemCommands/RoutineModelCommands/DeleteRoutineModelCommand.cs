using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.navigation;
using Uniceps.navigation.NavigationSystem;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels.Metrics;
using Uniceps.ViewModels.RoutineTemplateViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.RoutineModelCommands
{
    public class DeleteRoutineModelCommand : AsyncCommandBase
    {
        private readonly RoutineTempDataStore _routineTempDataStore;
        private readonly INavigationService _navigationService;
        private readonly RoutineDetailsViewModel _routineDetailsView;

        public DeleteRoutineModelCommand(RoutineTempDataStore routineTempDataStore, INavigationService navigationService, RoutineDetailsViewModel routineDetailsView)
        {
            _routineTempDataStore = routineTempDataStore;
            _navigationService = navigationService;
            _routineDetailsView = routineDetailsView;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            if (_routineDetailsView.RoutineModel  != null)
            {
                if(MessageBox.Show("سيتم حذف هذا البرنامج مع جميع التمارين والجولات ... هل انت متاكد","تحذير",MessageBoxButton.YesNo,MessageBoxImage.Warning)==MessageBoxResult.Yes)
                {
                    await _routineTempDataStore.Delete(_routineDetailsView.RoutineModel.Id);
                    _navigationService.GoBack();
                }
            }
        }
    }
}
