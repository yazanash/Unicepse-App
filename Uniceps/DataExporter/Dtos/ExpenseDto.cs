using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Dtos
{
    public class ExpenseDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; } = Guid.NewGuid();
        public string? Description { get; set; }
        public double Value { get; set; }
        public DateTime date { get; set; }
    }
}
