using ArtDatabanken.WebService.TaxonAttributeService.Database;

namespace ArtDatabanken.WebService.TaxonAttributeService
{
    /// <summary>
    /// Extension methods to the WebServiceContext class.
    /// </summary>
    public static class WebServiceContextExtension
    {
        /// <summary>
        /// Get taxon attribute database.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon attribute database.</returns>
        public static TaxonAttributeServer GetTaxonAttributeDatabase(this WebServiceContext context)
        {
            return (TaxonAttributeServer)(context.GetDatabase());
        }
    }
}
