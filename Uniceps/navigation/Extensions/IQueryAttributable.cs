using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.navigation.Extensions
{
    public interface IQueryAttributable
    {
        void ApplyQueryAttributes(IDictionary<string, object> query);
    }
}
