using System;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extensions to WebRegionCategory instances.
    /// </summary>
    public static class WebRegionCategoryExtension
    {
        /// <summary>
        /// Get web region category as string.
        /// </summary>
        /// <param name="regionCategory">Region category.</param>
        /// <returns>Web region category as string.</returns>
        public static String WebToString(this WebRegionCategory regionCategory)
        {
            StringBuilder stringBuilder;

            if (regionCategory.IsNull())
            {
                return String.Empty;
            }
            else
            {
                stringBuilder = new StringBuilder();
                stringBuilder.Append("Region category:");
                stringBuilder.Append(" Id = " + regionCategory.Id);
                if (regionCategory.GUID.IsNotEmpty())
                {
                    stringBuilder.Append(", GUID = " + regionCategory.GUID);
                }

                stringBuilder.Append(", Name = " + regionCategory.Name);
                if (regionCategory.NativeIdSource.IsNotEmpty())
                {
                    stringBuilder.Append(", NativeIdSource = " + regionCategory.NativeIdSource);
                }

                stringBuilder.Append(", SortOrder = " + regionCategory.SortOrder);
                stringBuilder.Append(", TypeId = " + regionCategory.TypeId);
                if (regionCategory.IsCountryIsoCodeSpecified)
                {
                    stringBuilder.Append(", CountryIsoCode = " + regionCategory.CountryIsoCode);
                }

                if (regionCategory.IsLevelSpecified)
                {
                    stringBuilder.Append(", Level = " + regionCategory.Level);
                }

                return stringBuilder.ToString();
            }
        }
    }
}
