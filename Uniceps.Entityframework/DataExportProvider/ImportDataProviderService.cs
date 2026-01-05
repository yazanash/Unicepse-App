using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Exceptions;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.Entityframework.DataExportProvider
{
    public class ImportDataProviderService : IImportDataProvider
    {
        private readonly UnicepsDbContextFactory _contextFactory;
        public ImportDataProviderService(UnicepsDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task SyncAccounting(List<Credit> credits, List<Expenses> expenses)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                context.ChangeTracker.Clear();
                foreach (Credit credit in credits)
                {
                    Employee? emp =await context.Set<Employee>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == credit.EmpPersonSyncId);
                    if(emp!=null)
                    credit.EmpPersonId = emp.Id;
                    Credit? entity = await context.Set<Credit>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == credit.SyncId);
                    if (entity != null )
                    {
                        if (entity.UpdatedAt <= credit.UpdatedAt)
                        {
                            entity.MergeWith(credit);
                            context.Set<Credit>().Update(entity);
                        }
                    }
                    else
                    {

                        EntityEntry<Credit> CreatedResult = await context.Set<Credit>().AddAsync(credit);
                    }
                }
                foreach (Expenses expense in expenses)
                {
                    Expenses? entity = await context.Set<Expenses>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == expense.SyncId);
                    if (entity != null )
                    {
                        if (entity.UpdatedAt <= expense.UpdatedAt)
                        {
                            entity.MergeWith(expense);
                            context.Set<Expenses>().Update(entity);
                        }
                     

                    }
                    else
                    {
                        EntityEntry<Expenses> CreatedResult = await context.Set<Expenses>().AddAsync(expense);
                    }
                }


                await context.SaveChangesAsync();
            }
        }
        public async Task SyncPlayerDetails(List<Metric> metrics, List<DailyPlayerReport> dailyPlayerReports)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                context.ChangeTracker.Clear();
                foreach (Metric metric in metrics)
                {
                    Player? player = await context.Set<Player>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == metric.PlayerSyncId);
                    if(player!=null)
                    metric.PlayerId = player.Id;
                    Metric? entity = await context.Set<Metric>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == metric.SyncId);
                    if (entity != null)
                    {
                        if (entity.UpdatedAt <= metric.UpdatedAt)
                        {
                            entity.MergeWith(metric);
                            context.Set<Metric>().Update(entity);
                        }
                        

                    }
                    else
                    {

                        EntityEntry<Metric> CreatedResult = await context.Set<Metric>().AddAsync(metric);
                    }
                }
                foreach (DailyPlayerReport dailyPlayerReport in dailyPlayerReports)
                {
                    Player? player = await context.Set<Player>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == dailyPlayerReport.PlayerSyncId);
                    if (player != null)
                        dailyPlayerReport.PlayerId = player.Id;
                    Subscription? subscription = await context.Set<Subscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == dailyPlayerReport.SubscriptionSyncId);
                    if (subscription != null)
                        dailyPlayerReport.SubscriptionId = subscription.Id;
                    dailyPlayerReport.Code = subscription!.Code ?? "";
                    DailyPlayerReport? entity = await context.Set<DailyPlayerReport>().AsNoTracking().FirstOrDefaultAsync((e) => e.SyncId == dailyPlayerReport.SyncId);
                    if (entity != null)
                    {
                        if (entity.UpdatedAt <= dailyPlayerReport.UpdatedAt)
                        {
                            entity.MergeWith(dailyPlayerReport);
                            context.Set<DailyPlayerReport>().Update(entity);
                        }
                      

                    }
                    else
                    {
                        EntityEntry<DailyPlayerReport> CreatedResult = await context.Set<DailyPlayerReport>().AddAsync(dailyPlayerReport);
                    }
                }


                await context.SaveChangesAsync();
            }
        }
        public async Task SyncPlayers(List<Player> players)
        {
            using var context = _contextFactory.CreateDbContext();
            context.ChangeTracker.Clear();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                foreach (var incomingPlayer in players)
                {
                    int localPlayerId = await SyncPlayer(context, incomingPlayer);
                    await ValidateSubscriptionsForPlayer(context, localPlayerId, incomingPlayer.Subscriptions.ToList());
                }


                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        private async Task<int> SyncPlayer(UnicepsDbContext context, Player incomingPlayer)
        {
            var existingPlayer = await context.Set<Player>().AsNoTracking()
                       .FirstOrDefaultAsync(p => p.SyncId == incomingPlayer.SyncId);

            if (existingPlayer != null)
            {
                // التحديث فقط إذا كان ملف الاستيراد أحدث
                if (existingPlayer.UpdatedAt <= incomingPlayer.UpdatedAt)
                {
                    existingPlayer.MergeWith(incomingPlayer);
                    context.Set<Player>().Update(existingPlayer);
                    await context.SaveChangesAsync();
                  
                }
                return existingPlayer.Id;
            }
            else
            {
                Player newPlayer = new Player();
                newPlayer.MergeWith(incomingPlayer);

                await context.Set<Player>().AddAsync(newPlayer);
                await context.SaveChangesAsync();
                return newPlayer.Id;
            }
          

        }
        private async Task ValidateSubscriptionsForPlayer(UnicepsDbContext context,int localPlayerId, List<Subscription> subscriptions)
        {
            foreach (var incomingSubscription in subscriptions)
            {
              int localSubscriptionId = await SyncSubscription(context, localPlayerId, incomingSubscription);
                if (incomingSubscription.Payments != null && incomingSubscription.Payments.Count() > 0)
                    await SyncPayments(context, localPlayerId, localSubscriptionId, incomingSubscription.Payments);

            }
        }
        private async Task<int> SyncSubscription(UnicepsDbContext context,int localPlayerId ,Subscription incomingSubscription)
        {
            var existingSubscription = await context.Set<Subscription>().AsNoTracking()
                   .FirstOrDefaultAsync(p => p.SyncId == incomingSubscription.SyncId);

            incomingSubscription.PlayerId = localPlayerId;
            Sport? sport =await context.Set<Sport>().FirstOrDefaultAsync(x => x.SyncId == incomingSubscription.SportSyncId);
            if (sport != null)
                incomingSubscription.SportId = sport.Id;
               Employee? trainer = await context.Set<Employee>().FirstOrDefaultAsync(x => x.SyncId == incomingSubscription.TrainerSyncId);
            if (trainer != null)
                incomingSubscription.TrainerId = trainer.Id;



            if (existingSubscription != null)
            {
                // التحديث فقط إذا كان ملف الاستيراد أحدث
                if (existingSubscription.UpdatedAt <= incomingSubscription.UpdatedAt)
                {
                    existingSubscription.MergeWith(incomingSubscription);
                    context.Set<Subscription>().Update(existingSubscription);
                    await context.SaveChangesAsync();
                   
                }
                return existingSubscription.Id;
            }
            else
            {
                Subscription newSubscription = new Subscription();
                newSubscription.MergeWith(incomingSubscription);

                await context.Set<Subscription>().AddAsync(newSubscription);
                await context.SaveChangesAsync();
                return newSubscription.Id;
            }
        
        }
        private async Task SyncPayments(UnicepsDbContext context,int localPlayerId,int localSubscriptionId, List<PlayerPayment> payments)
        {
            foreach (var incomingPay in payments)
            {
                var existingPay = context.Set<PlayerPayment>().AsNoTracking()
                    .FirstOrDefault(p => p.SyncId == incomingPay.SyncId);
                incomingPay.PlayerId = localPlayerId;
                incomingPay.SubscriptionId = localSubscriptionId;

                if (existingPay != null)
                {
                    if (existingPay.UpdatedAt <= incomingPay.UpdatedAt)
                    {
                        existingPay.MergeWith(incomingPay);
                    }
                }
                else
                {
                    PlayerPayment newPayment = new PlayerPayment();
                    newPayment.MergeWith(incomingPay);

                    await context.Set<PlayerPayment>().AddAsync(newPayment);
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task SyncReferences(List<Sport> sports, List<Employee> employees)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                context.ChangeTracker.Clear();
                foreach (Sport sport in sports)
                {
                    var existingSport = await context.Set<Sport>().FirstOrDefaultAsync(e => e.SyncId == sport.SyncId);
                    if (existingSport != null)
                    {
                        if (existingSport.UpdatedAt <= sport.UpdatedAt)
                        {
                            existingSport.MergeWith(sport);
                            context.Set<Sport>().Update(existingSport);
                        }
                          
                    }
                    else
                    {
                        sport.Id = 0;
                        Sport newSport = new Sport();
                        newSport.MergeWith(sport);
                        await context.Set<Sport>().AddAsync(newSport);
                    }
                }

                foreach (Employee emp in employees)
                {
                    var existingEmp = await context.Set<Employee>().FirstOrDefaultAsync(e => e.SyncId == emp.SyncId);
                    if (existingEmp != null)
                    {
                        if (existingEmp.UpdatedAt <= emp.UpdatedAt)
                        {
                            existingEmp.MergeWith(emp);
                            context.Set<Employee>().Update(existingEmp);
                        }
                           
                    }
                    else
                    {
                        Employee newEmp = new Employee();
                        newEmp.MergeWith(emp);

                        await context.Set<Employee>().AddAsync(newEmp);
                    }
                }

                await context.SaveChangesAsync();

                foreach (Sport incomingSport in sports)
                {
                    var existingSport = await context.Set<Sport>()
                        .Include(s => s.Trainers)
                        .FirstOrDefaultAsync(s => s.SyncId == incomingSport.SyncId);

                    if (existingSport != null && incomingSport.Trainers != null)
                    {
                        foreach (var incomingTrainer in incomingSport.Trainers)
                        {
                            if (!existingSport.Trainers!.Any(t => t.SyncId == incomingTrainer.SyncId))
                            {
                                var trainerInDb = await context.Set<Employee>()
                                    .FirstOrDefaultAsync(e => e.SyncId == incomingTrainer.SyncId);

                                if (trainerInDb != null)
                                    existingSport.Trainers!.Add(trainerInDb);
                            }
                        }
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task SyncRoutines(List<RoutineModel> routines)
        {
            using (UnicepsDbContext context = _contextFactory.CreateDbContext())
            {
                context.ChangeTracker.Clear();
                foreach (RoutineModel routine in routines)
                {
                   foreach(var day in routine.Days)
                    {
                        await GetItemsExercise(context, day.RoutineItems);
                    }
                    var existingRoutine = await context.Set<RoutineModel>().FirstOrDefaultAsync(e => e.SyncId == routine.SyncId);
                    if (existingRoutine != null)
                    {
                        if (existingRoutine.UpdatedAt <= routine.UpdatedAt)
                        {
                            context.Set<RoutineModel>().Update(existingRoutine);
                        }

                    }
                    else
                    {
                        await context.Set<RoutineModel>().AddAsync(routine);
                    }
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task GetItemsExercise(UnicepsDbContext context, List<RoutineItemModel> routineItemModels)
        {
            foreach (var item in routineItemModels)
            {
                if (item.Exercise != null)
                {
                    Exercises? exercises = await context.Set<Exercises>().FirstOrDefaultAsync(x => x.Tid == item.Exercise.Tid);
                    if (exercises != null)
                    {
                        item.ExerciseId = exercises.Id;
                        item.Exercise = null;
                    }
                       
                }
            }
        }
    }
}
