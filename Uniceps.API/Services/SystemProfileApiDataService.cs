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
                Phone = systemProfile.PhoneNumber,
                Address = systemProfile.Address,
                OwnerName = systemProfile.OwnerName,
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
        public async Task<ApiResponse<ProfilePictureResponse>> UploadProfilePicture(string filePath)
        {
            return await _client.PostPictureAsync<string, ProfilePictureResponse>("ProfilePicture", filePath);
        }
        public async Task<string> DownloadAndSaveProfilePicture(string imageName)
        {
            string folder = GetLocalFolder();
            string localPath = Path.Combine(folder, imageName);

            if (File.Exists(localPath))
                return localPath;

            var response = await _client.GetByteArrayAsync($"ProfilePicture/{imageName}");
            if (response.StatusCode != 200 || response.Data == null)
                throw new Exception("Failed to download profile picture: " + response.ErrorMessage);

            await File.WriteAllBytesAsync(localPath, response.Data);
            return localPath;
        }
        private string GetLocalFolder()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Uniceps",
                "ProfilePictures");

            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            return folder;
        }
    }
}
