using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;

namespace Unicepse.Core.Services
{
    public interface ILicenseDataService : IDataService<License>
    {
        Task<License> GetById(string id);
        public License? ActiveLicenses();
    }
}
