﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Models.TrainingProgram
{
    public class RoutineItems
    {

        public int Id { get; set; }
        public virtual Exercises? Exercise { get; set; }
        public string? Notes { get; set; }
        public string? Orders   { get; set; }   
    }
}