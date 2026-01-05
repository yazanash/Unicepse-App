using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.DataExporter.Mappers
{
    public interface IMapperExtension<T,TDto>
    {
        TDto ToDto(T data);
        T FromDto(TDto data);
    }
}
