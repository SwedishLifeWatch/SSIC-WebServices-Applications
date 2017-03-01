using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to the
    /// WebSpeciesObservationSearchCriteria class.
    /// </summary>
    public static class WebSpeciesObservationSearchCriteriaExtension
    {
        /// <summary>
        /// Prepare for new where condition part.
        /// </summary>
        /// <param name="stringBuilder">Where condition.</param>
        private static void AddWhereCondition(StringBuilder stringBuilder)
        {
            if (stringBuilder.ToString().IsNotEmpty())
            {
                stringBuilder.Append(" AND ");
            }
        }

        /// <summary>
        /// Append where conditions to the specified StringBuilder for each WebDataField
        /// </summary>
        /// <param name="webDataFields">WebDataField list to append where conditions for</param>
        /// <param name="fieldName">The field name to use</param>
        /// <param name="whereCondition">The where condition</param>
        /// <param name="newWhereConditionPart">Tells if this is a new where condition part (AND) or if it should be appended (OR)</param>
        private static void AppendConditionForWebDataFields(List<WebDataField> webDataFields, string fieldName, StringBuilder whereCondition, bool appendToWhereCondition)
        {
            if (webDataFields.Any())
            {
                if (appendToWhereCondition)
                {
                    whereCondition.Append(" OR ");
                }
                else
                {
                    AddWhereCondition(whereCondition);
                }

                if (webDataFields.Count == 1)
                {
                    whereCondition.AppendFormat(" (" + fieldName + " = " + webDataFields[0].Value + " ) ");
                }
                else
                {
                    whereCondition.Append(" (" + fieldName + " IN (" + webDataFields[0].Value);

                    for (int index = 1; index < webDataFields.Count; index++)
                    {
                        whereCondition.Append(", " + webDataFields[index].Value);
                    }

                    whereCondition.Append("))");
                }
            }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="isElasticsearchUsed">
        /// Indicates if Elsticsearch is used.
        /// Some handling of search criteria differs depending
        /// on which data source that will be used.
        /// </param>
        /// <param name="mapping">Information about fields in Elasticsearch.</param>
        /// <param name="useRegionOptimization">Indicates if optimization of county and province regions should be used.</param>
        public static void CheckData(this WebSpeciesObservationSearchCriteria searchCriteria,
                                     WebServiceContext context,
                                     Boolean isElasticsearchUsed = false,
                                     Dictionary<String, WebSpeciesObservationField> mapping = null,
                                     Boolean useRegionOptimization = true)
        {
            Boolean isBirdNestActivityFound, isRegionFound,
                    isSpeciesActivityIdFound;
            Int32 index;
            List<WebRegion> regions;
            List<WebRole> currentRoles;
            List<WebSpeciesActivity> speciesActivities;

            // Check search criteria Accuracy.
            if (searchCriteria.IsAccuracySpecified)
            {
                // Accuracy must be zero or larger.
                if (searchCriteria.Accuracy < 0)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: Accuray must be zero or larger. Current value = " + searchCriteria.Accuracy);
                }
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            // Check search criteria BirdNestActivityLimit.
            if (searchCriteria.IsBirdNestActivityLimitSpecified)
            {
                // Bird nest activity limit must match id
                // of one of the bird nest activities.
                isBirdNestActivityFound = false;
                foreach (WebSpeciesActivity birdNestActivity in WebServiceData.SpeciesActivityManager.GetBirdNestActivities(context))
                {
                    if (birdNestActivity.Id == searchCriteria.BirdNestActivityLimit)
                    {
                        isBirdNestActivityFound = true;
                        break;
                    }
                }

                if (!isBirdNestActivityFound)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: Invalid BirdNestActivityLimit. Current value = " + searchCriteria.BirdNestActivityLimit);
                }
            }
#endif

            // Check search criteria BoundingBox.
            searchCriteria.BoundingBox.CheckData();
            if (searchCriteria.BoundingBox.IsNotNull())
            {
                // Convert bounding box into a polygon.
                if (searchCriteria.Polygons.IsEmpty())
                {
                    searchCriteria.Polygons = new List<WebPolygon>();
                }

                searchCriteria.Polygons.Add(searchCriteria.BoundingBox.GetPolygon());
            }

            // Check search criteria ChangeDateTime.
            searchCriteria.ChangeDateTime.CheckData(false);

#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.DataSourceGuids.IsNotEmpty())
            {
                for (index = 0; index < searchCriteria.DataSourceGuids.Count; index++)
                {
                    searchCriteria.DataSourceGuids[index].CheckNotEmpty("DataSourceGuid " + index);
                    searchCriteria.DataSourceGuids[index] = searchCriteria.DataSourceGuids[index].CheckInjection();
                }
            }
#else
            // Check search criteria DataProviderGuids.
            if (searchCriteria.DataProviderGuids.IsNotEmpty())
            {
                for (index = 0; index < searchCriteria.DataProviderGuids.Count; index++)
                {
                    searchCriteria.DataProviderGuids[index].CheckNotEmpty("DataSourceGuid " + index);
                    searchCriteria.DataProviderGuids[index] = searchCriteria.DataProviderGuids[index].CheckInjection();
                }

                // Check that all data provider guids are valid.
                foreach (String dataProviderGuid in searchCriteria.DataProviderGuids)
                {
                    if (WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProvider(context,
                                                                                                   dataProviderGuid).IsNull())
                    {
                        throw new ArgumentException("Unknown species observation data provider GUID = " + dataProviderGuid);
                    }
                }
            }
#endif

            // Check search criteria FieldLogicalOperator.
            if (searchCriteria.FieldSearchCriteria.IsNotEmpty() &&
                (searchCriteria.FieldSearchCriteria.Count > 1))
            {
                switch (searchCriteria.GetFieldLogicalOperator())
                {
                    case LogicalOperator.And:
                    case LogicalOperator.Or:
                        break;
                    default:
                        throw new ArgumentException("Not supported field logical operator = " + searchCriteria.GetFieldLogicalOperator());
                }
            }

            // Check search criteria FieldSearchCriteria.
            if (isElasticsearchUsed)
            {
                searchCriteria.FieldSearchCriteria.CheckData(mapping);
            }

            // Check search criteria IncludeRedListCategories.
            if (searchCriteria.IncludeRedListCategories.IsNotEmpty())
            {
                foreach (RedListCategory redListCategory in searchCriteria.IncludeRedListCategories)
                {
                    if ((redListCategory < RedListCategory.DD) ||
                        (RedListCategory.NT < redListCategory))
                    {
                        throw new ArgumentException("Not supported red list category = " + redListCategory.ToString());
                    }
                }
            }

            // Check search criteria IsAccuracyConsidered.
            if (searchCriteria.IsAccuracyConsidered)
            {
                // Check that at least one geometry has been specified.
                if (searchCriteria.BoundingBox.IsNull() &&
                    searchCriteria.Polygons.IsEmpty() &&
                    searchCriteria.RegionGuids.IsEmpty())
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: Search criteria IsAccuracyConsidered requires that at least one of BoundingBox, Polygons and RegionGuids must be specified.");
                }
            }

            // Check search criteria IsDisturbanceSensitivityConsidered.
            if (searchCriteria.IsDisturbanceSensitivityConsidered)
            {
                // Check that at least one geometry has been specified.
                if (searchCriteria.BoundingBox.IsNull() &&
                    searchCriteria.Polygons.IsEmpty() &&
                    searchCriteria.RegionGuids.IsEmpty())
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: Search criteria IsDisturbanceSensitivityConsidered requires that at least one of BoundingBox, Polygons and RegionGuids must be specified.");
                }
            }

            // Check search criteria LocalityNameSearchString.
            searchCriteria.LocalityNameSearchString.CheckData(true, isElasticsearchUsed);

            // Check search criteria MinProtectionLevel.
            if (searchCriteria.IsMinProtectionLevelSpecified)
            {
                // MinProtectionLevel must be 1 or larger.
                if (searchCriteria.MinProtectionLevel < 1)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: MinProtectionLevel must be 1 or larger. Current value = " + searchCriteria.MaxProtectionLevel);
                }
            }

            // Check search criteria MaxProtectionLevel and
            // MaxProtectionLevel.
            if ((searchCriteria.IsMaxProtectionLevelSpecified) &&
                (searchCriteria.IsMinProtectionLevelSpecified))
            {
                // MaxProtectionLevel must be equal or larger than
                // MinProtectionLevel.
                if (searchCriteria.MaxProtectionLevel <
                    searchCriteria.MinProtectionLevel)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: MaxProtectionLevel must be larger than MinProtectionLevel." +
                                                " Current value MaxProtectionLevel = " + searchCriteria.MaxProtectionLevel +
                                                " Current value MinProtectionLevel = " + searchCriteria.MinProtectionLevel);
                }
            }

            // Check search criteria MaxProtectionLevel.
            if (searchCriteria.IsMaxProtectionLevelSpecified)
            {
                // MaxProtectionLevel must be 1 or larger.
                if (searchCriteria.MaxProtectionLevel < 1)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: MaxProtectionLevel must be 1 or larger. Current value = " + searchCriteria.MaxProtectionLevel);
                }
            }

            // Adjust max protection level to access rights.
            if (!isElasticsearchUsed)
            {
                currentRoles = context.CurrentRoles;
                if (currentRoles.IsSimpleSpeciesObservationAccessRights())
                {
                    if (searchCriteria.IsMaxProtectionLevelSpecified)
                    {
                        searchCriteria.MaxProtectionLevel = Math.Min(searchCriteria.MaxProtectionLevel,
                                                                     currentRoles.GetMaxProtectionLevel());
                    }
                    else
                    {
                        searchCriteria.MaxProtectionLevel = currentRoles.GetMaxProtectionLevel();
                    }

                    searchCriteria.IsMaxProtectionLevelSpecified = true;
                }
            }

            // Check search criteria ObservationDateTime.
            searchCriteria.ObservationDateTime.CheckData(true);

            ////            if (searchCriteria.ObserverIds.IsNotEmpty())
            ////            {
            ////                throw new NotImplementedException("WebSpeciesObservationSearchCriteria: Property ObserverIds is currently not used.");
            ////            }

            // Check search criteria ObserverSearchString.
            searchCriteria.ObserverSearchString.CheckData(true, isElasticsearchUsed);

            // Check search criteria Polygons.
            if (searchCriteria.Polygons.IsNotEmpty())
            {
                foreach (WebPolygon polygon in searchCriteria.Polygons)
                {
                    polygon.CheckData();
                }
            }

            ////            if (searchCriteria.ProjectGuids.IsNotEmpty())
            ////            {
            ////                throw new NotImplementedException("WebSpeciesObservationSearchCriteria: Property ProjectIds is currently not used.");
            ////            }

            // Check search criteria RegionGuids.
            if (searchCriteria.RegionGuids.IsNotEmpty())
            {
                if (useRegionOptimization)
                {
                    for (index = 0; index < searchCriteria.RegionGuids.Count; index++)
                    {
                        searchCriteria.RegionGuids[index] = searchCriteria.RegionGuids[index].CheckInjection();
                    }

                    // Make a list of RegionGUID from searchCriteria.RegionGuids
                    List<RegionGUID> regionGuidList = new List<RegionGUID>();
                    foreach (string regionGuidStr in searchCriteria.RegionGuids)
                    {
                        RegionGUID regionGuid = new RegionGUID(regionGuidStr);
                        regionGuidList.Add(regionGuid);
                    }

                    // Get counties and provinces only
                    List<RegionGUID> countiesAndProvinceRegionGuids =
                        regionGuidList.Where(x => x.CategoryId == (int)RegionCategoryId.County ||
                                                  x.CategoryId == (int)RegionCategoryId.Province).ToList();

                    bool hasCustomCounties = false;
                    bool hasCustomProvinces = false;

                    if (countiesAndProvinceRegionGuids.IsNotEmpty())
                    {
                        // Check that county regions are valid (i.e, exist)
                        List<WebRegion> existingCounties =
                            WebServiceData.SpeciesObservationManager.GetCountyRegions(context);
                        List<RegionGUID> countyRegionGuids =
                            regionGuidList.Where(x => x.CategoryId == (int)RegionCategoryId.County).ToList();

                        foreach (RegionGUID countyRegionGuid in countyRegionGuids)
                        {
                            bool countyRegionExist =
                                existingCounties.Any(x => x.CategoryId == countyRegionGuid.CategoryId &&
                                                          x.NativeId == countyRegionGuid.NativeId);
                            if (!countyRegionExist)
                            {
                                throw new ArgumentException(
                                    "WebSpeciesObservationSearchCriteria: There are no County with specified NativeId: " +
                                    countyRegionGuid.NativeId);
                            }
                        }

                        // Check that province regions are valid (i.e, exist)
                        List<WebRegion> existingProvinces =
                            WebServiceData.SpeciesObservationManager.GetProvinceRegions(context);
                        List<RegionGUID> provinceRegionGuids =
                            regionGuidList.Where(x => x.CategoryId == (int)RegionCategoryId.Province).ToList();

                        foreach (RegionGUID provinceRegionGuid in provinceRegionGuids)
                        {
                            bool provinceRegionExist =
                                existingProvinces.Any(x => x.CategoryId == provinceRegionGuid.CategoryId &&
                                                           x.NativeId == provinceRegionGuid.NativeId);
                            if (!provinceRegionExist)
                            {
                                throw new ArgumentException(
                                    "WebSpeciesObservationSearchCriteria: There are no Province with specified NativeId: " +
                                    provinceRegionGuid.NativeId);
                            }
                        }

                        // Get value representing if there are custom counties in the regions (i.e, Kalmar fastland or Öland)
                        hasCustomCounties = countyRegionGuids.Any(x =>
                            x.CategoryId == (int)RegionCategoryId.County &&
                            (x.NativeId.Equals(CountyFeatureId.KalmarFastland) ||
                             x.NativeId.Equals(CountyFeatureId.Oland)));

                        // Get value representing if there are custom provinces in the regions (i.e, Lappland)
                        hasCustomProvinces = provinceRegionGuids.Any(x =>
                            x.CategoryId == (int)RegionCategoryId.Province &&
                            (x.NativeId.Equals(ProvinceFeatureId.TorneLappmark) ||
                             x.NativeId.Equals(ProvinceFeatureId.AseleLappmark) ||
                             x.NativeId.Equals(ProvinceFeatureId.LuleLappmark) ||
                             x.NativeId.Equals(ProvinceFeatureId.LyckseleLappmark) ||
                             x.NativeId.Equals(ProvinceFeatureId.PiteLappmark)));

                        // Get value representing if regions contains regions other than counties or provinces
                        bool hasRegionsNotCountyOrProvince =
                            regionGuidList.Any(
                                x =>
                                    x.CategoryId != (int)RegionCategoryId.County &&
                                    x.CategoryId != (int)RegionCategoryId.Province);

                        // If there are custom counties or provinces AND other regions, that is not supported
                        if (hasRegionsNotCountyOrProvince && (hasCustomCounties || hasCustomProvinces))
                        {
                            throw new NotSupportedException(
                                @"WebSpeciesObservationSearchCriteria: It is not allowed to have both custom 
                            counties/provinces and other regions in the same WebSpeciesObservationSearchCriteria.");
                        }

                        // If there are 
                        // 1. Regions other than counties or regions
                        // 2. AND Regions that are counties or regions 
                        // 3. But NO custom region like Kalmar fastland, Lule Lappmark etc.
                        // Then use the standard way of filtering county and province regions
                        if (countiesAndProvinceRegionGuids.IsNotEmpty() && hasRegionsNotCountyOrProvince &&
                            !hasCustomCounties && !hasCustomProvinces)
                        {
                            // Use the standard way of filtering regions
                        }
                        else
                        {
                            CountyProvinceRegionSearchType selectedRegionSearchType = CountyProvinceRegionSearchType.ByCoordinate;

                            if (searchCriteria.DataFields.IsNull())
                            {
                                searchCriteria.DataFields = new List<WebDataField>();
                            }

                            String countyProvinceFilterType;
                            if (searchCriteria.DataFields.IsDataFieldSpecified(typeof(CountyProvinceRegionSearchType).ToString()))
                            {
                                countyProvinceFilterType = searchCriteria.DataFields.GetString(typeof(CountyProvinceRegionSearchType).ToString());
                                if (countyProvinceFilterType.Equals(CountyProvinceRegionSearchType.ByName.ToString()))
                                {
                                    selectedRegionSearchType = CountyProvinceRegionSearchType.ByName;
                                }
                            }
                            else
                            {
                                WebDataField dataFieldByCoordinate = new WebDataField()
                                {
                                    Name = typeof(CountyProvinceRegionSearchType).ToString(),
                                    Value = CountyProvinceRegionSearchType.ByCoordinate.ToString(),
                                    Type = WebDataType.String
                                };

                                searchCriteria.DataFields.Add(dataFieldByCoordinate);
                            }

                            if (!isElasticsearchUsed)
                            {
                                foreach (RegionGUID regionGuid in countiesAndProvinceRegionGuids)
                                {
                                    // Make a WebDataField for each county and province.
                                    searchCriteria.RegionGuids.Remove(regionGuid.GUID);

                                    string filterOnField = GetFieldName(regionGuid, selectedRegionSearchType);
                                    string fieldValue = regionGuid.NativeId;

                                    WebDataField dataField = CreateDataField(
                                        filterOnField,
                                        fieldValue,
                                        WebDataType.String);
                                    searchCriteria.DataFields.Add(dataField);
                                }
                            }
                        }
                    }

                    regions = WebServiceData.RegionManager.GetRegionsByGuids(context, searchCriteria.RegionGuids);

                    foreach (String regionGuid in searchCriteria.RegionGuids)
                    {
                        isRegionFound = false;
                        if (regions.IsNotEmpty())
                        {
                            foreach (WebRegion region in regions)
                            {
                                if (region.GUID.ToLower() == regionGuid.ToLower())
                                {
                                    isRegionFound = true;
                                    break;
                                }
                            }
                        }

                        if (!isRegionFound)
                        {
                            throw new ArgumentException("WebSpeciesObservationSearchCriteria: Unknown region guid = " +
                                                        regionGuid);
                        }
                    }
                }
                else
                {
                    for (index = 0; index < searchCriteria.RegionGuids.Count; index++)
                    {
                        searchCriteria.RegionGuids[index] = searchCriteria.RegionGuids[index].CheckInjection();
                    }

                    regions = WebServiceData.RegionManager.GetRegionsByGuids(context, searchCriteria.RegionGuids);
                    foreach (String regionGuid in searchCriteria.RegionGuids)
                    {
                        isRegionFound = false;
                        if (regions.IsNotEmpty())
                        {
                            foreach (WebRegion region in regions)
                            {
                                if (region.GUID.ToLower() == regionGuid.ToLower())
                                {
                                    isRegionFound = true;
                                    break;
                                }
                            }
                        }

                        if (!isRegionFound)
                        {
                            throw new ArgumentException("WebSpeciesObservationSearchCriteria: Unknown region guid = " + regionGuid);
                        }
                    }
                }
            }

            // Check search criteria RegistrationDateTime.
#if SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            searchCriteria.RegistrationDateTime.CheckData(false);
#else
            searchCriteria.ReportedDateTime.CheckData(false);
#endif

            // Check search criteria SpeciesActivityIds.
            //if (searchCriteria.SpeciesActivityIds.IsNotEmpty())
            //{
            //    // Each species activity id must match id
            //    // of one of the species activities.
            //    speciesActivities = WebServiceData.SpeciesActivityManager.GetSpeciesActivities(context);
            //    foreach (Int32 speciesActivityId in searchCriteria.SpeciesActivityIds)
            //    {
            //        isSpeciesActivityIdFound = false;
            //        foreach (WebSpeciesActivity speciesActivity in speciesActivities)
            //        {
            //            if (speciesActivity.Id == speciesActivityId)
            //            {
            //                isSpeciesActivityIdFound = true;
            //                break;
            //            }
            //        }
            //        if (!isSpeciesActivityIdFound)
            //        {
            //            throw new ArgumentException("WebSpeciesObservationSearchCriteria: Invalid species activity id. Current value = " + speciesActivityId);
            //        }
            //    }
            //}

            // Check search criteria TaxonIds.
            // TODO: Check that all ids belongs to
            // TODO: existing taxa (both valid or invalid).

            ////            if (searchCriteria.ValidationStatusIds.IsNotEmpty())
            ////            {
            ////                throw new NotImplementedException("WebSpeciesObservationSearchCriteria: Property TaxonValidationStatusIds is currently not used.");
            ////            }
        }

        /// <summary>
        /// Set field name to use when filtering on county and province regions.
        /// </summary>
        /// <param name="regionGuid">A RegionGuid to filter on</param>
        /// <param name="searchType">The RegionSearchType, default is ByCoordinate</param>
        /// <returns>The name of the field to filter on</returns>
        private static string GetFieldName(RegionGUID regionGuid, CountyProvinceRegionSearchType searchType)
        {
            string fieldName = string.Empty;

            if (regionGuid.CategoryId == (int)RegionCategoryId.County)
            {
                if (regionGuid.NativeId.Equals(CountyFeatureId.KalmarFastland) ||
                    regionGuid.NativeId.Equals(CountyFeatureId.Oland))
                {
                    fieldName = "CountyPartId";
                }
                else
                {
                    fieldName = "CountyId";
                }
            }

            if (regionGuid.CategoryId == (int)RegionCategoryId.Province)
            {
                if (regionGuid.NativeId.Equals(ProvinceFeatureId.LuleLappmark) ||
                    regionGuid.NativeId.Equals(ProvinceFeatureId.PiteLappmark) ||
                    regionGuid.NativeId.Equals(ProvinceFeatureId.TorneLappmark) ||
                    regionGuid.NativeId.Equals(ProvinceFeatureId.AseleLappmark) ||
                    regionGuid.NativeId.Equals(ProvinceFeatureId.LyckseleLappmark))
                {
                    fieldName = "ProvincePartId";
                }
                else
                {
                    fieldName = "ProvinceId";
                }
            }

            return fieldName + searchType;
        }

        /// <summary>
        /// Create a WeDataField
        /// </summary>
        /// <param name="name">The name of the WebDataField</param>
        /// <param name="info">Valud to put in property Information</param>
        /// <param name="webDataType">The WebDataType</param>
        /// <param name="value">The value of the WebDataField</param>
        /// <returns></returns>
        private static WebDataField CreateDataField(string name, string value, WebDataType webDataType)
        {
            WebDataField dataField = new WebDataField();

            dataField.Name = name;
            dataField.Type = webDataType;
            dataField.Value = value;

            return dataField;
        }

        /// <summary>
        /// Get bounding box for the species observation search criteria.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias.</param>
        /// <returns>Bounding box for the species observation search criteria.</returns>
        private static WebBoundingBox GetEnvelopingBoundingBox(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                               WebServiceContext context,
                                                               WebCoordinateSystem coordinateSystem)
        {
            List<WebRegionGeography> regionGeographies;
            WebBoundingBox envelopingBoundingBox, polygonBoundingBox;
            WebCoordinateSystem speciesObservationCoordinateSystem;

            envelopingBoundingBox = null;

            if (searchCriteria.Polygons.IsNotEmpty() ||
                searchCriteria.RegionGuids.IsNotEmpty())
            {
                speciesObservationCoordinateSystem = new WebCoordinateSystem();
                speciesObservationCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;

                // Find the outermost coordinates enveloping all
                // coordinates in the search criteria
                // (polygons, bounding box or regions)
                envelopingBoundingBox = new WebBoundingBox();

                if (searchCriteria.Polygons.IsNotEmpty())
                {
                    polygonBoundingBox = new WebBoundingBox();
                    foreach (WebPolygon polygon in searchCriteria.Polygons)
                    {
                        polygonBoundingBox.Add(polygon.GetBoundingBox());
                    }

                    if (coordinateSystem.GetWkt().ToLower() != speciesObservationCoordinateSystem.GetWkt().ToLower())
                    {
                        polygonBoundingBox = WebServiceData.CoordinateConversionManager.GetConvertedBoundingBox(polygonBoundingBox,
                                                                                                                coordinateSystem,
                                                                                                                speciesObservationCoordinateSystem).GetBoundingBox();
                    }

                    envelopingBoundingBox.Add(polygonBoundingBox);
                }

                if (searchCriteria.RegionGuids.IsNotEmpty())
                {
                    regionGeographies = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                                searchCriteria.RegionGuids,
                                                                                                speciesObservationCoordinateSystem);
                    foreach (WebRegionGeography regionGeography in regionGeographies)
                    {
                        envelopingBoundingBox.Add(regionGeography.BoundingBox);
                    }
                }

                if (searchCriteria.IsAccuracyConsidered ||
                    searchCriteria.IsDisturbanceSensitivityConsidered)
                {
                    // Add buffer to allow for species observations
                    // outside geometries to be included in result.
                    // This value has the disadvantage that species
                    // observations with bad accuracy will not be 
                    // included in the result.
                    envelopingBoundingBox.AddBuffer(10000);
                }
                else
                {
                    // Add buffer to avoid rounding problems.
                    envelopingBoundingBox.AddBuffer(10);
                }
            }

            return envelopingBoundingBox;
        }

        /// <summary>
        /// Get logical operator used between field search criteria.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Logical operator used between field search criteria.</returns>
        public static LogicalOperator GetFieldLogicalOperator(this WebSpeciesObservationSearchCriteria searchCriteria)
        {
            LogicalOperator logicalOperator;

            if (searchCriteria.DataFields.IsDataFieldSpecified("FieldLogicalOperator") &&
                Enum.TryParse(searchCriteria.DataFields.GetString("FieldLogicalOperator"), out logicalOperator))
            {
                return logicalOperator;
            }
            else
            {
                // Return default value.
                return LogicalOperator.And;
            }
        }

        /// <summary>
        /// Get species observation filter to be used with Elasticsearch.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="isProtectedSpeciesObservationIndicationUsed">Indicates if method GetProtectedSpeciesObservationIndication is used.</param>
        /// <returns>Species observation filter.</returns>
        public static String GetFilter(this WebSpeciesObservationSearchCriteria searchCriteria,
                                       WebServiceContext context,
                                       Boolean isProtectedSpeciesObservationIndicationUsed)
        {
            Boolean isFirstFilterSetting;
            Int32 index;
            List<Int32> speciesObservationDataProviderIds;
            String accessRights;
            StringBuilder filter;
            WebCoordinateSystem coordinateSystem;

            isFirstFilterSetting = true;
            filter = new StringBuilder();

            filter.Append("\"filter\": {\"bool\":{ \"must\" : [");

            if (searchCriteria.IsMinProtectionLevelSpecified &&
                (searchCriteria.MinProtectionLevel > (Int32)(SpeciesProtectionLevelEnum.Public)))
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"range\": {");
                filter.Append(" \"Conservation_ProtectionLevel\": {");
                filter.Append(" \"gte\": " + searchCriteria.MinProtectionLevel.WebToString());
                filter.Append("}}}");
            }

            if (searchCriteria.TaxonIds.IsNotEmpty())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"terms\": {");
                filter.Append(" \"Taxon_DyntaxaTaxonID\":[");
                filter.Append(searchCriteria.TaxonIds[0].WebToString());
                for (index = 1; index < searchCriteria.TaxonIds.Count; index++)
                {
                    filter.Append(", " + searchCriteria.TaxonIds[index].WebToString());
                }

                filter.Append("]}}");
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.IsBirdNestActivityLimitSpecified)
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"range\": {");
                filter.Append(" \"Occurrence_BirdNestActivityId\": {");
                filter.Append(" \"lte\": " + searchCriteria.BirdNestActivityLimit.WebToString());
                filter.Append("}}}");
            }
