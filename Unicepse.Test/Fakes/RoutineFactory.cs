using Bogus;
using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Test.Fakes
{
    public class RoutineFactory
    {
        private RoutineItemFactory _routineItemFactory;

        public RoutineFactory(RoutineItemFactory routineItemFactory)
        {
            _routineItemFactory = routineItemFactory;
        }

        public PlayerRoutine FakeRoutine()
        {
           
            var playerRoutineFaker = new Faker<PlayerRoutine>()
              .StrictMode(false)
              .Rules((fake, routine) =>
              {
                  routine.RoutineNo =fake.Company.CompanyName();
                  routine.RoutineData = DateTime.Now;
                  for (int i = 0; i < 15; i++)
                  {
                      //routine.RoutineSchedule.Add(_routineItemFactory.FakeRoutineItem());
                  }
              });
            
            return playerRoutineFaker;
        }
    }
}
