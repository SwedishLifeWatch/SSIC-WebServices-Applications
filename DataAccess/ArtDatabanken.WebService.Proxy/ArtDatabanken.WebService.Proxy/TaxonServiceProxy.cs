using System;
using System.Collections.Generic;
using System.ServiceModel;
using ArtDatabanken.Data;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy.TaxonService;

namespace ArtDatabanken.WebService.Proxy
{
    /// <summary>
    /// Class that manages Taxon service requests.
    /// </summary>
    public class TaxonServiceProxy : WebServiceProxyBase, ITransactionProxy, IWebService
    {
        /// <summary>
        /// Create a TaxonServiceProxy instance.
        /// </summary>
        public TaxonServiceProxy()
            : this(null)
        {
        }

        /// <summary>
        /// Create a TaxonServiceProxy instance.
        /// </summary>
        /// <param name="webServiceAddress">
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Taxon.ArtDatabankenSOA.se/TaxonService.svc.
        /// </param>
        public TaxonServiceProxy(String webServiceAddress)
        {
            WebServiceAddress = webServiceAddress;
            switch (Configuration.InstallationType)
            {
                case InstallationType.ArtportalenTest:
                    WebServiceComputer = WebServiceComputer.ArtportalenTest;
                    break;

                case InstallationType.ServerTest:
                    WebServiceComputer = WebServiceComputer.Moneses;
                    break;

                case InstallationType.LocalTest:
                    WebServiceComputer = WebServiceComputer.LocalTest;
                    break;

                case InstallationType.Production:
                    WebServiceComputer = WebServiceComputer.ArtDatabankenSoa;
                    break;

                case InstallationType.SpeciesFactTest:
                    WebServiceComputer = WebServiceComputer.SpeciesFactTest;
                    break;

                case InstallationType.SystemTest:
                    WebServiceComputer = WebServiceComputer.SystemTest;
                    break;

                case InstallationType.TwoBlueberriesTest:
                    WebServiceComputer = WebServiceComputer.TwoBlueberriesTest;
                    break;

                default:
                    throw new ApplicationException("Not handled installation type " + Configuration.InstallationType);
            }
        }

        /// <summary>
        /// Address to web service without internet protocol (http or https)
        /// and binding protocol (SOAP 1.1, SOAP 1.2 or binary).
        /// For example Taxon.ArtDatabankenSOA.se/TaxonService.svc.
        /// </summary>
        public String WebServiceAddress
        { get; set; }

        /// <summary>
        /// Check in a taxon revision.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <returns>Updated taxon revision with revision state = CLOSED</returns>
        public WebTaxonRevision CheckInTaxonRevision(WebClientInformation clientInformation,
                                                     WebTaxonRevision taxonRevision)
        {
            using (ClientProxy client = new ClientProxy(this, 15))
            {
                return client.Client.CheckInTaxonRevision(clientInformation, taxonRevision);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CheckOutTaxonRevision(clientInformation, taxonRevision);
            }
        }

