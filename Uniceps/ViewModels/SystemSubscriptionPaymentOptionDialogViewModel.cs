using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Uniceps.API.common;
using Uniceps.Commands;
using Uniceps.LicenseManager;
using Uniceps.Stores;
using ZXing;

namespace Uniceps.ViewModels
{
    public class SystemSubscriptionPaymentOptionDialogViewModel : ViewModelBase
    {
        private readonly ActivationService _activationService;
        private readonly LicenseStore _licenseStore;
        public SystemSubscriptionPaymentOptionDialogViewModel(ActivationService activationService, LicenseStore licenseStore)
        {
            _activationService = activationService;
            _licenseStore = licenseStore;
        }
        public event Action? PaymentChose;
       private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set {  _isLoading = value; OnPropertyChanged(nameof(IsLoading)); }
        }
        public ICommand GetLicenseCommand => new AsyncRelayCommand(ExecuteGetLicenseCommand);
        private async Task ExecuteGetLicenseCommand()
        {
            IsLoading = true;
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Uniceps License (*.unxlic)|*.unxlic",
                Title = "اختر ملف الترخيص"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var result = await _activationService.ActivateFromLicenseFile(openFileDialog.FileName);
                _licenseStore.Update(_activationService.GetCurrentLicenseStatus());
                IsLoading = false;
                if (_licenseStore.Current.IsFullVersion)
                {
                    MessageBox.Show(result, "نتيجة التفعيل");
                    OnPaymentChose();
                    MessageBox.Show("قم باعادة تشغيل التطبيق لفعيل الترخيص", "تنويه");
                    Application.Current.Shutdown();
                }


            }
            IsLoading = false;
        }
        internal void OnPaymentChose()
        {
            PaymentChose?.Invoke();
        }
    }
}
