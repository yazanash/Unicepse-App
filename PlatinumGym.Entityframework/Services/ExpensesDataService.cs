using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Models.Expenses;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;

namespace PlatinumGym.Entityframework.Services
{
    public class ExpensesDataService : GenericDataService<Expenses>
    {

        public ExpensesDataService(PlatinumGymDbContextFactory contextFactory) : base(contextFactory)
        {

        }
    }
}
