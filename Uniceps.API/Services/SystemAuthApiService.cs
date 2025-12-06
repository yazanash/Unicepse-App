using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;
using Uniceps.API.ResponseModels;

namespace Uniceps.API.Services
{
    public class SystemAuthApiService
    {
        private readonly UnicepseApiClientV2 _client;

        public SystemAuthApiService(UnicepseApiClientV2 client)
        {
            _client = client;
        }

        public async Task<ApiResponse<object>> RequestOTP(string email)
        {
            RequestOTPDto emailPost = new RequestOTPDto() { Email = email };
            return await _client.PostAsync<RequestOTPDto, object>("Authentication", emailPost);
        }
        public async Task<ApiResponse<VerifyOtpResponse>> VerifyOTP(string email,string otp)
        {
            VerifyOTPDto verifyOTPDto = new()
            {
                Email = email,
                OTP = otp,
            };
            return await _client.PostAsync<VerifyOTPDto, VerifyOtpResponse>("Authentication/VerifyOtp", verifyOTPDto);
        }
        public async Task<ApiResponse<object>> VerifyToken()
        {
            return await _client.GetAsync<object>("Authentication");
        }
    }
}