#endif

            if (searchCriteria.IsAccuracySpecified)
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"range\": {");
                filter.Append(" \"Location_CoordinateUncertaintyInMeters\": {");
                filter.Append(" \"lte\": " + ((Int32)(searchCriteria.Accuracy)).WebToString());
                filter.Append("}}}");
            }

            if (searchCriteria.ChangeDateTime.IsNotNull())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(searchCriteria.ChangeDateTime.GetFilter("DarwinCore_Modified"));
            }

            if (searchCriteria.ObservationDateTime.IsNotNull())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(searchCriteria.ObservationDateTime.GetFilter("Event_Start", "Event_End", "ObservationDateTimeAccuracy"));
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.DataProviderGuids.IsNotEmpty() &&
                !searchCriteria.IsAllSpeciesObservationDataProvidersSelected(context))
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                speciesObservationDataProviderIds = searchCriteria.GetSpeciesObservationDataProviderIds(context);
                filter.Append("{ \"terms\": {");
                filter.Append(" \"DarwinCore_DataProviderId\":[");
                filter.Append(speciesObservationDataProviderIds[0].WebToString());
                for (index = 1; index < speciesObservationDataProviderIds.Count; index++)
                {
                    filter.Append(", " + speciesObservationDataProviderIds[index].WebToString());
                }

                filter.Append("]}}");
            }
