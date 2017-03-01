using ArtDatabanken.WebService.TaxonService.Database;

namespace ArtDatabanken.WebService.TaxonService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get taxon database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon database.</returns>
        public static TaxonServer GetTaxonDatabase(this WebServiceContext context)
        {
            return (TaxonServer)(context.GetDatabase());
        }
    }
}
