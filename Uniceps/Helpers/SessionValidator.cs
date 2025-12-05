using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.BackgroundServices;

namespace Uniceps.Helpers
{
    public class SessionValidator
    {
        private readonly ISessionManager _sessionManager;
        private readonly SystemAuthApiService _systemAuthApiService;
        private readonly UnicepsePrepAPIKey _apiKey;
        public SessionValidator(ISessionManager sessionManager, SystemAuthApiService systemAuthApiService, UnicepsePrepAPIKey apiKey)
        {
            _sessionManager = sessionManager;
            _systemAuthApiService = systemAuthApiService;
            _apiKey = apiKey;
        }

        public async Task<bool> HasValidSession()
        {
            var session = _sessionManager.LoadSession();
            if (session == null || string.IsNullOrEmpty(session.Token))
                return false;

            try
            {
                bool internetAvailable = InternetAvailability.IsInternetAvailable();
                if (internetAvailable)
                {
                    _apiKey.updateToken(session.Token, "");
                    var result = await _systemAuthApiService.VerifyToken();
                    if( result.StatusCode == 200)
                    {
                        return true;
                    }
                    _sessionManager.ClearSession();
                    return false;
                }
                else
                {
                    return _sessionManager.IsLoggedIn();
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
