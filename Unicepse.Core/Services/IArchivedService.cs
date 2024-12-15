﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Services
{
    public interface IArchivedService<T>
    {
        Task<IEnumerable<T>> GetAll();
    }
}
