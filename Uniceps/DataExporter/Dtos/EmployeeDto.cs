using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Dtos
{
    public class EmployeeDto
    {
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; }
        public bool Salary { get; set; }
        public bool Parcent { get; set; }
        public double SalaryValue { get; set; }
        public int ParcentValue { get; set; }
        public bool IsSecrtaria { get; set; }
        public string? Position { get; set; }
        public DateTime StartDate { get; set; }
        public double Balance { get; set; }
        public bool IsActive { get; set; }
        public bool IsTrainer { get; set; }
        public List<Guid>? SportsIds { get; set; } = new();
    }
}
