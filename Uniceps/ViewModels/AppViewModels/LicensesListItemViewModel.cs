using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Uniceps.Core.Models;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.AppViewModels
{
    public class LicensesListItemViewModel : ViewModelBase
    {
        public License license;

        public LicensesListItemViewModel(License license)
        {
            this.license = license;
        }

        public string? GymId => license.GymId;
        public string? Plan => license.Plan;
        public string? SubscribeDate => license.SubscribeDate.ToShortDateString();
        public string? SubscribeEndDate => license.SubscribeEndDate.ToShortDateString();
        public bool IsActive => license.SubscribeEndDate >= DateTime.Now;
        public Brush IsActiveColor => IsActive ? Brushes.Green : Brushes.Red;
        public string? IsActiveText=> IsActive ? "فعال": "غير فعال";
        public string? Price => license.Price;

    }
}
