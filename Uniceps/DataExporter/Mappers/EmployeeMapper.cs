using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class EmployeeMapper 
    {
        public static Employee FromDto(EmployeeDto data)
        {
            Employee employee = new Employee()
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Salary = data.Salary,
                Parcent = data.Parcent,
                SalaryValue = data.SalaryValue,
                ParcentValue = data.ParcentValue,
                IsSecrtaria = data.IsSecrtaria,
                Position = data.Position,
                StartDate = data.StartDate,
                Balance = data.Balance,
                IsActive = data.IsActive,
                IsTrainer = data.IsTrainer,
                FullName = data.FullName,
                GenderMale = data.GenderMale,
                BirthDate = data.BirthDate,
                Phone = data.Phone,
            };
            return employee;
        }

        public static EmployeeDto ToDto(Employee data)
        {
            EmployeeDto employeeDto = new EmployeeDto()
            {
                CreatedAt = data.CreatedAt,
                UpdatedAt = data.UpdatedAt,
                SyncId = data.SyncId,
                Salary = data.Salary,
                Parcent = data.Parcent,
                SalaryValue = data.SalaryValue,
                ParcentValue = data.ParcentValue,
                IsSecrtaria = data.IsSecrtaria,
                Position = data.Position,
                StartDate = data.StartDate,
                Balance = data.Balance,
                IsActive = data.IsActive,
                IsTrainer = data.IsTrainer,
                FullName = data.FullName,
                GenderMale = data.GenderMale,
                BirthDate = data.BirthDate,
                Phone = data.Phone,
            };
            employeeDto.SportsIds = data.Sports?.Select(x => x.SyncId).ToList();
            return employeeDto;
        }
    }
}
