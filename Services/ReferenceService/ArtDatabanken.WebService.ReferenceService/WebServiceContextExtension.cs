using ArtDatabanken.WebService.ReferenceService.Database;

namespace ArtDatabanken.WebService.ReferenceService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get reference database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Reference database.</returns>
        public static ReferenceServer GetReferenceDatabase(this WebServiceContext context)
        {
            return (ReferenceServer)(context.GetDatabase());
        }
    }
}
