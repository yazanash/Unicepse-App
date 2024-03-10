using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlatinumGym.Core.Models.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Entityframework.ModelsConigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.Property(p => p.FullName).HasColumnType("nvarchar(4000)");
            builder.Property(p => p.Phone).HasColumnType("nvarchar(4000)");
            builder.Property(p => p.Position).HasColumnType("nvarchar(4000)");
        }
    }
}
