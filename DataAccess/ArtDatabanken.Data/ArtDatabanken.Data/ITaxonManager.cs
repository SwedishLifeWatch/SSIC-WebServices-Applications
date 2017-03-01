using System;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the TaxonManager interface.
    /// </summary>
    public interface ITaxonManager : IManager
    {
        /// <summary>
        /// This property is used to retrieve or update taxon information.
        /// </summary>
        ITaxonDataSource DataSource { get; set; }

        /// <summary>
        /// This property is used to retrieve taxon information from PESI.
        /// </summary>
        IPesiNameDataSource PesiNameDataSource { get; set; }

        /// <summary>
        /// Checks in a revision.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        void CheckInTaxonRevision(IUserContext userContext,
                                  ITaxonRevision taxonRevision);

        /// <summary>
        /// Check out a revision.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevision">The revision.</param>
        void CheckOutTaxonRevision(IUserContext userContext,
                                   ITaxonRevision taxonRevision);

        /// <summary>
        /// Call to PESI WebService to get GUID for taxon.
        /// Match by taxon scientific name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        void CreatePESIData(IUserContext userContext, ITaxon taxon, ITaxonRevisionEvent taxonRevisionEvent);

        /// <summary>
        /// Creates a taxon, with revisionevent
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision"> The revision.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="scientificName">Scientific name.</param>
        /// <param name="commonName">Common name.</param>
        /// <param name="author">Author.</param>
        /// <param name="alertStatusId">Alert status id.</param>
        /// <param name="parentTaxon">Parent taxon.</param>
        /// <param name="taxonCategory">The taxon category.</param>
        /// <param name="conceptDefinition">Concept definition.</param>
        void CreateTaxon(IUserContext userContext,
                         ITaxonRevision taxonRevision,
                         ITaxon taxon,
                         String scientificName,
                         String commonName,
                         String author,
                         TaxonAlertStatusId alertStatusId,
                         ITaxon parentTaxon,
                         ITaxonCategory taxonCategory,
                         String conceptDefinition);

        /// <summary>
        /// Create a taxon name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">Taxon name.</param>
        void CreateTaxonName(IUserContext userContext,
                             ITaxonRevision taxonRevision,
                             ITaxonName taxonName);

        /// <summary>
        /// Removes the taxon by setting TaxonProperty.IsValid = false
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="taxonRevision">The revision.</param>
        void DeleteTaxon(IUserContext userContext,
                         ITaxon taxon,
                         ITaxonRevision taxonRevision);

        /// <summary>
        /// Deletes a name of the taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">TaxonName object.</param>
        void DeleteTaxonName(IUserContext userContext,
                             ITaxonRevision taxonRevision,
                             ITaxonName taxonName);

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">Taxon revision.</param>
        void DeleteTaxonRevision(IUserContext userContext,
                                 ITaxonRevision taxonRevision);

        /// <summary>
        /// Rolls back all changes for one revision event
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        /// <param name="taxonRevision">The revision.</param>
        void DeleteTaxonRevisionEvent(IUserContext userContext,
                                      ITaxonRevisionEvent taxonRevisionEvent,
                                      ITaxonRevision taxonRevision);

        /// <summary>
        /// Get lumpsplitevent by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="lumpSplitEventGuid">GUID.</param>
        /// <returns>Requested object.</returns>       
        ILumpSplitEvent GetLumpSplitEvent(IUserContext userContext,
                                          String lumpSplitEventGuid);

        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <returns>
        /// </returns>
        LumpSplitEventList GetLumpSplitEventsByNewReplacingTaxon(IUserContext userContext,
                                                                 ITaxon taxon);

        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <returns>
        /// </returns>
        LumpSplitEventList GetLumpSplitEventsByOldReplacedTaxon(IUserContext userContext,
                                                                ITaxon taxon);

        /// <summary>
        /// Get specified lump split event type.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="lumpSplitEventTypeId">Id for lump split event type.</param>
        /// <returns>Specified lump split event type.</returns>
        ILumpSplitEventType GetLumpSplitEventType(IUserContext userContext,
                                                  Int32 lumpSplitEventTypeId);

        /// <summary>
        /// Get specified lump split event type.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="lumpSplitEventTypeId">Id for lump split event type.</param>
        /// <returns>Specified lump split event type.</returns>
        ILumpSplitEventType GetLumpSplitEventType(IUserContext userContext,
                                                  LumpSplitEventTypeId lumpSplitEventTypeId);

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All lump split event types.</returns>
        LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext);

        /// <summary>
        /// Get list of taxa matching search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">search criteria for taxon</param>
        /// <returns>
        /// Returns list of taxon or null if no taxon are found.
        /// </returns>
        TaxonList GetTaxa(IUserContext userContext,
                          ITaxonSearchCriteria searchCriteria);

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonIds">Taxon ids</param>
        /// <returns>Taxa with specified ids.</returns>
        TaxonList GetTaxa(IUserContext userContext, List<Int32> taxonIds);

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonIds">Taxon ids</param>
        /// <returns>Taxa with specified ids.</returns>
        TaxonList GetTaxa(IUserContext userContext, List<TaxonId> taxonIds);

        /// <summary>
        /// Get taxon by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonGuid">GUID.</param>
        /// <returns>Requested taxon.</returns>       
        ITaxon GetTaxon(IUserContext userContext, String taxonGuid);

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>       
        ITaxon GetTaxon(IUserContext userContext, Int32 taxonId);

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>       
        ITaxon GetTaxon(IUserContext userContext, TaxonId taxonId);

        /// <summary>
        /// Get taxon alert status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonAlertStatusId">Id for requested taxon alert status.</param>
        /// <returns>Taxon alert status with specified id.</returns>
        ITaxonAlertStatus GetTaxonAlertStatus(IUserContext userContext,
                                              Int32 taxonAlertStatusId);

        /// <summary>
        /// Get taxon alert status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonAlertStatusId">Id for requested taxon alert status.</param>
        /// <returns>Taxon alert status with specified id.</returns>
        ITaxonAlertStatus GetTaxonAlertStatus(IUserContext userContext,
                                              TaxonAlertStatusId taxonAlertStatusId);

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon alert statuses.</returns>
        TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext);

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon categories.</returns>       
        TaxonCategoryList GetTaxonCategories(IUserContext userContext);

        /// <summary>
        /// Get all taxon categories related to specified taxon.
        /// This includes: 
        /// Taxon categories for parent taxa to specified taxon.
        /// Taxon category for specified taxon.
        /// Taxon categories for child taxa to specified taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>All taxon categories related to specified taxon.</returns>
        TaxonCategoryList GetTaxonCategories(IUserContext userContext,
                                             ITaxon taxon);

        /// <summary>
        /// Get taxon category with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonCategoryId">Id of taxon category to be retrived.</param>
        /// <returns>Taxon category with specified id.</returns>
        ITaxonCategory GetTaxonCategory(IUserContext userContext,
                                        Int32 taxonCategoryId);

        /// <summary>
        /// Get taxon category by specified name
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        ITaxonCategory GetTaxonCategory(IUserContext userContext, string name);

        /// <summary>
        /// Get taxon category with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonCategoryId">Id of taxon category to be retrived.</param>
        /// <returns>Taxon category with specified id.</returns>
        ITaxonCategory GetTaxonCategory(IUserContext userContext,
                                        TaxonCategoryId taxonCategoryId);

        /// <summary>
        /// Get list of changes made regarding taxa.
        /// Current version return changes regarding:
        /// - new taxon
        /// - new/edited taxon name (scientific + common)
        /// - lump/split events
        /// - taxon category changes
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="rootTaxon">A root taxon. Changes made for child taxa to this root taxon are returned.
        /// If parameter is NULL all changes are returned</param>
        /// <param name="dateFrom">Return changes from and including this date.</param>
        /// <param name="dateTo">Return changes to and including this date.</param>
        /// <returns>List of changes made</returns> 
        TaxonChangeList GetTaxonChange(IUserContext userContext,
                                       ITaxon rootTaxon,
                                       DateTime dateFrom,
                                       DateTime dateTo);

        /// <summary>
        /// Get taxon change status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonChangeStatusId">Id for requested taxon change status.</param>
        /// <returns>Taxon change status with specified id.</returns>
        ITaxonChangeStatus GetTaxonChangeStatus(IUserContext userContext,
                                                Int32 taxonChangeStatusId);

        /// <summary>
        /// Get taxon change status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonChangeStatusId">Id for requested taxon change status.</param>
        /// <returns>Taxon change status with specified id.</returns>
        ITaxonChangeStatus GetTaxonChangeStatus(IUserContext userContext,
                                                TaxonChangeStatusId taxonChangeStatusId);

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon change statuses.</returns>
        TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext);

        /// <summary>
        /// Get a list of taxon quality summary. 
        /// </summary>
        /// <param name="userContext">UserContext</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>List of ITaxonQualitySummary with taxa quality summary.</returns>
        TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                       ITaxon taxon);

        /// <summary>
        /// Get a list of taxon statistics. 
        /// </summary>
        /// <param name="userContext">UserContext</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>List of ITaxonStatistics with taxa statistics.</returns>
        TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext,
                                                         ITaxon taxon);

        /// <summary>
        /// Get concept definition for specified taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition for specified taxon.</returns>
        String GetTaxonConceptDefinition(IUserContext userContext,
                                         ITaxon taxon);

        /// <summary>
        /// Get taxonname by Id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameId">Id for TaxonName.</param>
        /// <returns>Requested object.</returns>       
        ITaxonName GetTaxonName(IUserContext userContext,
                                Int32 taxonNameId);

        /// <summary>
        /// Get taxon name with specified GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameGuid">Taxon name GUID.</param>
        /// <returns>Taxon name with specified GUID.</returns>       
        ITaxonName GetTaxonName(IUserContext userContext,
                                String taxonNameGuid);

        /// <summary>
        /// Get a list of all taxon name calegories ie TaxonNameCategoryList class. 
        /// </summary>
        /// <param name="userContext">Usercontext</param>
        /// <returns>TaxonNameCategoryList with all taxon name calegories.</returns>
        TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext);

        /// <summary>
        /// Gets a taxon name category by its id.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonNameCategoryId">Id of taxon name category to be retrived.</param>
        /// <returns></returns>
        ITaxonNameCategory GetTaxonNameCategory(IUserContext userContext,
                                                Int32 taxonNameCategoryId);

        /// <summary>
        /// Get taxon name category type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameCategoryTypeId">Id for requested taxon category type.</param>
        /// <returns>Taxon name category type with specified id.</returns>
        ITaxonNameCategoryType GetTaxonNameCategoryType(IUserContext userContext,
                                                        Int32 taxonNameCategoryTypeId);

        /// <summary>
        /// Get taxon name category type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameCategoryTypeId">Id for requested taxon category type.</param>
        /// <returns>Taxon name category type with specified id.</returns>
        ITaxonNameCategoryType GetTaxonNameCategoryType(IUserContext userContext,
                                                        TaxonNameCategoryTypeId taxonNameCategoryTypeId);

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon name category types.</returns>
        TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext);

        /// <summary>
        /// Gets or sets GetTaxonNamesBySearchCriteria.
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="searchCriteria">
        /// The taxon Name Search Criteria.
        /// </param>
        /// <returns>
        /// The get taxon names by search criteria.
        /// </returns>
        TaxonNameList GetTaxonNames(IUserContext userContext,
                                    ITaxonNameSearchCriteria searchCriteria);

        /// <summary>
        /// Get all taxon names for specified taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>All taxon names for specified taxon.</returns>
        TaxonNameList GetTaxonNames(IUserContext userContext,
                                    ITaxon taxon);

        /// <summary>
        /// Get all taxon names for specified taxa.
        /// The result is sorted in the same order as input taxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxa">Taxa.</param>
        /// <returns>Taxon names.</returns>
        List<TaxonNameList> GetTaxonNames(IUserContext userContext,
                                          TaxonList taxa);

        /// <summary>
        /// Get taxon name status with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameStatusId">Taxon name status id.</param>
        /// <returns>Taxon name status with specified id.</returns>
        ITaxonNameStatus GetTaxonNameStatus(IUserContext userContext,
                                            Int32 taxonNameStatusId);

        /// <summary>
        /// Get taxon name status with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameStatusId">Taxon name status id.</param>
        /// <returns>Taxon name status with specified id.</returns>
        ITaxonNameStatus GetTaxonNameStatus(IUserContext userContext,
                                            TaxonNameStatusId taxonNameStatusId);

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext);


        /// <summary>
        /// Get taxon name usage with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameUsageId">Taxon name usage id.</param>
        /// <returns>Taxon name status with specified id.</returns>
        ITaxonNameUsage GetTaxonNameUsage(IUserContext userContext,
                                            Int32 taxonNameUsageId);

        /// <summary>
        /// Get taxon name usage with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameUsageId">Taxon name usage id.</param>
        /// <returns>Taxon name usage with specified id.</returns>
        ITaxonNameUsage GetTaxonNameUsage(IUserContext userContext,
                                            TaxonNameUsageId taxonNameUsageId);

        /// <summary>
        /// Get information about possible usage for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Information about possible usage for taxon names.</returns>
        TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext);



        /// <summary>
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <returns>
        /// </returns>
        TaxonPropertiesList GetTaxonProperties(IUserContext userContext,
                                               ITaxon taxon);

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        TaxonRelationList GetTaxonRelations(IUserContext userContext,
                                            ITaxonRelationSearchCriteria searchCriteria);

        /// <summary>
        /// Load revision based on Id.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevisionId">
        /// The revision id.
        /// </param>
        /// <returns>
        /// </returns>
        ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                        Int32 taxonRevisionId);

        /// <summary>
        /// Get revision by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionGuid">GUID.</param>
        /// <returns>Requested object.</returns>       
        ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                        String taxonRevisionGuid);

        /// <summary>
        /// Get revision by id.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevisionEventId">
        /// The revision id.
        /// </param>
        /// <returns>
        /// </returns>
        ITaxonRevisionEvent GetTaxonRevisionEvent(IUserContext userContext,
                                                  Int32 taxonRevisionEventId);

        /// <summary>
        /// Get revision event selected ny revision id.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevisionId">Revision id</param>
        /// <returns>Revision event list selected by revision.</returns>
        TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext,
                                                      Int32 taxonRevisionId);

        /// <summary>
        /// Get taxon revision event type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionEventTypeId">Id for taxon revision event type.</param>
        /// <returns>Requested taxon revision event type.</returns>
        ITaxonRevisionEventType GetTaxonRevisionEventType(IUserContext userContext,
                                                          Int32 taxonRevisionEventTypeId);

        /// <summary>
        /// Get taxon revision event type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionEventTypeId">Id for taxon revision event type.</param>
        /// <returns>Requested taxon revision event type.</returns>
        ITaxonRevisionEventType GetTaxonRevisionEventType(IUserContext userContext,
                                                          TaxonRevisionEventTypeId taxonRevisionEventTypeId);

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision event types.</returns>
        TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext);

        /// <summary>
        /// Get revisons by search criteria
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="searchCriteria">Serack criteria to search revisions for</param>
        /// <returns></returns>
        TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                            ITaxonRevisionSearchCriteria searchCriteria);

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A list of revisions.</returns>
        TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                            ITaxon taxon);

        /// <summary>
        /// Get taxon revision state with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionStateId">Id for taxon revision state.</param>
        /// <returns>Requested taxon revision state.</returns>
        ITaxonRevisionState GetTaxonRevisionState(IUserContext userContext,
                                                  Int32 taxonRevisionStateId);

        /// <summary>
        /// Get taxon revision state with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionStateId">Id for taxon revision state.</param>
        /// <returns>Requested taxon revision state.</returns>
        ITaxonRevisionState GetTaxonRevisionState(IUserContext userContext,
                                                  TaxonRevisionStateId taxonRevisionStateId);

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision states.</returns>
        TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext);

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        TaxonTreeNodeList GetTaxonTrees(IUserContext userContext,
                                        ITaxonTreeSearchCriteria searchCriteria);

        /// <summary>
        /// Test if it is ok to lump taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxa">Taxa.</param>
        /// <param name="taxonAfter">Taxon after.</param>
        /// <returns>True, if it is ok to lump taxon.</returns>
        Boolean IsOkToLumpTaxa(IUserContext userContext,
                               TaxonList taxa,
                               ITaxon taxonAfter);

        /// <summary>
        /// Test if it is ok to split taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonBefore">Taxon before.</param>
        /// <param name="taxaAfter">Taxa after.</param>
        /// <returns>True, if it is ok to split taxon.</returns>
        Boolean IsOkToSplitTaxon(IUserContext userContext,
                                 ITaxon taxonBefore,
                                 TaxonList taxaAfter);

        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonBefore">
        /// The taxon before.
        /// </param>
        /// <param name="taxonAfter">
        /// The taxon after.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        void LumpTaxon(IUserContext userContext,
                       TaxonList taxonBefore,
                       ITaxon taxonAfter,
                       ITaxonRevision taxonRevision);

        /// <summary>
        /// Moves a list of taxa from one parent to another. The new relation will be sorted last.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxa">List of taxa that will be mmoved.</param>
        /// <param name="previousParent">The previous parent.</param>
        /// <param name="newParent">The new parent.</param>
        /// <param name="taxonRevision">The revision. </param>
        void MoveTaxa(IUserContext userContext,
                      TaxonList taxa,
                      ITaxon previousParent,
                      ITaxon newParent,
                      ITaxonRevision taxonRevision);

        /// <summary>
        /// Moves taxon from one parent to another. If previosParent is null a new parent is created. The new relation will be sorted last.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="previousParent">The previous parent.</param>
        /// <param name="newParent">The new parent.</param>
        /// <param name="taxonRevision">The revision.</param>
        void MoveTaxon(IUserContext userContext,
                       ITaxon taxon,
                       ITaxon previousParent,
                       ITaxon newParent,
                       ITaxonRevision taxonRevision);

        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonBefore">
        /// The taxon before.
        /// </param>
        /// <param name="taxaAfter">
        /// The taxon after.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        void SplitTaxon(IUserContext userContext,
                        ITaxon taxonBefore,
                        TaxonList taxaAfter,
                        ITaxonRevision taxonRevision);

        /// <summary>
        /// Update taxon category for taxon.
        /// Creates all related changes in the revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="taxonRevision">Revision.</param>
        /// <param name="category">The category.</param>
        void UpdateTaxon(IUserContext userContext,
                         ITaxon taxon,
                         ITaxonRevision taxonRevision,
                         ITaxonCategory category);

        /// <summary>
        /// Update the taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="conceptDefinitionPartString">Taxon description.</param>
        /// <param name="category">Taxon category.</param>
        /// <param name="alertStatus">Alert status.</param>
        /// <param name="isMicrospecies">Is Microspecies</param>
        void UpdateTaxon(IUserContext userContext,
            ITaxon taxon,
            ITaxonRevision taxonRevision,
            String conceptDefinitionPartString,
            ITaxonCategory category,
            TaxonAlertStatusId alertStatus, 
            bool isMicrospecies);

        /// <summary>
        /// Create new taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">The taxon object.</param>
        /// <param name="taxonRevisionEvent">Revision event connected to the change.
        /// Information about the new taxon.
        /// This object is updated with information 
        /// about the created taxon.
        /// </param>
        /// <param name="lumpSplitEvents">Creat lump split events if this parameter is not null.</param>
        /// <param name="taxonRelations">Create taxonrelations if this parameter is not null.</param>
        void UpdateTaxon(IUserContext userContext,
                         ITaxon taxon,
                         ITaxonRevisionEvent taxonRevisionEvent,
                         LumpSplitEventList lumpSplitEvents,
                         IList<ITaxonRelation> taxonRelations);

        /// <summary>
        /// Change a taxon name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">TaxonName object with name to be changed.</param>
        void UpdateTaxonName(IUserContext userContext,
                             ITaxonRevision taxonRevision,
                             ITaxonName taxonName);

        /// <summary>
        /// Change taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonNames">List of TaxonName objects.</param>
        void UpdateTaxonNames(IUserContext userContext,
                              ITaxonRevision taxonRevision,
                              TaxonNameList taxonNames);

        /// <summary>
        /// Creates a new revision
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        void UpdateTaxonRevision(IUserContext userContext,
                                 ITaxonRevision taxonRevision);
        
        /// <summary>
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonIds">
        /// The taxa ids.
        /// </param>
        /// <param name="parentTaxonId">
        /// Parent taxon id.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        void UpdateTaxonTreeSortOrder(IUserContext userContext,
                                      List<Int32> taxonIds,
                                      Int32 parentTaxonId,
                                      ITaxonRevision taxonRevision);
    }
}