#endif

            if (searchCriteria.FieldSearchCriteria.IsNotEmpty())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                if ((searchCriteria.FieldSearchCriteria.Count > 1) &&
                    (searchCriteria.GetFieldLogicalOperator() == LogicalOperator.Or))
                {
                    filter.Append("{\"bool\":{ \"should\" : [");
                }

                filter.Append(searchCriteria.FieldSearchCriteria.GetFilter());

                if ((searchCriteria.FieldSearchCriteria.Count > 1) &&
                    (searchCriteria.GetFieldLogicalOperator() == LogicalOperator.Or))
                {
                    filter.Append("]}}");
                }
            }

            // Add condition for IncludeNeverFoundObservations
            // IncludeNotRediscoveredObservations and
            // IncludePositiveObservations.
            if (searchCriteria.IncludeNeverFoundObservations ||
                searchCriteria.IncludeNotRediscoveredObservations ||
                searchCriteria.IncludePositiveObservations)
            {
                if (!(searchCriteria.IncludeNeverFoundObservations &&
                      searchCriteria.IncludeNotRediscoveredObservations &&
                      searchCriteria.IncludePositiveObservations))
                {
                    if (isFirstFilterSetting)
                    {
                        isFirstFilterSetting = false;
                    }
                    else
                    {
                        filter.Append(", ");
                    }

                    if ((searchCriteria.IncludeNeverFoundObservations && searchCriteria.IncludeNotRediscoveredObservations) ||
                        (searchCriteria.IncludeNeverFoundObservations && searchCriteria.IncludePositiveObservations) ||
                        (searchCriteria.IncludeNotRediscoveredObservations && searchCriteria.IncludePositiveObservations))
                    {
                        filter.Append("{\"bool\":{ \"should\" : [");
                    }

                    if (searchCriteria.IncludeNeverFoundObservations)
                    {
                        filter.Append("{ \"term\": {");
                        filter.Append(" \"Occurrence_IsNeverFoundObservation\": true");
                        filter.Append("}}");
                    }

                    if (searchCriteria.IncludeNotRediscoveredObservations)
                    {
                        if (searchCriteria.IncludeNeverFoundObservations)
                        {
                            filter.Append(", ");
                        }

                        filter.Append("{ \"term\": {");
                        filter.Append(" \"Occurrence_IsNotRediscoveredObservation\": true");
                        filter.Append("}}");
                    }

                    if (searchCriteria.IncludePositiveObservations)
                    {
                        if (searchCriteria.IncludeNeverFoundObservations ||
                            searchCriteria.IncludeNotRediscoveredObservations)
                        {
                            filter.Append(", ");
                        }

                        filter.Append("{ \"term\": {");
                        filter.Append(" \"Occurrence_IsPositiveObservation\": true");
                        filter.Append("}}");
                    }

                    if ((searchCriteria.IncludeNeverFoundObservations && searchCriteria.IncludeNotRediscoveredObservations) ||
                        (searchCriteria.IncludeNeverFoundObservations && searchCriteria.IncludePositiveObservations) ||
                        (searchCriteria.IncludeNotRediscoveredObservations && searchCriteria.IncludePositiveObservations))
                    {
                        filter.Append("]}}");
                    }
                }
                // else: All observations are included.
                //       No need to add condition.
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.IsIsNaturalOccurrenceSpecified)
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"term\": {");
                filter.Append(" \"Occurrence_IsNaturalOccurrence\": ");
                if (searchCriteria.IsNaturalOccurrence)
                {
                    filter.Append("true");
                }
                else
                {
                    filter.Append("false");
                }

                filter.Append("}}");
            }
