using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Exceptions;

namespace Uniceps.Entityframework.Services.RelationService
{
    public class SportTrainersDeleteConnection : IDeleteConnectionService<Sport>
    {
        private readonly UnicepsDbContextFactory _contextFactory;

        public SportTrainersDeleteConnection(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> DeleteConnection(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            Sport? entity = await context.Set<Sport>().AsNoTracking().Include(x => x.Trainers).AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذه الرياضة غير موجودة");
            foreach (var trainer in entity.Trainers!)
            {
                context.Attach(trainer);
                entity.Trainers!.Remove(trainer);
            }

            context.Set<Sport>().Update(entity!);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
