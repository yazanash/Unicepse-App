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
    public class GymProfileGetLatestService : IGetSingleLatestService<GymProfile>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public GymProfileGetLatestService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public GymProfile? Get()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = context.Set<GymProfile>().FirstOrDefault();
            return entity;
        }
    }
}
