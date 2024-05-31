using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;

namespace Unicepse.Commands.SubscriptionCommand
{
    public class LoadSportItemsCommand : AsyncCommandBase
    {
        private readonly SportDataStore _sportStore;

        public LoadSportItemsCommand(SportDataStore sportStore)
        {
            _sportStore = sportStore;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            //try
            //{
            await _sportStore.GetAll();
            //}
            //catch (Exception ex)
            //{
            //    //_sportListing.ErrorMessage = "Failed to load Sports . Please restart the application.";
            //    System.Windows.MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //    //_sportListing.IsLoading = false;
            //}
        }
    }
}
