using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Stores;
using Uniceps.utlis.common;
using Uniceps.Commands.LicenseCommand;

namespace Uniceps.ViewModels.AppViewModels
{
    public class LicenseViewModel : ViewModelBase
    {
        private readonly LicenseDataStore _licenseDataStore;

        public LicenseViewModel(LicenseDataStore licenseDataStore)
        {
            _licenseDataStore = licenseDataStore;
            VerifyLicenseCommand = new VerifyLicenseCommand(_licenseDataStore, this);
        }
        private string? _licenseKey;
        public string? LicenseKey
        {
            get { return _licenseKey; }
            set { _licenseKey = value; OnPropertyChanged(nameof(LicenseKey)); }
        }
        public ICommand? VerifyLicenseCommand { get; }

        public event Action? LicenseAction;

        public void OnLicenseAction()
        {
            LicenseAction?.Invoke();
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
    }
}
