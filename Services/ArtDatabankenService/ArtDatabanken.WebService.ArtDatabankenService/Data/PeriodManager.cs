using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// PeriodManager.
    /// </summary>
    public class PeriodManager
    {
        /// <summary>
        /// Inserts a list of period ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="periodIds">Id for periods to insert.</param>
        /// <param name="periodUsage">How user selected periods should be used.</param>
        public static void AddUserSelectedPeriods(WebServiceContext context,
                                                  List<Int32> periodIds,
                                                  UserSelectedPeriodUsage periodUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable periodTable;

            if (periodIds.IsNotEmpty())
            {
                // Delete all periods that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedPeriods(context);

                // Insert the new list of periods.
                periodTable = new DataTable(UserSelectedPeriodData.TABLE_NAME);
                column = new DataColumn(UserSelectedPeriodData.REQUEST_ID, typeof(Int32));
                periodTable.Columns.Add(column);
                column = new DataColumn(UserSelectedPeriodData.PERIOD_ID, typeof(Int32));
                periodTable.Columns.Add(column);
                column = new DataColumn(UserSelectedPeriodData.PERIOD_USAGE, typeof(String));
                periodTable.Columns.Add(column);
                foreach (Int32 periodId in periodIds)
                {
                    row = periodTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = periodId;
                    row[2] = periodUsage.ToString();
                    periodTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedPeriods(context, periodTable);
            }
        }

        /// <summary>
        /// Delete all periods that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedPeriods(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedPeriods(context);
        }

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Periods.</returns>
        public static List<WebPeriod> GetPeriods(WebServiceContext context)
        {
            List<WebPeriod> periods;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllPeriods";
            periods = (List<WebPeriod>)context.GetCachedObject(cacheKey);

            if (periods.IsNull())
            {
                // Get information from database.
                periods = new List<WebPeriod>();
                using (DataReader dataReader = DataServer.GetPeriods(context))
                {
                    while (dataReader.Read())
                    {
                        periods.Add(new WebPeriod(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, periods, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return periods;
        }

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Periods.</returns>
        public static List<WebPeriodType> GetPeriodTypes(WebServiceContext context)
        {
            List<WebPeriodType> periodTypes;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllPeriodTypes";
            periodTypes = (List<WebPeriodType>)context.GetCachedObject(cacheKey);

            if (periodTypes.IsNull())
            {
                // Get information from database.
                periodTypes = new List<WebPeriodType>();
                using (DataReader dataReader = DataServer.GetPeriodTypes(context))
                {
                    while (dataReader.Read())
                    {
                        periodTypes.Add(new WebPeriodType(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, periodTypes, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return periodTypes;
        }
    }
}
