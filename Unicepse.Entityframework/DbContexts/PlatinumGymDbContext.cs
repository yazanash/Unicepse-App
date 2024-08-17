using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unicepse.Core.Models.Authentication;
using Unicepse.Core.Models.DailyActivity;
using Unicepse.Core.Models.Employee;
using Unicepse.Core.Models.Expenses;
using Unicepse.Core.Models.Metric;
using Unicepse.Core.Models.Payment;
using Unicepse.Core.Models.Player;
using Unicepse.Core.Models.Sport;
using Unicepse.Core.Models.Subscription;
using Unicepse.Core.Models.TrainingProgram;
using Unicepse.Entityframework.ModelsConigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Unicepse.Core.Models;

namespace Unicepse.Entityframework.DbContexts
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
        public DbSet<License>?  Licenses { get; set; }
        public DbSet<GymProfile>? GymProfile { get; set; }

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
