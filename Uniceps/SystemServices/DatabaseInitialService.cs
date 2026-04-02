using ClosedXML.Parser;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Uniceps.Core.Models.RoutineModels;
using Uniceps.Core.Models.TrainingProgram;
using Uniceps.Entityframework.DbContexts;

namespace Uniceps.SystemServices
{
    public class DatabaseInitialService
    {
        private readonly UnicepsDbContextFactory _dbFactory;
        public DatabaseInitialService(UnicepsDbContextFactory dbFactory) => _dbFactory = dbFactory;
        public async Task SetupAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            await db.Database.MigrateAsync();
            await MigrateLegacyExercisesAsync();
            await DeleteUnusedExercisesAsync();
        }
        public async Task MigrateLegacyExercisesAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            var routineItems = await db.Set<RoutineItemModel>()
                .Where(r => r.ExerciseId != 0 && string.IsNullOrEmpty(r.ExerciseV2Id))
                .ToListAsync();

            if (!routineItems.Any()) return;

            foreach (var item in routineItems)
            {
                var oldExercises = await db.Set<Exercises>().FirstOrDefaultAsync(x => x.Id == item.ExerciseId);
                if (oldExercises == null) continue;

                var newExerciseId = oldExercises.Id.ToString();

                var existingNew = await db.Set<ExerciseV2>().FindAsync(newExerciseId);

                if (existingNew == null)
                {
                    existingNew = new ExerciseV2
                    {
                        ExerciseId = newExerciseId ?? "",
                        Name = oldExercises.Name ?? "تمرين قديم",
                        IsLegacy = true,
                        IsActive = false,
                        Version = -1,
                        ImagePath = oldExercises.ImagePath,
                        LastUpdated = DateTime.MinValue,
                        Description = "بيانات مستوردة من النظام القديم",
                    };
                    db.Set<ExerciseV2>().Add(existingNew);
                }

                item.ExerciseV2Id = newExerciseId;
                item.ExerciseId = 0;
            }

            await db.SaveChangesAsync();


        }
        public async Task DeleteUnusedExercisesAsync()
        {
            using var db = _dbFactory.CreateDbContext();
            var oldExercisesList = await db.Set<Exercises>().ToListAsync();

            foreach (var old in oldExercisesList)
            {
                if (!string.IsNullOrEmpty(old.ImagePath) && File.Exists(old.ImagePath))
                {
                    try { File.Delete(old.ImagePath); } catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }

            db.Set<Exercises>().RemoveRange(oldExercisesList);
            await db.SaveChangesAsync();
        }

    }
}
