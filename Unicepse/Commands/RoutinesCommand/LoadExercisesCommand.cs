using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels;
using Unicepse.Stores;

namespace Unicepse.Commands.RoutinesCommand
{
    public class LoadExercisesCommand : AsyncCommandBase
    {
        private readonly RoutineDataStore _routineDataStore;
        private readonly ListingViewModelBase _routineListing;

        public LoadExercisesCommand(RoutineDataStore routineDataStore, ListingViewModelBase routineListing)
        {
            _routineDataStore = routineDataStore;
            _routineListing = routineListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _routineListing.ErrorMessage = null;
            _routineListing.IsLoading = true;

            try
            {

                await _routineDataStore.GetAllExercises();
            }
            catch (Exception)
            {
                _routineListing.ErrorMessage = "خطأ في تحميل التمارين الرياضية يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _routineListing.IsLoading = false;
            }
        }
    }
}
