using PlatinumGym.Core.Models;
//using PlatinumGymPro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGymPro.ViewModels.PlayersViewModels
{
    public class FiltersItemViewModel : ViewModelBase
    {
        public Filter Filter;
        public int Id =>Filter.Id;
        public string? Content => Filter.Content;

        public FiltersItemViewModel(Filter filter)
        {
            Filter = filter;
        }
    }
}
