using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.Core.Services
{
    public interface ILicenseDataService : IDataService<License>
    {
        public License? ActiveLicenses();
    }
}
