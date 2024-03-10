namespace PlatinumGymPro.Enums
{
    public enum Filter
    {
        GenderMale,
        GenderFemale,
        HaveDebt,
        SubscribeEnd,
        Active,
        InActive,
        WithoutTrainer
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
        Shoulders= 2,
        Back = 3,
        Legs = 4, 
        Biceps = 5,
        Triceps = 6,
        Calves = 7,
        Abs = 8,
    }
}