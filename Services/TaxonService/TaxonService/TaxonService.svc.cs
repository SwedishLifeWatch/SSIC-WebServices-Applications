using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Log;
using ArtDatabanken.WebService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.TaxonService.Data;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Wcf;
using DatabaseManager = ArtDatabanken.WebService.TaxonService.Data.DatabaseManager;
using ReferenceManager = ArtDatabanken.WebService.Data.ReferenceManager;
using UserManager = ArtDatabanken.WebService.Data.UserManager;
using WebTaxon = ArtDatabanken.WebService.Data.WebTaxon;
using WebTaxonName = ArtDatabanken.WebService.Data.WebTaxonName;
using TaxonManager = ArtDatabanken.WebService.TaxonService.Data.TaxonManager;

namespace TaxonService
{
    /// <summary>
    /// Implementation of the service that handles taxon information.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class TaxonService : WebServiceBase, ITaxonService
    {   
        /// <summary>
        /// Static constructor.
        /// </summary>
        static TaxonService()
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
        /// Check in a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <returns>Updated taxon revision with revision state = CLOSED</returns>
        public WebTaxonRevision CheckInTaxonRevision(WebClientInformation clientInformation,
                                                     WebTaxonRevision taxonRevision)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.RevisionCheckIn(context, taxonRevision);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Check out a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <returns>Updated taxon revision with revision state = ONGOING</returns>
        public WebTaxonRevision CheckOutTaxonRevision(WebClientInformation clientInformation,
                                                      WebTaxonRevision taxonRevision)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.RevisionCheckOut(context, taxonRevision);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public WebLumpSplitEvent CreateLumpSplitEvent(WebClientInformation clientInformation,
                                                      WebLumpSplitEvent lumpSplitEvent)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateLumpSplitEvent(context, lumpSplitEvent);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Create a new taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxon">Object representing the taxon.</param>
        /// <param name="taxonRevisionEvent">Taxon revision event.</param>
        /// <returns>WebTaxon object with the created taxon.</returns>
        public WebTaxon CreateTaxon(WebClientInformation clientInformation,
                                    WebTaxon taxon,
                                    WebTaxonRevisionEvent taxonRevisionEvent)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateTaxon(context, taxon, taxonRevisionEvent);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Creates a new taxon name.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonName">Object representing the taxon name.</param>
        /// <returns></returns>
        public WebTaxonName CreateTaxonName(WebClientInformation clientInformation, WebTaxonName taxonName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateTaxonName(context, taxonName);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public WebTaxonProperties CreateTaxonProperties(WebClientInformation clientInformation, WebTaxonProperties taxonProperties)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateTaxonProperties(context, taxonProperties);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRelation">
        /// The taxon relation.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public WebTaxonRelation CreateTaxonRelation(WebClientInformation clientInformation, WebTaxonRelation taxonRelation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateTaxonRelation(context, taxonRelation);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public WebTaxonRevision CreateTaxonRevision(WebClientInformation clientInformation,
                                                    WebTaxonRevision taxonRevision)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateRevision(context, taxonRevision);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public WebTaxonRevisionEvent CreateTaxonRevisionEvent(WebClientInformation clientInformation,
                                                              WebTaxonRevisionEvent taxonRevisionEvent)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.CreateRevisionEvent(context, taxonRevisionEvent);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionId">Id for a taxon revision.</param>
        public void DeleteTaxonRevision(WebClientInformation clientInformation,
                                        Int32 taxonRevisionId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    TaxonManager.DeleteRevision(context, taxonRevisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Delete specified taxon revision event.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionEventId">Id for the taxon revision event to be deleted.</param>
        public void DeleteTaxonRevisionEvent(WebClientInformation clientInformation,
                                             Int32 taxonRevisionEventId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    TaxonManager.DeleteTaxonRevisionEvent(context, taxonRevisionEventId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get a lump split event by GUID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="lumpSplitEventGuid">GUID for a lump split event.</param>
        /// <returns>Requested lump split event.</returns>    
        public WebLumpSplitEvent GetLumpSplitEventByGuid(WebClientInformation clientInformation,
                                                         String lumpSplitEventGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetLumpSplitEventByGUID(context, lumpSplitEventGuid);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get lump split event with specified id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="lumpSplitEventId">Id of the lump split event.</param>
        /// <returns>Lump split event with specified id.</returns>
        public WebLumpSplitEvent GetLumpSplitEventById(WebClientInformation clientInformation,
                                                       Int32 lumpSplitEventId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetLumpSplitEventById(context, lumpSplitEventId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get lump split events related to
        /// specified new replacing taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="newReplacingTaxonId">Id of the new replacing taxon.</param>
        /// <returns>Lump split events related to specified new replacing taxon.</returns>
        public List<WebLumpSplitEvent> GetLumpSplitEventsByNewReplacingTaxon(WebClientInformation clientInformation,
                                                                             Int32 newReplacingTaxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetLumpSplitEventsByTaxon(context, newReplacingTaxonId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get lump split events related to specified old replaced taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="oldReplacedTaxonId">Id of the old replaced taxon.</param>
        /// <returns>Lump split events related to specified old replaced taxon.</returns>
        public List<WebLumpSplitEvent> GetLumpSplitEventsByOldReplacedTaxon(WebClientInformation clientInformation,
                                                                            Int32 oldReplacedTaxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetLumpSplitEventsByReplacedTaxon(context, oldReplacedTaxonId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All lump split event types.</returns>
        public List<WebLumpSplitEventType> GetLumpSplitEventTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetLumpSplitEventTypes(context);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary> 
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get information about a reference relation.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="referenceRelationId">Id for reference relation.</param>
        /// <returns>A WebReferenceRelation object.</returns>
        public WebReferenceRelation GetReferenceRelationById(WebClientInformation clientInformation, int referenceRelationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetReferenceRelationById(context, referenceRelationId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get reference relations that are related to specified object.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="relatedObjectGuid">GUID for the related object.</param>
        /// <returns>Reference relations that are related to specified object.</returns>
        public List<WebReferenceRelation> GetReferenceRelationsByRelatedObjectGuid(WebClientInformation clientInformation,
                                                                                   String relatedObjectGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetReferenceRelationsByGuid(context, relatedObjectGuid);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Obsolete method. Use web service ReferenceService instead.
        /// Get all reference relation types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All reference relation types.</returns>
        public List<WebReferenceRelationType> GetReferenceRelationTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetReferenceRelationTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get list of taxon objects  idetified by its id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonIds">List of taxon id.</param>
        /// <returns>
        /// All taxon with id matching the list of id
        /// or NULL if there are no taxon.
        /// </returns>        
        public List<WebTaxon> GetTaxaByIds(WebClientInformation clientInformation,
                                                       List<Int32> taxonIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxaByIds(context, taxonIds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxa that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Taxa search criteria.</param>
        /// <returns>with  matching search criteria</returns>
        public List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation,
                                                      WebTaxonSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxaBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon alert statuses.</returns>
        public List<WebTaxonAlertStatus> GetTaxonAlertStatuses(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonAlertStatuses(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get a taxon by GUID
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonGuid">Taxons GUID.</param>
        /// <returns>
        /// WebTaxon with information about a Taxon
        /// or NULL if the Taxon is not found.
        /// </returns>    
        public WebTaxon GetTaxonByGuid(WebClientInformation clientInformation, String taxonGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonByGUID(context, taxonGuid);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get a taxon by specific ID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonId">Taxon Id.</param>
        /// <returns>
        /// WebTaxon with information about a Taxon
        /// or NULL if the Taxon is not found.
        /// </returns>    
        public WebTaxon GetTaxonById(WebClientInformation clientInformation,
                                     Int32 taxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonById(context, taxonId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon categories.</returns>       
        public List<WebTaxonCategory> GetTaxonCategories(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonCategories(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public List<WebTaxonCategory> GetTaxonCategoriesByTaxonId(WebClientInformation clientInformation,
                                                                  Int32 taxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonCategoriesByTaxonId(context, taxonId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public List<WebTaxonChange> GetTaxonChange(WebClientInformation clientInformation, Int32 rootTaxonId, Boolean isRootTaxonIdSpecified, DateTime dateFrom, DateTime dateTo)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonChange(context, rootTaxonId, isRootTaxonIdSpecified, dateFrom, dateTo);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon change statuses.</returns>
        public List<WebTaxonChangeStatus> GetTaxonChangeStatuses(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonChangeStatuses(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get child quality statistics for specified root taxon.
        /// The specified root taxon is included in the statistics.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="rootTaxonId">Id of the root taxon</param>
        /// <returns>Child quality statistics for specified root taxon.</returns>     
        public List<WebTaxonChildQualityStatistics> GetTaxonChildQualityStatistics(WebClientInformation clientInformation,
                                                                                   Int32 rootTaxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonQualitySummary(context, rootTaxonId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get child statistics for specified root taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="rootTaxonId">Id of the root taxon</param>
        /// <returns>Child statistics for specified root taxon.</returns>     
        public List<WebTaxonChildStatistics> GetTaxonChildStatistics(WebClientInformation clientInformation,
                                                                     Int32 rootTaxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonStatistics(context, rootTaxonId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get concept definition for specified taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition for specified taxon.</returns>
        public String GetTaxonConceptDefinition(WebClientInformation clientInformation,
                                                WebTaxon taxon)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonConceptDefinition(context, taxon);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get a taxonname by GUID
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonNameGuid">Taxons GUID.</param>
        /// <returns>
        /// </returns>    
        public WebTaxonName GetTaxonNameByGuid(WebClientInformation clientInformation, String taxonNameGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameByGUID(context, taxonNameGuid);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon name by id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonNameId">The taxon name id.</param>
        /// <returns>WebTaxonName or NULL if no name is found</returns>
        public WebTaxonName GetTaxonNameById(WebClientInformation clientInformation, Int32 taxonNameId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameById(context, taxonNameId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get list of all taxon name category objects.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>
        /// All taxon name categories in DB
        /// or NULL if there are no taxon name category.
        /// </returns>        
        public List<WebTaxonNameCategory> GetTaxonNameCategories(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameCategories(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon name category types.</returns>
        public List<WebTaxonNameCategoryType> GetTaxonNameCategoryTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameCategoryTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the taxon names by search criteria
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>List of WebTaxonNames or NULL if no names are found</returns>
        public List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebClientInformation clientInformation,
                                                                WebTaxonNameSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNamesBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the taxon names by taxon id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>List of WebTaxonNames or NULL if no names are found</returns>
        public List<WebTaxonName> GetTaxonNamesByTaxonId(WebClientInformation clientInformation, Int32 taxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNamesByTaxonId(context, taxonId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon names for a taxon ids.
        /// The result is sorted in the same order as input taxon ids.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxon names.</returns>
        public List<List<WebTaxonName>> GetTaxonNamesByTaxonIds(WebClientInformation clientInformation,
                                                                List<Int32> taxonIds)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNamesByTaxonIds(context, taxonIds);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public List<WebTaxonNameStatus> GetTaxonNameStatuses(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameStatus(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about possible usage for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possible usage for taxon names.</returns>
        public List<WebTaxonNameUsage> GetTaxonNameUsages(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonNameUsage(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public List<WebTaxonProperties> GetTaxonPropertiesByTaxonId(WebClientInformation clientInformation, int taxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonPropertiesByTaxonId(context, taxonId);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public WebTaxonRelation GetTaxonRelationById(WebClientInformation clientInformation, int taxonRelationId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonRelationById(context, taxonRelationId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        public List<WebTaxonRelation> GetTaxonRelationsBySearchCriteria(WebClientInformation clientInformation,
                                                                        WebTaxonRelationSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonRelationsBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon revision with specified GUID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionGuid">Taxon revision GUID.</param>
        /// <returns>Taxon revision with specified GUID.</returns>    
        public WebTaxonRevision GetTaxonRevisionByGuid(WebClientInformation clientInformation,
                                                       String taxonRevisionGuid)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionByGUID(context, taxonRevisionGuid);
                }

                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon revision with specified id.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionId">Taxon revision id.</param>
        /// <returns>Taxon revision with specified id.</returns>    
        public WebTaxonRevision GetTaxonRevisionById(WebClientInformation clientInformation,
                                                     Int32 taxonRevisionId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionById(context, taxonRevisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon revision event with specified id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEventId">The revision event id.</param>
        /// <returns>Taxon revision event with specified id.</returns>
        public WebTaxonRevisionEvent GetTaxonRevisionEventById(WebClientInformation clientInformation,
                                                               Int32 taxonRevisionEventId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionEventById(context, taxonRevisionEventId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public List<WebTaxonRevisionEvent> GetTaxonRevisionEventsByTaxonRevisionId(WebClientInformation clientInformation,
                                                                                   Int32 taxonRevisionId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionEventsByRevisionId(context, taxonRevisionId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision event types.</returns>
        public List<WebTaxonRevisionEventType> GetTaxonRevisionEventTypes(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonRevisionEventTypes(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get taxon revisons that matches search criteria.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">Revision search criteria.</param>
        /// <returns>Taxon revisons that matches search criteria.</returns>
        public List<WebTaxonRevision> GetTaxonRevisionsBySearchCriteria(WebClientInformation clientInformation,
                                                                        WebTaxonRevisionSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionBySearchCriteria(context, searchCriteria);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>List of web revisions.</returns>
        public List<WebTaxonRevision> GetTaxonRevisionsByTaxonId(WebClientInformation clientInformation,
                                                                 Int32 taxonId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetRevisionsByTaxon(context, taxonId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision states.</returns>
        public List<WebTaxonRevisionState> GetTaxonRevisionStates(WebClientInformation clientInformation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonRevisionStates(context);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public List<WebTaxonTreeNode> GetTaxonTreesBySearchCriteria(WebClientInformation clientInformation,
                                                                    WebTaxonTreeSearchCriteria searchCriteria)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.GetTaxonTreesBySearchCriteria(context, searchCriteria);
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
        public override WebLoginResponse Login(String userName,
                                               String password,
                                               String applicationIdentifier,
                                               Boolean isActivationRequired)
        {
            using (WebServiceContext context = new WebServiceContext(userName, applicationIdentifier))
            {
                try
                {
                    return TaxonManager.Login(context,
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
        /// Updates the name of the taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonName">Object with name of the taxon.</param>
        /// <returns>The updated WebTaxonName object.</returns>
        public WebTaxonName UpdateTaxonName(WebClientInformation clientInformation,
                    WebTaxonName taxonName)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.UpdateTaxonName(context, taxonName);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

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
        public WebTaxonProperties UpdateTaxonProperties(WebClientInformation clientInformation, WebTaxonProperties taxonProperties)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.UpdateTaxonProperties(context, taxonProperties);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }   
        }

        /// <summary>
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRelation">
        /// The taxon relation.
        /// </param>
        /// <returns>
        /// </returns>
        /// <exception cref="Exception">
        /// </exception>
        public WebTaxonRelation UpdateTaxonRelation(WebClientInformation clientInformation, WebTaxonRelation taxonRelation)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.UpdateTaxonRelation(context, taxonRelation);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update taxon revision.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevision">Taxon revision.</param>
        /// <returns>Updated taxon revision.</returns>
        public WebTaxonRevision UpdateTaxonRevision(WebClientInformation clientInformation,
                                                    WebTaxonRevision taxonRevision)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    return TaxonManager.UpdateRevision(context, taxonRevision);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update taxon revision event.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonRevisionEventId">Taxon revision event id.</param>
        public void UpdateTaxonRevisionEvent(WebClientInformation clientInformation,
                                             Int32 taxonRevisionEventId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    TaxonManager.UpdateRevisionEvent(context, taxonRevisionEventId);
                }
                catch (Exception exception)
                {
                    WebServiceData.LogManager.LogError(context, exception);
                    throw;
                }
            }
        }

        /// <summary>
        /// Update sort order for all child taxa with same parent taxon.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="parentTaxonId">Id of the parent taxon.</param>
        /// <param name="childTaxonIds">Sorted list of child taxon ids.</param>
        /// <param name="taxonRevisionEventId">The taxon revision event id.</param>
        public void UpdateTaxonTreeSortOrder(WebClientInformation clientInformation,
                                             Int32 parentTaxonId,
                                             List<Int32> childTaxonIds,
                                             Int32 taxonRevisionEventId)
        {
            using (WebServiceContext context = GetWebServiceContext(clientInformation))
            {
                try
                {
                    TaxonManager.SetTaxonTreeSortOrder(context, parentTaxonId, childTaxonIds, taxonRevisionEventId);
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
