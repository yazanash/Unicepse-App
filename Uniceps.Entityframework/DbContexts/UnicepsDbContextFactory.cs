using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Interceptors;

namespace Uniceps.Entityframework.DbContexts
{
    public class UnicepsDbContextFactory
    {
        private readonly string? _connectionString;
        private readonly bool _useSqlite;
        public UnicepsDbContextFactory(string? connectionString, bool useSqlite)
        {
            _connectionString = connectionString;
            _useSqlite = useSqlite;
        }

        public UnicepsDbContext CreateDbContext()
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
                return new UnicepsDbContext(options.Options);
            }



        }
    }
}
