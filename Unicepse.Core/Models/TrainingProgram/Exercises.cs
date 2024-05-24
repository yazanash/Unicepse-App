using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Models.TrainingProgram
{
    public class Exercises
    {
        public int Id { get; set; }
        public int Tid { get; set; }
        public string? Name { get;set;}
        public int GroupId { get;set;}
        public string? Muscel { get; set; }
        public string? ImageId { get; set; }

    }
}
