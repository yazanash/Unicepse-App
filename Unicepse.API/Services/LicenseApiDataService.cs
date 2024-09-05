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
        
        public async Task<bool> VerifyLicense()
        {
            bool IsValid = await _client.GetCodeAsync<bool>($"licenses/verify");
            return IsValid;
        }

        public async Task<GymProfile> GetGymProfile(string GymId)
        {
            GymDto gymDto = await _client.GetAsync<GymDto>($"gyms/{GymId}");
            byte[] logo = await _client.GetByteArrayAsync("https://upload.wikimedia.org/wikipedia/commons/4/47/PNG_transparency_demonstration_1.png");
            if (logo != null)
            {
                string localFolderPath = AppDomain.CurrentDomain.BaseDirectory;
                string localPath = Path.Combine(localFolderPath, $"{gymDto.id}.jpg");
                File.WriteAllBytes(localPath, logo);
                gymDto.logo = localPath;
            }
            return gymDto.ToGymProfile();
        }
    }
}
