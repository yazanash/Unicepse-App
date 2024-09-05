using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unicepse.BackgroundServices;
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
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    await _licenseDataStore.VerifyLicense(_licenseViewModel.LicenseKey!);
                    MessageBox.Show("Licenses got successfully");
                    _licenseViewModel.OnLicenseAction();
                }
                else
                {
                    MessageBox.Show("لا يوجد اتصال بالانترنت يرجى الاتصال والمحاولة لاحقا");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in license validation " + ex.Message);
            }
        }
    }
}
