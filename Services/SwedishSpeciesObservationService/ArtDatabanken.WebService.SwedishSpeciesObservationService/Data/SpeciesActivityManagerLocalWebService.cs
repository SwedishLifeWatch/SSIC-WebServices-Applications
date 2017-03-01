using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// This class contains handling of species activity objects.
    /// </summary>
    public class SpeciesActivityManagerLocalWebService : WebService.Data.SpeciesActivityManager
    {
        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All bird nest activities.</returns>
        public override List<WebSpeciesActivity> GetBirdNestActivities(WebServiceContext context)
        {
            return SpeciesActivityManager.GetBirdNestActivities(context);
        }

        /// <summary>
        /// Get all species activities.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All species activities.</returns>
        public override List<WebSpeciesActivity> GetSpeciesActivities(WebServiceContext context)
        {
            return SpeciesActivityManager.GetSpeciesActivities(context);
        }
    }
}
