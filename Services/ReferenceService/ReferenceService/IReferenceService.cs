using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace ReferenceService
{
    /// <summary>
    /// Interface to the reference web service.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IReferenceService
    {
        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void CommitTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">New reference to create.</param>
        [OperationContract]
        [OperationTelemetry]
        void CreateReference(WebClientInformation clientInformation,
                             WebReference reference);

        /// <summary>
        /// Creates a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelation">Reference relation object.</param>
        /// <returns>A WebReferenceRelation object with the created relation.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebReferenceRelation CreateReferenceRelation(WebClientInformation clientInformation,
                                                     WebReferenceRelation referenceRelation);

        /// <summary>
        /// Delete specified reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">The reference relation id.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteReferenceRelation(WebClientInformation clientInformation,
                                     Int32 referenceRelationId);

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [OperationContract]
        List<WebLogRow> GetLog(WebClientInformation clientInformation,
                               LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary> 
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>A WebReferenceRelation object.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebReferenceRelation GetReferenceRelationById(WebClientInformation clientInformation,
                                                      Int32 referenceRelationId);

        /// <summary>
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebClientInformation clientInformation,
                                                                            String relatedObjectGuid);

        /// <summary>
        /// Get all reference relation types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All reference relation types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReferenceRelationType> GetReferenceRelationTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All references.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReference> GetReferences(WebClientInformation clientInformation);

        /// <summary>
        /// Get specified references.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="referenceIds">Reference ids.</param>
        /// <returns>Specified references.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReference> GetReferencesByIds(WebClientInformation clientInformation,
                                              List<Int32> referenceIds);

        /// <summary>
        /// Get references that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Reference search criteria.</param>
        /// <returns>References that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReference> GetReferencesBySearchCriteria(WebClientInformation clientInformation,
                                                         WebReferenceSearchCriteria searchCriteria);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">The password.</param>
        /// <param name="applicationIdentifier">
        /// Identifier of the application that the user uses.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates that the user account must
        /// be activated before login can succeed.
        /// </param>
        /// <returns>
        /// Token and user authorities for the specified application
        /// or null if the login failed.
        /// </returns>
        [OperationContract]
        WebLoginResponse Login(String userName,
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
        /// Rollback a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void RollbackTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negative impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        [OperationContract]
        void StartTransaction(WebClientInformation clientInformation,
                              Int32 timeout);

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Update existing reference.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="reference">Existing reference to update.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateReference(WebClientInformation clientInformation,
                             WebReference reference);
    }
}
