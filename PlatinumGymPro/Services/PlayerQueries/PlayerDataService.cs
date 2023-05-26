using Microsoft.EntityFrameworkCore;
using PlatinumGymPro.DbContexts;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services.PlayerQueries
{
    public class PlayerDataService<T> : GenericDataService<T> where T : Player
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public PlayerDataService(PlatinumGymDbContextFactory contextFactory)
            : base(contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<IEnumerable<Player>> GetByStatus(bool status)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<T>? entities = await context.Set<T>().Where(x=>x.IsSubscribed==status).ToListAsync();
                return entities;
            }
        }
    }
}
