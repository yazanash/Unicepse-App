using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.DbContexts
{
    public class PlatinumGymDbContextFactory
    {
        private readonly string? _connectionString;

        public PlatinumGymDbContextFactory(string? connectionString)
        {
            _connectionString = connectionString;
        }

        public PlatinumGymDbContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(_connectionString).Options;

            return new PlatinumGymDbContext(options);
        }
    }
}
