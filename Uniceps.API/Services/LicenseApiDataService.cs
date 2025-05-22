using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API;
using Uniceps.API.Models;
using Uniceps.Core.Models;
using Uniceps.Core.Exceptions;

namespace Uniceps.API.Services
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
            if (licenseDto == null)
                throw new NotExistException("هذا المفتاح غير موجود");
            return licenseDto.ToLicense();
        }
        public async Task<License> GetLicenseById(string id)
        {
            LicenseDto licenseDto = await _client.GetAsync<LicenseDto>($"licenses/get-info");
            if (licenseDto == null)
                throw new NotExistException("هذا الترخيص غير موجود");
            return licenseDto.ToLicense();
        }
        public async Task<int> VerifyLicense()
        {
            int IsValid = await _client.GetCodeAsync<bool>($"licenses/verify");
            return IsValid;
        }

        public async Task<GymProfile> GetGymProfile(string GymId)
        {

            GymDto gymDto = await _client.GetAsync<GymDto>($"gyms/{GymId}");
            byte[]? logo = await _client.GetByteArrayAsync($"gyms/logos/{GymId}");
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
