using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Uniceps.utlis.Converters
{
    public class EnumDisplayNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Enum enumValue)
            {
                var field = enumValue.GetType().GetField(enumValue.ToString());
                var attr = field?.GetCustomAttribute<DisplayAttribute>();
                return attr?.Name ?? enumValue.ToString();
            }
            return value?.ToString() ?? "-----";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
