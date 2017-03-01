using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebStringSearchCriteria class.
    /// </summary>
    public static class WebStringSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="isSingleCompareOperatorExpected">
        /// Indicates if exactly one compare operator should be specified.
        /// </param>
        /// <param name="isElasticsearchUsed">
        /// Indicates if Elsticsearch is used.
        /// Some handling of search criteria differs depending
        /// on which data source that will be used.
        /// </param>
        public static void CheckData(this WebStringSearchCriteria searchCriteria,
                                     Boolean isSingleCompareOperatorExpected,
                                     Boolean isElasticsearchUsed = false)
        {
            if (searchCriteria.IsNotNull())
            {
                if (isSingleCompareOperatorExpected)
                {
                    // Exactly one string compare operator must be specified.
                    if (searchCriteria.CompareOperators.IsEmpty() ||
                        searchCriteria.CompareOperators.Count > 1)
                    {
                        throw new ArgumentException("WebStringSearchCriteria: Exactly one string compare operator must be specified.");
                    }
                }
                else
                {
                    // At least one string compare operator must be specified.
                    if (searchCriteria.CompareOperators.IsEmpty())
                    {
                        throw new ArgumentException("WebStringSearchCriteria: At least one string compare operator must be specified.");
                    }
                }

                if (isElasticsearchUsed)
                {
                    searchCriteria.SearchString = searchCriteria.SearchString.CheckJsonInjection();
                }
                else
                {
                    searchCriteria.SearchString = searchCriteria.SearchString.CheckInjection();
                }
            }
        }

        /// <summary>
        /// Get string filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="stringColumn">Name of the field with a string value.</param>
        /// <returns>String filter.</returns>
        public static String GetFilter(this WebStringSearchCriteria searchCriteria,
                                       String stringColumn)
        {
            if (searchCriteria.IsNotNull())
            {
                switch (searchCriteria.CompareOperators[0])
                {
                    case StringCompareOperator.BeginsWith:
                        return "{ \"prefix\" : { \"" + stringColumn + "_Lowercase" + "\" : \"" + searchCriteria.SearchString.ToLower() + "\" }}";

                    case StringCompareOperator.Contains:
                        return "{\"query\" : { \"wildcard\" : { \"" + stringColumn + "_Lowercase" + "\" : \"*" + searchCriteria.SearchString.ToLower() + "*\"}}}";

                    case StringCompareOperator.EndsWith:
                        return "{\"query\" : { \"wildcard\" : { \"" + stringColumn + "_Lowercase" + "\" : \"*" + searchCriteria.SearchString.ToLower() + "\"}}}";

                    case StringCompareOperator.Equal:
                        return "{ \"term\" : { \"" + stringColumn + "_Lowercase" + "\" : \"" + searchCriteria.SearchString.ToLower() + "\" }}";

                    case StringCompareOperator.Like:
                        return "{\"query\" : { \"wildcard\" : { \"" + stringColumn + "_Lowercase" + "\" : \"" + searchCriteria.SearchString.ToLower() + "\"}}}";

                    case StringCompareOperator.NotEqual:
                        return "{not : { \"term\" : { \"" + stringColumn + "_Lowercase" + "\" : \"" + searchCriteria.SearchString.ToLower() + "\" }}}";

                    default:
                        throw new ArgumentException("Not handled string compare operator = " + searchCriteria.CompareOperators[0]);
                }
            }

            return null;
        }

        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified search criteria.
        /// This procedure only handles one string compare operator.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="stringColumn">Name of column with string value.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this WebStringSearchCriteria searchCriteria,
                                               String stringColumn)
        {
            if (searchCriteria.IsNotNull())
            {
                switch (searchCriteria.CompareOperators[0])
                {
                    case StringCompareOperator.BeginsWith:
                        return " (" + stringColumn + " LIKE '" + searchCriteria.SearchString + "%') ";

                    case StringCompareOperator.Contains:
                        return " (" + stringColumn + " LIKE '%" + searchCriteria.SearchString + "%') ";

                    case StringCompareOperator.EndsWith:
                        return " (" + stringColumn + " LIKE '%" + searchCriteria.SearchString + "') ";

                    case StringCompareOperator.Equal:
                        return " (" + stringColumn + " = '" + searchCriteria.SearchString + "') ";

                    case StringCompareOperator.Like:
                        return " (" + stringColumn + " LIKE '" + searchCriteria.SearchString + "') ";

                    case StringCompareOperator.NotEqual:
                        return " (" + stringColumn + " <> '" + searchCriteria.SearchString + "') ";

                    default:
                        throw new ArgumentException("Not handled string compare operator = " + searchCriteria.CompareOperators[0].ToString());
                }
            }

            return null;
        }

        /// <summary>
        /// Get search criteria system as string.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Search criteria as string.</returns>
        public static String WebToString(this WebStringSearchCriteria searchCriteria)
        {
            Int32 index;
            StringBuilder stringBuilder;

            if (searchCriteria.IsNull())
            {
                return String.Empty;
            }
            else
            {
                stringBuilder = new StringBuilder();
                stringBuilder.Append("String search criteria: ");
                if (searchCriteria.CompareOperators.IsNotEmpty())
                {
                    if (searchCriteria.CompareOperators.Count == 1)
                    {
                        stringBuilder.Append("Compare operator = " + searchCriteria.CompareOperators[0] + ", ");
                    }
                    else
                    {
                        stringBuilder.Append("Compare operators = [" + searchCriteria.CompareOperators[0]);
                        for (index = 1; index < searchCriteria.CompareOperators.Count; index++)
                        {
                            stringBuilder.Append(", " + searchCriteria.CompareOperators[index]);
                        }

                        stringBuilder.Append("], ");
                    }
                }

                stringBuilder.Append("Search string = [" + searchCriteria.SearchString + "]");
                return stringBuilder.ToString();
            }
        }
    }
}
