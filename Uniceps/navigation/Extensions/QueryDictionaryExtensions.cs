using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.navigation.Extensions
{
    public static class QueryDictionaryExtensions
    {
        public static T? GetValueOrDefault<T>(this IDictionary<string, object> query, string key)
        {
            if (query.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return default;
        }

        public static bool TryGetValue<T>(this IDictionary<string, object> query, string key, out T? result)
        {
            result = default;
            if (query.TryGetValue(key, out var value) && value is T typedValue)
            {
                result = typedValue;
                return true;
            }
            return false;
        }
    }
}
