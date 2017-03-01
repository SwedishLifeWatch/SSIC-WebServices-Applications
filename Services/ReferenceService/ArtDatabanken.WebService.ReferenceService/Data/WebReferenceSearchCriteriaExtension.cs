using System;
using System.Text;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService.Data
{
    using ArtDatabanken.Data;

    /// <summary>
    /// Contains extension to the WebReferenceSearchCriteria class.
    /// </summary>
    public static class WebReferenceSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">The reference instance.</param>
        public static void CheckData(this WebReferenceSearchCriteria searchCriteria)
        {
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.NameSearchString.CheckData(true);
            searchCriteria.TitleSearchString.CheckData(true);

            if (searchCriteria.NameSearchString.IsNull() &&
                searchCriteria.TitleSearchString.IsNull() &&
                searchCriteria.Years.IsEmpty())
            {
                throw new ArgumentException("WebReferenceSearchCriteria: At least one search criteria must be specified.");
            }

            if (searchCriteria.LogicalOperator == LogicalOperator.Not)
            {
                throw new ArgumentException("WebReferenceSearchCriteria: Logical operator not is not supported.");
            }
        }

        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified search criteria.
        /// This procedure only handles one string compare operator.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this WebReferenceSearchCriteria searchCriteria)
        {
            String condition;
            StringBuilder whereCondition;

            whereCondition = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
                whereCondition.Append(searchCriteria.NameSearchString.GetWhereCondition(ReferenceData.NAME_COLUMN_NAME));
                condition = searchCriteria.TitleSearchString.GetWhereCondition(ReferenceData.TITLE_COLUMN_NAME);
                if (whereCondition.ToString().IsNotEmpty() &&
                    condition.IsNotEmpty())
                {
                    whereCondition.Append(" " + searchCriteria.LogicalOperator + " ");
                }

                whereCondition.Append(condition);
                condition = searchCriteria.Years.GetWhereCondition(ReferenceData.YEAR_COLUMN_NAME);
                if (whereCondition.ToString().IsNotEmpty() &&
                    condition.IsNotEmpty())
                {
                    whereCondition.Append(" " + searchCriteria.LogicalOperator + " ");
                }

                whereCondition.Append(condition);
            }

            return whereCondition.ToString();
        }
    }
}
