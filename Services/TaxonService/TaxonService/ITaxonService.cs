using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.WebService.Data;
using Microsoft.ApplicationInsights.Wcf;

namespace TaxonService
{
    /// <summary>
    /// Interface to service that handles taxon information.
    /// </summary>
    [ServiceContract(Namespace = "urn:WebServices.ArtDatabanken.slu.se",
                     SessionMode = SessionMode.NotAllowed)]
    public interface ITaxonService
    {
        /// <summary>
        /// Check in a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <returns>Updated taxon revision with revision state = CLOSED</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision CheckInTaxonRevision(WebClientInformation clientInformation,
                                              WebTaxonRevision taxonRevision);

        /// <summary>
        /// Check out a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <returns>Updated taxon revision with revision state = ONGOING</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision CheckOutTaxonRevision(WebClientInformation clientInformation,
                                               WebTaxonRevision taxonRevision);

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void ClearCache(WebClientInformation clientInformation);

        /// <summary>
        /// Commit a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void CommitTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="lumpSplitEvent">
        /// The lump split event.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebLumpSplitEvent CreateLumpSplitEvent(WebClientInformation clientInformation,
                                               WebLumpSplitEvent lumpSplitEvent);

        /// <summary>
        /// Create a new taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxon">Object representing the taxon.</param>
        /// <param name="taxonRevisionEvent">Taxon revision event.</param>
        /// <returns>WebTaxon object with the created taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxon CreateTaxon(WebClientInformation clientInformation,
                             WebTaxon taxon,
                             WebTaxonRevisionEvent taxonRevisionEvent);

        /// <summary>
        /// Creates a new taxon name.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonName">Object representing the taxon name.</param>
        /// <returns>Created taxon name.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonName CreateTaxonName(WebClientInformation clientInformation,
                                     WebTaxonName taxonName);

        /// <summary>
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonProperties">
        /// The taxon properties.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonProperties CreateTaxonProperties(WebClientInformation clientInformation,
                                                 WebTaxonProperties taxonProperties);

        /// <summary>
        /// Creates a new taxonrelation.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRelation">The taxon relation.</param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRelation CreateTaxonRelation(WebClientInformation clientInformation,
                                             WebTaxonRelation taxonRelation);

        /// <summary>
        /// Creates a new Revision.
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        /// <returns>
        /// The newly created object.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision CreateTaxonRevision(WebClientInformation clientInformation,
                                             WebTaxonRevision taxonRevision);

        /// <summary>
        /// Creates a new Revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEvent">
        /// The revision Event.
        /// </param>
        /// <returns>
        /// The newly created object.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevisionEvent CreateTaxonRevisionEvent(WebClientInformation clientInformation,
                                                       WebTaxonRevisionEvent taxonRevisionEvent);

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionId">Id for a taxon revision.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteTaxonRevision(WebClientInformation clientInformation,
                                 Int32 taxonRevisionId);

        /// <summary>
        /// Delete specified taxon revision event.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionEventId">Id for the taxon revision event to be deleted.</param>
        [OperationContract]
        [OperationTelemetry]
        void DeleteTaxonRevisionEvent(WebClientInformation clientInformation,
                                      Int32 taxonRevisionEventId);

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void DeleteTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
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
        /// Get a lump split event by GUID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="lumpSplitEventGuid">GUID for a lump split event.</param>
        /// <returns>Requested lump split event.</returns>    
        [OperationContract]
        [OperationTelemetry]
        WebLumpSplitEvent GetLumpSplitEventByGuid(WebClientInformation clientInformation,
                                                  String lumpSplitEventGuid);

        /// <summary>
        /// Get lump split event with specified id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="lumpSplitEventId">Id of the lump split event.</param>
        /// <returns>Lump split event with specified id.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebLumpSplitEvent GetLumpSplitEventById(WebClientInformation clientInformation,
                                                Int32 lumpSplitEventId);

        /// <summary>
        /// Get lump split events related to
        /// specified new replacing taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="newReplacingTaxonId">Id of the new replacing taxon.</param>
        /// <returns>Lump split events related to specified new replacing taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebLumpSplitEvent> GetLumpSplitEventsByNewReplacingTaxon(WebClientInformation clientInformation,
                                                                      Int32 newReplacingTaxonId);

        /// <summary>
        /// Get lump split events related to specified old replaced taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="oldReplacedTaxonId">Id of the old replaced taxon.</param>
        /// <returns>Lump split events related to specified old replaced taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebLumpSplitEvent> GetLumpSplitEventsByOldReplacedTaxon(WebClientInformation clientInformation,
                                                                     Int32 oldReplacedTaxonId);

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All lump split event types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebLumpSplitEventType> GetLumpSplitEventTypes(WebClientInformation clientInformation);

        /// <summary> 
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>A WebReferenceRelation object.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebReferenceRelation GetReferenceRelationById(WebClientInformation clientInformation,
                                                      Int32 referenceRelationId);

        /// <summary>
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebClientInformation clientInformation,
                                                                            String relatedObjectGuid);
        /// <summary>
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get all reference relation types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All reference relation types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebReferenceRelationType> GetReferenceRelationTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        [OperationContract]
        List<WebResourceStatus> GetStatus(WebClientInformation clientInformation);

        /// <summary>
        /// Get list of taxon objects  idetified by its id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonIds">List of taxon id.</param>
        /// <returns>
        /// All taxon with id matching the list of id
        /// or NULL if there are no taxon.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaByIds(WebClientInformation clientInformation,
                                    List<Int32> taxonIds);

        /// <summary>
        /// Get taxa that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Taxa search criteria.</param>
        /// <returns>All taxa with  matching search criteria
        /// or NULL if there are no taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation,
                                               WebTaxonSearchCriteria searchCriteria);

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon alert statuses.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonAlertStatus> GetTaxonAlertStatuses(WebClientInformation clientInformation);

        /// <summary>
        /// Get a taxon by GUID
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonGuid">Taxons GUID.</param>
        /// <returns>
        /// WebTaxon with information about a Taxon
        /// or NULL if the Taxon is not found.
        /// </returns>    
        [OperationContract]
        [OperationTelemetry]
        WebTaxon GetTaxonByGuid(WebClientInformation clientInformation,
                                String taxonGuid);

        /// <summary>
        /// Gets a taxon by its Id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonId">Id of taxon.</param>
        /// <returns>WebTaxon object with the selected taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxon GetTaxonById(WebClientInformation clientInformation,
                              Int32 taxonId);

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon categories.</returns>       
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonCategory> GetTaxonCategories(WebClientInformation clientInformation);

        /// <summary>
        /// Get all taxon categories related to specified taxon.
        /// This includes: 
        /// Taxon categories for parent taxa to specified taxon.
        /// Taxon category for specified taxon.
        /// Taxon categories for child taxa to specified taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>All taxon categories related to specified taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonCategory> GetTaxonCategoriesByTaxonId(WebClientInformation clientInformation,
                                                           Int32 taxonId);

        /// <summary>
        /// Get list of changes made in taxon tree.
        /// Current version return changes regarding:
        /// - new taxon
        /// - new/edited taxon name (scientific + common)
        /// - lump/split events
        /// - taxon category changes
        /// </summary>
        /// <param name="clientInformation">WebClientInformation.</param>
        /// <param name="rootTaxonId">A root taxon id. Changes made for child taxa to this root taxon are returned.
        /// If parameter is NULL all changes are returned</param>
        /// <param name="isRootTaxonIdSpecified">Boolean true if rootTaxonId has a value, otherwise false.</param>
        /// <param name="dateFrom">Return changes from and including this date.</param>
        /// <param name="dateTo">Return changes to and including this date.</param>
        /// <returns>List of changes made</returns> 
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonChange> GetTaxonChange(WebClientInformation clientInformation,
                                            Int32 rootTaxonId,
                                            Boolean isRootTaxonIdSpecified,
                                            DateTime dateFrom,
                                            DateTime dateTo);

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon change statuses.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonChangeStatus> GetTaxonChangeStatuses(WebClientInformation clientInformation);

        /// <summary>
        /// Get child quality statistics for specified root taxon.
        /// The specified root taxon is included in the statistics.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="rootTaxonId">Id of the root taxon</param>
        /// <returns>Child quality statistics for specified root taxon.</returns>     
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonChildQualityStatistics> GetTaxonChildQualityStatistics(WebClientInformation clientInformation,
                                                                            Int32 rootTaxonId);

        /// <summary>
        /// Get child statistics for specified root taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="rootTaxonId">Id of the root taxon</param>
        /// <returns>Child statistics for specified root taxon.</returns>     
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonChildStatistics> GetTaxonChildStatistics(WebClientInformation clientInformation,
                                                              Int32 rootTaxonId);

        /// <summary>
        /// Get concept definition for specified taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition for specified taxon.</returns>
        [OperationContract]
        [OperationTelemetry]
        String GetTaxonConceptDefinition(WebClientInformation clientInformation,
                                         WebTaxon taxon);

        /// <summary>
        /// Get a taxonname by GUID
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonNameGuid">Taxons GUID.</param>
        /// <returns>
        /// </returns>    
        [OperationContract]
        [OperationTelemetry]
        WebTaxonName GetTaxonNameByGuid(WebClientInformation clientInformation,
                                        String taxonNameGuid);

        /// <summary>
        /// Get taxon name by id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonNameId">The taxon name id.</param>
        /// <returns>WebTaxonName or NULL if no name is found</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonName GetTaxonNameById(WebClientInformation clientInformation,
                                      Int32 taxonNameId);

        /// <summary>
        /// Gets all taxon name categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>WebTaxonNameCategory list with the all taxon name cateories.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameCategory> GetTaxonNameCategories(WebClientInformation clientInformation);

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon name category types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameCategoryType> GetTaxonNameCategoryTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Gets the taxon names by search criteria
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>List of WebTaxonNames or NULL if no names are found</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebClientInformation clientInformation,
                                                         WebTaxonNameSearchCriteria searchCriteria);

        /// <summary>
        /// Gets the taxon names by taxon id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>List of WebTaxonNames or NULL if no names are found</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonName> GetTaxonNamesByTaxonId(WebClientInformation clientInformation,
                                                  Int32 taxonId);

        /// <summary>
        /// Get all taxon names for a taxon ids.
        /// The result is sorted in the same order as input taxon ids.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxon names.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<List<WebTaxonName>> GetTaxonNamesByTaxonIds(WebClientInformation clientInformation,
                                                         List<Int32> taxonIds);

        /// <summary>
        /// Get information about possbile statuses for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possbile statuses for taxon names.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameStatus> GetTaxonNameStatuses(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about possible usage for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possible usage for taxon names.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonNameUsage> GetTaxonNameUsages(WebClientInformation clientInformation);

        /// <summary>
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonId">
        /// The taxon properties id.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonProperties> GetTaxonPropertiesByTaxonId(WebClientInformation clientInformation,
                                                             Int32 taxonId);

        /// <summary>
        /// Gets taxonrelation by id.
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRelationId">
        /// The id.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRelation GetTaxonRelationById(WebClientInformation clientInformation,
                                              Int32 taxonRelationId);

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRelation> GetTaxonRelationsBySearchCriteria(WebClientInformation clientInformation,
                                                                 WebTaxonRelationSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxon revision with specified GUID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionGuid">Taxon revision GUID.</param>
        /// <returns>Taxon revision with specified GUID.</returns>    
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision GetTaxonRevisionByGuid(WebClientInformation clientInformation,
                                                String taxonRevisionGuid);

        /// <summary>
        /// Get taxon revision with specified id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionId">Taxon revision id.</param>
        /// <returns>Taxon revision with specified id.</returns>    
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision GetTaxonRevisionById(WebClientInformation clientInformation,
                                              Int32 taxonRevisionId);

        /// <summary>
        /// Get taxon revision event with specified id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEventId">The revision event id.</param>
        /// <returns>Taxon revision event with specified id.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevisionEvent GetTaxonRevisionEventById(WebClientInformation clientInformation,
                                                        Int32 taxonRevisionEventId);

        /// <summary>
        /// Get taxon revision events that belongs
        /// to specified taxon revision.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionId">Id of the taxon revision </param>
        /// <returns>
        /// Taxon revision events that belongs
        /// to specified taxon revision.
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRevisionEvent> GetTaxonRevisionEventsByTaxonRevisionId(WebClientInformation clientInformation,
                                                                            Int32 taxonRevisionId);

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision event types.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRevisionEventType> GetTaxonRevisionEventTypes(WebClientInformation clientInformation);

        /// <summary>
        /// Get taxon revisons that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Revision search criteria.</param>
        /// <returns>Taxon revisons that matches search criteria.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRevision> GetTaxonRevisionsBySearchCriteria(WebClientInformation clientInformation,
                                                                 WebTaxonRevisionSearchCriteria searchCriteria);

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>List of web revisions.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRevision> GetTaxonRevisionsByTaxonId(WebClientInformation clientInformation,
                                                          Int32 taxonId);

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision states.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonRevisionState> GetTaxonRevisionStates(WebClientInformation clientInformation);

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        [OperationContract]
        [OperationTelemetry]
        List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebClientInformation clientInformation,
                                                             WebTaxonTreeSearchCriteria searchCriteria);

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
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void RollbackTransaction(WebClientInformation clientInformation);

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="userName">User name.</param>
        [OperationContract]
        void StartTrace(WebClientInformation clientInformation,
                        String userName);

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
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
        /// <param name="clientInformation">Client information.</param>
        [OperationContract]
        void StopTrace(WebClientInformation clientInformation);

        /// <summary>
        /// Updates an existing taxon name.
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonName">
        /// The taxon name.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonName UpdateTaxonName(WebClientInformation clientInformation, WebTaxonName taxonName);

        /// <summary>
        /// Updates an existing taxonproperty
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonProperties">
        /// The taxon properties.
        /// </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonProperties UpdateTaxonProperties(WebClientInformation clientInformation, WebTaxonProperties taxonProperties);

        /// <summary>
        /// Updates a taxonrelation.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRelation">The taxon relation. </param>
        /// <returns>
        /// </returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRelation UpdateTaxonRelation(WebClientInformation clientInformation, WebTaxonRelation taxonRelation);

        /// <summary>
        /// Update taxon revision.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevision">Taxon revision.</param>
        /// <returns>Updated taxon revision.</returns>
        [OperationContract]
        [OperationTelemetry]
        WebTaxonRevision UpdateTaxonRevision(WebClientInformation clientInformation,
                                             WebTaxonRevision taxonRevision);

        /// <summary>
        /// Update taxon revision event.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionEventId">Taxon revision event id.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateTaxonRevisionEvent(WebClientInformation clientInformation,
                                      Int32 taxonRevisionEventId);

        /// <summary>
        /// Update sort order for all child taxa with same parent taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="parentTaxonId">Id of the parent taxon.</param>
        /// <param name="childTaxonIds">Sorted list of child taxon ids.</param>
        /// <param name="taxonRevisionEventId">The taxon revision event id.</param>
        [OperationContract]
        [OperationTelemetry]
        void UpdateTaxonTreeSortOrder(WebClientInformation clientInformation,
                                      Int32 parentTaxonId,
                                      List<Int32> childTaxonIds,
                                      Int32 taxonRevisionEventId);
    }
}
