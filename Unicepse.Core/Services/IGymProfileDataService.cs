using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;

namespace Unicepse.Core.Services
{
    public interface IGymProfileDataService: IDataService<GymProfile>
    {
        Task<GymProfile?> GetByGymID(string id);
        Task<GymProfile?> Get();
    }
}
