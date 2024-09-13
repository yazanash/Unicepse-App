﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException() : base()
        {


        }
        public ConflictException(string? message) : base(message)
        {


        }
    }
}
