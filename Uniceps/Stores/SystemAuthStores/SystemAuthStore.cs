using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;
using Uniceps.API.ResponseModels;
using Uniceps.API.Services;
using Uniceps.Helpers;
using Uniceps.Models;

namespace Uniceps.Stores.SystemAuthStores
{
    public class SystemAuthStore : ISystemAuthStore
    {
        private readonly SystemAuthApiService _systemAuthApiService;
        private readonly ISessionManager _sessionManager;
        private readonly UnicepsePrepAPIKey _apiKey;
        public SystemAuthStore(SystemAuthApiService systemAuthApiService, ISessionManager sessionManager, UnicepsePrepAPIKey apiKey)
        {
            _systemAuthApiService = systemAuthApiService;
            _sessionManager = sessionManager;
            _apiKey = apiKey;
        }
        public event Action<bool>? LoginStateChanged;
        public event Action<bool>? OTPRequested;
        public event Action<bool>? OTPVerificationResult;

        public async Task RequestOTP(string email)
        {
            ApiResponse<object> response = await _systemAuthApiService.RequestOTP(email);
            if (response.StatusCode == 200)
            {
                OTPRequested?.Invoke(true);
            }
        }

        public async Task VerifyOTP(string email, string otp)
        {
            ApiResponse<VerifyOtpResponse> response = await _systemAuthApiService.VerifyOTP(email, otp);
            if (response.StatusCode == 200 || response.StatusCode == 201)
            {
                var session = new SessionData
                {
                    Token = response.Data!.Token!,
                    Expiration = response.Data.ExpiresAt,
                    BusinessId = response.Data!.Id,
                    UserType = response.Data!.UserType
                };
                _sessionManager.SaveSession(session);
                _apiKey.updateToken(session.Token, "");
                OTPVerificationResult?.Invoke(true);
                LoginStateChanged?.Invoke(true);

            }
            else
            {
                OTPVerificationResult?.Invoke(false);
                LoginStateChanged?.Invoke(false);
            }
        }
        public void Logout()
        {
            _sessionManager.ClearSession();
            LoginStateChanged?.Invoke(false);
        }

    }
}
