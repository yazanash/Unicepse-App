using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Services
{
    internal interface IQueryService<T> where T : DomainObject
    {
        Task<IEnumerable<T>> GetPlayerByActiveStatus(bool status);
        Task<IEnumerable<T>> GetPlayerByTrainer(int Trainer_id);

    }
}
