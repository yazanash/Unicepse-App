using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.Models
{
    public class Employee:Person
    {
      
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
        public ICollection<Sport>? Sports { get; set; }
    }
}
