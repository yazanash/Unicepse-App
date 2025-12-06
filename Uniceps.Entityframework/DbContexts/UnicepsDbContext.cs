using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Serilog.Context;
using Uniceps.Core.Models.Expenses;
using Uniceps.Core.Models;
using Uniceps.Core.Models.Payment;
using Uniceps.Core.Models.DailyActivity;
using Uniceps.Core.Models.Authentication;
using Uniceps.Core.Models.Employee;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Core.Models.Subscription;
using Uniceps.Core.Models.Player;
using Uniceps.Core.Models.Sport;
using Uniceps.Core.Models.Metric;
using Uniceps.Core.Models.SyncModel;
using Uniceps.Core.Models.SystemAuthModels;

namespace Uniceps.Entityframework.DbContexts
{
    public class UnicepsDbContext : DbContext
    {
        public UnicepsDbContext(DbContextOptions options) : base(options)
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
        public DbSet<Exercises>? Exercises { get; set; }
        public DbSet<MuscleGroup>? MuscleGroups { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<Metric>? Metrics { get; set; }
        public DbSet<AuthenticationLog>? authenticationLogs { get; set; }
        public DbSet<SyncObject>? SyncObjects { get; set; }
        public DbSet<SystemProfile>? SystemProfiles { get; set; }
        public DbSet<SystemSubscription>? SystemSubscriptions { get; set; }
        public DbSet<RoutineModel>? RoutineModels { get; set; }
        public DbSet<DayGroup>? DayGroups { get; set; }
        public DbSet<RoutineItemModel>? RoutineItemModels { get; set; }
        public DbSet<SetModel>? SetModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sport>()
              .HasMany(c => c.Trainers)
              .WithMany(e => e.Sports);


            modelBuilder.Entity<User>()
          .Property(c => c.Role)
          .HasConversion<int>();

            modelBuilder.Entity<DayGroup>()
        .HasMany(w => w.RoutineItems)
        .WithOne(e => e.Day)
        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<RoutineItemModel>()
        .HasMany(w => w.Sets)
        .WithOne(e => e.RoutineItem)
        .OnDelete(DeleteBehavior.Cascade);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlayerPayment>()
    .HasOne(p => p.Subscription)
    .WithMany(s => s.Payments)
    .HasForeignKey(p => p.SubscriptionId)
    .OnDelete(DeleteBehavior.Restrict); // الحل

            modelBuilder.Entity<PlayerPayment>()
                .HasOne(p => p.Player)
                .WithMany(p => p.Payments)
                .HasForeignKey(p => p.PlayerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Subscription>()
    .HasOne<Player>()
    .WithMany(p => p.Subscriptions)
    .HasForeignKey(s => s.PlayerId)
    .OnDelete(DeleteBehavior.Restrict);
        }
    }
    public class SqliteUnicepsContext : UnicepsDbContext
    {
        public SqliteUnicepsContext(DbContextOptions options) : base(options)
        {
        }
    }

}
