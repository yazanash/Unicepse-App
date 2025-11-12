using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Models;
using Uniceps.API.ResponseModels;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.API.Services
{
    public class SystemProfileApiDataService
    {
        private readonly UnicepseApiClientV2 _client;

        public SystemProfileApiDataService(UnicepseApiClientV2 client)
        {
            _client = client;
        }
        public async Task<ApiResponse<SystemProfileDto>> Create(SystemProfile systemProfile)
        {
            SystemProfileDto systemProfileDto = new()
            {
                Name = systemProfile.DisplayName,
                DateOfBirth = systemProfile.BirthDate,
                Gender = systemProfile.Gender,
                Phone = systemProfile.PhoneNumber
            };
            return await _client.PostAsync<SystemProfileDto, SystemProfileDto>("Profile", systemProfileDto);
        }
        public async Task<ApiResponse<SystemProfile>> Update(SystemProfile systemProfile)
        {
            SystemProfileDto systemProfileDto = new()
            {
                Name = systemProfile.DisplayName,
                DateOfBirth = systemProfile.BirthDate,
                Gender = systemProfile.Gender,
                Phone = systemProfile.PhoneNumber
            };
            return await _client.PutAsync<SystemProfileDto, SystemProfile>("Profile", systemProfileDto);
        }
        public async Task<ApiResponse<SystemProfileDto>> Get()
        {
            return await _client.GetAsync<SystemProfileDto>("Profile");
        }
    }
}
