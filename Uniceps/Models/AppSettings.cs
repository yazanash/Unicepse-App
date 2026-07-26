using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Models
{
    public class AppSettings
    {
        public string GymName { get; set; } = "نادي جديد";
        public string LogoPath { get; set; } = "";
        public string ContactNumber { get; set; } = "";
        public string OwnerName { get; set; } = "";
        public string BackupPath { get; set; } = "";
        public string ExportFolderPath { get; set; } = "";
        public DateTime? LastBackupDate { get; set; }
        public string AppTheme { get; set; } = "Light";
        public int SubscriptionRemainderDays { get; set; } = 2;
        public int SubscriptionRemainderExpirationDays { get;  set; } = 2;

        public int BackupRemainderDays = 2;
    }
}
