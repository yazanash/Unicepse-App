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
        public RoutineItems FakeRoutineItem(Exercises excer)
        {
            var playerRoutineFaker = new Faker<RoutineItems>()
              .StrictMode(false)
              .Rules((fake, routineItem) =>
              {
                  routineItem.Exercises = excer;
                  routineItem.Orders = "13 5";
                  routineItem.Notes = "12 10 8";
              });
            return playerRoutineFaker;
        }
    }
}
