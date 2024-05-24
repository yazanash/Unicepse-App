using Bogus;
using Unicepse.Core.Models.Sport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Test.Fakes
{
    public class SportFactory
    {
        public Sport FakeSport()
        {
            var sportFaker = new Faker<Sport>()
              .StrictMode(false)
              .Rules((fake, sport) =>
              {
                  sport.Name = fake.Company.CompanyName();
                  sport.DaysCount = 30;
                  sport.DaysInWeek = 6;
                  sport.DailyPrice = 2000;
                  sport.IsActive = true;
                  sport.Price = 30000;
              });
            return sportFaker;
        }
    }
}
