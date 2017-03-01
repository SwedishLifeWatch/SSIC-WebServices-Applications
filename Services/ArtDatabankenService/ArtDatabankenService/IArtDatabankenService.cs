using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.ArtDatabankenService.Data;
using Microsoft.ApplicationInsights.Wcf;
using NotUsedWebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using WebTaxon = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxon;
using NotUsedWebTaxonName = ArtDatabanken.WebService.Data.WebTaxonName;
using WebTaxonName = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonName;
using NotUsedWebTaxonNameSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonNameSearchCriteria;
using WebTaxonNameSearchCriteria = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonNameSearchCriteria;
using NotUsedWebTaxonSearchCriteria = ArtDatabanken.WebService.Data.WebTaxonSearchCriteria;
using WebTaxonSearchCriteria = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonSearchCriteria;
using WebTaxonTreeNode = ArtDatabanken.WebService.ArtDatabankenService.Data.WebTaxonTreeNode;

namespace ArtDatabankenService
{
    /// <summary>
    /// Interface to the ArtDatabanken web service.
    /// </summary>
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IArtDatabankenService" in both code and config file together.
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface IArtDatabankenService
    {
        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void ClearCache(String clientToken);

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void CommitTransaction(String clientToken);

        /// <summary>
        /// Create a new reference.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="reference">New reference.</param>
        [OperationContract]
        [OperationTelemetry]
        void CreateReference(String clientToken, WebReference reference);

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void DeleteTrace(String clientToken);

        /// <summary>
        /// Get all bird nest activities.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about bird nest activities.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebBirdNestActivity> GetBirdNestActivities(String clientToken);

        /// <summary>
        /// Get information about cities that matches the search string.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchString">String that city name must match.</param>
        /// <returns>Information about cities.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebCity> GetCitiesBySearchString(String clientToken,
                                              String searchString);

        /// <summary>
        /// Get information about swedish counties.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about swedish counties.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebCounty> GetCounties(String clientToken);

        /// <summary>
        /// Get information about databases.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Table with information about databases.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebDatabase> GetDatabases(String clientToken);

        /// <summary>
        /// Get information about database update.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about database update.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebDatabaseUpdate GetDatabaseUpdate(String clientToken);

        /// <summary>
        /// Get information about different endangered lists.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about endangered lists.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebEndangeredList> GetEndangeredLists(String clientToken);

        /// <summary>
        /// Get information about all factor data types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor field data types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorDataType> GetFactorDataTypes(String clientToken);

        /// <summary>
        /// Get information about all factor field enums.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor field enums.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorFieldEnum> GetFactorFieldEnums(String clientToken);

        /// <summary>
        /// Get information about all factor field types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor field types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorFieldType> GetFactorFieldTypes(String clientToken);

        /// <summary>
        /// Get all factor origins.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor origins.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorOrigin> GetFactorOrigins(String clientToken);

        /// <summary>
        /// Get information about all factors.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factors.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactor> GetFactors(String clientToken);

