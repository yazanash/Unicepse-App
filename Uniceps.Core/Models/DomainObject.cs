using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Models
{
    public class DomainObject
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Guid SyncId { get; set; } = Guid.NewGuid();
        public long ServerVersion { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;
        public bool IsSynced { get; set; } = false;
    }
}
