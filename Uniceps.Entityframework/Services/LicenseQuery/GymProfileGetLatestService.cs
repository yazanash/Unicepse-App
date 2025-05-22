using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.LicenseQuery
{
    public class GymProfileGetLatestService : IGetSingleLatestService<GymProfile>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public GymProfileGetLatestService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public GymProfile? Get()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            GymProfile? entity = context.Set<GymProfile>().FirstOrDefault();
            return entity;
        }
    }
}
