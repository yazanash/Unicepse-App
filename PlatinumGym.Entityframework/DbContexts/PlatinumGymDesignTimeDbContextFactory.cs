using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.DbContexts
{
    public class PlatinumGymDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PlatinumGymDbContext>
    {
        public PlatinumGymDbContext CreateDbContext(string[] args)
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(@"data source=.\sqlexpress;initial catalog=PlatinumDBD; integrated security=SSPI ; TrustServerCertificate=True;").Options;

            return new PlatinumGymDbContext(options);
        }
    }
}
