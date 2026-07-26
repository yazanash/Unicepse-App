using System.ComponentModel.DataAnnotations;

namespace Uniceps.utlis.common
{
    public enum PlayerFilter
    {
        [Display(Name = "ذكور")]
        GenderMale,
        [Display(Name = "اناث")]
        GenderFemale,
        [Display(Name = "ديون")]
        HaveDebt,
        [Display(Name = "منتهي الاشتراك")]
        SubscribeEnd,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غير فعال")]
        InActive,
        [Display(Name = "بدون مدرب")]
        WithoutTrainer,
        [Display(Name = "الكل")]
        All
    }
    public enum Filter
    {
        [Display(Name = "موظف")]
        Employee,
        [Display(Name = "مدرب")]
        Trainer,
        [Display(Name = "سكرتارية")]
        Secretary,
        [Display(Name = "الكل")]
        All
    }
    public enum Order
    {
        [Display(Name = "الاسم")]
        ByName,
        [Display(Name = "الديون")]
        ByDebt,
        [Display(Name = "نهاية الاشتراك")]
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
        Active,
        [Display(Name = "الديون")]
        HasDebt,
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