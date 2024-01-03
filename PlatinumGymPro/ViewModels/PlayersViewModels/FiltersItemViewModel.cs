
using PlatinumGymPro.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{

    public class FiltersItemViewModel : ViewModelBase
    {
        public Enums.Filter Filter;
        public int Id { get;  }
        public string? Content { get;  }

        public FiltersItemViewModel(Filter filter,int id,string name)
        {
            Filter = filter;
            Id=id;
            Content = name;
        }
    }
}
