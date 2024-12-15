using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.ViewModels;
using Unicepse.Stores;
using Unicepse.Stores.RoutineStores;

namespace Unicepse.Commands.RoutinesCommand
{
    public class LoadExercisesCommand : AsyncCommandBase
    {
        private readonly ExercisesDataStore _exercisesDataStore;
        private readonly ListingViewModelBase _routineListing;

        public LoadExercisesCommand(ExercisesDataStore exercisesDataStore, ListingViewModelBase routineListing)
        {
            _exercisesDataStore = exercisesDataStore;
            _routineListing = routineListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _routineListing.ErrorMessage = null;
            _routineListing.IsLoading = true;

            try
            {

                await _exercisesDataStore.GetAll();
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
