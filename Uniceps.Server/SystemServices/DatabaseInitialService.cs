using Microsoft.EntityFrameworkCore;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Server.SystemServices
{
    public class DatabaseInitialService
    {
        private readonly UnicepsDbContextFactory _factory;

        public DatabaseInitialService(UnicepsDbContextFactory factory)
        {
            _factory = factory;
        }

        public async Task InitializeAsync()
        {
            using var context = _factory.CreateDbContext();

            await context.Database.MigrateAsync();

            // Seed data
            // Default roles
            // Default settings
        }
    }
}
