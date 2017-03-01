using System;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionSearchCriteria class.
    /// </summary>
    public static class WebRegionSearchCriteriaExtension
    {
        /// <summary>
        /// Get WebRegionSearchCriteria as a string.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>WebRegionSearchCriteria as a string</returns>
        public static String WebToString(this WebRegionSearchCriteria searchCriteria)
        {
            StringBuilder stringBuilder;

            stringBuilder = new StringBuilder();

            if (searchCriteria.IsNotNull())
            {
                stringBuilder.Append("Region search criteria: ");
                if (searchCriteria.Categories.IsNotEmpty())
                {
                    stringBuilder.Append(searchCriteria.Categories.WebToString() + ", ");
                }

                if (searchCriteria.CountryIsoCodes.IsNotEmpty())
                {
                    stringBuilder.Append("CountryIsoCodes = " + searchCriteria.CountryIsoCodes.WebToString() + ", ");
                }

                if (searchCriteria.NameSearchString.IsNotNull())
                {
                    stringBuilder.Append("NameSearchString = " + searchCriteria.NameSearchString.WebToString() + ", ");
                }

                if (searchCriteria.Type.IsNotNull())
                {
                    stringBuilder.Append("Type = [" + searchCriteria.Type.WebToString() + "]");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
