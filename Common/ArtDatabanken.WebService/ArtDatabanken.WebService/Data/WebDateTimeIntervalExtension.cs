using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    using System.Text;

    /// <summary>
    /// Contains extension to the WebDateTimeInterval class.
    /// </summary>
    public static class WebDateTimeIntervalExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="dateTimeInterval">DateTime interval.</param>
        /// <param name="checkBeginEnd">
        /// Indicates if Begin must have a lower value than End.
        /// </param>
        public static void CheckData(this WebDateTimeInterval dateTimeInterval, Boolean checkBeginEnd)
        {
            if (dateTimeInterval.IsNotNull())
            {
                if (checkBeginEnd && (dateTimeInterval.Begin.Ticks > dateTimeInterval.End.Ticks))
                {
                    throw new ArgumentException(
                        "Begin must be older than End. Begin = " + Convert.ToString(dateTimeInterval.Begin) + " End = "
                        + Convert.ToString(dateTimeInterval.End));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeInterval"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="startColumn"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        public static string CreateSearchCriteriaDayOfYear(
            this WebDateTimeInterval dateTimeInterval,
            WebDateTimeSearchCriteria searchCriteria,
            string startColumn,
            string endColumn)
        {
            String condition = String.Empty;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            int intervalBeginDay = dateTimeInterval.Begin.DayOfYear;
            int intervalEndDay = dateTimeInterval.End.DayOfYear;

            string obsStartDay = " (DATEPART(dayofyear, " + startColumn + ")) ";
            string obsEndDay = " (DATEPART(dayofyear, " + endColumn + ")) ";

            if (dateTimeInterval.Begin.DayOfYear <= dateTimeInterval.End.DayOfYear)
            {
                // The interval is within a year
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.Excluding:
                        condition = " ( " + obsStartDay + " >= " + intervalBeginDay + ") " + " AND (" + obsEndDay
                                    + " <= " + intervalEndDay + ") " + " AND (" + obsStartDay + " <= " + obsEndDay + ")";
                        break;
                    case CompareOperator.Including:
                        condition = " ( " + obsStartDay + " <= " + intervalEndDay + ")" + " AND " + " ( " + obsEndDay
                                    + " >= " + intervalBeginDay + ")";
                        break;
                    default:
                        throw new ArgumentException(
                            "Not supported operator in WebDateTimeSearchCriteria. Operator = " + searchCriteria.Operator);
                }
            }
            else
            {
                // if the interval is NOT WITHIN a year
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.Excluding:
                        //Check that the startmonth + startdate is after the intervalStart
                        //Check that the endmonth + enddate is before the intervalEnd
                        //Check that the observation not is longer than the interval
                        int days = (dateTimeInterval.End - dateTimeInterval.Begin).Days;

                        string obsLengthCheck = "(DATEDIFF(day, [start], [end]) <= " + days + ")";

                        string obsBeforeNye = "(" + obsStartDay + " >= " + intervalBeginDay + ") AND " + "(" + obsEndDay
                                              + " >= " + intervalBeginDay + ") ";

                        string obsAfterNyd = "(" + obsStartDay + " <= " + intervalBeginDay + ") AND" + "(" + obsEndDay
                                             + " <= " + intervalEndDay + ")";

                        string obsOverNy = "(" + obsStartDay + " >= " + intervalBeginDay + ") AND " + "(" + obsEndDay
                                           + " <= " + intervalEndDay + ")";

                        condition = obsLengthCheck + " AND (" + "(" + obsBeforeNye + ") OR (" + obsAfterNyd + ") OR ("
                                    + obsOverNy + "))";

                        break;


                    case CompareOperator.Including:
                        condition = " ((( " + obsStartDay + " >= " + intervalBeginDay + ") OR (" + obsStartDay + " <= "
                                    + intervalEndDay + " )) " + " OR " + " (( " + obsEndDay + " <= " + intervalEndDay
                                    + ") OR (" + obsEndDay + " >= " + intervalBeginDay + " ))) ";

                        break;
                    default:
                        throw new ArgumentException(
                            "Not supported operator in WebDateTimeSearchCriteria. Operator = " + searchCriteria.Operator);
                }
            }
#endif
            return condition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeInterval"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="startColumn"></param>
        /// <param name="endColumn"></param>
        /// <returns></returns>
        public static string CreateSearchCriteriaDates(
            this WebDateTimeInterval dateTimeInterval,
            WebDateTimeSearchCriteria searchCriteria,
            string startColumn,
            string endColumn)
        {
            string condition = String.Empty;
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE

            //Calculate by using month and date
            //AND DATEPART(mm, [start]) >= 9 AND DATEPART(dd, [start]) >= 9 
            //AND DATEPART(mm, [end]) <= 9 AND DATEPART(dd, [end]) <= 10
            //string startMonth = "DATEPART(mm, [start])";
            //string startDay = "DATEPART(dd, [start])";
            string startMonthDay = "DATEPART(mm, [start]) * 100 + DATEPART(dd, [start])";
            // string endMonth = "DATEPART(mm, [end])";
            // string endDay = "DATEPART(dd, [end])";
            string endMonthDay = "DATEPART(mm, [end]) * 100 + DATEPART(dd, [end])";

            // int intervalBeginMonth = dateTimeInterval.Begin.Month;
            // int intervalBeginDay = dateTimeInterval.Begin.Day;
            int intervalBeginMonthDay = dateTimeInterval.Begin.Month * 100 + dateTimeInterval.Begin.Day;
            // int intervalEndMonth = dateTimeInterval.End.Month;
            // int intervalEndDay = dateTimeInterval.End.Day;
            int intervalEndMonthDay = dateTimeInterval.End.Month * 100 + dateTimeInterval.End.Day;
            //int nye = 1231;
            //int nyd = 101;

            if (dateTimeInterval.Begin.DayOfYear <= dateTimeInterval.End.DayOfYear)
            {
                // The interval is WITHIN a year

                switch (searchCriteria.Operator)
                {

                    case CompareOperator.Excluding:
                        //Check that the startmonth + startdate is after the intervalStart
                        //Check that the endmonth + enddate is before the intervalEnd
                        //Check that the observation not is longer than the interval
                        int days = (dateTimeInterval.End - dateTimeInterval.Begin).Days;
                        condition = " ( " + "(" + startMonthDay + ") >= " + intervalBeginMonthDay + " AND " + "("
                                    + endMonthDay + ") <= " + intervalEndMonthDay + " AND " + days
                                    + " >= DATEDIFF(day, [start], [end]) )";
                        break;

                    case CompareOperator.Including:
                        //Check that the startmonth + startdate is before the intervalEnd
                        //Check that the endmonth + enddate is after the intervalStart
                        condition = " ( " + "(" + startMonthDay + ") <= " + intervalEndMonthDay + " AND " + "("
                                    + endMonthDay + ") >= " + intervalBeginMonthDay + " )";
                        break;

                    default:
                        throw new ArgumentException(
                            "Not supported operator in WebDateTimeSearchCriteria. Operator = " + searchCriteria.Operator);
                }
            }
            else
            {
                // The interval is NOT WITHIN a year
                switch (searchCriteria.Operator)
                {
                    case CompareOperator.Excluding:
                        //Check that the startmonth + startdate is after the intervalStart
                        //Check that the endmonth + enddate is before the intervalEnd
                        //Check that the observation not is longer than the interval
                        int days = (dateTimeInterval.End - dateTimeInterval.Begin).Days;

                        string obsLengthCheck = "(DATEDIFF(day, [start], [end]) <= " + days + ")";

                        string obsBeforeNye = "(" + startMonthDay + " >= " + intervalBeginMonthDay + ") AND " + "("
                                              + endMonthDay + " >= " + intervalBeginMonthDay + ") ";

                        string obsAfterNyd = "(" + startMonthDay + " <= " + intervalEndMonthDay + ") AND" + "("
                                             + endMonthDay + " <= " + intervalEndMonthDay + ")";

                        string obsOverNy = "(" + startMonthDay + " >= " + intervalBeginMonthDay + ") AND " + "("
                                           + endMonthDay + " <= " + intervalEndMonthDay + ")";

                        condition = obsLengthCheck + " AND (" + "(" + obsBeforeNye + ") OR (" + obsAfterNyd + ") OR ("
                                    + obsOverNy + "))";

                        break;
                    case CompareOperator.Including:
                        condition = " ((( " + startMonthDay + " >= " + intervalBeginMonthDay + ") OR (" + startMonthDay
                                    + " <= " + intervalEndMonthDay + " )) " + " OR " + " (( " + endMonthDay + " <= "
                                    + intervalEndMonthDay + ") OR (" + endMonthDay + " >= " + intervalBeginMonthDay
                                    + " ))) ";
                        break;

                    default:
                        throw new ArgumentException(
                            "Not supported operator in WebDateTimeSearchCriteria. Operator = " + searchCriteria.Operator);
                }
            }
#endif
            return condition;
        }

        /// <summary>
        /// Get string filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="dateTimeColumn">Name of the field with a DateTime value.</param>
        /// <returns>String filter.</returns>
        public static String GetFilter(this WebDateTimeInterval searchCriteria, String dateTimeColumn)
        {
            StringBuilder filter;

            filter = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if (searchCriteria.IsDayOfYearSpecified)
                {
                    // Check if the interval is within a year. 
                    if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                    {
                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfYear\": {");
                        filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear + ",");
                        filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                        filter.Append("}}}");
                    }
                    else
                    {
                        // If the interval is not within a year.
                        filter.Append("{\"bool\":{ \"should\" : [");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfYear\": {");
                        filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                        filter.Append("}}}, ");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfYear\": {");
                        filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                        filter.Append("}}}");

                        filter.Append("]}}");
                    }
                }
                else
                {
                    // Day of year is not specified.
                    // Use exact day and month value to compare with.

                    // Check if the interval is within a year. 
                    if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                    {
                        // Handle search criteria Begin.
                        filter.Append("{\"bool\":{ \"should\" : [");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_MonthOfYear\": {");
                        filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                        filter.Append("}}}, ");

                        filter.Append("{\"bool\":{ \"must\" : [");

                        filter.Append("{ \"term\": {\"" + dateTimeColumn + "_MonthOfYear\": ");
                        filter.Append(searchCriteria.Begin.Month);
                        filter.Append("}}, ");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfMonth\": {");
                        filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                        filter.Append("}}}");

                        filter.Append("]}}"); // End AND.
                        filter.Append("]}},"); // End OR.

                        // Handle search criteria End.
                        filter.Append("{\"bool\":{ \"should\" : [");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_MonthOfYear\": {");
                        filter.Append(" \"lt\": " + searchCriteria.End.Month);
                        filter.Append("}}}, ");

                        filter.Append("{\"bool\":{ \"must\" : [");

                        filter.Append("{ \"term\": {\"" + dateTimeColumn + "_MonthOfYear\": ");
                        filter.Append(searchCriteria.End.Month);
                        filter.Append("}}, ");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfMonth\": {");
                        filter.Append(" \"lte\": " + searchCriteria.End.Day);
                        filter.Append("}}}");

                        filter.Append("]}}"); // End AND.
                        filter.Append("]}}"); // End OR.
                    }
                    else
                    {
                        // If the interval is not within a year.
                        // Handle search criteria Begin.
                        filter.Append("{\"bool\":{ \"should\" : [");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_MonthOfYear\": {");
                        filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                        filter.Append("}}}, ");

                        filter.Append("{\"bool\":{ \"must\" : [");

                        filter.Append("{ \"term\": {\"" + dateTimeColumn + "_MonthOfYear\": ");
                        filter.Append(searchCriteria.Begin.Month);
                        filter.Append("}}, ");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfMonth\": {");
                        filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                        filter.Append("}}}");

                        filter.Append("]}}, "); // End AND.

                        // Handle search criteria End.
                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_MonthOfYear\": {");
                        filter.Append(" \"lt\": " + searchCriteria.End.Month);
                        filter.Append("}}}, ");

                        filter.Append("{\"bool\":{ \"must\" : [");

                        filter.Append("{ \"term\": {\"" + dateTimeColumn + "_MonthOfYear\": ");
                        filter.Append(searchCriteria.End.Month);
                        filter.Append("}}, ");

                        filter.Append("{ \"range\": {\"" + dateTimeColumn + "_DayOfMonth\": {");
                        filter.Append(" \"lte\": " + searchCriteria.End.Day);
                        filter.Append("}}}");

                        filter.Append("]}}"); // End AND.
                        filter.Append("]}}"); // End OR.
                    }
                }
