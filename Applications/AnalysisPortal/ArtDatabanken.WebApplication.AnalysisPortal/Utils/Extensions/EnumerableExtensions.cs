using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether two enumerable lists is equivalent.
        /// The order of the elements doesn't matter so {1,2,4} == {2,4,1}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="first">The first.</param>
        /// <param name="second">The second.</param>
        /// <returns>
        ///   <c>true</c> if [is equivalent to] [the specified first]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEquivalentTo<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            if ((first == null) != (second == null))
            {
                return false;
            }

            if (!object.ReferenceEquals(first, second))
            {
                if (first.Count() != second.Count())
                {
                    return false;
                }

                if ((first.Count() != 0) && HaveMismatchedElement<T>(first, second))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool HaveMismatchedElement<T>(IEnumerable<T> first, IEnumerable<T> second)
        {
            int firstCount;
            int secondCount;

            var firstElementCounts = GetElementCounts<T>(first, out firstCount);
            var secondElementCounts = GetElementCounts<T>(second, out secondCount);

            if (firstCount != secondCount)
            {
                return true;
            }

            foreach (var kvp in firstElementCounts)
            {
                firstCount = kvp.Value;
                secondElementCounts.TryGetValue(kvp.Key, out secondCount);

                if (firstCount != secondCount)
                {
                    return true;
                }
            }

            return false;
        }

        private static Dictionary<T, int> GetElementCounts<T>(IEnumerable<T> enumerable, out int nullCount)
        {
            var dictionary = new Dictionary<T, int>();
            nullCount = 0;

            foreach (T element in enumerable)
            {
                if (element == null)
                {
                    nullCount++;
                }
                else
                {
                    int num;
                    dictionary.TryGetValue(element, out num);
                    num++;
                    dictionary[element] = num;
                }
            }

            return dictionary;
        }

        private static int GetHashCode<T>(IEnumerable<T> enumerable)
        {
            int hash = 17;

            foreach (T val in enumerable.OrderBy(x => x))
            {
                hash = (hash * 23) + val.GetHashCode();
            }

            return hash;
        }
    } 
}
