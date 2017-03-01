using System;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Client.GeoReferenceService
{
    /// <summary>
    /// This class is used to retrieve region related information.
    /// </summary>
    public class RegionDataSource : GeoReferenceDataSourceBase, IRegionDataSource
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
        public RegionCategoryList GetRegionCategories(IUserContext userContext,
                                                      Int32? countryIsoCode)
        {
            List<WebRegionCategory> webRegionCategories;
            Boolean isCountryIsoCodeSpecified = false;
            Int32 _countryIsoCode = Int32.MinValue; 
            if (countryIsoCode.HasValue)
            {
                _countryIsoCode = countryIsoCode.Value;
                isCountryIsoCodeSpecified = true;
            }

            webRegionCategories = WebServiceProxy.GeoReferenceService.GetRegionCategories(GetClientInformation(userContext), 
                                                                                          isCountryIsoCodeSpecified,
                                                                                          _countryIsoCode);
            return GetRegionCategories(userContext, webRegionCategories);
        }

        /// <summary>
        /// Get region categories from list of WebRegionCategories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegionCategories">List of WebRegionCategories.</param>
        /// <returns>RegionCategoryList.</returns>
        protected RegionCategoryList GetRegionCategories(IUserContext userContext,
                                                         List<WebRegionCategory> webRegionCategories)
        {
            RegionCategoryList regionCategories;

            regionCategories = new RegionCategoryList();
            foreach (WebRegionCategory webRegionCategory in webRegionCategories)
            {
                regionCategories.Add(GetRegionCategory(userContext, webRegionCategory));
            }
            return regionCategories;
        }

        /// <summary>
        /// Get web region categories from region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategories">RegionCategories.</param>
        /// <returns>List of WebRegionCategory objects.</returns>
        protected List<WebRegionCategory> GetRegionCategories(IUserContext userContext,
                                                              RegionCategoryList regionCategories)
        {
            List<WebRegionCategory> webRegionCategories;

            webRegionCategories = null;
            if (regionCategories.IsNotEmpty())
            {
                webRegionCategories = new List<WebRegionCategory>();
                foreach (IRegionCategory regionCategory in regionCategories)
                {
                    webRegionCategories.Add(GetRegionCategory(userContext, regionCategory));
                }
            }
            return webRegionCategories;
        }

        /// <summary>
        /// Get region category from web region category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegionCategory">Web region category.</param>
        /// <returns>Region category.</returns>
        protected IRegionCategory GetRegionCategory(IUserContext userContext, WebRegionCategory webRegionCategory)
        {
            return new RegionCategory(webRegionCategory.Id,
                                      webRegionCategory.CountryIsoCode,
                                      webRegionCategory.GUID,
                                      webRegionCategory.Level,
                                      webRegionCategory.Name,
                                      webRegionCategory.NativeIdSource,
                                      webRegionCategory.SortOrder,
                                      webRegionCategory.TypeId,
                                      GetDataContext(userContext));
        }

        /// <summary>
        /// Get WebRegionCategory from RegionCategory
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategory">RegionCategory</param>
        /// <returns>WebRegionCategory.</returns>
        private WebRegionCategory GetRegionCategory(IUserContext userContext,
                                                    IRegionCategory regionCategory)
        {
            WebRegionCategory webRegionCategory;
            webRegionCategory = new WebRegionCategory();
            if (regionCategory.CountryIsoCode.HasValue)
            {
                webRegionCategory.CountryIsoCode = regionCategory.CountryIsoCode.Value;
            }
            webRegionCategory.GUID = regionCategory.GUID;
            webRegionCategory.Id = regionCategory.Id;
            
            webRegionCategory.IsCountryIsoCodeSpecified = regionCategory.CountryIsoCode.HasValue;
            webRegionCategory.IsLevelSpecified = regionCategory.Level.HasValue;
            if (regionCategory.Level.HasValue)
            {
                webRegionCategory.Level = regionCategory.Level.Value;
            }
            webRegionCategory.Name = regionCategory.Name;
            webRegionCategory.NativeIdSource = regionCategory.NativeIdSource;
            webRegionCategory.SortOrder = regionCategory.SortOrder;
            webRegionCategory.TypeId = regionCategory.TypeId;
            return webRegionCategory;
        }
                
        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>       
        public RegionList GetRegionsByCategories(IUserContext userContext,
                                                 RegionCategoryList regionCategories)
        {
            List<WebRegion> webRegions;
            WebCoordinateSystem webCoordinateSystem = new WebCoordinateSystem();
            List<WebRegionCategory> webRegionCategories = new List<WebRegionCategory>();
            webRegionCategories = GetRegionCategories(userContext, regionCategories);
            webRegions = WebServiceProxy.GeoReferenceService.GetRegionsByCategories(GetClientInformation(userContext),
                                                                                    webRegionCategories);
            return GetRegions(userContext, webRegions);
        }

        /// <summary>
        /// Gets the regions by list of GUID values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionGUIDs">List of region GUID values.</param>
        /// <returns>Return a list of regions matching the list of GUID values.</returns>
        public RegionList GetRegionsByGUIDs(IUserContext userContext,
                                          List<String> regionGUIDs)
        {
            List<WebRegion> webRegions;
            WebCoordinateSystem webCoordinateSystem = new WebCoordinateSystem();
            webRegions = WebServiceProxy.GeoReferenceService.GetRegionsByGUIDs(GetClientInformation(userContext),
                                                                             regionGUIDs);
            return GetRegions(userContext, webRegions);
        }

        /// <summary>
        /// Gets the regions by list of regionId values.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionIds">List of region id values.</param>
        /// <returns>Return a list of regions matching the list of id values.</returns>
        public RegionList GetRegionsByIds(IUserContext userContext,
                                          List<Int32> regionIds)
        {
            List<WebRegion> webRegions;
            WebCoordinateSystem webCoordinateSystem = new WebCoordinateSystem();
            webRegions = WebServiceProxy.GeoReferenceService.GetRegionsByIds(GetClientInformation(userContext),
                                                                             regionIds);
            return GetRegions(userContext, webRegions);
        }

        /// <summary>
        /// Get regions that matches the search criterias.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Region search criterias.</param>
        /// <returns>Regions that matches the search criterias.</returns>     
        public RegionList GetRegionsBySearchCriteria(IUserContext userContext,
                                                     IRegionSearchCriteria searchCriteria)
        {
            List<WebRegion> webRegions;
            WebRegionSearchCriteria webRegionSearchCriteria = new WebRegionSearchCriteria();
            webRegionSearchCriteria = GetSearchCriteria(userContext, searchCriteria);
            WebCoordinateSystem webCoordinateSystem = new WebCoordinateSystem();
            webRegions = WebServiceProxy.GeoReferenceService.GetRegionsBySearchCriteria(GetClientInformation(userContext),
                                                                                        webRegionSearchCriteria);
            return GetRegions(userContext, webRegions);

        }

        /// <summary>
        /// Get region type from web region type.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegionType">Web region type.</param>
        /// <returns>Region type.</returns>
        protected IRegionType GetRegionType(IUserContext userContext,
                                            WebRegionType webRegionType)
        {
            return new RegionType(webRegionType.Id,
                                  webRegionType.Name,
                                  GetDataContext(userContext));
        }

        /// <summary>
        /// Get WebRegionType from RegionType
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="regionType">RegionType</param>
        /// <returns>WebRegionType.</returns>
        protected WebRegionType GetRegionType(IUserContext userContext,
                                              IRegionType regionType)
        {
            WebRegionType webRegionType = new WebRegionType();
            webRegionType.Id = regionType.Id;
            webRegionType.Name = regionType.Name;
            return webRegionType;
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All countries.</returns>
        public RegionTypeList GetRegionTypes(IUserContext userContext)
        {
            List<WebRegionType> webRegionTypes;

            webRegionTypes = WebServiceProxy.GeoReferenceService.GetRegionTypes(GetClientInformation(userContext));
            return GetRegionTypes(userContext, webRegionTypes);
        }

        /// <summary>
        /// Get region types from web region types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRegionTypes">Web region types.</param>
        /// <returns>Region types.</returns>
        private RegionTypeList GetRegionTypes(IUserContext userContext,
                                              List<WebRegionType> webRegionTypes)
        {
            RegionTypeList regionTypes;

            regionTypes = new RegionTypeList();
            foreach (WebRegionType webRegionType in webRegionTypes)
            {
                regionTypes.Add(GetRegionType(userContext, webRegionType));
            }
            return regionTypes;
        }

        /// <summary>
        /// Get web search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebRegionSearchCriteria GetSearchCriteria(IUserContext userContext,
                                                          IRegionSearchCriteria searchCriteria)
        {
            WebRegionSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebRegionSearchCriteria();
            List<WebRegionCategory> webRegionCategories;

            webRegionCategories = null;
            if (searchCriteria.Categories.IsNotEmpty())
            {
                webRegionCategories = new List<WebRegionCategory>();
                foreach (IRegionCategory regionCategory in searchCriteria.Categories)
                {
                    webRegionCategories.Add(GetRegionCategory(userContext, regionCategory));
                }
            }
            webSearchCriteria.Categories = webRegionCategories;
            webSearchCriteria.CountryIsoCodes = searchCriteria.CountryIsoCodes;
            if (searchCriteria.NameSearchString.IsNotNull())
            {
                webSearchCriteria.NameSearchString = GetStringSearchCriteria(userContext, searchCriteria.NameSearchString);
            }
            if (searchCriteria.Type.IsNotNull())
            {
                webSearchCriteria.Type = GetRegionType(userContext, searchCriteria.Type);
            }

            return webSearchCriteria;
        }

        private WebStringSearchCriteria GetStringSearchCriteria (IUserContext userContext, 
                                                                 StringSearchCriteria stringSearchCriteria)
        {
            WebStringSearchCriteria webStringSearchCriteria = new WebStringSearchCriteria();
            webStringSearchCriteria.CompareOperators = stringSearchCriteria.CompareOperators;
            webStringSearchCriteria.SearchString = stringSearchCriteria.SearchString;
            return webStringSearchCriteria;
        }
    }
}