#endif

            if (searchCriteria.IsMaxProtectionLevelSpecified)
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append("{ \"range\": {");
                filter.Append(" \"Conservation_ProtectionLevel\": {");
                filter.Append(" \"lte\": " + searchCriteria.MaxProtectionLevel.WebToString());
                filter.Append("}}}");
            }

            if (searchCriteria.LocalityNameSearchString.IsNotNull())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(searchCriteria.LocalityNameSearchString.GetFilter("Location_Locality"));
            }

            if (searchCriteria.ObserverSearchString.IsNotNull())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(searchCriteria.ObserverSearchString.GetFilter("Occurrence_RecordedBy"));
            }

            if (searchCriteria.Polygons.IsNotEmpty())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                // Filter on bounding box first
                // in order to improve performance:
                coordinateSystem = new WebCoordinateSystem();
                coordinateSystem.Id = CoordinateSystemId.WGS84;
                filter.Append("{ \"geo_shape\": {");
                filter.Append(" \"Location\": {");
                filter.Append(" \"shape\": {");
                filter.Append(" \"type\": \"envelope\", ");
                filter.Append(" \"coordinates\": ");
                if (searchCriteria.IsAccuracyConsidered ||
                    searchCriteria.IsDisturbanceSensitivityConsidered)
                {
                    filter.Append(searchCriteria.Polygons.GetEnvelopeJson(coordinateSystem, 10000));
                }
                else
                {
                    filter.Append(searchCriteria.Polygons.GetEnvelopeJson(coordinateSystem, 10));
                }

                filter.Append("}}}},");

                if (searchCriteria.IsAccuracyConsidered)
                {
                    if (searchCriteria.IsDisturbanceSensitivityConsidered)
                    {
                        filter.Append("{\"bool\":{ \"should\" : [");

                        // First OR part.
                        filter.Append("{\"bool\":{ \"must\" : [");

                        // First AND part.
                        filter.Append("{ \"script\": {");
                        filter.Append(" \"script\": \"doc['Location_CoordinateUncertaintyInMeters'].value == doc['Location_MaxCoordinateUncertaintyInMetersOrDisturbanceRadius'].value\"");
                        filter.Append("}}");

                        // Second AND part.
                        filter.Append(",{ \"geo_shape\": {");
                        filter.Append(" \"CoordinateUncertaintyInMeters\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");

                        filter.Append("]}}");

                        // Second OR part.
                        filter.Append(",{\"bool\":{ \"must\" : [");

                        // First AND part.
                        filter.Append("{ \"script\": {");
                        filter.Append(" \"script\": \"doc['Location_CoordinateUncertaintyInMeters'].value != doc['Location_MaxCoordinateUncertaintyInMetersOrDisturbanceRadius'].value\"");
                        filter.Append("}}");

                        // Second AND part.
                        filter.Append(",{ \"geo_shape\": {");
                        filter.Append(" \"DisturbanceRadius\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");

                        filter.Append("]}}");

                        filter.Append("]}}");
                    }
                    else
                    {
                        filter.Append("{ \"geo_shape\": {");
                        filter.Append(" \"CoordinateUncertaintyInMeters\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");
                    }
                }
                else
                {
                    if (searchCriteria.IsDisturbanceSensitivityConsidered)
                    {
                        filter.Append("{\"bool\":{ \"should\" : [");

                        // First OR part.
                        filter.Append("{\"bool\":{ \"must\" : [");

                        // First AND part.
                        filter.Append("{ \"term\": {");
                        filter.Append(" \"Location_DisturbanceRadius\": 0");
                        filter.Append("}}");

                        // Second AND part.
                        filter.Append(",{ \"geo_shape\": {");
                        filter.Append(" \"Location\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");

                        filter.Append("]}}");

                        // Second OR part.
                        filter.Append(",{ \"geo_shape\": {");
                        filter.Append(" \"DisturbanceRadius\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");

                        filter.Append("]}}");
                    }
                    else
                    {
                        filter.Append("{ \"geo_shape\": {");
                        filter.Append(" \"Location\": {");
                        filter.Append(" \"shape\": {");
                        filter.Append(" \"type\": \"multipolygon\", ");
                        filter.Append(" \"coordinates\": ");
                        filter.Append(searchCriteria.Polygons.GetJson());
                        filter.Append("}}}}");
                    }
                }
            }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.ReportedDateTime.IsNotNull())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(searchCriteria.ReportedDateTime.GetFilter("DarwinCore_ReportedDate"));
            }
