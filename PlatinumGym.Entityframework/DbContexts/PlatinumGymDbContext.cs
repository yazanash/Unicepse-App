using Microsoft.EntityFrameworkCore;
using PlatinumGym.Core.Models.Authentication;
using PlatinumGym.Core.Models.DailyActivity;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Expenses;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Models.Player;
using PlatinumGym.Core.Models.Sport;
using PlatinumGym.Core.Models.Subscription;
using PlatinumGym.Core.Models.TrainingProgram;
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
        public DbSet<TrainingProgram>? TrainingProgram { get; set; }
        public DbSet<Training>? Training { get; set; }
        public DbSet<TrainingCategory>? TrainingCategory { get; set; }
        public DbSet<User>? Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Sport>()
            //  .HasMany(c => c.Trainers)
            //  .WithMany(e => e.Sports);

            //modelBuilder.Entity<Subscription>()
            //  .HasOne(c => c.Sport);

            //modelBuilder.Entity<Subscription>()
            // .HasOne(c => c.Player);

            //modelBuilder.Entity<Subscription>()
            // .HasOne(c => c.Trainer);
            base.OnModelCreating(modelBuilder);
        }
    }
}
