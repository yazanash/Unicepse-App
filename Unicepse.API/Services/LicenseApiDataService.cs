using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.API.Models;
using Unicepse.Core.Models;

namespace Unicepse.API.Services
{
    public class LicenseApiDataService
    {
        private readonly UnicepseApiPrepHttpClient _client;

        public LicenseApiDataService(UnicepseApiPrepHttpClient client)
        {
            _client = client;
        }
        public async Task<License> GetLicense(string ProductKey)
        {
            LicenseDto licenseDto = await _client.GetAsync<LicenseDto>($"licenses/{ProductKey}");
            return licenseDto.ToLicense();
        }

        public async Task<GymProfile> GetGymProfile(string GymId)
        {
            GymDto gymDto = await _client.GetAsync<GymDto>($"gyms/{GymId}");
            return gymDto.ToGymProfile();
        }
    }
}
