using PlatinumGymPro.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PlatinumGymPro.Commands.Sport
{
    public class DeleteSportCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;
        public DeleteSportCommand( SportDataStore sportStore)
        {

            _sportStore = sportStore;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            await _sportStore.DeleteConnectedTrainers(_sportStore.SelectedSport!.Id);
            await _sportStore.Delete(_sportStore.SelectedSport!);

            MessageBox.Show("Sport deleted successfully");
        }

    }
}
