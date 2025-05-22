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
    public class LicenseGetLatestService : IGetSingleLatestService<License>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public LicenseGetLatestService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public License? Get()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            License? entities = context.Set<License>().Where(x => x.SubscribeEndDate >= DateTime.Now).FirstOrDefault();
            return entities;
        }
    }
}
