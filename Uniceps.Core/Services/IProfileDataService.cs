using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.Core.Services
{
    public interface IProfileDataService
    {
        Task<SystemProfile?> Get(string id);
        Task<SystemProfile> Create(SystemProfile entity);
        Task<SystemProfile> Update(SystemProfile entity);
        Task<bool> Delete(string id);
    }
}
