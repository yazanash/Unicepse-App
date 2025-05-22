using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.PublicIdServices
{
    public class GymPidService : IPublicIdService<GymProfile>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public GymPidService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<GymProfile?> GetByUID(string uid)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = await context.Set<GymProfile>().FirstOrDefaultAsync((e) => e.GymId == uid);

            return entity;
        }
    }
}
