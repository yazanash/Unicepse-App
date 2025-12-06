using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Exceptions
{
    public class FreeLimitException : Exception
    {
        public FreeLimitException()
        {


        }
        public FreeLimitException(string? message) : base(message)
        {


        }
    }
}
