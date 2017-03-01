using System;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the Boolean class.
    /// </summary>
    public static class BooleanExtension
    {
        /// <summary>
        /// Get a fixed string representation of a Boolean value
        /// to use over the internet.
        /// </summary>
        /// <param name='value'>Boolean value to convert to a string.</param>
        /// <returns>The Boolean value as a string.</returns>
        public static String WebToString(this Boolean value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns a string description for a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="trueValue">The textual value to be returned if the current boolean is true.</param>
        /// <param name="falseValue">The textual value to be returned if the current boolean is false.</param>
        /// <returns>A string representation of the current boolean value.</returns>
        public static String ToString(this Boolean value, String trueValue, String falseValue)
        {
            return ToString(value, null, trueValue, falseValue);
        }

        /// <summary>
        /// Returns a string description for a boolean value.        
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="provider">
        /// The format provider.
        /// If <c>null</c> the default provider is used.
        /// </param>
        /// <param name="trueValue">The textual value to be returned if the current boolean is true.</param>
        /// <param name="falseValue">The textual value to be returned if the current boolean is false.</param>
        /// <returns>A string representation of the current boolean value.</returns>
        public static String ToString(this Boolean value, IFormatProvider provider, String trueValue, String falseValue)
        {
            return value ? trueValue.ToString(provider) : falseValue.ToString(provider);
        }

        /// <summary>
        /// Returns a string description for a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="trueValue">The textual value to be returned if the current boolean is true.</param>
        /// <param name="falseValue">The textual value to be returned if the current boolean is false.</param>
        /// <param name="defaultValue">The string to show when the value is <c>null</c>.</param>
        /// <returns>A string representation of the current boolean value.</returns>
        public static string ToString(this Boolean? value, String trueValue, String falseValue, String defaultValue)
        {
            return ToString(value, null, trueValue, falseValue, defaultValue);
        }

        /// <summary>
        /// Returns a string description for a boolean value.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        /// <param name="provider">
        ///     The format provider.
        ///     If <c>null</c> the default provider is used.
        /// </param>
        /// <param name="trueValue">The textual value to be returned if the current boolean is true.</param>
        /// <param name="falseValue">The textual value to be returned if the current boolean is false.</param>
        /// <param name="defaultValue">The string to show when the value is <c>null</c>.</param>
        /// <returns>A string representation of the current boolean value.</returns>
        public static string ToString(this Boolean? value, IFormatProvider provider, String trueValue, String falseValue, String defaultValue)
        {
            return value.HasValue
                       ? value.Value ? trueValue.ToString(provider) : falseValue.ToString(provider)
                       : defaultValue;
        }
    }
}
