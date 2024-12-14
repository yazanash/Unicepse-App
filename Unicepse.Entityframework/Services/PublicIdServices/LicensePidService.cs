using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.PublicIdServices
{
    public class LicensePidService : IPublicIdService<License>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public LicensePidService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<License?> GetByUID(string uid)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.LicenseId == uid);
            return entity!;
        }
    }
}
