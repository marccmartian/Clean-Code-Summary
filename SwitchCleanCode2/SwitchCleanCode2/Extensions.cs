using System;
using System.Collections.Generic;

namespace SwitchCleanCode2
{
    public static class Extensions
    {
        public static void Foreach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }
    }
}