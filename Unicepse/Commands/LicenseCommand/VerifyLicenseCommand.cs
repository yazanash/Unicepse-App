using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.Stores;
using Unicepse.ViewModels._ِAppViewModels;

namespace Unicepse.Commands.LicenseCommand
{
    public class VerifyLicenseCommand : AsyncCommandBase
    {
        private readonly LicenseDataStore _licenseDataStore;
        private readonly LicenseViewModel _licenseViewModel;

        public VerifyLicenseCommand(LicenseDataStore licenseDataStore, LicenseViewModel licenseViewModel)
        {
            _licenseDataStore = licenseDataStore;
            _licenseViewModel = licenseViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            try
            {
                await _licenseDataStore.VerifyLicense(_licenseViewModel.LicenseKey!);
                MessageBox.Show("Licenses getted successfully");
                _licenseViewModel.OnLicenseAction();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error in license validation " + ex.Message);
            }
        }
    }
}
