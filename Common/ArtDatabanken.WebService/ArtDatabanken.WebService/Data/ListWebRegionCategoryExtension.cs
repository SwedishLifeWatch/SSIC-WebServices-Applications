using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to a list of WebRegionCategory instances.
    /// </summary>
    public static class ListWebRegionCategoryExtension
    {
        /// <summary>
        /// Get web region categories as string.
        /// </summary>
        /// <param name="regionCategories">Region categories.</param>
        /// <returns>Web region categories as string.</returns>
        public static String WebToString(this List<WebRegionCategory> regionCategories)
        {
            Int32 index;
            StringBuilder stringBuilder;

            if (regionCategories.IsEmpty())
            {
                return String.Empty;
            }
            else
            {
                if (regionCategories.Count == 1)
                {
                    return regionCategories[0].WebToString();
                }
                else
                {
                    stringBuilder = new StringBuilder();
                    stringBuilder.Append("[[" + regionCategories[0].WebToString() + "]");
                    for (index = 1; index < regionCategories.Count; index++)
                    {
                        stringBuilder.Append(", [" + regionCategories[index].WebToString() + "]");
                    }

                    stringBuilder.Append("]");
                    return stringBuilder.ToString();
                }
            }
        }
    }
}
