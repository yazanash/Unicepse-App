﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicepse.Core.Models.TrainingProgram;

namespace Unicepse.Core.Services
{
    public interface IGetExercisesService
    {
        Task<IEnumerable<Exercises>> GetAll();
    }
}
