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
    public class GymPidService : IPublicIdService<GymProfile>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public GymPidService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<GymProfile?> GetByUID(string uid)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.GymId == uid);

            return entity;
        }
    }
}
