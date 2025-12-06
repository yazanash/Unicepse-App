using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Models;
using Uniceps.Stores.SystemAuthStores;

namespace Uniceps.Helpers
{
    public class UserContextValidator
    {
        private readonly IProfileDataStore _profileManager;
        private readonly SystemSubscriptionStore _systemSubscriptionManager;
        private readonly ISessionManager _sessionManager;
        public UserContextValidator(IProfileDataStore profileManager, ISessionManager sessionManager, SystemSubscriptionStore systemSubscriptionManager)
        {
            _profileManager = profileManager;
            _sessionManager = sessionManager;
            _systemSubscriptionManager = systemSubscriptionManager;
        }

        public async Task<UserContextState> EvaluateStageAsync()
        {
            if (!_sessionManager.IsLoggedIn())
                return UserContextState.UnAuthenticated;
            SessionData? session = _sessionManager.LoadSession();
            bool hasProfile = await _profileManager.CheckAndSyncProfileAsync(session!.BusinessId!);

            bool hasSubscription = await _systemSubscriptionManager.CheckAndSyncSubscriptionAsync();
            if (!hasSubscription)
                return UserContextState.NoSubscription;

            return UserContextState.Ready;
        }
        public string? GetId()
        {
            SessionData? session = _sessionManager.LoadSession();
            return session != null ? session.BusinessId : null;
        }
    }
}
