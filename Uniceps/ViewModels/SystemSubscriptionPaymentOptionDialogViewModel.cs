using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Uniceps.API.common;
using Uniceps.Commands;

namespace Uniceps.ViewModels
{
    public class SystemSubscriptionPaymentOptionDialogViewModel:ViewModelBase
    {
        public SystemSubscriptionPaymentOptionDialogViewModel(string paymentCardUrl, string paymentCashUrl)
        {
            PaymentCardUrl = paymentCardUrl;
            PaymentCashUrl = paymentCashUrl;
            OnPropertyChanged(nameof(CanCardExecute));
        }
        public event Action? PaymentChose;

        public string PaymentCardUrl { get; set; }
        public string PaymentCashUrl { get; set; }
        public ICommand PayWithCardCommand => new RelayCommand(ExecutePayWithCardCommand);
        private void ExecutePayWithCardCommand()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = PaymentCardUrl,
                UseShellExecute = true // This ensures it opens in the default browser
            });
            OnPaymentChose();
        }
        public ICommand PayWithCashCommand => new RelayCommand(ExecutePayWithCashCommand);
        private void ExecutePayWithCashCommand()
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = PaymentCashUrl,
                UseShellExecute = true // This ensures it opens in the default browser
            });
            OnPaymentChose();
        }
        bool CanCardExecute => !string.IsNullOrEmpty(PaymentCardUrl);
        internal void OnPaymentChose()
        {
            PaymentChose?.Invoke();
        }
    }
}
