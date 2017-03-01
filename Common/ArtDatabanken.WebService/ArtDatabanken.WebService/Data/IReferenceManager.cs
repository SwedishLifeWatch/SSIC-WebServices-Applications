using System;
using System.Collections.Generic;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Interface that handles reference related information.
    /// </summary>
    public interface IReferenceManager
    {
        /// <summary> 
        /// Get information about specified reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>Specified reference relation.</returns>
        WebReferenceRelation GetReferenceRelationById(WebServiceContext context,
                                                      Int32 referenceRelationId);

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebServiceContext context,
                                                                            String relatedObjectGuid);

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All reference relation types.</returns>
        List<WebReferenceRelationType> GetReferenceRelationTypes(WebServiceContext context);
    }
}
