using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Stores.SystemAuthStores
{
    public interface ISystemAuthStore
    {
        public event Action<bool>? OTPRequested;
        public event Action<bool>? OTPVerificationResult;
        public Task RequestOTP(string email);
        public Task VerifyOTP(string email,string otp);
        public event Action<bool>? LoginStateChanged;
        void Logout();
    }
}
