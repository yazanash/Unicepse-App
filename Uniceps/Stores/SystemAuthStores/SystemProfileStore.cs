using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.common;
using Uniceps.API.Services;
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
        private readonly UserFlowService _userFlowService;
        public SystemProfileStore(IProfileDataService profileDataService, AccountStore accountStore, SystemProfileApiDataService systemProfileApiDataService, UserFlowService userFlowService)
        {
            _profileDataService = profileDataService;
            _accountStore = accountStore;
            _systemProfileApiDataService = systemProfileApiDataService;
            _userFlowService = userFlowService;
        }

        public event Action<SystemProfile>? Created;
        public event Action? Loaded;
        public event Action<SystemProfile>? Updated;
        public event Action<int>? Deleted;
        public async Task Add(SystemProfile entity)
        {
          var apiResponse =   await _systemProfileApiDataService.Create(entity);
            if (apiResponse.StatusCode == 201|| apiResponse.StatusCode == 200)
            {
                entity.BusinessId = _accountStore.BusinessId;
                await _profileDataService.Create(entity);
                await _userFlowService.RefreshUserContextAsync();
                Created?.Invoke(entity);
            }
        }
        public async Task Update(SystemProfile entity)
        {
            await _profileDataService.Update(entity);
            Updated?.Invoke(entity);
        }
    }
}
