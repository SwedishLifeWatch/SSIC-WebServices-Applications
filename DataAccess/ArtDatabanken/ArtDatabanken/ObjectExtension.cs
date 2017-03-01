using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Object class.
    /// </summary>
    public static class ObjectExtension
    {
        /// <summary>
        /// Check that an argument is not null.
        /// </summary>
        /// <param name='argument'>Argument to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentNullException">Thrown if argument is null.</exception>
        public static void CheckNotNull(this Object argument, String argumentName)
        {
            if (argument.IsNull())
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Check that an argument is null.
        /// </summary>
        /// <param name='argument'>Argument to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentException">Thrown if argument is not null.</exception>
        public static void CheckNull(this Object argument, String argumentName)
        {
            if (argument.IsNotNull())
            {
                throw new ArgumentException("Argument " + argumentName + " must be null");
            }
        }

        /// <summary>
        /// Test if an object reference is not null.
        /// </summary>
        /// <param name='testObject'>Reference to test.</param>
        /// <returns>False if object reference is null, otherwise true.</returns>
        public static Boolean IsNotNull(this Object testObject)
        {
            return testObject != null;
        }

        /// <summary>
        /// Test if an object reference is null.
        /// </summary>
        /// <param name='testObject'>Reference to test.</param>
        /// <returns>True if object reference is null, otherwise false.</returns>
        public static Boolean IsNull(this Object testObject)
        {
            return testObject == null;
        }
    }
}
