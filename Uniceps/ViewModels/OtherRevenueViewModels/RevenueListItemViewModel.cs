using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uniceps.ViewModels.OtherRevenueViewModels
{
    public class RevenueListItemViewModel:ViewModelBase
    {
        public OtherRevenue Revenue;

        public RevenueListItemViewModel(OtherRevenue revenue)
        {
            Revenue = revenue;
        }
        private int _order;
        public int Order
        {
            get { return _order; }
            set { _order = value; OnPropertyChanged(nameof(Order)); }
        }
        public decimal Amount => Revenue.Amount;
        public string Service => Revenue.Service;
        public string Description  => Revenue.Description;
        public string CustomerName  => Revenue.CustomerName;
        public DateTime Date  => Revenue.Date;

        internal void Update(OtherRevenue obj)
        {
            Revenue = obj;
            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(Service));
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(CustomerName));
            OnPropertyChanged(nameof(Date));
        }
    }
}
