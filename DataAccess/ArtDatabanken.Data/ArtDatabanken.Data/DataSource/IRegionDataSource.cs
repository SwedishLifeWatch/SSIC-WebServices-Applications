using System;
using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the RegionDataSource interface.
    /// This interface is used to retrieve region related information.
    /// </summary>
    public interface IRegionDataSource : IDataSource
    {
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
        RegionCategoryList GetRegionCategories(IUserContext userContext,
                                               Int32? countryIsoCode);

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        RegionList GetRegionsByCategories(IUserContext userContext,
                                          RegionCategoryList regionCategories);

        /// <summary>
        /// Gets the regions by list of GUID values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionGUIDs">List of region GUID values.</param>
        /// <returns>Return a list of regions matching the list of GUID values.</returns>
        RegionList GetRegionsByGUIDs(IUserContext userContext,
                                   List<String> regionGUIDs);

        /// <summary>
        /// Gets the regions by list of regionId values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionIds">List of region id values.</param>
        /// <returns>Return a list of regions matching the list of id values.</returns>
        RegionList GetRegionsByIds(IUserContext userContext,
                                   List<Int32> regionIds);

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>       
        RegionList GetRegionsBySearchCriteria(IUserContext userContext,
                                              IRegionSearchCriteria searchCriteria);

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Region types.</returns>
        RegionTypeList GetRegionTypes(IUserContext userContext);
    }
}
