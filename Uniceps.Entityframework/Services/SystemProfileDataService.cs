using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services
{
    public class SystemProfileDataService : IProfileDataService
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SystemProfileDataService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<SystemProfile> Create(SystemProfile entity)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<SystemProfile> CreatedResult = await context.Set<SystemProfile>().AddAsync(entity);
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(string id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SystemProfile? entity = await context.Set<SystemProfile>().FirstOrDefaultAsync((e) => e.BusinessId == id);
            if (entity == null)
                throw new NotExistException("هذا الحساب غير موجود");
            context.Set<SystemProfile>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<SystemProfile?> Get(string id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            SystemProfile? entity = await context.Set<SystemProfile>().AsNoTracking().FirstOrDefaultAsync((e) => e.BusinessId == id);
            return entity;
        }

        public async Task<SystemProfile> Update(SystemProfile entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Set<SystemProfile>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
