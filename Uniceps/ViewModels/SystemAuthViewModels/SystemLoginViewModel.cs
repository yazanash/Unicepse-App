using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.Commands;
using Uniceps.Commands.SystemAuthCommands;
using Uniceps.Stores;
using Uniceps.Stores.SystemAuthStores;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class SystemLoginViewModel : ViewModelBase
    {
        private readonly ISystemAuthStore _systemAuthStore;

        public SystemLoginViewModel(ISystemAuthStore systemAuthStore)
        {
            _systemAuthStore = systemAuthStore;
            _systemAuthStore.OTPRequested += _systemAuthStore_OTPRequested;
            _systemAuthStore.OTPVerificationResult += _systemAuthStore_OTPVerificationResult; ;
            RequestOTPCommand = new LoginSystemUserCommand(_systemAuthStore, this);
        }

        private void _systemAuthStore_OTPVerificationResult(bool success)
        {
            if (success)
            {
                OnOTPVerifiedAction();
            }
            else
            {
                ErrorMessage = "فشل في إرسال الرمز، تأكد من البريد الإلكتروني.";
            }
        }

        private void _systemAuthStore_OTPRequested(bool isOTPRequested)
        {
            IsOTPRequested = isOTPRequested;
            if (!isOTPRequested)
            {
                ErrorMessage = "فشل في إرسال الرمز، تأكد من البريد الإلكتروني.";
            }
        }

        private string? _email;
        public string? Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        private string? _otp;
        public string? OTP
        {
            get { return _otp; }
            set { _otp = value; OnPropertyChanged(nameof(OTP)); }
        }
        public ICommand? RequestOTPCommand { get; }
        public ICommand ResetCommand => new RelayCommand(ExecuteReset);

        private void ExecuteReset()
        {
            IsOTPRequested = false;

        }

        public event Action? OTPVerifiedAction;

        public void OnOTPVerifiedAction()
        {
            OTPVerifiedAction?.Invoke();
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
        private bool _isOTPRequested;
        public bool IsOTPRequested
        {
            get
            {
                return _isOTPRequested;
            }
            set
            {
                _isOTPRequested = value;
                OnPropertyChanged(nameof(IsOTPRequested));
            }
        }
        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
        public bool HasError=>!string.IsNullOrEmpty(ErrorMessage);
    }
}
