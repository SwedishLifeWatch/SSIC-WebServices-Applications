using System;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class contains handling of individual category related objects.
    /// </summary>
    public class IndividualCategoryManager : ManagerBase
    {
        private static IndividualCategoryList _individualCategories = null;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static IndividualCategoryManager()
        {
            ManagerBase.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Makes access to the private member _individualcategories thread safe.
        /// </summary>
        private static IndividualCategoryList IndividualCategories
        {
            get
            {
                IndividualCategoryList individualCategories;

                lock (_lockObject)
                {
                    individualCategories = _individualCategories;
                }
                return individualCategories;
            }
            set
            {
                lock (_lockObject)
                {
                    _individualCategories = value;
                }
            }
        }

        /// <summary>
        /// Get default individual category object.
        /// </summary>
        /// <returns>Default individual category.</returns>
        public static IndividualCategory GetDefaultIndividualCategory()
        {
            return GetIndividualCategory(IndividualCategoryId.Default);
        }

        /// <summary>
        /// Get the requested individual category object.
        /// </summary>
        /// <param name='individualcategoryId'>Id of requested individual category.</param>
        /// <returns>Requested individual category.</returns>
        /// <exception cref="ArgumentException">Thrown if no individual category has the requested id.</exception>
        public static IndividualCategory GetIndividualCategory(IndividualCategoryId individualcategoryId)
        {
            return GetIndividualCategory((Int32)individualcategoryId);
        }

        /// <summary>
        /// Get the requested individual category object.
        /// </summary>
        /// <param name='individualcategoryId'>Id of requested individual category.</param>
        /// <returns>Requested individual category.</returns>
        /// <exception cref="ArgumentException">Thrown if no individual category has the requested id.</exception>
        public static IndividualCategory GetIndividualCategory(Int32 individualcategoryId)
        {
            return GetIndividualCategories().Get(individualcategoryId);
        }

        /// <summary>
        /// Convert an Individual Category to a WebIndividualCategory.
        /// </summary>
        /// <param name="individualCategory">The Individua Category.</param>
        /// <returns>A WebIndividual Category.</returns>
        public static WebIndividualCategory GetIndividualCategory(IndividualCategory individualCategory)
        {
            WebIndividualCategory webIndividualCategory;

            webIndividualCategory = new WebIndividualCategory();
            webIndividualCategory.Id = individualCategory.Id;
            webIndividualCategory.Definition = individualCategory.Definition;
            webIndividualCategory.Name = individualCategory.Name;
            webIndividualCategory.SortOrder = individualCategory.SortOrder;

            return webIndividualCategory;
        }

        /// <summary>
        /// Get all individual category objects.
        /// </summary>
        /// <returns>All individual categories.</returns>
        public static IndividualCategoryList GetIndividualCategories()
        {
            IndividualCategoryList individualCategories = null;

            for (Int32 getAttempts = 0; (individualCategories.IsNull()) && (getAttempts < 3); getAttempts++)
            {
                LoadIndividualCategories();
                individualCategories = IndividualCategories;
            }
            return individualCategories;
        }

        /// <summary>
        /// Get individual categories from web service.
        /// </summary>
        private static void LoadIndividualCategories()
        {
            IndividualCategoryList individualCategories;

            if (IndividualCategories.IsNull())
            {
                // Get data from web service.
                individualCategories = new IndividualCategoryList(true);
                foreach (WebIndividualCategory webIndividualCategory in WebServiceClient.GetIndividualCategories())
                {
                    individualCategories.Add(new IndividualCategory(webIndividualCategory.Id,
                                                                    webIndividualCategory.Name,
                                                                    webIndividualCategory.Definition,
                                                                    webIndividualCategory.SortOrder));
                }
                IndividualCategories = individualCategories;
            }
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        private static void RefreshCache()
        {
            IndividualCategories = null;
        }
    }
}
