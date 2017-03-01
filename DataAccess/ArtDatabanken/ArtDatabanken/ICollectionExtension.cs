using System;
using System.Collections;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the ICollection interface.
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public static class ICollectionExtension
    {
        /// <summary>
        /// Check that a collection is empty.
        /// </summary>
        /// <param name='collection'>Collection to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentException">Thrown if collection is not empty.</exception>
        public static void CheckEmpty(this ICollection collection, String argumentName)
        {
            if (collection.IsNotEmpty())
            {
                throw new ArgumentException("Argument " + argumentName + " must be empty");
            }
        }

        /// <summary>
        /// Check that a collection is not empty.
        /// </summary>
        /// <param name='collection'>Collection to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentException">Thrown if collection is empty.</exception>
        public static void CheckNotEmpty(this ICollection collection, String argumentName)
        {
            if (collection.IsEmpty())
            {
                throw new ArgumentException("Argument " + argumentName + " can not be empty");
            }
        }

        /// <summary>
        /// Test if a collection is empty.
        /// </summary>
        /// <param name='collection'>Collection to test.</param>
        /// <returns>True if collection is null or has 0 elements.</returns>
        public static Boolean IsEmpty(this ICollection collection)
        {
            return (collection == null) || (collection.Count == 0);
        }

        /// <summary>
        /// Test if a collection is not empty.
        /// </summary>
        /// <param name='collection'>Collection to test.</param>
        /// <returns>True if collection contains at least one element.</returns>
        public static Boolean IsNotEmpty(this ICollection collection)
        {
            return (collection != null) && (collection.Count >= 1);
        }
    }
}
