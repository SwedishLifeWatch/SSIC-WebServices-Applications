using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.GIS.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Remove all non ASCII characters from a string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string RemoveNonAscii(this string text)
        {
            return Encoding.ASCII.GetString(
                Encoding.Convert(
                    Encoding.UTF8,
                    Encoding.GetEncoding(
                        Encoding.ASCII.EncodingName,
                        new EncoderReplacementFallback(string.Empty),
                        new DecoderExceptionFallback()
                        ),
                    Encoding.UTF8.GetBytes(text)
                )
            );
        }

        /// <summary>
        /// Replace swedish characters
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ReplaceSwedishChars(this string text)
        {
            if (text == null)
            {
                return null;
            }

            text = text.Replace('å', 'a')
                    .Replace('Å', 'A')
                    .Replace('ä', 'a')
                    .Replace('Ä', 'A')
                    .Replace('ö', 'o')
                    .Replace('Ö', 'O');

            return text;
        }
    }
}
