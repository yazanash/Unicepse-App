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
        public Task<bool> RequestOTP(string email);
        public Task<bool> VerifyOTP(string email,string otp);
        public event Action<bool>? LoginStateChanged;
        void Logout();
    }
}
