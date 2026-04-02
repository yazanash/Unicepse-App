using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.SystemServices
{
    public static class NotificationManager
    {
        public static void SendBackupNeededNotification()
        {
            new ToastContentBuilder()
          .AddHeader("6221", "تنبيه الأمان والبيانات", "")
          .AddText(" حماية البيانات")
          .AddText("لم يتم إجراء نسخة احتياطية اليوم. يرجى تأمين بيانات المشتركين الآن.")
          .AddAttributionText("نظام Uniceps الذكي")
          .AddButton(new ToastButton()
              .SetContent("إجراء نسخة الآن")
              .AddArgument("action", "runBackup")
              .SetBackgroundActivation())
          .Show();
        }
    }
}
