using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.LicenseQuery
{
    public class LicenseGetLatestService : IGetSingleLatestService<License>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public LicenseGetLatestService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public License? Get()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            License? entities =  context.Set<License>().Where(x => x.SubscribeEndDate >= DateTime.Now).FirstOrDefault();
            return entities;
        }
    }
}
