using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Database;
using ArtDatabanken.WebService.SpeciesObservation.Data;
using ArtDatabanken.WebService.SpeciesObservation.Database;
using ArtDatabanken.WebService.SwedishSpeciesObservationService.Database;
using Microsoft.SqlServer.Types;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Manager of species observation information.
    /// </summary>
    public static class SpeciesObservationManager
    {
        /// <summary>
        /// Check if user has access rights to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCore">A species observation in Darwin Core compatible format.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebDarwinCore webDarwinCore)
        {
            Boolean hasAccessRight;

            hasAccessRight = false;
            foreach (WebRole role in context.CurrentRoles)
            {
                if (CheckAccessRights(context,
                                      webDarwinCore,
                                      role))
                {
                    hasAccessRight = true;
                    break;
                }
            }

            return hasAccessRight;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCore">Species observation in Darwin Core compatible format.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebDarwinCore webDarwinCore,
                                                 WebAuthority authority)
        {
            List<WebRegionGeography> regionsGeography;
            Dictionary<Int32, WebTaxon> taxa;
            WebPoint point;

            // Test if authority is related to species observations.
            if (authority.Identifier != AuthorityIdentifier.Sighting.ToString())
            {
                return false;
            }

            // Test if authority has enough protection level.
            if (authority.MaxProtectionLevel < webDarwinCore.Conservation.ProtectionLevel)
            {
                return false;
            }

            // Test if species observation is inside regions.
            if (authority.RegionGUIDs.IsNotEmpty())
            {
                point = new WebPoint(webDarwinCore.Location.CoordinateX,
                                     webDarwinCore.Location.CoordinateY);
                regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                           authority.RegionGUIDs,
                                                                                           WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem);
                if (!regionsGeography.IsPointInsideGeometry(context,
                                                            WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem,
                                                            point))
                {
                    return false;
                }
            }

            // Test if species observation belongs to specified taxa.
            if (authority.TaxonGUIDs.IsNotEmpty())
            {
                taxa = WebServiceData.TaxonManager.GetTaxaByAuthority(context, authority);
                if (!taxa.ContainsKey(webDarwinCore.Taxon.DyntaxaTaxonID))
                {
                    return false;
                }
            }

            // Species observation has passed all tests.
            // User has access right to this species observation.
            return true;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCore">Species observation in Darwin Core compatible format.</param>
        /// <param name="role">Check access right in this role.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckAccessRights(WebServiceContext context,
                                                 WebDarwinCore webDarwinCore,
                                                 WebRole role)
        {
            foreach (WebAuthority authority in role.Authorities)
            {
                if (CheckAccessRights(context,
                                      webDarwinCore,
                                      authority))
                {
                    return true;
                }
            }

            return false;
        }

        ///// <summary>
        ///// Check that species observation search criteria is valid.
        ///// This method should only be used together with Elasticsearch.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="searchCriteria">Search criteria.</param>
        ///// <param name="coordinateSystem">
        ///// Coordinate system used in geometry search criteria
        ///// and returned species observations.
        ///// </param>
        //private static void CheckData(WebServiceContext context,
        //                              WebSpeciesObservationSearchCriteria searchCriteria,
        //                              WebCoordinateSystem coordinateSystem)
        //{
        //    Dictionary<String, WebSpeciesObservationField> mapping;

        //    searchCriteria.CheckNotNull("searchCriteria");
        //    mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, context.GetElastisearchSpeciesObservationProxy());
        //    searchCriteria.CheckData(context, true, mapping);
        //    searchCriteria.Polygons = WebSpeciesObservationServiceData.SpeciesObservationManager.ConvertToElasticSearchCoordinates(context,
        //                                                                searchCriteria.Polygons,
        //                                                                searchCriteria.RegionGuids,
        //                                                                coordinateSystem);
        //    searchCriteria.TaxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, true);
        //}

        /// <summary>
        /// Check if user has access rights to a species observation
        /// for usage in method GetProtectedSpeciesObservationIndication.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCore">A species observation in Darwin Core compatible format.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckIndicationAccessRights(WebServiceContext context,
                                                           WebDarwinCore webDarwinCore)
        {
            Boolean hasAccessRight;

            if (context.GetUser().Type == UserType.Application)
            {
                hasAccessRight = true;
            }
            else
            {
                hasAccessRight = false;
                foreach (WebRole role in context.CurrentRoles)
                {
                    foreach (WebAuthority authority in role.Authorities)
                    {
                        if (CheckIndicationAccessRights(context,
                                                        webDarwinCore,
                                                        authority))
                        {
                            hasAccessRight = true;
                            break;
                        }
                    }
                }
            }

            return hasAccessRight;
        }

        /// <summary>
        /// Check if user has access right to a species observation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCore">Species observation in Darwin Core compatible format.</param>
        /// <param name="authority">Check access right in this authority.</param>
        /// <returns>True if user has access right to provided observation.</returns>
        private static Boolean CheckIndicationAccessRights(WebServiceContext context,
                                                           WebDarwinCore webDarwinCore,
                                                           WebAuthority authority)
        {
            List<WebRegionGeography> regionsGeography;
            WebPoint point;

            // Test if authority is related to species observation indication.
            if (authority.Identifier != AuthorityIdentifier.SightingIndication.ToString())
            {
                return false;
            }

            // Test if species observation is inside regions.
            if (authority.RegionGUIDs.IsNotEmpty())
            {
                point = new WebPoint(webDarwinCore.Location.CoordinateX,
                                     webDarwinCore.Location.CoordinateY);
                regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
                                                                                           authority.RegionGUIDs,
                                                                                           WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem);
                if (!regionsGeography.IsPointInsideGeometry(context,
                                                            WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem,
                                                            point))
                {
                    return false;
                }
            }

            // Species observation has passed all tests.
            // User has access right to this species observation.
            return true;
        }

        /// <summary>
        /// Check that species observation search criteria is valid.
        /// To use in method GetProtectedSpeciesObservationIndication().
        /// Do not use this method in any other context.
        /// </summary>
        /// <param name="context"> Web service request context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <param name="isElasticsearchUsed">
        /// Indicates if Elsticsearch is used.
        /// Some handling of search criteria differs depending
        /// on which data source that will be used.
        /// </param>
        private static void CheckSearchCriteria(WebServiceContext context,
                                                WebSpeciesObservationSearchCriteria searchCriteria,
                                                Boolean isElasticsearchUsed)
        {
            Boolean isBirdNestActivityFound, isRegionFound;
            DateTime latestBegin;
            Int32 index;
            List<WebRegion> regions;

            searchCriteria.CheckNotNull("searchCriteria");

            // Check that at least one geometry has been specified.
            if (searchCriteria.BoundingBox.IsNull() &&
                searchCriteria.Polygons.IsEmpty() &&
                searchCriteria.RegionGuids.IsEmpty())
            {
                throw new ArgumentException("WebSpeciesObservationSearchCriteria: At least one of BoundingBox, Polygons and RegionGuids must be specified.");
            }

            // Check search criteria Accuracy.
            if (searchCriteria.IsAccuracySpecified)
            {
                // Accuracy must be zero or larger.
                if (searchCriteria.Accuracy < 0)
                {
                    throw new ArgumentException("WebSpeciesObservationSearchCriteria: Accuray must be zero or larger. Current value = " + searchCriteria.Accuracy);
                }
            }

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

            // Check search criteria BoundingBox.
            if (searchCriteria.BoundingBox.IsNotNull())
            {
                searchCriteria.BoundingBox.CheckData();

                // Convert bounding box into a polygon.
                if (searchCriteria.Polygons.IsNull())
                {
                    searchCriteria.Polygons = new List<WebPolygon>();
                }

                searchCriteria.Polygons.Add(searchCriteria.BoundingBox.GetPolygon());
                searchCriteria.BoundingBox = null;
            }

            searchCriteria.ChangeDateTime = null;

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

            searchCriteria.FieldSearchCriteria = null;
            searchCriteria.IncludeNeverFoundObservations = false;
            searchCriteria.IncludeNotRediscoveredObservations = false;
            searchCriteria.IncludePositiveObservations = true;
            searchCriteria.IncludeRedListCategories = null;
            searchCriteria.IncludeRedlistedTaxa = false;
            searchCriteria.IsAccuracyConsidered = true;
            searchCriteria.IsDisturbanceSensitivityConsidered = true;
            searchCriteria.IsIsNaturalOccurrenceSpecified = true;
            searchCriteria.IsNaturalOccurrence = true;
            searchCriteria.LocalityNameSearchString = null;
            searchCriteria.IsMaxProtectionLevelSpecified = false;

            // Set search criteria MinProtectionLevel.
            searchCriteria.IsMinProtectionLevelSpecified = (context.GetUser().Type != UserType.Application);
            searchCriteria.MinProtectionLevel = GetMinProtectionLevel(context);

            // Check search criteria ObservationDateTime.
            searchCriteria.ObservationDateTime.CheckData(true);
            if (searchCriteria.ObservationDateTime.IsNotNull())
            {
                latestBegin = new DateTime(DateTime.Now.Year - 1, 1, 1);
                if (latestBegin < searchCriteria.ObservationDateTime.Begin)
                {
                    searchCriteria.ObservationDateTime.Begin = latestBegin;
                }

                searchCriteria.ObservationDateTime.End = DateTime.Now + new TimeSpan(366, 0, 0, 0);
                searchCriteria.ObservationDateTime.PartOfYear = null;
            }

            searchCriteria.ObserverIds = null;
            searchCriteria.ObserverSearchString = null;

            // Check search criteria Polygons.
            if (searchCriteria.Polygons.IsNotEmpty())
            {
                foreach (WebPolygon polygon in searchCriteria.Polygons)
                {
                    polygon.CheckData();
                }
            }

            searchCriteria.ProjectGuids = null;

            // Check search criteria RegionGuids.
            if (searchCriteria.RegionGuids.IsNotEmpty())
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

            searchCriteria.ReportedDateTime = null;
            searchCriteria.SpeciesActivityIds = null;
            searchCriteria.TaxonIds = GetProtectedSpeciesObservationIndicationTaxonIds(context, isElasticsearchUsed);
            searchCriteria.ValidationStatusIds = null;
        }

        /// <summary>
        /// Check if extra species observation fields should be removed.
        /// These extra fields has been added for the species
        /// observation handling to work properly.
        /// </summary>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="speciesObservationSpecification">Specification of which fields that should be included in the result.</param>
        private static void CheckSpeciesObservationFields(List<WebSpeciesObservation> speciesObservations,
                                                          WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            Int32 fieldIndex;
            List<WebSpeciesObservationFieldSpecification> necessaryFieldSpecifications,
                                                          removeFieldSpecifications,
                                                          requestedFieldSpecifications;
            WebSpeciesObservationFieldSpecification coordinateSystemFieldSpecification;

            requestedFieldSpecifications = speciesObservationSpecification.GetFields();
            if (speciesObservations.IsEmpty() ||
                requestedFieldSpecifications.IsEmpty())
            {
                // Nothing to check.
                return;
            }

            // Get specification for fields that should be removed.
            necessaryFieldSpecifications = GetSpeciesObservationFields();
            removeFieldSpecifications = new List<WebSpeciesObservationFieldSpecification>();
            foreach (WebSpeciesObservationFieldSpecification necessaryFieldSpecification in necessaryFieldSpecifications)
            {
                if (!(requestedFieldSpecifications.ContainsFieldSpecification(necessaryFieldSpecification)))
                {
                    removeFieldSpecifications.Add(necessaryFieldSpecification);
                }
            }

            coordinateSystemFieldSpecification = new WebSpeciesObservationFieldSpecification();
            coordinateSystemFieldSpecification.Class = new WebSpeciesObservationClass();
            coordinateSystemFieldSpecification.Class.Id = SpeciesObservationClassId.Location;
            coordinateSystemFieldSpecification.Property = new WebSpeciesObservationProperty();
            coordinateSystemFieldSpecification.Property.Id = SpeciesObservationPropertyId.CoordinateSystemWkt;
            if (!(requestedFieldSpecifications.ContainsFieldSpecification(coordinateSystemFieldSpecification)))
            {
                removeFieldSpecifications.Add(coordinateSystemFieldSpecification);
            }

            // Remove fields that has not been requested.
            if (removeFieldSpecifications.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                {
                    for (fieldIndex = speciesObservation.Fields.Count - 1; fieldIndex >= 0; fieldIndex--)
                    {
                        if (removeFieldSpecifications.ContainsFieldSpecification(speciesObservation.Fields[fieldIndex].ClassIdentifier,
                                                                                 speciesObservation.Fields[fieldIndex].PropertyIdentifier))
                        {
                            speciesObservation.Fields.RemoveAt(fieldIndex);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if retrieval of species observations should be logged.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCores">Species observations in Darwin Core compatible format.</param>
        /// <param name="method">Used method in web service.</param>
        private static void CheckSpeciesObservationLog(WebServiceContext context,
                                                       List<WebDarwinCore> webDarwinCores,
                                                       String method)
        {
            Dictionary<Int32, Int32> protectionInformation;
            Int32 taxonId;
            List<WebDarwinCore> logSpeciesObservations;

            protectionInformation = GetProtectionInformation(context);
            logSpeciesObservations = new List<WebDarwinCore>();
            if (webDarwinCores.IsNotEmpty())
            {
                foreach (WebDarwinCore speciesObservation in webDarwinCores)
                {
                    taxonId = speciesObservation.Taxon.DyntaxaTaxonID;
                    if ((speciesObservation.Conservation.ProtectionLevel > 1) &&
                        protectionInformation.ContainsKey(taxonId) &&
                        (protectionInformation[taxonId] > 1))
                    {
                        // Log retrieval of protected species observation.
                        logSpeciesObservations.Add(speciesObservation);
                    }
                }
            }

            if (logSpeciesObservations.IsNotEmpty())
            {
                LogSpeciesObservations(context, logSpeciesObservations, protectionInformation, method);
            }
        }

        /// <summary>
        /// Check if retrieval of species observations should be logged.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observations in Darwin Core compatible format.</param>
        /// <param name="method">Used method in web service.</param>
        private static void CheckSpeciesObservationLog(WebServiceContext context,
                                                       List<WebSpeciesObservation> speciesObservations,
                                                       String method)
        {
            Dictionary<Int32, Int32> protectionInformation;
            WebSpeciesObservationField protectionLevelField, taxonField;
            Int32 protectionLevel, taxonId;
            List<WebSpeciesObservation> logSpeciesObservations;

            protectionInformation = GetProtectionInformation(context);
            logSpeciesObservations = new List<WebSpeciesObservation>();
            if (speciesObservations.IsNotEmpty())
            {
                foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                {
                    protectionLevelField = speciesObservation.Fields.GetField(SpeciesObservationClassId.Conservation.ToString(),
                                                                              SpeciesObservationPropertyId.ProtectionLevel.ToString());
                    protectionLevel = protectionLevelField.Value.WebParseInt32();
                    taxonField = speciesObservation.Fields.GetField(SpeciesObservationClassId.Taxon.ToString(),
                                                                    SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                    taxonId = taxonField.Value.WebParseInt32();
                    if ((protectionLevel > 1) &&
                        protectionInformation.ContainsKey(taxonId) &&
                        (protectionInformation[taxonId] > 1))
                    {
                        // Log retrieval of protected species observation.
                        logSpeciesObservations.Add(speciesObservation);
                    }
                }
            }

            if (logSpeciesObservations.IsNotEmpty())
            {
                LogSpeciesObservations(context, logSpeciesObservations, protectionInformation, method);
            }
        }

        /// <summary>
        /// Convert coordinates from one coordinate system to a
        /// requested coordinate system in species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="requestedCoordinateSystem">Which coordinate system the coordinates should be converted to.</param>
        private static void ConvertToRequestedCoordinates(WebServiceContext context,
                                                          List<WebDarwinCore> speciesObservations,
                                                          WebCoordinateSystem requestedCoordinateSystem)
        {
            Dictionary<String, WebPoint> points;
            String pointCachKey;
            WebPoint fromPoint, toPoint;

            // If requested coordinate system is same as database
            // coordinate system, return with no change.
            if (requestedCoordinateSystem.GetWkt().ToLower() !=
                WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem.GetWkt().ToLower() &&
                speciesObservations.IsNotEmpty())
            {
                points = new Dictionary<String, WebPoint>();

                foreach (WebDarwinCore speciesObservation in speciesObservations)
                {
                    pointCachKey = (speciesObservation.Location.CoordinateX + "_" + speciesObservation.Location.CoordinateY);

                    if (points.ContainsKey(pointCachKey))
                    {
                        toPoint = points[pointCachKey];
                    }
                    else
                    {
                        fromPoint = new WebPoint(speciesObservation.Location.CoordinateX,
                                                 speciesObservation.Location.CoordinateY);

                        toPoint = WebServiceData.CoordinateConversionManager.GetConvertedPoint(context,
                                                                                               fromPoint,
                                                                                               WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem,
                                                                                               requestedCoordinateSystem);
                        points.Add(pointCachKey, toPoint);
                    }

                    speciesObservation.Location.CoordinateX = toPoint.X;
                    speciesObservation.Location.CoordinateY = toPoint.Y;
                }
            }
        }

        /// <summary>
        /// Convert coordinates from one coordinate system to a
        /// requested coordinate system in species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="requestedCoordinateSystem">Which coordinate system the coordinates should be converted to.</param>
        private static void ConvertToRequestedCoordinates(WebServiceContext context,
                                                          List<WebSpeciesObservation> speciesObservations,
                                                          WebCoordinateSystem requestedCoordinateSystem)
        {
            Dictionary<String, WebPoint> points;
            Int32 coordinateX, coordinateY;
            String pointCachKey;
            WebPoint fromPoint, toPoint;
            WebSpeciesObservationField coordinateSystemField,
                                       coordinateXField,
                                       coordinateYField;

            if (speciesObservations.IsNotEmpty())
            {
                // If requested coordinate system is same as database
                // coordinate system, return with no change.
                if (requestedCoordinateSystem.GetWkt().ToLower()
                    != WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem.GetWkt().ToLower())
                {
                    points = new Dictionary<String, WebPoint>();

                    foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                    {
                        coordinateXField =
                            speciesObservation.Fields.GetField(
                                SpeciesObservationClassId.Location.ToString(),
                                SpeciesObservationPropertyId.CoordinateX.ToString());
                        coordinateX = coordinateXField.Value.WebParseInt32();
                        coordinateYField =
                            speciesObservation.Fields.GetField(
                                SpeciesObservationClassId.Location.ToString(),
                                SpeciesObservationPropertyId.CoordinateY.ToString());
                        coordinateY = coordinateYField.Value.WebParseInt32();
                        pointCachKey = (coordinateX + "_" + coordinateY);

                        if (points.ContainsKey(pointCachKey))
                        {
                            toPoint = points[pointCachKey];
                        }
                        else
                        {
                            fromPoint = new WebPoint(coordinateX, coordinateY);

                            toPoint = WebServiceData.CoordinateConversionManager.GetConvertedPoint(
                                context,
                                fromPoint,
                                WebServiceData.SpeciesObservationManager.SpeciesObservationCoordinateSystem,
                                requestedCoordinateSystem);
                            points.Add(pointCachKey, toPoint);
                        }

                        coordinateXField.Type = WebDataType.Float64;
                        coordinateXField.Value = toPoint.X.WebToString();
                        coordinateYField.Type = WebDataType.Float64;
                        coordinateYField.Value = toPoint.Y.WebToString();
                    }
                }
                else
                {
                    // Change data type on fields CoordinateX and CoordinateY.
                    foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                    {
                        coordinateXField = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                                              SpeciesObservationPropertyId.CoordinateX.ToString());
                        coordinateX = coordinateXField.Value.WebParseInt32();
                        coordinateYField = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                                              SpeciesObservationPropertyId.CoordinateY.ToString());
                        coordinateY = coordinateYField.Value.WebParseInt32();
                        coordinateXField.Type = WebDataType.Float64;
                        coordinateXField.Value = ((Double)coordinateX).WebToString();
                        coordinateYField.Type = WebDataType.Float64;
                        coordinateYField.Value = ((Double)coordinateY).WebToString();
                    }
                }

                // Add coordinate system field.
                coordinateSystemField = new WebSpeciesObservationField();
                coordinateSystemField.ClassIdentifier = SpeciesObservationClassId.Location.ToString();
                coordinateSystemField.PropertyIdentifier = SpeciesObservationPropertyId.CoordinateSystemWkt.ToString();
                coordinateSystemField.Type = WebDataType.String;
                coordinateSystemField.Value = requestedCoordinateSystem.GetWkt();
                foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                {
                    speciesObservation.Fields.Add(coordinateSystemField);
                }
            }
        }

        ///// <summary>
        ///// Convert polygons from provided coordinate
        ///// system to a Elasticsearch coordinate system.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="inputPolygons">Input polygons.</param>
        ///// <param name="regionGuids">Region GUIDs.</param>
        ///// <param name="inputCoordinateSystem">Which coordinate system the coordinates should be converted from.</param>
        ///// <returns>Polygons in Elasticsearch coordinate system.</returns>
        //private static List<WebPolygon> ConvertToElasticSearchCoordinates(WebServiceContext context,
        //                                                                  List<WebPolygon> inputPolygons,
        //                                                                  List<String> regionGuids,
        //                                                                  WebCoordinateSystem inputCoordinateSystem)
        //{
        //    List<WebPolygon> outputPolygons;
        //    List<WebRegionGeography> regionsGeography;
        //    WebCoordinateSystem speciesObservationCoordinateSystem;

        //    outputPolygons = null;
        //    speciesObservationCoordinateSystem = new WebCoordinateSystem();
        //    speciesObservationCoordinateSystem.Id = CoordinateSystemId.WGS84;
        //    if (inputPolygons.IsNotEmpty())
        //    {
        //        if (inputCoordinateSystem.GetWkt().ToLower() == speciesObservationCoordinateSystem.GetWkt().ToLower())
        //        {
        //            outputPolygons = inputPolygons;
        //        }
        //        else
        //        {
        //            outputPolygons = WebServiceData.CoordinateConversionManager.GetConvertedPolygons(inputPolygons,
        //                                                                                             inputCoordinateSystem,
        //                                                                                             speciesObservationCoordinateSystem);
        //        }
        //    }

        //    if (regionGuids.IsNotEmpty())
        //    {
        //        regionsGeography = WebServiceData.RegionManager.GetRegionsGeographyByGuids(context,
        //                                                                                   regionGuids,
        //                                                                                   speciesObservationCoordinateSystem);
        //        if (outputPolygons.IsNull())
        //        {
        //            outputPolygons = new List<WebPolygon>();
        //        }

        //        foreach (WebRegionGeography regionGeography in regionsGeography)
        //        {
        //            // ReSharper disable once PossibleNullReferenceException
        //            outputPolygons.AddRange(regionGeography.MultiPolygon.Polygons);
        //        }
        //    }

        //    return outputPolygons;
        //}

        /// <summary>
        /// Get taxa that is subject of an action plan.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Ids for taxa that is subject of an action plan.</returns>
        private static DataIdInt32List GetActionPlanTaxonIds(WebServiceContext context)
        {
            Dictionary<Int32, TaxonInformation> taxonInformationCache;
            DataIdInt32List actionPlanTaxonIds;

            taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
            actionPlanTaxonIds = new DataIdInt32List(true);
            foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
            {
                if (0 < taxonInformation.ActionPlanId)
                {
                    actionPlanTaxonIds.Add(taxonInformation.DyntaxaTaxonId);
                }
            }

            return actionPlanTaxonIds;
        }

        /// <summary>
        /// Convert one species observation from JSON to
        /// class WebSpeciesObservation.
        /// </summary>
        /// <param name="speciesObservations">Species observation is stored in this list.</param>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetDarwinCore(List<WebDarwinCore> speciesObservations,
                                           String speciesObservationsJson,
                                           Dictionary<String, WebSpeciesObservationField> mapping,
                                           Int32 startIndex)
        {
            Boolean isFieldFound;
            WebDarwinCore speciesObservation;

            speciesObservation = new WebDarwinCore();
            speciesObservations.Add(speciesObservation);

            // Skip general part.
            startIndex = speciesObservationsJson.IndexOf("_source", startIndex, StringComparison.Ordinal);
            startIndex = speciesObservationsJson.IndexOf("{", startIndex, StringComparison.Ordinal) + 1;

            do
            {
                startIndex = GetDarwinCoreField(out isFieldFound,
                                                speciesObservation,
                                                speciesObservationsJson,
                                                mapping,
                                                startIndex);
            }
            while (isFieldFound);

            return startIndex;
        }

        /// <summary>
        /// Get all county regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All county regions</returns>
        public static List<WebRegion> GetCountyRegions(WebServiceContext context)
        {
            List<WebRegion> regions;
            String cacheKey;
            WebRegion region;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationCountyRegionsCacheKey;
            regions = (List<WebRegion>) (context.GetCachedObject(cacheKey));

            if (regions.IsEmpty())
            {
                // Data not in cache.
                regions = new List<WebRegion>();

                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetCountyRegions())
                {
                    while (dataReader.Read())
                    {
                        region = new WebRegion();
                        region.LoadData(dataReader);
                        regions.Add(region);
                    }
                }
                // Add information to cache.
                context.AddCachedObject(cacheKey,
                    regions,
                    DateTime.Now + new TimeSpan(1, 0, 0, 0),
                    CacheItemPriority.High);
            }
            return regions;
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in returned species observations.
        /// </param>
        /// <returns>Species observations.</returns>
        public static WebDarwinCoreInformation GetDarwinCoreByIds(WebServiceContext context,
                                                                  List<Int64> speciesObservationIds,
                                                                  WebCoordinateSystem coordinateSystem)
        {
            Int32 maxProtectionLevel;
            WebDarwinCoreInformation webDarwinCoreInformation;

            // Check access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check data.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            coordinateSystem.CheckData();
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");
            if (speciesObservationIds.Count > Settings.Default.MaxSpeciesObservationWithInformation)
            {
                throw new ArgumentException("Too many species observations was retrieved!, Limit is set to " +
                                            Settings.Default.MaxSpeciesObservationWithInformation + " observations with information.");
            }

            // Get species observations.
            maxProtectionLevel = context.CurrentRoles.GetMaxProtectionLevel();
            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetDarwinCoreByIds(speciesObservationIds, maxProtectionLevel))
            {
                webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader);
            }

            GetExternalInformation(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            ConvertToRequestedCoordinates(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, webDarwinCoreInformation.SpeciesObservations, "GetDarwinCoreByIds");
            return webDarwinCoreInformation;
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <returns>Species observations.</returns>
        public static WebDarwinCoreInformation GetDarwinCoreByIdsElasticsearch(WebServiceContext context,
                                                                               List<Int64> speciesObservationIds,
                                                                               WebCoordinateSystem coordinateSystem)
        {
            Int32 index;
            StringBuilder filter;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            coordinateSystem.CheckData();
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");
            if (speciesObservationIds.Count > Settings.Default.MaxSpeciesObservationWithInformation)
            {
                throw new ArgumentException("Too many species observations was retrieved!, Limit is set to " +
                                            Settings.Default.MaxSpeciesObservationWithInformation + " observations with information.");
            }

            // Get species observation filter.
            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"size\": " + speciesObservationIds.Count);
            filter.Append(", " + GetDarwinCoreFields(context));
            filter.Append(", \"filter\": {\"bool\":{ \"must\" : [");
            filter.Append("{ \"terms\": {");
            filter.Append(" \"");
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                filter.Append(elastisearch.GetFieldName(SpeciesObservationClassId.DarwinCore, SpeciesObservationPropertyId.Id));
            }
            
            filter.Append("\":[");
            filter.Append(speciesObservationIds[0].WebToString());
            for (index = 1; index < speciesObservationIds.Count; index++)
            {
                filter.Append(", " + speciesObservationIds[index].WebToString());
            }

            filter.Append("]}}");
            filter.Append(", " + context.CurrentRoles.GetSpeciesObservationAccessRightsJson(context));
            filter.Append("]}}}");

            return GetDarwinCoreInformationElasticsearch(context,
                                                         null,
                                                         coordinateSystem,
                                                         filter.ToString(),
                                                         "GetDarwinCoreByIdsElasticsearch");
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static WebDarwinCoreInformation GetDarwinCoreBySearchCriteria(WebServiceContext context,
                                                                             WebSpeciesObservationSearchCriteria searchCriteria,
                                                                             WebCoordinateSystem coordinateSystem,
                                                                             List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            String geometryWhereCondition, joinCondition,
                   sortOrderString, whereCondition;
            WebDarwinCoreInformation webDarwinCoreInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            coordinateSystem.CheckData();
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);

            // Get the conditions.
            geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
            joinCondition = searchCriteria.GetJoinCondition();
            sortOrderString = sortOrder.GetSqlSortOrder(context, searchCriteria, false);
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);

            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetDarwinCoreBySearchCriteria(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                 searchCriteria.GetRegionIds(context),
                                                                                                                 WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false),
                                                                                                                 joinCondition,
                                                                                                                 whereCondition,
                                                                                                                 geometryWhereCondition,
                                                                                                                 sortOrderString))
            {
                webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader);
            }

            GetExternalInformation(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            ConvertToRequestedCoordinates(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, webDarwinCoreInformation.SpeciesObservations, "GetDarwinCoreBySearchCriteria");
            return webDarwinCoreInformation;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem"> Coordinate system used in geometry search criteria
        /// and returned species observations. </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static WebDarwinCoreInformation GetDarwinCoreBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                          WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                          WebCoordinateSystem coordinateSystem,
                                                                                          List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            StringBuilder filter;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);
            sortOrder.CheckData();

            // Get filter.
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
            }

            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"size\": " + Settings.Default.MaxSpeciesObservationWithInformation.WebToString());
            filter.Append(", " + GetDarwinCoreFields(context));
            filter.Append(", " + searchCriteria.GetFilter(context, false));
            filter.Append(", " + sortOrder.GetSortOrderJson(mapping));
            filter.Append("}");

            return GetDarwinCoreInformationElasticsearch(context,
                                                         searchCriteria,
                                                         coordinateSystem,
                                                         filter.ToString(),
                                                         "GetDarwinCoreBySearchCriteriaElasticsearch");
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Species observations are returned in a format
        /// that is compatible with Darwin Core 1.5.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static List<WebDarwinCore> GetDarwinCoreBySearchCriteriaPage(WebServiceContext context,
                                                                            WebSpeciesObservationSearchCriteria searchCriteria,
                                                                            WebCoordinateSystem coordinateSystem,
                                                                            WebSpeciesObservationPageSpecification pageSpecification)
        {
            Int64 endRow, startRow;
            String geometryWhereCondition, joinCondition,
                   sortOrder, whereCondition;
            WebDarwinCoreInformation webDarwinCoreInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            coordinateSystem.CheckData();
            pageSpecification.CheckData();
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);

            // Get search parameters.
            geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
            joinCondition = searchCriteria.GetJoinCondition();
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
            sortOrder = pageSpecification.GetSqlSortOrder(context, searchCriteria);
            startRow = pageSpecification.Start;
            endRow = pageSpecification.GetEndRow();

            if (context.CurrentRoles.IsSimpleSpeciesObservationAccessRights())
            {
                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetDarwinCoreBySearchCriteriaPage(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                         searchCriteria.GetRegionIds(context),
                                                                                                                         WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false),
                                                                                                                         joinCondition,
                                                                                                                         whereCondition,
                                                                                                                         geometryWhereCondition,
                                                                                                                         sortOrder,
                                                                                                                         startRow,
                                                                                                                         endRow))
                {
                    webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader);
                }
            }
            else
            {
                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetDarwinCoreBySearchCriteria(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                     searchCriteria.GetRegionIds(context),
                                                                                                                     WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false),
                                                                                                                     joinCondition,
                                                                                                                     whereCondition,
                                                                                                                     geometryWhereCondition,
                                                                                                                     sortOrder))
                {
                    webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader, startRow, endRow);
                }
            }

            GetExternalInformation(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            ConvertToRequestedCoordinates(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, webDarwinCoreInformation.SpeciesObservations, "GetDarwinCoreBySearchCriteriaPage");
            return webDarwinCoreInformation.SpeciesObservations;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static List<WebDarwinCore> GetDarwinCoreBySearchCriteriaPageElasticsearch(WebServiceContext context,
                                                                                         WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                         WebCoordinateSystem coordinateSystem,
                                                                                         WebSpeciesObservationPageSpecification pageSpecification)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            StringBuilder filter;
            WebDarwinCoreInformation speciesObservationInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            pageSpecification.CheckData();
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);

            // Get filter.
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
            }

            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"from\": " + (pageSpecification.Start - 1).WebToString());
            filter.Append(", \"size\": " + pageSpecification.Size);
            filter.Append(", " + GetDarwinCoreFields(context));
            filter.Append(", " + searchCriteria.GetFilter(context, false));
            filter.Append(", " + pageSpecification.SortOrder.GetSortOrderJson(mapping));
            filter.Append("}");

            // Get species observations.
            speciesObservationInformation = GetDarwinCoreInformationElasticsearch(context,
                                                                                  null,
                                                                                  coordinateSystem,
                                                                                  filter.ToString(),
                                                                                  "GetDarwinCoreBySearchCriteriaPageElasticsearch");
            return speciesObservationInformation.SpeciesObservations;
        }

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="changedFrom">Start date and time for changes that should be returned. </param>
        /// <param name="isChangedFromSpecified">Indicates if parameter changedFrom should be used. </param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="isChangedToSpecified">Indicates if parameter changedTo should be used. </param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="isChangedIdSpecified">Indicates if parameter changeId should be used. </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="searchCriteria">
        /// Only species observations that matches the search 
        /// criteria are included in the returned information.
        /// This parameter is optional and may be null.
        /// There is no check on search criteria for
        /// deleted species observations.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations. </param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        public static WebDarwinCoreChange GetDarwinCoreChange(WebServiceContext context,
                                                              DateTime changedFrom,
                                                              Boolean isChangedFromSpecified,
                                                              DateTime changedTo,
                                                              Boolean isChangedToSpecified,
                                                              Int64 changeId,
                                                              Boolean isChangedIdSpecified,
                                                              Int64 maxReturnedChanges,
                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                              WebCoordinateSystem coordinateSystem)
        {
            String joinCondition, whereCondition;
            WebDarwinCoreInformation webDarwinCoreInformation;
            List<SqlGeometry> polygonsAsGeometry;
            List<Int32> regionIds;
            List<Int32> taxonIds;
            String speciesObservationId;
            WebDarwinCoreChange webDarwinCoreChange;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckManualDatabaseUpdate(context);
            coordinateSystem.CheckData();
            if (searchCriteria.IsNull())
            {
                searchCriteria = new WebSpeciesObservationSearchCriteria();
            }

            searchCriteria.CheckData(context);

            // Get search parameters.
            joinCondition = searchCriteria.GetJoinCondition();
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
            polygonsAsGeometry = searchCriteria.GetPolygonsAsGeometry(coordinateSystem);
            regionIds = searchCriteria.GetRegionIds(context);
            taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);

            webDarwinCoreChange = new WebDarwinCoreChange();
            webDarwinCoreChange.UpdatedSpeciesObservations = new List<WebDarwinCore>();
            webDarwinCoreChange.DeletedSpeciesObservationGuids = new List<String>();
            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetDarwinCoreChange(changedFrom,
                                                                                                       isChangedFromSpecified,
                                                                                                       changedTo,
                                                                                                       isChangedToSpecified,
                                                                                                       changeId,
                                                                                                       isChangedIdSpecified,
                                                                                                       maxReturnedChanges,
                                                                                                       polygonsAsGeometry,
                                                                                                       regionIds,
                                                                                                       taxonIds,
                                                                                                       joinCondition,
                                                                                                       whereCondition))
            {
                // CREATED
                webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader);
                webDarwinCoreChange.CreatedSpeciesObservations = webDarwinCoreInformation.SpeciesObservations;

                // UPDATED
                dataReader.NextResultSet();
                webDarwinCoreInformation = GetDarwinCoreInformation(context, dataReader);
                webDarwinCoreChange.UpdatedSpeciesObservations = webDarwinCoreInformation.SpeciesObservations;

                // DELETED
                dataReader.NextResultSet();
                while (dataReader.Read())
                {
                    speciesObservationId = dataReader.GetString("occuranceId");
                    webDarwinCoreChange.DeletedSpeciesObservationGuids.Add(speciesObservationId);
                }

                // MAX CHANGE ID
                dataReader.NextResultSet();
                while (dataReader.Read())
                {
                    webDarwinCoreChange.MaxChangeId = dataReader.GetInt64(0, 0);
                }

                // MORE
                dataReader.NextResultSet();

                webDarwinCoreChange.IsMoreSpeciesObservationsAvailable = false;

                while (dataReader.Read())
                {
                    Int32 total = dataReader.GetInt32(0, 0); // borde vara Int64, men det returnerar inte db
                    if (total > 0)
                    {
                        webDarwinCoreChange.IsMoreSpeciesObservationsAvailable = true;
                    }
                }
            }

            webDarwinCoreChange.MaxChangeCount = Settings.Default.MaxPageSize;

            if (!webDarwinCoreChange.IsMoreSpeciesObservationsAvailable && webDarwinCoreChange.MaxChangeId == 0)
            {
                webDarwinCoreChange.MaxChangeId = -1;
            }

            GetExternalInformation(context, webDarwinCoreChange.CreatedSpeciesObservations, coordinateSystem);
            GetExternalInformation(context, webDarwinCoreChange.UpdatedSpeciesObservations, coordinateSystem);
            ConvertToRequestedCoordinates(context, webDarwinCoreChange.CreatedSpeciesObservations, coordinateSystem);
            ConvertToRequestedCoordinates(context, webDarwinCoreChange.UpdatedSpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, webDarwinCoreChange.CreatedSpeciesObservations, "GetDarwinCoreChange");
            CheckSpeciesObservationLog(context, webDarwinCoreChange.UpdatedSpeciesObservations, "GetDarwinCoreChange");
            return webDarwinCoreChange;
        }

        /// <summary>
        /// Convert one species observation field from JSON to
        /// class WebSpeciesObservationField.
        /// </summary>
        /// <param name="isFieldFound">
        /// Indicates if field has been read. If no field has been read
        /// then we have reached the end of this species observation.
        /// </param>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="speciesObservationsJson">Species observation field in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetDarwinCoreField(out Boolean isFieldFound,
                                                WebDarwinCore speciesObservation,
                                                String speciesObservationsJson,
                                                Dictionary<String, WebSpeciesObservationField> mapping,
                                                Int32 startIndex)
        {
            Int32 stopIndex;
            String fieldKey, value;
            String[] splitKey;
            WebSpeciesObservationField mappingField;

            isFieldFound = false;

            // Get field key.
            if (speciesObservationsJson[startIndex] != '}')
            {
                stopIndex = speciesObservationsJson.IndexOf(':', startIndex);
                fieldKey = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 2);
                startIndex = stopIndex + 1;
                if (mapping.ContainsKey(fieldKey))
                {
                    splitKey = fieldKey.Split('_');
                    if (splitKey.Length > 2)
                    {
                        // This field should not be returned.
                        // Return next field instead.
                        stopIndex = speciesObservationsJson.IndexOf(',', startIndex);
                        startIndex = stopIndex + 1;
                        return GetDarwinCoreField(out isFieldFound,
                                                  speciesObservation,
                                                  speciesObservationsJson,
                                                  mapping,
                                                  startIndex);
                    }
                    else
                    {
                        // Create field instance.
                        mappingField = mapping[fieldKey];
                        startIndex = GetSpeciesObservationFieldValue(out value,
                                                                     mappingField.Type,
                                                                     speciesObservationsJson,
                                                                     startIndex);
                        GetDarwinCoreField(speciesObservation, mappingField, value);
                        isFieldFound = true;
                    }
                }
                else
                {
                    if (fieldKey == "\"sort")
                    {
                        // No more fields in current species observation.
                        // Read to next species observation.
                        stopIndex = speciesObservationsJson.IndexOf('}', startIndex);
                        startIndex = stopIndex + 1;
                    }
                    else
                    {
                        throw new Exception("Unknown field name = " + fieldKey);
                    }
                }
            }
            else
            {
                // No more fields in current species observation.
                // Read to next species observation.
                startIndex += 2;
            }

            return startIndex;
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreField(WebDarwinCore speciesObservation,
                                               WebSpeciesObservationField field,
                                               String value)
        {
            switch (field.ClassIdentifier)
            {
                case "Conservation":
                    GetDarwinCoreFieldConservation(speciesObservation, field, value);
                    break;
                case "DarwinCore":
                    GetDarwinCoreFieldDarwinCore(speciesObservation, field, value);
                    break;
                case "Event":
                    GetDarwinCoreFieldEvent(speciesObservation, field, value);
                    break;
                case "GeologicalContext":
                    GetDarwinCoreFieldGeologicalContext(speciesObservation, field, value);
                    break;
                case "Identification":
                    GetDarwinCoreFieldIdentification(speciesObservation, field, value);
                    break;
                case "Location":
                    GetDarwinCoreFieldLocation(speciesObservation, field, value);
                    break;
                case "MeasurementOrFact":
                    GetDarwinCoreFieldMeasurementOrFact(speciesObservation, field, value);
                    break;
                case "Occurrence":
                    GetDarwinCoreFieldOccurrence(speciesObservation, field, value);
                    break;
                case "Project":
                    GetDarwinCoreFieldProject(speciesObservation, field, value);
                    break;
                case "ResourceRelationship":
                    GetDarwinCoreFieldResourceRelationship(speciesObservation, field, value);
                    break;
                case "Taxon":
                    GetDarwinCoreFieldTaxon(speciesObservation, field, value);
                    break;
                default:
                    throw new ApplicationException("Unknown species observation class = " + field.ClassIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldConservation(WebDarwinCore speciesObservation,
                                                           WebSpeciesObservationField field,
                                                           String value)
        {
            if (speciesObservation.Conservation.IsNull())
            {
                speciesObservation.Conservation = new WebDarwinCoreConservation();
            }

            switch (field.PropertyIdentifier)
            {
                case "ActionPlan":
                    speciesObservation.Conservation.ActionPlan = value.WebParseBoolean();
                    break;
                case "ConservationRelevant":
                    speciesObservation.Conservation.ConservationRelevant = value.WebParseBoolean();
                    break;
                case "Natura2000":
                    speciesObservation.Conservation.Natura2000 = value.WebParseBoolean();
                    break;
                case "ProtectedByLaw":
                    speciesObservation.Conservation.ProtectedByLaw = value.WebParseBoolean();
                    break;
                case "ProtectionLevel":
                    speciesObservation.Conservation.ProtectionLevel = value.WebParseInt32();
                    break;
                case "RedlistCategory":
                    speciesObservation.Conservation.RedlistCategory = value;
                    break;
                case "SwedishImmigrationHistory":
                    speciesObservation.Conservation.SwedishImmigrationHistory = value;
                    break;
                case "SwedishOccurrence":
                    speciesObservation.Conservation.SwedishOccurrence = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldDarwinCore(WebDarwinCore speciesObservation,
                                                         WebSpeciesObservationField field,
                                                         String value)
        {
            switch (field.PropertyIdentifier)
            {
                case "AccessRights":
                    speciesObservation.AccessRights = value;
                    break;
                case "BasisOfRecord":
                    speciesObservation.BasisOfRecord = value;
                    break;
                case "BibliographicCitation":
                    speciesObservation.BibliographicCitation = value;
                    break;
                case "CollectionCode":
                    speciesObservation.CollectionCode = value;
                    break;
                case "CollectionID":
                    speciesObservation.CollectionID = value;
                    break;
                case "DataGeneralizations":
                    speciesObservation.DataGeneralizations = value;
                    break;
                case "DatasetID":
                    speciesObservation.DatasetID = value;
                    break;
                case "DatasetName":
                    speciesObservation.DatasetName = value;
                    break;
                case "DynamicProperties":
                    speciesObservation.DynamicProperties = value;
                    break;
                case "Id":
                    speciesObservation.Id = value.WebParseInt64();
                    break;
                case "InformationWithheld":
                    speciesObservation.InformationWithheld = value;
                    break;
                case "InstitutionCode":
                    speciesObservation.InstitutionCode = value;
                    break;
                case "InstitutionID":
                    speciesObservation.InstitutionID = value;
                    break;
                case "Language":
                    speciesObservation.Language = value;
                    break;
                case "Modified":
                    speciesObservation.Modified = value.WebParseDateTime();
                    break;
                case "Owner":
                    speciesObservation.Owner = value;
                    break;
                case "OwnerInstitutionCode":
                    speciesObservation.OwnerInstitutionCode = value;
                    break;
                case "References":
                    speciesObservation.References = value;
                    break;
                case "ReportedBy":
                    speciesObservation.ReportedBy = value;
                    break;
                case "ReportedDate":
                    speciesObservation.ReportedDate = value.WebParseDateTime();
                    break;
                case "Rights":
                    speciesObservation.Rights = value;
                    break;
                case "RightsHolder":
                    speciesObservation.RightsHolder = value;
                    break;
                case "SpeciesObservationURL":
                    speciesObservation.SpeciesObservationURL = value;
                    break;
                case "Type":
                    speciesObservation.Type = value;
                    break;
                case "ValidationStatus":
                    speciesObservation.ValidationStatus = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldEvent(WebDarwinCore speciesObservation,
                                                    WebSpeciesObservationField field,
                                                    String value)
        {
            if (speciesObservation.Event.IsNull())
            {
                speciesObservation.Event = new WebDarwinCoreEvent();
            }

            switch (field.PropertyIdentifier)
            {
                case "Day":
                    speciesObservation.Event.Day = value.WebParseInt32();
                    break;
                case "End":
                    speciesObservation.Event.End = value.WebParseDateTime();
                    break;
                case "EndDayOfYear":
                    speciesObservation.Event.EndDayOfYear = value.WebParseInt32();
                    break;
                case "EventDate":
                    speciesObservation.Event.EventDate = value;
                    break;
                case "EventID":
                    speciesObservation.Event.EventID = value;
                    break;
                case "EventRemarks":
                    speciesObservation.Event.EventRemarks = value;
                    break;
                case "EventTime":
                    speciesObservation.Event.EventTime = value;
                    break;
                case "FieldNotes":
                    speciesObservation.Event.FieldNotes = value;
                    break;
                case "FieldNumber":
                    speciesObservation.Event.FieldNumber = value;
                    break;
                case "Habitat":
                    speciesObservation.Event.Habitat = value;
                    break;
                case "Month":
                    speciesObservation.Event.Month = value.WebParseInt32();
                    break;
                case "SamplingEffort":
                    speciesObservation.Event.SamplingEffort = value;
                    break;
                case "SamplingProtocol":
                    speciesObservation.Event.SamplingProtocol = value;
                    break;
                case "Start":
                    speciesObservation.Event.Start = value.WebParseDateTime();
                    break;
                case "StartDayOfYear":
                    speciesObservation.Event.StartDayOfYear = value.WebParseInt32();
                    break;
                case "VerbatimEventDate":
                    speciesObservation.Event.VerbatimEventDate = value;
                    break;
                case "Year":
                    speciesObservation.Event.Year = value.WebParseInt32();
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldGeologicalContext(WebDarwinCore speciesObservation,
                                                                WebSpeciesObservationField field,
                                                                String value)
        {
            if (speciesObservation.GeologicalContext.IsNull())
            {
                speciesObservation.GeologicalContext = new WebDarwinCoreGeologicalContext();
            }

            switch (field.PropertyIdentifier)
            {
                case "Bed":
                    speciesObservation.GeologicalContext.Bed = value;
                    break;
                case "EarliestAgeOrLowestStage":
                    speciesObservation.GeologicalContext.EarliestAgeOrLowestStage = value;
                    break;
                case "EarliestEonOrLowestEonothem":
                    speciesObservation.GeologicalContext.EarliestEonOrLowestEonothem = value;
                    break;
                case "EarliestEpochOrLowestSeries":
                    speciesObservation.GeologicalContext.EarliestEpochOrLowestSeries = value;
                    break;
                case "EarliestEraOrLowestErathem":
                    speciesObservation.GeologicalContext.EarliestEraOrLowestErathem = value;
                    break;
                case "EarliestPeriodOrLowestSystem":
                    speciesObservation.GeologicalContext.EarliestPeriodOrLowestSystem = value;
                    break;
                case "Formation":
                    speciesObservation.GeologicalContext.Formation = value;
                    break;
                case "GeologicalContextID":
                    speciesObservation.GeologicalContext.GeologicalContextID = value;
                    break;
                case "Group":
                    speciesObservation.GeologicalContext.Group = value;
                    break;
                case "HighestBiostratigraphicZone":
                    speciesObservation.GeologicalContext.HighestBiostratigraphicZone = value;
                    break;
                case "LatestAgeOrHighestStage":
                    speciesObservation.GeologicalContext.LatestAgeOrHighestStage = value;
                    break;
                case "LatestEonOrHighestEonothem":
                    speciesObservation.GeologicalContext.LatestEonOrHighestEonothem = value;
                    break;
                case "LatestEpochOrHighestSeries":
                    speciesObservation.GeologicalContext.LatestEpochOrHighestSeries = value;
                    break;
                case "LatestEraOrHighestErathem":
                    speciesObservation.GeologicalContext.LatestEraOrHighestErathem = value;
                    break;
                case "LatestPeriodOrHighestSystem":
                    speciesObservation.GeologicalContext.LatestPeriodOrHighestSystem = value;
                    break;
                case "LithostratigraphicTerms":
                    speciesObservation.GeologicalContext.LithostratigraphicTerms = value;
                    break;
                case "LowestBiostratigraphicZone":
                    speciesObservation.GeologicalContext.LowestBiostratigraphicZone = value;
                    break;
                case "Member":
                    speciesObservation.GeologicalContext.Member = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldIdentification(WebDarwinCore speciesObservation,
                                                             WebSpeciesObservationField field,
                                                             String value)
        {
            if (speciesObservation.Identification.IsNull())
            {
                speciesObservation.Identification = new WebDarwinCoreIdentification();
            }

            switch (field.PropertyIdentifier)
            {
                case "DateIdentified":
                    speciesObservation.Identification.DateIdentified = value;
                    break;
                case "IdentificationID":
                    speciesObservation.Identification.IdentificationID = value;
                    break;
                case "IdentificationQualifier":
                    speciesObservation.Identification.IdentificationQualifier = value;
                    break;
                case "IdentificationReferences":
                    speciesObservation.Identification.IdentificationReferences = value;
                    break;
                case "IdentificationRemarks":
                    speciesObservation.Identification.IdentificationRemarks = value;
                    break;
                case "IdentificationVerificationStatus":
                    speciesObservation.Identification.IdentificationVerificationStatus = value;
                    break;
                case "IdentifiedBy":
                    speciesObservation.Identification.IdentifiedBy = value;
                    break;
                case "TypeStatus":
                    speciesObservation.Identification.TypeStatus = value;
                    break;
                case "UncertainDetermination":
                    speciesObservation.Identification.UncertainDetermination = value.WebParseBoolean();
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldLocation(WebDarwinCore speciesObservation,
                                                       WebSpeciesObservationField field,
                                                       String value)
        {
            if (speciesObservation.Location.IsNull())
            {
                speciesObservation.Location = new WebDarwinCoreLocation();
            }

            switch (field.PropertyIdentifier)
            {
                case "Continent":
                    speciesObservation.Location.Continent = value;
                    break;
                case "CoordinateM":
                    speciesObservation.Location.CoordinateM = value;
                    break;
                case "CoordinatePrecision":
                    speciesObservation.Location.CoordinatePrecision = value;
                    break;
                case "CoordinateSystemWkt":
                    speciesObservation.Location.CoordinateSystemWkt = value;
                    break;
                case "CoordinateUncertaintyInMeters":
                    speciesObservation.Location.CoordinateUncertaintyInMeters = value;
                    break;
                case "CoordinateX":
                    speciesObservation.Location.CoordinateX = value.WebParseInt32();
                    break;
                case "CoordinateY":
                    speciesObservation.Location.CoordinateY = value.WebParseInt32();
                    break;
                case "CoordinateZ":
                    speciesObservation.Location.CoordinateZ = value;
                    break;
                case "Country":
                    speciesObservation.Location.Country = value;
                    break;
                case "CountryCode":
                    speciesObservation.Location.CountryCode = value;
                    break;
                case "County":
                    speciesObservation.Location.County = value;
                    break;
                case "DecimalLatitude":
                    speciesObservation.Location.DecimalLatitude = value.WebParseDouble();
                    break;
                case "DecimalLongitude":
                    speciesObservation.Location.DecimalLongitude = value.WebParseDouble();
                    break;
                case "FootprintSpatialFit":
                    speciesObservation.Location.FootprintSpatialFit = value;
                    break;
                case "FootprintSRS":
                    speciesObservation.Location.FootprintSRS = value;
                    break;
                case "FootprintWKT":
                    speciesObservation.Location.FootprintWKT = value;
                    break;
                case "GeodeticDatum":
                    speciesObservation.Location.GeodeticDatum = value;
                    break;
                case "GeoreferencedBy":
                    speciesObservation.Location.GeoreferencedBy = value;
                    break;
                case "GeoreferencedDate":
                    speciesObservation.Location.GeoreferencedDate = value;
                    break;
                case "GeoreferenceProtocol":
                    speciesObservation.Location.GeoreferenceProtocol = value;
                    break;
                case "GeoreferenceRemarks":
                    speciesObservation.Location.GeoreferenceRemarks = value;
                    break;
                case "GeoreferenceSources":
                    speciesObservation.Location.GeoreferenceSources = value;
                    break;
                case "GeoreferenceVerificationStatus":
                    speciesObservation.Location.GeoreferenceVerificationStatus = value;
                    break;
                case "HigherGeography":
                    speciesObservation.Location.HigherGeography = value;
                    break;
                case "HigherGeographyID":
                    speciesObservation.Location.HigherGeographyID = value;
                    break;
                case "Island":
                    speciesObservation.Location.Island = value;
                    break;
                case "IslandGroup":
                    speciesObservation.Location.IslandGroup = value;
                    break;
                case "Locality":
                    speciesObservation.Location.Locality = value;
                    break;
                case "LocationAccordingTo":
                    speciesObservation.Location.LocationAccordingTo = value;
                    break;
                case "LocationId":
                    speciesObservation.Location.LocationId = value;
                    break;
                case "LocationRemarks":
                    speciesObservation.Location.LocationRemarks = value;
                    break;
                case "LocationURL":
                    speciesObservation.Location.LocationURL = value;
                    break;
                case "MaximumDepthInMeters":
                    speciesObservation.Location.MaximumDepthInMeters = value;
                    break;
                case "MaximumDistanceAboveSurfaceInMeters":
                    speciesObservation.Location.MaximumDistanceAboveSurfaceInMeters = value;
                    break;
                case "MaximumElevationInMeters":
                    speciesObservation.Location.MaximumElevationInMeters = value;
                    break;
                case "MinimumDepthInMeters":
                    speciesObservation.Location.MinimumDepthInMeters = value;
                    break;
                case "MinimumDistanceAboveSurfaceInMeters":
                    speciesObservation.Location.MinimumDistanceAboveSurfaceInMeters = value;
                    break;
                case "MinimumElevationInMeters":
                    speciesObservation.Location.MinimumElevationInMeters = value;
                    break;
                case "Municipality":
                    speciesObservation.Location.Municipality = value;
                    break;
                case "Parish":
                    speciesObservation.Location.Parish = value;
                    break;
                case "PointRadiusSpatialFit":
                    speciesObservation.Location.PointRadiusSpatialFit = value;
                    break;
                case "StateProvince":
                    speciesObservation.Location.StateProvince = value;
                    break;
                case "VerbatimCoordinates":
                    speciesObservation.Location.VerbatimCoordinates = value;
                    break;
                case "VerbatimCoordinateSystem":
                    speciesObservation.Location.VerbatimCoordinateSystem = value;
                    break;
                case "VerbatimDepth":
                    speciesObservation.Location.VerbatimDepth = value;
                    break;
                case "VerbatimElevation":
                    speciesObservation.Location.VerbatimElevation = value;
                    break;
                case "VerbatimLatitude":
                    speciesObservation.Location.VerbatimLatitude = value;
                    break;
                case "VerbatimLocality":
                    speciesObservation.Location.VerbatimLocality = value;
                    break;
                case "VerbatimLongitude":
                    speciesObservation.Location.VerbatimLongitude = value;
                    break;
                case "VerbatimSRS":
                    speciesObservation.Location.VerbatimSRS = value;
                    break;
                case "WaterBody":
                    speciesObservation.Location.WaterBody = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldMeasurementOrFact(WebDarwinCore speciesObservation,
                                                                WebSpeciesObservationField field,
                                                                String value)
        {
            if (speciesObservation.MeasurementOrFact.IsNull())
            {
                speciesObservation.MeasurementOrFact = new WebDarwinCoreMeasurementOrFact();
            }

            switch (field.PropertyIdentifier)
            {
                case "MeasurementAccuracy":
                    speciesObservation.MeasurementOrFact.MeasurementAccuracy = value;
                    break;
                case "MeasurementDeterminedBy":
                    speciesObservation.MeasurementOrFact.MeasurementDeterminedBy = value;
                    break;
                case "MeasurementDeterminedDate":
                    speciesObservation.MeasurementOrFact.MeasurementDeterminedDate = value;
                    break;
                case "MeasurementID":
                    speciesObservation.MeasurementOrFact.MeasurementID = value;
                    break;
                case "MeasurementMethod":
                    speciesObservation.MeasurementOrFact.MeasurementMethod = value;
                    break;
                case "MeasurementRemarks":
                    speciesObservation.MeasurementOrFact.MeasurementRemarks = value;
                    break;
                case "MeasurementType":
                    speciesObservation.MeasurementOrFact.MeasurementType = value;
                    break;
                case "MeasurementUnit":
                    speciesObservation.MeasurementOrFact.MeasurementUnit = value;
                    break;
                case "MeasurementValue":
                    speciesObservation.MeasurementOrFact.MeasurementValue = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldOccurrence(WebDarwinCore speciesObservation,
                                                         WebSpeciesObservationField field,
                                                         String value)
        {
            if (speciesObservation.Occurrence.IsNull())
            {
                speciesObservation.Occurrence = new WebDarwinCoreOccurrence();
            }

            switch (field.PropertyIdentifier)
            {
                case "AssociatedMedia":
                    speciesObservation.Occurrence.AssociatedMedia = value;
                    break;
                case "AssociatedOccurrences":
                    speciesObservation.Occurrence.AssociatedOccurrences = value;
                    break;
                case "AssociatedReferences":
                    speciesObservation.Occurrence.AssociatedReferences = value;
                    break;
                case "AssociatedSequences":
                    speciesObservation.Occurrence.AssociatedSequences = value;
                    break;
                case "AssociatedTaxa":
                    speciesObservation.Occurrence.AssociatedTaxa = value;
                    break;
                case "Behavior":
                    speciesObservation.Occurrence.Behavior = value;
                    break;
                case "CatalogNumber":
                    speciesObservation.Occurrence.CatalogNumber = value;
                    break;
                case "Disposition":
                    speciesObservation.Occurrence.Disposition = value;
                    break;
                case "EstablishmentMeans":
                    speciesObservation.Occurrence.EstablishmentMeans = value;
                    break;
                case "IndividualCount":
                    speciesObservation.Occurrence.IndividualCount = value;
                    break;
                case "IndividualID":
                    speciesObservation.Occurrence.IndividualID = value;
                    break;
                case "IsNaturalOccurrence":
                    speciesObservation.Occurrence.IsNaturalOccurrence = value.WebParseBoolean();
                    break;
                case "IsNeverFoundObservation":
                    speciesObservation.Occurrence.IsNeverFoundObservation = value.WebParseBoolean();
                    break;
                case "IsNotRediscoveredObservation":
                    speciesObservation.Occurrence.IsNotRediscoveredObservation = value.WebParseBoolean();
                    break;
                case "IsPositiveObservation":
                    speciesObservation.Occurrence.IsPositiveObservation = value.WebParseBoolean();
                    break;
                case "LifeStage":
                    speciesObservation.Occurrence.LifeStage = value;
                    break;
                case "OccurrenceID":
                    speciesObservation.Occurrence.OccurrenceID = value;
                    break;
                case "OccurrenceRemarks":
                    speciesObservation.Occurrence.OccurrenceRemarks = value;
                    break;
                case "OccurrenceStatus":
                    speciesObservation.Occurrence.OccurrenceStatus = value;
                    break;
                case "OccurrenceURL":
                    speciesObservation.Occurrence.OccurrenceURL = value;
                    break;
                case "OtherCatalogNumbers":
                    speciesObservation.Occurrence.OtherCatalogNumbers = value;
                    break;
                case "Preparations":
                    speciesObservation.Occurrence.Preparations = value;
                    break;
                case "PreviousIdentifications":
                    speciesObservation.Occurrence.PreviousIdentifications = value;
                    break;
                case "Quantity":
                    speciesObservation.Occurrence.Quantity = value;
                    break;
                case "QuantityUnit":
                    speciesObservation.Occurrence.QuantityUnit = value;
                    break;
                case "RecordedBy":
                    speciesObservation.Occurrence.RecordedBy = value;
                    break;
                case "RecordNumber":
                    speciesObservation.Occurrence.RecordNumber = value;
                    break;
                case "ReproductiveCondition":
                    speciesObservation.Occurrence.ReproductiveCondition = value;
                    break;
                case "Sex":
                    speciesObservation.Occurrence.Sex = value;
                    break;
                case "Substrate":
                    speciesObservation.Occurrence.Substrate = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldProject(WebDarwinCore speciesObservation,
                                                      WebSpeciesObservationField field,
                                                      String value)
        {
            if (speciesObservation.Project.IsNull())
            {
                speciesObservation.Project = new WebDarwinCoreProject();
            }

            switch (field.PropertyIdentifier)
            {
                case "IsPublic":
                    speciesObservation.Project.IsPublic = value.WebParseBoolean();
                    break;
                case "ProjectCategory":
                    speciesObservation.Project.ProjectCategory = value;
                    break;
                case "ProjectDescription":
                    speciesObservation.Project.ProjectDescription = value;
                    break;
                case "ProjectEndDate":
                    speciesObservation.Project.ProjectEndDate = value;
                    break;
                case "ProjectID":
                    speciesObservation.Project.ProjectID = value;
                    break;
                case "ProjectName":
                    speciesObservation.Project.ProjectName = value;
                    break;
                case "ProjectOwner":
                    speciesObservation.Project.ProjectOwner = value;
                    break;
                case "ProjectStartDate":
                    speciesObservation.Project.ProjectStartDate = value;
                    break;
                case "ProjectURL":
                    speciesObservation.Project.ProjectURL = value;
                    break;
                case "SurveyMethod":
                    speciesObservation.Project.SurveyMethod = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldResourceRelationship(WebDarwinCore speciesObservation,
                                                                   WebSpeciesObservationField field,
                                                                   String value)
        {
            if (speciesObservation.ResourceRelationship.IsNull())
            {
                speciesObservation.ResourceRelationship = new WebDarwinCoreResourceRelationship();
            }

            switch (field.PropertyIdentifier)
            {
                case "RelatedResourceID":
                    speciesObservation.ResourceRelationship.RelatedResourceID = value;
                    break;
                case "RelationshipAccordingTo":
                    speciesObservation.ResourceRelationship.RelationshipAccordingTo = value;
                    break;
                case "RelationshipEstablishedDate":
                    speciesObservation.ResourceRelationship.RelationshipEstablishedDate = value;
                    break;
                case "RelationshipOfResource":
                    speciesObservation.ResourceRelationship.RelationshipOfResource = value;
                    break;
                case "RelationshipRemarks":
                    speciesObservation.ResourceRelationship.RelationshipRemarks = value;
                    break;
                case "ResourceID":
                    speciesObservation.ResourceRelationship.ResourceID = value;
                    break;
                case "ResourceRelationshipID":
                    speciesObservation.ResourceRelationship.ResourceRelationshipID = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Get information about fields that are included
        /// in the DarwinCore format.
        /// This information is used in calls to Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>
        /// Information about fields that are included
        /// in the DarwinCore format.
        /// </returns>
        private static String GetDarwinCoreFields(WebServiceContext context)
        {
            Boolean isFirstField;
            FieldDefinitionList fieldDefinitions;
            String darwinCoreFields;
            String[] splitFieldName;
            StringBuilder darwinCoreFieldsBuilder;

            // Get data from cache.
            darwinCoreFields = (String)context.GetCachedObject(Settings.Default.SpeciesObservationDarwinCoreFieldsCacheKey);

            if (darwinCoreFields.IsNull())
            {
                // Generate string with information about included fields.
                darwinCoreFieldsBuilder = new StringBuilder();
                darwinCoreFieldsBuilder.Append("\"_source\":{\"include\":[");
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    fieldDefinitions = elastisearch.GetSpeciesObservationMapping();
                }

                if (fieldDefinitions.IsNotEmpty())
                {
                    isFirstField = true;
                    foreach (FieldDefinition fieldDefinition in fieldDefinitions)
                    {
                        splitFieldName = fieldDefinition.Name.Split('_');
                        if ((splitFieldName.Length == 2) &&
                            IsDarwinCoreField(splitFieldName[0], splitFieldName[1]))
                        {
                            if (isFirstField)
                            {
                                isFirstField = false;
                            }
                            else
                            {
                                darwinCoreFieldsBuilder.Append(",");
                            }

                            darwinCoreFieldsBuilder.Append("\"" + fieldDefinition.Name + "\"");
                        }
                    }
                }

                darwinCoreFieldsBuilder.Append("]}");
                darwinCoreFields = darwinCoreFieldsBuilder.ToString();

                // Store data in cache.
                context.AddCachedObject(Settings.Default.SpeciesObservationDarwinCoreFieldsCacheKey,
                                        darwinCoreFields,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return darwinCoreFields;
        }

        /// <summary>
        /// Insert one field value into the species observation.
        /// </summary>
        /// <param name="speciesObservation">Field information should be put into this species observation.</param>
        /// <param name="field">Species observation field information.</param>
        /// <param name="value">The value that should be inserted into the species observation.</param>
        private static void GetDarwinCoreFieldTaxon(WebDarwinCore speciesObservation,
                                                    WebSpeciesObservationField field,
                                                    String value)
        {
            if (speciesObservation.Taxon.IsNull())
            {
                speciesObservation.Taxon = new WebDarwinCoreTaxon();
            }

            switch (field.PropertyIdentifier)
            {
                case "AcceptedNameUsage":
                    speciesObservation.Taxon.AcceptedNameUsage = value;
                    break;
                case "AcceptedNameUsageID":
                    speciesObservation.Taxon.AcceptedNameUsageID = value;
                    break;
                case "Class":
                    speciesObservation.Taxon.Class = value;
                    break;
                case "DyntaxaTaxonID":
                    speciesObservation.Taxon.DyntaxaTaxonID = value.WebParseInt32();
                    break;
                case "Family":
                    speciesObservation.Taxon.Family = value;
                    break;
                case "Genus":
                    speciesObservation.Taxon.Genus = value;
                    break;
                case "HigherClassification":
                    speciesObservation.Taxon.HigherClassification = value;
                    break;
                case "InfraspecificEpithet":
                    speciesObservation.Taxon.InfraspecificEpithet = value;
                    break;
                case "Kingdom":
                    speciesObservation.Taxon.Kingdom = value;
                    break;
                case "NameAccordingTo":
                    speciesObservation.Taxon.NameAccordingTo = value;
                    break;
                case "NameAccordingToID":
                    speciesObservation.Taxon.NameAccordingToID = value;
                    break;
                case "NamePublishedIn":
                    speciesObservation.Taxon.NamePublishedIn = value;
                    break;
                case "NamePublishedInID":
                    speciesObservation.Taxon.NamePublishedInID = value;
                    break;
                case "NamePublishedInYear":
                    speciesObservation.Taxon.NamePublishedInYear = value;
                    break;
                case "NomenclaturalCode":
                    speciesObservation.Taxon.NomenclaturalCode = value;
                    break;
                case "NomenclaturalStatus":
                    speciesObservation.Taxon.NomenclaturalStatus = value;
                    break;
                case "Order":
                    speciesObservation.Taxon.Order = value;
                    break;
                case "OrganismGroup":
                    speciesObservation.Taxon.OrganismGroup = value;
                    break;
                case "OriginalNameUsage":
                    speciesObservation.Taxon.OriginalNameUsage = value;
                    break;
                case "OriginalNameUsageID":
                    speciesObservation.Taxon.OriginalNameUsageID = value;
                    break;
                case "ParentNameUsage":
                    speciesObservation.Taxon.ParentNameUsage = value;
                    break;
                case "ParentNameUsageID":
                    speciesObservation.Taxon.ParentNameUsageID = value;
                    break;
                case "Phylum":
                    speciesObservation.Taxon.Phylum = value;
                    break;
                case "ScientificName":
                    speciesObservation.Taxon.ScientificName = value;
                    break;
                case "ScientificNameAuthorship":
                    speciesObservation.Taxon.ScientificNameAuthorship = value;
                    break;
                case "ScientificNameID":
                    speciesObservation.Taxon.ScientificNameID = value;
                    break;
                case "SpecificEpithet":
                    speciesObservation.Taxon.SpecificEpithet = value;
                    break;
                case "Subgenus":
                    speciesObservation.Taxon.Subgenus = value;
                    break;
                case "TaxonConceptID":
                    speciesObservation.Taxon.TaxonConceptID = value;
                    break;
                case "TaxonConceptStatus":
                    speciesObservation.Taxon.TaxonConceptStatus = value;
                    break;
                case "TaxonID":
                    speciesObservation.Taxon.TaxonID = value;
                    break;
                case "TaxonomicStatus":
                    speciesObservation.Taxon.TaxonomicStatus = value;
                    break;
                case "TaxonRank":
                    speciesObservation.Taxon.TaxonRank = value;
                    break;
                case "TaxonRemarks":
                    speciesObservation.Taxon.TaxonRemarks = value;
                    break;
                // Support for field taxon sort order has been removed since data
                // update handling (species observation) could not cope with all changes. 
                //case "TaxonSortOrder":
                //    speciesObservation.Taxon.TaxonSortOrder = value.WebParseInt32();
                //    break;
                case "TaxonURL":
                    speciesObservation.Taxon.TaxonURL = value;
                    break;
                case "VerbatimScientificName":
                    speciesObservation.Taxon.VerbatimScientificName = value;
                    break;
                case "VerbatimTaxonRank":
                    speciesObservation.Taxon.VerbatimTaxonRank = value;
                    break;
                case "VernacularName":
                    speciesObservation.Taxon.VernacularName = value;
                    break;
                default:
                    throw new ApplicationException("Unknown species observation property = " + field.PropertyIdentifier);
            }
        }

        /// <summary>
        /// Get darwin core information.
        /// If max species observation count is exceeded,
        /// return only species observation ids.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="dataReader">An open data reader.</param>
        /// <returns>Darwin core information.</returns>
        private static WebDarwinCoreInformation GetDarwinCoreInformation(WebServiceContext context,
                                                                         DataReader dataReader)
        {
            Boolean isSimpleSpeciesObservationAccessRights;
            Int32 speciesObservationCount;
            WebDarwinCore webDarwinCore;
            WebDarwinCoreInformation webDarwinCoreInformation;

            isSimpleSpeciesObservationAccessRights = context.CurrentRoles.IsSimpleSpeciesObservationAccessRights();
            speciesObservationCount = 0;
            webDarwinCoreInformation = new WebDarwinCoreInformation();
            webDarwinCoreInformation.SpeciesObservations = new List<WebDarwinCore>();
            while (dataReader.Read() && (speciesObservationCount <= Settings.Default.MaxSpeciesObservationWithInformation))
            {
                webDarwinCore = new WebDarwinCore();
                webDarwinCore.Load(dataReader);
                if (isSimpleSpeciesObservationAccessRights ||
                    (webDarwinCore.Conservation.ProtectionLevel <= (Int32)(SpeciesProtectionLevelEnum.Public)) ||
                    CheckAccessRights(context, webDarwinCore))
                {
                    speciesObservationCount++;
                    webDarwinCoreInformation.SpeciesObservations.Add(webDarwinCore);
                }
            }

            if (speciesObservationCount > Settings.Default.MaxSpeciesObservationWithInformation)
            {
                // Too many species observations with information.
                // Return only species observation ids.
                webDarwinCoreInformation.SpeciesObservationIds = new List<Int64>();
                foreach (WebDarwinCore speciesObservation in webDarwinCoreInformation.SpeciesObservations)
                {
                    webDarwinCoreInformation.SpeciesObservationIds.Add(speciesObservation.Id);
                }

                webDarwinCoreInformation.SpeciesObservations = null;

                if (isSimpleSpeciesObservationAccessRights)
                {
                    while (dataReader.Read() && (speciesObservationCount <= Settings.Default.MaxSpeciesObservation))
                    {
                        speciesObservationCount++;
                        webDarwinCoreInformation.SpeciesObservationIds.Add(dataReader.GetInt64((Int32)DarwinCoreColumn.Id));
                    }
                }
                else
                {
                    while (dataReader.Read() && (speciesObservationCount <= Settings.Default.MaxSpeciesObservation))
                    {
                        webDarwinCore = new WebDarwinCore();
                        webDarwinCore.Load(dataReader);
                        if ((webDarwinCore.Conservation.ProtectionLevel <= (Int32)(SpeciesProtectionLevelEnum.Public)) ||
                            CheckAccessRights(context, webDarwinCore))
                        {
                            webDarwinCoreInformation.SpeciesObservationIds.Add(webDarwinCore.Id);
                        }
                    }
                }

                if (speciesObservationCount > Settings.Default.MaxSpeciesObservation)
                {
                    // Too many species observations with ids.
                    throw new ApplicationException("Too many species observations was retrieved!, Limit is set to " +
                                                   Settings.Default.MaxSpeciesObservation + " observations.");
                }
            }

            webDarwinCoreInformation.SetCount();
            return webDarwinCoreInformation;
        }

        /// <summary>
        /// Get darwin core information by selected interval.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="dataReader">The dataReader.</param>
        /// <param name="startRow">Define starting row.</param>
        /// <param name="endRow">Define ending row.</param>
        /// <returns>Return darwin core information by selected interval.</returns>
        private static WebDarwinCoreInformation GetDarwinCoreInformation(WebServiceContext context,
                                                                         DataReader dataReader,
                                                                         Int64 startRow,
                                                                         Int64 endRow)
        {
            Int32 rowCounter;
            WebDarwinCore webDarwinCore;
            WebDarwinCoreInformation webDarwinCoreInformation;

            rowCounter = 0;
            webDarwinCoreInformation = new WebDarwinCoreInformation();
            webDarwinCoreInformation.SpeciesObservations = new List<WebDarwinCore>();

            while (dataReader.Read() && (rowCounter < Settings.Default.MaxSpeciesObservationWithInformation))
            {
                webDarwinCore = new WebDarwinCore();
                webDarwinCore.Load(dataReader);

                if ((webDarwinCore.Conservation.ProtectionLevel <= (Int32)(SpeciesProtectionLevelEnum.Public)) ||
                    CheckAccessRights(context, webDarwinCore))
                {
                    rowCounter++;

                    if (startRow <= rowCounter)
                    {
                        webDarwinCoreInformation.SpeciesObservations.Add(webDarwinCore);
                    }

                    if (endRow <= rowCounter)
                    {
                        break;
                    }
                }
            }

            webDarwinCoreInformation.SetCount();
            return webDarwinCoreInformation;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the filter.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// This parameter should be null if ids should not be
        /// retrieved when filter matches to many observations
        /// with information.
        /// </param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="filter">
        /// Filter that is used when species observations are retrieved.
        /// </param>
        /// <param name="methodName">
        /// Name of the method that the web service user uses.
        /// This name is used when logging information about
        /// usage of protected information.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        private static WebDarwinCoreInformation GetDarwinCoreInformationElasticsearch(WebServiceContext context,
                                                                                      WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                      WebCoordinateSystem coordinateSystem,
                                                                                      String filter,
                                                                                      String methodName)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            Int32 startIndex;
            DocumentFilterResponse speciesObservationResponse;
            StringBuilder idFilter;
            WebDarwinCoreInformation darwinCoreInformation;

            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                speciesObservationResponse = elastisearch.GetSpeciesObservations(filter);
            }

            darwinCoreInformation = new WebDarwinCoreInformation();
            if (speciesObservationResponse.TimedOut)
            {
                throw new Exception("The question timed out! Filter = " + filter);
            }

            if (searchCriteria.IsNotNull() &&
                (speciesObservationResponse.DocumentCount >
                 Settings.Default.MaxSpeciesObservation))
            {
                // Too many species observations with ids.
                throw new ApplicationException("Too many species observations was retrieved!, Limit is set to " +
                                               Settings.Default.MaxSpeciesObservation + " observations.");
            }

            if (speciesObservationResponse.DocumentCount > 0)
            {
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
                }

                startIndex = 0;

                if (searchCriteria.IsNotNull() &&
                    (speciesObservationResponse.DocumentCount >
                     Settings.Default.MaxSpeciesObservationWithInformation))
                {
                    // Get ids only.
                    idFilter = new StringBuilder();
                    idFilter.Append("{");
                    idFilter.Append(" \"size\": " + speciesObservationResponse.DocumentCount);
                    idFilter.Append(", \"_source\":{\"include\":[");
                    idFilter.Append("\"");
                    using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                    {
                        idFilter.Append(elastisearch.GetFieldName(SpeciesObservationClassId.DarwinCore,
                                                                  SpeciesObservationPropertyId.Id));
                    }

                    idFilter.Append("\"]}");
                    idFilter.Append(", " + searchCriteria.GetFilter(context, false));
                    idFilter.Append("}");
                    using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                    {
                        speciesObservationResponse = elastisearch.GetSpeciesObservations(idFilter.ToString());
                    }

                    darwinCoreInformation.SpeciesObservationIds = new List<Int64>();
                    while (startIndex < (speciesObservationResponse.DocumentsJson.Length - 10))
                    {
                        startIndex = GetSpeciesObservationId(context,
                                                             darwinCoreInformation.SpeciesObservationIds,
                                                             speciesObservationResponse.DocumentsJson,
                                                             mapping,
                                                             startIndex);
                    }
                }
                else
                {
                    darwinCoreInformation.SpeciesObservations = new List<WebDarwinCore>();
                    while (startIndex < (speciesObservationResponse.DocumentsJson.Length - 10))
                    {
                        startIndex = GetDarwinCore(darwinCoreInformation.SpeciesObservations,
                                                   speciesObservationResponse.DocumentsJson,
                                                   mapping,
                                                   startIndex);
                    }
                }
            }

            darwinCoreInformation.SetCount();
            ConvertToRequestedCoordinates(context, darwinCoreInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, darwinCoreInformation.SpeciesObservations, methodName);
            return darwinCoreInformation;
        }

        /// <summary>
        /// Get information about fields in species
        /// observations that should not be returned.
        /// This information is used in calls to Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>
        /// Information about fields in species
        /// observations that should not be returned.
        /// </returns>
        private static String GetExcludedFields(WebServiceContext context)
        {
            FieldDefinitionList fieldDefinitions;
            String excludedFields;
            String[] splitFieldName;
            StringBuilder excludedFieldsBuilder;

            // Get data from cache.
            excludedFields = (String)context.GetCachedObject(Settings.Default.SpeciesObservationExcludedFieldsCacheKey);

            if (excludedFields.IsNull())
            {
                // Generate string with information about excluded fields.
                excludedFieldsBuilder = new StringBuilder();
                excludedFieldsBuilder.Append("\"_source\":{\"exclude\":[" +
                    ////                                  "\"CoordinateUncertaintyInMeters\"," +
                                      "\"DarwinCore_DataProviderId\"," +
                    ////                                  "\"DisturbanceRadius\"," +
                    ////                                  "\"Location\"," +
                                      "\"Location_CoordinateXRt90\"," +
                                      "\"Location_CoordinateXSweref99\"," +
                                      "\"Location_CoordinateYRt90\"," +
                                      "\"Location_CoordinateYSweref99\"," +
                                      "\"Location_DisturbanceRadius\"," +
                                      "\"Location_MaxCoordinateUncertaintyInMetersOrDisturbanceRadius\"," +
                    ////                                  "\"ObservationDateTimeAccuracy\"," +
                                      "\"Occurrence_BirdNestActivityId\"," +
                                      "\"Occurrence_SpeciesActivityId\"");
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    fieldDefinitions = elastisearch.GetSpeciesObservationMapping();
                }

                if (fieldDefinitions.IsNotEmpty())
                {
                    foreach (FieldDefinition fieldDefinition in fieldDefinitions)
                    {
                        // TODO: remove the last test when database has been updated.
                        splitFieldName = fieldDefinition.Name.Split('_');
                        if ((splitFieldName.Length != 2) &&
                            !(fieldDefinition.Name.StartsWith("ProjectParameter") ||
                              fieldDefinition.Name.StartsWith("Project_ProjectParameter")))
                        {
                            excludedFieldsBuilder.Append(",\"" + fieldDefinition.Name + "\"");
                        }
                    }
                }

                excludedFieldsBuilder.Append("]}");
                excludedFields = excludedFieldsBuilder.ToString();

                // Store data in cache.
                context.AddCachedObject(Settings.Default.SpeciesObservationExcludedFieldsCacheKey,
                                        excludedFields,
                                        DateTime.Now + new TimeSpan(0, 3, 0, 0),
                                        CacheItemPriority.High);
            }

            return excludedFields;
        }

        /// <summary>
        /// Add information about coordinate system, data provider
        /// and taxon to species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="webDarwinCores">Species observations in Darwin Core compatible format.</param>
        /// <param name="coordinateSystem">The coordinate system.</param>
        private static void GetExternalInformation(WebServiceContext context,
                                                   List<WebDarwinCore> webDarwinCores,
                                                   WebCoordinateSystem coordinateSystem)
        {
            Dictionary<Int32, WebSpeciesObservationDataProvider> dataProviders;
            Dictionary<Int32, TaxonInformation> taxonInformations;
            String coordinateSystemWkt;
            TaxonInformation taxonInformation;
            WebSpeciesObservationDataProvider dataProvider;

            if (webDarwinCores.IsNotEmpty())
            {
                coordinateSystemWkt = coordinateSystem.GetWkt();
                dataProviders = WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProvidersDictionary(context);
                taxonInformations = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);

                foreach (WebDarwinCore webDarwinCore in webDarwinCores)
                {
                    dataProvider = dataProviders[webDarwinCore.DatasetID.WebParseInt32()];
                    taxonInformation = taxonInformations[webDarwinCore.Taxon.DyntaxaTaxonID];

                    // Load species observation information.
                    webDarwinCore.DatasetID = dataProvider.Guid;
                    webDarwinCore.DatasetName = dataProvider.Name;

                    // Load conservation information.
                    webDarwinCore.Conservation.ActionPlan = taxonInformation.ActionPlan;
                    webDarwinCore.Conservation.ConservationRelevant = taxonInformation.ConservationRelevant;
                    webDarwinCore.Conservation.Natura2000 = taxonInformation.Natura2000;
                    webDarwinCore.Conservation.ProtectedByLaw = taxonInformation.ProtectedByLaw;
                    webDarwinCore.Conservation.RedlistCategory = taxonInformation.RedlistCategory;
                    webDarwinCore.Conservation.SwedishImmigrationHistory = taxonInformation.SwedishImmigrationHistory;
                    webDarwinCore.Conservation.SwedishOccurrence = taxonInformation.SwedishOccurrence;

                    // Load location information.
                    webDarwinCore.Location.CoordinateSystemWkt = coordinateSystemWkt;

                    // Load taxon information.
                    //// webDarwinCore.Taxon.AcceptedNameUsage = taxonInformation.AcceptedNameUsage;
                    //// webDarwinCore.Taxon.AcceptedNameUsageID = taxonInformation.AcceptedNameUsageID;
                    webDarwinCore.Taxon.Class = taxonInformation.Class;
                    webDarwinCore.Taxon.Family = taxonInformation.Family;
                    webDarwinCore.Taxon.Genus = taxonInformation.Genus;
                    webDarwinCore.Taxon.HigherClassification = taxonInformation.HigherClassification;
                    webDarwinCore.Taxon.InfraspecificEpithet = taxonInformation.InfraspecificEpithet;
                    webDarwinCore.Taxon.Kingdom = taxonInformation.Kingdom;
                    webDarwinCore.Taxon.NameAccordingTo = taxonInformation.NameAccordingTo;
                    webDarwinCore.Taxon.NameAccordingToID = taxonInformation.NameAccordingToId;
                    webDarwinCore.Taxon.NamePublishedIn = taxonInformation.NamePublishedIn;
                    webDarwinCore.Taxon.NamePublishedInID = taxonInformation.NamePublishedInId;
                    webDarwinCore.Taxon.NamePublishedInYear = taxonInformation.NamePublishedInYear;
                    webDarwinCore.Taxon.NomenclaturalCode = taxonInformation.NomenclaturalCode;
                    webDarwinCore.Taxon.NomenclaturalStatus = taxonInformation.NomenclaturalStatus;
                    webDarwinCore.Taxon.Order = taxonInformation.Order;
                    webDarwinCore.Taxon.OrganismGroup = taxonInformation.OrganismGroup;
                    webDarwinCore.Taxon.OriginalNameUsage = taxonInformation.OriginalNameUsage;
                    webDarwinCore.Taxon.OriginalNameUsageID = taxonInformation.OriginalNameUsageId;
                    //// webDarwinCore.Taxon.ParentNameUsage = taxonInformation.ParentNameUsage;
                    //// webDarwinCore.Taxon.ParentNameUsageID = taxonInformation.ParentNameUsageID;
                    webDarwinCore.Taxon.Phylum = taxonInformation.Phylum;
                    webDarwinCore.Taxon.ScientificName = taxonInformation.ScientificName;
                    webDarwinCore.Taxon.ScientificNameAuthorship = taxonInformation.ScientificNameAuthorship;
                    webDarwinCore.Taxon.ScientificNameID = taxonInformation.ScientificNameId;
                    webDarwinCore.Taxon.SpecificEpithet = taxonInformation.SpecificEpithet;
                    webDarwinCore.Taxon.Subgenus = taxonInformation.Subgenus;
                    webDarwinCore.Taxon.TaxonConceptID = taxonInformation.TaxonConceptId;
                    webDarwinCore.Taxon.TaxonConceptStatus = taxonInformation.TaxonConceptStatus;
                    webDarwinCore.Taxon.TaxonID = taxonInformation.TaxonConceptId;
                    webDarwinCore.Taxon.TaxonomicStatus = taxonInformation.TaxonomicStatus;
                    webDarwinCore.Taxon.TaxonRank = taxonInformation.TaxonRank;
                    webDarwinCore.Taxon.TaxonRemarks = taxonInformation.TaxonRemarks;
                    // Support for field taxon sort order has been removed since data
                    // update handling (species observation) could not cope with all changes. 
                    // webDarwinCore.Taxon.TaxonSortOrder = taxonInformation.TaxonSortOrder;
                    webDarwinCore.Taxon.VernacularName = taxonInformation.VernacularName;
                }
            }
        }

        /// <summary>
        /// Get information related to which fields that are returned.
        /// This information is used in calls to Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationSpecification">Species observation specification.</param>
        /// <returns>Information related to which fields that are returned.</returns>
        public static String GetFields(WebServiceContext context,
                                       WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            Boolean isFirstField;
            FieldDefinitionList fieldDefinitions;
            List<WebSpeciesObservationFieldSpecification> fieldSpecifications;
            String[] splitFieldName;
            StringBuilder fieldsBuilder;

            fieldSpecifications = speciesObservationSpecification.GetFields();
            if (fieldSpecifications.IsEmpty())
            {
                return GetExcludedFields(context);
            }

            // Merge fields that must be included for species
            // observation handling to be correct.
            fieldSpecifications.Merge(GetSpeciesObservationFields());

            // Generate string with information about included fields.
            fieldsBuilder = new StringBuilder();
            fieldsBuilder.Append("\"_source\":{\"include\":[");
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                fieldDefinitions = elastisearch.GetSpeciesObservationMapping();
            }

            if (fieldDefinitions.IsNotEmpty())
            {
                isFirstField = true;
                foreach (FieldDefinition fieldDefinition in fieldDefinitions)
                {
                    splitFieldName = fieldDefinition.Name.Split('_');
                    if ((splitFieldName.Length == 2) &&
                        (fieldSpecifications.ContainsFieldSpecification(splitFieldName[0], splitFieldName[1])))
                    {
                        if (isFirstField)
                        {
                            isFirstField = false;
                        }
                        else
                        {
                            fieldsBuilder.Append(",");
                        }

                        fieldsBuilder.Append("\"" + fieldDefinition.Name + "\"");
                    }
                }
            }

            fieldsBuilder.Append("]}");

            return fieldsBuilder.ToString();
        }

        /// <summary>
        /// Get min species observation protection level in
        /// authorities of type sighting indication.
        /// This value is used in method
        /// GetProtectedSpeciesObservationIndication.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>
        /// Min species observation protection level in
        /// authorities of type sighting indication.
        /// </returns>
        private static Int32 GetMinProtectionLevel(WebServiceContext context)
        {
            Int32 minProtectionLevel;
            List<WebRole> currentRoles;

            if (context.GetUser().Type == UserType.Application)
            {
                minProtectionLevel = 1;
            }
            else
            {
                currentRoles = context.CurrentRoles;
                minProtectionLevel = Int32.MaxValue;
                foreach (WebRole role in currentRoles)
                {
                    if (role.Authorities.IsNotEmpty())
                    {
                        foreach (WebAuthority authority in role.Authorities)
                        {
                            if (authority.Identifier == AuthorityIdentifier.SightingIndication.ToString())
                            {
                                minProtectionLevel = Math.Min(minProtectionLevel,
                                                              authority.MaxProtectionLevel);
                            }
                        }
                    }
                }

                if (minProtectionLevel < Int32.MaxValue)
                {
                    minProtectionLevel += 1;
                }
            }

            return minProtectionLevel;
        }

        /// <summary>
        /// Get taxa that is protected in Natura 2000.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Ids for taxa that is protected in Natura 2000.</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "Reviewed. Suppression is OK here.")]
        private static DataIdInt32List GetNatura2000TaxonIds(WebServiceContext context)
        {
            Dictionary<Int32, TaxonInformation> taxonInformationCache;
            DataIdInt32List natura2000TaxonIds;

            taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
            natura2000TaxonIds = new DataIdInt32List(true);
            foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
            {
                if (taxonInformation.Natura2000)
                {
                    natura2000TaxonIds.Add(taxonInformation.DyntaxaTaxonId);
                }
            }

            return natura2000TaxonIds;
        }

        /// <summary>
        /// Get taxa that is protected by law.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Ids for taxa that is protected by law.</returns>
        private static DataIdInt32List GetProtectedByLawTaxonIds(WebServiceContext context)
        {
            DataIdInt32List protectedByLawTaxonIds;
            List<WebSpeciesFact> speciesFacts;
            WebSpeciesFactSearchCriteria searchCriteria;

            // Get species facts.
            // We can not use TaxonInformation.ProtectedByLaw
            // since we want all protected taxa whether they are
            // locally or nationally protected.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            searchCriteria.FactorIds.Add((Int32)(FactorId.ProtectedByLaw));
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add((Int32)(IndividualCategoryId.Default));
            speciesFacts = WebServiceData.SpeciesFactManager.GetSpeciesFactsBySearchCriteria(context, searchCriteria);

            // Get taxon ids.
            protectedByLawTaxonIds = new DataIdInt32List(true);
            foreach (WebSpeciesFact speciesFact in speciesFacts)
            {
                protectedByLawTaxonIds.Merge(speciesFact.TaxonId);
            }

            return protectedByLawTaxonIds;
        }

        /// <summary>
        /// Get an indication if specified geometries contains any
        /// protected species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// At least one of BoundingBox, Polygons and RegionGuids
        /// must be specified.
        /// Search criteria that may be used: Accuracy,
        /// BirdNestActivityLimit, BoundingBox, IsAccuracyConsidered,
        /// IsDisturbanceSensitivityConsidered, MinProtectionLevel,
        /// ObservationDateTime, Polygons and RegionGuids.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// True, if specified geometries contains any
        /// protected species observations.
        /// </returns>
        public static Boolean GetProtectedSpeciesObservationIndication(WebServiceContext context,
                                                                       WebSpeciesObservationSearchCriteria searchCriteria,
                                                                       WebCoordinateSystem coordinateSystem)
        {
            Int32 rowCounter;
            String geometryWhereCondition, joinCondition, whereCondition;
            WebDarwinCore webDarwinCore;
            WebDarwinCoreInformation webDarwinCoreInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.SightingIndication);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            CheckSearchCriteria(context, searchCriteria, false);
            coordinateSystem.CheckData();

            // Get the conditions.
            geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
            joinCondition = searchCriteria.GetJoinCondition();
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);

            using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetProtectedSpeciesObservationIndication(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                            searchCriteria.GetRegionIds(context),
                                                                                                                            searchCriteria.TaxonIds,
                                                                                                                            joinCondition,
                                                                                                                            whereCondition,
                                                                                                                            geometryWhereCondition))
            {
                rowCounter = 1;
                webDarwinCoreInformation = new WebDarwinCoreInformation();
                webDarwinCoreInformation.SpeciesObservations = new List<WebDarwinCore>();
                while (dataReader.Read() && (rowCounter < Settings.Default.MaxProtectedSpeciesObservationIndications))
                {
                    rowCounter++;
                    webDarwinCore = new WebDarwinCore();
                    webDarwinCore.LoadProtectedSpeciesObservationIndication(dataReader);
                    if (CheckIndicationAccessRights(context, webDarwinCore))
                    {
                        webDarwinCoreInformation.SpeciesObservations.Add(webDarwinCore);
                    }

                    // ELSE: Species observation is used outside
                    // region that the user has access rights to.
                    // This does not have to be a mistake by the user since
                    // the method takes into account the accuracy and
                    // disturbance sensitivity of observations
                    // outside the specified geometries.
                }
            }

            GetExternalInformation(context, webDarwinCoreInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, webDarwinCoreInformation.SpeciesObservations, "GetProtectedSpeciesObservationIndication");

            return webDarwinCoreInformation.SpeciesObservations.IsNotEmpty();
        }

        /// <summary>
        /// Get an indication if specified geometries contains any
        /// protected species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// At least one of BoundingBox, Polygons and RegionGuids
        /// must be specified.
        /// Search criteria that may be used: Accuracy,
        /// BirdNestActivityLimit, BoundingBox, IsAccuracyConsidered,
        /// IsDisturbanceSensitivityConsidered, MinProtectionLevel,
        /// ObservationDateTime, Polygons and RegionGuids.
        /// </param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// True, if specified geometries contains any
        /// protected species observations.
        /// </returns>
        public static Boolean GetProtectedSpeciesObservationIndicationElasticsearch(WebServiceContext context,
                                                                                    WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                    WebCoordinateSystem coordinateSystem)
        {
            StringBuilder filter;
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.SightingIndication);

            // Check that data is valid.
            coordinateSystem.CheckData();
            CheckSearchCriteria(context, searchCriteria, true);

            // Get filter.
            searchCriteria.Polygons = WebSpeciesObservationServiceData.SpeciesObservationManager.ConvertToElasticSearchCoordinates(context,
                                                                        searchCriteria.Polygons,
                                                                        searchCriteria.RegionGuids,
                                                                        coordinateSystem);
            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"size\": " + Settings.Default.MaxProtectedSpeciesObservationIndications.WebToString());
            filter.Append(", " + GetProtectedSpeciesObservationIndicationFields(context));
            filter.Append(", " + searchCriteria.GetFilter(context, true));
            filter.Append("}");

            speciesObservationInformation = GetSpeciesObservationInformationElasticsearch(context,
                                                                                          null,
                                                                                          coordinateSystem,
                                                                                          filter.ToString(),
                                                                                          null,
                                                                                          "GetProtectedSpeciesObservationIndicationElasticsearch");
            return speciesObservationInformation.SpeciesObservations.IsNotEmpty();
        }

        /// <summary>
        /// Get information about fields that are included
        /// when protected species observation indication is used.
        /// This information is used in calls to Elasticsearch.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <returns>
        /// Information about fields that are included
        /// when protected species observation indication is used.
        /// This information is used in calls to Elasticsearch.
        /// </returns>
        private static String GetProtectedSpeciesObservationIndicationFields(WebServiceContext context)
        {
            Boolean isFirstField;
            FieldDefinitionList fieldDefinitions;
            String fields;
            String[] splitFieldName;
            StringBuilder fieldsBuilder;

            // Get data from cache.
            fields = (String)context.GetCachedObject(Settings.Default.ProtectedSpeciesObservationIndicationFieldsCacheKey);

            if (fields.IsNull())
            {
                // Generate string with information about included fields.
                fieldsBuilder = new StringBuilder();
                fieldsBuilder.Append("\"_source\":{\"include\":[");
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    fieldDefinitions = elastisearch.GetSpeciesObservationMapping();
                }

                if (fieldDefinitions.IsNotEmpty())
                {
                    isFirstField = true;
                    foreach (FieldDefinition fieldDefinition in fieldDefinitions)
                    {
                        splitFieldName = fieldDefinition.Name.Split('_');
                        if ((splitFieldName.Length == 2) &&
                            IsProtectedSpeciesObservationIndicationField(splitFieldName[0], splitFieldName[1]))
                        {
                            if (isFirstField)
                            {
                                isFirstField = false;
                            }
                            else
                            {
                                fieldsBuilder.Append(",");
                            }

                            fieldsBuilder.Append("\"" + fieldDefinition.Name + "\"");
                        }
                    }
                }

                fieldsBuilder.Append("]}");
                fields = fieldsBuilder.ToString();

                // Store data in cache.
                context.AddCachedObject(Settings.Default.ProtectedSpeciesObservationIndicationFieldsCacheKey,
                                        fields,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return fields;
        }

        /// <summary>
        /// Get fixed list of taxa that is used in protected species
        /// observation indication.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="isElasticsearchUsed">
        /// Indicates if Elsticsearch is used.
        /// Some handling of search criteria differs depending
        /// on which data source that will be used.
        /// </param>
        /// <returns>
        /// Fixed list of taxa that is used in
        /// protected species observation indication.
        /// </returns>
        private static List<Int32> GetProtectedSpeciesObservationIndicationTaxonIds(WebServiceContext context,
                                                                                    Boolean isElasticsearchUsed)
        {
            DataIdInt32List tempTaxonIds;
            List<Int32> taxonIds;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.ProtectedSpeciesObservationIndicationTaxaCacheKey + "#" + context.ClientToken.ApplicationIdentifier + "#IsElasticsearchUsed=" + isElasticsearchUsed;
            taxonIds = (List<Int32>)(context.GetCachedObject(cacheKey));

            if (taxonIds.IsNull())
            {
                switch (context.ClientToken.ApplicationIdentifier)
                {
                    case "SkogsstyrelsenInGe":
                        tempTaxonIds = new DataIdInt32List(true);
                        //tempTaxonIds.AddRange(GetRedlistedTaxonIds(context, true, null));
                        //tempTaxonIds.Merge(GetNatura2000TaxonIds(context));
                        //tempTaxonIds.Merge(GetProtectedByLawTaxonIds(context));
                        tempTaxonIds.Merge(GetSwedishForestAgencyTaxonIds(context));
                        taxonIds = tempTaxonIds.GetInt32List();
                        break;

                    default:
                        tempTaxonIds = new DataIdInt32List(true);
                        tempTaxonIds.AddRange(WebSpeciesObservationServiceData.TaxonManager.GetRedlistedTaxonIds(context, true, null));
                        tempTaxonIds.Merge(GetActionPlanTaxonIds(context));
                        tempTaxonIds.Merge(GetNatura2000TaxonIds(context));
                        tempTaxonIds.Merge(GetProtectedByLawTaxonIds(context));
                        taxonIds = tempTaxonIds.GetInt32List();
                        break;
                }

                if (isElasticsearchUsed)
                {
                    taxonIds = WebServiceData.TaxonManager.GetChildTaxonIds(context, taxonIds);
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonIds,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.Normal);
            }

            return taxonIds;
        }

        /// <summary>
        /// Gets the protected taxa list from ASP.NET cache.
        /// Key in dictionary with protection information is
        /// taxon id and value contains protection level.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Hash table with all protected taxa.</returns>
        private static Dictionary<Int32, Int32> GetProtectionInformation(WebServiceContext context)
        {
            Dictionary<Int32, Int32> protectionInformation;
            Dictionary<Int32, TaxonInformation> taxonInformationCache;

            // Get data from cache.
            protectionInformation = (Dictionary<Int32, Int32>)context.GetCachedObject(Settings.Default.ProtectedTaxaCacheKey);

            // Data not in cache - store it in the cache
            if (protectionInformation.IsNull())
            {
                // Get protection level for all protected taxa.
                taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
                protectionInformation = new Dictionary<Int32, Int32>();
                foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
                {
                    if (taxonInformation.ProtectionLevel > (Int32)(SpeciesProtectionLevelEnum.Public))
                    {
                        protectionInformation[taxonInformation.DyntaxaTaxonId] = taxonInformation.ProtectionLevel;
                    }
                }

                // Store data in cache.
                context.AddCachedObject(Settings.Default.ProtectedTaxaCacheKey,
                                        protectionInformation,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return protectionInformation;
        }

        /// <summary>
        /// Get all province regions
        /// </summary>
        /// <param name="context"></param>
        /// <returns>All province regions</returns>
        public static List<WebRegion> GetProvinceRegions(WebServiceContext context)
        {
            List<WebRegion> regions;
            String cacheKey;
            WebRegion region;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationProvinceRegionsCacheKey;
            regions = (List<WebRegion>)(context.GetCachedObject(cacheKey));

            if (regions.IsEmpty())
            {
                // Data not in cache.
                regions = new List<WebRegion>();

                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetProvinceRegions())
                {
                    while (dataReader.Read())
                    {
                        region = new WebRegion();
                        region.LoadData(dataReader);
                        regions.Add(region);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        regions,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return regions;
        }

        ///// <summary>
        ///// Get information about red listed taxa.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <returns>Information about red listed taxa.</returns>
        //private static Hashtable GetRedlistedTaxa(WebServiceContext context)
        //{
        //    Dictionary<RedListCategory, List<Int32>> redlistedTaxonIds;
        //    Hashtable redlistedTaxaInformation;
        //    List<Int32> allRedlistedTaxonIds;
        //    RedListCategory redListCategory;
        //    Dictionary<Int32, TaxonInformation> taxonInformationCache;

        //    // Get data from cache.
        //    redlistedTaxaInformation = (Hashtable)context.GetCachedObject(GetRedlistedTaxaCacheKey());

        //    if (redlistedTaxaInformation.IsNull())
        //    {
        //        // Data not in cache - store it in the cache.
        //        // Init data structures.
        //        allRedlistedTaxonIds = new List<Int32>();
        //        redlistedTaxaInformation = new Hashtable();
        //        redlistedTaxonIds = new Dictionary<RedListCategory, List<Int32>>();
        //        for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
        //        {
        //            redlistedTaxonIds[redListCategory] = new List<Int32>();
        //        }

        //        // Extract red list information from taxon information.
        //        taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
        //        foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
        //        {
        //            if (taxonInformation.RedlistCategory.IsNotEmpty())
        //            {
        //                allRedlistedTaxonIds.Add(taxonInformation.DyntaxaTaxonId);
        //                switch (taxonInformation.RedlistCategory.Substring(0, 2).ToUpper())
        //                {
        //                    case "CR":
        //                        redlistedTaxonIds[RedListCategory.CR].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                    case "DD":
        //                        redlistedTaxonIds[RedListCategory.DD].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                    case "EN":
        //                        redlistedTaxonIds[RedListCategory.EN].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                    case "NT":
        //                        redlistedTaxonIds[RedListCategory.NT].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                    case "RE":
        //                        redlistedTaxonIds[RedListCategory.RE].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                    case "VU":
        //                        redlistedTaxonIds[RedListCategory.VU].Add(taxonInformation.DyntaxaTaxonId);
        //                        break;
        //                }
        //            }
        //        }

        //        // Save red list information into hashtable.
        //        redlistedTaxaInformation[GetRedlistedTaxaCacheKey()] = allRedlistedTaxonIds;
        //        foreach (RedListCategory tempRedListCategory in redlistedTaxonIds.Keys)
        //        {
        //            redlistedTaxaInformation[GetRedlistedTaxaCacheKey(tempRedListCategory)] = redlistedTaxonIds[tempRedListCategory];
        //        }

        //        // Store data in cache.
        //        context.AddCachedObject(GetRedlistedTaxaCacheKey(),
        //                                redlistedTaxaInformation,
        //                                DateTime.Now + new TimeSpan(1, 0, 0, 0),
        //                                CacheItemPriority.High);
        //    }

        //    return redlistedTaxaInformation;
        //}

        ///// <summary>
        ///// Get cache key for all red listed taxa.
        ///// </summary>
        ///// <returns>Cache key for all red listed taxa.</returns>
        //private static String GetRedlistedTaxaCacheKey()
        //{
        //    return Settings.Default.RedlistedTaxaCacheKey;
        //}

        ///// <summary>
        ///// Get cache key for taxa that is red listed
        ///// in specified red list category.
        ///// </summary>
        ///// <param name="redlistCategory">Cache key for taxa belonging to specified red list category should be returned.</param>
        ///// <returns>Cache key for red listed taxa.</returns>
        //private static String GetRedlistedTaxaCacheKey(RedListCategory redlistCategory)
        //{
        //    return Settings.Default.RedlistedTaxaCacheKey +
        //           WebService.Settings.Default.CacheKeyDelimiter +
        //           redlistCategory;
        //}

        ///// <summary>
        ///// Get taxon ids for red listed taxa.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="includeRedlistedTaxa">If true all red listed taxa should be returned.</param>
        ///// <param name="redlistCategories">Taxa belonging to specified red list categories should be returned.</param>
        ///// <returns>Requested red listed taxa.</returns>
        //private static List<Int32> GetRedlistedTaxonIds(WebServiceContext context,
        //                                                Boolean includeRedlistedTaxa,
        //                                                List<RedListCategory> redlistCategories)
        //{
        //    Hashtable redlistedTaxaInformation;
        //    List<Int32> redlistedTaxonIds;

        //    // Get cached information.
        //    redlistedTaxaInformation = GetRedlistedTaxa(context);

        //    if (includeRedlistedTaxa)
        //    {
        //        redlistedTaxonIds = (List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey()]);
        //    }
        //    else
        //    {
        //        redlistedTaxonIds = new List<Int32>();
        //        if (redlistCategories.IsNotEmpty())
        //        {
        //            foreach (RedListCategory redListCategory in redlistCategories)
        //            {
        //                redlistedTaxonIds.AddRange((List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey(redListCategory)]));
        //            }
        //        }
        //    }

        //    return redlistedTaxonIds;
        //}

        /// <summary>
        /// Convert one species observation from JSON to
        /// class WebSpeciesObservation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observation is stored in this list.</param>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetSpeciesObservation(WebServiceContext context,
                                                   List<WebSpeciesObservation> speciesObservations,
                                                   String speciesObservationsJson,
                                                   Dictionary<String, WebSpeciesObservationField> mapping,
                                                   Int32 startIndex)
        {
            WebSpeciesObservation speciesObservation;
            WebSpeciesObservationField field;

            speciesObservation = new WebSpeciesObservation();
            speciesObservation.Fields = new List<WebSpeciesObservationField>();
            speciesObservations.Add(speciesObservation);

            // Skip general part.
            startIndex = speciesObservationsJson.IndexOf("_source", startIndex, StringComparison.Ordinal);
            startIndex = speciesObservationsJson.IndexOf("{", startIndex, StringComparison.Ordinal) + 1;

            do
            {
                startIndex = GetSpeciesObservationField(context,
                                                        out field,
                                                        speciesObservationsJson,
                                                        mapping,
                                                        startIndex);
                if (field.IsNotNull())
                {
                    speciesObservation.Fields.Add(field);
                }
            }
            while (field.IsNotNull());

            return startIndex;
        }

        /// <summary>
        /// Convert a WebDarwinCore instance to
        /// a WebSpeciesObservation instance.
        /// </summary>
        /// <param name="darwinCoreObservation">Species observation in DarwinCore format.</param>
        /// <returns>A WebSpeciesObservation instance.</returns>
        private static WebSpeciesObservation GetSpeciesObservation(WebDarwinCore darwinCoreObservation)
        {
            WebSpeciesObservation speciesObservation;

            speciesObservation = new WebSpeciesObservation();
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.AccessRights,
                                        darwinCoreObservation.AccessRights);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.BasisOfRecord,
                                        darwinCoreObservation.BasisOfRecord);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.BibliographicCitation,
                                        darwinCoreObservation.BibliographicCitation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.CollectionCode,
                                        darwinCoreObservation.CollectionCode);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.CollectionID,
                                        darwinCoreObservation.CollectionID);
            GetSpeciesObservation(darwinCoreObservation.Conservation, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.DataGeneralizations,
                                        darwinCoreObservation.DataGeneralizations);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.DatasetID,
                                        darwinCoreObservation.DatasetID);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.DatasetName,
                                        darwinCoreObservation.DatasetName);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.DynamicProperties,
                                        darwinCoreObservation.DynamicProperties);
            GetSpeciesObservation(darwinCoreObservation.Event, speciesObservation);
            GetSpeciesObservation(darwinCoreObservation.GeologicalContext, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Id,
                                        darwinCoreObservation.Id);
            GetSpeciesObservation(darwinCoreObservation.Identification, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.InformationWithheld,
                                        darwinCoreObservation.InformationWithheld);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.InstitutionCode,
                                        darwinCoreObservation.InstitutionCode);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.InstitutionID,
                                        darwinCoreObservation.InstitutionID);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Language,
                                        darwinCoreObservation.Language);
            GetSpeciesObservation(darwinCoreObservation.Location, speciesObservation);
            GetSpeciesObservation(darwinCoreObservation.MeasurementOrFact, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Modified,
                                        darwinCoreObservation.Modified);
            GetSpeciesObservation(darwinCoreObservation.Occurrence, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Owner,
                                        darwinCoreObservation.Owner);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.OwnerInstitutionCode,
                                        darwinCoreObservation.OwnerInstitutionCode);
            GetSpeciesObservation(darwinCoreObservation.Project, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.References,
                                        darwinCoreObservation.References);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.ReportedBy,
                                        darwinCoreObservation.ReportedBy);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.ReportedDate,
                                        darwinCoreObservation.ReportedDate);
            GetSpeciesObservation(darwinCoreObservation.ResourceRelationship, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Rights,
                                        darwinCoreObservation.Rights);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.RightsHolder,
                                        darwinCoreObservation.RightsHolder);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.SpeciesObservationURL,
                                        darwinCoreObservation.SpeciesObservationURL);
            GetSpeciesObservation(darwinCoreObservation.Taxon, speciesObservation);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.Type,
                                        darwinCoreObservation.Type);
            speciesObservation.AddField(SpeciesObservationClassId.DarwinCore,
                                        SpeciesObservationPropertyId.ValidationStatus,
                                        darwinCoreObservation.ValidationStatus);

            return speciesObservation;
        }

        /// <summary>
        /// Add DarwinCore conservation information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreConservation">DarwinCore conservation information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreConservation darwinCoreConservation,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreConservation.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.ActionPlan,
                                            darwinCoreConservation.ActionPlan);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.ConservationRelevant,
                                            darwinCoreConservation.ConservationRelevant);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.Natura2000,
                                            darwinCoreConservation.Natura2000);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.ProtectedByLaw,
                                            darwinCoreConservation.ProtectedByLaw);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.ProtectionLevel,
                                            darwinCoreConservation.ProtectionLevel);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.RedlistCategory,
                                            darwinCoreConservation.RedlistCategory);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.SwedishImmigrationHistory,
                                            darwinCoreConservation.SwedishImmigrationHistory);
                speciesObservation.AddField(SpeciesObservationClassId.Conservation,
                                            SpeciesObservationPropertyId.SwedishOccurrence,
                                            darwinCoreConservation.SwedishOccurrence);
            }
        }

        /// <summary>
        /// Add DarwinCore event information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreEvent">DarwinCore event information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreEvent darwinCoreEvent,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreEvent.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.Day,
                                            darwinCoreEvent.Day);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.End,
                                            darwinCoreEvent.End);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.EndDayOfYear,
                                            darwinCoreEvent.EndDayOfYear);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.EventDate,
                                            darwinCoreEvent.EventDate);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.EventID,
                                            darwinCoreEvent.EventID);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.EventRemarks,
                                            darwinCoreEvent.EventRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.EventTime,
                                            darwinCoreEvent.EventTime);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.FieldNotes,
                                            darwinCoreEvent.FieldNotes);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.FieldNumber,
                                            darwinCoreEvent.FieldNumber);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.Habitat,
                                            darwinCoreEvent.Habitat);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.Month,
                                            darwinCoreEvent.Month);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.SamplingEffort,
                                            darwinCoreEvent.SamplingEffort);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.SamplingProtocol,
                                            darwinCoreEvent.SamplingProtocol);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.Start,
                                            darwinCoreEvent.Start);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.StartDayOfYear,
                                            darwinCoreEvent.StartDayOfYear);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.VerbatimEventDate,
                                            darwinCoreEvent.VerbatimEventDate);
                speciesObservation.AddField(SpeciesObservationClassId.Event,
                                            SpeciesObservationPropertyId.Year,
                                            darwinCoreEvent.Year);
            }
        }

        /// <summary>
        /// Add DarwinCore geological context information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreGeologicalContext">DarwinCore geological context information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreGeologicalContext darwinCoreGeologicalContext,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreGeologicalContext.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.Bed,
                                            darwinCoreGeologicalContext.Bed);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.EarliestAgeOrLowestStage,
                                            darwinCoreGeologicalContext.EarliestAgeOrLowestStage);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.EarliestEonOrLowestEonothem,
                                            darwinCoreGeologicalContext.EarliestEonOrLowestEonothem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.EarliestEpochOrLowestSeries,
                                            darwinCoreGeologicalContext.EarliestEpochOrLowestSeries);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.EarliestEraOrLowestErathem,
                                            darwinCoreGeologicalContext.EarliestEraOrLowestErathem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.EarliestPeriodOrLowestSystem,
                                            darwinCoreGeologicalContext.EarliestPeriodOrLowestSystem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.Formation,
                                            darwinCoreGeologicalContext.Formation);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.GeologicalContextID,
                                            darwinCoreGeologicalContext.GeologicalContextID);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.Group,
                                            darwinCoreGeologicalContext.Group);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.HighestBiostratigraphicZone,
                                            darwinCoreGeologicalContext.HighestBiostratigraphicZone);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LatestAgeOrHighestStage,
                                            darwinCoreGeologicalContext.LatestAgeOrHighestStage);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LatestEonOrHighestEonothem,
                                            darwinCoreGeologicalContext.LatestEonOrHighestEonothem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LatestEpochOrHighestSeries,
                                            darwinCoreGeologicalContext.LatestEpochOrHighestSeries);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LatestEraOrHighestErathem,
                                            darwinCoreGeologicalContext.LatestEraOrHighestErathem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LatestPeriodOrHighestSystem,
                                            darwinCoreGeologicalContext.LatestPeriodOrHighestSystem);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LithostratigraphicTerms,
                                            darwinCoreGeologicalContext.LithostratigraphicTerms);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.LowestBiostratigraphicZone,
                                            darwinCoreGeologicalContext.LowestBiostratigraphicZone);
                speciesObservation.AddField(SpeciesObservationClassId.GeologicalContext,
                                            SpeciesObservationPropertyId.Member,
                                            darwinCoreGeologicalContext.Member);
            }
        }

        /// <summary>
        /// Add DarwinCore identification information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreIdentification">DarwinCore identification information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreIdentification darwinCoreIdentification,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreIdentification.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.DateIdentified,
                                            darwinCoreIdentification.DateIdentified);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentificationID,
                                            darwinCoreIdentification.IdentificationID);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentificationQualifier,
                                            darwinCoreIdentification.IdentificationQualifier);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentificationReferences,
                                            darwinCoreIdentification.IdentificationReferences);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentificationRemarks,
                                            darwinCoreIdentification.IdentificationRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentificationVerificationStatus,
                                            darwinCoreIdentification.IdentificationVerificationStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.IdentifiedBy,
                                            darwinCoreIdentification.IdentifiedBy);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.TypeStatus,
                                            darwinCoreIdentification.TypeStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Identification,
                                            SpeciesObservationPropertyId.UncertainDetermination,
                                            darwinCoreIdentification.UncertainDetermination);
            }
        }

        /// <summary>
        /// Add DarwinCore location information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreLocation">DarwinCore location information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreLocation darwinCoreLocation,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreLocation.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Continent,
                                            darwinCoreLocation.Continent);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateM,
                                            darwinCoreLocation.CoordinateM);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinatePrecision,
                                            darwinCoreLocation.CoordinatePrecision);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateSystemWkt,
                                            darwinCoreLocation.CoordinateSystemWkt);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateUncertaintyInMeters,
                                            darwinCoreLocation.CoordinateUncertaintyInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateX,
                                            darwinCoreLocation.CoordinateX);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateY,
                                            darwinCoreLocation.CoordinateY);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CoordinateZ,
                                            darwinCoreLocation.CoordinateZ);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Country,
                                            darwinCoreLocation.Country);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.CountryCode,
                                            darwinCoreLocation.CountryCode);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.County,
                                            darwinCoreLocation.County);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.DecimalLatitude,
                                            darwinCoreLocation.DecimalLatitude);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.DecimalLongitude,
                                            darwinCoreLocation.DecimalLongitude);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.FootprintSpatialFit,
                                            darwinCoreLocation.FootprintSpatialFit);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.FootprintSRS,
                                            darwinCoreLocation.FootprintSRS);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.FootprintWKT,
                                            darwinCoreLocation.FootprintWKT);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeodeticDatum,
                                            darwinCoreLocation.GeodeticDatum);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferencedBy,
                                            darwinCoreLocation.GeoreferencedBy);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferencedDate,
                                            darwinCoreLocation.GeoreferencedDate);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferenceProtocol,
                                            darwinCoreLocation.GeoreferenceProtocol);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferenceRemarks,
                                            darwinCoreLocation.GeoreferenceRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferenceSources,
                                            darwinCoreLocation.GeoreferenceSources);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.GeoreferenceVerificationStatus,
                                            darwinCoreLocation.GeoreferenceVerificationStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.HigherGeography,
                                            darwinCoreLocation.HigherGeography);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.HigherGeographyID,
                                            darwinCoreLocation.HigherGeographyID);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Island,
                                            darwinCoreLocation.Island);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.IslandGroup,
                                            darwinCoreLocation.IslandGroup);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Locality,
                                            darwinCoreLocation.Locality);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.LocationAccordingTo,
                                            darwinCoreLocation.LocationAccordingTo);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.LocationId,
                                            darwinCoreLocation.LocationId);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.LocationRemarks,
                                            darwinCoreLocation.LocationRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.LocationURL,
                                            darwinCoreLocation.LocationURL);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MaximumDepthInMeters,
                                            darwinCoreLocation.MaximumDepthInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MaximumDistanceAboveSurfaceInMeters,
                                            darwinCoreLocation.MaximumDistanceAboveSurfaceInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MaximumElevationInMeters,
                                            darwinCoreLocation.MaximumElevationInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MinimumDepthInMeters,
                                            darwinCoreLocation.MinimumDepthInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MinimumDistanceAboveSurfaceInMeters,
                                            darwinCoreLocation.MinimumDistanceAboveSurfaceInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.MinimumElevationInMeters,
                                            darwinCoreLocation.MinimumElevationInMeters);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Municipality,
                                            darwinCoreLocation.Municipality);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.Parish,
                                            darwinCoreLocation.Parish);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.PointRadiusSpatialFit,
                                            darwinCoreLocation.PointRadiusSpatialFit);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.StateProvince,
                                            darwinCoreLocation.StateProvince);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimCoordinates,
                                            darwinCoreLocation.VerbatimCoordinates);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimCoordinateSystem,
                                            darwinCoreLocation.VerbatimCoordinateSystem);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimDepth,
                                            darwinCoreLocation.VerbatimDepth);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimElevation,
                                            darwinCoreLocation.VerbatimElevation);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimLatitude,
                                            darwinCoreLocation.VerbatimLatitude);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimLocality,
                                            darwinCoreLocation.VerbatimLocality);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimLongitude,
                                            darwinCoreLocation.VerbatimLongitude);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.VerbatimSRS,
                                            darwinCoreLocation.VerbatimSRS);
                speciesObservation.AddField(SpeciesObservationClassId.Location,
                                            SpeciesObservationPropertyId.WaterBody,
                                            darwinCoreLocation.WaterBody);
            }
        }

        /// <summary>
        /// Add DarwinCore measurement or fact information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreMeasurementOrFact">DarwinCore measurement or fact information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreMeasurementOrFact darwinCoreMeasurementOrFact,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreMeasurementOrFact.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementAccuracy,
                                            darwinCoreMeasurementOrFact.MeasurementAccuracy);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementDeterminedBy,
                                            darwinCoreMeasurementOrFact.MeasurementDeterminedBy);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementDeterminedDate,
                                            darwinCoreMeasurementOrFact.MeasurementDeterminedDate);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementID,
                                            darwinCoreMeasurementOrFact.MeasurementID);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementMethod,
                                            darwinCoreMeasurementOrFact.MeasurementMethod);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementRemarks,
                                            darwinCoreMeasurementOrFact.MeasurementRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementType,
                                            darwinCoreMeasurementOrFact.MeasurementType);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementUnit,
                                            darwinCoreMeasurementOrFact.MeasurementUnit);
                speciesObservation.AddField(SpeciesObservationClassId.MeasurementOrFact,
                                            SpeciesObservationPropertyId.MeasurementValue,
                                            darwinCoreMeasurementOrFact.MeasurementValue);
            }
        }

        /// <summary>
        /// Add DarwinCore occurrence information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreOccurrence">DarwinCore occurrence information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreOccurrence darwinCoreOccurrence,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreOccurrence.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.AssociatedMedia,
                                            darwinCoreOccurrence.AssociatedMedia);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.AssociatedOccurrences,
                                            darwinCoreOccurrence.AssociatedOccurrences);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.AssociatedReferences,
                                            darwinCoreOccurrence.AssociatedReferences);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.AssociatedSequences,
                                            darwinCoreOccurrence.AssociatedSequences);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.AssociatedTaxa,
                                            darwinCoreOccurrence.AssociatedTaxa);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Behavior,
                                            darwinCoreOccurrence.Behavior);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.CatalogNumber,
                                            darwinCoreOccurrence.CatalogNumber);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Disposition,
                                            darwinCoreOccurrence.Disposition);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.EstablishmentMeans,
                                            darwinCoreOccurrence.EstablishmentMeans);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IndividualCount,
                                            darwinCoreOccurrence.IndividualCount);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IndividualID,
                                            darwinCoreOccurrence.IndividualID);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IsNaturalOccurrence,
                                            darwinCoreOccurrence.IsNaturalOccurrence);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IsNeverFoundObservation,
                                            darwinCoreOccurrence.IsNeverFoundObservation);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IsNotRediscoveredObservation,
                                            darwinCoreOccurrence.IsNotRediscoveredObservation);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.IsPositiveObservation,
                                            darwinCoreOccurrence.IsPositiveObservation);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.LifeStage,
                                            darwinCoreOccurrence.LifeStage);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.OccurrenceID,
                                            darwinCoreOccurrence.OccurrenceID);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.OccurrenceRemarks,
                                            darwinCoreOccurrence.OccurrenceRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.OccurrenceStatus,
                                            darwinCoreOccurrence.OccurrenceStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.OccurrenceURL,
                                            darwinCoreOccurrence.OccurrenceURL);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.OtherCatalogNumbers,
                                            darwinCoreOccurrence.OtherCatalogNumbers);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Preparations,
                                            darwinCoreOccurrence.Preparations);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.PreviousIdentifications,
                                            darwinCoreOccurrence.PreviousIdentifications);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Quantity,
                                            darwinCoreOccurrence.Quantity);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.QuantityUnit,
                                            darwinCoreOccurrence.QuantityUnit);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.RecordedBy,
                                            darwinCoreOccurrence.RecordedBy);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.RecordNumber,
                                            darwinCoreOccurrence.RecordNumber);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.ReproductiveCondition,
                                            darwinCoreOccurrence.ReproductiveCondition);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Sex,
                                            darwinCoreOccurrence.Sex);
                speciesObservation.AddField(SpeciesObservationClassId.Occurrence,
                                            SpeciesObservationPropertyId.Substrate,
                                            darwinCoreOccurrence.Substrate);
            }
        }

        /// <summary>
        /// Add DarwinCore project information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreProject">DarwinCore project information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreProject darwinCoreProject,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreProject.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.IsPublic,
                                            darwinCoreProject.IsPublic);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectCategory,
                                            darwinCoreProject.ProjectCategory);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectDescription,
                                            darwinCoreProject.ProjectDescription);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectEndDate,
                                            darwinCoreProject.ProjectEndDate);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectID,
                                            darwinCoreProject.ProjectID);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectName,
                                            darwinCoreProject.ProjectName);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectOwner,
                                            darwinCoreProject.ProjectOwner);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectStartDate,
                                            darwinCoreProject.ProjectStartDate);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.ProjectURL,
                                            darwinCoreProject.ProjectURL);
                speciesObservation.AddField(SpeciesObservationClassId.Project,
                                            SpeciesObservationPropertyId.SurveyMethod,
                                            darwinCoreProject.SurveyMethod);
            }
        }

        /// <summary>
        /// Add DarwinCore resource relationship information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreResourceRelationship">DarwinCore resource relationship information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreResourceRelationship darwinCoreResourceRelationship,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreResourceRelationship.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.RelatedResourceID,
                                            darwinCoreResourceRelationship.RelatedResourceID);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.RelationshipAccordingTo,
                                            darwinCoreResourceRelationship.RelationshipAccordingTo);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.RelationshipEstablishedDate,
                                            darwinCoreResourceRelationship.RelationshipEstablishedDate);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.RelationshipOfResource,
                                            darwinCoreResourceRelationship.RelationshipOfResource);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.RelationshipRemarks,
                                            darwinCoreResourceRelationship.RelationshipRemarks);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.ResourceID,
                                            darwinCoreResourceRelationship.ResourceID);
                speciesObservation.AddField(SpeciesObservationClassId.ResourceRelationship,
                                            SpeciesObservationPropertyId.ResourceRelationshipID,
                                            darwinCoreResourceRelationship.ResourceRelationshipID);
            }
        }

        /// <summary>
        /// Add DarwinCore taxon information
        /// to a species observation.
        /// </summary>
        /// <param name="darwinCoreTaxon">DarwinCore taxon information.</param>
        /// <param name="speciesObservation">Species observation.</param>
        private static void GetSpeciesObservation(WebDarwinCoreTaxon darwinCoreTaxon,
                                                  WebSpeciesObservation speciesObservation)
        {
            if (darwinCoreTaxon.IsNotNull())
            {
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.AcceptedNameUsage,
                                            darwinCoreTaxon.AcceptedNameUsage);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.AcceptedNameUsageID,
                                            darwinCoreTaxon.AcceptedNameUsageID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Class,
                                            darwinCoreTaxon.Class);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.DyntaxaTaxonID,
                                            darwinCoreTaxon.DyntaxaTaxonID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Family,
                                            darwinCoreTaxon.Family);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Genus,
                                            darwinCoreTaxon.Genus);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.HigherClassification,
                                            darwinCoreTaxon.HigherClassification);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.InfraspecificEpithet,
                                            darwinCoreTaxon.InfraspecificEpithet);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Kingdom,
                                            darwinCoreTaxon.Kingdom);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NameAccordingTo,
                                            darwinCoreTaxon.NameAccordingTo);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NameAccordingToID,
                                            darwinCoreTaxon.NameAccordingToID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NamePublishedIn,
                                            darwinCoreTaxon.NamePublishedIn);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NamePublishedInID,
                                            darwinCoreTaxon.NamePublishedInID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NamePublishedInYear,
                                            darwinCoreTaxon.NamePublishedInYear);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NomenclaturalCode,
                                            darwinCoreTaxon.NomenclaturalCode);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.NomenclaturalStatus,
                                            darwinCoreTaxon.NomenclaturalStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Order,
                                            darwinCoreTaxon.Order);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.OrganismGroup,
                                            darwinCoreTaxon.OrganismGroup);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.OriginalNameUsage,
                                            darwinCoreTaxon.OriginalNameUsage);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.OriginalNameUsageID,
                                            darwinCoreTaxon.OriginalNameUsageID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.ParentNameUsage,
                                            darwinCoreTaxon.ParentNameUsage);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.ParentNameUsageID,
                                            darwinCoreTaxon.ParentNameUsageID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Phylum,
                                            darwinCoreTaxon.Phylum);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.ScientificName,
                                            darwinCoreTaxon.ScientificName);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.ScientificNameAuthorship,
                                            darwinCoreTaxon.ScientificNameAuthorship);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.ScientificNameID,
                                            darwinCoreTaxon.ScientificNameID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.SpecificEpithet,
                                            darwinCoreTaxon.SpecificEpithet);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.Subgenus,
                                            darwinCoreTaxon.Subgenus);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonConceptID,
                                            darwinCoreTaxon.TaxonConceptID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonConceptStatus,
                                            darwinCoreTaxon.TaxonConceptStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonID,
                                            darwinCoreTaxon.TaxonID);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonomicStatus,
                                            darwinCoreTaxon.TaxonomicStatus);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonRank,
                                            darwinCoreTaxon.TaxonRank);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonRemarks,
                                            darwinCoreTaxon.TaxonRemarks);
                //speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                //                            SpeciesObservationPropertyId.TaxonSortOrder,
                //                            darwinCoreTaxon.TaxonSortOrder);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.TaxonURL,
                                            darwinCoreTaxon.TaxonURL);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.VerbatimScientificName,
                                            darwinCoreTaxon.VerbatimScientificName);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.VerbatimTaxonRank,
                                            darwinCoreTaxon.VerbatimTaxonRank);
                speciesObservation.AddField(SpeciesObservationClassId.Taxon,
                                            SpeciesObservationPropertyId.VernacularName,
                                            darwinCoreTaxon.VernacularName);
            }
        }

        /// <summary>
        /// Get information about species observations that has
        /// changed.
        /// 
        /// Scope is restricted to those observations that the
        /// user has access rights to. There is no access right
        /// check on deleted species observations. This means
        /// that a client can obtain information about deleted
        /// species observations that the client has not
        /// received any create or update information about.
        /// 
        /// Max 25000 species observation changes can be
        /// retrieved in one web service call.
        /// Exactly one of the parameters changedFrom and 
        /// changeId should be specified.
        /// </summary>
        /// <param name="context">Web service request context. </param>
        /// <param name="changedFrom">Start date and time for changes that should be returned.</param>
        /// <param name="isChangedFromSpecified">Indicates if parameter changedFrom should be used.</param>
        /// <param name="changedTo">
        /// End date and time for changes that should be
        /// returned. Parameter changedTo is optional and works
        /// with either parameter changedFrom or changeId.
        /// </param>
        /// <param name="isChangedToSpecified">Indicates if parameter changedTo should be used.</param>
        /// <param name="changeId">
        /// Start id for changes that should be returned.
        /// The species observation that is changed in the
        /// specified change id may be included in returned
        /// information.
        /// </param>
        /// <param name="isChangedIdSpecified">Indicates if parameter changeId should be used. </param>
        /// <param name="maxReturnedChanges">
        /// Requested maximum number of changes that should
        /// be returned. This property is used by the client
        /// to avoid problems with resource limitations on
        /// the client side.
        /// Max 25000 changes are returned if property
        /// maxChanges has a higher value than 25000.
        /// </param>
        /// <param name="searchCriteria">
        /// Only species observations that matches the search 
        /// criteria are included in the returned information.
        /// This parameter is optional and may be null.
        /// There is no check on search criteria for
        /// deleted species observations. </param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>
        /// Information about changed species observations.
        /// </returns>
        public static WebSpeciesObservationChange GetSpeciesObservationChange(WebServiceContext context,
                                                                              DateTime changedFrom,
                                                                              Boolean isChangedFromSpecified,
                                                                              DateTime changedTo,
                                                                              Boolean isChangedToSpecified,
                                                                              Int64 changeId,
                                                                              Boolean isChangedIdSpecified,
                                                                              Int64 maxReturnedChanges,
                                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                                              WebCoordinateSystem coordinateSystem,
                                                                              WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            WebDarwinCoreChange darwinCoreChange;
            WebSpeciesObservationChange speciesObservationChange;

            darwinCoreChange = GetDarwinCoreChange(context,
                                                   changedFrom,
                                                   isChangedFromSpecified,
                                                   changedTo,
                                                   isChangedToSpecified,
                                                   changeId,
                                                   isChangedIdSpecified,
                                                   maxReturnedChanges,
                                                   searchCriteria,
                                                   coordinateSystem);
            speciesObservationChange = new WebSpeciesObservationChange();
            speciesObservationChange.CreatedSpeciesObservations = GetSpeciesObservations(context, darwinCoreChange.CreatedSpeciesObservations);
            speciesObservationChange.DeletedSpeciesObservationGuids = darwinCoreChange.DeletedSpeciesObservationGuids;
            speciesObservationChange.IsMoreSpeciesObservationsAvailable = darwinCoreChange.IsMoreSpeciesObservationsAvailable;
            speciesObservationChange.MaxChangeCount = darwinCoreChange.MaxChangeCount;
            speciesObservationChange.MaxChangeId = darwinCoreChange.MaxChangeId;
            speciesObservationChange.UpdatedSpeciesObservations = GetSpeciesObservations(context, darwinCoreChange.UpdatedSpeciesObservations);
            return speciesObservationChange;
        }

        /// <summary>
        /// Get no of species observations that matches WebSpeciesObservationSearchCriteria and specified what
        /// coordinate system to use in WebCoordinateSystem.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">
        /// WebCoordinateSystem i.e coordinate system
        /// which WebSpeciesObservationSearchCriteria is using.
        /// </param>
        /// <returns>Number of species observations that matches search criteria.</returns>
        public static Int64 GetSpeciesObservationCountBySearchCriteria(WebServiceContext context,
                                                                       WebSpeciesObservationSearchCriteria searchCriteria,
                                                                       WebCoordinateSystem coordinateSystem)
        {
            Int64 speciesObservationCount;
            String geometryWhereCondition, joinCondition, whereCondition;
            List<Int64> speciesObservationIds;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckAutomaticDatabaseUpdate(context);
            searchCriteria.CheckNotNull("searchCriteria");
            searchCriteria.CheckData(context);
            coordinateSystem.CheckData();

            // Get species observation ids.
            speciesObservationIds = GetSpeciesObservationIdsAccessRights(context,
                                                                         searchCriteria,
                                                                         coordinateSystem);

            // If speciesObservationIds is empty return o otherwise return number of ids.
            speciesObservationCount = speciesObservationIds.Count;

            // Get all data required to performe/build up a db search.
            geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
            joinCondition = searchCriteria.GetJoinCondition();
            whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
            var taxonIds = WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false);
            speciesObservationCount += context.GetSpeciesObservationDatabase().GetSpeciesObservationCountBySearchCriteria(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                          searchCriteria.GetRegionIds(context),
                                                                                                                          taxonIds,
                                                                                                                          joinCondition,
                                                                                                                          whereCondition,
                                                                                                                          geometryWhereCondition);
            return speciesObservationCount;
        }

        /// <summary>
        /// Get number of species observations that matches
        /// provided species observation search criteria.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria.</param>
        /// <returns>
        /// Number of species observations that matches
        /// provided species observation search criteria.
        /// </returns>
        public static Int64 GetSpeciesObservationCountBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                    WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                    WebCoordinateSystem coordinateSystem)
        {
            return WebSpeciesObservationServiceData.SpeciesObservationManager.GetSpeciesObservationCountBySearchCriteriaElasticsearch(context,
                                                                                                                                      searchCriteria,
                                                                                                                                      coordinateSystem);
        }

        /// <summary>
        /// Get all the data providers that we harvest data from to our database.
        /// </summary>
        /// <param name="context">Information of the web client.</param>
        /// <returns>List of data providers.</returns>
        public static List<WebSpeciesObservationDataProvider> GetSpeciesObservationDataProviders(WebServiceContext context)
        {
            return WebServiceData.SpeciesObservationManager.GetSpeciesObservationDataProviders(context);
        }

        /// <summary>
        /// Convert one species observation field from JSON to
        /// class WebSpeciesObservationField.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationField">Field is returned in this parameter.</param>
        /// <param name="speciesObservationsJson">Species observation field in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetSpeciesObservationField(WebServiceContext context,
                                                        out WebSpeciesObservationField speciesObservationField,
                                                        String speciesObservationsJson,
                                                        Dictionary<String, WebSpeciesObservationField> mapping,
                                                        Int32 startIndex)
        {
            ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions = null;
            Int32 stopIndex;
            String fieldKey, projectParameterIdentifier, value;
            String[] splitKey;
            WebSpeciesObservationField mappingField;
            WebSpeciesObservationFieldDescriptionExtended fieldDescription;

            // Get field key.
            if (speciesObservationsJson[startIndex] != '}')
            {
                stopIndex = speciesObservationsJson.IndexOf(':', startIndex);
                fieldKey = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 2);
                startIndex = stopIndex + 1;
                if (mapping.ContainsKey(fieldKey))
                {
                    mappingField = mapping[fieldKey];
                    splitKey = fieldKey.Split('_');
                    if (splitKey.Length > 2)
                    {
                        // Get field in order to advance startIndex to next position.
                        startIndex = GetSpeciesObservationFieldValue(
                            out value,
                            mappingField.Type,
                            speciesObservationsJson,
                            startIndex);

                        // This field should not be returned.
                        // Return next field instead.
                        return GetSpeciesObservationField(context,
                                                          out speciesObservationField,
                                                          speciesObservationsJson,
                                                          mapping,
                                                          startIndex);
                    }
                    else
                    {
                        // Create field instance.
                        speciesObservationField = new WebSpeciesObservationField();
                        speciesObservationField.ClassIdentifier = mappingField.ClassIdentifier;
                        speciesObservationField.PropertyIdentifier = mappingField.PropertyIdentifier;
                        speciesObservationField.Type = mappingField.Type;
                        startIndex = GetSpeciesObservationFieldValue(out value,
                                                                     speciesObservationField.Type,
                                                                     speciesObservationsJson,
                                                                     startIndex);
                        speciesObservationField.Value = value;

                        if (speciesObservationField.ClassIdentifier.StartsWith("ProjectParameter") &&
                            speciesObservationField.PropertyIdentifier.StartsWith("ProjectParameter"))
                        {
                            // Handle species observation project parameter.
                            if (fieldDescriptions.IsNull())
                            {
                                fieldDescriptions = GetSpeciesObservationFieldDescriptions(context);
                            }

                            projectParameterIdentifier = speciesObservationField.ClassIdentifier + "_" +
                                                         speciesObservationField.PropertyIdentifier;
                            if (fieldDescriptions.ContainsKey(projectParameterIdentifier))
                            {
                                fieldDescription = fieldDescriptions[projectParameterIdentifier];
                                speciesObservationField.ClassIdentifier = SpeciesObservationClassId.Project.ToString();
                                speciesObservationField.PropertyIdentifier = fieldDescription.Name;
                                if (speciesObservationField.DataFields.IsNull())
                                {
                                    speciesObservationField.DataFields = new List<WebDataField>();
                                }

                                if (fieldDescription.Mappings.IsEmpty())
                                {
                                    WebServiceData.LogManager.Log(context,
                                                                  "Species observation field with PropertyIdentifier = " +
                                                                  projectParameterIdentifier + " does not have any mapping.",
                                                                  LogType.Error,
                                                                  null);
                                }
                                else
                                {
                                    speciesObservationField.DataFields.AddRange(fieldDescription.Mappings[0].DataFields);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (fieldKey == "\"sort")
                    {
                        // No more fields in current species observation.
                        // Read to next species observation.
                        stopIndex = speciesObservationsJson.IndexOf('}', startIndex);
                        startIndex = stopIndex + 1;
                        speciesObservationField = null;
                    }
                    else
                    {
                        throw new Exception("Unknown field name = " + fieldKey);
                    }
                }
            }
            else
            {
                // No more fields in current species observation.
                // Read to next species observation.
                startIndex += 2;
                speciesObservationField = null;
            }

            return startIndex;
        }

        /// <summary>
        /// Get species observation field descriptions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>
        /// Species observation field descriptions.
        /// Key in dicionary is property identifier.
        /// </returns>
        private static ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended> GetSpeciesObservationFieldDescriptions(WebServiceContext context)
        {
            ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            String cacheKey;

            // Get cached information.
            cacheKey = Settings.Default.SpeciesObservationFieldDescriptionsCacheKey;
            fieldDescriptions = (ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended>)(context.GetCachedObject(cacheKey));

            if (fieldDescriptions.IsEmpty())
            {
                // Data not in cache.
                fieldDescriptions = new ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended>();
                foreach (WebSpeciesObservationFieldDescriptionExtended fieldDescription in WebServiceData.MetadataManager.GetSpeciesObservationFieldDescriptionsExtended(context))
                {

                    if (fieldDescription.Mappings.IsNotEmpty() &&
                        fieldDescription.Mappings[0].GetPropertyIdentifier().IsNotEmpty() &&
                        fieldDescription.Mappings[0].GetPropertyIdentifier().Contains("ProjectParameter"))
                    {
                        fieldDescriptions[fieldDescription.Mappings[0].GetPropertyIdentifier()] = fieldDescription;
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        fieldDescriptions,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return fieldDescriptions;
        }

        /// <summary>
        /// Get fields that must be included for species 
        /// observation handling to work properly.
        /// </summary>
        /// <returns>
        /// Fields that must be included for species 
        /// observation handling to work properly.
        /// </returns>
        private static List<WebSpeciesObservationFieldSpecification> GetSpeciesObservationFields()
        {
            List<WebSpeciesObservationFieldSpecification> fieldSpecifications;

            fieldSpecifications = new List<WebSpeciesObservationFieldSpecification>();
            fieldSpecifications.Merge(SpeciesObservationClassId.Conservation,
                                      SpeciesObservationPropertyId.ProtectionLevel);
            fieldSpecifications.Merge(SpeciesObservationClassId.DarwinCore,
                                      SpeciesObservationPropertyId.Id);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.CoordinateX);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.CoordinateY);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.County);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.DecimalLatitude);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.DecimalLongitude);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.Locality);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.Municipality);
            fieldSpecifications.Merge(SpeciesObservationClassId.Location,
                                      SpeciesObservationPropertyId.StateProvince);
            fieldSpecifications.Merge(SpeciesObservationClassId.Occurrence,
                                      SpeciesObservationPropertyId.OccurrenceID);
            fieldSpecifications.Merge(SpeciesObservationClassId.Taxon,
                                      SpeciesObservationPropertyId.DyntaxaTaxonID);
            fieldSpecifications.Merge(SpeciesObservationClassId.Taxon,
                                      SpeciesObservationPropertyId.ScientificName);
            fieldSpecifications.Merge(SpeciesObservationClassId.Taxon,
                                      SpeciesObservationPropertyId.VernacularName);
            return fieldSpecifications;
        }

        /// <summary>
        /// Convert one species observation field value from JSON to
        /// class WebSpeciesObservationField.
        /// </summary>
        /// <param name="value">Field value is returned in this parameter.</param>
        /// <param name="type">Field value is of this type.</param>
        /// <param name="speciesObservationsJson">Species observation field in JSON format.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetSpeciesObservationFieldValue(out String value,
                                                             WebDataType type,
                                                             String speciesObservationsJson,
                                                             Int32 startIndex)
        {
            Boolean stringEndFound;
            Int32 stopIndex;

            switch (type)
            {
                case WebDataType.Boolean:
                    switch (speciesObservationsJson.Substring(startIndex, 4))
                    {
                        case "fals":
                            value = Boolean.FalseString;
                            startIndex += 6;
                            break;
                        case "true":
                            value = Boolean.TrueString;
                            startIndex += 5;
                            break;
                        default:
                            throw new Exception("Not handled boolean data type value = " + speciesObservationsJson.Substring(startIndex, 4));
                    }

                    break;

                case WebDataType.DateTime:
                    stopIndex = speciesObservationsJson.IndexOf('\"', startIndex + 1);
                    value = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 1);
                    startIndex = stopIndex + 2;
                    break;

                case WebDataType.Float64:
                    stopIndex = speciesObservationsJson.IndexOfAny(new[] { ',', '}' }, startIndex);
                    value = speciesObservationsJson.Substring(startIndex, stopIndex - startIndex);

                    // Convert float value from Elasticsearch
                    // format to SOAP web service format.
                    value = value.WebParseDouble().WebToString();
                    startIndex = stopIndex + 1;
                    break;

                case WebDataType.Int32:
                case WebDataType.Int64:
                    stopIndex = speciesObservationsJson.IndexOfAny(new[] { ',', '}' }, startIndex);
                    value = speciesObservationsJson.Substring(startIndex, stopIndex - startIndex);
                    startIndex = stopIndex + 1;
                    break;

                case WebDataType.String:
                    stopIndex = startIndex + 1;
                    stringEndFound = false;
                    while (!stringEndFound)
                    {
                        stringEndFound = (speciesObservationsJson[stopIndex] == '"') &&
                                         (speciesObservationsJson[stopIndex - 1] != '\\');
                        if (!stringEndFound)
                        {
                            stopIndex++;
                        }
                    }

                    value = speciesObservationsJson.Substring(startIndex + 1, stopIndex - startIndex - 1);
                    startIndex = stopIndex + 2;

                    // Remove escape of special characters.
                    value = value.Replace("\\\"", "\"");
                    value = value.Replace("\\\\", "\\");
                    break;

                default:
                    throw new Exception("Not handled data type = " + type);
            }

            return startIndex;
        }

        /// <summary>
        /// Get one species observation id from JSON.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Species observation ids are stored in this list.</param>
        /// <param name="speciesObservationsJson">Species observation in JSON format.</param>
        /// <param name="mapping">Species observation field information mapping.</param>
        /// <param name="startIndex">Current position in the species observation JSON string.</param>
        /// <returns>Updated current position in the species observation JSON string.</returns>
        private static Int32 GetSpeciesObservationId(WebServiceContext context,
                                                     List<Int64> speciesObservationIds,
                                                     String speciesObservationsJson,
                                                     Dictionary<String, WebSpeciesObservationField> mapping,
                                                     Int32 startIndex)
        {
            WebSpeciesObservationField field;

            // Skip general part.
            startIndex = speciesObservationsJson.IndexOf("_source", startIndex, StringComparison.Ordinal);
            startIndex = speciesObservationsJson.IndexOf("{", startIndex, StringComparison.Ordinal) + 1;

            do
            {
                startIndex = GetSpeciesObservationField(context,
                                                        out field,
                                                        speciesObservationsJson,
                                                        mapping,
                                                        startIndex);
                if (field.IsNotNull())
                {
                    speciesObservationIds.Add(field.Value.WebParseInt64());
                }
            }
            while (field.IsNotNull());

            return startIndex;
        }

        /// <summary>
        /// Get species observation ids for
        /// user with complex access rights.
        /// Returned species observations ids are limited to
        /// protected species observations.
        /// </summary>
        /// <param name="context">The web service context, information on user, requestId, connection etc.</param>
        /// <param name="searchCriteria">The species observation search criteria- defined in class WebSpeciesObservationSearchCriteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in search criteria.</param>
        /// <returns>Species observations ids for protected species observations.</returns>
        private static List<Int64> GetSpeciesObservationIdsAccessRights(WebServiceContext context,
                                                                        WebSpeciesObservationSearchCriteria searchCriteria,
                                                                        WebCoordinateSystem coordinateSystem)
        {
            Boolean isMinProtectionLevelSpecified;
            Int32 minProtectionLevel;
            List<Int64> speciesObservationIds;
            List<WebRole> currentRoles;
            SpeciesObservationAccessRights speciesObservation;
            String geometryWhereCondition, joinCondition, whereCondition;

            speciesObservationIds = new List<Int64>();
            currentRoles = context.CurrentRoles;
            if (!currentRoles.IsSimpleSpeciesObservationAccessRights())
            {
                // Save user specified min protection level.
                isMinProtectionLevelSpecified = searchCriteria.IsMinProtectionLevelSpecified;
                minProtectionLevel = searchCriteria.MinProtectionLevel;

                // Set temporary min protection level. 
                searchCriteria.IsMinProtectionLevelSpecified = true;
                if (isMinProtectionLevelSpecified)
                {
                    searchCriteria.MinProtectionLevel = Math.Max(2,
                                                                 searchCriteria.MinProtectionLevel);
                }
                else
                {
                    searchCriteria.MinProtectionLevel = 2;
                }

                // Get species observation ids.
                geometryWhereCondition = searchCriteria.GetGeometryWhereCondition();
                joinCondition = searchCriteria.GetJoinCondition();
                whereCondition = searchCriteria.GetWhereConditionNew(context, coordinateSystem);
                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationsAccessRights(searchCriteria.GetPolygonsAsGeometry(coordinateSystem),
                                                                                                                          searchCriteria.GetRegionIds(context),
                                                                                                                          WebSpeciesObservationServiceData.TaxonManager.GetTaxonIds(context, searchCriteria, false),
                                                                                                                          joinCondition,
                                                                                                                          whereCondition,
                                                                                                                          geometryWhereCondition))
                {
                    while (dataReader.Read())
                    {
                        speciesObservation = new SpeciesObservationAccessRights();
                        speciesObservation.Load(dataReader);
                        if (speciesObservation.CheckAccessRights(context))
                        {
                            speciesObservationIds.Add(speciesObservation.Id);
                        }
                    }
                }

                // Adjust protection levels for search on public
                // species observations.
                searchCriteria.MaxProtectionLevel = 1;
                searchCriteria.IsMinProtectionLevelSpecified = isMinProtectionLevelSpecified;
                searchCriteria.MinProtectionLevel = minProtectionLevel;
            }

            return speciesObservationIds;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the filter.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">
        /// The species observation search criteria.
        /// This parameter should be null if ids should not be
        /// retrieved when filter matches to many observations
        /// with information.
        /// </param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="filter">
        /// Filter that is used when species observations are retrieved.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <param name="methodName">
        /// Name of the method that the web service user uses.
        /// This name is used when logging information about
        /// usage of protected information.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationInformationElasticsearch(WebServiceContext context,
                                                                                                     WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                     WebCoordinateSystem coordinateSystem,
                                                                                                     String filter,
                                                                                                     WebSpeciesObservationSpecification speciesObservationSpecification,
                                                                                                     String methodName)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            Int32 startIndex;
            DocumentFilterResponse speciesObservationResponse;
            StringBuilder idFilter;
            WebSpeciesObservationInformation speciesObservationInformation;

            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                speciesObservationResponse = elastisearch.GetSpeciesObservations(filter);
            }

            speciesObservationInformation = new WebSpeciesObservationInformation();
            if (speciesObservationResponse.TimedOut)
            {
                throw new Exception("The question timed out! Filter = " + filter);
            }

            if (searchCriteria.IsNotNull() &&
                (speciesObservationResponse.DocumentCount >
                 Settings.Default.MaxSpeciesObservation))
            {
                // Too many species observations with ids.
                throw new ApplicationException("Too many species observations was retrieved!, Limit is set to " +
                                               Settings.Default.MaxSpeciesObservation + " observations.");
            }

            if (speciesObservationResponse.DocumentCount > 0)
            {
                using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                {
                    mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
                }

                startIndex = 0;

                if (searchCriteria.IsNotNull() &&
                    (speciesObservationResponse.DocumentCount >
                     Settings.Default.MaxSpeciesObservationWithInformation))
                {
                    // Get ids only.
                    idFilter = new StringBuilder();
                    idFilter.Append("{");
                    idFilter.Append(" \"size\": " + speciesObservationResponse.DocumentCount);
                    idFilter.Append(", \"_source\":{\"include\":[");
                    idFilter.Append("\"");
                    using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                    {
                        idFilter.Append(elastisearch.GetFieldName(SpeciesObservationClassId.DarwinCore,
                                                                  SpeciesObservationPropertyId.Id));
                    }

                    idFilter.Append("\"]}");
                    idFilter.Append(", " + searchCriteria.GetFilter(context, false));
                    idFilter.Append("}");
                    using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
                    {
                        speciesObservationResponse = elastisearch.GetSpeciesObservations(idFilter.ToString());
                    }

                    speciesObservationInformation.SpeciesObservationIds = new List<Int64>();
                    while (startIndex < (speciesObservationResponse.DocumentsJson.Length - 10))
                    {
                        startIndex = GetSpeciesObservationId(context,
                                                             speciesObservationInformation.SpeciesObservationIds,
                                                             speciesObservationResponse.DocumentsJson,
                                                             mapping,
                                                             startIndex);
                    }
                }
                else
                {
                    speciesObservationInformation.SpeciesObservations = new List<WebSpeciesObservation>();
                    while (startIndex < (speciesObservationResponse.DocumentsJson.Length - 10))
                    {
                        startIndex = GetSpeciesObservation(context,
                                                           speciesObservationInformation.SpeciesObservations,
                                                           speciesObservationResponse.DocumentsJson,
                                                           mapping,
                                                           startIndex);
                    }
                }
            }

            speciesObservationInformation.SetCount();
            ConvertToRequestedCoordinates(context, speciesObservationInformation.SpeciesObservations, coordinateSystem);
            CheckSpeciesObservationLog(context, speciesObservationInformation.SpeciesObservations, methodName);
            CheckSpeciesObservationFields(speciesObservationInformation.SpeciesObservations,
                                          speciesObservationSpecification);
            return speciesObservationInformation;
        }

        /// <summary>
        /// Get definition of species observation log table.
        /// </summary>
        /// <returns>Definition of species observation log table.</returns>
        private static DataTable GetSpeciesObservationLogTable()
        {
            DataColumn column;
            DataTable table;

            table = new DataTable(SpeciesObservationLogData.TABLE_NAME);
            column = new DataColumn(SpeciesObservationLogData.COMMON_NAME, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.PROTECTION_LEVEL, typeof(Int32));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.PERSON, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.TIME, typeof(DateTime));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.TAXON_ID, typeof(Int32));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.SCIENTIFIC_NAME, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.APPLICATION, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.WEB_SERVICE_USER, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.APPLICATION_IDENTIFIER, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.TCP_IP, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.SQL_SERVER_USER, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.X, typeof(Double));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.Y, typeof(Double));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.PROVINCE, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.COUNTY, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.MUNICIPALITY, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.LOCALITY, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.SPECIES_OBSERVATION_GUID, typeof(String));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.USER_ID, typeof(Int32));
            table.Columns.Add(column);
            column = new DataColumn(SpeciesObservationLogData.METHOD, typeof(String));
            table.Columns.Add(column);
            return table;
        }

        /// <summary>
        /// Get species observation project parameters for specified species observations.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for species observations.</param>
        /// <returns>Species observation project parameters for specified species observations.</returns>
        private static Dictionary<Int64, List<WebSpeciesObservationField>> GetSpeciesObservationProjectParameters(WebServiceContext context,
                                                                                                                  List<Int64> speciesObservationIds)
        {
            ConcurrentDictionary<String, WebSpeciesObservationFieldDescriptionExtended> fieldDescriptions;
            Dictionary<Int64, List<WebSpeciesObservationField>> speciesObservationProjectParameters;
            List<WebSpeciesObservationField> speciesObservationFields;
            WebSpeciesObservationFieldDescriptionExtended fieldDescription;
            WebSpeciesObservationFieldProjectParameter speciesObservationField;

            speciesObservationProjectParameters = new Dictionary<Int64, List<WebSpeciesObservationField>>();
            if (speciesObservationIds.IsNotEmpty())
            {
                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetSpeciesObservationProjectParametersByIds(speciesObservationIds))
                {
                    while (dataReader.Read())
                    {
                        speciesObservationField = new WebSpeciesObservationFieldProjectParameter();
                        speciesObservationField.LoadData(dataReader);
                        if (speciesObservationProjectParameters.ContainsKey(speciesObservationField.SpeciesObservationId))
                        {
                            speciesObservationFields = speciesObservationProjectParameters[speciesObservationField.SpeciesObservationId];
                        }
                        else
                        {
                            speciesObservationFields = new List<WebSpeciesObservationField>();
                            speciesObservationProjectParameters[speciesObservationField.SpeciesObservationId] = speciesObservationFields;
                        }

                        speciesObservationFields.Add(speciesObservationField.GetSpeciesObservationField());
                    }
                }

                if (speciesObservationProjectParameters.Values.IsNotEmpty())
                {
                    fieldDescriptions = GetSpeciesObservationFieldDescriptions(context);
                    foreach (List<WebSpeciesObservationField> fields in speciesObservationProjectParameters.Values)
                    {
                        if (fields.IsNotEmpty())
                        {
                            foreach (WebSpeciesObservationField field in fields)
                            {
                                if (fieldDescriptions.ContainsKey(field.PropertyIdentifier))
                                {
                                    fieldDescription = fieldDescriptions[field.PropertyIdentifier];
                                    field.PropertyIdentifier = fieldDescription.Name;
                                    if (field.DataFields.IsNull())
                                    {
                                        field.DataFields = new List<WebDataField>();
                                    }

                                    if (fieldDescription.Mappings.IsEmpty())
                                    {
                                        WebServiceData.LogManager.Log(context, "Species observation field with PropertyIdentifier = " + field.PropertyIdentifier + " does not have any mapping.", LogType.Error, null);
                                    }
                                    else
                                    {
                                        field.DataFields.AddRange(fieldDescription.Mappings[0].DataFields);
                                    }
                                }
                                else
                                {
                                    WebServiceData.LogManager.Log(context, "Species observation field with PropertyIdentifier = " + field.PropertyIdentifier + " does not have any meta data.", LogType.Error, null);
                                }
                            }
                        }
                    }
                }
            }

            return speciesObservationProjectParameters;
        }

        /// <summary>
        /// Convert WebDarwinCore instances to
        /// WebSpeciesObservation instances.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="darwinCoreObservations">Species observations in DarwinCore format.</param>
        /// <returns>WebSpeciesObservation instances.</returns>
        private static List<WebSpeciesObservation> GetSpeciesObservations(WebServiceContext context,
                                                                          List<WebDarwinCore> darwinCoreObservations)
        {
            Dictionary<Int64, List<WebSpeciesObservationField>> speciesObservationFields;
            List<WebSpeciesObservation> speciesObservations;
            WebSpeciesObservation speciesObservation;

            speciesObservationFields = GetSpeciesObservationProjectParameters(context, darwinCoreObservations.GetIds());
            speciesObservations = null;
            if (darwinCoreObservations.IsNotEmpty())
            {
                speciesObservations = new List<WebSpeciesObservation>();
                foreach (WebDarwinCore darwinCoreObservation in darwinCoreObservations)
                {
                    speciesObservation = GetSpeciesObservation(darwinCoreObservation);
                    if (speciesObservationFields.ContainsKey(darwinCoreObservation.Id))
                    {
                        foreach (WebSpeciesObservationField speciesObservationField in speciesObservationFields[darwinCoreObservation.Id])
                        {
                            speciesObservation.Fields.Add(speciesObservationField);
                        }
                    }

                    speciesObservations.Add(speciesObservation);
                }
            }

            return speciesObservations;
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationsByIds(WebServiceContext context,
                                                                                   List<Int64> speciesObservationIds,
                                                                                   WebCoordinateSystem coordinateSystem,
                                                                                   WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            WebSpeciesObservationInformation speciesObservationInformation;
            WebDarwinCoreInformation darwinCoreInformation;

            darwinCoreInformation = GetDarwinCoreByIds(context,
                                                       speciesObservationIds,
                                                       coordinateSystem);
            speciesObservationInformation = new WebSpeciesObservationInformation();
            speciesObservationInformation.MaxSpeciesObservationCount = darwinCoreInformation.MaxSpeciesObservationCount;
            speciesObservationInformation.SpeciesObservationCount = darwinCoreInformation.SpeciesObservationCount;
            speciesObservationInformation.SpeciesObservationIds = darwinCoreInformation.SpeciesObservationIds;
            speciesObservationInformation.SpeciesObservations = GetSpeciesObservations(context, darwinCoreInformation.SpeciesObservations);
            return speciesObservationInformation;
        }

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationIds">Ids for species observations to get.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned species observations.</param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationsByIdsElasticsearch(WebServiceContext context,
                                                                                                List<Int64> speciesObservationIds,
                                                                                                WebCoordinateSystem coordinateSystem,
                                                                                                WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            Int32 index;
            StringBuilder filter;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            coordinateSystem.CheckData();
            speciesObservationSpecification.CheckData();
            speciesObservationIds.CheckNotEmpty("speciesObservationIds");
            if (speciesObservationIds.Count > Settings.Default.MaxSpeciesObservationWithInformation)
            {
                throw new ArgumentException("Too many species observations was retrieved!, Limit is set to " +
                                            Settings.Default.MaxSpeciesObservationWithInformation + " observations with information.");
            }

            // Get species observation filter.
            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"size\": " + speciesObservationIds.Count);
            filter.Append(", " + GetFields(context, speciesObservationSpecification));
            filter.Append(", \"filter\": {\"bool\":{ \"must\" : [");
            filter.Append("{ \"terms\": {");
            filter.Append(" \"");
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                filter.Append(elastisearch.GetFieldName(SpeciesObservationClassId.DarwinCore,
                                                        SpeciesObservationPropertyId.Id));
            }

            filter.Append("\":[");
            filter.Append(speciesObservationIds[0].WebToString());
            for (index = 1; index < speciesObservationIds.Count; index++)
            {
                filter.Append(", " + speciesObservationIds[index].WebToString());
            }

            filter.Append("]}}");
            filter.Append(", " + context.CurrentRoles.GetSpeciesObservationAccessRightsJson(context));
            filter.Append("]}}}");

            return GetSpeciesObservationInformationElasticsearch(context,
                                                                 null,
                                                                 coordinateSystem,
                                                                 filter.ToString(),
                                                                 speciesObservationSpecification,
                                                                 "GetSpeciesObservationsByIdsElasticsearch");
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <param name="coordinateSystem">Coordinate system used in geometry search criteria
        /// and returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(WebServiceContext context,
                                                                                              WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                              WebCoordinateSystem coordinateSystem,
                                                                                              WebSpeciesObservationSpecification speciesObservationSpecification,
                                                                                              List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            WebSpeciesObservationInformation speciesObservationInformation;
            WebDarwinCoreInformation darwinCoreInformation;

            darwinCoreInformation = GetDarwinCoreBySearchCriteria(context,
                                                                  searchCriteria,
                                                                  coordinateSystem,
                                                                  sortOrder);
            speciesObservationInformation = new WebSpeciesObservationInformation();
            speciesObservationInformation.MaxSpeciesObservationCount = darwinCoreInformation.MaxSpeciesObservationCount;
            speciesObservationInformation.SpeciesObservationCount = darwinCoreInformation.SpeciesObservationCount;
            speciesObservationInformation.SpeciesObservationIds = darwinCoreInformation.SpeciesObservationIds;
            speciesObservationInformation.SpeciesObservations = GetSpeciesObservations(context, darwinCoreInformation.SpeciesObservations);
            return speciesObservationInformation;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static List<WebSpeciesObservation> GetSpeciesObservationsBySearchCriteriaPage(WebServiceContext context,
                                                                                             WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                             WebCoordinateSystem coordinateSystem,
                                                                                             WebSpeciesObservationPageSpecification pageSpecification,
                                                                                             WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            List<WebDarwinCore> darwinCoreObservations;
            List<WebSpeciesObservation> speciesObservations;

            darwinCoreObservations = GetDarwinCoreBySearchCriteriaPage(context,
                                                                       searchCriteria,
                                                                       coordinateSystem,
                                                                       pageSpecification);
            speciesObservations = GetSpeciesObservations(context, darwinCoreObservations);
            return speciesObservations;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// This method provides paging functionality of the result.
        /// Max page size is 10000 species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem">
        /// Coordinate system used in geometry search criteria
        /// and returned species observations.
        /// </param>
        /// <param name="pageSpecification">
        /// SpecificationId of paging information when
        /// species observations are retrieved.
        /// </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static List<WebSpeciesObservation> GetSpeciesObservationsBySearchCriteriaPageElasticsearch(WebServiceContext context,
                                                                                                          WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                          WebCoordinateSystem coordinateSystem,
                                                                                                          WebSpeciesObservationPageSpecification pageSpecification,
                                                                                                          WebSpeciesObservationSpecification speciesObservationSpecification)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            StringBuilder filter;
            WebSpeciesObservationInformation speciesObservationInformation;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            pageSpecification.CheckData();
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);
            speciesObservationSpecification.CheckData();

            // Get filter.
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
            }

            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"from\": " + (pageSpecification.Start - 1).WebToString());
            filter.Append(", \"size\": " + pageSpecification.Size);
            filter.Append(", " + GetFields(context, speciesObservationSpecification));
            filter.Append(", " + searchCriteria.GetFilter(context, false));
            filter.Append(", " + pageSpecification.SortOrder.GetSortOrderJson(mapping));
            filter.Append("}");

            // Get species observations.
            speciesObservationInformation = GetSpeciesObservationInformationElasticsearch(context,
                                                                                          null,
                                                                                          coordinateSystem,
                                                                                          filter.ToString(),
                                                                                          speciesObservationSpecification,
                                                                                          "GetSpeciesObservationsBySearchCriteriaPageElasticsearch");
            return speciesObservationInformation.SpeciesObservations;
        }

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// Max 25000 observations with information
        /// can be retrieved in one call.
        /// Max 1000000 observation ids can be retrieved in one call.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="coordinateSystem"> Coordinate system used in geometry search criteria
        /// and returned species observations. </param>
        /// <param name="speciesObservationSpecification">
        /// Specify which subset of the data that should be
        /// returned for each species observation.
        /// All information for each species observation is
        /// returned if this parameter is null. 
        /// This parameter is currently not used.
        /// </param>
        /// <param name="sortOrder">
        /// Defines how species observations should be sorted.
        /// This parameter is optional. Random order is used
        /// if no sort order has been specified.
        /// </param>
        /// <returns>Information about requested species observations.</returns>
        public static WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteriaElasticsearch(WebServiceContext context,
                                                                                                           WebSpeciesObservationSearchCriteria searchCriteria,
                                                                                                           WebCoordinateSystem coordinateSystem,
                                                                                                           WebSpeciesObservationSpecification speciesObservationSpecification,
                                                                                                           List<WebSpeciesObservationFieldSortOrder> sortOrder)
        {
            Dictionary<String, WebSpeciesObservationField> mapping;
            StringBuilder filter;

            // Check users access rights.
            WebServiceData.AuthorizationManager.CheckAuthorization(context, AuthorityIdentifier.Sighting);

            // Check that data is valid.
            WebSpeciesObservationServiceData.SpeciesObservationManager.CheckData(context, searchCriteria, coordinateSystem);
            sortOrder.CheckData();
            speciesObservationSpecification.CheckData();

            // Get filter.
            using (ElasticsearchSpeciesObservationProxy elastisearch = WebServiceData.DatabaseManager.GetElastisearchSpeciesObservationProxy())
            {
                mapping = WebSpeciesObservationServiceData.SpeciesObservationManager.GetMapping(context, elastisearch);
            }

            filter = new StringBuilder();
            filter.Append("{");
            filter.Append(" \"size\": " + Settings.Default.MaxSpeciesObservationWithInformation.WebToString());
            filter.Append(", " + GetFields(context, speciesObservationSpecification));
            filter.Append(", " + searchCriteria.GetFilter(context, false));
            filter.Append(", " + sortOrder.GetSortOrderJson(mapping));
            filter.Append("}");

            return GetSpeciesObservationInformationElasticsearch(context,
                                                                 searchCriteria,
                                                                 coordinateSystem,
                                                                 filter.ToString(),
                                                                 speciesObservationSpecification,
                                                                 "GetSpeciesObservationsBySearchCriteriaElasticsearch");
        }

        ///// <summary>
        ///// Get ids for requested taxa.
        ///// </summary>
        ///// <param name="context">Web service request context.</param>
        ///// <param name="searchCriteria">The species observation search criteria.</param>
        ///// <param name="includeChildTaxa">Include child taxa in returned taxa.</param>
        ///// <returns>Ids for requested taxa.</returns>
        //private static List<Int32> GetTaxonIds(WebServiceContext context,
        //                                       WebSpeciesObservationSearchCriteria searchCriteria,
        //                                       Boolean includeChildTaxa)
        //{
        //    List<Int32> redlistedTaxonIds, taxonIds;

        //    taxonIds = searchCriteria.TaxonIds;
        //    if (searchCriteria.IncludeRedlistedTaxa ||
        //        searchCriteria.IncludeRedListCategories.IsNotEmpty())
        //    {
        //        // Get redlisted taxon ids.
        //        redlistedTaxonIds = GetRedlistedTaxonIds(context,
        //                                                 searchCriteria.IncludeRedlistedTaxa,
        //                                                 searchCriteria.IncludeRedListCategories);
        //        if (redlistedTaxonIds.IsNotEmpty())
        //        {
        //            if (taxonIds.IsEmpty())
        //            {
        //                taxonIds = redlistedTaxonIds;
        //            }
        //            else
        //            {
        //                foreach (Int32 taxonId in redlistedTaxonIds)
        //                {
        //                    if (!taxonIds.Contains(taxonId))
        //                    {
        //                        taxonIds.Add(taxonId);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (includeChildTaxa)
        //    {
        //        taxonIds = WebServiceData.TaxonManager.GetChildTaxonIds(context, taxonIds);
        //    }

        //    return taxonIds;
        //}

        /// <summary>
        /// Get taxa that is defined for usage 
        /// by the swedish forest agency.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Ids for taxa that defined for usage by the swedish forest agency.</returns>
        public static DataIdInt32List GetSwedishForestAgencyTaxonIds(WebServiceContext context)
        {
            DataIdInt32List swedishForestAgencyTaxonIds;
            List<WebSpeciesFact> speciesFacts;
            WebSpeciesFactSearchCriteria searchCriteria;

            // Get species facts.
            searchCriteria = new WebSpeciesFactSearchCriteria();
            searchCriteria.FactorIds = new List<Int32>();
            //searchCriteria.FactorIds.Add(2647); // Birds.
            // All species used by swedish forest agency.
            searchCriteria.FactorIds.Add(2648);
            searchCriteria.IndividualCategoryIds = new List<Int32>();
            searchCriteria.IndividualCategoryIds.Add((Int32)(IndividualCategoryId.Default));
            speciesFacts = WebServiceData.SpeciesFactManager.GetSpeciesFactsBySearchCriteria(context, searchCriteria);

            // Get taxon ids.
            swedishForestAgencyTaxonIds = new DataIdInt32List(true);
            foreach (WebSpeciesFact speciesFact in speciesFacts)
            {
                swedishForestAgencyTaxonIds.Merge(speciesFact.TaxonId);
            }

            return swedishForestAgencyTaxonIds;
        }

        /// <summary>
        /// Test if specified species observation class and species
        /// observation property defines a species observation
        /// field that is included in the DarwinCore format.
        /// </summary>
        /// <param name="classIdentifier">Species observation class identifier.</param>
        /// <param name="propertyIdentifier">Species observation property identifier.</param>
        /// <returns>
        /// True, if specified species observation class and species
        /// observation property defines a species observation
        /// field that is included in the DarwinCore format.
        /// </returns>
        private static Boolean IsDarwinCoreField(String classIdentifier,
                                                 String propertyIdentifier)
        {
            Boolean isDarwinCoreField;

            isDarwinCoreField = false;
            switch (classIdentifier)
            {
                case "Conservation":
                    switch (propertyIdentifier)
                    {
                        case "ActionPlan":
                        case "ConservationRelevant":
                        case "Natura2000":
                        case "ProtectedByLaw":
                        case "ProtectionLevel":
                        case "RedlistCategory":
                        case "SwedishImmigrationHistory":
                        case "SwedishOccurrence":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "DarwinCore":
                    switch (propertyIdentifier)
                    {
                        case "AccessRights":
                        case "BasisOfRecord":
                        case "BibliographicCitation":
                        case "CollectionCode":
                        case "CollectionID":
                        case "DataGeneralizations":
                        case "DatasetID":
                        case "DatasetName":
                        case "DynamicProperties":
                        case "Id":
                        case "InformationWithheld":
                        case "InstitutionCode":
                        case "InstitutionID":
                        case "Language":
                        case "Modified":
                        case "Owner":
                        case "OwnerInstitutionCode":
                        case "References":
                        case "ReportedBy":
                        case "ReportedDate":
                        case "Rights":
                        case "RightsHolder":
                        case "SpeciesObservationURL":
                        case "Type":
                        case "ValidationStatus":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Event":
                    switch (propertyIdentifier)
                    {
                        case "Day":
                        case "End":
                        case "EndDayOfYear":
                        case "EventDate":
                        case "EventID":
                        case "EventRemarks":
                        case "EventTime":
                        case "FieldNotes":
                        case "FieldNumber":
                        case "Habitat":
                        case "Month":
                        case "SamplingEffort":
                        case "SamplingProtocol":
                        case "Start":
                        case "StartDayOfYear":
                        case "VerbatimEventDate":
                        case "Year":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "GeologicalContext":
                    switch (propertyIdentifier)
                    {
                        case "Bed":
                        case "EarliestAgeOrLowestStage":
                        case "EarliestEonOrLowestEonothem":
                        case "EarliestEpochOrLowestSeries":
                        case "EarliestEraOrLowestErathem":
                        case "EarliestPeriodOrLowestSystem":
                        case "Formation":
                        case "GeologicalContextID":
                        case "Group":
                        case "HighestBiostratigraphicZone":
                        case "LatestAgeOrHighestStage":
                        case "LatestEonOrHighestEonothem":
                        case "LatestEpochOrHighestSeries":
                        case "LatestEraOrHighestErathem":
                        case "LatestPeriodOrHighestSystem":
                        case "LithostratigraphicTerms":
                        case "LowestBiostratigraphicZone":
                        case "Member":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Identification":
                    switch (propertyIdentifier)
                    {
                        case "DateIdentified":
                        case "IdentificationID":
                        case "IdentificationQualifier":
                        case "IdentificationReferences":
                        case "IdentificationRemarks":
                        case "IdentificationVerificationStatus":
                        case "IdentifiedBy":
                        case "TypeStatus":
                        case "UncertainDetermination":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Location":
                    switch (propertyIdentifier)
                    {
                        case "Continent":
                        case "CoordinateM":
                        case "CoordinatePrecision":
                        case "CoordinateSystemWkt":
                        case "CoordinateUncertaintyInMeters":
                        case "CoordinateX":
                        case "CoordinateY":
                        case "CoordinateZ":
                        case "Country":
                        case "CountryCode":
                        case "County":
                        case "DecimalLatitude":
                        case "DecimalLongitude":
                        case "FootprintSpatialFit":
                        case "FootprintSRS":
                        case "FootprintWKT":
                        case "GeodeticDatum":
                        case "GeoreferencedBy":
                        case "GeoreferencedDate":
                        case "GeoreferenceProtocol":
                        case "GeoreferenceRemarks":
                        case "GeoreferenceSources":
                        case "GeoreferenceVerificationStatus":
                        case "HigherGeography":
                        case "HigherGeographyID":
                        case "Island":
                        case "IslandGroup":
                        case "Locality":
                        case "LocationAccordingTo":
                        case "LocationId":
                        case "LocationRemarks":
                        case "LocationURL":
                        case "MaximumDepthInMeters":
                        case "MaximumDistanceAboveSurfaceInMeters":
                        case "MaximumElevationInMeters":
                        case "MinimumDepthInMeters":
                        case "MinimumDistanceAboveSurfaceInMeters":
                        case "MinimumElevationInMeters":
                        case "Municipality":
                        case "Parish":
                        case "PointRadiusSpatialFit":
                        case "StateProvince":
                        case "VerbatimCoordinates":
                        case "VerbatimCoordinateSystem":
                        case "VerbatimDepth":
                        case "VerbatimElevation":
                        case "VerbatimLatitude":
                        case "VerbatimLocality":
                        case "VerbatimLongitude":
                        case "VerbatimSRS":
                        case "WaterBody":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "MeasurementOrFact":
                    switch (propertyIdentifier)
                    {
                        case "MeasurementAccuracy":
                        case "MeasurementDeterminedBy":
                        case "MeasurementDeterminedDate":
                        case "MeasurementID":
                        case "MeasurementMethod":
                        case "MeasurementRemarks":
                        case "MeasurementType":
                        case "MeasurementUnit":
                        case "MeasurementValue":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Occurrence":
                    switch (propertyIdentifier)
                    {
                        case "AssociatedMedia":
                        case "AssociatedOccurrences":
                        case "AssociatedReferences":
                        case "AssociatedSequences":
                        case "AssociatedTaxa":
                        case "Behavior":
                        case "CatalogNumber":
                        case "Disposition":
                        case "EstablishmentMeans":
                        case "IndividualCount":
                        case "IndividualID":
                        case "IsNaturalOccurrence":
                        case "IsNeverFoundObservation":
                        case "IsNotRediscoveredObservation":
                        case "IsPositiveObservation":
                        case "LifeStage":
                        case "OccurrenceID":
                        case "OccurrenceRemarks":
                        case "OccurrenceStatus":
                        case "OccurrenceURL":
                        case "OtherCatalogNumbers":
                        case "Preparations":
                        case "PreviousIdentifications":
                        case "Quantity":
                        case "QuantityUnit":
                        case "RecordedBy":
                        case "RecordNumber":
                        case "ReproductiveCondition":
                        case "Sex":
                        case "Substrate":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Project":
                    switch (propertyIdentifier)
                    {
                        case "IsPublic":
                        case "ProjectCategory":
                        case "ProjectDescription":
                        case "ProjectEndDate":
                        case "ProjectID":
                        case "ProjectName":
                        case "ProjectOwner":
                        case "ProjectStartDate":
                        case "ProjectURL":
                        case "SurveyMethod":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "ResourceRelationship":
                    switch (propertyIdentifier)
                    {
                        case "RelatedResourceID":
                        case "RelationshipAccordingTo":
                        case "RelationshipEstablishedDate":
                        case "RelationshipOfResource":
                        case "RelationshipRemarks":
                        case "ResourceID":
                        case "ResourceRelationshipID":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Taxon":
                    switch (propertyIdentifier)
                    {
                        case "AcceptedNameUsage":
                        case "AcceptedNameUsageID":
                        case "Class":
                        case "DyntaxaTaxonID":
                        case "Family":
                        case "Genus":
                        case "HigherClassification":
                        case "InfraspecificEpithet":
                        case "Kingdom":
                        case "NameAccordingTo":
                        case "NameAccordingToID":
                        case "NamePublishedIn":
                        case "NamePublishedInID":
                        case "NamePublishedInYear":
                        case "NomenclaturalCode":
                        case "NomenclaturalStatus":
                        case "Order":
                        case "OrganismGroup":
                        case "OriginalNameUsage":
                        case "OriginalNameUsageID":
                        case "ParentNameUsage":
                        case "ParentNameUsageID":
                        case "Phylum":
                        case "ScientificName":
                        case "ScientificNameAuthorship":
                        case "ScientificNameID":
                        case "SpecificEpithet":
                        case "Subgenus":
                        case "TaxonConceptID":
                        case "TaxonConceptStatus":
                        case "TaxonID":
                        case "TaxonomicStatus":
                        case "TaxonRank":
                        case "TaxonRemarks":
                        // Support for taxon sort order has been removed.
                        // case "TaxonSortOrder":
                        case "TaxonURL":
                        case "VerbatimScientificName":
                        case "VerbatimTaxonRank":
                        case "VernacularName":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;
            }

            return isDarwinCoreField;
        }

        /// <summary>
        /// Test if specified species observation class and species
        /// observation property defines a species observation
        /// field that is included when protected species
        /// observation indication is used.
        /// </summary>
        /// <param name="classIdentifier">Species observation class identifier.</param>
        /// <param name="propertyIdentifier">Species observation property identifier.</param>
        /// <returns>
        /// True, if specified species observation class and species
        /// observation property defines a species observation
        /// field that is included when protected species
        /// observation indication is used.
        /// </returns>
        private static Boolean IsProtectedSpeciesObservationIndicationField(String classIdentifier,
                                                                            String propertyIdentifier)
        {
            Boolean isDarwinCoreField;

            isDarwinCoreField = false;
            switch (classIdentifier)
            {
                case "Conservation":
                    switch (propertyIdentifier)
                    {
                        case "ProtectionLevel":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "DarwinCore":
                    switch (propertyIdentifier)
                    {
                        case "Id":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Location":
                    switch (propertyIdentifier)
                    {
                        case "CoordinateX":
                        case "CoordinateY":
                        case "County":
                        case "DecimalLatitude":
                        case "DecimalLongitude":
                        case "Locality":
                        case "Municipality":
                        case "StateProvince":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Occurrence":
                    switch (propertyIdentifier)
                    {
                        case "OccurrenceID":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;

                case "Taxon":
                    switch (propertyIdentifier)
                    {
                        case "DyntaxaTaxonID":
                        case "ScientificName":
                        case "VernacularName":
                            isDarwinCoreField = true;
                            break;
                    }

                    break;
            }

            return isDarwinCoreField;
        }

        /// <summary>
        /// Log retrieval of species observations for protected taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="protectionInformation">Information about taxon protection.</param>
        /// <param name="method">Used method in web service.</param>
        private static void LogSpeciesObservations(WebServiceContext context,
                                                   List<WebDarwinCore> speciesObservations,
                                                   Dictionary<Int32, Int32> protectionInformation,
                                                   String method)
        {
            DataRow row;
            DataTable table;
            WebApplication application;
            WebPerson person;
            WebUser user;

            if (speciesObservations.IsNotEmpty())
            {
                user = WebServiceData.UserManager.GetUser(context);
                switch (user.Type)
                {
                    case UserType.Application:
                        application = WebServiceData.ApplicationManager.GetApplication(context, user.ApplicationId);
                        person = null;
                        break;
                    case UserType.Person:
                        application = null;
                        person = WebServiceData.UserManager.GetPerson(context);
                        break;
                    default:
                        throw new Exception("Unhandled user type :" + user.Type);
                }

                table = GetSpeciesObservationLogTable();

                foreach (WebDarwinCore speciesObservation in speciesObservations)
                {
                    row = table.NewRow();
                    row[0] = speciesObservation.Taxon.VernacularName;
                    row[1] = protectionInformation[speciesObservation.Taxon.DyntaxaTaxonID];
                    row[2] = null;
                    if ((user.Type == UserType.Person) && person.IsNotNull())
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        row[2] = person.FirstName + " " + person.LastName;
                    }
                    else
                    {
                        row[2] = context.GetUser().UserName;
                    }

                    row[3] = DateTime.Now;
                    row[4] = speciesObservation.Taxon.DyntaxaTaxonID;
                    row[5] = speciesObservation.Taxon.ScientificName;
                    row[6] = null;
                    if ((user.Type == UserType.Application) && (application != null) && (application.Name.IsNotNull()))
                    {
                        row[6] = application.Name;
                    }

                    row[7] = context.ClientToken.UserName;
                    row[8] = context.ClientToken.ApplicationIdentifier;
                    row[9] = context.ClientToken.ClientIpAddress;
                    row[10] = null; // Is automatically set by database.
                    row[11] = speciesObservation.Location.DecimalLongitude;
                    row[12] = speciesObservation.Location.DecimalLatitude;
                    row[13] = speciesObservation.Location.StateProvince;
                    row[14] = speciesObservation.Location.County;
                    row[15] = speciesObservation.Location.Municipality;
                    row[16] = speciesObservation.Location.Locality;
                    row[17] = speciesObservation.Occurrence.OccurrenceID;
                    row[18] = user.Id;
                    row[19] = method;
                    table.Rows.Add(row);
                }

                LogSpeciesObservations(context, table);
            }
        }

        /// <summary>
        /// Log retrieval of species observations for protected taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservations">Species observations.</param>
        /// <param name="protectionInformation">Information about taxon protection.</param>
        /// <param name="method">Used method in web service.</param>
        private static void LogSpeciesObservations(WebServiceContext context,
                                                   List<WebSpeciesObservation> speciesObservations,
                                                   Dictionary<Int32, Int32> protectionInformation,
                                                   String method)
        {
            DataRow row;
            DataTable table;
            Int32 taxonId;
            WebApplication application;
            WebPerson person;
            WebSpeciesObservationField field;
            WebUser user;

            if (speciesObservations.IsNotEmpty())
            {
                user = WebServiceData.UserManager.GetUser(context);
                switch (user.Type)
                {
                    case UserType.Application:
                        application = WebServiceData.ApplicationManager.GetApplication(context, user.ApplicationId);
                        person = null;
                        break;
                    case UserType.Person:
                        application = null;
                        person = WebServiceData.UserManager.GetPerson(context);
                        break;
                    default:
                        throw new Exception("Unhandled user type :" + user.Type);
                }

                table = GetSpeciesObservationLogTable();

                foreach (WebSpeciesObservation speciesObservation in speciesObservations)
                {
                    row = table.NewRow();
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Taxon.ToString(),
                                                               SpeciesObservationPropertyId.VernacularName.ToString());
                    if (field.IsNotNull())
                    {
                        row[0] = field.Value;
                    }

                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Taxon.ToString(),
                                                               SpeciesObservationPropertyId.DyntaxaTaxonID.ToString());
                    taxonId = field.Value.WebParseInt32();
                    // row[1] = protectionInformation[taxonId];
                    row[1] = protectionInformation[32];
                    row[2] = null;
                    if ((user.Type == UserType.Person) && person.IsNotNull())
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        row[2] = person.FirstName + " " + person.LastName;
                    }
                    else
                    {
                        row[2] = context.GetUser().UserName;
                    }

                    row[3] = DateTime.Now;
                    row[4] = taxonId;
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Taxon.ToString(),
                                                               SpeciesObservationPropertyId.ScientificName.ToString());
                    row[5] = field.Value;
                    row[6] = null;
                    if ((user.Type == UserType.Application) && (application != null) && (application.Name.IsNotNull()))
                    {
                        row[6] = application.Name;
                    }

                    row[7] = context.ClientToken.UserName;
                    row[8] = context.ClientToken.ApplicationIdentifier;
                    row[9] = context.ClientToken.ClientIpAddress;
                    row[10] = null; // Is automatically set by database.
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.DecimalLongitude.ToString());
                    row[11] = field.Value.WebParseDouble();
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.DecimalLatitude.ToString());
                    row[12] = field.Value.WebParseDouble();
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.StateProvince.ToString());
                    if (field.IsNotNull())
                    {
                        row[13] = field.Value;
                    }

                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.County.ToString());
                    if (field.IsNotNull())
                    {
                        row[14] = field.Value;
                    }

                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.Municipality.ToString());
                    if (field.IsNotNull())
                    {
                        row[15] = field.Value;
                    }

                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Location.ToString(),
                                                               SpeciesObservationPropertyId.Locality.ToString());
                    row[16] = field.Value;
                    field = speciesObservation.Fields.GetField(SpeciesObservationClassId.Occurrence.ToString(),
                                                               SpeciesObservationPropertyId.OccurrenceID.ToString());
                    row[17] = field.Value;
                    row[18] = user.Id;
                    row[19] = method;
                    table.Rows.Add(row);
                }

                LogSpeciesObservations(context, table);
            }
        }

        /// <summary>
        /// Log retrieval of species observations for protected taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="speciesObservationLogTable">Species observation log table.</param>
        private static void LogSpeciesObservations(WebServiceContext context,
                                                   DataTable speciesObservationLogTable)
        {
            if (context.GetDatabase().HasPendingTransaction())
            {
                // Create new database connection that has no transaction.
                // The log entry may be removed (rollback of transaction) 
                // if the database connection has an active transaction.
                using (WebServiceDataServer database = WebServiceData.DatabaseManager.GetDatabase(context))
                {
                    database.AddTableData(speciesObservationLogTable);
                }
            }
            else
            {
                context.GetDatabase().AddTableData(speciesObservationLogTable);
            }
        }
    }
}