#endif

            if (isProtectedSpeciesObservationIndicationUsed)
            {
                accessRights = context.CurrentRoles.GetProtectedSpeciesObservationIndicationAccessRightsJson(context);
            }
            else
            {
                accessRights = context.CurrentRoles.GetSpeciesObservationAccessRightsJson(context);
            }

            if (accessRights.IsNotEmpty())
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                filter.Append(accessRights);
            }

            if (searchCriteria.DataFields.IsDataFieldSpecified("Periodicity"))
            {
                if (isFirstFilterSetting)
                {
                    isFirstFilterSetting = false;
                }
                else
                {
                    filter.Append(", ");
                }

                switch (searchCriteria.DataFields.GetString("Periodicity"))
                {
                    case "Daily":
                    case "DayOfTheYear":
                        filter.Append("{ \"term\" : { \"ObservationDateTimeIsOneDay\" : true }}");
                        break;

                    case "Weekly":
                    case "WeekOfTheYear":
                        filter.Append("{ \"term\" : { \"ObservationDateTimeIsOneWeek\" : true }}");
                        break;

                    case "Monthly":
                    case "MonthOfTheYear":
                        filter.Append("{ \"term\" : { \"ObservationDateTimeIsOneMonth\" : true }}");
                        break;

                    case "Yearly":
                        filter.Append("{ \"term\" : { \"ObservationDateTimeIsOneYear\" : true }}");
                        break;
                }
            }

            filter.Append("]}}");

            return filter.ToString();
        }

        /// <summary>
        /// Get SQL geometry where condition.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>SQL geometry where condition.</returns>
        public static String GetGeometryWhereCondition(this WebSpeciesObservationSearchCriteria searchCriteria)
        {
            StringBuilder whereCondition;

            whereCondition = new StringBuilder();

            // Add condition for geometries.
            if (searchCriteria.BoundingBox.IsNotNull() ||
                searchCriteria.Polygons.IsNotEmpty() ||
                searchCriteria.RegionGuids.IsNotEmpty())
            {
                if (searchCriteria.IsAccuracyConsidered ||
                    searchCriteria.IsDisturbanceSensitivityConsidered)
                {
                    if (searchCriteria.IsAccuracyConsidered &&
                        searchCriteria.IsDisturbanceSensitivityConsidered)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(@" (SpeciesObservation.Point_GoogleMercator.STBuffer(SpeciesObservation.maxAccuracyOrDisturbanceRadius).STIntersects(@geometryUnion) = 1) ");
                    }
                    else
                    {
                        if (searchCriteria.IsAccuracyConsidered)
                        {
                            AddWhereCondition(whereCondition);
                            whereCondition.Append(@" (SpeciesObservation.Point_GoogleMercator.STBuffer(SpeciesObservation.coordinateUncertaintyInMeters).STIntersects(@geometryUnion) = 1) ");
                        }
                        if (searchCriteria.IsDisturbanceSensitivityConsidered)
                        {
                            AddWhereCondition(whereCondition);
                            whereCondition.Append(@" (SpeciesObservation.Point_GoogleMercator.STBuffer(SpeciesObservation.disturbanceRadius).STIntersects(@geometryUnion) = 1) ");
                        }
                    }
                }
                else
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(@" (SpeciesObservation.Point_GoogleMercator.STIntersects(@geometryUnion) = 1) ");
                }
            }

            return whereCondition.ToString();
        }

        /// <summary>
        /// Get SQL join condition.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>SQL join condition.</returns>
        public static String GetJoinCondition(this WebSpeciesObservationSearchCriteria searchCriteria)
        {
            if (searchCriteria.IncludeRedListCategories.IsNotEmpty() ||
                searchCriteria.IncludeRedlistedTaxa ||
                searchCriteria.TaxonIds.IsNotEmpty())
            {
                return @" INNER JOIN #TaxonIds AS InputTaxa ON InputTaxa.TaxonId = O.dyntaxaTaxonId ";
            }
            else
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Get polygons as SqlGeography.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias. </param>
        /// <returns>Region ids.</returns>
        public static List<SqlGeography> GetPolygonsAsGeography(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                                WebCoordinateSystem coordinateSystem)
        {
            List<SqlGeography> polygons;
            List<WebPolygon> webPolygons;
            WebCoordinateSystem speciesObservationCoordinateSystem;

            polygons = null;
            if (searchCriteria.Polygons.IsNotEmpty())
            {
                speciesObservationCoordinateSystem = new WebCoordinateSystem();
                speciesObservationCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
                polygons = new List<SqlGeography>();
                if (coordinateSystem.GetWkt().ToLower() != speciesObservationCoordinateSystem.GetWkt().ToLower())
                {
                    webPolygons = WebServiceData.CoordinateConversionManager.GetConvertedPolygons(searchCriteria.Polygons,
                                                                                                  coordinateSystem,
                                                                                                  speciesObservationCoordinateSystem);
                }
                else
                {
                    webPolygons = searchCriteria.Polygons;
                }
                foreach (WebPolygon polygon in webPolygons)
                {
                    polygons.Add(polygon.GetGeography());
                }
            }
            return polygons;
        }

        /// <summary>
        /// Get polygons as SqlGeometry.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criterias. </param>
        /// <returns>Region ids.</returns>
        public static List<SqlGeometry> GetPolygonsAsGeometry(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            List<SqlGeometry> polygons;
            List<WebPolygon> webPolygons;
            WebCoordinateSystem speciesObservationCoordinateSystem;

            polygons = null;
            if (searchCriteria.Polygons.IsNotEmpty())
            {
                speciesObservationCoordinateSystem = new WebCoordinateSystem();
                speciesObservationCoordinateSystem.Id = CoordinateSystemId.GoogleMercator;
                polygons = new List<SqlGeometry>();
                if (coordinateSystem.GetWkt().ToLower() != speciesObservationCoordinateSystem.GetWkt().ToLower())
                {
                    webPolygons = WebServiceData.CoordinateConversionManager.GetConvertedPolygons(searchCriteria.Polygons,
                                                                                                  coordinateSystem,
                                                                                                  speciesObservationCoordinateSystem);
                }
                else
                {
                    webPolygons = searchCriteria.Polygons;
                }
                foreach (WebPolygon polygon in webPolygons)
                {
                    polygons.Add(polygon.GetGeometry());
                }
            }
            return polygons;
        }

        /// <summary>
        /// Get max and min protection level values.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="minProtectionLevel">Min protection level.</param>
        /// <param name="maxProtectionLevel">Max protection level.</param>
        public static void GetProtectionLevels(this WebSpeciesObservationSearchCriteria searchCriteria,
                                               out Int32? minProtectionLevel,
                                               out Int32? maxProtectionLevel)
        {
            maxProtectionLevel = null;
            minProtectionLevel = null;
            if (searchCriteria.IsNotNull())
            {
                if (searchCriteria.IsMaxProtectionLevelSpecified)
                {
                    maxProtectionLevel = searchCriteria.MaxProtectionLevel;
                }
                if (searchCriteria.IsMinProtectionLevelSpecified)
                {
                    minProtectionLevel = searchCriteria.MinProtectionLevel;
                }
            }
        }

        /// <summary>
        /// Get regions ids.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <returns>Region ids.</returns>
        public static List<Int32> GetRegionIds(this WebSpeciesObservationSearchCriteria searchCriteria,
                                               WebServiceContext context)
        {
            return WebServiceData.RegionManager.GetRegionIdsByGuids(context, searchCriteria.RegionGuids);
        }

        /// <summary>
        /// Get ids for all specified species observation data sources.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Information of the web client.</param>
        /// <returns>Ids for all specified species observation data sources.</returns>
        private static List<Int32> GetSpeciesObservationDataProviderIds(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        WebServiceContext context)
        {
            Boolean isDataSourceFound;
            List<Int32> speciesObservationDataSourceIds;
            List<WebSpeciesObservationDataProvider> speciesObservationDataProviders;

            speciesObservationDataSourceIds = new List<Int32>();
#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            if (searchCriteria.DataProviderGuids.IsNotEmpty())
            {
                speciesObservationDataProviders = WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProviders(context);
                foreach (String speciesObservationDataProviderGuid in searchCriteria.DataProviderGuids)
                {
                    isDataSourceFound = false;
                    foreach (WebSpeciesObservationDataProvider speciesObservationDataProvider in speciesObservationDataProviders)
                    {
                        if (speciesObservationDataProvider.Guid.ToLower() == speciesObservationDataProviderGuid.ToLower())
                        {
                            isDataSourceFound = true;
                            speciesObservationDataSourceIds.Add(speciesObservationDataProvider.Id);
                            break;
                        }
                    }

                    if (!isDataSourceFound)
                    {
                        // Can not find data source.
                        throw new ArgumentException("Unknown data source GUID = " + speciesObservationDataProviderGuid + " in species observation search criteria");
                    }
                }
            }
#endif
            return speciesObservationDataSourceIds;
        }

        /// <summary>
        /// Get SQL where condition.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criterias
        /// </param>
        /// <returns>SQL where condition.</returns>
        public static String GetWhereCondition(this WebSpeciesObservationSearchCriteria searchCriteria,
                                               WebServiceContext context,
                                               WebCoordinateSystem coordinateSystem)
        {
            Int32 index;
            List<Int32> speciesObservationDataProviderIds;
            String condition;
            StringBuilder whereCondition;
            //WebBoundingBox boundingBox;

            whereCondition = new StringBuilder();

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            // Add condition for IncludeNeverFoundObservations
            // IncludeNotRediscoveredObservations and
            // IncludePositiveObservations.
            if (searchCriteria.IncludeNeverFoundObservations ||
                searchCriteria.IncludeNotRediscoveredObservations ||
                searchCriteria.IncludePositiveObservations)
            {
                if (!(searchCriteria.IncludeNeverFoundObservations &&
                      searchCriteria.IncludeNotRediscoveredObservations &&
                      searchCriteria.IncludePositiveObservations))
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(" (");
                    condition = String.Empty;
                    if (searchCriteria.IncludeNeverFoundObservations)
                    {
                        whereCondition.Append(" O.isNeverFoundObservation = 1 ");
                        condition = " OR ";
                    }
                    if (searchCriteria.IncludeNotRediscoveredObservations)
                    {
                        whereCondition.Append(condition +
                                              " O.isNotRediscoveredObservation = 1 ");
                        condition = " OR ";
                    }
                    if (searchCriteria.IncludePositiveObservations)
                    {
                        whereCondition.Append(condition +
                                              " O.isPositiveObservation = 1 ");
                    }
                    whereCondition.Append(") ");
                }
                // else: All observations are included.
                //       No need to add condition.
            }
            else
            {
                // No type of observations has been selected.
                // Set default condition.
                AddWhereCondition(whereCondition);
                whereCondition.Append(" (O.isPositiveObservation = 1) ");
            }

            // Add condition for bounding box.
            //boundingBox = searchCriteria.GetEnvelopingBoundingBox(context, coordinateSystem);
            //if (boundingBox.IsNotNull())
            //{
            //    whereCondition.Append(" AND " +
            //                          "(O.coordinateX <= " + (Int32)(boundingBox.Max.X) + " AND " +
            //                          " O.coordinateX >= " + (Int32)(boundingBox.Min.X) + " AND " +
            //                          " O.coordinateY <= " + (Int32)(boundingBox.Max.Y) + " AND " +
            //                          " O.coordinateY >= " + (Int32)(boundingBox.Min.Y) + ") ");
            //}

            // Add condition for accuracy.
            if (searchCriteria.IsAccuracySpecified)
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(" (O.coordinateUncertaintyInMeters <= " + (Int32)(searchCriteria.Accuracy) + ") ");
            }

            // Add condition for bird nest activity limit.
            if (searchCriteria.IsBirdNestActivityLimitSpecified)
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(" ((O.birdNestActivityId IS NULL) OR (O.birdNestActivityId <= " + searchCriteria.BirdNestActivityLimit + ")) ");
            }

            // Add condition for ChangeDateTime
            if (searchCriteria.ChangeDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ChangeDateTime.GetWhereCondition("O.[modified]"));
            }

            // Add condition for data providers.
            if (searchCriteria.DataProviderGuids.IsNotEmpty() &&
                !searchCriteria.IsAllSpeciesObservationDataProvidersSelected(context))
            {
                speciesObservationDataProviderIds = searchCriteria.GetSpeciesObservationDataProviderIds(context);
                AddWhereCondition(whereCondition);
                if (speciesObservationDataProviderIds.Count == 1)
                {
                    whereCondition.Append(" (O.dataProviderId = " + speciesObservationDataProviderIds[0] + " ) ");
                }
                else
                {
                    whereCondition.Append(" (O.dataProviderId IN (" + speciesObservationDataProviderIds[0]);
                    for (index = 1; index < speciesObservationDataProviderIds.Count; index++)
                    {
                        whereCondition.Append(", " + speciesObservationDataProviderIds[index]);
                    }
                    whereCondition.Append("))");
                }
            }

            // Add condition for IsNaturalOccurrence.
            if (searchCriteria.IsIsNaturalOccurrenceSpecified)
            {
                AddWhereCondition(whereCondition);
                if (searchCriteria.IsNaturalOccurrence)
                {
                    whereCondition.Append(" (O.isNaturalOccurrence = 1) ");
                }
                else
                {
                    whereCondition.Append(" (O.isNaturalOccurrence = 0) ");
                }
            }

            if (searchCriteria.IsMaxProtectionLevelSpecified ||
                searchCriteria.IsMinProtectionLevelSpecified)
            {
                if ((searchCriteria.IsMaxProtectionLevelSpecified &&
                     searchCriteria.IsMinProtectionLevelSpecified &&
                     (searchCriteria.MaxProtectionLevel == searchCriteria.MinProtectionLevel)) ||
                    (searchCriteria.IsMaxProtectionLevelSpecified &&
                     !searchCriteria.IsMinProtectionLevelSpecified &&
                     (searchCriteria.MaxProtectionLevel == 1)))
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(" (O.protectionLevel = " + searchCriteria.MaxProtectionLevel + ") ");
                }
                else
                {
                    // Add condition for MaxProtectionLevel.
                    if (searchCriteria.IsMaxProtectionLevelSpecified)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(" (O.protectionLevel <= " + searchCriteria.MaxProtectionLevel + ") ");
                    }
                    // else This must be a call to method GetProtectedSpeciesObservationIndication().

                    // Add condition for MinProtectionLevel.
                    if (searchCriteria.IsMinProtectionLevelSpecified)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(" (O.protectionLevel >= " + searchCriteria.MinProtectionLevel + ") ");
                    }
                }
            }

            // Add condition for LocalityNameSearchString
            if (searchCriteria.LocalityNameSearchString.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.LocalityNameSearchString.GetWhereCondition("O.[locality]"));
            }

            // Add condition for ObserverSearchString
            if (searchCriteria.ObserverSearchString.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ObserverSearchString.GetWhereCondition("O.[recordedBy]"));
            }

            // Add condition for ObservationDateTime
            if (searchCriteria.ObservationDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ObservationDateTime.GetWhereCondition("O.[start]", "O.[end]"));
            }

            // Add condition for RegistrationDateTime
            if (searchCriteria.ReportedDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ReportedDateTime.GetWhereCondition("O.[reportedDate]"));
            }

            // Add condition for species activities.
            if (searchCriteria.SpeciesActivityIds.IsNotEmpty())
            {
                AddWhereCondition(whereCondition);
                if (searchCriteria.SpeciesActivityIds.Count == 1)
                {
                    whereCondition.Append(" (O.activityId = " + searchCriteria.SpeciesActivityIds[0] + " ) ");
                }
                else
                {
                    whereCondition.Append(" (O.activityId IN (" + searchCriteria.SpeciesActivityIds[0]);
                    for (index = 1; index < searchCriteria.SpeciesActivityIds.Count; index++)
                    {
                        whereCondition.Append(", " + searchCriteria.SpeciesActivityIds[index]);
                    }
                    whereCondition.Append("))");
                }
            }

            // Add condition for geometries.
            if (searchCriteria.BoundingBox.IsNotNull() ||
                searchCriteria.Polygons.IsNotEmpty() ||
                searchCriteria.RegionGuids.IsNotEmpty())
            {
                if (searchCriteria.IsAccuracyConsidered ||
                    searchCriteria.IsDisturbanceSensitivityConsidered)
                {
                    if (searchCriteria.IsAccuracyConsidered &&
                        searchCriteria.IsDisturbanceSensitivityConsidered)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(O.maxAccuracyOrDisturbanceRadius).STIntersects(@geometryUnion) = 1) ");
                    }
                    else
                    {
                        if (searchCriteria.IsAccuracyConsidered)
                        {
                            AddWhereCondition(whereCondition);
                            whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(O.coordinateUncertaintyInMeters).STIntersects(@geometryUnion) = 1) ");
                        }
                        if (searchCriteria.IsDisturbanceSensitivityConsidered)
                        {
                            AddWhereCondition(whereCondition);
                            whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(O.disturbanceRadius).STIntersects(@geometryUnion) = 1) ");
                        }
                    }
                }
                else
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(@" (O.Point_GoogleMercator.STIntersects(@geometryUnion) = 1) ");
                }
            }

