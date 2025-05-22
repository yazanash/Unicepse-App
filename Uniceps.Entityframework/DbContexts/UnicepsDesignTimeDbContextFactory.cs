using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.DbContexts
{
    public class UnicepsDesignTimeDbContextFactory : IDesignTimeDbContextFactory<UnicepsDbContext>
    {
        public UnicepsDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer("data source=.\\sqlexpress;initial catalog=Uniceps; integrated security=SSPI ; TrustServerCertificate=True;").Options;

            return new UnicepsDbContext(options);

        }
    }
    public class UnicepseDesignTimeDbContextFactory : IDesignTimeDbContextFactory<SqliteUnicepsContext>
    {
        public SqliteUnicepsContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlite(@"data source=Uniceps.db").Options;

            return new SqliteUnicepsContext(options);

        }
    }
}
