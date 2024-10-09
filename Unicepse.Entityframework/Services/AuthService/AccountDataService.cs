using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Unicepse.Core.Exceptions;
using Unicepse.Core.Models.Authentication;
using Unicepse.Core.Services;
using Unicepse.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.Services.AuthService
{
    public class AccountDataService : IAccountDataService<User>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;
        private readonly IPasswordHasher _passwordHasher;
        public AccountDataService(PlatinumGymDbContextFactory contextFactory, IPasswordHasher passwordHasher)
        {
            _contextFactory = contextFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Create(User entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User existed_user = await GetByUsername(entity.UserName!);
            if (existed_user != null)
                throw new ConflictException();
           
            string pass = _passwordHasher.HashPassword(entity.Password);
            entity.Password = pass;
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
            if (context.Users!.Count() < 2)
            {
                throw new Exception("لا يمكن حذف مستخدم في حال عدم وجود مستخدمين اخرين");

            }
            if (entityToDelete.Role == Core.Common.Roles.Admin)
            {
                if (context.Users!.Where(x => x.Role == Core.Common.Roles.Admin).Count() < 2)
                {
                    throw new Exception("يجب ان يكون هناك على الاقل مدير واحد للنظام");

                }
            }
           
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
        public bool HasUsers()
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            bool entities = context.Set<User>().Any();
            return entities;
        }

        public async Task<User> Update(User entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException();
            if(entity.Role != Core.Common.Roles.Admin)
            if (context.Users!.Where(x => x.Role==Core.Common.Roles.Admin).Count() == 1 && entityToUpdate.Role==Core.Common.Roles.Admin)
            {
                throw new Exception("يجب ان يكون هناك على الاقل مدير واحد للنظام");

            }
            string pass = _passwordHasher.HashPassword(entity.Password);
            entity.Password = pass;
           
            context.Set<User>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<User> Disable(User entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException();
            entity.Disable = true;
            context.Set<User>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public void AuthenticationLogging(AuthenticationLog entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            context.Entry(entity.User!).State = EntityState.Detached;
            if (context.Entry(entity.User!).State == EntityState.Detached)
                context.Entry(entity.User!).State = EntityState.Unchanged;
            EntityEntry<AuthenticationLog> CreatedResult =  context.Set<AuthenticationLog>().Add(entity);
            context.SaveChanges();
        }
        public async Task<IEnumerable<AuthenticationLog>> GetAllAuthenticationLogging(DateTime date)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<AuthenticationLog>? entities = await context.Set<AuthenticationLog>().Include(x=>x.User).AsNoTracking().Where(x=>
            x.LoginDateTime.Day== date.Day &&
             x.LoginDateTime.Year == date.Year && 
             x.LoginDateTime.Month == date.Month).ToListAsync();
            return entities;
        }
        public async Task<User> GetByUsername(string username)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.UserName == username);
            return entity!;
        }

    }
}
