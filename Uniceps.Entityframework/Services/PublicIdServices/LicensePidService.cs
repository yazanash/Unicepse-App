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
    public class LicensePidService : IPublicIdService<License>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public LicensePidService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<License?> GetByUID(string uid)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            License? entity = await context.Set<License>().FirstOrDefaultAsync((e) => e.LicenseId == uid);
            return entity!;
        }
    }
}
