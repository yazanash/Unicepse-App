using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.SystemAuthModels;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class SystemPlanModelListItemViewModel:ViewModelBase
    {
        public SystemPlanModel SystemPlanModel;
        public SystemPlanItem SystemPlanItem;
        public SystemPlanModelListItemViewModel(SystemPlanModel systemPlanModel,SystemPlanItem systemPlanItem)
        {
            SystemPlanModel = systemPlanModel;
            SystemPlanItem = systemPlanItem;
        }
        public Guid Id => SystemPlanModel.Id;
        public string? Name => SystemPlanModel.Name;
        public decimal Price => SystemPlanItem.Price;
        public int Days => SystemPlanItem.DaysCount;
        public string? DurationString=> SystemPlanItem.DurationString;
        public bool IsFree => SystemPlanItem.IsFree;
    }
}