#endif
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
        /// <param name="compareOperator">
        /// Operator that should be used between this date time
        /// interval search criteria and the data that it is compared to.
        /// Operator must be set to CompareOperator:Excluding or
        /// CompareOperator:Including.
        /// </param>
        /// <returns>String filter.</returns>
        public static String GetFilter(this WebDateTimeInterval searchCriteria,
                                       String startDateTimeColumn,
                                       String endDateTimeColumn,
                                       String accuracyColumn,
                                       CompareOperator compareOperator)
        {
            Int64 accuracyInSeconds;
            StringBuilder filter;

            filter = new StringBuilder();
            if (searchCriteria.IsNotNull())
            {
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
                if (compareOperator == CompareOperator.Excluding)
                {
                    filter.Append("{\"bool\":{ \"must\" : [");

                    if (searchCriteria.IsDayOfYearSpecified)
                    {
                        // Check if the interval is within a year. 
                        if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                        {
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append(", \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");
                            filter.Append(",{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append(", \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");

                            // Remove observations with bad accuracy.
                            accuracyInSeconds = (searchCriteria.End.DayOfYear - searchCriteria.Begin.DayOfYear + 1) * 86400;
                            filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"lte\": " + accuracyInSeconds);
                            filter.Append("}}}");
                        }
                        else
                        {
                            // If the interval is not within a year.
                            filter.Append("{\"bool\":{ \"should\" : [");
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append("}}}");
                            filter.Append(",{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");
                            filter.Append("]}}");

                            filter.Append(",{\"bool\":{ \"should\" : [");
                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append("}}}");
                            filter.Append(",{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");
                            filter.Append("]}}");

                            // Remove observations with bad accuracy.
                            accuracyInSeconds = (searchCriteria.End.DayOfYear + 366 - searchCriteria.Begin.DayOfYear) * 86400;
                            filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"lte\": " + accuracyInSeconds);
                            filter.Append("}}}");
                        }
                    }
                    else
                    {
                        // Day of year is not specified.
                        // Use exact day and month value to compare with.

                        // Check if the interval is within a year. 
                        if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                        {
                            // Handle start field compared to search criteria Begin.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}},"); // End OR.

                            // Handle start field compared to search criteria End.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}},"); // End OR.

                            // Handle end field compared to search criteria Begin.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}},"); // End OR.

                            // Handle end field compared to search criteria End.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}}"); // End OR.

                            // Remove observations with bad accuracy.
                            accuracyInSeconds = (searchCriteria.End.DayOfYear - searchCriteria.Begin.DayOfYear + 1) * 86400;
                            filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"lte\": " + accuracyInSeconds);
                            filter.Append("}}}");
                        }
                        else
                        {
                            // Handle start field compared to search criteria Begin.
                            filter.Append("{\"bool\":{ \"should\" : [");
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}},"); // End OR.

                            // Handle start field compared to search criteria End.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}}"); // End OR.
                            filter.Append("]}},"); // End OR.

                            // Handle end field compared to search criteria Begin.
                            filter.Append("{\"bool\":{ \"should\" : [");
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}},"); // End OR.

                            // Handle end field compared to search criteria End.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End AND.
                            filter.Append("]}}"); // End OR.
                            filter.Append("]}}"); // End OR.

                            // Remove observations with bad accuracy.
                            accuracyInSeconds = (searchCriteria.End.DayOfYear + 366 - searchCriteria.Begin.DayOfYear) * 86400;
                            filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"lte\": " + accuracyInSeconds);
                            filter.Append("}}}");
                        }
                    }

                    filter.Append("]}}"); // End AND.
                }

                if (compareOperator == CompareOperator.Including)
                {
                    if (searchCriteria.IsDayOfYearSpecified)
                    {
                        // Check if the interval is within a year. 
                        if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                        {
                            // If the interval is within a year.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append(", \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}, ");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append(", \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append("}}}");
                            filter.Append(",{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");
                            filter.Append("]}}, ");

                            // Add observations with bad accuracy.
                            accuracyInSeconds = 366 * 86400;
                            filter.Append("{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"gte\": " + accuracyInSeconds);
                            filter.Append("}}}");

                            filter.Append("]}}");
                        }
                        else
                        {
                            // If the interval is not within a year.
                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append("}}}");

                            filter.Append(",{ \"range\": {\"" + startDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");

                            filter.Append(",{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.DayOfYear);
                            filter.Append("}}}");

                            filter.Append(",{ \"range\": {\"" + endDateTimeColumn + "_DayOfYear\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.DayOfYear);
                            filter.Append("}}}");

                            filter.Append(",{ \"script\": {");
                            filter.Append(" \"script\": \"doc['" + startDateTimeColumn + "_DayOfYear'].value > ");
                            filter.Append("doc['" + endDateTimeColumn + "_DayOfYear'].value\"");
                            filter.Append("}}");

                            // Add observations with bad accuracy.
                            accuracyInSeconds = 366 * 86400;
                            filter.Append(",{ \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"gte\": " + accuracyInSeconds);
                            filter.Append("}}}");

                            filter.Append("]}}");
                        }
                    }
                    else
                    {
                        // Day of year is not specified.
                        // Use exact day and month value to compare with.

                        // Check if the interval is within a year. 
                        if (searchCriteria.Begin.DayOfYear <= searchCriteria.End.DayOfYear)
                        {
                            filter.Append("{\"bool\":{ \"should\" : [");

                            // Check if start is inside of search criteria.
                            filter.Append("{\"bool\":{ \"must\" : [");

                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.

                            filter.Append(", {\"bool\":{ \"should\" : [");
                            
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.
                            filter.Append("]}}"); // End AND.

                            // Check if end is inside of search criteria.
                            filter.Append(", {\"bool\":{ \"must\" : [");

                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append(" { \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.

                            filter.Append(", {\"bool\":{ \"should\" : [");

                            filter.Append(" { \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.
                            filter.Append("]}}"); // End AND.

                            // Check if start and end covers the entire search criteria.
                            filter.Append(", {\"bool\":{ \"must\" : [");

                            filter.Append("{\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lt\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.

                            filter.Append(", {\"bool\":{ \"should\" : [");

                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.End.Month);
                            filter.Append("}}}, ");

                            filter.Append("{\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}, ");
                            filter.Append("{ \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gt\": " + searchCriteria.End.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append("]}}"); // End OR.
                            filter.Append("]}}"); // End AND.

                            // Add observations with bad accuracy.
                            accuracyInSeconds = 366 * 86400;
                            filter.Append(", { \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"gte\": " + accuracyInSeconds);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End OR.
                        }
                        else
                        {
                            // Interval is not within a year. 
                            filter.Append("{\"bool\":{ \"should\" : [");

                            // Check if start is inside of search criteria.
                            filter.Append("{ \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}");

                            filter.Append(", {\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}");
                            filter.Append(", { \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append(", { \"range\": {\"" + startDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}");

                            filter.Append(", {\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + startDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}");
                            filter.Append(", { \"range\": {\"" + startDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            // Check if end is inside of search criteria.
                            filter.Append(", { \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"gt\": " + searchCriteria.Begin.Month);
                            filter.Append("}}}");

                            filter.Append(", {\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.Begin.Month);
                            filter.Append("}}");
                            filter.Append(", { \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"gte\": " + searchCriteria.Begin.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            filter.Append(", { \"range\": {\"" + endDateTimeColumn + "_MonthOfYear\": {");
                            filter.Append(" \"lt\": " + searchCriteria.End.Month);
                            filter.Append("}}}");

                            filter.Append(", {\"bool\":{ \"must\" : [");
                            filter.Append("{ \"term\": {\"" + endDateTimeColumn + "_MonthOfYear\": ");
                            filter.Append(searchCriteria.End.Month);
                            filter.Append("}}");
                            filter.Append(", { \"range\": {\"" + endDateTimeColumn + "_DayOfMonth\": {");
                            filter.Append(" \"lte\": " + searchCriteria.End.Day);
                            filter.Append("}}}");
                            filter.Append("]}}"); // End AND.

                            // Check if start and end covers the entire search criteria.
                            filter.Append(",{ \"script\": {");
                            filter.Append(" \"script\": \"doc['" + startDateTimeColumn + "_MonthOfYear'].value > ");
                            filter.Append("doc['" + endDateTimeColumn + "_MonthOfYear'].value\"");
                            filter.Append("}}");

                            filter.Append(", {\"bool\":{ \"must\" : [");
                            filter.Append("{ \"script\": {");
                            filter.Append(" \"script\": \"doc['" + startDateTimeColumn + "_MonthOfYear'].value == ");
                            filter.Append("doc['" + endDateTimeColumn + "_MonthOfYear'].value\"");
                            filter.Append("}}");
                            filter.Append(",{ \"script\": {");
                            filter.Append(" \"script\": \"doc['" + startDateTimeColumn + "_DayOfMonth'].value > ");
                            filter.Append("doc['" + endDateTimeColumn + "_DayOfMonth'].value\"");
                            filter.Append("}}");
                            filter.Append("]}}"); // End AND.

                            // Add observations with bad accuracy.
                            accuracyInSeconds = 366 * 86400;
                            filter.Append(", { \"range\": {\"" + accuracyColumn + "\": {");
                            filter.Append(" \"gte\": " + accuracyInSeconds);
                            filter.Append("}}}");

                            filter.Append("]}}"); // End OR.
                        }
                    }
                }
#endif
            }

            return filter.ToString();
        }
    }
}