using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services.AuthService
{
    public class AccountDataService : IAccountDataService<User>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public AccountDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<User> Create(User entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User existed_user = await GetByUsername(entity.UserName!);
            if (existed_user != null)
                throw new ConflictException();
            EntityEntry<User> CreatedResult = await context.Set<User>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.Id == id);
            context.Set<User>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<User> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<User>? entities = await context.Set<User>().ToListAsync();
            return entities;
        }

        public async Task<User> Update(User entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException();
            context.Set<User>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<User> GetByUsername(string username)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.UserName == username);
            return entity!;
        }
    }
}
