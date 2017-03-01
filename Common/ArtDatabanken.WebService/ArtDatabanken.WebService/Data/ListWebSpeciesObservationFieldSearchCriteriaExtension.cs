using System;
using System.Collections.Generic;
using System.Text;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list of
    /// type WebSpeciesObservationFieldSearchCriteria.
    /// </summary>
    public static class ListWebSpeciesObservationFieldSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="mapping">Information about fields in Elasticsearch.</param>
        public static void CheckData(this List<WebSpeciesObservationFieldSearchCriteria> searchCriteria,
                                     Dictionary<String, WebSpeciesObservationField> mapping)
        {
            if (searchCriteria.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria in searchCriteria)
                {
                    fieldSearchCriteria.CheckData(mapping);
                }
            }
        }

        /// <summary>
        /// Get species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Species observation filter.</returns>
        public static String GetFilter(this List<WebSpeciesObservationFieldSearchCriteria> searchCriteria)
        {
            Boolean isFirstFilterSetting;
            StringBuilder filter;

            isFirstFilterSetting = true;
            filter = new StringBuilder();
            if (searchCriteria.IsNotEmpty())
            {
                foreach (WebSpeciesObservationFieldSearchCriteria fieldSearchCriteria in searchCriteria)
                {
                    if (isFirstFilterSetting)
                    {
                        isFirstFilterSetting = false;
                    }
                    else
                    {
                        filter.Append(", ");
                    }

                    filter.Append(fieldSearchCriteria.GetFilter());
                }
            }

            return filter.ToString();
        }
    }
}
