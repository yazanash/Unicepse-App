using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Unicepse.Core.Models.TrainingProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Entityframework.ModelsConigurations
{
    public class RoutineItemsConfiguration : IEntityTypeConfiguration<RoutineItems>
    {
        public void Configure(EntityTypeBuilder<RoutineItems> builder)
        {
            builder.Property(p => p.Notes).HasColumnType("nvarchar(4000)");
            builder.Property(p => p.Orders).HasColumnType("nvarchar(4000)");
        }
    }
}
