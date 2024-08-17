using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services
{
    public class GymProfileDataService : GenericDataService<GymProfile>
    {
        public GymProfileDataService(PlatinumGymDbContextFactory contextFactory) : base(contextFactory)
        {
        }
    }
}
