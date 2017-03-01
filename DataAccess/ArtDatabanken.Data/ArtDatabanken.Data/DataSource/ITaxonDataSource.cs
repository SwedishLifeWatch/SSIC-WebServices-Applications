using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data.DataSource
{
     /// <summary>
    /// Definition of the TaxonDataSource interface.
    /// This interface is used to retrieve taxon related information.
    /// </summary>
     public interface ITaxonDataSource : IDataSource
     {
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
         /// Create lump split event.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="lumpSplitEvent">
         /// Information about the new lump split event.
         /// This object is updated with latest lump split event information.
         /// </param>    
         void CreateLumpSplitEvent(IUserContext userContext,
                                   ILumpSplitEvent lumpSplitEvent);

         /// <summary>
         /// Create taxon.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxon">
         /// Information about the new taxon.
         /// This object is updated with latest taxon information.
         /// </param>    
         /// <param name="taxonRevisionEvent">The taxon is created in this taxon revision event.</param>
         void CreateTaxon(IUserContext userContext,
                          ITaxon taxon,
                          ITaxonRevisionEvent taxonRevisionEvent);

         /// <summary>
         /// Create new taxon name.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonName"> Information about the new taxon name category.
         /// This object is updated with information 
         /// about the created taxon name.
         /// </param>    
         void CreateTaxonName(IUserContext userContext,
                              ITaxonName taxonName);

         /// <summary>
         /// Create taxon properties.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonProperties">
         /// Information about the new taxon properties.
         /// This object is updated with latest taxon properties information.
         /// </param>    
         void CreateTaxonProperties(IUserContext userContext,
                                    ITaxonProperties taxonProperties);

         /// <summary>
         /// Create taxon relation.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonRelation">
         /// Information about the new taxon relation.
         /// This object is updated with latest taxon relation information.
         /// </param>    
         void CreateTaxonRelation(IUserContext userContext,
                                  ITaxonRelation taxonRelation);

         /// <summary>
         /// Create a revision event
         /// </summary>
         /// <param name="userContext"> The user context.</param>
         /// <param name="taxonRevisionEvent">The revision event object.</param>
         void CreateTaxonRevisionEvent(IUserContext userContext,
                                       ITaxonRevisionEvent taxonRevisionEvent);

         /// <summary>
         /// </summary>
         /// <param name="userContext">
         /// The user context.
         /// </param>
         /// <param name="taxonRevision">
         /// The revision id.
         /// </param>
         void DeleteTaxonRevision(IUserContext userContext,
                                  ITaxonRevision taxonRevision);

         /// <summary>
         /// Rolls back all changes for one revision event.
         /// </summary>
         /// <param name="userContext">User context.</param>
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
         /// <param name="userContext">User context.</param>
         /// <param name="newReplacingTaxon">New replacing taxon.</param>
         /// <returns>
         /// </returns>
         LumpSplitEventList GetLumpSplitEventsByNewReplacingTaxon(IUserContext userContext,
                                                                  ITaxon newReplacingTaxon);

         /// <summary>
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="oldReplacedTaxon">Taxon.</param>
         /// <returns>
         /// </returns>
         LumpSplitEventList GetLumpSplitEventsByOldReplacedTaxon(IUserContext userContext,
                                                                 ITaxon oldReplacedTaxon);

         /// <summary>
         /// Get all lump split event types.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <returns>All lump split event types.</returns>
         LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext);

         /// <summary>
         /// Get list of taxon objects.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonIds">List of taxon ids</param>
         /// <returns>
         /// Returns list of taxon or null if no taxon are found.
         /// </returns>
         TaxonList GetTaxa(IUserContext userContext,
                           List<Int32> taxonIds);

         /// <summary>
         /// Get list of taxon objects matching search criteria.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="searchCriteria">Search criteia for taxon</param>
         /// <returns>
         /// Returns list of taxon or null if no taxon are found.
         /// </returns>
         TaxonList GetTaxa(IUserContext userContext,
                           ITaxonSearchCriteria searchCriteria);

         /// <summary>
         /// Get taxon by id.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonId">Taxon id.</param>
         /// <returns>Requested taxon.</returns>       
         ITaxon GetTaxon(IUserContext userContext, Int32 taxonId);

         /// <summary>
         /// Get taxon by GUID.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonGuid">GUID.</param>
         /// <returns>Requested taxon.</returns>       
         ITaxon GetTaxon(IUserContext userContext, String taxonGuid);

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
         /// Get list of changes made regarding taxa.
         /// Current version return changes regarding:
         /// - new taxon
         /// - new/edited taxon name (scientific + common)
         /// - lump/split events
         /// - taxon category changes
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <param name="rootTaxonId">A root taxon. Changes made for child taxa to this root taxon are returned.
         /// If parameter is NULL all changes are returned</param>
         /// <param name="dateFrom">Return changes from and including this date.</param>
         /// <param name="dateTo">Return changes to and including this date.</param>
         /// <returns>List of changes made</returns> 
         TaxonChangeList GetTaxonChange(IUserContext userContext,
                                        ITaxon rootTaxonId,
                                        DateTime dateFrom,
                                        DateTime dateTo);

         /// <summary>
         /// Get all taxon change statuses.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <returns>All taxon change statuses.</returns>
         TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext);

         /// <summary>
         /// Get taxon quality summary
         /// </summary>
         /// <param name="userContext">User context</param>
         /// <param name="taxon">Taxon.</param>
         /// <returns>Returns list of taxon quality summary objects.
         /// </returns>        
         TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                        ITaxon taxon);

         /// <summary>
         /// Get taxon statistics from database.
         /// </summary>
         /// <param name="userContext">User context</param>
         /// <param name="taxon">Taxon.</param>
         /// <returns>Returns list of taxon statistics objects or null if no taxon statistics exists.
         /// </returns>        
         TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext,
                                                          ITaxon taxon);

         /// <summary>
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxon">Taxon.</param>
         /// <returns>
         /// </returns>
         String GetTaxonConceptDefinition(IUserContext userContext,
                                          ITaxon taxon);

         /// <summary>
         /// Get taxonname by GUID.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonNameGuid">GUID.</param>
         /// <returns>Requested object.</returns>       
         ITaxonName GetTaxonName(IUserContext userContext,
                                 String taxonNameGuid);

         /// <summary>
         /// Get taxon name by taxon name id.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <param name="taxonNameId">The taxon name id.</param>
         /// <returns>TaxonName.</returns>
         ITaxonName GetTaxonName(IUserContext userContext,
                                 Int32 taxonNameId);

         /// <summary>
         /// Get all taxon name categories.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <returns>All existing taxon name categories.</returns>       
         TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext);

         /// <summary>
         /// Get all taxon name category types.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <returns>All taxon name category types.</returns>
         TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext);

         /// <summary>
         /// Gets the taxon names by taxon id.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <param name="taxon">The taxon.</param>
         /// <returns>TaxonNameList with names for the taxon.</returns>
         TaxonNameList GetTaxonNames(IUserContext userContext,
                                     ITaxon taxon);

         /// <summary>
         /// </summary>
         /// <param name="userContext">
         /// The user context.
         /// </param>
         /// <param name="searchCriteria">
         /// The taxon name search criteria.
         /// </param>
         /// <returns>
         /// </returns>
         TaxonNameList GetTaxonNames(IUserContext userContext,
                                     ITaxonNameSearchCriteria searchCriteria);

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
         /// Get information about possbile status for taxon names.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <returns>Information about possbile status for taxon names.</returns>
         TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext);


         /// <summary>
         /// Get information about possible name usage for taxon names.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <returns>Information about possible name usage for taxon names.</returns>
         TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext);

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
         /// Get revision by GUID.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonRevisionGuid">GUID.</param>
         /// <returns>Requested object.</returns>       
         ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                         String taxonRevisionGuid);

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
         /// Get revision event by id.
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
         /// Get revision event by id.
         /// </summary>
         /// <param name="userContext">The user context.</param>
         /// <param name="taxonRevisionId">The revision id to find data for. </param>
         /// <returns>A list of RevisionEvents.</returns>
         TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext,
                                                       Int32 taxonRevisionId);

         /// <summary>
         /// Get all taxon revision event types.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <returns>All taxon revision event types.</returns>
         TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext);

         /// <summary>
         /// Get all revisions that affected a taxon or its childtaxa.
         /// </summary>
         /// <param name="userContext"> The user context.</param>
         /// <param name="taxon">The taxon.</param>
         /// <returns>A list of revisions.</returns>
         TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                             ITaxon taxon);

         /// <summary>
         /// Get revisions by searchcriteria 
         /// </summary>
         /// <param name="userContext">User context</param>
         /// <param name="searchCriteria">Search criteria to seach revisions for.</param>
         /// <returns></returns>
         TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                             ITaxonRevisionSearchCriteria searchCriteria);

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
         /// Saves taxon name.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonNames">List of taxonnames that are to be saved. </param>    
         /// <param name="taxonRevisionEvent">Revision event.</param>
         void SaveTaxonNames(IUserContext userContext,
                             TaxonNameList taxonNames,
                             ITaxonRevisionEvent taxonRevisionEvent);

         /// <summary>
         /// Saves a revision.
         /// </summary>
         /// <param name="userContext">
         /// The user context.
         /// </param>
         /// <param name="taxonRevision">
         /// The revision.
         /// </param>
         void SaveTaxonRevision(IUserContext userContext,
                                ITaxonRevision taxonRevision);

         /// <summary>
         /// Update taxon properties.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonProperties">
         /// Information about the updated taxon properties.
         /// This object is updated with latest taxon properties information.
         /// </param>    
         void UpdateTaxonProperties(IUserContext userContext,
                                    ITaxonProperties taxonProperties);

         /// <summary>
         /// Update taxon relation.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonRelation">
         /// Information about the updated taxon relation.
         /// This object is updated with latest taxon relation information.
         /// </param>    
         void UpdateTaxonRelation(IUserContext userContext,
                                  ITaxonRelation taxonRelation);

         /// <summary>
         /// Update taxon revision event.
         /// </summary>
         /// <param name="userContext">User context.</param>
         /// <param name="taxonRevisionEvent">
         /// Information about the updated taxon revision event.
         /// </param>    
         void UpdateTaxonRevisionEvent(IUserContext userContext,
                                       ITaxonRevisionEvent taxonRevisionEvent);

         /// <summary>
         /// Internal sorting of all taxa w/ same parent taxon.
         /// </summary>
         /// <param name="userContext">User context</param>
         /// <param name="taxonIdParent">Id of the parent taxon.</param>
         /// <param name="taxonIdChildren">Sorted list of taxa ids.</param>
         /// <param name="taxonRevisionEvent">The revision event.</param>
         /// <returns></returns>
         void UpdateTaxonTreeSortOrder(IUserContext userContext,
                                       Int32 taxonIdParent,
                                       List<Int32> taxonIdChildren,
                                       ITaxonRevisionEvent taxonRevisionEvent);

         
     }
}
