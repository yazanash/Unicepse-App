using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainersViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.TrainersCommands
{
    public class LoadTrainersCommand : AsyncCommandBase
    {

        //private readonly TrainerStore _trainerStore;
        private readonly TrainersListViewModel _trainersListViewModel;

        public LoadTrainersCommand(TrainersListViewModel trainersListViewModel)
        {
            //this._trainerStore = trainerStore;
            this._trainersListViewModel = trainersListViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _trainersListViewModel.ErrorMessage = null;
            _trainersListViewModel.IsLoading = true;

            try
            {
                //await _trainerStore.Load();
            }
            catch (Exception)
            {
                _trainersListViewModel.ErrorMessage = "Failed to load YouTube viewers. Please restart the application.";
            }
            finally
            {
                _trainersListViewModel.IsLoading = false;
            }
        }
    }
}
