using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services.RelationService
{
    public class TrainerSportsDeleteConnection : IDeleteConnectionService<Employee>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public TrainerSportsDeleteConnection(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> DeleteConnection(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            Employee? entity = await context.Set<Employee>().AsNoTracking().Include(x => x.Sports).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا الموظف غير موجود");
            foreach (var sport in entity.Sports!)
            {
                context.Attach(sport);
                entity.Sports!.Remove(sport);
            }
            context.Set<Employee>().Update(entity!);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
