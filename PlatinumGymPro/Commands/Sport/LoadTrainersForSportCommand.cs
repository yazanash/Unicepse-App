using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.SportsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.SportsCommands
{
    public class LoadTrainersForSportCommand : AsyncCommandBase
    {
        //private readonly TrainerStore _trainerStore;
        private readonly AddSportViewModel _addSportViewModel;
        public LoadTrainersForSportCommand( AddSportViewModel addSportViewModel)
        {
            //_trainerStore = trainerStore;
            _addSportViewModel = addSportViewModel;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _addSportViewModel.ErrorMessage = null;
            _addSportViewModel.IsLoading = true;

            try
            {
                //await _trainerStore.Load();
            }
            catch (Exception)
            {
                _addSportViewModel.ErrorMessage = "Failed to load YouTube viewers. Please restart the application.";
            }
            finally
            {
                _addSportViewModel.IsLoading = false;
            }
        }
    }
}
