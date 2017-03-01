using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to a list of String instances.
    /// </summary>
    public static class ListStringExtension
    {
        /// <summary>
        /// Get a list of strings as a string.
        /// </summary>
        /// <param name="stringList">A list of strings.</param>
        /// <returns>A list of strings as a string.</returns>
        public static String WebToString(this List<String> stringList)
        {
            Boolean isFirstItem;
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();
            if (stringList.IsNotEmpty())
            {
                if (stringList.Count == 1)
                {
                    stringBuilder.Append("[" + stringList[0] + "]");
                }
                else
                {
                    isFirstItem = true;
                    stringBuilder.Append("[");
                    foreach (String text in stringList)
                    {
                        if (isFirstItem)
                        {
                            isFirstItem = false;
                        }
                        else
                        {
                            stringBuilder.Append(", ");
                        }

                        stringBuilder.Append("[" + text + "]");
                    }

                    stringBuilder.Append("]");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
