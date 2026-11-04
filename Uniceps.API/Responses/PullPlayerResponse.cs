using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Player;

namespace Uniceps.API.Responses
{
    public class PullPlayerResponse
    {
        public Player ToModel()
        {
            return new Player
            {
                CreatedAt = CreatedAt,
                UpdatedAt = UpdatedAt,
                SyncId = SyncId,
                FullName = FullName,
                Phone = Phone,
                BirthDate = BirthDate,
                GenderMale = GenderMale,
                FingerprintData = FingerprintData,
                MediclStatus = MediclStatus,
                ServerVersion = ServerVersion
            };
        }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Guid SyncId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
        public string FingerprintData { get; set; } = string.Empty;
        public string MediclStatus { get; set; } = string.Empty;
        public long ServerVersion { get; set; }
    }
}
