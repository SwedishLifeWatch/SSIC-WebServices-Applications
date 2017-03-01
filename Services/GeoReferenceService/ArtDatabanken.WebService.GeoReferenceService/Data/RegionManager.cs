using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.GeoReferenceService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// Manager of region related information.
    /// </summary>
    public class RegionManager
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static RegionManager()
        {
            GeographyCoordinateSystem = new WebCoordinateSystem();
            GeographyCoordinateSystem.Id = CoordinateSystemId.WGS84;
            RegionCoordinateSystem = new WebCoordinateSystem();
            RegionCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
            CityCoordinateSystem = new WebCoordinateSystem();
            CityCoordinateSystem.Id = CoordinateSystemId.Rt90_25_gon_v;
        }

        /// <summary>
        /// Coordinate system used for regions that
        /// are stored in the database.
        /// </summary>
        private static WebCoordinateSystem RegionCoordinateSystem { get; set; }

        /// <summary>
        /// Coordinate system used for geography copy of regions
        /// polygons that are stored in the database.
        /// </summary>
        private static WebCoordinateSystem GeographyCoordinateSystem { get; set; }

        /// <summary>
        /// Coordinate system used for cities stored in the database
        /// </summary>
        private static WebCoordinateSystem CityCoordinateSystem { get; set; }

        /// <summary>
        /// Check if user has access rights to region geography information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        private static void CheckAccessRights(WebServiceContext context)
        {
            List<WebRole> roles;

            roles = context.GetRoles();
            if (roles.IsNotEmpty())
            {
                foreach (WebRole role in roles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == "RegionGeography")
                            {
                                // Ok to access region geography.
                                return;
                            }
                        }
                    }
                }
            }

            // Not ok to access region geography.
            throw new ApplicationException("User:" +
                                            context.GetUser().UserName +
                                            " does not have access rights to region geography information in application:" +
                                            context.ClientToken.ApplicationIdentifier);
        }

        /// <summary>
        /// Gets cities by name string
        /// </summary>
        /// <param name="context"></param>
        /// <param name="searchCriteria"></param>
        /// <param name="coordinateSystem"></param>
        /// <returns></returns>
        public static List<WebCityInformation> GetCitiesByNameSearchString(WebServiceContext context, WebStringSearchCriteria searchCriteria, WebCoordinateSystem coordinateSystem)
        {
            // Check data
            coordinateSystem.CheckData();
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.SearchString = searchCriteria.SearchString.CheckInjection();

            // Get information from database
            var cityInformations = new List<WebCityInformation>();
            using (
                DataReader dataReader =
                    context.GetGeoReferenceDatabase().GetCitiesByNameSearchString(searchCriteria.SearchString))
            {
                while (dataReader.Read())
                {
                    WebCityInformation cityInformation = new WebCityInformation();
                    cityInformation.LoadData(dataReader);

                    if (
                        !WebCoordinateSystemExtension.GetWkt(CityCoordinateSystem)
                            .Equals(WebCoordinateSystemExtension.GetWkt(coordinateSystem)))
                    {
                        WebPoint fromPoint = new WebPoint(cityInformation.CoordinateX, cityInformation.CoordinateY);
                        
                        // Transform coordinates from RT90 to specified coordinate system
                        WebPoint point = WebServiceData.CoordinateConversionManager.GetConvertedPoint(fromPoint,
                                                                                                      CityCoordinateSystem,
                                                                                                      coordinateSystem);
                        cityInformation.CoordinateX = point.X;
                        cityInformation.CoordinateY = point.Y;
                    }
                    cityInformations.Add(cityInformation);
                }
            }
            return cityInformations;
        }

        /// <summary>
        /// Get all region categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All region categories.</returns>       
        private static List<WebRegionCategory> GetRegionCategories(WebServiceContext context)
        {
            List<WebRegionCategory> regionCategories;
            String cacheKey;
            WebRegionCategory regionCategory;

            // Get cached information.
            cacheKey = Settings.Default.RegionCategoriesCacheKey;
            regionCategories = (List<WebRegionCategory>) (context.GetCachedObject(cacheKey));

            if (regionCategories.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionCategories())
                {
                    regionCategories = new List<WebRegionCategory>();
                    while (dataReader.Read())
                    {
                        regionCategory = new WebRegionCategory();
                        regionCategory.LoadData(dataReader);
                        regionCategories.Add(regionCategory);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, regionCategories, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
            }

            return regionCategories;
        }

        /// <summary>
        /// Get region categories.
        /// All region categories are returned if parameter countryIsoCode is not specified.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="isCountryIsoCodeSpecified">Indicates if parameter countryIsoCode is specified.</param>
        /// <param name="countryIsoCode">
        /// Get region categories related to this country.
        /// Country iso codes are specified in standard ISO-3166.
        /// </param>
        /// <returns>Region categories.</returns>       
        public static List<WebRegionCategory> GetRegionCategories(WebServiceContext context,
                                                                  Boolean isCountryIsoCodeSpecified,
                                                                  Int32 countryIsoCode)
        {
            List<WebRegionCategory> allRegionCategories, regionCategories;

            allRegionCategories = GetRegionCategories(context);
            if (isCountryIsoCodeSpecified)
            {
                // Get region categories related to specified country.
                regionCategories = new List<WebRegionCategory>();
                foreach (WebRegionCategory regionCategory in allRegionCategories)
                {
                    if (regionCategory.IsCountryIsoCodeSpecified &&
                        (regionCategory.CountryIsoCode == countryIsoCode))
                    {
                        regionCategories.Add(regionCategory);
                    }
                }
            }
            else
            {
                regionCategories = allRegionCategories;
            }
            return regionCategories;
        }

        /// <summary>
        /// Get regions from an open DataReader.
        /// </summary>
        /// <param name="dataReader">An open DataReader.</param>
        /// <returns>Regions.</returns>
        private static List<WebRegion> GetRegions(DataReader dataReader)
        {
            WebRegion region;
            List<WebRegion> regions = new List<WebRegion>();

            while (dataReader.Read())
            {
                region = new WebRegion();
                region.LoadData(dataReader);
                regions.Add(region);
            }
            return regions;
        }

        /// <summary>
        /// Get regions related to specified region categories.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionCategories">Get regions related to specified region categories.</param>
        /// <returns>Regions related to specified region categories.</returns>
        public static List<WebRegion> GetRegionsByCategories(WebServiceContext context,
                                                             List<WebRegionCategory> regionCategories)
        {
            List<WebRegion> regions;

            if (regionCategories.IsEmpty())
            {
                regions = null;
            }
            else
            {
                regions = new List<WebRegion>();
                foreach (WebRegionCategory regionCategory in regionCategories)
                {
                    regions.AddRange(GetRegionsByCategory(context, regionCategory));
                }
            }

            return regions;
        }

        /// <summary>
        /// Get regions related to specified region category.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionCategory">Get regions related to specified region category.</param>
        /// <returns>Regions related to specified region categories.</returns>
        private static List<WebRegion> GetRegionsByCategory(WebServiceContext context,
                                                            WebRegionCategory regionCategory)
        {
            List<Int32> regionCategoryIds;
            List<WebRegion> regions;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.RegionsInCategoryCacheKey + ":" + regionCategory.Id;
            regions = (List<WebRegion>)(context.GetCachedObject(cacheKey));

            if (regions.IsNull())
            {
                // Get information from database.
                regionCategoryIds = new List<Int32>();
                regionCategoryIds.Add(regionCategory.Id);

                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsByCategories(regionCategoryIds))
                {
                    regions = GetRegions(dataReader);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, regions, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
            }

            return regions;
        }

        /// <summary>
        /// Get regions by GUIDs.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="guids">Region GUIDs.</param>
        /// <returns>Regions matching provided GUIDs.</returns>       
        public static List<WebRegion> GetRegionsByGuids(WebServiceContext context,
                                                        List<String> guids)
        {
            List<WebRegion> regions;

            regions = null;
            if (guids.IsNotEmpty())
            {
                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsByGUIDs(guids))
                {
                    regions = GetRegions(dataReader);
                }
            }

            return regions;
        }

        /// <summary>
        /// Get regions by ids.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <returns>Regions matching provided ids.</returns>       
        public static List<WebRegion> GetRegionsByIds(WebServiceContext context,
                                                      List<Int32> regionIds)
        {
            List<WebRegion> regions;

            regions = null;
            if (regionIds.IsNotEmpty())
            {
                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsByIds(regionIds))
                {
                    regions = GetRegions(dataReader);
                }
            }
            return regions;
        }

        /// <summary>
        /// Get regions that matches the search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Regions that matches the search criteria</returns>
        public static List<WebRegion> GetRegionsBySearchCriteria(WebServiceContext context,
                                                                 WebRegionSearchCriteria searchCriteria)
        {
            Int32? typeId;
            List<Int32> categoryIds;
            String nameSearchString;

            // Check data.
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckStrings();

            // Region category.
            categoryIds = new List<Int32>();
            if (searchCriteria.Categories.IsNotNull())
            {
                foreach (WebRegionCategory regionCategory in searchCriteria.Categories)
                {
                    categoryIds.Add(regionCategory.Id);
                }
            }
            // Region name search string.
            nameSearchString = null;
            if (searchCriteria.NameSearchString.IsNotNull())
            {
                nameSearchString = searchCriteria.NameSearchString.SearchString.CheckInjection();
            }

            // Region type.
            typeId = null;
            if (searchCriteria.Type.IsNotNull())
            {
                typeId = searchCriteria.Type.Id;
            }

            // Get information from database.
            using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsBySearchCriteria(nameSearchString,
                                                                                                        typeId,
                                                                                                        categoryIds,
                                                                                                        searchCriteria.CountryIsoCodes))
            {
                return GetRegions(dataReader);
            }
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="guids">Region guids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public static List<WebRegionGeography> GetRegionsGeographyByGuids(WebServiceContext context,
                                                                          List<String> guids,
                                                                          WebCoordinateSystem coordinateSystem)
        {
            List<WebRegionGeography> regionsGeography;
            WebRegionGeography regionGeography;

            // Check access rights.
            CheckAccessRights(context);

            // Check data.
            coordinateSystem.CheckData();

            // Get information from database.
            regionsGeography = null;
            if (guids.IsNotEmpty())
            {
                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsGeographyByGUIDs(guids))
                {
                    regionsGeography = new List<WebRegionGeography>();
                    while (dataReader.Read())
                    {
                        regionGeography = new WebRegionGeography();
                        regionGeography.LoadData(dataReader);
                        regionGeography = WebServiceData.CoordinateConversionManager.GetConvertedRegionGeography(regionGeography,
                                                                                                                 RegionCoordinateSystem,
                                                                                                                 coordinateSystem);
                        regionsGeography.Add(regionGeography);
                    }
                }
            }

            return regionsGeography;
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionIds">Region ids.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public static List<WebRegionGeography> GetRegionsGeographyByIds(WebServiceContext context,
                                                                        List<Int32> regionIds,
                                                                        WebCoordinateSystem coordinateSystem)
        {
            List<WebRegionGeography> regionsGeography;
            WebRegionGeography regionGeography;

            // Check access rights.
            CheckAccessRights(context);

            // Check data.
            coordinateSystem.CheckData();

            // Get information from database.
            regionsGeography = new List<WebRegionGeography>();
            using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionsGeographyByIds(regionIds))
            {
                while (dataReader.Read())
                {
                    regionGeography = new WebRegionGeography();
                    regionGeography.LoadData(dataReader);
                    regionGeography = WebServiceData.CoordinateConversionManager.GetConvertedRegionGeography(regionGeography,
                                                                                                             RegionCoordinateSystem,
                                                                                                             coordinateSystem);
                    regionsGeography.Add(regionGeography);
                }
            }
            return regionsGeography;
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All region types.</returns>
        public static List<WebRegionType> GetRegionTypes(WebServiceContext context)
        {
            List<WebRegionType> regionTypes;
            String cacheKey;
            WebRegionType regionType;

            // Get cached information.
            cacheKey = Settings.Default.RegionTypesCacheKey;
            regionTypes = (List<WebRegionType>) (context.GetCachedObject(cacheKey));

            if (regionTypes.IsNull())
            {
                // Get information from database.
                using (DataReader dataReader = context.GetGeoReferenceDatabase().GetRegionTypes())
                {
                    regionTypes = new List<WebRegionType>();
                    while (dataReader.Read())
                    {
                        regionType = new WebRegionType();
                        regionType.LoadData(dataReader);
                        regionTypes.Add(regionType);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey, regionTypes, DateTime.Now + new TimeSpan(1, 0, 0, 0), CacheItemPriority.High);
            }

            return regionTypes;
        }
        
        /// <summary>
        /// Update GeoReferenceService with region information from Artportalen.
        /// </summary>
        public static void UpdateRegionInformationFromArtportalen(WebServiceContext context)
        {
            WebCoordinateSystem artportalenCoordinateSystem = new WebCoordinateSystem();
            artportalenCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;

            DataTable regionTable = new DataTable();
            regionTable.TableName = "TempArea";
            regionTable.Columns.Add("Id", typeof(int));
            regionTable.Columns.Add("AreaDatasetId", typeof(int));
            regionTable.Columns.Add("FeatureId", typeof(string));
            regionTable.Columns.Add("Name", typeof(string));
            regionTable.Columns.Add("ShortName", typeof(string));
            regionTable.Columns.Add("PolygonGeometry", typeof(SqlGeometry));
            regionTable.Columns.Add("Bbox", typeof(string));
            regionTable.Columns.Add("AreaDatasetSubTypeId", typeof(int));
            regionTable.Columns.Add("AttributesXml", typeof(string));
            regionTable.Columns.Add("PolygonGeography", typeof(SqlGeography));

            DataTable regionCategoryTable = new DataTable();
            regionCategoryTable.TableName = "TempAreaDataset";
            regionCategoryTable.Columns.Add("Id", typeof(int));
            regionCategoryTable.Columns.Add("CountryIsoCode", typeof(int));
            regionCategoryTable.Columns.Add("Name", typeof(string));
            regionCategoryTable.Columns.Add("IsRequired", typeof(bool));
            regionCategoryTable.Columns.Add("IsIndirect", typeof(bool));
            regionCategoryTable.Columns.Add("AllowOverrideInderectType", typeof(bool));
            regionCategoryTable.Columns.Add("AreaDatasetCategoryId", typeof(int));
            regionCategoryTable.Columns.Add("SortOrder", typeof(int));
            regionCategoryTable.Columns.Add("AttributesTohtmlXslt", typeof(string));
            regionCategoryTable.Columns.Add("AreaLevel", typeof(int));
            regionCategoryTable.Columns.Add("HasStatistics", typeof(bool));
            regionCategoryTable.Columns.Add("IsValidationArea", typeof(bool));

            DataTable regionTypeTable = new DataTable();
            regionTypeTable.TableName = "TempAreaDatasetCategory";
            regionTypeTable.Columns.Add("Id", typeof(int));
            regionTypeTable.Columns.Add("Name", typeof(string));

            using (ArtportalenServer artportalenServer = new ArtportalenServer())
            {
                int regionCount = 0;

                using (DataReader dataReader = artportalenServer.GetRegionInformation())
                {
                    var contextSpeciesObservationDatabase = context.GetSpeciesObservationDatabase();
                    contextSpeciesObservationDatabase.EmptyTempAreaTables();
                    Debug.WriteLine(DateTime.Now.WebToString() + " Data in temp area tables deleted");

                    while (dataReader.Read())
                    {
                        WebRegionGeometry webRegionGeometry = new WebRegionGeometry();

                        webRegionGeometry.LoadData(dataReader);
                        if (artportalenCoordinateSystem.GetWkt() != RegionCoordinateSystem.GetWkt())
                        {
                            webRegionGeometry.Polygon = WebServiceData.CoordinateConversionManager.GetConvertedMultiPolygon(webRegionGeometry.Polygon,
                                                                                                                            artportalenCoordinateSystem,
                                                                                                                            RegionCoordinateSystem);
                            webRegionGeometry.BoundingBox = webRegionGeometry.Polygon.GetBoundingBox();
                        }
                        if (artportalenCoordinateSystem.GetWkt() != GeographyCoordinateSystem.GetWkt())
                        {
                            webRegionGeometry.PolygonGeography = WebServiceData.CoordinateConversionManager.GetConvertedMultiPolygon(webRegionGeometry.Polygon,
                                                                                                                                     artportalenCoordinateSystem,
                                                                                                                                     GeographyCoordinateSystem);
                        }

                        DataRow areaRow = regionTable.NewRow();
                        areaRow[0] = webRegionGeometry.Id;
                        areaRow[1] = webRegionGeometry.AreaDatasetId;
                        areaRow[2] = webRegionGeometry.FeatureId;
                        areaRow[3] = webRegionGeometry.Name;
                        areaRow[4] = webRegionGeometry.ShortName;
                        areaRow[5] = webRegionGeometry.Polygon.GetGeometry();
                        if (areaRow[5].ToString() == "Null")
                        {
                            Debug.WriteLine(DateTime.Now.WebToString() + " Null in GetGeometry for " + webRegionGeometry.Id);
                            continue;
                        }
                        const string delimiter = ",";
                        areaRow[6] = webRegionGeometry.BoundingBox.Min.X.WebToStringR() + delimiter + webRegionGeometry.BoundingBox.Min.Y.WebToStringR() + delimiter
                                   + webRegionGeometry.BoundingBox.Max.X.WebToStringR() + delimiter + webRegionGeometry.BoundingBox.Max.Y.WebToStringR();
                        areaRow[7] = webRegionGeometry.AreaDatasetSubTypeId;
                        areaRow[8] = webRegionGeometry.AttributesXml;
                        areaRow[9] = webRegionGeometry.PolygonGeography.GetGeography();
                        if (areaRow[9].ToString() == "Null")
                        {
                            Debug.WriteLine(DateTime.Now.WebToString() + " Null in GetGeography for " + webRegionGeometry.Id);
                            continue;
                        }
                        regionTable.Rows.Add(areaRow);
                        Debug.WriteLineIf(decimal.Remainder(++regionCount, 100) == 0, DateTime.Now.WebToString() + " AreaDataRow: " + regionCount);
                        if (decimal.Remainder(regionCount, 1000) == 0)
                        {
                            contextSpeciesObservationDatabase.AddTableData(regionTable);
                            Debug.WriteLine(DateTime.Now.WebToString() + " Area rows added to TempArea");
                            regionTable.Rows.Clear();
                        }
                    }

                    contextSpeciesObservationDatabase.AddTableData(regionTable);
                    Debug.WriteLine(DateTime.Now.WebToString() + " Area rows added to TempArea");

                    dataReader.NextResultSet();
                    while (dataReader.Read())
                    {
                        AreaDataset areaDataset = new AreaDataset();
                        areaDataset.LoadData(dataReader);

                        DataRow areaDatasetRow = regionCategoryTable.NewRow();
                        areaDatasetRow[0] = areaDataset.Id;
                        areaDatasetRow[1] = areaDataset.CountryIsoCode;
                        areaDatasetRow[2] = areaDataset.Name;

                        areaDatasetRow[3] = areaDataset.IsRequired;
                        areaDatasetRow[4] = areaDataset.IsIndirect;
                        areaDatasetRow[5] = areaDataset.AllowOverrideIndirectType;
                        areaDatasetRow[6] = areaDataset.AreaDatasetCategoryId;
                        areaDatasetRow[7] = areaDataset.SortOrder;
                        areaDatasetRow[8] = areaDataset.AttributesToHtmlXslt;
                        areaDatasetRow[9] = areaDataset.AreaLevel;
                        areaDatasetRow[10] = areaDataset.HasStatistics;
                        areaDatasetRow[11] = areaDataset.IsValidationArea;

                        regionCategoryTable.Rows.Add(areaDatasetRow);
                    }

                    contextSpeciesObservationDatabase.AddTableData(regionCategoryTable);
                    Debug.WriteLine(DateTime.Now.WebToString() + " AreaDataset rows added to TempAreaDataset");

                    dataReader.NextResultSet();

                    while (dataReader.Read())
                    {
                        AreaDatasetCategory areaDatasetCategory = new AreaDatasetCategory();
                        areaDatasetCategory.LoadData(dataReader);

                        DataRow areaDatasetCategoryRow = regionTypeTable.NewRow();
                        areaDatasetCategoryRow[0] = areaDatasetCategory.Id;
                        areaDatasetCategoryRow[1] = areaDatasetCategory.Name;

                        regionTypeTable.Rows.Add(areaDatasetCategoryRow);
                    }

                    contextSpeciesObservationDatabase.AddTableData(regionTypeTable);
                    Debug.WriteLine(DateTime.Now.WebToString() + " AreaDatasetCategory rows added to TempAreaDatasetCategory");

                    // Move from temporary tables to production tables.
                    contextSpeciesObservationDatabase.MoveTempToArea();
                    Debug.WriteLine(DateTime.Now.WebToString() + " Area data moved from temp to area tables");
                }
            }
        }
    }
}
