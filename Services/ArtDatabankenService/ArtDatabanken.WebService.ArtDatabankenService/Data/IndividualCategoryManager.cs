using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Caching;
using ArtDatabanken.WebService.ArtDatabankenService;
using ArtDatabanken.WebService.ArtDatabankenService.Database;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Individual Category Manager
    /// </summary>
    public class IndividualCategoryManager
    {
        /// <summary>
        /// Inserts a list of individual category ids into the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="individualCategoryIds">Id for individual categories to insert.</param>
        /// <param name="individualCategoryUsage">How user selected individual categories should be used.</param>
        public static void AddUserSelectedIndividualCategories(WebServiceContext context,
                                                               List<Int32> individualCategoryIds,
                                                               UserSelectedIndividualCategoryUsage individualCategoryUsage)
        {
            DataColumn column;
            DataRow row;
            DataTable individualCategoryTable;

            if (individualCategoryIds.IsNotEmpty())
            {
                // Delete all individual categories that belong to this request from the "temporary" tables.
                // This is done to avoid problem with restarts of the webservice.
                DeleteUserSelectedIndividualCategories(context);

                // Insert the new list of individual categories.
                individualCategoryTable = new DataTable(UserSelectedIndividualCategoryData.TABLE_NAME);
                column = new DataColumn(UserSelectedIndividualCategoryData.REQUEST_ID, typeof(Int32));
                individualCategoryTable.Columns.Add(column);
                column = new DataColumn(UserSelectedIndividualCategoryData.INDIVIDUAL_CATEGORY_ID, typeof(Int32));
                individualCategoryTable.Columns.Add(column);
                column = new DataColumn(UserSelectedIndividualCategoryData.INDIVIDUAL_CATEGORY_USAGE, typeof(String));
                individualCategoryTable.Columns.Add(column);
                foreach (Int32 individualCategoryId in individualCategoryIds)
                {
                    row = individualCategoryTable.NewRow();
                    row[0] = context.RequestId;
                    row[1] = individualCategoryId;
                    row[2] = individualCategoryUsage.ToString();
                    individualCategoryTable.Rows.Add(row);
                }
                DataServer.AddUserSelectedIndividualCategories(context, individualCategoryTable);
            }
        }

        /// <summary>
        /// Delete all individual categories that belong to this request from the database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        public static void DeleteUserSelectedIndividualCategories(WebServiceContext context)
        {
            DataServer.DeleteUserSelectedIndividualCategories(context);
        }

        /// <summary>
        /// Get all Individual Categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Individual Categories.</returns>
        public static List<WebIndividualCategory> GetIndividualCategories(WebServiceContext context)
        {
            List<WebIndividualCategory> individualcategories;
            String cacheKey;

            // Get cached information.
            cacheKey = "AllIndividualCategories";
            individualcategories = (List<WebIndividualCategory>)context.GetCachedObject(cacheKey);

            if (individualcategories.IsNull())
            {
                // Get information from database.
                individualcategories = new List<WebIndividualCategory>();
                using (DataReader dataReader = DataServer.GetIndividualCategories(context))
                {
                    while (dataReader.Read())
                    {
                        individualcategories.Add(new WebIndividualCategory(dataReader));

                    }

                    // Add information to cache.
                    context.AddCachedObject(cacheKey, individualcategories, DateTime.Now + new TimeSpan(12, 0, 0), CacheItemPriority.AboveNormal);
                }
            }
            return individualcategories;
        }
    }
}
