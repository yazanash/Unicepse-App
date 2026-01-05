using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Employee;

namespace Uniceps.DataExporter.Dtos
{
    public class CreditDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; }
        public Guid EmpPersonSyncId { get; set; }
        public int EmpPersonId { get; set; }
        public double CreditValue { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
    }
}
