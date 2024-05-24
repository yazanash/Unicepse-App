using Bogus;
using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Test.Fakes
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
