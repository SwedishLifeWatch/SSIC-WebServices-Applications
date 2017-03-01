using System.Collections.Generic;

namespace ArtDatabanken.GIS.Helpers
{
    /// <summary>
    /// Hash code helper
    /// </summary>
    public static class HashCodeHelper
    {

        /// <summary>
        /// GetOrderIndependentHashCodeByXor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        // One downside of that is that the hash for { "x", "x" } is the same as the hash for { "y", "y" }. 
        public static int GetOrderIndependentHashCodeByXor<T>(IEnumerable<T> source)
        {
            int hash = 0;
            foreach (T element in source)
            {
                hash = hash ^ EqualityComparer<T>.Default.GetHashCode(element);
            }
            return hash;
        }

        /// <summary>
        /// GetOrderIndependentHashCodeByAddition. There are still some nasty cases (e.g. {1, -1} and {2, -2}
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        // 
        public static int GetOrderIndependentHashCodeByAddition<T>(IEnumerable<T> source)
        {
            int hash = 0;
            foreach (T element in source)
            {
                hash = unchecked(hash +
                    EqualityComparer<T>.Default.GetHashCode(element));
            }
            return hash;
        }

        /// <summary>
        /// GetOrderIndependentHashCodeByMultiplication
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetOrderIndependentHashCodeByMultiplication<T>(IEnumerable<T> source)
        {
            int hash = 0;
            foreach (T element in source)
            {
                int h = EqualityComparer<T>.Default.GetHashCode(element);
                if (h != 0)
                    hash = unchecked(hash * h);
            }
            return hash;
        }

        /// <summary>
        /// GetOrderIndependentHashCode
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int GetOrderIndependentHashCode<T>(IEnumerable<T> source)
        {
            if (source == null)
                return 0;
            int hash = 0;
            int curHash;
            int bitOffset = 0;
            // Stores number of occurences so far of each value.
            var valueCounts = new Dictionary<T, int>();

            foreach (T element in source)
            {
                curHash = EqualityComparer<T>.Default.GetHashCode(element);
                if (valueCounts.TryGetValue(element, out bitOffset))
                    valueCounts[element] = bitOffset + 1;
                else
                    valueCounts.Add(element, bitOffset);

                // The current hash code is shifted (with wrapping) one bit
                // further left on each successive recurrence of a certain
                // value to widen the distribution.
                // 37 is an arbitrary low prime number that helps the
                // algorithm to smooth out the distribution.
                hash = unchecked(hash + ((curHash << bitOffset) |
                    (curHash >> (32 - bitOffset))) * 37);
            }

            return hash;
        }
        

    }
}
