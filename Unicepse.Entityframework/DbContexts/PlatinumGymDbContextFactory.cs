using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Entityframework.Interceptors;

namespace Unicepse.Entityframework.DbContexts
{
    public class PlatinumGymDbContextFactory
    {
        private readonly string? _connectionString;
        private readonly bool _useSqlite;
        public PlatinumGymDbContextFactory(string? connectionString,bool useSqlite)
        {
            _connectionString = connectionString;
            _useSqlite = useSqlite;
        }

        public PlatinumGymDbContext CreateDbContext()
        {

            DbContextOptionsBuilder options = new DbContextOptionsBuilder().UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                .AddInterceptors(new TimestampInterceptor());

            if (_useSqlite)
            {
                options.UseSqlite(_connectionString);
                return new SqliteUnicepsContext(options.Options);
            }
            else
            {
                options.UseSqlServer(_connectionString);
                return new PlatinumGymDbContext(options.Options);
            }
               

            
        }
    }
}
