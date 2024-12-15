﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Stores.AccountantStores
{
    public interface IDailyAccountantStore<T>
    {
        Task GetDaily(DateTime dateTo);
    }
}
