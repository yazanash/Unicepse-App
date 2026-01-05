using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;
using Uniceps.Helpers;

namespace Uniceps.Stores.SystemAuthStores
{
    public class SystemProfileStore : IProfileDataStore
    {
        private readonly IProfileDataService _profileDataService;
        private readonly AccountStore _accountStore;
        private readonly SystemProfileApiDataService _systemProfileApiDataService;
        public SystemProfileStore(IProfileDataService profileDataService, AccountStore accountStore, SystemProfileApiDataService systemProfileApiDataService)
        {
            _profileDataService = profileDataService;
            _accountStore = accountStore;
            _systemProfileApiDataService = systemProfileApiDataService;
        }

        public event Action<SystemProfile>? Created;
        public event Action<SystemProfile>? Updated;
        public async Task CreateOrUpdate(SystemProfile entity)
        {
            if (string.IsNullOrEmpty(_accountStore.BusinessId))
                throw new Exception("يرجى تسجيل الدخول لانشاء ملف تعريفي");

            var localProfile = await _profileDataService.Get(_accountStore.BusinessId);
            if (localProfile != null)
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    var apiResponse = await _systemProfileApiDataService.Update(entity);
                    if (apiResponse.StatusCode == 201 || apiResponse.StatusCode == 200)
                    {
                        entity.BusinessId = _accountStore.BusinessId;
                        entity.Id = localProfile.Id;
                        await _profileDataService.Update(entity);
                        Updated?.Invoke(entity);
                    }
                    else if (apiResponse.StatusCode == 401)
                    {
                        throw new Exception("انت غير مسجل يرجى التسجيل الدخول");
                    }
                }
                else
                {
                    throw new Exception("لا يوجد اتصال بالانترنت .... حاول مرة اخرى");
                }
            }
            else
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    var apiResponse = await _systemProfileApiDataService.Create(entity);
                    if (apiResponse.StatusCode == 201 || apiResponse.StatusCode == 200)
                    {
                        entity.BusinessId = _accountStore.BusinessId;
                        await _profileDataService.Create(entity);
                        Created?.Invoke(entity);
                    }
                    else if (apiResponse.StatusCode == 401)
                    {
                        throw new Exception("انت غير مسجل يرجى التسجيل الدخول");
                    }
                }
                else
                {
                    throw new Exception("لا يوجد اتصال بالانترنت .... حاول مرة اخرى");
                }
            }

        }

        public async Task<bool> CheckAndSyncProfileAsync(string businessId)
        {
            var localProfile = await _profileDataService.Get(businessId);
            try
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    var remoteProfile = await _systemProfileApiDataService.Get();
                    if (remoteProfile.Data != null && remoteProfile.StatusCode == 200 && remoteProfile.Data != null)
                    {
                        SystemProfile profile = new SystemProfile()
                        {
                            BusinessId = businessId,
                            PhoneNumber = remoteProfile.Data.Phone ?? "",
                            DisplayName = remoteProfile.Data.Name ?? "",
                            OwnerName = remoteProfile.Data.OwnerName ?? "",
                            Address = remoteProfile.Data.Address ?? "",
                            Gender = remoteProfile.Data.Gender,
                            BirthDate = remoteProfile.Data.DateOfBirth,
                            ProfileImagePath = remoteProfile.Data.PictureUrl
                        };
                        if (!string.IsNullOrEmpty(profile.ProfileImagePath))
                        {
                            profile.LocalProfileImagePath = await _systemProfileApiDataService.DownloadAndSaveProfilePicture(profile.ProfileImagePath);
                        }
                        if (localProfile != null)
                        {
                            profile.Id = localProfile.Id;
                            _accountStore.SystemProfile = await _profileDataService.Update(profile);

                        }
                        else
                            _accountStore.SystemProfile = await _profileDataService.Create(profile);

                        return true;
                    }
                }
                else
                {
                    if (localProfile != null)
                    {
                        _accountStore.SystemProfile = localProfile;
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                if (localProfile != null)
                {
                    _accountStore.SystemProfile = localProfile;
                    return true;

                }
                return false;
            }
        }
        public async Task UploadProfilePicture(string filePath)
        {
            if (_accountStore.SystemProfile != null)
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    var apiResponse = await _systemProfileApiDataService.UploadProfilePicture(filePath);
                    if (apiResponse.StatusCode == 201 || apiResponse.StatusCode == 200)
                    {
                        string? imageName = apiResponse.Data?.ImageUrl;
                        if (string.IsNullOrEmpty(imageName))
                            throw new Exception("API did not return a valid image name.");

                        // حفظ نسخة محلية.

                        _accountStore.SystemProfile.LocalProfileImagePath = await _systemProfileApiDataService.DownloadAndSaveProfilePicture(imageName);
                        _accountStore.SystemProfile = await _profileDataService.Update(_accountStore.SystemProfile);
                    }
                }
                else
                {
                    throw new Exception("لا يوجد اتصال بالانترنت .... حاول مرة اخرى");
                }
            }
        }


    }
}
