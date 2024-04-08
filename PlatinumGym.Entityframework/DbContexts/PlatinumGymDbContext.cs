using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Expenses;
using PlatinumGym.Core.Models.Metric;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Core.Models.TrainingProgram;
using PlatinumGym.Entityframework.ModelsConigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.DbContexts
{
    public class PlatinumGymDbContext : DbContext
    {
        public PlatinumGymDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Player>? Players { get; set; }
        public DbSet<PlayerPayment>? PlayerPayments { get; set; }
        public DbSet<Subscription>? Subscriptions { get; set; }
        public DbSet<Employee>? Employees { get; set; }
        public DbSet<Credit>? Credit { get; set; }
        public DbSet<DailyPlayerReport>? DailyPlayerReport { get; set; }
        public DbSet<Expenses>? Expenses { get; set; }
        //public DbSet<Offer>? Offer { get; set; }
        public DbSet<PayReferance>? PayReferance { get; set; }
        public DbSet<Sport>? Sports { get; set; }
        public DbSet<TrainerDueses>? TrainerDueses { get; set; }
        public DbSet<Exercises>? Exercises { get; set; }
        public DbSet<RoutineItems>? RoutineItems { get; set; }
        public DbSet<PlayerRoutine>? PlayerRoutine { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Metric>? Metrics { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // modelBuilder
            //.Entity<PlayerRoutine>()
            //.OwnsMany(routine => routine.RoutineSchedule, builder => { builder.ToJson(); });
            // modelBuilder.Entity<PlayerRoutine>()
            //         .Property(e => e.RoutineSchedule)
            //         .HasConversion((itemData) => JsonConvert.SerializeObject(itemData), str => JsonConvert.DeserializeObject<List<RoutineItems>>(str)!);

            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ExercisesConfiguration());
            modelBuilder.ApplyConfiguration(new ExpensesConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerConfiguration());
            modelBuilder.ApplyConfiguration(new PlayerPaymentsConfiguration());
            modelBuilder.ApplyConfiguration(new RoutineItemsConfiguration());
            modelBuilder.ApplyConfiguration(new SubscriptionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new SportConfiguration());
            modelBuilder.Entity<PlayerRoutine>()
         .Property(c => c.DaysGroupMap)
         .HasConversion(
             v => JsonConvert.SerializeObject(v),
             v => JsonConvert.DeserializeObject<Dictionary<int, List<int>>>(v)!);

            modelBuilder.Entity<Sport>()
              .HasMany(c => c.Trainers)
              .WithMany(e => e.Sports);

            modelBuilder.Entity<Subscription>()
              .HasOne(c => c.Sport);

            modelBuilder.Entity<Subscription>()
             .HasOne(c => c.Player);

            modelBuilder.Entity<Subscription>()
             .HasOne(c => c.Trainer)
             .WithMany(c => c.PlayerTrainings).HasForeignKey(x => x.TrainerId);

            string fileName = "Training.json";
            string jsonString = File.ReadAllText(fileName);
            List<Exercises> exercises = JsonConvert.DeserializeObject<List<Exercises>>(jsonString)!;

            modelBuilder.Entity<Exercises>().HasData(exercises);

            base.OnModelCreating(modelBuilder);
        }
    }
    class SportConfiguration : IEntityTypeConfiguration<Sport>
    {
        public void Configure(EntityTypeBuilder<Sport> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar(4000)");
        }
    }
}
