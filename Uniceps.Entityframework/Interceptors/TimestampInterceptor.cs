using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;

namespace Uniceps.Entityframework.Interceptors
{
    public class TimestampInterceptor
        : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;
            if (context == null) return base.SavingChangesAsync(eventData, result);

            foreach (var entry in context.ChangeTracker.Entries<DomainObject>())
            {
                DateTime timestamp = DateTime.Now;
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = timestamp;
                    entry.Entity.UpdatedAt = timestamp;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = timestamp;
                }
            }

            return base.SavingChangesAsync(eventData, result);
        }
    }
}
