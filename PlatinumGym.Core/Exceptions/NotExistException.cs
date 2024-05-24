using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicepse.Core.Exceptions
{
    public class NotExistException : Exception
    {
        public NotExistException() 
        {
            

        }
        public NotExistException(string? message) : base(message)
        {
           
          
        }
    }
}
