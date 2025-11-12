using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;

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
                _apiKey.updateToken(session.Token,"");
                var result = await _systemAuthApiService.VerifyToken();
                return result.StatusCode == 200;
            }
            catch
            {
                return false;
            }
        }
        public string? GetBusinessId()
        {
            return _sessionManager.LoadSession()?.BusinessId;
        }
    }
}
