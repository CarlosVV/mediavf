using System;
using System.Collections.Generic;

namespace AutoTrade.Core
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Loops through a collection and performs an action on each item
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable == null)
                throw new ArgumentNullException("enumerable");
            if (action == null)
                throw new ArgumentNullException("action");

            // loop through and perform action on each item
            foreach (T item in enumerable)
                action(item);
        }
    }
}