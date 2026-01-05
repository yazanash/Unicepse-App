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
using Uniceps.Services;

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

        public async Task<bool> RequestOTP(string email)
        {
            try
            {

                ApiResponse<object> response = await _systemAuthApiService.RequestOTP(email);
                if (response.StatusCode == 200)
                {
                    OTPRequested?.Invoke(true);
                    return true;
                }
                return false;
            }
            catch  
            {
                return false;
            }
        }

        public async Task<bool> VerifyOTP(string email, string otp)
        {
            try
            {

            VerifyOTPDto verifyOTPDto = new VerifyOTPDto()
            {
                OTP = otp,
                Email = email,
                AppVersion = DeviceInfoService.AppVersion,
                DeviceId = DeviceInfoService.DeviceId,
                DeviceModel = DeviceInfoService.DeviceModel,
                DeviceToken = DeviceInfoService.DeviceToken,
                OsVersion = DeviceInfoService.OsVersion,
                Platform = DeviceInfoService.Platform,

            };
            ApiResponse<VerifyOtpResponse> response = await _systemAuthApiService.VerifyOTP(verifyOTPDto);
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
                return true;
            }
            else
            {
                OTPVerificationResult?.Invoke(false);
                LoginStateChanged?.Invoke(false);
                return true;
            }
            }
            catch
            {
                return false;
            }
        }
        public void Logout()
        {
            _sessionManager.ClearSession();
            LoginStateChanged?.Invoke(false);
        }

    }
}
