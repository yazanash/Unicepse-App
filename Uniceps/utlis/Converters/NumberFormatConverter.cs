using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Uniceps.utlis.Converters
{
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                return number.ToString("N0", CultureInfo.InvariantCulture);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out double number))
            {
                return number;
            }
            return value;
        }
    }
}
