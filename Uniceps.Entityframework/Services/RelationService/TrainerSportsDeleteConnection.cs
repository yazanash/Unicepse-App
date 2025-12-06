using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services.RelationService
{
    public class TrainerSportsDeleteConnection : IDeleteConnectionService<Employee>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public TrainerSportsDeleteConnection(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<bool> DeleteConnection(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
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
