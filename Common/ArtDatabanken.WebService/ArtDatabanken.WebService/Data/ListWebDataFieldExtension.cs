using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to a list of WebDataField instances.
    /// </summary>
    public static class ListWebDataFieldExtension
    {
        /// <summary>
        /// Get web data fields as string.
        /// </summary>
        /// <param name="dataFields">Data fields.</param>
        /// <returns>Web data fields as string.</returns>
        public static String WebToString(this List<WebDataField> dataFields)
        {
            Int32 index;
            StringBuilder stringBuilder;

            if (dataFields.IsEmpty())
            {
                return String.Empty;
            }
            else
            {
                if (dataFields.Count == 1)
                {
                    return ", " + dataFields[0].WebToString();
                }
                else
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.Append(", [[" + dataFields[0].WebToString() + "]");
                    for (index = 1; index < dataFields.Count; index++)
                    {
                        stringBuilder.Append(", [" + dataFields[index].WebToString() + "]");
                    }

                    stringBuilder.Append("]");
                    return stringBuilder.ToString();
                }
            }
        }
    }
}
