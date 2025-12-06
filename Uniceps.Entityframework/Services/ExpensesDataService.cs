using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Models.Expenses;
using Uniceps.Entityframework.DbContexts;
using Uniceps.Core.Services;

namespace Uniceps.Entityframework.Services
{
    public class ExpensesDataService : GenericDataService<Expenses>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public ExpensesDataService(UnicepsDbContextFactory contextFactory) : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }
    }
}
