namespace TaxonService
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using ArtDatabanken.WebService;
    using ArtDatabanken.WebService.Data;
    using ArtDatabanken.WebService.TaxonService.Data;
    using DatabaseManager = ArtDatabanken.WebService.TaxonService.Data.DatabaseManager;
    using TaxonManager = ArtDatabanken.WebService.TaxonService.Data.TaxonManager;
    using UserManager = ArtDatabanken.WebService.Data.UserManager;

    /// <summary>
    /// Implementation of the service that handles Dyntaxa revision.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class DyntaxaInternalService : WebServiceBase, IDyntaxaInternalService
    {   
        /// <summary>
        /// Static constructor.
        /// </summary>
        static DyntaxaInternalService()
        {
            WebServiceData.AuthorizationManager = new AuthorizationManager();
            WebServiceData.DatabaseManager = new DatabaseManager();
            WebServiceData.LogManager = new LogManager();
            WebServiceData.ReferenceManager = new ReferenceManager();
            WebServiceData.UserManager = new UserManager();
            WebServiceData.WebServiceManager = new WebServiceManager();
            ArtDatabanken.Data.ArtDatabankenService.UserManager.Login(WebServiceData.WebServiceManager.Name, WebServiceData.WebServiceManager.Password, "ArtDatabankenSOA", false);
        }

        /// <summary>
        /// Gets dyntaxa revision species fact latest change for specific <paramref name="factorId" /> and
        /// <paramref name="taxonId" />  if any changes have been made in <paramref name="taxonRevisionId" />.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="factorId">The factor id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="taxonRevisionId">The taxon revision.</param>
        /// <returns>
        /// Species fact information if it has changed during the revision; otherwise null.
        /// </returns>
        public WebDyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(
            WebClientInformation clientInformation,
            Int32 factorId,
            Int32 taxonId,
            Int32 taxonRevisionId)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.GetDyntaxaRevisionSpeciesFact(context, factorId, taxonId, taxonRevisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all dyntaxa revision species fact latest change for specific revision id
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="taxonRevisionId"></param>
        /// <returns></returns>
        public List<WebDyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(
            WebClientInformation clientInformation,
            Int32 taxonRevisionId)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.GetAllDyntaxaRevisionSpeciesFacts(context, taxonRevisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new Dyntaxa revision species fact.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="dyntaxaRevisionSpeciesFact">Object representing the Dyntaxa revision species fact.</param>
        /// <returns>
        /// Created Dyntaxa revision species fact.
        /// </returns>
        public WebDyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(
            WebClientInformation clientInformation,
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.CreateDyntaxaRevisionSpeciesFact(context, dyntaxaRevisionSpeciesFact);                    
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new complete Revision event, i.e. all revision event data is set.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEvent">
        /// The revision Event.
        /// </param>
        /// <returns>
        /// The newly created object.
        /// </returns>
        public WebTaxonRevisionEvent CreateCompleteRevisionEvent(
            WebClientInformation clientInformation,
            WebTaxonRevisionEvent taxonRevisionEvent)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.CreateCompleteRevisionEvent(context, taxonRevisionEvent);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Token and user roles for the specified application
        /// or null if the login failed.
        /// </returns>       
        public override WebLoginResponse Login(
            String userName,
            String password,
            String applicationIdentifier,
            Boolean isActivationRequired)
        {
            using (WebServiceContext context = new WebServiceContext(userName, applicationIdentifier))
            {
                try
                {
                    return TaxonManager.Login(
                        context,
                        userName,
                        password,
                        applicationIdentifier,
                        isActivationRequired);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="revisionId"></param>
        public bool SetRevisionSpeciesFactPublished(WebClientInformation clientInformation, int revisionId)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.SetRevisionSpeciesFactPublished(context, revisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get dyntaxa revision reference relation item(s).
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="relatedObjectGUID">The related object unique identifier.</param>
        /// <returns>
        /// A List of WebDyntaxaRevisionReferenceRelation if any revision steps have been
        /// made for specified (revisionId,relatedObjectGUID); otherwise null.
        /// </returns>
        public List<WebDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation, 
            int revisionId,
            string relatedObjectGUID)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.GetDyntaxaRevisionReferenceRelation(context, revisionId, relatedObjectGUID);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all Dyntaxa Revision Reference relation items.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId">Revision id.</param>
        /// <returns></returns>
        public List<WebDyntaxaRevisionReferenceRelation> GetAllDyntaxaRevisionReferenceRelations(
            WebClientInformation clientInformation, 
            int revisionId)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.GetAllDyntaxaRevisionReferenceRelations(context, revisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the dyntaxa revision reference relation by identifier.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionReferenceRelation GetDyntaxaRevisionReferenceRelationById(
            WebClientInformation clientInformation,
            int id)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.GetDyntaxaRevisionReferenceRelationById(context, id);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Set revision reference relation published flag to true
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        public bool SetRevisionReferenceRelationPublished(WebClientInformation clientInformation, int revisionId)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.SetRevisionReferenceRelationPublished(context, revisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a dyntaxa revision reference relation..
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="dyntaxaRevisionReferenceRelation">The dyntaxa revision reference relation.</param>
        /// <returns></returns>
        public WebDyntaxaRevisionReferenceRelation CreateDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation,
            WebDyntaxaRevisionReferenceRelation dyntaxaRevisionReferenceRelation)
        {
            using (WebServiceContext context = new WebServiceContext(clientInformation))
            {
                try
                {
                    return DyntaxaManager.CreateDyntaxaRevisionReferenceRelation(context, dyntaxaRevisionReferenceRelation);
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