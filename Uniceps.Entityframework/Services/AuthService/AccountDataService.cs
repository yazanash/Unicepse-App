using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Uniceps.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Services;
using Uniceps.Core.Common;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.Services.AuthService
{
    public class AccountDataService : IAccountDataService<User>
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AccountDataService(UnicepsDbContextFactory contextFactory, IPasswordHasher<User> passwordHasher)
        {
            _contextFactory = contextFactory;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> Create(User entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User existed_user = await GetByUsername(entity.UserName!);
            if (existed_user != null)
                throw new ConflictException("هذا المستخدم موجود بالفعل");

            string pass = _passwordHasher.HashPassword(entity, entity.Password);
            entity.Password = pass;
            EntityEntry<User> CreatedResult = await context.Set<User>().AddAsync(entity);
            await context.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User entityToDelete = await Get(id);
            if (entityToDelete == null)
                throw new NotExistException();
            if (context.Users!.Count() < 2)
            {
                throw new Exception("لا يمكن حذف مستخدم في حال عدم وجود مستخدمين اخرين");

            }
            if (entityToDelete.Role == Roles.Admin)
            {
                if (context.Users!.Where(x => x.Role == Roles.Admin).Count() < 2)
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
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException("هذا المستخدم غير موجود");
            return entity!;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<User>? entities = await context.Set<User>().ToListAsync();
            return entities;
        }
        public async Task<bool> HasUsers()
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            bool entities = await context.Set<User>().AnyAsync();
            return entities;
        }

        public async Task<User> Update(User entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا المستخدم غير موجود");
            if (entity.Role != Roles.Admin)
                if (context.Users!.Where(x => x.Role == Roles.Admin).Count() == 1 && entityToUpdate.Role == Roles.Admin)
                {
                    throw new Exception("يجب ان يكون هناك على الاقل مدير واحد للنظام");

                }
            string pass = _passwordHasher.HashPassword(entity, entity.Password);
            entity.Password = pass;

            context.Set<User>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
        public async Task<User> Disable(User entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User entityToUpdate = await Get(entity.Id);
            if (entityToUpdate == null)
                throw new NotExistException("هذا المستخدم غير موجود");
            entity.Disable = true;
            context.Set<User>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public void AuthenticationLogging(AuthenticationLog entity)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            context.Entry(entity.User!).State = EntityState.Detached;
            if (context.Entry(entity.User!).State == EntityState.Detached)
                context.Entry(entity.User!).State = EntityState.Unchanged;
            EntityEntry<AuthenticationLog> CreatedResult = context.Set<AuthenticationLog>().Add(entity);
            context.SaveChanges();
        }
        public async Task<IEnumerable<AuthenticationLog>> GetAllAuthenticationLogging(DateTime date)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();

            IEnumerable<AuthenticationLog>? entities = await context.Set<AuthenticationLog>().Include(x => x.User).AsNoTracking().Where(x =>
            x.LoginDateTime.Day == date.Day &&
             x.LoginDateTime.Year == date.Year &&
             x.LoginDateTime.Month == date.Month).ToListAsync();
            return entities;
        }
        public async Task<User> GetByUsername(string username)
        {
            using UnicepsDbContext context = _contextFactory.CreateDbContext();
            User? entity = await context.Set<User>().FirstOrDefaultAsync((e) => e.UserName == username);
            return entity!;
        }

    }
}
