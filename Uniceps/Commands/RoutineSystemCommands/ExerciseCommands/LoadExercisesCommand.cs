using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;

namespace Uniceps.Commands.RoutineSystemCommands.ExerciseCommands
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
                await _exercisesDataStore.GetAllMuscleGroups();
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
