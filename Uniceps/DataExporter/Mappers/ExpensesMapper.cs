using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Models.Expenses;
using Uniceps.DataExporter.Dtos;

namespace Uniceps.DataExporter.Mappers
{
    public static class ExpensesMapper 
    {
        public static Expenses FromDto(ExpenseDto data)
        {
            Expenses expenses = new Expenses()
            {
                CreatedAt = data.CreatedAt,
                date = data.date,
                Description = data.Description,
                SyncId = data.SyncId,
                UpdatedAt = data.UpdatedAt,
                Value = data.Value,
                 
            };
            return expenses;
        }

        public static ExpenseDto ToDto(Expenses data)
        {
            ExpenseDto expenses = new ExpenseDto()
            {
                CreatedAt = data.CreatedAt,
                date = data.date,
                Description = data.Description,
                SyncId = data.SyncId,
                UpdatedAt = data.UpdatedAt,
                Value = data.Value,
                
            };
            return expenses;
        }
    }
}
