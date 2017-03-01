using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Proxy;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class that handles reference related information.
    /// </summary>
    public class ReferenceManager : ManagerBase, IReferenceManager
    {
        /// <summary> 
        /// Get information about specified reference relation.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>Specified reference relation.</returns>
        public virtual WebReferenceRelation GetReferenceRelationById(WebServiceContext context,
                                                                     Int32 referenceRelationId)
        {
            WebClientInformation clientInformation;
            
            // Get information from reference service.
            clientInformation = GetClientInformation(context, WebServiceId.ReferenceService);
            return WebServiceProxy.ReferenceService.GetReferenceRelationById(clientInformation, referenceRelationId);
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public virtual List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebServiceContext context,
                                                                                           String relatedObjectGuid)
        {
            WebClientInformation clientInformation;
            
            // Get information from reference service.
            clientInformation = GetClientInformation(context, WebServiceId.ReferenceService);
            return WebServiceProxy.ReferenceService.GetReferenceRelationsByRelatedObjectGuid(clientInformation, relatedObjectGuid);
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>All reference relation types.</returns>
        public virtual List<WebReferenceRelationType> GetReferenceRelationTypes(WebServiceContext context)
        {
            WebClientInformation clientInformation;

            // Get information from reference service.
            clientInformation = GetClientInformation(context, WebServiceId.ReferenceService);
            return WebServiceProxy.ReferenceService.GetReferenceRelationTypes(clientInformation);
        }
    }
}
