using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace TaxonService
{
    /// <summary>
    /// Interface to service that handles specific Dyntaxa information. 
    /// Usually Revision information.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IDyntaxaInternalService
    {
        /// <summary>
        /// Gets dyntaxa revision species fact latest change for specific <paramref name="factorId"/> and 
        /// <paramref name="taxonId"/>  if any changes have been made in <paramref name="taxonRevisionId"/>.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>                
        /// <param name="factorId">The factor id.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <param name="taxonRevisionId">The taxon revision.</param>
        /// <returns>Species fact information if it has changed during the revision; otherwise null.</returns>
        [OperationContract]
        WebDyntaxaRevisionSpeciesFact GetDyntaxaRevisionSpeciesFact(
            WebClientInformation clientInformation,
            Int32 factorId,
            Int32 taxonId,
            Int32 taxonRevisionId);

        /// <summary>
        /// Get all dyntaxa revision species fact latest change for specific revision id
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="taxonRevisionId"></param>
        /// <returns></returns>
        [OperationContract]
        List<WebDyntaxaRevisionSpeciesFact> GetAllDyntaxaRevisionSpeciesFacts(
            WebClientInformation clientInformation,
            Int32 taxonRevisionId);

        /// <summary>
        /// Creates a new Dyntaxa revision species fact.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>        
        /// <param name="dyntaxaRevisionSpeciesFact">Object representing the Dyntaxa revision species fact.</param>
        /// <returns>Created Dyntaxa revision species fact.</returns>
        [OperationContract]
        WebDyntaxaRevisionSpeciesFact CreateDyntaxaRevisionSpeciesFact(
            WebClientInformation clientInformation,
            WebDyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact);      

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
        [OperationContract]
        WebTaxonRevisionEvent CreateCompleteRevisionEvent(
            WebClientInformation clientInformation,
            WebTaxonRevisionEvent taxonRevisionEvent);

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
        [OperationContract]
        WebLoginResponse Login(
            String userName,
            String password,
            String applicationIdentifier,
            Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void Logout(WebClientInformation clientInformation);

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        [OperationContract]
        Boolean Ping();

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        [OperationContract]
        void StartTransaction(
            WebClientInformation clientInformation,
            Int32 timeout);

        /// <summary>
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void RollbackTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void CommitTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Set revision species fact published flag to true
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="revisionId"></param>
        [OperationContract]
        bool SetRevisionSpeciesFactPublished(WebClientInformation clientInformation, int revisionId);

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
        [OperationContract]
        List<WebDyntaxaRevisionReferenceRelation> GetDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation,
            int revisionId,
            string relatedObjectGUID);

        /// <summary>
        /// Get all Dyntaxa Revision Reference relation items.
        /// </summary>        
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        [OperationContract]
        List<WebDyntaxaRevisionReferenceRelation> GetAllDyntaxaRevisionReferenceRelations(
            WebClientInformation clientInformation,
            int revisionId);

        /// <summary>
        /// Gets the dyntaxa revision reference relation by identifier.
        /// </summary>        
        /// <param name="clientInformation">Client information.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [OperationContract]
        WebDyntaxaRevisionReferenceRelation GetDyntaxaRevisionReferenceRelationById(
            WebClientInformation clientInformation,
            int id);

        /// <summary>
        /// Set revision reference relation published flag to true
        /// </summary>        
        /// <param name="clientInformation">Client information.</param>
        /// <param name="revisionId"></param>
        /// <returns></returns>
        [OperationContract]
        bool SetRevisionReferenceRelationPublished(WebClientInformation clientInformation, int revisionId);

        /// <summary>
        /// Creates a dyntaxa revision reference relation..
        /// </summary>        
        /// <param name="clientInformation">Client information.</param>
        /// <param name="dyntaxaRevisionReferenceRelation">The dyntaxa revision reference relation.</param>
        /// <returns></returns>        
        [OperationContract]
        WebDyntaxaRevisionReferenceRelation CreateDyntaxaRevisionReferenceRelation(
            WebClientInformation clientInformation,
            WebDyntaxaRevisionReferenceRelation dyntaxaRevisionReferenceRelation);
    }
}
