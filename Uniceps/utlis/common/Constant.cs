using System.ComponentModel.DataAnnotations;

namespace Uniceps.utlis.common
{
    public enum Filter
    {
        GenderMale,
        GenderFemale,
        HaveDebt,
        SubscribeEnd,
        Active,
        InActive,
        WithoutTrainer,


        Employee,
        Trainer,
        Secretary,
        All
    }
    public enum Order
    {
        ByName,
        ByDebt,
        BySubscribeEnd
    }

    public enum EMuscleGroup
    {
        Chest = 1,
        Shoulders = 2,
        Back = 3,
        Legs = 4,
        Biceps = 5,
        Triceps = 6,
        Calves = 7,
        Abs = 8,
    }
    public enum SubscriptionStatus
    {
        [Display(Name = "الكل")]
        None,
        [Display(Name = "منتهي")]
        Expired,
        [Display(Name = "سينتهي")]
        EndingSoon,
        [Display(Name = "تم تجديده")]
        Renewed,
        [Display(Name = "فعال")]
        Active
    }
    public enum FileTypes
    {
        [Display(Name = "Routine")]
        Routine,
    }
    public enum FileFormatType
    {
        [Display(Name = "تصدير بصيغة PDF")]
        PDF,
        [Display(Name = "تصدير بصيغة Unx")]
        UniFile,
    }
}