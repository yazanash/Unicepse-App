using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Services;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;
using Uniceps.Stores;

namespace Uniceps.Helpers
{
    public class ProfileManager
    {
        private readonly IProfileDataService _profileDataService;
        private readonly SystemProfileApiDataService _systemProfileApiDataService;
        private readonly AccountStore  _accountStore;
        public ProfileManager(IProfileDataService profileDataService, SystemProfileApiDataService systemProfileApiDataService, AccountStore accountStore)
        {
            _profileDataService = profileDataService;
            _systemProfileApiDataService = systemProfileApiDataService;
            _accountStore = accountStore;
        }
        public async Task<bool> CheckAndSyncProfileAsync(string businessId)
        {
            // 1. تحقق من البروفايل المحلي
            var localProfile = await _profileDataService.Get(businessId);

            // 2. إ جيبو من السيرفر
            var remoteProfile = await _systemProfileApiDataService.Get();
            if (remoteProfile.Data != null && remoteProfile.StatusCode == 200 && remoteProfile.Data != null)
            {
                SystemProfile profile = new SystemProfile()
                {
                    BusinessId = businessId,
                    PhoneNumber = remoteProfile.Data.Phone??"",
                    DisplayName = remoteProfile.Data.Name??"",
                    Gender = remoteProfile.Data.Gender,
                    BirthDate = remoteProfile.Data.DateOfBirth,
                    ProfileImagePath = remoteProfile.Data.PictureUrl
                };
                if (localProfile != null)
                {
                    profile.Id = localProfile.Id;
                    _accountStore.SystemProfile = await _profileDataService.Update(profile);
                   
                }
                else
                    _accountStore.SystemProfile = await _profileDataService.Create(profile);

                return true;
            }

            // 3. إذا ما كان موجود لا هون ولا هون
            return false;
        }

    }
}
