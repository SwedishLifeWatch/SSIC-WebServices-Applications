using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.Helpers
{
    /// <summary>
    /// Extension methods for collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Checks if two sequences are equal.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static bool SequenceEqualEx<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (ReferenceEquals(null, first) && ReferenceEquals(null, second))
                return true;
            if (ReferenceEquals(null, first))
            {
                if (!ReferenceEquals(null, second))
                {
                    return !second.Any();
                }
            }

            if (ReferenceEquals(null, second))
            {
                if (!ReferenceEquals(null, first))
                {
                    return !first.Any();
                }
            }
            return first.SequenceEqual(second);
        }

    }
}
