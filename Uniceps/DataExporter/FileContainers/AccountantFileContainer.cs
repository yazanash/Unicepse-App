using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.FileContainers
{
    public class AccountantFileContainer
    {
        public List<CreditDto> CreditDtos { get; set; } = new();
        public List<ExpenseDto> ExpenseDtos{ get; set; } = new();
    }
}