#if GEOMETRY_COLLECTION_SEARCH
            if (searchCriteria.BoundingBox.IsNotNull() ||
                searchCriteria.Polygons.IsNotEmpty() ||
                searchCriteria.RegionGuids.IsNotEmpty())
            {
                AddWhereCondition(whereCondition);
                if (searchCriteria.IsAccuracyConsidered ||
                    searchCriteria.IsDisturbanceSensitivityConsidered)
                {
                    if (searchCriteria.IsAccuracyConsidered &&
                        searchCriteria.IsDisturbanceSensitivityConsidered)
                    {
                        whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(CASE WHEN O.coordinateUncertaintyInMeters > T.disturbanceRadius THEN O.coordinateUncertaintyInMeters ELSE T.disturbanceRadius END).STIntersects(@geometryCollection) = 1) ");
                    }
                    else
                    {
                        if (searchCriteria.IsAccuracyConsidered)
                        {
                            whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(O.coordinateUncertaintyInMeters).STIntersects(@geometryCollection) = 1) ");
                        }
                        if (searchCriteria.IsDisturbanceSensitivityConsidered)
                        {
                            whereCondition.Append(@" (O.Point_GoogleMercator.STBuffer(T.disturbanceRadius).STIntersects(@geometryCollection) = 1) ");
                        }
                    }
                }
                else
                {
                    whereCondition.Append(@" (O.Point_GoogleMercator.STIntersects(@geometryCollection) = 1) ");
                }
            }
