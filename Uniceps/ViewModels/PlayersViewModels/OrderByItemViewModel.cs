using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.utlis.common;

namespace Uniceps.ViewModels.PlayersViewModels
{
    public class OrderByItemViewModel : ViewModelBase
    {
        public Order OrderBy;
        public int Id { get; }
        public string? Content { get; }

        public OrderByItemViewModel(Order orderBy, int id, string name)
        {
            OrderBy = orderBy;
            Id = id;
            Content = name;
        }
    }
}
