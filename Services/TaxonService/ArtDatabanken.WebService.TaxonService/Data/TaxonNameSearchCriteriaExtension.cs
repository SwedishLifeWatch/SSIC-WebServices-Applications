using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.TaxonService.Data
{
    /// <summary>
    /// Contains extension methods to the WebTaxonNameSearchCriteria class.
    /// </summary>
    public static class TaxonNameSearchCriteriaExtension
    {
        /// <summary>
        /// Check the data in current object
        /// </summary>
        /// <param name='searchCriteria'>Search criteria.</param>
        public static void CheckData(this WebTaxonNameSearchCriteria searchCriteria)
        {
            if (searchCriteria.IsNotNull())
            {
                searchCriteria.CheckStrings();
                if (searchCriteria.AuthorSearchString.IsNotNull())
                {
                    searchCriteria.AuthorSearchString.SearchString = searchCriteria.AuthorSearchString.SearchString.CheckSqlInjection();
                }

                if (searchCriteria.NameSearchString.IsNotNull())
                {
                    searchCriteria.NameSearchString.SearchString = searchCriteria.NameSearchString.SearchString.CheckSqlInjection();
                }
            }
        }
    }
}
