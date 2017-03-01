using ArtDatabanken.WebService.AnalysisService.Database;

namespace ArtDatabanken.WebService.AnalysisService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get analysis service database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Species observation database.</returns>
        public static AnalysisServer GetAnalysisDatabase(this WebServiceContext context)
        {
            if (context != null)
            {
                return (AnalysisServer)(context.GetDatabase());
            }

            return null;
        }
    }
}
