using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.API.Models
{
    public class UnicepsePrepAPIKey
    {
        public string Key { get; }

        public UnicepsePrepAPIKey(string key)
        {
            Key = key;
        }
    }
}
