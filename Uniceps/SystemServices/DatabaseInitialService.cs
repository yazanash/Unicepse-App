using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.SystemServices
{
    public class DatabaseInitialService
    {
        private readonly UnicepsDbContextFactory _dbFactory;
        public DatabaseInitialService(UnicepsDbContextFactory dbFactory) => _dbFactory = dbFactory;
        public async Task SetupAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            await db.Database.MigrateAsync();
        }
    }
}