#endif
#endif

            if (searchCriteria.FieldSearchCriteria.IsNotEmpty())
            {
                LogicalOperator logicalOperator = LogicalOperator.And;

                if (searchCriteria.DataFields.IsDataFieldSpecified("FieldLogicalOperator"))
                {
                    logicalOperator = (LogicalOperator)Enum.Parse(typeof(LogicalOperator), searchCriteria.DataFields.GetString("FieldLogicalOperator"));
                }

                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.FieldSearchCriteria.GetWhereCondition(logicalOperator));
            }

            return whereCondition.ToString();
        }

        /// <summary>
        /// Get SQL where condition.
        /// The difference between GetWhereConditionNew() and 
        /// GetWhereCondition() is that this method optimize handling
        /// of region search criteria.
        /// This method assumes that method
        /// GetGeometryWhereCondition() is also used.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria.
        /// </param>
        /// <returns>SQL where condition.</returns>
        public static String GetWhereConditionNew(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                  WebServiceContext context,
                                                  WebCoordinateSystem coordinateSystem)
        {
            Int32 index;
            List<Int32> speciesObservationDataProviderIds;
            String condition;
            StringBuilder whereCondition;
            WebBoundingBox boundingBox;

            whereCondition = new StringBuilder();

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
            // Add condition for IncludeNeverFoundObservations
            // IncludeNotRediscoveredObservations and
            // IncludePositiveObservations.
            if (searchCriteria.IncludeNeverFoundObservations ||
                searchCriteria.IncludeNotRediscoveredObservations ||
                searchCriteria.IncludePositiveObservations)
            {
                if (!(searchCriteria.IncludeNeverFoundObservations &&
                      searchCriteria.IncludeNotRediscoveredObservations &&
                      searchCriteria.IncludePositiveObservations))
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(" (");
                    condition = String.Empty;
                    if (searchCriteria.IncludeNeverFoundObservations)
                    {
                        whereCondition.Append(" O.isNeverFoundObservation = 1 ");
                        condition = " OR ";
                    }
                    if (searchCriteria.IncludeNotRediscoveredObservations)
                    {
                        whereCondition.Append(condition +
                                              " O.isNotRediscoveredObservation = 1 ");
                        condition = " OR ";
                    }
                    if (searchCriteria.IncludePositiveObservations)
                    {
                        whereCondition.Append(condition +
                                              " O.isPositiveObservation = 1 ");
                    }
                    whereCondition.Append(") ");
                }
                // else: All observations are included.
                //       No need to add condition.
            }
            else
            {
                // No type of observations has been selected.
                // Set default condition.
                AddWhereCondition(whereCondition);
                whereCondition.Append(" (O.isPositiveObservation = 1) ");
            }

            // Add condition for bounding box.
            boundingBox = searchCriteria.GetEnvelopingBoundingBox(context, coordinateSystem);
            if (boundingBox.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(" (O.coordinateX <= " + (Int32)(boundingBox.Max.X) + " AND " +
                                      " O.coordinateX >= " + (Int32)(boundingBox.Min.X) + " AND " +
                                      " O.coordinateY <= " + (Int32)(boundingBox.Max.Y) + " AND " +
                                      " O.coordinateY >= " + (Int32)(boundingBox.Min.Y) + ") ");
            }

            // Add condition for accuracy.
            if (searchCriteria.IsAccuracySpecified)
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(" (O.coordinateUncertaintyInMeters <= " + (Int32)(searchCriteria.Accuracy) + ") ");
            }

            // Add condition for bird nest activity limit.
            if (searchCriteria.IsBirdNestActivityLimitSpecified)
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(" ((O.birdNestActivityId IS NULL) OR (O.birdNestActivityId <= " + searchCriteria.BirdNestActivityLimit + ")) ");
            }

            // Add condition for ChangeDateTime
            if (searchCriteria.ChangeDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ChangeDateTime.GetWhereCondition("O.[modified]"));
            }

            // Add condition for data providers.
            if (searchCriteria.DataProviderGuids.IsNotEmpty() &&
                !searchCriteria.IsAllSpeciesObservationDataProvidersSelected(context))
            {
                speciesObservationDataProviderIds = searchCriteria.GetSpeciesObservationDataProviderIds(context);
                AddWhereCondition(whereCondition);
                if (speciesObservationDataProviderIds.Count == 1)
                {
                    whereCondition.Append(" (O.dataProviderId = " + speciesObservationDataProviderIds[0] + " ) ");
                }
                else
                {
                    whereCondition.Append(" (O.dataProviderId IN (" + speciesObservationDataProviderIds[0]);
                    for (index = 1; index < speciesObservationDataProviderIds.Count; index++)
                    {
                        whereCondition.Append(", " + speciesObservationDataProviderIds[index]);
                    }
                    whereCondition.Append("))");
                }
            }

            // Add condition for IsNaturalOccurrence.
            if (searchCriteria.IsIsNaturalOccurrenceSpecified)
            {
                AddWhereCondition(whereCondition);
                if (searchCriteria.IsNaturalOccurrence)
                {
                    whereCondition.Append(" (O.isNaturalOccurrence = 1) ");
                }
                else
                {
                    whereCondition.Append(" (O.isNaturalOccurrence = 0) ");
                }
            }

            if (searchCriteria.IsMaxProtectionLevelSpecified ||
                searchCriteria.IsMinProtectionLevelSpecified)
            {
                if ((searchCriteria.IsMaxProtectionLevelSpecified &&
                     searchCriteria.IsMinProtectionLevelSpecified &&
                     (searchCriteria.MaxProtectionLevel == searchCriteria.MinProtectionLevel)) ||
                    (searchCriteria.IsMaxProtectionLevelSpecified &&
                     !searchCriteria.IsMinProtectionLevelSpecified &&
                     (searchCriteria.MaxProtectionLevel == 1)))
                {
                    AddWhereCondition(whereCondition);
                    whereCondition.Append(" (O.protectionLevel = " + searchCriteria.MaxProtectionLevel + ") ");
                }
                else
                {
                    // Add condition for MaxProtectionLevel.
                    if (searchCriteria.IsMaxProtectionLevelSpecified)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(" (O.protectionLevel <= " + searchCriteria.MaxProtectionLevel + ") ");
                    }
                    // else This must be a call to method GetProtectedSpeciesObservationIndication().

                    // Add condition for MinProtectionLevel.
                    if (searchCriteria.IsMinProtectionLevelSpecified)
                    {
                        AddWhereCondition(whereCondition);
                        whereCondition.Append(" (O.protectionLevel >= " + searchCriteria.MinProtectionLevel + ") ");
                    }
                }
            }

            // Add condition for LocalityNameSearchString
            if (searchCriteria.LocalityNameSearchString.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.LocalityNameSearchString.GetWhereCondition("O.[locality]"));
            }

            // Add condition for ObserverSearchString
            if (searchCriteria.ObserverSearchString.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ObserverSearchString.GetWhereCondition("O.[recordedBy]"));
            }

            // Add condition for ObservationDateTime
            if (searchCriteria.ObservationDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ObservationDateTime.GetWhereCondition("O.[start]", "O.[end]"));
            }

            // Add condition for RegistrationDateTime
            if (searchCriteria.ReportedDateTime.IsNotNull())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.ReportedDateTime.GetWhereCondition("O.[reportedDate]"));
            }

            // Add condition for species activities.
            //if (searchCriteria.SpeciesActivityIds.IsNotEmpty())
            //{
            //    AddWhereCondition(whereCondition);
            //    if (searchCriteria.SpeciesActivityIds.Count == 1)
            //    {
            //        whereCondition.Append(" (O.activityId = " + searchCriteria.SpeciesActivityIds[0] + " ) ");
            //    }
            //    else
            //    {
            //        whereCondition.Append(" (O.activityId IN (" + searchCriteria.SpeciesActivityIds[0]);
            //        for (index = 1; index < searchCriteria.SpeciesActivityIds.Count; index++)
            //        {
            //            whereCondition.Append(", " + searchCriteria.SpeciesActivityIds[index]);
            //        }
            //        whereCondition.Append("))");
            //    }
            //}

            // Check for County and Province Regions for searching by id or name.
            if (searchCriteria.DataFields.IsNotEmpty() &&
                searchCriteria.DataFields.IsDataFieldSpecified(typeof(CountyProvinceRegionSearchType).ToString()))
            {
                String fieldNameEnding = CountyProvinceRegionSearchType.ByCoordinate.ToString();
                if (searchCriteria.DataFields.IsDataFieldSpecified(typeof(CountyProvinceRegionSearchType).ToString()))
                {
                    String fieldNameEndingValue = searchCriteria.DataFields.GetString(typeof(CountyProvinceRegionSearchType).ToString());
                    if (fieldNameEndingValue.Equals(CountyProvinceRegionSearchType.ByName.ToString()))
                    {
                        fieldNameEnding = CountyProvinceRegionSearchType.ByName.ToString();
                    }
                }

                // Append condition for CountyIdByCoordinate field
                List<WebDataField> countyIdByCoordinateFields = searchCriteria.DataFields.Where(x => x.Name.Equals("CountyId" + fieldNameEnding)).ToList();
                if (countyIdByCoordinateFields.IsNotEmpty())
                {
                    AppendConditionForWebDataFields(countyIdByCoordinateFields, "O.CountyId" + fieldNameEnding, whereCondition, false);
                }

                // Append condition for ProvinceIdByCoordinate field
                List<WebDataField> provinceIdByCoordinateFields = searchCriteria.DataFields.Where(x => x.Name.Equals("ProvinceId" + fieldNameEnding)).ToList();
                if (provinceIdByCoordinateFields.IsNotEmpty())
                {
                    AppendConditionForWebDataFields(provinceIdByCoordinateFields, "O.ProvinceId" + fieldNameEnding, whereCondition, countyIdByCoordinateFields.Any());
                }

                // Append Condition for CountyPartIdByCoordinate field
                List<WebDataField> countyPartIdByCoordinateFields = searchCriteria.DataFields.Where(x => x.Name.Equals("CountyPartId" + fieldNameEnding)).ToList();
                if (countyPartIdByCoordinateFields.IsNotEmpty())
                {
                    AppendConditionForWebDataFields(countyPartIdByCoordinateFields, "O.CountyPartId" + fieldNameEnding, whereCondition, countyIdByCoordinateFields.Any() || provinceIdByCoordinateFields.Any());
                }

                // Append condition for ProvincePartIdByCoordinate field
                List<WebDataField> provincePartIdByCoordinateFields = searchCriteria.DataFields.Where(x => x.Name.Equals("ProvincePartId" + fieldNameEnding)).ToList();
                if (provincePartIdByCoordinateFields.IsNotEmpty())
                {
                    AppendConditionForWebDataFields(
                        provincePartIdByCoordinateFields,
                        "O.ProvincePartId" + fieldNameEnding,
                        whereCondition,
                        countyIdByCoordinateFields.Any() || provinceIdByCoordinateFields.Any() ||
                        countyPartIdByCoordinateFields.Any());
                }
            }

