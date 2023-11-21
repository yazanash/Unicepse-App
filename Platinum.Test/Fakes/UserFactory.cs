using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bogus;
using PlatinumGym.Core.Models.Authentication;
namespace Platinum.Test.Fakes
{
    public class UserFactory
    {
        public User FakeUser()
        {
            var userFaker = new Faker<User>()
              .StrictMode(false)
              .Rules((fake, user) =>
              {
                  user.UserName = fake.Person.UserName;
                  user.Password = fake.Internet.Password();
                  user.IsAdmin = fake.Random.Bool();
              });
            return userFaker;
        }
    }
}
