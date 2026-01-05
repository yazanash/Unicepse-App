using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Dtos
{
    public class SportDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid SyncId { get; set; } = Guid.NewGuid();
        public string? Name { get; set; }
        public double Price { get; set; }
        public bool IsActive { get; set; }
        public int DaysInWeek { get; set; }
        public List<Guid> Trainers { get; set; } = new();
        public int DaysCount { get; set; }
    }
}
