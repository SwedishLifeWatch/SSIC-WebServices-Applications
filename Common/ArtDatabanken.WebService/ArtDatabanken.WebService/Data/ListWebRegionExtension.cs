using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension methods to a generic list of type WebRegion.
    /// </summary>
    public static class ListWebRegionExtension
    {
        /// <summary>
        /// Get region with specified id.
        /// </summary>
        /// <param name="regions">Regions.</param>
        /// <param name="regionId">Region id.</param>
        /// <returns>Region with specified id.</returns>
        public static WebRegion Get(this List<WebRegion> regions,
                                    Int32 regionId)
        {
            if (regions.IsNotEmpty())
            {
                foreach (WebRegion region in regions)
                {
                    if (regionId == region.Id)
                    {
                        return region;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get region with specified GUID.
        /// </summary>
        /// <param name="regions">Regions.</param>
        /// <param name="regionGuid">Region GUID.</param>
        /// <returns>Region with specified GUID.</returns>
        public static WebRegion Get(this List<WebRegion> regions,
                                    String regionGuid)
        {
            if (regions.IsNotEmpty())
            {
                foreach (WebRegion region in regions)
                {
                    if (regionGuid.ToUpper() == region.GUID.ToUpper())
                    {
                        return region;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Get region ids.
        /// </summary>
        /// <param name="regions">Regions.</param>
        /// <returns>Region ids.</returns>
        public static List<Int32> GetIds(this List<WebRegion> regions)
        {
            List<Int32> ids;

            ids = new List<Int32>();
            if (regions.IsNotEmpty())
            {
                foreach (WebRegion region in regions)
                {
                    ids.Add(region.Id);
                }
            }
            return ids;
        }
    }
}
