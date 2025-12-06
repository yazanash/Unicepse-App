using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.Test.Fakes
{
    public class PlayerFactory
    {

        public PlayerFactory()
        {

        }
        public Player FakePlayer()
        {
            var faker = new Faker<Player>()
               .StrictMode(false)
               .Rules((fake, player) =>
               {
                   player.FullName = fake.Person.FullName;
                   player.Phone = fake.Person.Phone;
                   player.BirthDate = fake.Random.Number(1970, 2023);
                   player.GenderMale = fake.Random.Bool();
                   player.SubscribeDate = fake.Date.Past(60);
                   player.SubscribeEndDate = player.SubscribeDate.AddMonths(1);
                   player.IsSubscribed = fake.Random.Bool();
                   player.IsTakenContainer = fake.Random.Bool();
                   player.Balance = Convert.ToDouble(fake.Commerce.Price(10, 100));
                   player.Hieght = fake.Random.Number(100, 210);
                   player.Weight = fake.Random.Number(100, 210);

               });
            return faker;
        }
        public Player FakePlayerWithId()
        {
            var faker = new Faker<Player>()
               .StrictMode(false)
               .Rules((fake, player) =>
               {
                   player.Id = fake.Random.Int(62530, 956556);
                   player.FullName = fake.Person.FullName;
                   player.Phone = fake.Person.Phone;
                   player.BirthDate = fake.Random.Number(1970, 2023);
                   player.GenderMale = fake.Random.Bool();
                   player.SubscribeDate = fake.Date.Past(60);
                   player.SubscribeEndDate = player.SubscribeDate.AddMonths(1);
                   player.IsSubscribed = fake.Random.Bool();
                   player.IsTakenContainer = fake.Random.Bool();
                   player.Balance = Convert.ToDouble(fake.Commerce.Price(10, 100));
                   player.Hieght = fake.Random.Number(100, 210);
                   player.Weight = fake.Random.Number(100, 210);

               });
            return faker;
        }
    }
}
