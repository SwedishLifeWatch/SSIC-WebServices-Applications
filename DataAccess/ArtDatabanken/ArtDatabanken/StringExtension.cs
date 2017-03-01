using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ArtDatabanken
{
    /// <summary>
    /// Contains extension methods to the String class.
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Check that an argument is empty.
        /// </summary>
        /// <param name='argument'>Argument to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentException">Thrown if argument is not empty.</exception>
        public static void CheckEmpty(this String argument, String argumentName)
        {
            if (argument.IsNotEmpty())
            {
                throw new ArgumentException("Argument " + argumentName + " must be empty");
            }
        }

        /// <summary>
        /// Check that an argument is not empty.
        /// </summary>
        /// <param name='argument'>Argument to test.</param>
        /// <param name='argumentName'>Name of argument.</param>
        /// <exception cref="ArgumentException">Thrown if argument is empty.</exception>
        public static void CheckNotEmpty(this String argument, String argumentName)
        {
            if (argument.IsEmpty())
            {
                throw new ArgumentException("Argument " + argumentName + " can not be empty");
            }
        }

        /// <summary>
        /// Test if a string represents a data time value.
        /// </summary>
        /// <param name='text'>String to test.</param>
        /// <returns>True if string represents a data time value.</returns>
        public static Boolean IsDateTime(this String text)
        {
            DateTime dateTime;

            return text.IsNotEmpty() && DateTime.TryParse(text, out dateTime);
        }

        /// <summary>
        /// Test if a string is empty.
        /// </summary>
        /// <param name='text'>String to test.</param>
        /// <returns>True if string contains no visible character.</returns>
        public static Boolean IsEmpty(this String text)
        {
            return (text == null) || (text.Trim().Length == 0);
        }

        /// <summary>
        /// Test if a string is not empty.
        /// </summary>
        /// <param name='text'>String to test.</param>
        /// <returns>True if string contains at least one visible character.</returns>
        public static Boolean IsNotEmpty(this String text)
        {
            return (text != null) && (text.Trim().Length >= 1);
        }

        /// <summary>
        /// Test if a string is a valid email address.
        /// </summary>
        /// <param name='value'>String to test.</param>
        /// <returns>True if string is a valid email address.</returns>
        public static Boolean IsValidEmail(this String value)
        {
            // string reg = @"^((([\w]+\.[\w]+)+)|([\w]+))@(([\w]+\.)+)([A-Za-z]{1,3})$";
            string reg = @"^(([A-Za-z0-9]+_+)|([A-Za-z0-9]+\-+)|([A-Za-z0-9]+\.+)|([A-Za-z0-9]+\++))*[A-Za-z0-9]+@((\w+\-+)|(\w+\.))*\w{1,63}\.[a-zA-Z]{2,6}$";
            return Regex.IsMatch(value, reg);
        }

        /// <summary>
        /// Returns a string where the first letter is capitalized.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns>A string where the first letter is capitalized.</returns>
        public static String FirstLetterToUpper(this string str)
        {
            if (str != null)
            {
                if (str.Length > 1)
                {
                    return char.ToUpper(str[0]) + str.Substring(1);
                }

                return str.ToUpper();
            }

            return null;
        }

        /// <summary>
        /// Removes all duplicated blanks in a string.
        /// </summary>
        /// <param name="value">Current string value.</param>
        /// <returns>A new string value without duplicated blanks.</returns>
        public static String RemoveDuplicateBlanks(this String value)
        {
            Regex r = new Regex(@"\s+");
            return r.Replace(value, @" ");
        }

        /// <summary>
        /// Returns the <paramref name="str"/> or the <paramref name="defaultValue"/> if the <paramref name="str"/> is <c>null</c>.
        /// </summary>
        /// <param name="str">The string.</param>        
        /// <param name="defaultValue">The string to show when the source is <c>null</c>. 
        /// If <c>null</c> an empty string is returned.</param>
        /// <param name="considerWhiteSpaceIsEmpty">if set to <c>true</c> and <paramref name="str"/> is white space, then the <paramref name="defaultValue"/> is returned.</param>
        /// <returns>The <paramref name="str"/> or the <paramref name="defaultValue"/> if the <paramref name="str"/> is <c>null</c>.</returns>
        public static String ToString(this String str, String defaultValue, Boolean considerWhiteSpaceIsEmpty = false)
        {
            return ToString(str, null, defaultValue, considerWhiteSpaceIsEmpty);            
        }

        /// <summary>
        /// Returns the <paramref name="str"/> or the <paramref name="defaultValue"/> if the <paramref name="str"/> is <c>null</c>.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="provider">
        /// The format provider.
        /// If <c>null</c> the default provider is used.
        /// </param>
        /// <param name="defaultValue">The string to show when the source is <c>null</c>. 
        /// If <c>null</c> an empty string is returned.</param>
        /// <param name="considerWhiteSpaceIsEmpty">if set to <c>true</c> and <paramref name="str"/> is white space, then the <paramref name="defaultValue"/> is returned.</param>
        /// <returns>The <paramref name="str"/> or the <paramref name="defaultValue"/> if the <paramref name="str"/> is <c>null</c>.</returns>
        public static String ToString(this String str, IFormatProvider provider, String defaultValue, Boolean considerWhiteSpaceIsEmpty = false)
        {
            return (considerWhiteSpaceIsEmpty ? str.IsEmpty() : String.IsNullOrEmpty(str)) ? defaultValue : str.ToString(provider);
        }

        /// <summary>
        /// Parse a Boolean value that has been
        /// received over the internet.
        /// </summary>
        /// <param name='value'>Boolean value to convert to a string.</param>
        /// <returns>The Boolean value as a string.</returns>
        public static Boolean WebParseBoolean(this String value)
        {
            return Boolean.Parse(value);
        }

        /// <summary>
        /// Parse a DateTime value that has been
        /// received over the internet.
        /// </summary>
        /// <param name='value'>DateTime value to convert to a string.</param>
        /// <returns>The DateTime value as a string.</returns>
        public static DateTime WebParseDateTime(this String value)
        {
            return DateTime.Parse(value);
        }

        /// <summary>
        /// Parse a Double value that has been
        /// received over the internet.
        /// </summary>
        /// <param name='value'>Double value to convert to a string.</param>
        /// <returns>The Double value as a string.</returns>
        public static Double WebParseDouble(this String value)
        {
            NumberStyles styles;
            
            styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign;
            return Double.Parse(value.Replace(",", "."), styles, CultureInfo.CreateSpecificCulture("en-GB"));
        }

        /// <summary>
        /// Parse a Int32 value that has been
        /// received over the internet.
        /// </summary>
        /// <param name='value'>Int32 value to convert to a string.</param>
        /// <returns>The Int32 value as a string.</returns>
        public static Int32 WebParseInt32(this String value)
        {
            return Int32.Parse(value);
        }

        /// <summary>
        /// Parse a Int64 value that has been
        /// received over the internet.
        /// </summary>
        /// <param name='value'>Int64 value to convert to a string.</param>
        /// <returns>The Int64 value as a string.</returns>
        public static Int64 WebParseInt64(this String value)
        {
            return Int64.Parse(value);
        }

        /// <summary>
        /// Wrapper method for all injection checks
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <returns>A version of text that is safe to use.</returns>
        public static String CheckInjection(this String text)
        {
            text = CheckSqlInjection(text);
            text = CheckHTMLInjection(text);
            return text;
        }

        /// <summary>
        /// This is an attempt to handle problems with Json injection.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <returns>A version of text that is safe to use.</returns>
        public static String CheckJsonInjection(this String text)
        {
            if (text.IsNotEmpty())
            {
                text = text.Replace("\\", "\\\\");
                return text.Replace("\"", "\\\"");
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// This is an attempt to handle problems with SQL injection.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <returns>A version of text that is safe to use.</returns>
        public static String CheckSqlInjection(this String text)
        {
            if (text.IsNotEmpty())
            {
                return text.Replace("'", "''");
            }
            else
            {
                return text;
            }
        }

        /// <summary>
        /// This is an attempt to handle problems with HTML and Java script injection.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <returns>A version of text that is safe to use.</returns>
        public static String CheckHTMLInjection(this String text)
        {
            if (text.IsNotEmpty())
            {
                // Regex = String contains "<" + any chars or \n + ">"
                if (Regex.IsMatch(text, "<(.|\n)*?>"))
                {
                    text = text.Replace("<", "");
                    text = text.Replace(">", "");
                }
            }
            return text;
        }

        /// <summary>
        /// Check that the string is not longer than specified length.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <param name='maxLength'>Max length for string.</param>
        /// <exception cref="ArgumentException">Thrown if string is to long.</exception>
        public static void CheckLength(this String text, Int32 maxLength)
        {
            if (text.IsNotEmpty() &&
                text.Length > maxLength)
            {
                throw new ArgumentException("String exceeds max length = " + maxLength);
            }
        }

        /// <summary>
        /// Check that the string matches the regular expression.
        /// </summary>
        /// <param name='text'>Text to test.</param>
        /// <param name='regularExpression'>Regular expression.</param>
        /// <exception cref="ArgumentException">Thrown if string does not matche the regular expression.</exception>
        public static void CheckRegularExpression(this String text,
                                                  String regularExpression)
        {
            if (text.IsNull() &&
                !Regex.IsMatch(text, regularExpression))
            {
                throw new ArgumentException("String does not match regular expression, text = " + text);
            }
        }
    }
}
