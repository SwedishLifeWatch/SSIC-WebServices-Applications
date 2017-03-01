using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles region related information.
    /// </summary>
    public class RegionManager : IRegionManager
    {
        /// <summary>
        /// This interface is used to handle persistent data.
        /// </summary>
        public IRegionDataSource DataSource
        { get; set; }

        /// <summary>
        /// Coordinate system used when returning geometric data from this class.
        /// </summary>
        public ICoordinateSystem CoordinateSystem
        { get; private set; }

        /// <summary>
        /// Constructor method for this class.
        /// </summary>
        /// <param name="coordinateSystem">Coordinate system used when returning geometric data from this class.</param>
        public RegionManager(ICoordinateSystem coordinateSystem)
        {
            this.CoordinateSystem = coordinateSystem;
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get region categories.
        /// All region categories are returned if parameter countryIsoCode is not specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryIsoCode">
        /// Get region categories related to this country.
        /// Country iso codes are specified in standard ISO-3166.
        /// </param>
        /// <returns>Region categories.</returns>       
        public virtual RegionCategoryList GetRegionCategories(IUserContext userContext,
                                                              Int32? countryIsoCode)
        {
            return DataSource.GetRegionCategories(userContext, countryIsoCode);
        }

        /// <summary>
        /// Get region category with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategoryId">Region category id.</param>
        /// <returns>Region category with specified id.</returns>       
        public virtual IRegionCategory GetRegionCategory(IUserContext userContext,
                                                         Int32 regionCategoryId)
        {
            return GetRegionCategories(userContext, null).Get(regionCategoryId);
        }

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        public virtual RegionList GetRegionsByCategories(IUserContext userContext,
                                                         RegionCategoryList regionCategories)
        {
            return DataSource.GetRegionsByCategories(userContext, regionCategories);
        }

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategory">Get regions related to specified region category.</param>
        /// <returns>Regions related to specified region category.</returns>       
        public virtual RegionList GetRegionsByCategory(IUserContext userContext,
                                                       IRegionCategory regionCategory)
        {
            RegionCategoryList regionCategories;

            regionCategories = new RegionCategoryList();
            regionCategories.Add(regionCategory);
            return GetRegionsByCategories(userContext, regionCategories);
        }

        /// <summary>
        /// Gets the regions by list of GUID values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionGUIDs">List of region GUID values.</param>
        /// <returns>Return a list of regions matching the list of GUID values.</returns>
        public virtual RegionList GetRegionsByGUIDs(IUserContext userContext,
                                                  List<String> regionGUIDs)
        {
            return DataSource.GetRegionsByGUIDs(userContext, regionGUIDs);
        }

        /// <summary>
        /// Gets the regions by list of regionId values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionIds">List of region id values.</param>
        /// <returns>Return a list of regions matching the list of id values.</returns>
        public virtual RegionList GetRegionsByIds(IUserContext userContext,
                                                  List<Int32> regionIds)
        {
            return DataSource.GetRegionsByIds(userContext, regionIds);
        }

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>     
        public virtual RegionList GetRegionsBySearchCriteria(IUserContext userContext,
                                                             IRegionSearchCriteria searchCriteria)
        {
            return DataSource.GetRegionsBySearchCriteria(userContext, searchCriteria);
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Region types.</returns>
        public virtual RegionTypeList GetRegionTypes(IUserContext userContext)
        {
            return DataSource.GetRegionTypes(userContext);
        }
    }
}
