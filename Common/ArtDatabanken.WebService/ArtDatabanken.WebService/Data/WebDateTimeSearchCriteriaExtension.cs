using System;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebDateTimeSearchCriteria class.
    /// </summary>
    public static class WebDateTimeSearchCriteriaExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="isComparedToRange">
        /// Indicates if date time search criteria is used to compare
        /// with date time information that also has a range.
        /// The value of this parameter affects which checks on
        /// the search criteria that is made.
        /// </param>
        public static void CheckData(this WebDateTimeSearchCriteria searchCriteria,
                                     Boolean isComparedToRange)
        {
            if (searchCriteria.IsNotNull())
            {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if ((!isComparedToRange) &&
                    searchCriteria.Accuracy.IsNotNull())
                {
                    // Accuracy is only used together with range comparison.
                    throw new ArgumentException("Accuracy is only used together with range comparison.");
                }
                searchCriteria.Accuracy.CheckData();

                // Begin must be older than End.
                if (searchCriteria.End.Ticks < searchCriteria.Begin.Ticks)
                {
                    throw new ArgumentException("Begin must be older than End. Begin = " + Convert.ToString(searchCriteria.Begin) + " End = " + Convert.ToString(searchCriteria.End));
                }

                if (isComparedToRange &&
                    !((searchCriteria.Operator == CompareOperator.Excluding) ||
                      (searchCriteria.Operator == CompareOperator.Including)))
                {
                    // Operator must be set to Excluding or Including
                    // when compared data has a range.
                    throw new ArgumentException("Operator must be set to Excluding or Including when compared data has a range.");
                }
#endif

                if (searchCriteria.PartOfYear.IsNotEmpty())
                {
                    foreach (WebDateTimeInterval partOfYear in searchCriteria.PartOfYear)
                    {
                        partOfYear.CheckData(true);
                    }
                }
            }
        }

        /// <summary>
        /// Get string filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="dateTimeColumn">Name of the field with a DateTime value.</param>
        /// <returns>String filter.</returns>
        public static String GetFilter(this WebDateTimeSearchCriteria searchCriteria,
                                       String dateTimeColumn)
        {
            Int32 index;
            StringBuilder filter;

            filter = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
                filter.Append("{ \"range\": {\"" + dateTimeColumn + "\": {");
                filter.Append(" \"gte\": \"" + searchCriteria.Begin.WebToString() + "\",");
                filter.Append(" \"lte\": \"" + searchCriteria.End.WebToString() + "\"");
                filter.Append("}}}");

                if (searchCriteria.PartOfYear.IsNotEmpty())
                {
                    filter.Append(", ");
                    if (searchCriteria.PartOfYear.Count == 1)
                    {
                        filter.Append(searchCriteria.PartOfYear[0].GetFilter(dateTimeColumn));
                    }
                    else
                    {
                        filter.Append("{\"bool\":{ \"should\" : [");
                        for (index = 0; index < searchCriteria.PartOfYear.Count; index++)
                        {
                            if (index > 0)
                            {
                                filter.Append(", ");
                            }

                            filter.Append(searchCriteria.PartOfYear[index].GetFilter(dateTimeColumn));
                        }

                        filter.Append("]}}");
                    }
                }
            }

            return filter.ToString();
        }

        /// <summary>
        /// Get string filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="startDateTimeColumn">Name of the field with a start DateTime value.</param>
        /// <param name="endDateTimeColumn">Name of the field with a end DateTime value.</param>
        /// <param name="accuracyColumn">Name of the field with an observation date time accuracy value.</param>
        /// <returns>String filter.</returns>
        public static String GetFilter(this WebDateTimeSearchCriteria searchCriteria,
                                       String startDateTimeColumn,
                                       String endDateTimeColumn,
                                       String accuracyColumn)
        {
            Int32 index;
            Int64 accuracyInSeconds;
            StringBuilder filter;

            filter = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if (searchCriteria.Operator == CompareOperator.Excluding)
                {
                    filter.Append("{ \"range\": {\"" + startDateTimeColumn + "\": {");
                    filter.Append(" \"gte\": \"" + searchCriteria.Begin.WebToString() + "\"");
                    filter.Append("}}},");
                    filter.Append("{ \"range\": {\"" + endDateTimeColumn + "\": {");
                    filter.Append(" \"lte\": \"" + searchCriteria.End.WebToString() + "\"");
                    filter.Append("}}}");
                }

                if (searchCriteria.Operator == CompareOperator.Including)
                {
                    filter.Append("{ \"range\": {\"" + startDateTimeColumn + "\": {");
                    filter.Append(" \"lte\": \"" + searchCriteria.End.WebToString() + "\"");
                    filter.Append("}}},");
                    filter.Append("{ \"range\": {\"" + endDateTimeColumn + "\": {");
                    filter.Append(" \"gte\": \"" + searchCriteria.Begin.WebToString() + "\"");
                    filter.Append("}}}");
                }

                if (0 < searchCriteria.Accuracy.GetTotalSeconds())
                {
                    accuracyInSeconds = searchCriteria.Accuracy.GetTotalSeconds();
                    filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                    filter.Append(" \"lte\": " + accuracyInSeconds);
                    filter.Append("}}}");
                }

                if (searchCriteria.PartOfYear.IsNotEmpty())
                {
                    filter.Append(", ");
                    if (searchCriteria.PartOfYear.Count == 1)
                    {
                        filter.Append(searchCriteria.PartOfYear[0].GetFilter(startDateTimeColumn,
                                                                             endDateTimeColumn,
                                                                             accuracyColumn,
                                                                             searchCriteria.Operator));
                    }
                    else
                    {
                        filter.Append("{\"bool\":{ \"should\" : [");
                        for (index = 0; index < searchCriteria.PartOfYear.Count; index++)
                        {
                            if (index > 0)
                            {
                                filter.Append(", ");
                            }

                            filter.Append(searchCriteria.PartOfYear[index].GetFilter(startDateTimeColumn,
                                                                                     endDateTimeColumn,
                                                                                     accuracyColumn,
                                                                                     searchCriteria.Operator));
                        }

                        filter.Append("]}}");
                    }
                }
#endif
            }

            return filter.ToString();
        }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="dateTimeColumn">Name of column with date time value.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this WebDateTimeSearchCriteria searchCriteria,
                                               String dateTimeColumn)
        {
            String loopCondition;
            StringBuilder whereCondition;

            whereCondition = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
                whereCondition.Append(" (" + dateTimeColumn + " >= '" + searchCriteria.Begin + "') AND " +
                                      " (" + dateTimeColumn + " <= '" + searchCriteria.End + "') ");

                if (searchCriteria.PartOfYear.IsNotEmpty())
                {
                    whereCondition.Append(" AND (");
                    loopCondition = String.Empty;
                    foreach (WebDateTimeInterval dateTimeInterval in searchCriteria.PartOfYear)
                    {
                        int intervalBeginDay = dateTimeInterval.Begin.DayOfYear;
                        int intervalEndDay = dateTimeInterval.End.DayOfYear;

                        // Adjust intervalYearLength to the lengt of year of the interval Begindate.
                        // DateTime yearmax = new DateTime(dateTimeInterval.Begin.Year, 12, 31);
                        // int intervalYearLength = yearmax.DayOfYear;

                        string obsDay = " (DATEPART(dayofyear, " + dateTimeColumn + ")) ";
                        // string obsEndDay = " (DATEPART(dayofyear, " + dateTimeColumn + ")) ";

                        //If the inteval should be calculated by day of year
                        if (dateTimeInterval.IsDayOfYearSpecified)
                        {
                            //Check if the interval is within a year 
                            if (dateTimeInterval.Begin.DayOfYear <= dateTimeInterval.End.DayOfYear)
                            {
                                whereCondition.Append(loopCondition +
                                                      " (" + obsDay + " >= " + intervalBeginDay + ") " +
                                                      " AND (" + obsDay + " <= " + intervalEndDay + ") ");

                            }
                            else
                            {
                                //if the interval is not within a year
                                whereCondition.Append(loopCondition +
                                                      " ( " +
                                                      " ( " + obsDay + " >= " + intervalBeginDay + ") OR " +
                                                      " ( " + obsDay + " <= " + intervalEndDay + ") " +
                                                      " ) " +
                                                      " AND " +
                                                      " ( " +
                                                      " ( " + obsDay + " <= " + intervalEndDay + ") OR " +
                                                      " ( " + obsDay + " >= " + intervalBeginDay + ") " +
                                                      " ) ");
                            }
                        }
                        else
                        {
                            whereCondition.Append(loopCondition +
                                                  " (" + obsDay + " >= " + intervalBeginDay + ") " +
                                                  " AND (" + obsDay + " <= " + intervalEndDay + ")");
                        }

                        loopCondition = " OR ";
                    }

                    whereCondition.Append(") ");
                }
            }

            return whereCondition.ToString();
        }
