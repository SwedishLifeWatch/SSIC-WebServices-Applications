using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Helper class for RedList application
    /// </summary>
    public class RedListedHelper
    {
        /// <summary>
        /// Checks if a redlisted category is between (or equal) to DD and NT
        /// </summary>
        /// <param name="redListCategory">The category</param>
        /// <returns>True if the category is in the interval, else false</returns>
        public static bool IsRedListedDdToNt(int? redListCategory)
        {
            return redListCategory >= (int)RedListCategory.DD &&
                    redListCategory <= (int)RedListCategory.NT;
        }

        /// <summary>
        /// Checks if a redlisted category is between (or equal) to DD and NE
        /// </summary>
        /// <param name="redListCategory">The category</param>
        /// <returns>True if the category is in the interval, else false</returns>
        public static bool IsRedListedDdToNe(int? redListCategory)
        {
            return redListCategory >= (int)RedListCategory.DD &&
                     redListCategory <= (int)RedListCategory.NE;
        }

        /// <summary>
        /// Get all categories between DD and NT
        /// </summary>
        /// <returns></returns>
        public static List<RedListCategory> GetRedListCategoriesDdToNt()
        {
            var redListCategories = new List<RedListCategory>();

            for (var category = RedListCategory.DD; category <= RedListCategory.NT; category++)
            {
                redListCategories.Add(category);
            }

            return redListCategories;
        }

        /// <summary>
        /// Get all categories between DD and NT (as a list of ints)
        /// </summary>
        /// <returns></returns>
        public static List<int> GetRedListCategoriesDdToNtAsIntList()
        {
            var redListCategories = new List<int>();

            for (var category = RedListCategory.DD; category <= RedListCategory.NT; category++)
            {
                redListCategories.Add((int)category);
            }

            return redListCategories;
        }

        /// <summary>
        /// Get all categories between DD and NE
        /// </summary>
        /// <returns></returns>
        public static IList<RedListCategory> GetRedListCategoriesDdToNe()
        {
            IList<RedListCategory> redListCategories = new List<RedListCategory>();

            for (var category = RedListCategory.DD; category <= RedListCategory.NE; category++)
            {
                redListCategories.Add(category);
            }

            return redListCategories;
        }

        /// <summary>
        /// Get all categories between DD and NE (as a list of ints)
        /// </summary>
        /// <returns></returns>
        public static List<int> GetRedListCategoriesDdToNeAsIntList()
        {
            var redListCategories = new List<int>();

            for (var category = RedListCategory.DD; category <= RedListCategory.NE; category++)
            {
                redListCategories.Add((int)category);
            }

            return redListCategories;
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns></returns>
        public static IList<RedListCategory> GetAllRedListCategories()
        {
            IList<RedListCategory> redListCategories = new List<RedListCategory>();

            for (var category = RedListCategory.EX; category <= RedListCategory.NE; category++)
            {
                redListCategories.Add(category);
            }

            return redListCategories;
        }
    }
}
