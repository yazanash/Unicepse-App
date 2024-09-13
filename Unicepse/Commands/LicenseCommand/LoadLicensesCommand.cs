using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Stores;
using Unicepse.ViewModels;

namespace Unicepse.Commands.LicenseCommand
{
    public class LoadLicensesCommand : AsyncCommandBase
    {
        private readonly LicenseDataStore _licenseDataStore;
        private readonly ListingViewModelBase _licenseListing;


        public LoadLicensesCommand( LicenseDataStore licenseDataStore, ListingViewModelBase licenseListing)
        {
            _licenseDataStore = licenseDataStore;
            _licenseListing =licenseListing;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            _licenseListing.ErrorMessage = null;
            _licenseListing.IsLoading = true;

            try
            {

                await _licenseDataStore.GetLicenses();
            }
            catch (Exception)
            {
                _licenseListing.ErrorMessage = "Failed to load Licenses. Please restart the application.";
            }
            finally
            {
                _licenseListing.IsLoading = false;
            }
        }
    }
}
