using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlatinumGym.Core.Exceptions
{
    public class InvalidDateExeption : Exception
    {
        public InvalidDateExeption( string? message) : base(message)
        {
        }
    }
}
