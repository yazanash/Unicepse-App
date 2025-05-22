using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.BackgroundServices;
using Uniceps.Commands;
using Uniceps.Stores;
using Uniceps.ViewModels.AppViewModels;

namespace Uniceps.Commands.LicenseCommand
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
                _licenseViewModel.IsLoading = true;
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    await _licenseDataStore.VerifyLicense(_licenseViewModel.LicenseKey!.Trim());
                    _licenseViewModel.IsLoading = false;
                    MessageBox.Show("تم تفعيل النسخة بنجاح");
                    _licenseViewModel.OnLicenseAction();
                }
                else
                {
                    _licenseViewModel.IsLoading = false;
                    MessageBox.Show("لا يوجد اتصال بالانترنت يرجى الاتصال والمحاولة لاحقا");
                }
            }
            catch (Exception ex)
            {
                _licenseViewModel.IsLoading = false;
                MessageBox.Show("خطأ في التحقق من النسخة " + ex.Message);
            }
        }
    }
}
