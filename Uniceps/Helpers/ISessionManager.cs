using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Models;

namespace Uniceps.Helpers
{
    public interface ISessionManager
    {
       public void SaveSession(SessionData session);
        public SessionData? LoadSession();
        public bool IsLoggedIn();
        public void ClearSession();
        public string? GetToken();
    }
}
