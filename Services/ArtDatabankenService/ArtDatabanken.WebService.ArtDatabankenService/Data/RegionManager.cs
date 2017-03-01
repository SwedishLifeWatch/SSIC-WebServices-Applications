using System;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Class that handles region related information.
    /// </summary>
    public class RegionManager : ManagerBase, IRegionManager
    {
        /// <summary>
        /// Get region geography cache key.
        /// </summary>
        /// <param name="regionId">Region id.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Region geography cache key.</returns>       
        private String GetRegionGeographyCacheKey(Int32 regionId,
                                                  WebCoordinateSystem coordinateSystem)
        {
            return GetRegionGeographyCacheKey(regionId.WebToString(), coordinateSystem);
        }

        /// <summary>
        /// Get region geography cache key.
        /// </summary>
        /// <param name="regionUniqueString">String that is unique for a region.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Region geography cache key.</returns>       
        private String GetRegionGeographyCacheKey(String regionUniqueString,
                                                  WebCoordinateSystem coordinateSystem)
        {
            return Settings.Default.RegionGeographyCacheKey +
                   Settings.Default.CacheKeyDelimiter +
                   regionUniqueString +
                   Settings.Default.CacheKeyDelimiter +
                   coordinateSystem.GetWkt();
        }

        /// <summary>
        /// Get ids for specified regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <returns>Ids for specified regions.</returns>       
        public virtual List<Int32> GetRegionIdsByGuids(WebServiceContext context,
                                                       List<String> regionGuids)
        {
            return GetRegionsByGuids(context, regionGuids).GetIds();
        }

        /// <summary>
        /// Get specified regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <returns>Geography for regions.</returns>       
        public virtual List<WebRegion> GetRegionsByGuids(WebServiceContext context,
                                                         List<String> regionGuids)
        {
            List<String> notChachedRegionGuids;
            List<WebRegion> notCachedRegions, regions;
            String regionGuidCacheKey;
            WebClientInformation clientInformation;
            WebRegion region;

            regions = new List<WebRegion>();
            notChachedRegionGuids = new List<String>();
            if (regionGuids.IsNotEmpty())
            {
                foreach (String regionGuid in regionGuids)
                {
                    // Get cached information.
                    regionGuidCacheKey = regionGuid.ToLower();
                    region = (WebRegion)(context.GetCachedObject(regionGuidCacheKey));
                    if (region.IsNull())
                    {
                        // Data not in cache - add GUID to not cached GUIDs.
                        notChachedRegionGuids.Add(regionGuid);
                    }
                    else
                    {
                        regions.Add(region);
                    }
                }
            }

            if (notChachedRegionGuids.IsNotEmpty())
            {
                // Get information from geo reference service.
                clientInformation = GetClientInformation(context, WebServiceId.GeoReferenceService);
                notCachedRegions = WebServiceProxy.GeoReferenceService.GetRegionsByGUIDs(clientInformation,
                                                                                         notChachedRegionGuids);
                regions.AddRange(notCachedRegions);

                foreach (String regionGuid in notChachedRegionGuids)
                {
                    // Add information to cache.
                    regionGuidCacheKey = regionGuid.ToLower();
                    context.AddCachedObject(regionGuidCacheKey,
                                            notCachedRegions.Get(regionGuid),
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.BelowNormal);
                }
            }

            return regions;
        }

        /// <summary>
        /// Get geography for regions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="regionGuids">Region GUIDs.</param>
        /// <param name="coordinateSystem">Coordinate system used in returned geography information.</param>
        /// <returns>Geography for regions.</returns>       
        public virtual List<WebRegionGeography> GetRegionsGeographyByGuids(WebServiceContext context,
                                                                           List<String> regionGuids,
                                                                           WebCoordinateSystem coordinateSystem)
        {
            List<String> notChachedRegionGuids;
            List<WebRegionGeography> notCachedRegionsGeography,
                                     regionsGeography;
            String regionGeographyGuidCacheKey;
            WebClientInformation clientInformation;
            WebRegionGeography regionGeography;

            regionsGeography = new List<WebRegionGeography>();
            notChachedRegionGuids = new List<String>();
            if (regionGuids.IsNotEmpty())
            {
                foreach (String regionGuid in regionGuids)
                {
                    // Get cached information.
                    regionGeographyGuidCacheKey = GetRegionGeographyCacheKey(regionGuid, coordinateSystem);
                    regionGeography = (WebRegionGeography)(context.GetCachedObject(regionGeographyGuidCacheKey));
                    if (regionGeography.IsNull())
                    {
                        // Data not in cache - add GUID to not cached GUIDs.
                        notChachedRegionGuids.Add(regionGuid);
                    }
                    else
                    {
                        regionsGeography.Add(regionGeography);
                    }
                }
            }

            if (notChachedRegionGuids.IsNotEmpty())
            {
                // Get information from geo reference service.
                clientInformation = GetClientInformation(context, WebServiceId.GeoReferenceService);
                notCachedRegionsGeography = WebServiceProxy.GeoReferenceService.GetRegionsGeographyByGUIDs(clientInformation,
                                                                                                           notChachedRegionGuids,
                                                                                                           coordinateSystem);
                regionsGeography.AddRange(notCachedRegionsGeography);

                foreach (String regionGuid in notChachedRegionGuids)
                {
                    // Add information to cache.
                    regionGeographyGuidCacheKey = GetRegionGeographyCacheKey(regionGuid, coordinateSystem);
                    context.AddCachedObject(regionGeographyGuidCacheKey,
                                            notCachedRegionsGeography.Get(regionGuid),
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.BelowNormal);
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
        public virtual List<WebRegionGeography> GetRegionsGeographyByIds(WebServiceContext context,
                                                                         List<Int32> regionIds,
                                                                         WebCoordinateSystem coordinateSystem)
        {
            List<Int32> notChachedRegionIds;
            List<WebRegionGeography> notCachedRegionsGeography,
                                     regionsGeography;
            String regionGeographyGuidCacheKey;
            WebClientInformation clientInformation;
            WebRegionGeography regionGeography;

            regionsGeography = new List<WebRegionGeography>();
            notChachedRegionIds = new List<Int32>();
            if (regionIds.IsNotEmpty())
            {
                foreach (Int32 regionId in regionIds)
                {
                    // Get cached information.
                    regionGeographyGuidCacheKey = GetRegionGeographyCacheKey(regionId, coordinateSystem);
                    regionGeography = (WebRegionGeography)(context.GetCachedObject(regionGeographyGuidCacheKey));
                    if (regionGeography.IsNull())
                    {
                        // Data not in cache - add GUID to not cached GUIDs.
                        notChachedRegionIds.Add(regionId);
                    }
                    else
                    {
                        regionsGeography.Add(regionGeography);
                    }
                }
            }

            if (notChachedRegionIds.IsNotEmpty())
            {
                // Get information from geo reference service.
                clientInformation = GetClientInformation(context, WebServiceId.GeoReferenceService);
                notCachedRegionsGeography = WebServiceProxy.GeoReferenceService.GetRegionsGeographyByIds(clientInformation,
                                                                                                         notChachedRegionIds,
                                                                                                         coordinateSystem);
                regionsGeography.AddRange(notCachedRegionsGeography);

                foreach (Int32 regionId in notChachedRegionIds)
                {
                    // Add information to cache.
                    regionGeographyGuidCacheKey = GetRegionGeographyCacheKey(regionId, coordinateSystem);
                    context.AddCachedObject(regionGeographyGuidCacheKey,
                                            notCachedRegionsGeography.Get(regionId),
                                            DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                            CacheItemPriority.BelowNormal);
                }
            }

            return regionsGeography;
        }
    }
}
