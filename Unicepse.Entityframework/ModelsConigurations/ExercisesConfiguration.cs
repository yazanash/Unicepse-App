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
    public class ExercisesConfiguration : IEntityTypeConfiguration<Exercises>
    {
        public void Configure(EntityTypeBuilder<Exercises> builder)
        {
            builder.Property(p => p.Name).HasColumnType("nvarchar(4000)");
            builder.Property(p => p.Muscel).HasColumnType("nvarchar(4000)");
            //builder.Property(p => p.Group).HasColumnType("nvarchar(8000)");
            builder.Property(p => p.ImageId).HasColumnType("nvarchar(4000)");
        }
    }
}
