﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Exceptions
{
    public class InvalidDateExeption : Exception
    {
        public InvalidDateExeption( string? message) : base(message)
        {
        }
    }
}
