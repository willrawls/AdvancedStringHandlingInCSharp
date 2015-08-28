using System.Collections.Generic;
using System.Linq;

namespace AdvancedStringHandlingInCSharp
{
    public static class Lists
    {
        public static string ListAsString<T>(this IList<T> target, string delimiter = "\n", string suffix = null) where T : class
        {
            if (target.IsEmpty())
                return string.Empty;
            var x = target.Select(v => v.AsString());
            if (suffix.IsNotEmpty())
                return string.Join(delimiter, x) + suffix;
            return string.Join(delimiter, x);
        }

        public static string ListAsString<T>(this IEnumerable<T> target, string delimiter = "\n") where T : class
        {
            if (target == null)
                return string.Empty;
            var x = target.Select(v => v.AsString());
            return string.Join(delimiter, x);
        }

        public static List<T> ToLinearList<T>(this List<List<T>> target) where T : class
        {
            var result = new List<T>();
            foreach (var outerList in target)
            {
                result.AddRange(outerList);
            }
            return result;
        }
    }
}