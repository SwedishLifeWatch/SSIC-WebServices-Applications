using System;
using ArtDatabanken.WebService.GeoReferenceService.Database;

namespace ArtDatabanken.WebService.GeoReferenceService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get Geo reference database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Geo reference database.</returns>
        public static GeoReferenceServer GetGeoReferenceDatabase(this WebServiceContext context)
        {
            return (GeoReferenceServer)(context.GetDatabase());
        }
    }
}
