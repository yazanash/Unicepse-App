using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels;

namespace Uniceps.Commands.LicenseCommand
{
    public class LoadLicensesCommand : AsyncCommandBase
    {
        private readonly LicenseDataStore _licenseDataStore;
        private readonly ListingViewModelBase _licenseListing;


        public LoadLicensesCommand(LicenseDataStore licenseDataStore, ListingViewModelBase licenseListing)
        {
            _licenseDataStore = licenseDataStore;
            _licenseListing = licenseListing;
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
                _licenseListing.ErrorMessage = "خطأ في تحميل التراخيص يرجى اعادة تشغيل البرنامج";
            }
            finally
            {
                _licenseListing.IsLoading = false;
            }
        }
    }
}
