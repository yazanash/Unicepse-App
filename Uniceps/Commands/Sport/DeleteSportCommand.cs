using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.Core.Exceptions;

namespace Uniceps.Commands.Sport
{
    public class DeleteSportCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;
        public DeleteSportCommand(SportDataStore sportStore)
        {

            _sportStore = sportStore;
        }
        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter);
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                if (MessageBox.Show("سيتم حذف هذه الرياضة , هل انت متاكد", "تنبيه", MessageBoxButton.YesNo,
                                         MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    await _sportStore.DeleteConnectedTrainers(_sportStore.SelectedSport!.Id);
                    await _sportStore.Delete(_sportStore.SelectedSport!);

                    MessageBox.Show("تم حذف الرياضة بنجاح");
                }

            }
            catch (NotExistException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
