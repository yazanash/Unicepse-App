using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.LicenseManager;

namespace Uniceps.SystemServices
{
    public class LicenseActivationService
    {
        private readonly ActivationService _activationService;

        public LicenseActivationService(ActivationService activationService)
        {
            _activationService = activationService;
        }

        public async Task HandleFileActivation( string filePath)
        {
            var result = await _activationService.ActivateFromLicenseFile(filePath);
            MessageBox.Show(result, "تفعيل الترخيص");
        }

        public async Task RequestLicenseFile()
        {

            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Uniceps License (*.unxlic)|*.unxlic",
                Title = "اختر ملف الترخيص"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var result = await _activationService.ActivateFromLicenseFile(openFileDialog.FileName);
                MessageBox.Show(result, "نتيجة التفعيل");

            }
        }
    }
}
