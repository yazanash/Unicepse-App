using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.SystemAuthViewModels
{
    public class FeatureListItemViewModel:ViewModelBase
    {
        public string Feature { get; set; }

        public FeatureListItemViewModel(string feature)
        {
            Feature = feature;
        }
    }
}
