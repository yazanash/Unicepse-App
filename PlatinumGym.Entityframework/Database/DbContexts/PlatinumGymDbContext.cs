using Microsoft.EntityFrameworkCore;
using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.DbContexts
{
    public class PlatinumGymDbContext : DbContext
    {
        public PlatinumGymDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Player>? Players { get; set; }
        public DbSet<PlayerPayment>? PlayerPayments { get; set; }
        public DbSet<PlayerTraining>? PlayerTrainings { get; set; }
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
         
            base.OnModelCreating(modelBuilder);
        }
    }
}
