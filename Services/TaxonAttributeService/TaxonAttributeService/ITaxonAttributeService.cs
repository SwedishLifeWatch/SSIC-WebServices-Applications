using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace TaxonAttributeService
{
    /// <summary>
    /// Interface to the taxon attribute web service.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface ITaxonAttributeService
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
        /// Create species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        [OperationContract]
        [OperationTelemetry]
        void CreateSpeciesFacts(WebClientInformation clientInformation,
                                List<WebSpeciesFact> createSpeciesFacts);

        /// <summary>
        /// Delete species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteSpeciesFacts(WebClientInformation clientInformation,
                                List<WebSpeciesFact> deleteSpeciesFacts);

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all factor data types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor data types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorDataType> GetFactorDataTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all factor field enumerations.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field enumerations.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorFieldEnum> GetFactorFieldEnums(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor field types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorFieldType> GetFactorFieldTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factor origins.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorOrigin> GetFactorOrigins(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all factors.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All factors.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactor> GetFactors(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">The factor search criteria.</param>
        /// <returns>Factors that matches the search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactor> GetFactorsBySearchCriteria(WebClientInformation clientInformation,
                                                   WebFactorSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about all factor trees.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor trees.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorTreeNode> GetFactorTrees(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Factor tree information.</param>
        /// <returns>Factor trees.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(WebClientInformation clientInformation,
                                                               WebFactorTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about all factor update modes.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Factor update modes.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorUpdateMode> GetFactorUpdateModes(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all individual categories.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All individual categories.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebIndividualCategory> GetIndividualCategories(WebClientInformation clientInformation);

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
        /// Get information about all periods.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All periods.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPeriod> GetPeriods(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all period types.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Period types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPeriodType> GetPeriodTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about all species fact qualities.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>All species fact qualities.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFactQuality> GetSpeciesFactQualities(WebClientInformation clientInformation);

        /// <summary>
        /// Get species facts with specified identifiers.
        /// Only existing species facts are returned,
        /// e.g. species fact identifiers that does not
        /// match existing species fact does not affect
        /// the returned species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIdentifiers">
        /// Species facts identifiers. E.g. WebSpeciesFacts
        /// instances where id for requested combination of
        /// factor, host, individual category, period and taxon
        /// has been set.
        /// Host id is only used together with taxonomic factors.
        /// Period id is only used together with periodic factors.
        /// </param>
        /// <returns>
        /// Existing species facts among the
        /// requested species facts.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsByIdentifiers(WebClientInformation clientInformation,
                                                          List<WebSpeciesFact> speciesFactIdentifiers);

        /// <summary>
        /// Get information about all species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species facts.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsByIds(WebClientInformation clientInformation, List<int> speciesFactIds);

        /// <summary>
        /// Get information about species facts that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsBySearchCriteria(WebClientInformation clientInformation,
                                                             WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Get taxa that matches fact search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa that matches fact search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation, WebSpeciesFactSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa count of taxa that matches fact search criteria.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Taxa count that matches fact search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        Int32 GetTaxaCountBySearchCriteria(WebClientInformation clientInformation, WebSpeciesFactSearchCriteria searchCriteria);

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
        /// Update species facts.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateSpeciesFacts(WebClientInformation clientInformation,
                                List<WebSpeciesFact> updateSpeciesFacts);
    }
}
