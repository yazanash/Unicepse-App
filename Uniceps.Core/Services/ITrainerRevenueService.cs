using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.Core.Services
{
    public interface ITrainerRevenueService
    {
        Task<TrainerDueses> GetTrainerDuesAsync(Employee trainer,DateTime reportDate);

        Task<double> GetTrainersAndEmployeesCredits(int year, int month);
        Task<bool> CloseTrainerAccountAsync(TrainerDueses finalDues);
    }
}