#endif

            if (searchCriteria.FieldSearchCriteria.IsNotEmpty())
            {
                AddWhereCondition(whereCondition);
                whereCondition.Append(searchCriteria.FieldSearchCriteria.GetWhereCondition(GetFieldLogicalOperator(searchCriteria)));
            }

            return whereCondition.ToString();
        }

#if !SWEDISH_SPECIES_OBSERVATION_SOAP_SERVICE
        /// <summary>
        /// Test if all data providers has been selected in search criteria.
        /// </summary>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="context">Information of the web client.</param>
        /// <returns>True, if all data providers has been selected in search criteria.</returns>
        private static Boolean IsAllSpeciesObservationDataProvidersSelected(this WebSpeciesObservationSearchCriteria searchCriteria,
                                                                            WebServiceContext context)
        {
            Boolean isAllDataProviders;
            List<Int32> dataProviderIds;
            List<WebSpeciesObservationDataProvider> dataProviders;

            if (searchCriteria.DataProviderGuids.IsEmpty())
            {
                isAllDataProviders = false;
            }
            else
            {
                isAllDataProviders = true;
                dataProviderIds = searchCriteria.GetSpeciesObservationDataProviderIds(context);
                dataProviders = WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProviders(context);
                foreach (WebSpeciesObservationDataProvider dataProvider in dataProviders)
                {
                    if ((dataProvider.SpeciesObservationCount > 0) &&
                        !dataProviderIds.Contains(dataProvider.Id))
                    {
                        isAllDataProviders = false;
                        break;
                    }
                }
            }

            return isAllDataProviders;
        }
#endif

        /// <summary>
        /// Validates that the selected GridCellSize in combination with the Boundingbox doesn't generate too many grid cells and throws an ArgumentException otherwise 
        /// Due to complexity, the validation isn't performed if either LocalityNameSearchString, RegionGuids or TaxonIds is used
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="coordinateSystem"></param>
        /// <param name="gridSpecification"></param>
        public static void ValidateGridCellSize(this WebSpeciesObservationSearchCriteria searchCriteria, WebCoordinateSystem coordinateSystem, WebGridSpecification gridSpecification)
        {
            const int gridCountLimit = 200000;
            if (gridSpecification != null && gridSpecification.IsGridCellSizeSpecified)
            {
                var numberOfGrids = GetGridCellCount(searchCriteria, coordinateSystem, gridSpecification);
                if (numberOfGrids > gridCountLimit)
                {
                    throw new ArgumentException(string.Format("Grid cell size is too small, a cell size of {0} would have a result with at least {1,0} grid cells (current limit is {2}).", gridSpecification.GridCellSize, numberOfGrids, gridCountLimit));
                }
            }
        }

        /// <summary>
        /// Returns the grid cell count that the selected GridCellSize and Boundingbox should generate. This isn't intended to be used directly, use the ValidateGridCellSize instead.
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <param name="coordinateSystem"></param>
        /// <param name="gridSpecification"></param>
        /// <returns></returns>
        public static double GetGridCellCount(this WebSpeciesObservationSearchCriteria searchCriteria, WebCoordinateSystem coordinateSystem, WebGridSpecification gridSpecification)
        {
            if (!gridSpecification.IsGridCellSizeSpecified ||
                searchCriteria.Polygons.IsEmpty() ||
                searchCriteria.RegionGuids.IsNotEmpty() ||
                searchCriteria.LocalityNameSearchString.IsNotNull() ||
                searchCriteria.TaxonIds.IsNotEmpty())
            {
                return 0;
            }

            var totalConvertedPolygonArea = GetPolygonsArea(searchCriteria.Polygons, coordinateSystem, gridSpecification.GetWebCoordinateSystem());
            var totalGridSpecificationArea = gridSpecification.BoundingBox != null ? gridSpecification.BoundingBox.GetPolygon().GetGeometry().STArea().Value : double.MaxValue;

            //Always use the smallest area as limit
            var totalArea = totalConvertedPolygonArea < totalGridSpecificationArea ? totalConvertedPolygonArea : totalGridSpecificationArea;            

            var cellSize = (double)gridSpecification.GridCellSize;
            return Math.Ceiling(totalArea / (cellSize * cellSize));
        }      

        /// <summary>
        /// Calculate the area of the polygons.
        /// </summary>
        /// <param name="polygons">The polygons.</param>
        /// <param name="polygonsCoordinateSystem">The polygons coordinate system.</param>
        /// <param name="calculateCoordinateSystem">The calculate coordinate system. Should be RT90 or SWEREF99TM (längdriktig).</param>
        /// <returns>The are of the polygons in m2.</returns>
        private static double GetPolygonsArea(List<WebPolygon> polygons, WebCoordinateSystem polygonsCoordinateSystem, WebCoordinateSystem calculateCoordinateSystem)
        {
            double totalConvertedPolygonArea = 0;
            foreach (var polygon in polygons)
            {
                // Convert the polygon to a flat coordinate system.
                WebPolygon convertedPolygon = WebServiceData.CoordinateConversionManager.GetConvertedPolygon(
                    polygon,
                    polygonsCoordinateSystem,
                    calculateCoordinateSystem);

                totalConvertedPolygonArea += convertedPolygon.GetGeometry().STArea().Value;
            }

            return totalConvertedPolygonArea;
        }
    }
}
