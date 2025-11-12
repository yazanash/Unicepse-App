using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Common;
using Uniceps.Entityframework.Services.AuthService;
using Uniceps.Models;

namespace Uniceps.Helpers
{
    public class UserContextValidator
    {
        private readonly ProfileManager _profileManager;
        private readonly SystemSubscriptionManager _systemSubscriptionManager;
        private readonly ISessionManager _sessionManager;
        private readonly IAuthenticationService _authenticationService;
        public UserContextValidator(ProfileManager profileManager, ISessionManager sessionManager, IAuthenticationService authenticationService, SystemSubscriptionManager systemSubscriptionManager)
        {
            _profileManager = profileManager;
            _sessionManager = sessionManager;
            _authenticationService = authenticationService;
            _systemSubscriptionManager = systemSubscriptionManager;
        }

        public async Task<UserContextState> EvaluateStageAsync()
        {
            // تحقق من وجود بروفايل (محلي أو سيرفر)
            if (!_sessionManager.IsLoggedIn())
                return UserContextState.UnAuthenticated;
            SessionData? session = _sessionManager.LoadSession();
            bool hasProfile = await _profileManager.CheckAndSyncProfileAsync(session!.BusinessId!);
            if (!hasProfile)
                return UserContextState.NoProfile;

            bool hasSubscription = await _systemSubscriptionManager.CheckAndSyncSubscriptionAsync();
            if (!hasSubscription)
                return UserContextState.NoSubscription;

            //bool hasLocalUser = await _authenticationService.HasUsers();
            //if (!hasLocalUser)
            //    return UserContextState.NoLocalUser;

            return UserContextState.Ready;
        }
        public string? GetId()
        {
            SessionData? session = _sessionManager.LoadSession();
            return session != null ? session.BusinessId : null;
        }
    }
}
