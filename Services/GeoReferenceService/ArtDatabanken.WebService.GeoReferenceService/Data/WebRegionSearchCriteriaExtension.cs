using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Contains extension to the WebRegionSearchCriteria class.
    /// </summary>
    public static class WebRegionSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">This region search criteria.</param>
        public static void CheckData(this WebRegionSearchCriteria searchCriteria)
        {
            if (searchCriteria.Categories.IsNotEmpty())
            {
                foreach (WebRegionCategory category in searchCriteria.Categories)
                {
                    category.CheckData();
                }
            }
        }
    }
}