#endif


#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Get string that can be added as part of a SQL where condition.
        /// Returned string matches specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="startColumn">Name of column with start date time value.</param>
        /// <param name="endColumn">Name of column with end date time value.</param>
        /// <returns>String that can be added as part of a SQL where condition.</returns>
        public static String GetWhereCondition(this WebDateTimeSearchCriteria searchCriteria,
                                               String startColumn,
                                               String endColumn)
        {
            String loopCondition;
            StringBuilder whereCondition;

            whereCondition = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.Excluding:
                        whereCondition.Append(" (" + startColumn + " >= '" + searchCriteria.Begin + "') AND " +
                                              " (" + endColumn + " <= '" + searchCriteria.End + "') ");
                        break;
                    case CompareOperator.Including:
                        whereCondition.Append(" (" + startColumn + " <= '" + searchCriteria.End + "') AND " +
                                              " (" + endColumn + " >= '" + searchCriteria.Begin + "') ");
                        break;
                    default:
                        throw new ArgumentException("Not supported operator in WebDateTimeSearchCriteria. Operator = " + searchCriteria.Operator);
                }

                if (searchCriteria.Accuracy.IsNotNull() && searchCriteria.Accuracy.IsDaysSpecified)
                {
                    if (searchCriteria.Accuracy.IsDaysSpecified)
                    {
                        whereCondition.Append(" AND ((DATEDIFF(day, " + startColumn + ", " + endColumn + ")) <= " + searchCriteria.Accuracy.Days + ") ");
                    }
                    else
                    {
                        throw new ArgumentException("Currently only Days are supprted when testing Accuracy in WebDateTimeSearchCriteria.");
                    }
                }

                if (searchCriteria.PartOfYear.IsNotEmpty())
                {
                    whereCondition.Append(" AND (");
                    loopCondition = String.Empty;

                    foreach (WebDateTimeInterval dateTimeInterval in searchCriteria.PartOfYear)
                    {
                        //If the interval should be calculated by day of year
                        if (dateTimeInterval.IsDayOfYearSpecified)
                        {
                            whereCondition.Append(loopCondition + dateTimeInterval.CreateSearchCriteriaDayOfYear(searchCriteria, startColumn, endColumn));
                        }
                        else
                        {
                            whereCondition.Append(loopCondition + dateTimeInterval.CreateSearchCriteriaDates(searchCriteria, startColumn, endColumn));
                        }
                        loopCondition = " OR ";
                    }
                    whereCondition.Append(") ");
                }
            }

            return whereCondition.ToString();
        }
#endif
    }
}