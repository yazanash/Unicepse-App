using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;

namespace Unicepse.Entityframework.Services
{
    public class ExpensesDataService : GenericDataService<Expenses>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        public ExpensesDataService(PlatinumGymDbContextFactory contextFactory) : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
