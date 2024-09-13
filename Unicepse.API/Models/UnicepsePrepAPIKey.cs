using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.API.Models
{
    public class UnicepsePrepAPIKey
    {
        public string Key { get; set; }
        public string GymId { get; set; }

        public UnicepsePrepAPIKey(string key)
        {
            Key = key;
        }
        public void updateToken(string token,string gymId)
        {
            Key = token;
            GymId = gymId;
        }
    }
}
