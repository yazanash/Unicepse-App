using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Core.Common;
using Uniceps.Core.Models.Player;

namespace Uniceps.Server.Requests
{
    public class CreatePlayerRequest
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public Guid SyncId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int BirthDate { get; set; }
        public bool GenderMale { get; set; }
        public string FingerprintData { get; set; } = string.Empty;
        public string MediclStatus { get; set; } = string.Empty;
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
            };
        }
    }
}
