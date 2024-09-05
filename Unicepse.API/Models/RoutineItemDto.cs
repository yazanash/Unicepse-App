using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;

namespace Unicepse.API.Models
{
    public class RoutineItemDto
    {
        public int id { get; set; }
        public int ExerciseId { get; set; }
        public string? ExerciseName { get; set; }
        public string? ExerciseImage { get; set; }
        public int Muscle_Group { get; set; }
        public string? sets { get; set; }
        public string? notes { get; set; }
        public int itemOrder { get; set; }

        internal RoutineItems ToRoutineItem()
        {
            RoutineItems routine = new RoutineItems()
            {
                Id = id,
                Exercises = new Exercises() {Id=ExerciseId, Name = ExerciseName, ImageId = ExerciseImage, GroupId = Muscle_Group },
                Orders = sets,
                Notes = notes,
                ItemOrder = itemOrder,
            };
            return routine;
        }

        internal void FromRoutineItem(RoutineItems entity)
        {

            id = entity.Id;
            ExerciseId = entity.Id;
            ExerciseName = entity.Exercises!.Name;
            ExerciseImage = entity.Exercises.ImageId;
            Muscle_Group = entity.Exercises.GroupId;
            sets = entity.Orders;
            notes = entity.Notes;
            itemOrder = entity.ItemOrder;

        }

    }
}
