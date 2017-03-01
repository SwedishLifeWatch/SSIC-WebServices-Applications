using System;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionType class.
    /// </summary>
    public static class WebRegionTypeExtension
    {
        /// <summary>
        /// Get WebRegionType as a string.
        /// </summary>
        /// <param name="regionType">Search criteria.</param>
        /// <returns>WebRegionType as a string</returns>
        public static String WebToString(this WebRegionType regionType)
        {
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();

            if (regionType.IsNotNull())
            {
                stringBuilder.Append("Region type: ");
                stringBuilder.Append("Id = " + regionType.Id);
                stringBuilder.Append(", Name = " + regionType.Name);
            }

            return stringBuilder.ToString();
        }
    }
}
