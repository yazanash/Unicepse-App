using PlatinumGym.Core.Models;
using PlatinumGymPro.Enums;
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
        public Order OrderBy;
        public int Id { get; }
        public string? Content { get; }

        public OrderByItemViewModel(Order orderBy,int id,string name)
        {
            OrderBy = orderBy;
            Id = id;
            Content = name;
        }
    }
}
