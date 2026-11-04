using System.ComponentModel.DataAnnotations;

namespace Uniceps.Core.Common
{
    public enum DataStatus
    {
        ToCreate = 0,
        ToUpdate = 1,
        ToDelete = 2,
        Synced = 3,
    }
    public enum DataType
    {
        Player = 0,
        Subscription = 1,
        Payment = 2,
        Metric = 3,
        Routine = 4,
        Attendance = 5,
    }
    public enum Roles
    {
        [Display(Name = "مدير النظام")]
        Admin = 0,
        [Display(Name = "مسؤول")]
        Supervisor = 1,
        [Display(Name = "مستخدم")]
        User = 2,
        [Display(Name = "محاسب")]
        Accountant = 3
    }
    public enum GenderType
    {
        Male = 1,
        Female = 2
    }
    public enum UserContextState
    {
        UnAuthenticated,
        NoProfile,
        NoSubscription,
        NoLocalUser,
        Ready
    }
}