        /// <summary>
        /// Get information about factors that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="factorSearchCriteria">The factor search criteria.</param>
        /// <returns>Information about factors.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactor> GetFactorsBySearchCriteria(String clientToken,
                                                   WebFactorSearchCriteria factorSearchCriteria);

        /// <summary>
        /// Get information about all factor trees.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor tree information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorTreeNode> GetFactorTrees(String clientToken);

        /// <summary>
        /// Get information about factor trees that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The factor tree search criteria.</param>
        /// <returns>Factor tree information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorTreeNode> GetFactorTreesBySearchCriteria(String clientToken,
                                                               WebFactorTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about all factor update modes.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Factor update modes.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebFactorUpdateMode> GetFactorUpdateModes(String clientToken);

        /// <summary>
        /// Get information about host taxa.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="factorId">Id for for factor to get host taxa information about.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetHostTaxa(String clientToken,
                                   Int32 factorId,
                                   TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get all host taxa associated with a sertain taxon.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Host taxa information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetHostTaxaByTaxonId(String clientToken,
                                   Int32 taxonId,
                                   TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get all individual categories.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Individual Categories.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebIndividualCategory> GetIndividualCategories(String clientToken);

        /// <summary>
        /// Get entries from the web service log
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        [OperationContract]
        List<ArtDatabanken.WebService.ArtDatabankenService.Data.WebLogRow> GetLog(String clientToken,
                               ArtDatabanken.WebService.ArtDatabankenService.Data.LogType type,
                               String userName,
                               Int32 rowCount);

        /// <summary>
        /// Get all periods.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Periods.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPeriod> GetPeriods(String clientToken);

        /// <summary>
        /// Get all period types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Period types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebPeriodType> GetPeriodTypes(String clientToken);

        /// <summary>
        /// Get information about swedish provinces.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Information about swedish provinces.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebProvince> GetProvinces(String clientToken);

        /// <summary>
        /// Get all references.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>References.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReference> GetReferences(String clientToken);

        /// <summary>
        /// Get references by search string.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchString">Search string.</param>
        /// <returns>References that matches the search string.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReference> GetReferencesBySearchString(String clientToken, String searchString);

        /// <summary>
        /// Get information about all species fact qualities.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Species fact qualities.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFactQuality> GetSpeciesFactQualities(String clientToken);

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesFactIds">Ids for species facts to get information about.</param>
        /// <returns>Species fact information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsById(String clientToken,
                                                 List<Int32> speciesFactIds);

        /// <summary>
        /// Get information about species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesFacts">Species facts to get information about.</param>
        /// <returns>Species facts information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsByIdentifier(String clientToken,
                                                         List<WebSpeciesFact> speciesFacts);

        /// <summary>
        /// Get information about species facts that correspond to user paramter selection.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="userParameterSelection">The user parameter selection.</param>
        /// <returns>Information about species facts.</returns>
        /// <exception cref="ArgumentException">Thrown if factorSearchCriteria is null.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebSpeciesFact> GetSpeciesFactsByUserParameterSelection(String clientToken,
                                                                     WebUserParameterSelection userParameterSelection);

        /// <summary>
        /// Get information about species observations
        /// that has changed in the specified date range.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 100000 observations of each change type (new or updated)
        /// with information can be retrieved in one call.
        /// Max 1000000 observations of each change type (deleted, new
        /// or updated), with GUIDs or ids, can be retrieved in one call.
        /// Parameters changedFrom and changedTo may be the same date.
        /// Parameter changedTo must not be today or in the future.
        /// If parameter changedTo is yesterday the method call
        /// must be made after the nightly update of the 
        /// species observations have been performed. 
        /// Currently it is ok to call this method after 05:00
        /// if yesterdays species observations should be retrieved.
        /// Only date part of parameters changedFrom and changedTo
        /// are used. It does not matter what time of day that is set
        /// in parameters changedFrom and changedTo.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="changedFrom">Start date for changes.</param>
        /// <param name="changedTo">End date for changes.</param>
        /// <returns>Information about changed species observations.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationChange GetSpeciesObservationChange(String clientToken,
                                                                DateTime changedFrom,
                                                                DateTime changedTo);

        /// <summary>
        /// Get number of species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Number of species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        [OperationContract]
        [OperationTelemetry]
        Int32 GetSpeciesObservationCountBySearchCriteria(String clientToken,
                                                         ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria searchCriteria);

        /// <summary>
        /// Get requested species observations.
        /// Scope is restricted to those observations
        /// that the user has access rights to.
        /// Max 10000 observations can be retrieved in one call.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="speciesObservationIds">Id for species observations to get.</param>
        /// <param name="userRoleId">In which role is the user retrieving species observations.</param>
        /// <returns>Species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if to many species observation ids has been given.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationInformation GetSpeciesObservationsById(String clientToken,
                                                                    List<Int64> speciesObservationIds,
                                                                    Int32 userRoleId);

        /// <summary>
        /// Get information about species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Information about requested species observations.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        [OperationContract]
        [OperationTelemetry]
        WebSpeciesObservationInformation GetSpeciesObservationsBySearchCriteria(String clientToken,
                                                                                ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria searchCriteria);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(String clientToken);

        /// <summary>
        /// Get all taxa utelizing a sertain host taxon and any of its child taxa.
        /// The method is restricted to faktors of type substrate.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="hostTaxonId">Id for host taxon.</param>
        /// <param name="taxonInformationType">Type of host taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaByHostTaxonId(String clientToken,
                                   Int32 hostTaxonId,
                                   TaxonInformationType taxonInformationType);
        
        /// <summary>
        /// Get information about taxa.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonIds">Ids for taxa to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxa information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaById(String clientToken,
                                   List<Int32> taxonIds,
                                   TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get taxa information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="hasOrganismGroupId">Indicates if organism group id is set.</param>
        /// <param name="organismGroupId">Organism group id.</param>
        /// <param name="hasEndangeredListId">Indicates if endangered list id is set.</param>
        /// <param name="endangeredListId">Endangered list id.</param>
        /// <param name="hasRedlistCategoryId">Indicates if redlist category id is set.</param>
        /// <param name="redlistCategoryId">Redlist category id.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaByOrganismOrRedlist(String clientToken,
                                                  Boolean hasOrganismGroupId,
                                                  Int32 organismGroupId,
                                                  Boolean hasEndangeredListId,
                                                  Int32 endangeredListId,
                                                  Boolean hasRedlistCategoryId,
                                                  Int32 redlistCategoryId,
                                                  TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get information about taxa that matches the query.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="dataQuery">Data query.</param>
        /// <param name="taxonInformationType">Type of taxa information to get.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if query is null.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaByQuery(String clientToken,
                                      WebDataQuery dataQuery,
                                      TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get information about taxa that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonSearchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if taxonSearchCriteria is null.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySearchCriteria(String clientToken,
                                               WebTaxonSearchCriteria taxonSearchCriteria);

        /// <summary>
        /// Get all taxa for the species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Taxa information.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySpeciesObservations(String clientToken,
                                                    ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria searchCriteria);

        /// <summary>
        /// Get number of unique taxa for species observations
        /// that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <returns>Number of unique taxa for species observations that matches the search criteria.</returns>
        /// <exception cref="ArgumentException">Thrown if information in speciesObservationSearchCriteria is inconsistent.</exception>
        /// <exception cref="UnauthorizedAccessException">Thrown if user is not member of the specied role.</exception>
        [OperationContract]
        [OperationTelemetry]
        Int32 GetTaxaCountBySpeciesObservations(String clientToken,
                                                ArtDatabanken.WebService.ArtDatabankenService.Data.WebSpeciesObservationSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about a taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Taxon to get information about.</param>
        /// <param name="taxonInformationType">Type of taxon information to get.</param>
        /// <returns>Taxon information.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxon GetTaxonById(String clientToken,
                              Int32 taxonId,
                              TaxonInformationType taxonInformationType);

        /// <summary>
        /// Get information about occurence in swedish
        /// counties for specified taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Information about occurence in swedish counties for specified taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonCountyOccurrence> GetTaxonCountyOccurence(String clientToken,
                                                               Int32 taxonId);

        /// <summary>
        /// Get taxon names for specified taxon.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>Taxon names.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonName> GetTaxonNames(String clientToken,
                                         Int32 taxonId);

        /// <summary>
        /// Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The taxon name search criteria.</param>
        /// <returns>Taxon names.</returns>
        /// <exception cref="ArgumentException">Thrown if searchCriteria is null.</exception>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonName> GetTaxonNamesBySearchCriteria(String clientToken,
                                                         WebTaxonNameSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about all taxon name types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon name types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameType> GetTaxonNameTypes(String clientToken);

        /// <summary>
        /// Get information about all taxon name use types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon name use types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameUseType> GetTaxonNameUseTypes(String clientToken);

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(String clientToken,
                                                             WebTaxonTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Get information about all taxon types.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Taxon types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonType> GetTaxonTypes(String clientToken);

        /// <summary>
        /// Get information about current web service user.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <returns>Returns user information.</returns>
        [OperationContract]
        ArtDatabanken.WebService.ArtDatabankenService.Data.WebUser GetUser(String clientToken);

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
        /// Client token to use in furter requestes or
        /// null if user was not logged in.
        /// </returns>       
        [OperationContract]
        String Login(String userName,
                     String password,
                     String applicationIdentifier,
                     Boolean isActivationRequired);

        /// <summary>
        /// Logout user. Release resources.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void Logout(String clientToken);

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
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void RollbackTransaction(String clientToken);

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(String clientToken, String userName);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        [OperationContract]
        void StartTransaction(String clientToken, Int32 timeout);

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        [OperationContract]
        void StopTrace(String clientToken);

        /// <summary>
        /// Update reference with specific id.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="reference">Reference object.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateReference(String clientToken, WebReference reference);

        /// <summary>
        /// Update species facts.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateSpeciesFacts(String clientToken,
                                List<WebSpeciesFact> createSpeciesFacts,
                                List<WebSpeciesFact> deleteSpeciesFacts,
                                List<WebSpeciesFact> updateSpeciesFacts);

        /// <summary>
        /// Update species facts. This method should only be used by Dyntaxa web application.
        /// </summary>
        /// <param name="clientToken">Client information.</param>
        /// <param name="createSpeciesFacts">New species facts to create.</param>
        /// <param name="deleteSpeciesFacts">Existing species facts to delete.</param>
        /// <param name="updateSpeciesFacts">Existing species facts to update.</param>
        /// <param name="fullName">Full Name of editor.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateDyntaxaSpeciesFacts(String clientToken,
                                       List<WebSpeciesFact> createSpeciesFacts,
                                       List<WebSpeciesFact> deleteSpeciesFacts,
                                       List<WebSpeciesFact> updateSpeciesFacts,
                                       String fullName);
    }
}
