using System;
using System.Text.RegularExpressions;

namespace ArtDatabanken.WebService.ArtDatabankenService
{
    /// <summary>
    /// Contains extension methods to the String class.
    /// </summary>
    public static class StringExtension
    {
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
        public static String CheckHTMLInjection (this String text) 
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
