using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Metric;

namespace Uniceps.Test.Fakes
{
    public class MetricFactory
    {
        public Metric FakeMetric()
        {
            var metric_faker = new Faker<Metric>()
               .StrictMode(false)
               .Rules((fake, metric) =>
               {
                   metric.Wieght = fake.Random.Double(20, 70);
                   metric.Hieght = fake.Random.Double(20, 70);
                   metric.Hips = fake.Random.Double(20, 70);
                   metric.L_Arm = fake.Random.Double(20, 70);
                   metric.R_Arm = fake.Random.Double(20, 70);
                   metric.Nick = fake.Random.Double(20, 70);
                   metric.L_Humerus = fake.Random.Double(20, 70);
                   metric.R_Humerus = fake.Random.Double(20, 70);
                   metric.R_Leg = fake.Random.Double(20, 70);
                   metric.L_Leg = fake.Random.Double(20, 70);
                   metric.L_Thigh = fake.Random.Double(20, 70);
                   metric.R_Thigh = fake.Random.Double(20, 70);
                   metric.Shoulders = fake.Random.Double(20, 70);
                   metric.Waist = fake.Random.Double(20, 70);
                   metric.Chest = fake.Random.Double(20, 70);
                   metric.CheckDate = DateTime.Now;
               });
            return metric_faker;
        }

        public Metric FakeMetricWithId()
        {
            var metric_faker = new Faker<Metric>()
               .StrictMode(false)
               .Rules((fake, metric) =>
               {
                   metric.Id = fake.Random.Int(10000, 99999);
                   metric.Player = new Core.Models.Player.Player() { Id = fake.Random.Int(10000, 99999) };
                   metric.Wieght = fake.Random.Double(20, 70);
                   metric.Hieght = fake.Random.Double(20, 70);
                   metric.Hips = fake.Random.Double(20, 70);
                   metric.L_Arm = fake.Random.Double(20, 70);
                   metric.R_Arm = fake.Random.Double(20, 70);
                   metric.Nick = fake.Random.Double(20, 70);
                   metric.L_Humerus = fake.Random.Double(20, 70);
                   metric.R_Humerus = fake.Random.Double(20, 70);
                   metric.R_Leg = fake.Random.Double(20, 70);
                   metric.L_Leg = fake.Random.Double(20, 70);
                   metric.L_Thigh = fake.Random.Double(20, 70);
                   metric.R_Thigh = fake.Random.Double(20, 70);
                   metric.Shoulders = fake.Random.Double(20, 70);
                   metric.Waist = fake.Random.Double(20, 70);
                   metric.Chest = fake.Random.Double(20, 70);
                   metric.CheckDate = DateTime.Now;
               });
            return metric_faker;
        }
    }
}
