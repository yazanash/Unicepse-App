using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Uniceps.API.Models;
using Uniceps.API.Services;
using Uniceps.Models;

namespace Uniceps.Helpers
{
    public class SessionManager : ISessionManager
    {
        private readonly string tokenFilePath;
        private readonly SystemAuthApiService _systemAuthApiService;
        private readonly UnicepsePrepAPIKey _apiKey;
        public SessionManager(SystemAuthApiService systemAuthApiService, UnicepsePrepAPIKey apiKey)
        {
            tokenFilePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Uniceps", "session.dat");

            Directory.CreateDirectory(Path.GetDirectoryName(tokenFilePath)!);
            _systemAuthApiService = systemAuthApiService;
            _apiKey = apiKey;
        }
        public void ClearSession()
        {
            if (File.Exists(tokenFilePath))
                File.Delete(tokenFilePath);
        }

        public string? GetToken()
        {
            return LoadSession()?.Token;
        }

        public bool IsLoggedIn()
        {
            var session = LoadSession();
            return session != null && session.Expiration > DateTime.UtcNow;
        }

        public SessionData? LoadSession()
        {
            if (!File.Exists(tokenFilePath))
                return null;

            try
            {
                var encrypted = File.ReadAllBytes(tokenFilePath);
                var decrypted = ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser);
                var json = Encoding.UTF8.GetString(decrypted);
                return JsonSerializer.Deserialize<SessionData>(json);
            }
            catch
            {
                return null;
            }
        }

        public void SaveSession(SessionData session)
        {
            var json = JsonSerializer.Serialize(session);
            var encrypted = ProtectedData.Protect(Encoding.UTF8.GetBytes(json), null, DataProtectionScope.CurrentUser);
            File.WriteAllBytes(tokenFilePath, encrypted);
        }
    }
}
