using System.Collections.Generic;
using System.Linq;

namespace GameDemo.Scripts.Extensions
{
    public static class EnumerableExtension
    {
        public static IEnumerable<T> ForEachWithSelect<T>(this IEnumerable<T> source, System.Action<T> action)
        {
            return source.Select(data =>
            {
                action(data);
                return data;
            });
        }
    }
}