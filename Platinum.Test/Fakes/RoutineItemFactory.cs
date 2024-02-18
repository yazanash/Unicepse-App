using Bogus;
using PlatinumGym.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platinum.Test.Fakes
{
    public class RoutineItemFactory
    {
        public RoutineItems FakeRoutineItem()
        {
            var playerRoutineFaker = new Faker<RoutineItems>()
              .StrictMode(false)
              .Rules((fake, routineItem) =>
              {
                  routineItem.Exercise = FakeExercises();
                  routineItem.Orders = "13*5";
                  routineItem.Notes = "12*10*8";
              });
            return playerRoutineFaker;
        }
        public Exercises FakeExercises()
        {
            var exercisesFaker = new Faker<Exercises>()
              .StrictMode(false)
              .Rules((fake, exercises) =>
              {
                  exercises.Muscel = "chest";
                  exercises.Name = fake.Name.FirstName();
                  exercises.Group = fake.Name.FirstName();
                  exercises.ImageId = "pack://application:,,,/Resources/Assets/Exercises/9.png";
              });
            return exercisesFaker;
        }
    }
}