        /// <summary>
        /// Clear data cache in web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void ClearCache(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.ClearCache(clientInformation);
            }
        }

        /// <summary>
        /// Close a web service client.
        /// </summary>
        /// <param name="client">Web service client.</param>
        protected override void CloseClient(Object client)
        {
            try
            {
                ((ClientBase<ITaxonService>)client).Close();
            }
            catch (Exception)
            {
                try
                {
                    ((ClientBase<ITaxonService>)client).Abort();
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception)
                {
                    // We are only interested in releasing resources.
                }
            }
        }

        /// <summary>
        /// Commit an transaction
        /// </summary>
        /// <param name="clientInformation"></param>
        public void CommitTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.CommitTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Create a web service client.
        /// </summary>
        /// <returns>A web service client.</returns>
        protected override Object CreateClient()
        {
            TaxonServiceClient client; 

            client = new TaxonServiceClient(GetBinding(),
                                            GetEndpointAddress());

            // Increase data size for all methods that  
            // sends or receives a lot of data. 
            IncreaseDataSize("GetLog", client.Endpoint);
            IncreaseDataSize("GetTaxaByIds", client.Endpoint);
            IncreaseDataSize("GetTaxaBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetTaxonNamesBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetTaxonNamesByTaxonIds", client.Endpoint);
            IncreaseDataSize("GetTaxonRelationsBySearchCriteria", client.Endpoint);
            IncreaseDataSize("GetTaxonRevisionEventsByTaxonRevisionId", client.Endpoint);
            IncreaseDataSize("GetTaxonTreesBySearchCriteria", client.Endpoint);

            return client;
        }

        /// <summary>
        /// Create a taxon revision lump split event.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="lumpSplitEvent">Information about the lump split event.</param>
        /// <returns>A taxon revision lump split event.</returns>
        public WebLumpSplitEvent CreateLumpSplitEvent(WebClientInformation clientInformation,
                                                      WebLumpSplitEvent lumpSplitEvent)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateLumpSplitEvent(clientInformation, lumpSplitEvent);
            }
        }

        /// <summary>
        /// Create a new taxon.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxon">Information about the new taxon.</param>
        /// <param name="taxonRevisionEvent">Revision event.</param>
        /// <returns>Object with updated taxon information.</returns>
        public WebTaxon CreateTaxon(WebClientInformation clientInformation,
                                    WebTaxon taxon,
                                    WebTaxonRevisionEvent taxonRevisionEvent    )
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxon(clientInformation, taxon, taxonRevisionEvent);
            }
        }

        /// <summary>
        /// Create a new taxon name.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonName">Information about the new taxon name.</param>
        /// <returns>Object with updated taxon name information.</returns>
        public WebTaxonName CreateTaxonName(WebClientInformation clientInformation,
                                            WebTaxonName taxonName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxonName(clientInformation, taxonName);
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
        public WebTaxonProperties CreateTaxonProperties(WebClientInformation clientInformation,
                                                        WebTaxonProperties taxonProperties)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxonProperties(clientInformation, taxonProperties);
            }
        }

        /// <summary>
        /// Create taxon relation.
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRelation">
        /// The taxon relation.
        /// </param>
        /// <returns>
        /// </returns>
        public WebTaxonRelation CreateTaxonRelation(WebClientInformation clientInformation,
                                                    WebTaxonRelation taxonRelation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxonRelation(clientInformation, taxonRelation);
            }
        }

        /// <summary>
        /// create revision.
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        /// <returns>
        /// </returns>
        public WebTaxonRevision CreateTaxonRevision(WebClientInformation clientInformation,
                                                    WebTaxonRevision taxonRevision)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxonRevision(clientInformation, taxonRevision);
            }
        }

        /// <summary>
        /// Create a taxon revision event.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEvent">Information about the event.</param>
        /// <returns>A taxon revision event.</returns>
        public WebTaxonRevisionEvent CreateTaxonRevisionEvent(WebClientInformation clientInformation,
                                                              WebTaxonRevisionEvent taxonRevisionEvent)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.CreateTaxonRevisionEvent(clientInformation, taxonRevisionEvent);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTaxonRevision(clientInformation, taxonRevisionId);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTaxonRevisionEvent(clientInformation, taxonRevisionEventId);
            }
        }

        /// <summary>
        /// Delete trace information from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void DeleteTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.DeleteTrace(clientInformation);
            }
        }

        /// <summary>
        /// Get concept definition string for specified taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition string for specified taxon.</returns>
        public String GetTaxonConceptDefinition(WebClientInformation clientInformation,
                                                WebTaxon taxon)
        {
            if (taxon.Id == 105572)
            {
                // Species Atomaria affinis/pseudaffinis causes exception in this metod.
                //  2016-03-02.
                // TODO: Remove this code when problem has been fixed.
                return "";
            }

            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonConceptDefinition(clientInformation, taxon);
            }
        }

        /// <summary>
        /// Get entries from the web service log.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="type">Get log entries of this type.</param>
        /// <param name="userName">Get log entries for this user. May be empty.</param>
        /// <param name="rowCount">Maximum number of log entries to get.</param>
        /// <returns> Requested web log entries.</returns>
        public List<WebLogRow> GetLog(WebClientInformation clientInformation,
                                      LogType type,
                                      String userName,
                                      Int32 rowCount)
        {
            using (ClientProxy client = new ClientProxy(this, 2))
            {
                return client.Client.GetLog(clientInformation, type, userName, rowCount);
            }
        }

        /// <summary>
        /// Get taxon lump split event with specified GUID.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="lumpSplitEventGuid">Lump split event GUID.</param>
        /// <returns>Taxon lump split event with specified GUID.</returns>
        public WebLumpSplitEvent GetLumpSplitEventByGuid(WebClientInformation clientInformation,
                                                         String lumpSplitEventGuid)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLumpSplitEventByGuid(clientInformation, lumpSplitEventGuid);
            }
        }

        /// <summary>
        /// Get taxon lump split events for specified taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="newReplacingTaxonId">Taxon id.</param>
        /// <returns>Taxon lump split events for specified taxon.</returns>
        public List<WebLumpSplitEvent> GetLumpSplitEventsByNewReplacingTaxon(WebClientInformation clientInformation,
                                                                             Int32 newReplacingTaxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLumpSplitEventsByNewReplacingTaxon(clientInformation, newReplacingTaxonId);
            }
        }

        /// <summary>
        /// Get taxon lump split events for replaced taxon.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="oldReplacedTaxonId">Replaced taxon id.</param>
        /// <returns>Taxon lump split events for replaced taxon.</returns>
        public List<WebLumpSplitEvent> GetLumpSplitEventsByOldReplacedTaxon(WebClientInformation clientInformation,
                                                                            Int32 oldReplacedTaxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLumpSplitEventsByOldReplacedTaxon(clientInformation, oldReplacedTaxonId);
            }
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All lump split event types.</returns>
        public List<WebLumpSplitEventType> GetLumpSplitEventTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetLumpSplitEventTypes(clientInformation);
            }
        }

        /// <summary>
        /// Get status for this web service.
        /// </summary>
        /// <param name="clientInformation">Information about the client that makes this web service call.</param>
        /// <returns>Status for this web service.</returns>       
        public List<WebResourceStatus> GetStatus(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetStatus(clientInformation);
            }
        }

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxa with specified ids.</returns>      
        public List<WebTaxon> GetTaxaByIds(WebClientInformation clientInformation,
                                           List<Int32> taxonIds)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxaByIds(clientInformation, taxonIds);
            }
        }

        /// <summary>
        /// Gets a list of taxa defined by search critera.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="searchCriteria">Search critera.</param>
        /// <returns></returns>
        public List<WebTaxon> GetTaxaBySearchCriteria(WebClientInformation clientInformation,
                                                      WebTaxonSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxaBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon alert statuses.</returns>
        public List<WebTaxonAlertStatus> GetTaxonAlertStatuses(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonAlertStatuses(clientInformation);
            }
        }

        /// <summary>
        /// Get taxon by GUID.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonGuid">GUID.</param>
        /// <returns>Requested taxon.</returns>       
        public WebTaxon GetTaxonByGuid(WebClientInformation clientInformation,
                                       String taxonGuid)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonByGuid(clientInformation, taxonGuid);
            }
        }

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>         
        public WebTaxon GetTaxonById(WebClientInformation clientInformation,
                                     Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxonById(clientInformation, taxonId);
            }
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon categories.</returns>       
        public List<WebTaxonCategory> GetTaxonCategories(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonCategories(clientInformation);
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
        [OperationContract]
        public List<WebTaxonCategory> GetTaxonCategoriesByTaxonId(WebClientInformation clientInformation,
                                                                  Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonCategoriesByTaxonId(clientInformation, taxonId);
            }
        }

        /// <summary>
        /// Get list of changes made regarding taxa.
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
        /// <param name="changedFrom">Return changes from and including this date.</param>
        /// <param name="changedTo">Return changes to and including this date.</param>
        /// <returns>List of changes made</returns> 
        public List<WebTaxonChange> GetTaxonChange(WebClientInformation clientInformation,
                                                   Int32 rootTaxonId,
                                                   Boolean isRootTaxonIdSpecified,
                                                   DateTime changedFrom,
                                                   DateTime changedTo)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonChange(clientInformation, rootTaxonId, isRootTaxonIdSpecified, changedFrom, changedTo);
            }
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon change statuses.</returns>
        public List<WebTaxonChangeStatus> GetTaxonChangeStatuses(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonChangeStatuses(clientInformation);
            }
        }

        /// <summary>
        /// Get taxon name with specified GUID.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <param name="taxonNameGuid">Taxon name GUID.</param>
        /// <returns>Taxon name with specified GUID.</returns>       
        public WebTaxonName GetTaxonNameByGuid(WebClientInformation clientInformation,
                                               String taxonNameGuid)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameByGuid(clientInformation, taxonNameGuid);
            }
        }

        /// <summary>
        /// Get taxon name by id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonNameId">Taxon name id.</param>
        /// <returns>Requested taxon name category.</returns>       
        public WebTaxonName GetTaxonNameById(WebClientInformation clientInformation,
                                             Int32 taxonNameId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameById(clientInformation, taxonNameId);
            }
        }

        /// <summary>
        /// Gets a list of all taxon name categories in DB.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <returns>List of all taxon name categories.</returns>       
        public List<WebTaxonNameCategory> GetTaxonNameCategories(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameCategories(clientInformation);
            }
        }

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public List<WebTaxonNameStatus> GetTaxonNameStatuses(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameStatuses(clientInformation);
            }
        }

        /// <summary>
        /// Get information about possible name usage for taxon names.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>Information about possible name usage for taxon names.</returns>
        public List<WebTaxonNameUsage> GetTaxonNameUsages(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameUsages(clientInformation);
            }
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="clientInformation">Client information.</param>
        /// <returns>All taxon name category types.</returns>
        public List<WebTaxonNameCategoryType> GetTaxonNameCategoryTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNameCategoryTypes(clientInformation);
            }
        }

        /// <summary>
        ///  Get taxon names that matches the search criteria.
        /// </summary>
        /// <param name="clientInformation"></param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns></returns>
        public List<WebTaxonName> GetTaxonNamesBySearchCriteria(WebClientInformation clientInformation,
                                                                WebTaxonNameSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 3))
            {

                return client.Client.GetTaxonNamesBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Gets the taxon names by taxon id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns>List of WebTaxonNames or NULL if no names are found</returns>
        public List<WebTaxonName> GetTaxonNamesByTaxonId(WebClientInformation clientInformation,
                                                         Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonNamesByTaxonId(clientInformation, taxonId);
            }
        }

        /// <summary>
        /// Get all taxon names for specified taxon ids.
        /// The result is sorted in the same order as input taxon ids.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxon names.</returns>
        public List<List<WebTaxonName>> GetTaxonNamesByTaxonIds(WebClientInformation clientInformation,
                                                                List<Int32> taxonIds)
        {
            using (ClientProxy client = new ClientProxy(this, 20))
            {
                return client.Client.GetTaxonNamesByTaxonIds(clientInformation, taxonIds);
            }
        }

        /// <summary>
        /// Gets the taxon properties by taxon id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonId">The taxon id.</param>
        /// <returns></returns>
        public List<WebTaxonProperties> GetTaxonPropertiesByTaxonId(WebClientInformation clientInformation,
                                                                    Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonPropertiesByTaxonId(clientInformation, taxonId);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonChildQualityStatistics(clientInformation, rootTaxonId);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonChildStatistics(clientInformation, rootTaxonId);
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
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxonRelationsBySearchCriteria(clientInformation, searchCriteria);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionByGuid(clientInformation, taxonRevisionGuid);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionById(clientInformation, taxonRevisionId);
            }
        }

        /// <summary>
        /// Get taxon revision event with specified id.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <param name="taxonRevisionEventId">The taxon revision event id.</param>
        /// <returns>Taxon revision event with specified id.</returns>
        public WebTaxonRevisionEvent GetTaxonRevisionEventById(WebClientInformation clientInformation,
                                                               Int32 taxonRevisionEventId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionEventById(clientInformation, taxonRevisionEventId);
            }
        }

        /// <summary>
        /// Get Revsion event list by revision id.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonRevisionId">Taxon revision event id.</param>
        /// <returns>Requested revision event list.</returns>       
        public List<WebTaxonRevisionEvent> GetTaxonRevisionEventsByTaxonRevisionId(WebClientInformation clientInformation,
                                                                                   Int32 taxonRevisionId)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionEventsByTaxonRevisionId(clientInformation, taxonRevisionId);
            }
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision event types.</returns>
        public List<WebTaxonRevisionEventType> GetTaxonRevisionEventTypes(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionEventTypes(clientInformation);
            }
        }

        /// <summary>
        /// Gets a list of all revisions defined by search critera.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="searchCriteria">Search critera.</param>
        /// <returns></returns>
        public List<WebTaxonRevision> GetTaxonRevisionsBySearchCriteria(WebClientInformation clientInformation,
                                                                        WebTaxonRevisionSearchCriteria searchCriteria)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionsBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonId">Id for taxon.</param>
        /// <returns>List of web revisions.</returns>
        public List<WebTaxonRevision> GetTaxonRevisionsByTaxonId(WebClientInformation clientInformation,
                                                                 Int32 taxonId)
        {
            using (ClientProxy client = new ClientProxy(this, 3))
            {
                return client.Client.GetTaxonRevisionsByTaxonId(clientInformation, taxonId);
            }
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="clientInformation">The client information.</param>
        /// <returns>All taxon revision states.</returns>
        public List<WebTaxonRevisionState> GetTaxonRevisionStates(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.GetTaxonRevisionStates(clientInformation);
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
            using (ClientProxy client = new ClientProxy(this, 5))
            {
                return client.Client.GetTaxonTreesBySearchCriteria(clientInformation, searchCriteria);
            }
        }

        /// <summary>
        /// Get address of currently used web service.
        /// </summary>
        /// <returns>Address of currently used web service.</returns>
        protected override String GetWebServiceAddress()
        {
            if (WebServiceAddress.IsEmpty())
            {
                if (Configuration.InstallationType != InstallationType.LocalTest)
                {
                    WebServiceAddress = WebServiceProxy.UserService.GetSoaWebServiceAddress(ApplicationIdentifier.TaxonService);
                }

                if (WebServiceAddress.IsEmpty())
                {
                    switch (WebServiceComputer)
                    {
                        case WebServiceComputer.ArtDatabankenSoa:
                            WebServiceAddress = Settings.Default.TaxonServiceArtDatabankenSoaAddress;
                            break;
                        case WebServiceComputer.LocalTest:
                            WebServiceAddress = Settings.Default.TaxonServiceLocalAddress;
                            break;
                        case WebServiceComputer.Moneses:
                            WebServiceAddress = Settings.Default.TaxonServiceMonesesAddress;
                            break;
                        default:
                            throw new Exception("Not handled computer in web service " + GetWebServiceName() + " " +
                                                WebServiceComputer);
                    }
                }
            }

            return WebServiceAddress;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userName">User name.</param>
        /// <param name="password">Password.</param>
        /// <param name="applicationIdentifier">
        /// Application identifier.
        /// User authorities for this application is included in
        /// the user context.
        /// </param>
        /// <param name="isActivationRequired">
        /// Flag that indicates if user must be activated
        /// for login to succed.
        /// </param>
        /// <returns>Web login response or null if login failed.</returns>
        public WebLoginResponse Login(String userName,
                                      String password,
                                      String applicationIdentifier,
                                      Boolean isActivationRequired)
        {
            WebServiceProxy.UserService.LoadSoaWebServiceAddresses(userName,
                                                                   password,
                                                                   applicationIdentifier,
                                                                   isActivationRequired);

            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.Login(userName, password, applicationIdentifier, isActivationRequired);
            }
        }

        /// <summary>
        /// Logout user from web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void Logout(WebClientInformation clientInformation)
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 1))
                {
                    client.Client.Logout(clientInformation);
                }
            }
            catch
            {
                // No need to handle errors.
                // Logout is only used to relase
                // resources in the web service.
            }
        }

        /// <summary>
        /// Check if the web service is up and running.
        /// </summary>
        /// <returns>
        /// True = OK.
        /// False = Some kind of problem.
        /// </returns>       
        public Boolean Ping()
        {
            try
            {
                using (ClientProxy client = new ClientProxy(this, 0, 10))
                {
                    return client.Client.Ping();
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Rollback a transaction
        /// </summary>
        /// <param name="clientInformation"></param>
        public void RollbackTransaction(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.RollbackTransaction(clientInformation);
            }
        }

        /// <summary>
        /// Start trace usage of web service for specified user.
        /// If no user is specified then all usage of web service
        /// is traced.
        /// Note: Tracing has negativ impact on web service performance.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="userName">User name.</param>
        public void StartTrace(WebClientInformation clientInformation,
                               String userName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTrace(clientInformation, userName);
            }
        }

        /// <summary>
        /// Start a transaction.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="timeout">
        /// Time before transaction timeouts if has not already finished.
        /// Unit is seconds.
        /// </param>
        public void StartTransaction(WebClientInformation clientInformation,
                                     Int32 timeout)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StartTransaction(clientInformation, timeout);
            }
        }

        /// <summary>
        /// Stop tracing usage of web service.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        public void StopTrace(WebClientInformation clientInformation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.StopTrace(clientInformation);
            }
        }

        /// <summary>
        /// Updates a taxon name.
        /// </summary>
        /// <param name="clientInformation">clientInformation.</param>
        /// <param name="taxonName">Information about the updated taxon name.</param>
        /// <returns>Object with updated taxon name information.</returns>
        public WebTaxonName UpdateTaxonName(WebClientInformation clientInformation,
                                            WebTaxonName taxonName)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateTaxonName(clientInformation, taxonName);
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
        public WebTaxonProperties UpdateTaxonProperties(WebClientInformation clientInformation,
                                                        WebTaxonProperties taxonProperties)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateTaxonProperties(clientInformation, taxonProperties);
            }
        }

        /// <summary>
        /// Update taxonrelation
        /// </summary>
        /// <param name="clientInformation">
        /// The client information.
        /// </param>
        /// <param name="taxonRelation">
        /// The taxon relation.
        /// </param>
        /// <returns>
        /// </returns>
        public WebTaxonRelation UpdateTaxonRelation(WebClientInformation clientInformation,
                                                    WebTaxonRelation taxonRelation)
        {
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateTaxonRelation(clientInformation, taxonRelation);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                return client.Client.UpdateTaxonRevision(clientInformation, taxonRevision);
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
            using (ClientProxy client = new ClientProxy(this, 1))
            {
                client.Client.UpdateTaxonRevisionEvent(clientInformation, taxonRevisionEventId);
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
            using (ClientProxy client = new ClientProxy(this, 3))
            {

                client.Client.UpdateTaxonTreeSortOrder(clientInformation, parentTaxonId, childTaxonIds, taxonRevisionEventId);
            }
        }

        /// <summary>
        /// Private class that encapsulate handling
        /// of web service connections.
        /// </summary>
        private class ClientProxy : IDisposable
        {
            private readonly Int32 _operationTimeout;
            private TaxonServiceClient _client;
            private readonly TaxonServiceProxy _webService;

            /// <summary>
            /// Constructor for the ClientProxy class.
            /// Allocates an instance of the real web service client.
            /// </summary>
            /// <param name="webService">Web service proxy.</param>
            /// <param name="operationTimeoutMinutes">Operation timeout to set in the client. Unit is minutes.</param>
            /// <param name="operationTimeoutSeconds">Operation timeout to set in the client. Unit is seconds.</param>
            public ClientProxy(TaxonServiceProxy webService,
                               Int32 operationTimeoutMinutes,
                               Int32 operationTimeoutSeconds = 0)
            {
                _operationTimeout = (operationTimeoutMinutes * 60) + operationTimeoutSeconds;
                _webService = webService;
                _client = (TaxonServiceClient)(_webService.PopClient(_operationTimeout));
                _webService.SetTimeout(Client.Endpoint.Binding, _operationTimeout);
            }

            /// <summary>
            /// Get the real web service client.
            /// </summary>

            public TaxonServiceClient Client
            {
                get { return _client; }
            }
            
            /// <summary>
            /// Implementation of the IDisposable interface.
            /// Recycle the client instance.
            /// </summary>
            public void Dispose()
            {
                if ((_client.State != CommunicationState.Opened) ||
                    (!_webService.PushClient(_client, _operationTimeout)))
                {
                    // Client is not in state open or
                    // was not added to the client pool.
                    // Release resources.
                    _client.Close();
                }
                _client = null;
            }
        }
    }
}
