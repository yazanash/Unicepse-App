using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatinumGym.Core.Exceptions;
using PlatinumGym.Core.Models.Employee;
using PlatinumGym.Core.Models.Payment;
using PlatinumGym.Core.Services;
using PlatinumGym.Entityframework.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.Services
{
    public class DausesDataService
    {

        public double GetParcent(PlayerPayment entity, Employee trainer, DateTime date)
        {
            if (entity.PayDate.Month == date.Month - 1)
            {
                DateTime firstDayInMonth = new DateTime(date.Year,date.Month,1);
                if (entity.To <= date)
                {
                    int days = (int)entity.To.Subtract(firstDayInMonth).TotalDays+1;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return (total * trainer.ParcentValue) / 100;
                }
                else if (entity.To > date)
                {
                    int days = (int)date.Subtract(firstDayInMonth).TotalDays ;
                    double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return (total * trainer.ParcentValue) / 100;
                }
            }
            else if (entity.From <= date)
            {
                if (entity.To <= date)
                    return ((entity.PaymentValue * trainer.ParcentValue) / 100);
                else if (entity.To > date)
                {
                    int days = (int) date.Subtract(entity.From).TotalDays;
                       double dayprice = entity.PaymentValue / entity.CoverDays;
                    double total = (days * dayprice);
                    return (total * trainer.ParcentValue) / 100;
                }
            }


            return 0;
        }
    }
}
