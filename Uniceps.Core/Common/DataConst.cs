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
        Admin = 0,
        Supervisor = 1,
        User = 2,
        Accountant = 3
    }
}