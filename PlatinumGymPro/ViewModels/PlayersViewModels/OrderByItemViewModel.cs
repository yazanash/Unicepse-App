using PlatinumGym.Core.Models;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class OrderByItemViewModel : ViewModelBase
    {
        public OrderBy OrderBy;
        public int Id => OrderBy.Id;
        public string? Content => OrderBy.Content;

        public OrderByItemViewModel(OrderBy orderBy)
        {
            OrderBy = orderBy;
        }
    }
}
