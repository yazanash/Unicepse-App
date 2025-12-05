using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Services
{
    public interface IExcelService<T> where T : class
    {
        public List<T> ImportFromExcel(string filePath);
        public void ExportToExcel(string filePath, IEnumerable<T> data);
    }
}
