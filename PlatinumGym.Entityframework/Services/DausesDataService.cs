using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
{
    public class DausesDataService : IDataService<TrainerDueses>
    {
        private readonly PlatinumGymDbContextFactory _contextFactory;

        public DausesDataService(PlatinumGymDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<TrainerDueses> Create(TrainerDueses entity)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                EntityEntry<TrainerDueses> CreatedResult = await context.Set<TrainerDueses>().AddAsync(entity);
                if (entity.Trainer != null)
                {
                    context.Attach(entity.Trainer);
                }
                await context.SaveChangesAsync();
                return CreatedResult.Entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            TrainerDueses? entity = await context.Set<TrainerDueses>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            context.Set<TrainerDueses>().Remove(entity!);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<TrainerDueses> Get(int id)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            TrainerDueses? entity = await context.Set<TrainerDueses>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new NotExistException();
            return entity!;
        }

        public async Task<IEnumerable<TrainerDueses>> GetAll()
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TrainerDueses>? entities = await context.Set<TrainerDueses>().AsNoTracking().ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<TrainerDueses>> GetAll(Employee employee)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TrainerDueses>? entities = await context.Set<TrainerDueses>().AsNoTracking()
                    .Where(x=>x.Trainer!.Id==employee.Id).ToListAsync();
                return entities;
            }
        }
        public async Task<IEnumerable<TrainerDueses>> GetAll(Employee employee,DateTime date)
        {
            using (PlatinumGymDbContext context = _contextFactory.CreateDbContext())
            {
                IEnumerable<TrainerDueses>? entities = await context.Set<TrainerDueses>().AsNoTracking()
                    .Where(x => x.Trainer!.Id == employee.Id&& x.IssueDate.Month==date.Month&&x.IssueDate.Year==date.Year).ToListAsync();
                return entities;
            }
        }
        public double GetParcent(PlayerPayment entity, DateTime date)
        {
            if (entity.PayDate.Month == date.Month - 1)
            {
                DateTime firstDayInMonth = new DateTime(date.Year,date.Month,1);
                if (entity.To <= date)
                {
                    int days = (int)entity.To.Subtract(firstDayInMonth).TotalDays+1;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return total ;
                }
                else if (entity.To > date)
                {
                    int days = (int)date.Subtract(firstDayInMonth).TotalDays ;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return total;
                }
            }
            else if (entity.From <= date)
            {
                if (entity.To <= date)
                    return (entity.PaymentValue);
                else if (entity.To > date)
                {
                    int days = (int) date.Subtract(entity.From).TotalDays;
                       double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return total;
                }
            }


            return 0;
        }

        public async Task<TrainerDueses> Update(TrainerDueses entity)
        {
            using PlatinumGymDbContext context = _contextFactory.CreateDbContext();
            TrainerDueses existed_employee = await Get(entity.Id);
            if (existed_employee == null)
                throw new NotExistException();
            context.Set<TrainerDueses>().Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
