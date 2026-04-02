using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.BackgroundServices;
using Uniceps.Stores.RoutineStores;
using Uniceps.ViewModels;

namespace Uniceps.SystemServices
{
    public class ExerciseSyncService
    {
        private readonly ExercisesDataStore _store;
        private readonly SplashScreenViewModel _splashScreenViewModel;
        public double ExCount = 0;
        public ExerciseSyncService(ExercisesDataStore store, SplashScreenViewModel splashScreenViewModel)
        {
            _store = store;
            _splashScreenViewModel = splashScreenViewModel;
            _store.MuscleGroupDownloaded += _store_MuscleGroupDownloaded;
            _store.GotExercises += _store_GotExercises;
        }

        private void _store_GotExercises(double obj)
        {
            _splashScreenViewModel.Message = "جاري تحميل التمارين";
            _splashScreenViewModel.Progress = (obj / ExCount) * 100;
        }

        private void _store_MuscleGroupDownloaded(double obj)
        {
            ExCount = obj;
            _splashScreenViewModel.Progress = 0;
        }

        public async Task SyncIfConnectedAsync()
        {
            if (InternetAvailability.IsInternetAvailable())
            {
                //try
                //{
                    await _store.GetAndVerifyMuscleGroups();
                    await _store.GetExcersisesWithMuscleGroups();
                //}
                //catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }
       

    }
}
