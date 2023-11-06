using PlatinumGymPro.Stores;
using PlatinumGymPro.ViewModels.TrainingViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Commands.TrainingCommands
{
    public class LoadSportsForTrainingCommand : AsyncCommandBase
    {
        //private readonly SportStore _sportStore;
        private readonly AddTrainingViewModel _addTrainingViewModel;

        public LoadSportsForTrainingCommand( AddTrainingViewModel addTrainingViewModel)
        {
            //this._sportStore = sportStore;
            this._addTrainingViewModel = addTrainingViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _addTrainingViewModel.ErrorMessage = null;
            _addTrainingViewModel.IsLoading = true;

            try
            {
                //await _sportStore.Load();
            }
            catch (Exception)
            {
                _addTrainingViewModel.ErrorMessage = "Failed to load YouTube viewers. Please restart the application.";
            }
            finally
            {
                _addTrainingViewModel.IsLoading = false;
            }
        }
    }
}
