using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.ReferenceService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using ReferenceManager = ArtDatabanken.WebService.ReferenceService.Data.ReferenceManager;

namespace ReferenceService
{
    /// <summary>
    /// Implementation of the reference web service.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ReferenceService : WebServiceBase, IReferenceService
    {
        /// <summary>
        /// Static constructor.
        /// </summary>
        static ReferenceService()
        {
            WebServiceData.ApplicationManager = new ApplicationManager();
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.CoordinateConversionManager = new CoordinateConversionManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.RegionManager = new RegionManager();
            WebServiceData.SpeciesActivityManager = new SpeciesActivityManager();
            WebServiceData.SpeciesObservationManager = new SpeciesObservationManager();
            WebServiceData.TaxonManager = new TaxonManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
        }

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">New reference to create.</param>
        public void CreateReference(WebClientInformation clientInformation,
                                    WebReference reference)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ReferenceManager.CreateReference(context, reference);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelation">Reference relation object to be created.</param>
        /// <returns>A WebReferenceRelation object with the created relation.</returns>
        public WebReferenceRelation CreateReferenceRelation(WebClientInformation clientInformation,
                                                            WebReferenceRelation referenceRelation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.CreateReferenceRelation(context, referenceRelation);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes specified reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">The reference relation id.</param>
        public void DeleteReferenceRelation(WebClientInformation clientInformation,
                                            Int32 referenceRelationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ReferenceManager.DeleteReferenceRelation(context, referenceRelationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary> 
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>A WebReferenceRelation object.</returns>
        public WebReferenceRelation GetReferenceRelationById(WebClientInformation clientInformation, int referenceRelationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferenceRelationById(context, referenceRelationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebClientInformation clientInformation,
                                                                                   String relatedObjectGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferenceRelationsByGuid(context, relatedObjectGuid);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All reference relation types.</returns>
        public List<WebReferenceRelationType> GetReferenceRelationTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferenceRelationTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All references.</returns>
        public List<WebReference> GetReferences(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferences(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        public List<WebReference> GetReferencesByIds(WebClientInformation clientInformation,
                                                     List<Int32> referenceIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferencesByIds(context, referenceIds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        public List<WebReference> GetReferencesBySearchCriteria(WebClientInformation clientInformation,
                                                                WebReferenceSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return ReferenceManager.GetReferencesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get web service context.
        /// This method is used to add Application Insights telemetry data from the request.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Web service context.</returns>
        private WebServiceContext GetWebServiceContext(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebServiceContext context;
            WebUser user;

            try
            {
                context = new WebServiceContext(clientInformation);
                try
                {
                    if (context.IsNotNull() && (Configuration.InstallationType == InstallationType.Production))
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            if (context.ClientToken.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = context.ClientToken.ApplicationIdentifier;
                                telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = context.ClientToken.ClientIpAddress;
                                telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = context.ClientToken.CreatedDate.WebToString();
                            }

                            user = context.GetUser();
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Do nothing. We don't want calls to fail because of logging problems.
                }
            }
            catch (ApplicationException)
            {
                LogClientToken(clientInformation);
                throw;
            }
            catch (ArgumentException)
            {
                LogClientToken(clientInformation);
                throw;
            }

            return context;
        }

        /// <summary>
        /// Add information about client to Application Insights.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        private void LogClientToken(WebClientInformation clientInformation)
        {
            RequestTelemetry telemetry;
            WebClientToken clientToken;
            WebUser user;

            try
            {
                if (Configuration.InstallationType == InstallationType.Production)
                {
                    clientToken = new WebClientToken(clientInformation.Token, WebServiceData.WebServiceManager.Key);
                    if (clientToken.IsNotNull())
                    {
                        telemetry = OperationContext.Current.GetRequestTelemetry();
                        if (telemetry.IsNotNull())
                        {
                            telemetry.Properties[TelemetryProperty.ApplicationIdentifier.ToString()] = clientToken.ApplicationIdentifier;
                            telemetry.Properties[TelemetryProperty.ClientIpAddress.ToString()] = clientToken.ClientIpAddress;
                            telemetry.Properties[TelemetryProperty.LoginDateTime.ToString()] = clientToken.CreatedDate.WebToString();

                            user = WebServiceData.UserManager.GetUser(clientToken.UserName);
                            if (user.IsNotNull())
                            {
                                telemetry.Properties[TelemetryProperty.UserId.ToString()] = user.Id.WebToString();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Do nothing. We don't want calls to fail because of logging problems.
            }
        }

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">Existing reference to update.</param>
        public void UpdateReference(WebClientInformation clientInformation,
                                    WebReference reference)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    ReferenceManager.UpdateReference(context, reference);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }
    }
}
