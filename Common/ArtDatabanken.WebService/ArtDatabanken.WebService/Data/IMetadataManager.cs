using System;
using System.Collections.Generic;
using System.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Interface for manager classes handling Metadata.
    /// </summary>
    public interface IMetadataManager
    {
        /// <summary>
        /// Adds SpeciesObservationFieldDescriptions to the database, remember to call ClearCache after adding or updating
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="table">The SpeciesObservationFieldDescription table that's supposed to be added</param>
        void UpdateSpeciesObservationFieldDescription(WebServiceContext context, DataTable table);

        /// <summary>
        /// Adds SpeciesObservationFieldMappings to the database, remember to call ClearCache after adding or updating
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="table">The SpeciesObservationFieldMapping table that's supposed to be added</param>
        void UpdateSpeciesObservationFieldMapping(WebServiceContext context, DataTable table);

        /// <summary>
        /// Get all Species Observation Field Descriptions Extended information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reloadCache">Optional, if set to true the cache will be reset.</param>
        /// <returns>A List with all Species Observation Field Descriptions.</returns>
        List<WebSpeciesObservationFieldDescriptionExtended> GetSpeciesObservationFieldDescriptionsExtended(WebServiceContext context,
                                                                           Boolean reloadCache = false);

        /// <summary>
        /// Get all Species Observation Field Descriptions.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="reloadCache">Optional, if set to true the cache will be reset.</param>
        /// <returns>A List with all Species Observation Field Descriptions.</returns>
        List<WebSpeciesObservationFieldDescription> GetSpeciesObservationFieldDescriptions(WebServiceContext context,
                                                                           Boolean reloadCache = false);
    }
}
