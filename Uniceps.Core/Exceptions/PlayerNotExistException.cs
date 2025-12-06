using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Core.Exceptions
{
    public class PlayerNotExistException : Exception
    {

        public PlayerNotExistException() : base()
        {


        }
        public PlayerNotExistException(string? message) : base(message)
        {


        }
    }
}
