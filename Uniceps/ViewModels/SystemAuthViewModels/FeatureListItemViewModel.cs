using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class FeatureListItemViewModel:ViewModelBase
    {
        public FeatureItem FeatureItem;

        public FeatureListItemViewModel(FeatureItem featureItem, int order)
        {
            FeatureItem = featureItem;
            Order = order;
        }
        public int Order { get; set; }
        public string Feature => FeatureItem.Name;
        public bool IsFree => FeatureItem.IsFree;
        public bool IsPremium => FeatureItem.IsPremium;
        public string? FreeIcon => FeatureItem.IsFree ? "CheckBold" : "Close";
        public string? PremiumIcon => FeatureItem.IsPremium? "CheckBold" : "Close";
        public string FreeLimitString => FeatureItem.FreeLimitString;
        public string PrimumLimitString => FeatureItem.PrimumLimitString;
        public Brush FreeIconColor=> FeatureItem.IsFree ? Brushes.LightBlue: Brushes.Red;
        public Brush PremiumIconColor => FeatureItem.IsPremium ? Brushes.LightBlue : Brushes.Red;
    }
}
