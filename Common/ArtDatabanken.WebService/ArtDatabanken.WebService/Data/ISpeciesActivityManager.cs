using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains handling of species activity objects.
    /// </summary>
    public interface ISpeciesActivityManager
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All bird nest activities.</returns>
        List<WebSpeciesActivity> GetBirdNestActivities(WebServiceContext context);

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species activities.</returns>
        List<WebSpeciesActivity> GetSpeciesActivities(WebServiceContext context);
    }
}
