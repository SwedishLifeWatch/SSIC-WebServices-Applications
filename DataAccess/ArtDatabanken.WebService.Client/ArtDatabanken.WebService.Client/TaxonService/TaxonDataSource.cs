using System;
using System.Collections;
using System.Collections.Generic;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;
using ArtDatabanken.WebService.Client.UserService;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.Proxy;
using Taxon = ArtDatabanken.Data.Taxon;
using TaxonList = ArtDatabanken.Data.TaxonList;
using TaxonName = ArtDatabanken.Data.TaxonName;
using User = ArtDatabanken.Data.User;

namespace ArtDatabanken.WebService.Client.TaxonService
{
    /// <summary>
    /// This class is used to handle taxon related information.
    /// </summary>
    public class TaxonDataSource : TaxonDataSourceBase, ITaxonDataSource
    {
        private const string IS_MICROSPECIES = "IsMicrospecies";

        /// <summary>
        /// Check in a taxon revision.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">
        /// The taxon revision. This object is updated  
        /// with status changes after revision check in.
        /// </param>
        public virtual void CheckInTaxonRevision(IUserContext userContext,
                                                 ITaxonRevision taxonRevision)
        {
            WebTaxonRevision webTaxonRevision;

            CheckTransaction(userContext);
            taxonRevision.ModifiedBy = userContext.User.Id;
            taxonRevision.ModifiedDate = DateTime.Now;
            webTaxonRevision = WebServiceProxy.TaxonService.CheckInTaxonRevision(GetClientInformation(userContext),
                                                                                 GetTaxonRevision(taxonRevision));
            UpdateTaxonRevision(userContext, taxonRevision, webTaxonRevision);
        }

        /// <summary>
        /// Check out a taxon revision.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevision">
        /// The taxon revision. This object is updated  
        /// with status changes after revision check in.
        /// </param>
        public virtual void CheckOutTaxonRevision(IUserContext userContext,
                                                  ITaxonRevision taxonRevision)
        {
            WebTaxonRevision webRevision;

            CheckTransaction(userContext);
            taxonRevision.ModifiedBy = userContext.User.Id;
            taxonRevision.ModifiedDate = DateTime.Now;
            webRevision = WebServiceProxy.TaxonService.CheckOutTaxonRevision(GetClientInformation(userContext),
                                                                             GetTaxonRevision(taxonRevision));
            UpdateTaxonRevision(userContext, taxonRevision, webRevision);
        }

        /// <summary>
        /// Create lump split event.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="lumpSplitEvent">
        /// Information about the new lump split event.
        /// This object is updated with latest lump split event information.
        /// </param>    
        public virtual void CreateLumpSplitEvent(IUserContext userContext,
                                                 ILumpSplitEvent lumpSplitEvent)
        {
            WebLumpSplitEvent webLumpSplitEvent;

            CheckTransaction(userContext);
            webLumpSplitEvent = WebServiceProxy.TaxonService.CreateLumpSplitEvent(GetClientInformation(userContext),
                                                                                  GetLumpSplitEvent(lumpSplitEvent));
            UpdateLumpSplitEvent(userContext, lumpSplitEvent, webLumpSplitEvent);
        }

        /// <summary>
        /// Create taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">
        /// Information about the new taxon.
        /// This object is updated with latest taxon information.
        /// </param>    
        /// <param name="taxonRevisionEvent">The taxon is created in this taxon revision event.</param>
        public virtual void CreateTaxon(IUserContext userContext,
                                        ITaxon taxon,
                                        ITaxonRevisionEvent taxonRevisionEvent)
        {
            WebTaxon webTaxon;

            CheckTransaction(userContext);
            webTaxon = WebServiceProxy.TaxonService.CreateTaxon(GetClientInformation(userContext),
                                                                GetTaxon(taxon),
                                                                GetTaxonRevisionEvent(taxonRevisionEvent));
            UpdateTaxon(userContext, taxon, webTaxon);
        }

        /// <summary>
        /// Create a taxon name.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonName">
        /// Information about the new taxon name.   
        /// This object is updated with changes after the creation.
        /// </param>
        public virtual void CreateTaxonName(IUserContext userContext,
                                            ITaxonName taxonName)
        {
            WebTaxonName webTaxonName;

            CheckTransaction(userContext);
            webTaxonName = WebServiceProxy.TaxonService.CreateTaxonName(GetClientInformation(userContext),
                                                                        GetTaxonName(taxonName));
            UpdateTaxonName(userContext, taxonName, webTaxonName);
        }

        /// <summary>
        /// Create taxon properties.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonProperties">
        /// Information about the new taxon properties.
        /// This object is updated with latest taxon properties information.
        /// </param>    
        public virtual void CreateTaxonProperties(IUserContext userContext,
                                                  ITaxonProperties taxonProperties)
        {
            WebTaxonProperties webTaxonProperties;

            CheckTransaction(userContext);
            webTaxonProperties = WebServiceProxy.TaxonService.CreateTaxonProperties(GetClientInformation(userContext),
                                                                                    GetTaxonProperties(taxonProperties));
            UpdateTaxonProperties(userContext, taxonProperties, webTaxonProperties);
        }

        /// <summary>
        /// Create taxon relation.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRelation">
        /// Information about the new taxon relation.
        /// This object is updated with latest taxon relation information.
        /// </param>    
        public virtual void CreateTaxonRelation(IUserContext userContext,
                                                ITaxonRelation taxonRelation)
        {
            TaxonList taxa;
            WebTaxonRelation webTaxonRelation;

            CheckTransaction(userContext);
            taxa = new TaxonList();
            taxa.Add(taxonRelation.ChildTaxon);
            taxa.Add(taxonRelation.ParentTaxon);
            webTaxonRelation = WebServiceProxy.TaxonService.CreateTaxonRelation(GetClientInformation(userContext),
                                                                                GetTaxonRelation(taxonRelation));
            UpdateTaxonRelation(userContext, taxonRelation, webTaxonRelation, taxa);
        }

        /// <summary>
        /// Create taxon revision event.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <param name="taxonRevisionEvent">
        /// Information about the new taxon revision event.   
        /// This object is updated with changes after the creation.
        /// </param>
        public virtual void CreateTaxonRevisionEvent(IUserContext userContext,
                                                     ITaxonRevisionEvent taxonRevisionEvent)
        {
            WebTaxonRevisionEvent webRevisionEvent;

            CheckTransaction(userContext);
            webRevisionEvent = WebServiceProxy.TaxonService.CreateTaxonRevisionEvent(GetClientInformation(userContext),
                                                                                     GetTaxonRevisionEvent(taxonRevisionEvent));
            UpdateTaxonRevisionEvent(userContext, taxonRevisionEvent, webRevisionEvent);
        }

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">Taxon revision that should be deleted.</param>
        public virtual void DeleteTaxonRevision(IUserContext userContext,
                                                ITaxonRevision taxonRevision)
        {
            CheckTransaction(userContext);
            WebServiceProxy.TaxonService.DeleteTaxonRevision(GetClientInformation(userContext),
                                                             taxonRevision.Id);
        }

        /// <summary>
        /// Rolls back all changes for one revisionevent
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <exception cref="NotImplementedException">
        /// </exception>
        public virtual void DeleteTaxonRevisionEvent(IUserContext userContext,
                                                     ITaxonRevisionEvent taxonRevisionEvent,
                                                     ITaxonRevision taxonRevision)
        {
            WebTaxonRevision webTaxonRevision;

            CheckTransaction(userContext);
            WebServiceProxy.TaxonService.DeleteTaxonRevisionEvent(GetClientInformation(userContext),
                                                                  taxonRevisionEvent.Id);
            webTaxonRevision = WebServiceProxy.TaxonService.GetTaxonRevisionById(GetClientInformation(userContext),
                                                                            taxonRevision.Id);
            UpdateTaxonRevision(userContext, taxonRevision, webTaxonRevision);
        }

        /// <summary>
        /// Convert a ILumpSplitEvent instance
        /// to a WebLumpSplitEvent instance.
        /// </summary>
        /// <param name="lumpSplitEvent">An ILumpSplitEvent object.</param>
        /// <returns>A WebLumpSplitEvent object.</returns>
        private WebLumpSplitEvent GetLumpSplitEvent(ILumpSplitEvent lumpSplitEvent)
        {
            WebLumpSplitEvent webLumpSplitEvent;

            webLumpSplitEvent = new WebLumpSplitEvent();
            webLumpSplitEvent.CreatedBy = lumpSplitEvent.CreatedBy;
            webLumpSplitEvent.CreatedDate = lumpSplitEvent.CreatedDate;
            webLumpSplitEvent.Description = lumpSplitEvent.Description;
            webLumpSplitEvent.Id = lumpSplitEvent.Id;
            webLumpSplitEvent.TaxonIdAfter = lumpSplitEvent.TaxonAfter.Id;
            webLumpSplitEvent.TaxonIdBefore = lumpSplitEvent.TaxonBefore.Id;
            webLumpSplitEvent.TypeId = lumpSplitEvent.Type.Id;

            webLumpSplitEvent.IsChangedInTaxonRevisionEventIdSpecified = lumpSplitEvent.ChangedInTaxonRevisionEventId.HasValue;
            if (lumpSplitEvent.ChangedInTaxonRevisionEventId.HasValue)
            {
                webLumpSplitEvent.ChangedInTaxonRevisionEventId = lumpSplitEvent.ChangedInTaxonRevisionEventId.Value;
            }

            webLumpSplitEvent.IsReplacedInTaxonRevisionEventIdSpecified = lumpSplitEvent.ReplacedInTaxonRevisionEventId.HasValue;
            if (lumpSplitEvent.ReplacedInTaxonRevisionEventId.HasValue)
            {
                webLumpSplitEvent.ReplacedInTaxonRevisionEventId = lumpSplitEvent.ReplacedInTaxonRevisionEventId.Value;
            }

            return webLumpSplitEvent;
        }

        /// <summary>
        /// Convert a WebLumpSplitEvent instance
        /// to an ILumpSplitEvent instance.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webLumpSplitEvent">A WebLumpSplitEvent object.</param>
        /// <returns>An ILumpSplitEvent object.</returns>
        private ILumpSplitEvent GetLumpSplitEvent(IUserContext userContext,
                                                  WebLumpSplitEvent webLumpSplitEvent)
        {
            ILumpSplitEvent lumpSplitEvent;

            lumpSplitEvent = new LumpSplitEvent();
            UpdateLumpSplitEvent(userContext, lumpSplitEvent, webLumpSplitEvent);
            return lumpSplitEvent;
        }

        /// <summary>
        /// Get lump split event with specified GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="lumpSplitEventGuid">GUID for a lump split event.</param>
        /// <returns>Lump split event with specified GUID.</returns>       
        public virtual ILumpSplitEvent GetLumpSplitEvent(IUserContext userContext,
                                                         String lumpSplitEventGuid)
        {
            WebLumpSplitEvent webLumpSplitEvent;

            CheckTransaction(userContext);
            webLumpSplitEvent = WebServiceProxy.TaxonService.GetLumpSplitEventByGuid(GetClientInformation(userContext),
                                                                                     lumpSplitEventGuid);
            return GetLumpSplitEvent(userContext, webLumpSplitEvent);
        }

        /// <summary>
        /// Convert a list of WebLumpSplitEvent instances
        /// to a LumpSplitEventList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webLumpSplitEvents">List of WebLumpSplitEvent instances.</param>
        /// <returns>Lump split events.</returns>
        private LumpSplitEventList GetLumpSplitEvents(IUserContext userContext,
                                                      List<WebLumpSplitEvent> webLumpSplitEvents)
        {
            LumpSplitEventList lumpSplitEvents;

            lumpSplitEvents = new LumpSplitEventList();
            if (webLumpSplitEvents.IsNotEmpty())
            {
                foreach (WebLumpSplitEvent webLumpSplitEvent in webLumpSplitEvents)
                {
                    lumpSplitEvents.Add(GetLumpSplitEvent(userContext, webLumpSplitEvent));
                }
            }
            return lumpSplitEvents;
        }

        /// <summary>
        /// Get taxon lump split events for specified taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="newReplacingTaxon">Taxon.</param>
        /// <returns>Taxon lump split events for specified taxon.</returns>
        public virtual LumpSplitEventList GetLumpSplitEventsByNewReplacingTaxon(IUserContext userContext,
                                                                                ITaxon newReplacingTaxon)
        {
            List<WebLumpSplitEvent> webLumpSplitEvents;

            CheckTransaction(userContext);
            webLumpSplitEvents = WebServiceProxy.TaxonService.GetLumpSplitEventsByNewReplacingTaxon(GetClientInformation(userContext),
                                                                                                    newReplacingTaxon.Id);
            return GetLumpSplitEvents(userContext, webLumpSplitEvents);
        }

        /// <summary>
        /// Get taxon lump split events for replaced taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="oldReplacedTaxon">Replaced taxon.</param>
        /// <returns>Taxon lump split events for replaced taxon.</returns>
        public virtual LumpSplitEventList GetLumpSplitEventsByOldReplacedTaxon(IUserContext userContext,
                                                                               ITaxon oldReplacedTaxon)
        {
            List<WebLumpSplitEvent> webLumpSplitEvents;

            CheckTransaction(userContext);
            webLumpSplitEvents = WebServiceProxy.TaxonService.GetLumpSplitEventsByOldReplacedTaxon(GetClientInformation(userContext),
                                                                                                   oldReplacedTaxon.Id);
            return GetLumpSplitEvents(userContext, webLumpSplitEvents);
        }

        /// <summary>
        /// Convert a WebLumpSplitEventType instance
        /// to a ILumpSplitEventType instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webLumpSplitEventType">A WebLumpSplitEventType object.</param>
        /// <returns>A ILumpSplitEventType instance.</returns>
        private ILumpSplitEventType GetLumpSplitEventType(IUserContext userContext,
                                                          WebLumpSplitEventType webLumpSplitEventType)
        {
            ILumpSplitEventType lumpSplitEventType;

            lumpSplitEventType = new LumpSplitEventType();
            lumpSplitEventType.DataContext = GetDataContext(userContext);
            lumpSplitEventType.Description = webLumpSplitEventType.Description;
            lumpSplitEventType.Id = webLumpSplitEventType.Id;
            lumpSplitEventType.Identifier = webLumpSplitEventType.Identifier;
            return lumpSplitEventType;
        }

        /// <summary>
        /// Convert a list of WebLumpSplitEventType instances
        /// to a LumpSplitEventTypeList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webLumpSplitEventTypes">List of WebLumpSplitEventType instances.</param>
        /// <returns>Lump split event types.</returns>
        private LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext,
                                                              List<WebLumpSplitEventType> webLumpSplitEventTypes)
        {
            LumpSplitEventTypeList lumpSplitEventTypes;

            lumpSplitEventTypes = new LumpSplitEventTypeList();
            if (webLumpSplitEventTypes.IsNotEmpty())
            {
                foreach (WebLumpSplitEventType webLumpSplitEventType in webLumpSplitEventTypes)
                {
                    lumpSplitEventTypes.Add(GetLumpSplitEventType(userContext, webLumpSplitEventType));
                }
            }
            return lumpSplitEventTypes;
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All lump split event types.</returns>
        public virtual LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext)
        {
            List<WebLumpSplitEventType> webLumpSplitEventTypes;

            CheckTransaction(userContext);
            webLumpSplitEventTypes = WebServiceProxy.TaxonService.GetLumpSplitEventTypes(GetClientInformation(userContext));
            return GetLumpSplitEventTypes(userContext, webLumpSplitEventTypes);
        }


        /// <summary>
        /// Gets the taxon revision identifier from DataFields.
        /// </summary>
        /// <param name="webData">The web data.</param>
        /// <returns>Taxon revision Id or null.</returns>
        private Int32? GetTaxonRevisionId(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified(Settings.Default.WebDataTaxonRevisionId))
            {
                return webData.DataFields.GetInt32(Settings.Default.WebDataTaxonRevisionId);
            }
            
            return null;
        }


        /// <summary>
        /// Get name of person that made the last
        /// modification this piece of data.
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <returns>Name of person that made the last modification this piece of data.</returns>
        private String GetModifiedByPerson(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified(Settings.Default.WebDataModifiedByPerson))
            {
                return webData.DataFields.GetString(Settings.Default.WebDataModifiedByPerson);
            }
            else
            {
                // This will happen when ModifiedByPerson has been
                // replaced by ModifiedBy in TaxonService.
                return null;
            }
        }

        /// <summary>
        /// Get dynamic data property SwedishReproCount for statistics.
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <returns>Value of SwedishReproCount.</returns>
        private Int32 GetSwedishReproCount(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified("SwedishReproCount"))
            {
                return webData.DataFields.GetInt32("SwedishReproCount");
            }
            else
            {
                // This will happen when dynamic prop SwedishPeproCount has been
                // replaced by the correct static prop in TaxonService.
                return Int32.MinValue;
            }
        }

        /// <summary>
        /// Convert a list of WebTaxon instances to a TaxonList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxa">List of WebTaxon instances.</param>
        /// <returns>Taxa.</returns>
        private TaxonList GetTaxa(IUserContext userContext,
                                  List<WebTaxon> webTaxa)
        {
            TaxonList taxa;

            taxa = new TaxonList();
            if (webTaxa.IsNotEmpty())
            {
                for (Int32 index = 0; index < webTaxa.Count; index++)
                {
                    taxa.Add(GetTaxon(userContext, webTaxa[index]));
                }
                //foreach (WebTaxon webTaxon in webTaxa)
                //{
                //    taxa.Add(GetTaxon(userContext, webTaxon));
                //}
            }
            return taxa;
        }

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonIds">Taxon ids.</param>
        /// <returns>Taxa with specified ids.</returns>      
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         List<Int32> taxonIds)
        {
            List<WebTaxon> webTaxa;

            CheckTransaction(userContext);
            webTaxa = WebServiceProxy.TaxonService.GetTaxaByIds(GetClientInformation(userContext),
                                                                taxonIds);
            return GetTaxa(userContext, webTaxa);
        }

        /// <summary>
        /// Get taxa that matches search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchCriteria">The taxon search criteria.</param>
        /// <returns>Taxa that matches search criteria.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         ITaxonSearchCriteria searchCriteria)
        {
            List<WebTaxon> webTaxa;

            CheckTransaction(userContext);
            webTaxa = WebServiceProxy.TaxonService.GetTaxaBySearchCriteria(GetClientInformation(userContext),
                                                                           GetTaxonSearchCriteria(searchCriteria));
            return GetTaxa(userContext, webTaxa);
        }

        /// <summary>
        /// Convert a ITaxon instance to a WebTaxon instance.
        /// </summary>
        /// <param name="taxon">An ITaxon object.</param>
        /// <returns>A WebTaxon object.</returns>
        private WebTaxon GetTaxon(ITaxon taxon)
        {
            WebTaxon webTaxon;

            webTaxon = new WebTaxon();
            if (taxon.AlertStatus.IsNull())
            {
                webTaxon.AlertStatusId = (Int32)(TaxonAlertStatusId.Green);
            }
            else
            {
                webTaxon.AlertStatusId = taxon.AlertStatus.Id;
            }
            webTaxon.Author = taxon.Author;
            webTaxon.CategoryId = taxon.Category.Id;
            if (taxon.ChangeStatus.IsNull())
            {
                webTaxon.ChangeStatusId = (Int32) (TaxonChangeStatusId.Unchanged);
            }
            else
            {
                webTaxon.ChangeStatusId = taxon.ChangeStatus.Id;
            }
            webTaxon.CommonName = taxon.CommonName;
            webTaxon.CreatedBy = taxon.CreatedBy;
            webTaxon.CreatedDate = taxon.CreatedDate;
            webTaxon.Guid = taxon.Guid;
            webTaxon.Id = taxon.Id;
            webTaxon.IsInRevision = taxon.IsInRevision;
            webTaxon.IsPublished = taxon.IsPublished;
            webTaxon.IsValid = taxon.IsValid;
            webTaxon.ModifiedBy = taxon.ModifiedBy;
            SetModifiedByPerson(webTaxon, taxon.ModifiedByPerson);
            webTaxon.ModifiedDate = taxon.ModifiedDate;
            webTaxon.PartOfConceptDefinition = taxon.PartOfConceptDefinition;
            webTaxon.ScientificName = taxon.ScientificName;
            webTaxon.SortOrder = taxon.SortOrder;
            webTaxon.ValidFromDate = taxon.ValidFromDate;
            webTaxon.ValidToDate = taxon.ValidToDate;

            return webTaxon;
        }

        /// <summary>
        /// Convert a WebTaxon instance to an ITaxon instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxon">An WebTaxon object.</param>
        /// <returns>An ITaxon object.</returns>
        private ITaxon GetTaxon(IUserContext userContext,
                                WebTaxon webTaxon)
        {
            ITaxon taxon;

            taxon = null;
            if (webTaxon.IsNotNull())
            {
                taxon = new Taxon();
                UpdateTaxon(userContext, taxon, webTaxon);
            }

            return taxon;
        }

        /// <summary>
        /// Get taxon with specified GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonGuid">Taxon GUID.</param>
        /// <returns>Requested taxon.</returns>
        public virtual ITaxon GetTaxon(IUserContext userContext, String taxonGuid)
        {
            WebTaxon taxon;

            CheckTransaction(userContext);
            taxon = WebServiceProxy.TaxonService.GetTaxonByGuid(GetClientInformation(userContext),
                                                                taxonGuid);
            return GetTaxon(userContext, taxon);
        }

        /// <summary>
        /// Get taxon with specifed Id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>
        public virtual ITaxon GetTaxon(IUserContext userContext,
                                       Int32 taxonId)
        {
            WebTaxon taxon;

            CheckTransaction(userContext);
            taxon = WebServiceProxy.TaxonService.GetTaxonById(GetClientInformation(userContext),
                                                              taxonId);
            return GetTaxon(userContext, taxon);
        }

        /// <summary>
        /// Convert a WebTaxonAlertStatus instance
        /// to a ITaxonAlertStatus instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonAlertStatus">A WebTaxonAlertStatus object.</param>
        /// <returns>A ITaxonAlertStatus instance.</returns>
        private ITaxonAlertStatus GetTaxonAlertStatus(IUserContext userContext,
                                                      WebTaxonAlertStatus webTaxonAlertStatus)
        {
            ITaxonAlertStatus taxonAlertStatus;

            taxonAlertStatus = new TaxonAlertStatus();
            taxonAlertStatus.DataContext = GetDataContext(userContext);
            taxonAlertStatus.Description = webTaxonAlertStatus.Description;
            taxonAlertStatus.Id = webTaxonAlertStatus.Id;
            taxonAlertStatus.Identifier = webTaxonAlertStatus.Identifier;
            return taxonAlertStatus;
        }

        /// <summary>
        /// Convert a list of WebTaxonAlertStatus instances
        /// to a TaxonAlertStatusList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonAlertStatuses">List of WebTaxonAlertStatus instances.</param>
        /// <returns>Taxon alert statuses.</returns>
        private TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext,
                                                           List<WebTaxonAlertStatus> webTaxonAlertStatuses)
        {
            TaxonAlertStatusList taxonAlertStatuses;

            taxonAlertStatuses = new TaxonAlertStatusList();
            if (webTaxonAlertStatuses.IsNotEmpty())
            {
                foreach (WebTaxonAlertStatus webTaxonAlertStatus in webTaxonAlertStatuses)
                {
                    taxonAlertStatuses.Add(GetTaxonAlertStatus(userContext, webTaxonAlertStatus));
                }
            }
            return taxonAlertStatuses;
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon alert statuses.</returns>
        public virtual TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext)
        {
            List<WebTaxonAlertStatus> webTaxonAlertStatuses;

            CheckTransaction(userContext);
            webTaxonAlertStatuses = WebServiceProxy.TaxonService.GetTaxonAlertStatuses(GetClientInformation(userContext));
            return GetTaxonAlertStatuses(userContext, webTaxonAlertStatuses);
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon categories.</returns>       
        public virtual TaxonCategoryList GetTaxonCategories(IUserContext userContext)
        {
            List<WebTaxonCategory> webTaxonCategories;

            CheckTransaction(userContext);
            webTaxonCategories = WebServiceProxy.TaxonService.GetTaxonCategories(GetClientInformation(userContext));
            return GetTaxonCategories(userContext, webTaxonCategories);
        }

        /// <summary>
        /// Convert a list of WebTaxonCategory instances
        /// to a TaxonCategoryList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonCategories">List of WebTaxonCategory instances.</param>
        /// <returns>Taxon categories.</returns>
        private TaxonCategoryList GetTaxonCategories(IUserContext userContext,
                                                     List<WebTaxonCategory> webTaxonCategories)
        {
            TaxonCategoryList taxonCategoryList;

            taxonCategoryList = new TaxonCategoryList(true);
            if (webTaxonCategories.IsNotEmpty())
            {
                foreach (WebTaxonCategory webTaxonCategory in webTaxonCategories)
                {
                    taxonCategoryList.Add(GetTaxonCategory(userContext, webTaxonCategory));
                }
            }
            return taxonCategoryList;
        }

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
        public virtual TaxonCategoryList GetTaxonCategories(IUserContext userContext,
                                                            ITaxon taxon)
        {
            List<WebTaxonCategory> webTaxonCategories;

            CheckTransaction(userContext);
            webTaxonCategories = WebServiceProxy.TaxonService.GetTaxonCategoriesByTaxonId(GetClientInformation(userContext),
                                                                                          taxon.Id);
            return GetTaxonCategories(userContext, webTaxonCategories);
        }

        /// <summary>
        /// Get taxon category from web taxon category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonCategory">A web taxon category.</param>
        /// <returns>A taxon category.</returns>
        private ITaxonCategory GetTaxonCategory(IUserContext userContext,
                                                WebTaxonCategory webTaxonCategory)
        {
            ITaxonCategory taxonCategory;

            taxonCategory = new TaxonCategory();
            taxonCategory.DataContext = GetDataContext(userContext);
            taxonCategory.Id = webTaxonCategory.Id;
            taxonCategory.IsMainCategory = webTaxonCategory.IsMainCategory;
            taxonCategory.IsTaxonomic = webTaxonCategory.IsTaxonomic;
            taxonCategory.Name = webTaxonCategory.Name;
            taxonCategory.ParentId = webTaxonCategory.ParentId;
            taxonCategory.SortOrder = webTaxonCategory.SortOrder;
            return taxonCategory;
        }

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
        public virtual TaxonChangeList GetTaxonChange(IUserContext userContext,
                                                      ITaxon rootTaxon,
                                                      DateTime dateFrom,
                                                      DateTime dateTo)
        {
            Boolean isRootTaxonIdSpecified;
            Int32 rootTaxonId;

            CheckTransaction(userContext);
            isRootTaxonIdSpecified = rootTaxon.IsNotNull();
            if (isRootTaxonIdSpecified)
            {
                rootTaxonId = rootTaxon.Id;
            }
            else
            {
                rootTaxonId = 0;
            }
            List<WebTaxonChange> webTaxonChanges = WebServiceProxy.TaxonService.GetTaxonChange(GetClientInformation(userContext),
                                                                                               rootTaxonId,
                                                                                               isRootTaxonIdSpecified,
                                                                                               dateFrom,
                                                                                               dateTo);
            return GetTaxonChange(userContext, webTaxonChanges);
        }

        /// <summary>
        /// Get a list of TaxonChange objects.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonChanges">List of WebTaxonChange objects</param>
        /// <returns>List of TaxonChange objects</returns>
        private TaxonChangeList GetTaxonChange(IUserContext userContext, List<WebTaxonChange> webTaxonChanges)
        {
            TaxonChangeList taxonChanges = new TaxonChangeList();
            if (webTaxonChanges.IsNotEmpty())
            {
                foreach (WebTaxonChange webTaxonChange in webTaxonChanges)
                {
                    taxonChanges.Add(GetTaxonChange(userContext, webTaxonChange));
                }
            }
            return taxonChanges;
        }

        /// <summary>
        /// Get TaxonChange object from WebTaxonChange object
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonChange">WebTaxonChange object.</param>
        /// <returns>TaxonChange object.</returns>
        private ITaxonChange GetTaxonChange(IUserContext userContext,
                                            WebTaxonChange webTaxonChange)
        {
            ITaxonChange taxonChange = new TaxonChange();
            UpdateTaxonChange(userContext, taxonChange, webTaxonChange);
            return taxonChange;
        }

        /// <summary>
        /// Convert a WebTaxonChangeStatus instance
        /// to a ITaxonChangeStatus instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonChangeStatus">A WebTaxonChangeStatus object.</param>
        /// <returns>A ITaxonChangeStatus instance.</returns>
        private ITaxonChangeStatus GetTaxonChangeStatus(IUserContext userContext,
                                                        WebTaxonChangeStatus webTaxonChangeStatus)
        {
            ITaxonChangeStatus taxonChangeStatus;

            taxonChangeStatus = new TaxonChangeStatus();
            taxonChangeStatus.DataContext = GetDataContext(userContext);
            taxonChangeStatus.Description = webTaxonChangeStatus.Description;
            taxonChangeStatus.Id = webTaxonChangeStatus.Id;
            taxonChangeStatus.Identifier = webTaxonChangeStatus.Identifier;
            return taxonChangeStatus;
        }

        /// <summary>
        /// Convert a list of WebTaxonChangeStatus instances
        /// to a TaxonChangeStatusList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonChangeStatuses">List of WebTaxonChangeStatus instances.</param>
        /// <returns>Taxon change statuses.</returns>
        private TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext,
                                                             List<WebTaxonChangeStatus> webTaxonChangeStatuses)
        {
            TaxonChangeStatusList taxonChangeStatuses;

            taxonChangeStatuses = new TaxonChangeStatusList();
            if (webTaxonChangeStatuses.IsNotEmpty())
            {
                foreach (WebTaxonChangeStatus webTaxonChangeStatus in webTaxonChangeStatuses)
                {
                    taxonChangeStatuses.Add(GetTaxonChangeStatus(userContext, webTaxonChangeStatus));
                }
            }
            return taxonChangeStatuses;
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon change statuses.</returns>
        public virtual TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext)
        {
            List<WebTaxonChangeStatus> webTaxonChangeStatuses;

            CheckTransaction(userContext);
            webTaxonChangeStatuses = WebServiceProxy.TaxonService.GetTaxonChangeStatuses(GetClientInformation(userContext));
            return GetTaxonChangeStatuses(userContext, webTaxonChangeStatuses);
        }

        /// <summary>
        /// Get taxon quality summary
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Returns list of taxon quality summary objects.
        /// </returns>        
        public virtual TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                                      ITaxon taxon)
        {
            List<WebTaxonChildQualityStatistics> webTaxonQualitySummary;

            CheckTransaction(userContext);
            webTaxonQualitySummary = WebServiceProxy.TaxonService.GetTaxonChildQualityStatistics(GetClientInformation(userContext),
                                                                                                 taxon.Id);
            return GetTaxonChildQualityStatistics(userContext, webTaxonQualitySummary, taxon);
        }

        /// <summary>
        /// Get a list of TaxonQualitySummary objects.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonQualitySummaryList">List of WebTaxonQualitySummary objects.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>List of TaxonQualitySummary objects.</returns>
        private TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                               List<WebTaxonChildQualityStatistics> webTaxonQualitySummaryList,
                                                                               ITaxon taxon)
        {
            TaxonChildQualityStatisticsList taxonQualitySummaryList;

            taxonQualitySummaryList = new TaxonChildQualityStatisticsList();
            if (webTaxonQualitySummaryList.IsNotEmpty())
            {
                foreach (WebTaxonChildQualityStatistics webTaxonQualitySummary in webTaxonQualitySummaryList)
                {
                    taxonQualitySummaryList.Add(GetTaxonChildQualityStatistics(userContext, webTaxonQualitySummary, taxon));
                }
            }
            return taxonQualitySummaryList;
        }

        /// <summary>
        /// Get TaxonQualitySummary object from WebTaxonQualitySummary object
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonQualitySummary">WebTaxonQualitySummary object.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>TaxonQualitySummary object.</returns>
        public virtual ITaxonChildQualityStatistics GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                                   WebTaxonChildQualityStatistics webTaxonQualitySummary,
                                                                                   ITaxon taxon)
        {
            ITaxonChildQualityStatistics taxonChildQualityStatistics;

            taxonChildQualityStatistics = new TaxonChildQualityStatistics();
            taxonChildQualityStatistics.ChildTaxaCount = webTaxonQualitySummary.ChildTaxaCount;
            taxonChildQualityStatistics.DataContext = GetDataContext(userContext);
            taxonChildQualityStatistics.QualityId = webTaxonQualitySummary.QualityId;
            taxonChildQualityStatistics.RootTaxon = taxon;
            return taxonChildQualityStatistics;
        }

        /// <summary>
        /// Get taxon statistics from database.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Returns list of taxon statistics objects or null if no taxon statistics exists.
        /// </returns>        
        public virtual TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext,
                                                                        ITaxon taxon)
        {
            List<WebTaxonChildStatistics> webTaxonChildStatistics;

            CheckTransaction(userContext);
            webTaxonChildStatistics = WebServiceProxy.TaxonService.GetTaxonChildStatistics(GetClientInformation(userContext),
                                                                                           taxon.Id);
            return GetTaxonChildStatistics(userContext, taxon, webTaxonChildStatistics);
        }

        /// <summary>
        /// Get a list of TaxonStatistics objects.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="webTaxonStatisticsList">List of WebTaxonStatistics</param>
        /// <returns>List of TaxonStatistics objects</returns>
        private TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext,
                                                                 ITaxon taxon,
                                                                 List<WebTaxonChildStatistics> webTaxonStatisticsList)
        {
            TaxonChildStatisticsList taxonChildStatistics;

            taxonChildStatistics = new TaxonChildStatisticsList();
            if (webTaxonStatisticsList.IsNotEmpty())
            {
                foreach (WebTaxonChildStatistics webTaxonStatistic in webTaxonStatisticsList)
                {
                    taxonChildStatistics.Add(GetTaxonChildStatistic(userContext, taxon, webTaxonStatistic));
                }
            }
            return taxonChildStatistics;
        }

        /// <summary>
        /// Get ITaxonChildStatistics object
        /// from WebTaxonChildStatistic object
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="webTaxonChildStatistics">WebTaxonChildStatistics object.</param>
        /// <returns>ITaxonChildStatistics object.</returns>
        private ITaxonChildStatistics GetTaxonChildStatistic(IUserContext userContext,
                                                             ITaxon taxon,
                                                             WebTaxonChildStatistics webTaxonChildStatistics)
        {
            ITaxonChildStatistics taxonChildStatistics;

            taxonChildStatistics = new TaxonChildStatistics();
            taxonChildStatistics.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxonChildStatistics.CategoryId);
            taxonChildStatistics.ChildTaxaCount = webTaxonChildStatistics.ChildTaxaCount;
            taxonChildStatistics.DataContext = GetDataContext(userContext);
            taxonChildStatistics.RootTaxon = taxon;
            taxonChildStatistics.SwedishChildTaxaCount = webTaxonChildStatistics.SwedishChildTaxaCount;
            taxonChildStatistics.SwedishReproCount = this.GetSwedishReproCount(webTaxonChildStatistics);
            return taxonChildStatistics;
        }

        /// <summary>
        /// Get concept definition for specified taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition string for specified taxon.</returns>
        public virtual String GetTaxonConceptDefinition(IUserContext userContext,
                                                        ITaxon taxon)
        {
            CheckTransaction(userContext);
            return WebServiceProxy.TaxonService.GetTaxonConceptDefinition(GetClientInformation(userContext),
                                                                          GetTaxon(taxon));
        }

        /// <summary>
        /// Convert a ITaxonName instance to a WebTaxonName instance.
        /// </summary>
        /// <param name="taxonName">A taxonName object.</param>
        /// <returns>A WebTaxonName instance.</returns>
        private WebTaxonName GetTaxonName(ITaxonName taxonName)
        {
            WebTaxonName webTaxonName;

            webTaxonName = new WebTaxonName();
            webTaxonName.Author = taxonName.Author;
            webTaxonName.CategoryId = taxonName.Category.Id;
            if (taxonName.ChangedInTaxonRevisionEventId.HasValue)
            {
                webTaxonName.ChangedInTaxonRevisionEventId = taxonName.ChangedInTaxonRevisionEventId.Value;
            }
            webTaxonName.CreatedBy = taxonName.CreatedBy;
            webTaxonName.CreatedDate = taxonName.CreatedDate;
            webTaxonName.Description = taxonName.Description;
            webTaxonName.Guid = taxonName.Guid;
            webTaxonName.Id = taxonName.Id;
            webTaxonName.IsChangedInTaxonRevisionEventIdSpecified = taxonName.ChangedInTaxonRevisionEventId.HasValue;
            webTaxonName.IsOkForSpeciesObservation = taxonName.IsOkForSpeciesObservation;
            webTaxonName.IsOriginalName = taxonName.IsOriginalName;
            webTaxonName.IsPublished = taxonName.IsPublished;
            webTaxonName.IsRecommended = taxonName.IsRecommended;
            webTaxonName.IsReplacedInTaxonRevisionEventIdSpecified = taxonName.ReplacedInTaxonRevisionEventId.HasValue;
            webTaxonName.IsUnique = taxonName.IsUnique;
            webTaxonName.ModifiedBy = taxonName.ModifiedBy;
            SetModifiedByPerson(webTaxonName, taxonName.ModifiedByPerson);
            SetNameUsageId(webTaxonName, taxonName.NameUsage.Id);
            webTaxonName.ModifiedDate = taxonName.ModifiedDate;
            webTaxonName.Name = taxonName.Name;
            if (taxonName.ReplacedInTaxonRevisionEventId.HasValue)
            {
                webTaxonName.ReplacedInTaxonRevisionEventId = taxonName.ReplacedInTaxonRevisionEventId.Value;
            }
            webTaxonName.StatusId = taxonName.Status.Id;
            webTaxonName.ValidFromDate = taxonName.ValidFromDate;
            webTaxonName.ValidToDate = taxonName.ValidToDate;
            webTaxonName.Taxon = GetTaxon(taxonName.Taxon);
            SetTaxonNameVersion(webTaxonName, taxonName.Version);
            return webTaxonName;
        }

        /// <summary>
        /// Convert a WebTaxonName instance to a ITaxonName instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonName">A WebTaxonName object.</param>
        /// <returns>A ITaxonName instance.</returns>
        private ITaxonName GetTaxonName(IUserContext userContext,
                                        WebTaxonName webTaxonName)
        {
            ITaxonName taxonName;

            taxonName = new TaxonName();
            UpdateTaxonName(userContext, taxonName, webTaxonName);
            return taxonName;
        }

        /// <summary>
        /// Get taxon name with specified GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameGuid">Taxon name GUID.</param>
        /// <returns>Taxon name with specified GUID.</returns>       
        public virtual ITaxonName GetTaxonName(IUserContext userContext,
                                               String taxonNameGuid)
        {
            WebTaxonName webTaxonName;

            CheckTransaction(userContext);
            webTaxonName = WebServiceProxy.TaxonService.GetTaxonNameByGuid(GetClientInformation(userContext),
                                                                           taxonNameGuid);
            return GetTaxonName(userContext, webTaxonName);
        }

        /// <summary>
        /// Get taxon name with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameId">Taxon name id.</param>
        /// <returns>Taxon name with specified id.</returns>
        public virtual ITaxonName GetTaxonName(IUserContext userContext,
                                               Int32 taxonNameId)
        {
            WebTaxonName taxonName;

            CheckTransaction(userContext);
            taxonName = WebServiceProxy.TaxonService.GetTaxonNameById(GetClientInformation(userContext),
                                                                      taxonNameId);
            return GetTaxonName(userContext, taxonName);
        }

        /// <summary>
        /// Convert a list of WebTaxonNameCategory instances
        /// to a TaxonNameCategoryList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonNameCategories">List of WebTaxonNameCategory instances.</param>
        /// <returns>Taxon name categories.</returns>
        private TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext,
                                                             List<WebTaxonNameCategory> webTaxonNameCategories)
        {
            TaxonNameCategoryList taxonNameCategoryList;

            taxonNameCategoryList = new TaxonNameCategoryList(true);
            if (webTaxonNameCategories.IsNotEmpty())
            {
                foreach (WebTaxonNameCategory webTaxonNameCategory in webTaxonNameCategories)
                {
                    taxonNameCategoryList.Add(GetTaxonNameCategory(userContext, webTaxonNameCategory));
                }
            }
            return taxonNameCategoryList;
        }

        /// <summary>
        /// Get all taxon name categories. 
        /// </summary>
        /// <param name="userContext">Usercontext</param>
        /// <returns>All taxon name categories.</returns>
        public virtual TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext)
        {
            List<WebTaxonNameCategory> webTaxonNameCategories;

            CheckTransaction(userContext);
            webTaxonNameCategories = WebServiceProxy.TaxonService.GetTaxonNameCategories(GetClientInformation(userContext));
            return GetTaxonNameCategories(userContext, webTaxonNameCategories);
        }

        /// <summary>
        /// Get taxon name category from web taxon name category.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameCategory">A web taxon name category.</param>
        /// <returns>A taxon name category.</returns>
        private ITaxonNameCategory GetTaxonNameCategory(IUserContext userContext,
                                                        WebTaxonNameCategory webTaxonNameCategory)
        {
            ITaxonNameCategory taxonNameCategory;

            taxonNameCategory = new TaxonNameCategory();
            taxonNameCategory.DataContext = GetDataContext(userContext);
            taxonNameCategory.Id = webTaxonNameCategory.Id;
            if (webTaxonNameCategory.IsLocaleIdSpecified)
            {
                taxonNameCategory.Locale = CoreData.LocaleManager.GetLocale(userContext, webTaxonNameCategory.LocaleId);
            }
            else
            {
                taxonNameCategory.Locale = null;
            }
            taxonNameCategory.Name = webTaxonNameCategory.Name;
            taxonNameCategory.ShortName = webTaxonNameCategory.ShortName;
            taxonNameCategory.SortOrder = webTaxonNameCategory.SortOrder; 
            taxonNameCategory.Type = CoreData.TaxonManager.GetTaxonNameCategoryType(userContext, webTaxonNameCategory.TypeId);
            return taxonNameCategory;
        }

        /// <summary>
        /// Convert a WebTaxonNameCategoryType instance
        /// to a ITaxonNameCategoryType instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameCategoryType">A WebTaxonNameCategoryType object.</param>
        /// <returns>A ITaxonNameCategoryType instance.</returns>
        private ITaxonNameCategoryType GetTaxonNameCategoryType(IUserContext userContext,
                                                                WebTaxonNameCategoryType webTaxonNameCategoryType)
        {
            ITaxonNameCategoryType taxonNameCategoryType;

            taxonNameCategoryType = new TaxonNameCategoryType();
            taxonNameCategoryType.DataContext = GetDataContext(userContext);
            taxonNameCategoryType.Description = webTaxonNameCategoryType.Description;
            taxonNameCategoryType.Id = webTaxonNameCategoryType.Id;
            taxonNameCategoryType.Identifier = webTaxonNameCategoryType.Identifier;
            return taxonNameCategoryType;
        }

        /// <summary>
        /// Convert a list of WebTaxonNameCategoryType instances
        /// to a TaxonNameCategoryTypeList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonNameCategoryTypes">List of WebTaxonNameCategoryTypes instances.</param>
        /// <returns>Taxon name category types.</returns>
        private TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext,
                                                                    List<WebTaxonNameCategoryType> webTaxonNameCategoryTypes)
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes;

            taxonNameCategoryTypes = new TaxonNameCategoryTypeList();
            if (webTaxonNameCategoryTypes.IsNotEmpty())
            {
                foreach (WebTaxonNameCategoryType webTaxonNameCategoryType in webTaxonNameCategoryTypes)
                {
                    taxonNameCategoryTypes.Add(GetTaxonNameCategoryType(userContext, webTaxonNameCategoryType));
                }
            }
            return taxonNameCategoryTypes;
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon name category types.</returns>
        public virtual TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext)
        {
            List<WebTaxonNameCategoryType> webTaxonNameCategoryTypes;

            CheckTransaction(userContext);
            webTaxonNameCategoryTypes = WebServiceProxy.TaxonService.GetTaxonNameCategoryTypes(GetClientInformation(userContext));
            return GetTaxonNameCategoryTypes(userContext, webTaxonNameCategoryTypes);
        }

        /// <summary>
        /// Get TaxonNames from list of WebTaxonName
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNames">List of WebTaxonName.</param>
        /// <returns>TaxonNameList.</returns>
        private TaxonNameList GetTaxonNames(IUserContext userContext,
                                            List<WebTaxonName> webTaxonNames)
        {
            TaxonNameList taxonNames = new TaxonNameList();
            if (webTaxonNames.IsNotEmpty())
            {
                foreach (WebTaxonName webTaxonName in webTaxonNames)
                {
                    taxonNames.Add(GetTaxonName(userContext, webTaxonName));
                }
            }
            return taxonNames;
        }

        /// <summary>
        /// Get taxon names that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Taxon name search criteria.</param>
        /// <returns>Taxon names that matches search criteria.</returns>
        public virtual TaxonNameList GetTaxonNames(IUserContext userContext,
                                                   ITaxonNameSearchCriteria searchCriteria)
        {
            List<WebTaxonName> webTaxonNames;

            CheckTransaction(userContext);
            webTaxonNames = WebServiceProxy.TaxonService.GetTaxonNamesBySearchCriteria(GetClientInformation(userContext),
                                                                                       GetTaxonNameSearchCriteria(searchCriteria));
            return GetTaxonNames(userContext, webTaxonNames);
        }

        /// <summary>
        /// Get all taxon names for specified taxa.
        /// The result is sorted in the same order as input taxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxa">Taxa.</param>
        /// <returns>Taxon names.</returns>
        public virtual List<TaxonNameList> GetTaxonNames(IUserContext userContext,
                                                         TaxonList taxa)
        {
            Int32 taxonIndex;
            List<List<WebTaxonName>> allWebTaxonNames;
            List<TaxonNameList> allTaxonNames;
            TaxonList tempTaxa;
            TaxonNameList taxonNames;

            CheckTransaction(userContext);

            // Get taxon names.
            allTaxonNames = new List<TaxonNameList>();

            if (taxa.IsNotEmpty())
            {
                if (taxa.Count > Settings.Default.GetTaxonNamesByTaxaLimit)
                {
                    // Problems with a lot of data.
                    // Divide retrieval of taxon names into smaller parts.
                    taxonIndex = 0;
                    tempTaxa = new TaxonList();
                    foreach (ITaxon tempTaxon in taxa)
                    {
                        taxonIndex++;
                        tempTaxa.Add(tempTaxon);
                        if (taxonIndex >= Settings.Default.GetTaxonNamesByTaxaLimit)
                        {
                            allTaxonNames.AddRange(GetTaxonNames(userContext, tempTaxa));
                            taxonIndex = 0;
                            tempTaxa = new TaxonList();
                        }
                    }
                    if (taxonIndex > 0)
                    {
                        allTaxonNames.AddRange(GetTaxonNames(userContext, tempTaxa));
                    }
                }
                else
                {
                    allWebTaxonNames = WebServiceProxy.TaxonService.GetTaxonNamesByTaxonIds(GetClientInformation(userContext),
                                                                                            taxa.GetIds());
                    foreach (List<WebTaxonName> webTaxonNames in allWebTaxonNames)
                    {
                        taxonNames = GetTaxonNames(userContext, webTaxonNames);
                        allTaxonNames.Add(taxonNames);
                    }
                    return allTaxonNames;
                }
            }
            return allTaxonNames;
        }

        /// <summary>
        /// Get all taxon names for specified taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>All taxon names for specified taxon.</returns>
        public virtual TaxonNameList GetTaxonNames(IUserContext userContext,
                                                   ITaxon taxon)
        {
            List<WebTaxonName> taxonNames;

            CheckTransaction(userContext);
            taxonNames = WebServiceProxy.TaxonService.GetTaxonNamesByTaxonId(GetClientInformation(userContext),
                                                                             taxon.Id);
            return GetTaxonNames(userContext, taxonNames);
        }

        /// <summary>
        /// Get web taxon name search criteria.
        /// </summary>
        /// <param name="searchCriteria">Taxon name search criteria.</param>
        /// <returns>Web taxon name search criteria.</returns>
        private WebTaxonNameSearchCriteria GetTaxonNameSearchCriteria(ITaxonNameSearchCriteria searchCriteria)
        {
            WebTaxonNameSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebTaxonNameSearchCriteria();
            if (searchCriteria.AuthorSearchString.IsNotNull())
            {
                webSearchCriteria.AuthorSearchString = new WebStringSearchCriteria();
                webSearchCriteria.AuthorSearchString.SearchString = searchCriteria.AuthorSearchString.SearchString;
            }
            webSearchCriteria.IsCategoryIdSpecified = searchCriteria.Category.IsNotNull();
            if (searchCriteria.Category.IsNotNull())
            {
                webSearchCriteria.CategoryId = searchCriteria.Category.Id;
            }
            webSearchCriteria.IsAuthorIncludedInNameSearchString = searchCriteria.IsAuthorIncludedInNameSearchString;
            webSearchCriteria.IsIsOkForSpeciesObservationSpecified = searchCriteria.IsOkForSpeciesObservation.HasValue;
            if (searchCriteria.IsOkForSpeciesObservation.HasValue)
            {
                webSearchCriteria.IsOkForSpeciesObservation = searchCriteria.IsOkForSpeciesObservation.Value;
            }
            webSearchCriteria.IsIsOriginalNameSpecified = searchCriteria.IsOriginalName.HasValue;
            if (searchCriteria.IsOriginalName.HasValue)
            {
                webSearchCriteria.IsOriginalName = searchCriteria.IsOriginalName.Value;
            }
            webSearchCriteria.IsIsRecommendedSpecified = searchCriteria.IsRecommended.HasValue;
            if (searchCriteria.IsRecommended.HasValue)
            {
                webSearchCriteria.IsRecommended = searchCriteria.IsRecommended.Value;
            }
            webSearchCriteria.IsIsUniqueSpecified = searchCriteria.IsUnique.HasValue;
            if (searchCriteria.IsUnique.HasValue)
            {
                webSearchCriteria.IsUnique = searchCriteria.IsUnique.Value;
            }
            webSearchCriteria.IsIsValidTaxonSpecified = searchCriteria.IsValidTaxon.HasValue;
            if (searchCriteria.IsValidTaxon.HasValue)
            {
                webSearchCriteria.IsValidTaxon = searchCriteria.IsValidTaxon.Value;
            }
            webSearchCriteria.IsIsValidTaxonNameSpecified = searchCriteria.IsValidTaxonName.HasValue;
            if (searchCriteria.IsValidTaxonName.HasValue)
            {
                webSearchCriteria.IsValidTaxonName = searchCriteria.IsValidTaxonName.Value;
            }
            if (searchCriteria.NameSearchString.IsNotNull())
            {
                webSearchCriteria.NameSearchString = new WebStringSearchCriteria();
                webSearchCriteria.NameSearchString.SearchString = searchCriteria.NameSearchString.SearchString;
                if (searchCriteria.NameSearchString.CompareOperators.IsNotEmpty())
                {
                    webSearchCriteria.NameSearchString.CompareOperators = searchCriteria.NameSearchString.CompareOperators;
                }
            }
            webSearchCriteria.IsStatusIdSpecified = searchCriteria.Status.IsNotNull();
            if (searchCriteria.Status.IsNotNull())
            {
                webSearchCriteria.StatusId = searchCriteria.Status.Id;
            }
            webSearchCriteria.TaxonIds = searchCriteria.TaxonIds;
            // add last modified dates as dynamic WebDataFields
            if (searchCriteria.LastModifiedStartDate.HasValue)
            {
                WebDataField dataField;
                dataField = new WebDataField();
                dataField.Name = "ModifiedDateStart";
                dataField.Type = WebDataType.DateTime;
                dataField.Value = searchCriteria.LastModifiedStartDate.Value.ToString();
                if (webSearchCriteria.DataFields.IsNull())
                {
                    webSearchCriteria.DataFields = new List<WebDataField>();
                }
                webSearchCriteria.DataFields.Add(dataField);
            }
            if (searchCriteria.LastModifiedEndDate.HasValue)
            {
                WebDataField dataField;
                dataField = new WebDataField();
                dataField.Name = "ModifiedDateEnd";
                dataField.Type = WebDataType.DateTime;
                dataField.Value = searchCriteria.LastModifiedEndDate.Value.ToString();
                if (webSearchCriteria.DataFields.IsNull())
                {
                    webSearchCriteria.DataFields = new List<WebDataField>();
                }
                webSearchCriteria.DataFields.Add(dataField);
            }
            return webSearchCriteria;
        }

        /// <summary>
        /// Get ITaxonNameStatus from WebTaxonNameStatus.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameStatus">Taxon name status.</param>
        /// <returns>A TaxonNameStatus object.</returns>
        private ITaxonNameStatus GetTaxonNameStatus(IUserContext userContext,
                                                    WebTaxonNameStatus webTaxonNameStatus)
        {
            TaxonNameStatus taxonNameStatus;

            taxonNameStatus = new TaxonNameStatus();
            taxonNameStatus.DataContext = GetDataContext(userContext);
            taxonNameStatus.Description = webTaxonNameStatus.Description;
            taxonNameStatus.Id = webTaxonNameStatus.Id;
            taxonNameStatus.Name = webTaxonNameStatus.Name;
            return taxonNameStatus;
        }


        /// <summary>
        /// Get name usage id.        
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <returns>Name usage id.</returns>
        private Int32 GetNameUsageId(WebData webData)
        {
            if (webData.DataFields.IsDataFieldSpecified(Settings.Default.WebDataModifiedByPerson))
            {
                return webData.DataFields.GetInt32(Settings.Default.WebDataNameUsageId);
            }
            else
            {                
                return -1;
            }
        }

        /// <summary>
        /// Get ITaxonNameUsage from WebTaxonNameUsage.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameUsage">Taxon name usage.</param>
        /// <returns>A TaxonNameUsage object.</returns>
        private ITaxonNameUsage GetTaxonNameUsage(IUserContext userContext,
                                                    WebTaxonNameUsage webTaxonNameUsage)
        {
            TaxonNameUsage taxonNameUsage;

            taxonNameUsage = new TaxonNameUsage();
            taxonNameUsage.DataContext = GetDataContext(userContext);
            taxonNameUsage.Description = webTaxonNameUsage.Description;
            taxonNameUsage.Id = webTaxonNameUsage.Id;
            taxonNameUsage.Name = webTaxonNameUsage.Name;
            return taxonNameUsage;
        }

        /// <summary>
        /// Get information about possbile statuses for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Information about possbile statuses for taxon names.</returns>
        public virtual TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext)
        {
            List<WebTaxonNameStatus> taxonNameStatus;

            CheckTransaction(userContext);
            taxonNameStatus = WebServiceProxy.TaxonService.GetTaxonNameStatuses(GetClientInformation(userContext));
            return GetTaxonNameStatuses(userContext, taxonNameStatus);
        }

        /// <summary>
        /// Get information about possible name usage for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Information about possible name usage for taxon names.
        /// </returns>
        public TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext)
        {
            List<WebTaxonNameUsage> taxonNameStatus;

            CheckTransaction(userContext);
            taxonNameStatus = WebServiceProxy.TaxonService.GetTaxonNameUsages(GetClientInformation(userContext));
            return GetTaxonNameUsages(userContext, taxonNameStatus);
        }

        /// <summary>
        /// Get a list of ITaxonNameStatus from a list of WebTaxonNameStatus.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameStatusList">List of taxon name status.</param>
        /// <returns>A list of ITaxonNameStatus.</returns>
        private TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext,
                                                         List<WebTaxonNameStatus> webTaxonNameStatusList)
        {
            TaxonNameStatusList taxonNameStatus;

            taxonNameStatus = new TaxonNameStatusList();
            if (webTaxonNameStatusList.IsNotEmpty())
            {
                foreach (WebTaxonNameStatus webTaxonNameStatus in webTaxonNameStatusList)
                {
                    taxonNameStatus.Add(GetTaxonNameStatus(userContext, webTaxonNameStatus));
                }
            }
            return taxonNameStatus;
        }

        /// <summary>
        /// Get a list of ITaxonNameUsage from a list of WebTaxonNameUsage.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonNameUsageList">List of taxon name usage.</param>
        /// <returns>A list of ITaxonNameUsage.</returns>
        private TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext,
                                                         List<WebTaxonNameUsage> webTaxonNameUsageList)
        {
            TaxonNameUsageList taxonNameUsage;

            taxonNameUsage = new TaxonNameUsageList();
            if (webTaxonNameUsageList.IsNotEmpty())
            {
                foreach (WebTaxonNameUsage webTaxonNameStatus in webTaxonNameUsageList)
                {
                    taxonNameUsage.Add(GetTaxonNameUsage(userContext, webTaxonNameStatus));
                }
            }

            return taxonNameUsage;
        }

        /// <summary>
        /// Get version of the WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>A WebTaxonName instance.</param>
        /// <returns>Version of the WebTaxonName.</returns>
        private Int32 GetTaxonNameVersion(WebTaxonName taxonName)
        {
            if (taxonName.DataFields.IsDataFieldSpecified(Settings.Default.TaxonNameVersionField))
            {
                return taxonName.DataFields.GetInt32(Settings.Default.TaxonNameVersionField);
            }
            else
            {
                // This should only happen if dynamic taxon name version
                // handling in TaxonService has been replaced with
                // correct version handling in GUID.
                return Int32.MinValue;
            }
        }

        private WebTaxonProperties GetTaxonProperties(ITaxonProperties taxonProperties)
        {
            WebTaxonProperties webTaxonProperties;

            webTaxonProperties = new WebTaxonProperties();
            webTaxonProperties.AlertStatusId = (Int32)(taxonProperties.AlertStatus);
            if (taxonProperties.ChangedInTaxonRevisionEvent.IsNotNull())
            {
                webTaxonProperties.ChangedInTaxonRevisionEvent = new WebTaxonRevisionEvent { Id = taxonProperties.ChangedInTaxonRevisionEvent.Id };
            }
            webTaxonProperties.ConceptDefinition = taxonProperties.ConceptDefinition;
            webTaxonProperties.Id = taxonProperties.Id;
            webTaxonProperties.IsPublished = taxonProperties.IsPublished;
            webTaxonProperties.IsValid = taxonProperties.IsValid;
            if (taxonProperties.ModifiedBy.IsNotNull())
            {
                webTaxonProperties.ModifiedBy = new WebUser { Id = taxonProperties.ModifiedBy.Id };
            }
            SetModifiedByPerson(webTaxonProperties, taxonProperties.ModifiedByPerson);
            webTaxonProperties.ModifiedDate = taxonProperties.ModifiedDate;
            webTaxonProperties.PartOfConceptDefinition = taxonProperties.PartOfConceptDefinition;
            if (taxonProperties.ReplacedInTaxonRevisionEvent.IsNotNull())
            {
                webTaxonProperties.ReplacedInTaxonRevisionEvent = new WebTaxonRevisionEvent { Id = taxonProperties.ReplacedInTaxonRevisionEvent.Id };
            }
            if (taxonProperties.Taxon.IsNotNull())
            {
                webTaxonProperties.Taxon = new WebTaxon { Id = taxonProperties.Taxon.Id };
            }
            if (taxonProperties.TaxonCategory.IsNotNull())
            {
                webTaxonProperties.TaxonCategory = new WebTaxonCategory { Id = taxonProperties.TaxonCategory.Id, Name = taxonProperties.TaxonCategory.Name };
            }
            webTaxonProperties.ValidFromDate = taxonProperties.ValidFromDate;
            webTaxonProperties.ValidToDate = taxonProperties.ValidToDate;
            
            if (webTaxonProperties.DataFields == null)
            {
                webTaxonProperties.DataFields = new List<WebDataField>();
            }
            
            webTaxonProperties.DataFields.Add(new WebDataField
            {
                Name = IS_MICROSPECIES,
                Type = WebDataType.Boolean,
                Value = taxonProperties.IsMicrospecies.ToString()
            });

            return webTaxonProperties;
        }

        /// <summary>
        /// Get all taxon properties for specified taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>All taxon properties for specified taxon.</returns>
        public virtual TaxonPropertiesList GetTaxonProperties(IUserContext userContext,
                                                              ITaxon taxon)
        {
            ITaxonProperties taxonProperties;
            TaxonPropertiesList taxonPropertiesList;
            List<WebTaxonProperties> webTaxonPropertiesList;

            CheckTransaction(userContext);
            webTaxonPropertiesList = WebServiceProxy.TaxonService.GetTaxonPropertiesByTaxonId(GetClientInformation(userContext),
                                                                                          taxon.Id);
            taxonPropertiesList = new TaxonPropertiesList();
            foreach (WebTaxonProperties webTaxonProperties in webTaxonPropertiesList)
            {
                taxonProperties = new TaxonProperties();
                UpdateTaxonProperties(userContext, taxonProperties, webTaxonProperties);
                taxonPropertiesList.Add(taxonProperties);
            }
            return taxonPropertiesList;
        }

        /// <summary>
        /// Convert a ITaxonRelation instance
        /// to a WebTaxonRelation instance.
        /// </summary>
        /// <param name="taxonRelation">A ITaxonRelation object.</param>
        /// <returns>A WebTaxonRelation instance.</returns>
        private WebTaxonRelation GetTaxonRelation(ITaxonRelation taxonRelation)
        {
            WebTaxonRelation webTaxonRelation;

            webTaxonRelation = new WebTaxonRelation();
            if (taxonRelation.ChangedInTaxonRevisionEventId.HasValue)
            {
                webTaxonRelation.ChangedInTaxonRevisionEventId = taxonRelation.ChangedInTaxonRevisionEventId.Value;
            }
            webTaxonRelation.ChildTaxonId = taxonRelation.ChildTaxon.Id;
            webTaxonRelation.CreatedBy = taxonRelation.CreatedBy;
            webTaxonRelation.CreatedDate = taxonRelation.CreatedDate;
            webTaxonRelation.Guid = taxonRelation.Guid;
            webTaxonRelation.Id = taxonRelation.Id;
            webTaxonRelation.IsChangedInTaxonRevisionEventIdSpecified = taxonRelation.ChangedInTaxonRevisionEventId.HasValue;
            webTaxonRelation.IsMainRelation = taxonRelation.IsMainRelation;
            webTaxonRelation.IsPublished = taxonRelation.IsPublished;
            webTaxonRelation.IsReplacedInTaxonRevisionEventIdSpecified = taxonRelation.ReplacedInTaxonRevisionEventId.HasValue;
            webTaxonRelation.ModifiedBy = taxonRelation.ModifiedBy;
            SetModifiedByPerson(webTaxonRelation, taxonRelation.ModifiedByPerson);
            webTaxonRelation.ModifiedDate = taxonRelation.ModifiedDate;
            webTaxonRelation.ParentTaxonId = taxonRelation.ParentTaxon.Id;
            if (taxonRelation.ReplacedInTaxonRevisionEventId.HasValue)
            {
                webTaxonRelation.ReplacedInTaxonRevisionEventId = taxonRelation.ReplacedInTaxonRevisionEventId.Value;
            }
            webTaxonRelation.SortOrder = taxonRelation.SortOrder;
            webTaxonRelation.ValidToDate = taxonRelation.ValidToDate;
            webTaxonRelation.ValidFromDate = taxonRelation.ValidFromDate;
            return webTaxonRelation;
        }

        /// <summary>
        /// Convert a WebTaxonRelation instance
        /// to a ITaxonRelation instance.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonRelation">A WebTaxonRelation object.</param>
        /// <param name="taxa">Taxa instances used to improve performance.</param>
        /// <returns>A ITaxonRelation instance.</returns>
        private ITaxonRelation GetTaxonRelation(IUserContext userContext,
                                                WebTaxonRelation webTaxonRelation,
                                                TaxonList taxa)
        {
            TaxonRelation taxonRelation;

            taxonRelation = new TaxonRelation();
            UpdateTaxonRelation(userContext, taxonRelation, webTaxonRelation, taxa);
            return taxonRelation;
        }

        /// <summary>
        /// Convert a list of WebTaxonRelation instances
        /// to a TaxonRelationList.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="webTaxonRelations">List of WebTaxonRelation instances.</param>
        /// <returns>Taxon relations.</returns>
        private TaxonRelationList GetTaxonRelations(IUserContext userContext,
                                                    List<WebTaxonRelation> webTaxonRelations)
        {
            Hashtable tempTaxonIds;
            List<Int32> taxonIds;
            TaxonRelationList taxonRelations;
            TaxonList taxa;

            taxonRelations = new TaxonRelationList();
            if (webTaxonRelations.IsNotEmpty())
            {
                // Get all taxa.
                // Hashtable tempTaxonIds is only used to improve
                // performance. It does not affect the functionality.
                tempTaxonIds = new Hashtable();
                taxonIds = new List<Int32>();
                foreach (WebTaxonRelation webTaxonRelation in webTaxonRelations)
                {
                    if (!tempTaxonIds.ContainsKey(webTaxonRelation.ChildTaxonId))
                    {
                        tempTaxonIds[webTaxonRelation.ChildTaxonId] = webTaxonRelation.ChildTaxonId;
                        taxonIds.Add(webTaxonRelation.ChildTaxonId);
                    }
                    if (!tempTaxonIds.ContainsKey(webTaxonRelation.ParentTaxonId))
                    {
                        tempTaxonIds[webTaxonRelation.ParentTaxonId] = webTaxonRelation.ParentTaxonId;
                        taxonIds.Add(webTaxonRelation.ParentTaxonId);
                    }
                }
                taxa = CoreData.TaxonManager.GetTaxa(userContext, taxonIds);

                // Transform WebTaxonRelations into ITaxonRelations.
                foreach (WebTaxonRelation webTaxonRelation in webTaxonRelations)
                {
                    taxonRelations.Add(GetTaxonRelation(userContext, webTaxonRelation, taxa));
                }
            }

            return taxonRelations;
        }

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        public virtual TaxonRelationList GetTaxonRelations(IUserContext userContext,
                                                           ITaxonRelationSearchCriteria searchCriteria)
        {
            List<WebTaxonRelation> webTaxonRelations;
            TaxonRelationList taxonRelations;

            CheckTransaction(userContext);
            webTaxonRelations = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(GetClientInformation(userContext),
                                                                                               GetTaxonRelationSearchCriteria(searchCriteria));
            taxonRelations = GetTaxonRelations(userContext, webTaxonRelations);
            switch (searchCriteria.Scope)
            {
                case TaxonRelationSearchScope.AllParentRelations:
                    // This sort is necessary if the user is in a revision.
                    // Normal Sort() method will result in strange order
                    // for new taxon relations.
                    // taxonRelations.SortTaxonCategory();
                    taxonRelations.SortParentTaxon();
                    break;
                case TaxonRelationSearchScope.AllChildRelations:
                    taxonRelations.SortChildTaxon();
                    break;
                default:
                    taxonRelations.Sort();
                    break;                    
            }
            return taxonRelations;
        }

        /// <summary>
        /// Get Web Taxon Relations by search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>WebTaxonRelations that matches search criteria.</returns>
        public List<WebTaxonRelation> GetWebTaxonRelations(
            IUserContext userContext,
            ITaxonRelationSearchCriteria searchCriteria)
        {
            List<WebTaxonRelation> webTaxonRelations;            

            CheckTransaction(userContext);
            webTaxonRelations = WebServiceProxy.TaxonService.GetTaxonRelationsBySearchCriteria(
                GetClientInformation(userContext),
                GetTaxonRelationSearchCriteria(searchCriteria));

            return webTaxonRelations;
        }

        /// <summary>
        /// Convert a list of WebTaxonRelation to TaxonRelationList.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRelations">The list of WebTaxonRelations to convert.</param>
        /// <returns>TaxonRelationList.</returns>
        public TaxonRelationList ConvertWebTaxonRelations(
            IUserContext userContext,
            List<WebTaxonRelation> webTaxonRelations)
        {
            TaxonRelationList taxonRelations;

            taxonRelations = GetTaxonRelations(userContext, webTaxonRelations);            
            taxonRelations.SortChildTaxon();             
            return taxonRelations;
        }

        /// <summary>
        /// Convert a ITaxonRelationSearchCriteria instance
        /// into a WebTaxonRelationSearchCriteria instance.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns> A WebTaxonRelationSearchCriteria instance.</returns>
        private WebTaxonRelationSearchCriteria GetTaxonRelationSearchCriteria(ITaxonRelationSearchCriteria searchCriteria)
        {
            WebTaxonRelationSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebTaxonRelationSearchCriteria();
            webSearchCriteria.IsIsMainRelationSpecified = searchCriteria.IsMainRelation.HasValue;
            if (searchCriteria.IsMainRelation.HasValue)
            {
                webSearchCriteria.IsMainRelation = searchCriteria.IsMainRelation.Value;
            }
            webSearchCriteria.IsIsValidSpecified = searchCriteria.IsValid.HasValue;
            if (searchCriteria.IsValid.HasValue)
            {
                webSearchCriteria.IsValid = searchCriteria.IsValid.Value;
            }
            webSearchCriteria.Scope = searchCriteria.Scope;
            if (searchCriteria.Taxa.IsNotEmpty())
            {
                webSearchCriteria.TaxonIds = searchCriteria.Taxa.GetIds();
            }
            return webSearchCriteria;
        }

        /// <summary>
        /// Get revision from web taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webRevision">A web revision.</param>
        /// <returns>A Revision.</returns>
        private ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                                WebTaxonRevision webRevision)
        {
            ITaxonRevision taxonRevision;

            taxonRevision = new TaxonRevision();
            UpdateTaxonRevision(userContext, taxonRevision, webRevision);
            return taxonRevision;
        }

        /// <summary>
        /// Get revision by id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionId">The revision id.</param>
        /// <returns>Revision</returns>
        public virtual ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                                       Int32 taxonRevisionId)
        {
            WebTaxonRevision webTaxonRevision;

            CheckTransaction(userContext);
            webTaxonRevision = WebServiceProxy.TaxonService.GetTaxonRevisionById(GetClientInformation(userContext),
                                                                                 taxonRevisionId);
            return GetTaxonRevision(userContext, webTaxonRevision);
        }

        /// <summary>
        /// Convert a ITaxonRevision instance
        /// to a WebTaxonRevision instance.
        /// </summary>
        /// <param name="taxonRevision">A ITaxonRevision object.</param>
        /// <returns>A WebTaxonRevision instance.</returns>
        private WebTaxonRevision GetTaxonRevision(ITaxonRevision taxonRevision)
        {
            WebTaxonRevision webTaxonRevision;

            webTaxonRevision = new WebTaxonRevision();
            webTaxonRevision.CreatedBy = taxonRevision.CreatedBy;
            webTaxonRevision.CreatedDate = taxonRevision.CreatedDate;
            webTaxonRevision.Description = taxonRevision.Description;
            webTaxonRevision.ExpectedEndDate = taxonRevision.ExpectedEndDate;
            webTaxonRevision.ExpectedStartDate = taxonRevision.ExpectedStartDate;
            webTaxonRevision.Guid = taxonRevision.Guid;
            webTaxonRevision.Id = taxonRevision.Id;
            webTaxonRevision.ModifiedBy = taxonRevision.ModifiedBy;
            webTaxonRevision.ModifiedDate = taxonRevision.ModifiedDate;
            webTaxonRevision.RootTaxon = GetTaxon(taxonRevision.RootTaxon);
            webTaxonRevision.StateId = taxonRevision.State.Id;
            return webTaxonRevision;
        }

        /// <summary>
        /// Get taxon revision with specified GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionGuid">Taxon revision GUID.</param>
        /// <returns>Taxon revision with specified GUID.</returns>       
        public virtual ITaxonRevision GetTaxonRevision(IUserContext userContext,
                                                       String taxonRevisionGuid)
        {
            WebTaxonRevision webTaxonRevision;

            CheckTransaction(userContext);
            webTaxonRevision = WebServiceProxy.TaxonService.GetTaxonRevisionByGuid(GetClientInformation(userContext),
                                                                              taxonRevisionGuid);
            return GetTaxonRevision(userContext, webTaxonRevision);
        }

        /// <summary>
        /// Get taxon revision event with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEventId">The taxon revision event id.</param>
        /// <returns>Taxon revision event with specified id.</returns>
        public virtual ITaxonRevisionEvent GetTaxonRevisionEvent(IUserContext userContext,
                                                                 Int32 taxonRevisionEventId)
        {
            WebTaxonRevisionEvent webRevisionEvent;

            CheckTransaction(userContext);
            webRevisionEvent = WebServiceProxy.TaxonService.GetTaxonRevisionEventById(GetClientInformation(userContext),
                                                                                      taxonRevisionEventId);

            var revisionEvent = new TaxonRevisionEvent();
            UpdateTaxonRevisionEvent(userContext, revisionEvent, webRevisionEvent);
            return revisionEvent;
        }

        /// <summary>
        /// Convert a ITaxonRevisionEvent instance
        /// to a WebTaxonRevisionEvent instance.
        /// </summary>
        /// <param name="taxonRevisionEvent">A ITaxonRevisionEvent object.</param>
        /// <returns>A WebTaxonRevisionEvent instance.</returns>
        private WebTaxonRevisionEvent GetTaxonRevisionEvent(ITaxonRevisionEvent taxonRevisionEvent)
        {
            WebTaxonRevisionEvent webTaxonRevisionEvent;

            webTaxonRevisionEvent = null;
            if (taxonRevisionEvent.IsNotNull())
            {
                webTaxonRevisionEvent = new WebTaxonRevisionEvent();
                webTaxonRevisionEvent.AffectedTaxa = taxonRevisionEvent.AffectedTaxa;
                webTaxonRevisionEvent.CreatedBy = taxonRevisionEvent.CreatedBy;
                webTaxonRevisionEvent.CreatedDate = taxonRevisionEvent.CreatedDate;
                webTaxonRevisionEvent.Id = taxonRevisionEvent.Id;
                webTaxonRevisionEvent.NewValue = taxonRevisionEvent.NewValue;
                webTaxonRevisionEvent.OldValue = taxonRevisionEvent.OldValue;
                webTaxonRevisionEvent.RevisionId = taxonRevisionEvent.RevisionId;
                webTaxonRevisionEvent.TypeId = taxonRevisionEvent.Type.Id;
            }

            return webTaxonRevisionEvent;
        }

        /// <summary>
        /// Convert a WebTaxonRevisionEvent instance
        /// to a ITaxonRevisionEvent instance.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionEvent">A WebTaxonRevisionEvent object.</param>
        /// <returns>A ITaxonRevisionEvent instance.</returns>
        private ITaxonRevisionEvent GetTaxonRevisionEvent(IUserContext userContext,
                                                          WebTaxonRevisionEvent webTaxonRevisionEvent)
        {
            ITaxonRevisionEvent taxonRevisionEvent;

            taxonRevisionEvent = new TaxonRevisionEvent();
            UpdateTaxonRevisionEvent(userContext, taxonRevisionEvent, webTaxonRevisionEvent);
            return taxonRevisionEvent;
        }

        /// <summary>
        /// Get taxon revision events for specified taxon revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionId">Id for taxon revision.</param>
        /// <returns>Taxon revision events for specified taxon revision.</returns>
        public virtual TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext,
                                                                     Int32 taxonRevisionId)
        {
            List<WebTaxonRevisionEvent> webTaxonRevisionEvents;

            CheckTransaction(userContext);
            webTaxonRevisionEvents = WebServiceProxy.TaxonService.GetTaxonRevisionEventsByTaxonRevisionId(GetClientInformation(userContext), taxonRevisionId);
            return GetTaxonRevisionEvents(userContext, webTaxonRevisionEvents);
        }

        /// <summary>
        /// Convert list of WebTaxonRevisionEvents to 
        /// TaxonRevisionEventList.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionEvents">List of WebTaxonRevisionEvents.</param>
        /// <returns>Taxon revision event list.</returns>
        private TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext,
                                                              List<WebTaxonRevisionEvent> webTaxonRevisionEvents)
        {
            TaxonRevisionEventList taxonRevisionEvents;

            taxonRevisionEvents = new TaxonRevisionEventList();
            if (webTaxonRevisionEvents.IsNotEmpty())
            {
                foreach (WebTaxonRevisionEvent webTaxonRevisionEvent in webTaxonRevisionEvents)
                {
                    taxonRevisionEvents.Add(GetTaxonRevisionEvent(userContext, webTaxonRevisionEvent));
                }
            }
            return taxonRevisionEvents;
        }

        /// <summary>
        /// Convert a WebTaxonRevisionEventType instance
        /// into a IRevisionEventType instance.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionEventType">A WebTaxonRevisionEventType instance.</param>
        /// <returns>A IRevisionEventType instance.</returns>
        private ITaxonRevisionEventType GetTaxonRevisionEventType(IUserContext userContext,
                                                                  WebTaxonRevisionEventType webTaxonRevisionEventType)
        {
            ITaxonRevisionEventType taxonTaxonRevisionEventType;

            taxonTaxonRevisionEventType = new TaxonRevisionEventType();
            taxonTaxonRevisionEventType.DataContext = GetDataContext(userContext);
            taxonTaxonRevisionEventType.Description = webTaxonRevisionEventType.Description;
            taxonTaxonRevisionEventType.Id = webTaxonRevisionEventType.Id;
            taxonTaxonRevisionEventType.Identifier = webTaxonRevisionEventType.Identifier;
            return taxonTaxonRevisionEventType;
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision event types.</returns>
        public virtual TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext)
        {
            List<WebTaxonRevisionEventType> webTaxonRevisionEventTypes;

            CheckTransaction(userContext);
            webTaxonRevisionEventTypes = WebServiceProxy.TaxonService.GetTaxonRevisionEventTypes(GetClientInformation(userContext));
            return GetTaxonRevisionEventTypes(userContext, webTaxonRevisionEventTypes);
        }

        /// <summary>
        /// Convert list of WebTaxonRevisionEventTypes to 
        /// TaxonRevisionEventTypeList.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionEventTypes">List of WebTaxonRevisionEventTypes.</param>
        /// <returns>Taxon revision event type list.</returns>
        private TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext,
                                                                      List<WebTaxonRevisionEventType> webTaxonRevisionEventTypes)
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes;

            taxonRevisionEventTypes = new TaxonRevisionEventTypeList();
            if (webTaxonRevisionEventTypes.IsNotEmpty())
            {
                foreach (WebTaxonRevisionEventType webTaxonRevisionEventType in webTaxonRevisionEventTypes)
                {
                    taxonRevisionEventTypes.Add(GetTaxonRevisionEventType(userContext, webTaxonRevisionEventType));
                }
            }
            return taxonRevisionEventTypes;
        }

        /// <summary>
        /// Get taxon revisions that matches search criteria.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchCriteria">Taxon revision search criteria. </param>
        /// <returns>Taxon revisions that matches search criteria.</returns>
        public virtual TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                                           ITaxonRevisionSearchCriteria searchCriteria)
        {
            List<WebTaxonRevision> webTaxonRevisions;

            CheckTransaction(userContext);
            webTaxonRevisions = WebServiceProxy.TaxonService.GetTaxonRevisionsBySearchCriteria(GetClientInformation(userContext),
                                                                                               GetTaxonRevisionSearchCriteria(searchCriteria));
            return GetTaxonRevisions(userContext, webTaxonRevisions);
        }

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A list of revisions.</returns>
        public virtual TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                                           ITaxon taxon)
        {
            List<WebTaxonRevision> webTaxonRevisions;

            CheckTransaction(userContext);
            webTaxonRevisions = WebServiceProxy.TaxonService.GetTaxonRevisionsByTaxonId(GetClientInformation(userContext), taxon.Id);
            return GetTaxonRevisions(userContext, webTaxonRevisions);
        }

        /// <summary>
        /// Convert list of WebTaxonRevisions to 
        /// TaxonRevisionList.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisions">List of WebTaxonRevisions.</param>
        /// <returns>Taxon revision list.</returns>
        private TaxonRevisionList GetTaxonRevisions(IUserContext userContext,
                                                    List<WebTaxonRevision> webTaxonRevisions)
        {
            TaxonRevisionList taxonRevisions;

            taxonRevisions = new TaxonRevisionList();
            if (webTaxonRevisions.IsNotEmpty())
            {
                foreach (WebTaxonRevision webTaxonRevision in webTaxonRevisions)
                {
                    taxonRevisions.Add(GetTaxonRevision(userContext, webTaxonRevision));
                }
            }
            return taxonRevisions;
        }

        /// <summary>
        /// Get web taxon revision search criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web revision search criteria.</returns>
        private WebTaxonRevisionSearchCriteria GetTaxonRevisionSearchCriteria(ITaxonRevisionSearchCriteria searchCriteria)
        {
            WebTaxonRevisionSearchCriteria webRevisionSearchCriteria;

            webRevisionSearchCriteria = new WebTaxonRevisionSearchCriteria();
            webRevisionSearchCriteria.StateIds = searchCriteria.StateIds;
            webRevisionSearchCriteria.TaxonIds = searchCriteria.TaxonIds;
            return webRevisionSearchCriteria;
        }

        /// <summary>
        /// Convert a WebTaxonRevisionState instance
        /// into a IRevisionState instance.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionState">A WebTaxonRevisionState instance.</param>
        /// <returns>A IRevisionState instance.</returns>
        private ITaxonRevisionState GetTaxonRevisionState(IUserContext userContext,
                                                          WebTaxonRevisionState webTaxonRevisionState)
        {
            ITaxonRevisionState taxonRevisionState;

            taxonRevisionState = new TaxonRevisionState();
            taxonRevisionState.DataContext = GetDataContext(userContext);
            taxonRevisionState.Description = webTaxonRevisionState.Description;
            taxonRevisionState.Id = webTaxonRevisionState.Id;
            taxonRevisionState.Identifier = webTaxonRevisionState.Identifier;
            return taxonRevisionState;
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision states.</returns>
        public TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext)
        {
            List<WebTaxonRevisionState> webTaxonRevisionStates;

            CheckTransaction(userContext);
            webTaxonRevisionStates = WebServiceProxy.TaxonService.GetTaxonRevisionStates(GetClientInformation(userContext));
            return GetTaxonRevisionStates(userContext, webTaxonRevisionStates);
        }

        /// <summary>
        /// Convert list of WebTaxonRevisionStates to 
        /// TaxonRevisionStateList.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="webTaxonRevisionStates">List of WebTaxonRevisionStates.</param>
        /// <returns>Taxon revision state list.</returns>
        private TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext,
                                                              List<WebTaxonRevisionState> webTaxonRevisionStates)
        {
            TaxonRevisionStateList taxonRevisionStates;

            taxonRevisionStates = new TaxonRevisionStateList();
            if (webTaxonRevisionStates.IsNotEmpty())
            {
                foreach (WebTaxonRevisionState webTaxonRevisionState in webTaxonRevisionStates)
                {
                    taxonRevisionStates.Add(GetTaxonRevisionState(userContext, webTaxonRevisionState));
                }
            }
            return taxonRevisionStates;
        }

        /// <summary>
        /// Get web taxonsearch criteria.
        /// </summary>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Web search criteria.</returns>
        private WebTaxonSearchCriteria GetTaxonSearchCriteria(ITaxonSearchCriteria searchCriteria)
        {
            WebTaxonSearchCriteria webTaxonSearchCriteria;

            webTaxonSearchCriteria = new WebTaxonSearchCriteria();
            webTaxonSearchCriteria.IsIsValidTaxonSpecified = searchCriteria.IsValidTaxon.HasValue;
            if (searchCriteria.IsValidTaxon.HasValue)
            {
                webTaxonSearchCriteria.IsValidTaxon = searchCriteria.IsValidTaxon.Value;
            }
            webTaxonSearchCriteria.Scope = searchCriteria.Scope;
            webTaxonSearchCriteria.SwedishImmigrationHistory = searchCriteria.SwedishImmigrationHistory;
            webTaxonSearchCriteria.SwedishOccurrence = searchCriteria.SwedishOccurrence;
            webTaxonSearchCriteria.TaxonIds = searchCriteria.TaxonIds;
            webTaxonSearchCriteria.TaxonCategoryIds = searchCriteria.TaxonCategoryIds;
            if (searchCriteria.TaxonNameSearchString.IsNotEmpty())
            {
                webTaxonSearchCriteria.TaxonNameSearchString = new WebStringSearchCriteria();
                webTaxonSearchCriteria.TaxonNameSearchString.SearchString = searchCriteria.TaxonNameSearchString;
            }
            return webTaxonSearchCriteria;
        }

        /// <summary>
        /// Get TaxonTreeNode from WebTaxonTreeNode.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="webTaxonTree">Web taxon tree node.</param>
        /// <param name="taxonTreeNodes">
        /// All taxon tree nodes that has been created so far.
        /// This parameter is used to avoid duplication of
        /// taxon tree nodes (taxon tree nodes with same taxon attached to it)
        /// if the same taxon appears more than once in the tree.
        /// </param>
        /// <param name="circularTaxonTreeNodes">
        /// All taxon tree nodes that contains circular taxon tree references.
        /// </param>
        /// <returns>A TaxonTreeNode instance.</returns>
        private ITaxonTreeNode GetTaxonTree(IUserContext userContext,
                                            WebTaxonTreeNode webTaxonTree,
                                            Hashtable taxonTreeNodes,
                                            List<WebTaxonTreeNode> circularTaxonTreeNodes)
        {
            ITaxonTreeNode child, taxonTree;

            if (taxonTreeNodes.ContainsKey(webTaxonTree.Taxon.Id))
            {
                taxonTree = (ITaxonTreeNode)(taxonTreeNodes[webTaxonTree.Taxon.Id]);
            }
            else
            {
                taxonTree = new TaxonTreeNode();
                taxonTree.DataContext = GetDataContext(userContext);
                taxonTree.Taxon = GetTaxon(userContext, webTaxonTree.Taxon);
                taxonTreeNodes[webTaxonTree.Taxon.Id] = taxonTree;
                if (webTaxonTree.Children.IsNotEmpty())
                {
                    taxonTree.Children = new TaxonTreeNodeList();
                    foreach (WebTaxonTreeNode webChild in webTaxonTree.Children)
                    {
                        child = GetTaxonTree(userContext,
                                             webChild,
                                             taxonTreeNodes,
                                             circularTaxonTreeNodes);
                        if (child.Parents.IsNull())
                        {
                            child.Parents = new TaxonTreeNodeList();
                        }
                        child.Parents.Add(taxonTree);
                        taxonTree.Children.Add(child);
                    }
                }
            }

            if (webTaxonTree.DataFields.IsDataFieldSpecified(Settings.Default.WebDataChildrenCircularTaxonIds))
            {
                circularTaxonTreeNodes.Add(webTaxonTree);
            }

            return taxonTree;
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public virtual TaxonTreeNodeList GetTaxonTrees(IUserContext userContext,
                                                       ITaxonTreeSearchCriteria searchCriteria)
        {
            Hashtable taxonTreeNodes;
            Int32 taxonId;
            ITaxonTreeNode child, taxonTree;
            List<WebTaxonTreeNode> webTaxonTrees;
            TaxonTreeNodeList taxonTrees;
            List<WebTaxonTreeNode> circularTaxonTreeNodes;
            String circularChildrenTaxonIds;
            String[] splitTaxonIds;

            CheckTransaction(userContext);
            webTaxonTrees = WebServiceProxy.TaxonService.GetTaxonTreesBySearchCriteria(GetClientInformation(userContext),
                                                                                       GetTaxonTreeSearchCriteria(searchCriteria));
            circularTaxonTreeNodes = new List<WebTaxonTreeNode>();
            taxonTrees = new TaxonTreeNodeList();
            taxonTreeNodes = new Hashtable();
            if (webTaxonTrees.IsNotEmpty())
            {
                foreach (WebTaxonTreeNode webTaxonTree in webTaxonTrees)
                {
                    taxonTrees.Add(GetTaxonTree(userContext,
                                                webTaxonTree,
                                                taxonTreeNodes,
                                                circularTaxonTreeNodes));
                }

                if (circularTaxonTreeNodes.IsNotEmpty())
                {
                    foreach (WebTaxonTreeNode webTaxonTree in circularTaxonTreeNodes)
                    {
                        circularChildrenTaxonIds = webTaxonTree.DataFields.GetString(Settings.Default.WebDataChildrenCircularTaxonIds);
                        splitTaxonIds = circularChildrenTaxonIds.Split(',');
                        foreach (String splitTaxonId in splitTaxonIds)
                        {
                            taxonId = splitTaxonId.WebParseInt32();
                            if (taxonTreeNodes.ContainsKey(taxonId))
                            {
                                child = (ITaxonTreeNode)(taxonTreeNodes[taxonId]);
                                if (child.ParentsCircular.IsNull())
                                {
                                    child.ParentsCircular = new TaxonTreeNodeList();
                                }
                            }
                            else
                            {
                                throw new ApplicationException("Circular taxon tree child node not found. Taxon id = " + taxonId);
                            }

                            if (taxonTreeNodes.ContainsKey(webTaxonTree.Taxon.Id))
                            {
                                taxonTree = (ITaxonTreeNode)(taxonTreeNodes[webTaxonTree.Taxon.Id]);
                                if (taxonTree.ChildrenCircular.IsNull())
                                {
                                    taxonTree.ChildrenCircular = new TaxonTreeNodeList();
                                }
                            }
                            else
                            {
                                throw new ApplicationException("Circular taxon tree parent node not found. Taxon id = " + webTaxonTree.Taxon.Id);
                            }

                            taxonTree.ChildrenCircular.Merge(child);
                            child.ParentsCircular.Merge(taxonTree);
                        }
                    }
                }
            }

            return taxonTrees;
        }

        /// <summary>
        /// Get WebTaxonTreeSearchCriteria from ITaxonTreeSearchCriteria.
        /// </summary>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>A WebTaxonTreeSearchCriteria instance.</returns>
        private WebTaxonTreeSearchCriteria GetTaxonTreeSearchCriteria(ITaxonTreeSearchCriteria searchCriteria)
        {
            WebTaxonTreeSearchCriteria webSearchCriteria;

            webSearchCriteria = new WebTaxonTreeSearchCriteria();
            webSearchCriteria.IsMainRelationRequired = false;
            webSearchCriteria.IsValidRequired = searchCriteria.IsValidRequired;
            webSearchCriteria.Scope = searchCriteria.Scope;
            webSearchCriteria.TaxonIds = searchCriteria.TaxonIds;
            return webSearchCriteria;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="userContext">User context.</param>
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
        /// <returns>User context or null if login failed.</returns>
        public virtual void Login(IUserContext userContext,
                                  String userName,
                                  String password,
                                  String applicationIdentifier,
                                  Boolean isActivationRequired)
        {
            WebLoginResponse loginResponse;

            loginResponse = WebServiceProxy.TaxonService.Login(userName,
                                                               password,
                                                               applicationIdentifier,
                                                               isActivationRequired);
            if (loginResponse.IsNotNull())
            {
                SetToken(userContext, loginResponse.Token);
            }
        }

        /// <summary>
        /// Logout user.
        /// </summary>
        /// <param name="userContext">User context.</param>
        public virtual void Logout(IUserContext userContext)
        {
            WebServiceProxy.TaxonService.Logout(GetClientInformation(userContext));
            SetToken(userContext, null);
        }

        /// <summary>
        /// Saves taxon name.
        /// Ny funktion efter re-design av TaxonName objektet -- GuNy 2012-03-22
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNames">List of taxonnames that are to be saved. </param>    
        /// <param name="taxonRevisionEvent">Revision event.</param>
        public virtual void SaveTaxonNames(IUserContext userContext,
                                           TaxonNameList taxonNames,
                                           ITaxonRevisionEvent taxonRevisionEvent)
        {
            CheckTransaction(userContext);

            if (taxonNames.IsNotNull())
            {
                foreach (ITaxonName taxonName in taxonNames)
                {
                    if (taxonName.Version == 0)
                    {
                        var webTaxonName = WebServiceProxy.TaxonService.CreateTaxonName(GetClientInformation(userContext), GetTaxonName(taxonName));
                        UpdateTaxonName(userContext, taxonName, webTaxonName);
                    }
                    else if (taxonName.GetReplacedInRevisionEvent(userContext).IsNotNull() && taxonName.GetReplacedInRevisionEvent(userContext).Id == taxonRevisionEvent.Id)
                    {
                        var webTaxonName = WebServiceProxy.TaxonService.UpdateTaxonName(GetClientInformation(userContext), GetTaxonName(taxonName));
                        UpdateTaxonName(userContext, taxonName, webTaxonName);
                    }
                }
            }

            // Update the RevisionEvent
            if (taxonRevisionEvent.IsNotNull())
            {
                WebServiceProxy.TaxonService.UpdateTaxonRevisionEvent(GetClientInformation(userContext), taxonRevisionEvent.Id);
            }
        }

        /// <summary>
        /// Save revision.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">
        /// The revision.
        /// </param>
        public void SaveTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            CheckTransaction(userContext);

            WebTaxonRevision webRevision;

            if (taxonRevision.Id <= 0)
            {
                webRevision = WebServiceProxy.TaxonService.CreateTaxonRevision(GetClientInformation(userContext), GetTaxonRevision(taxonRevision));
            }
            else
            {
                webRevision = WebServiceProxy.TaxonService.UpdateTaxonRevision(GetClientInformation(userContext), GetTaxonRevision(taxonRevision));
            }
            UpdateTaxonRevision(userContext, taxonRevision, webRevision);
        }

        /// <summary>
        /// Set GeoReferenceService as data source in the onion data model.
        /// </summary>
        public static void SetDataSource()
        {
            TaxonDataSource taxonDataSource;

            taxonDataSource = new TaxonDataSource();
            CoreData.TaxonManager.DataSource = new TaxonDataSource();
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedInEvent += taxonDataSource.Login;
            ((UserDataSource)(CoreData.UserManager.DataSource)).UserLoggedOutEvent += taxonDataSource.Logout;
        }


        /// <summary>
        /// Set name usage id.        
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <param name='nameUsageId'>Name usage id.</param>
        private void SetNameUsageId(WebData webData,
                                         Int32 nameUsageId)
        {
            WebDataField dataField;

            // Add name usage id as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataNameUsageId;
            dataField.Type = WebDataType.Int32;
            dataField.Value = nameUsageId.WebToString();
            if (webData.DataFields.IsNull())
            {
                webData.DataFields = new List<WebDataField>();
            }

            webData.DataFields.Add(dataField);
        }

        /// <summary>
        /// Set name of person that made the last
        /// modification this piece of data.
        /// </summary>
        /// <param name='webData'>A WebData instance.</param>
        /// <param name='modifiedByPerson'>Name of person that made the last modification this piece of data.</param>
        private void SetModifiedByPerson(WebData webData,
                                         String modifiedByPerson)
        {
            WebDataField dataField;

            // Add modified by person as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.WebDataModifiedByPerson;
            dataField.Type = WebDataType.String;
            dataField.Value = modifiedByPerson;
            if (webData.DataFields.IsNull())
            {
                webData.DataFields = new List<WebDataField>();
            }
            webData.DataFields.Add(dataField);
        }

        /// <summary>
        /// Set version in WebTaxonName.
        /// </summary>
        /// <param name='taxonName'>A WebTaxonName instance.</param>
        /// <param name='version'>Version of the WebTaxonName.</param>
        private void SetTaxonNameVersion(WebTaxonName taxonName,
                                         Int32 version)
        {
            WebDataField dataField;

            // Add version as dynamic property.
            dataField = new WebDataField();
            dataField.Name = Settings.Default.TaxonNameVersionField;
            dataField.Type = WebDataType.Int32;
            dataField.Value = version.WebToString();
            if (taxonName.DataFields.IsNull())
            {
                taxonName.DataFields = new List<WebDataField>();
            }
            taxonName.DataFields.Add(dataField);
        }

        private void UpdateLumpSplitEvent(IUserContext userContext,
                                          ILumpSplitEvent lumpSplitEvent,
                                          WebLumpSplitEvent webLumpSplitEvent)
        {
            if (webLumpSplitEvent.IsChangedInTaxonRevisionEventIdSpecified)
            {
                lumpSplitEvent.ChangedInTaxonRevisionEventId = webLumpSplitEvent.ChangedInTaxonRevisionEventId;
            }
            else
            {
                lumpSplitEvent.ChangedInTaxonRevisionEventId = null;
            }
            lumpSplitEvent.CreatedBy = webLumpSplitEvent.CreatedBy;
            lumpSplitEvent.CreatedDate = webLumpSplitEvent.CreatedDate;
            lumpSplitEvent.DataContext = GetDataContext(userContext);
            lumpSplitEvent.Description = webLumpSplitEvent.Description;
            lumpSplitEvent.Id = webLumpSplitEvent.Id;
            if (webLumpSplitEvent.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                lumpSplitEvent.ReplacedInTaxonRevisionEventId = webLumpSplitEvent.ReplacedInTaxonRevisionEventId;
            }
            else
            {
                lumpSplitEvent.ReplacedInTaxonRevisionEventId = null;
            }
            if (lumpSplitEvent.TaxonAfter.IsNull() ||
                (lumpSplitEvent.TaxonAfter.Id != webLumpSplitEvent.TaxonIdAfter))
            {
                lumpSplitEvent.TaxonAfter = CoreData.TaxonManager.GetTaxon(userContext, webLumpSplitEvent.TaxonIdAfter);
            }
            if (lumpSplitEvent.TaxonBefore.IsNull() ||
                (lumpSplitEvent.TaxonBefore.Id != webLumpSplitEvent.TaxonIdBefore))
            {
                lumpSplitEvent.TaxonBefore = CoreData.TaxonManager.GetTaxon(userContext, webLumpSplitEvent.TaxonIdBefore);
            }
            lumpSplitEvent.Type = CoreData.TaxonManager.GetLumpSplitEventType(userContext, webLumpSplitEvent.TypeId);
        }

        /// <summary>
        /// Update taxon object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="webTaxon">Web taxon.</param>
        private void UpdateTaxon(IUserContext userContext,
                                 ITaxon taxon,
                                 WebTaxon webTaxon)
        {
            taxon.AlertStatus = CoreData.TaxonManager.GetTaxonAlertStatus(userContext, webTaxon.AlertStatusId);
            taxon.Author = webTaxon.Author;
            taxon.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxon.CategoryId);
            taxon.ChangeStatus = CoreData.TaxonManager.GetTaxonChangeStatus(userContext, webTaxon.ChangeStatusId);
            taxon.CommonName = webTaxon.CommonName;
            taxon.CreatedBy = webTaxon.CreatedBy;
            taxon.CreatedDate = webTaxon.CreatedDate;
            taxon.DataContext = GetDataContext(userContext);
            taxon.Guid = webTaxon.Guid;
            taxon.Id = webTaxon.Id;
            taxon.IsInRevision = webTaxon.IsInRevision;
            if (taxon.IsInRevision)
            {
                taxon.RevisionId = GetTaxonRevisionId(webTaxon);
            }
            taxon.IsPublished = webTaxon.IsPublished;
            taxon.IsValid = webTaxon.IsValid;
            taxon.ModifiedBy = webTaxon.ModifiedBy;
            taxon.ModifiedByPerson = GetModifiedByPerson(webTaxon); //taxon.GetModifiedByPersonFullname(userContext); 
            taxon.ModifiedDate = webTaxon.ModifiedDate;
            taxon.PartOfConceptDefinition = webTaxon.PartOfConceptDefinition;
            taxon.ScientificName = webTaxon.ScientificName;
            taxon.SortOrder = webTaxon.SortOrder;
            taxon.ValidFromDate = webTaxon.ValidFromDate;
            taxon.ValidToDate = webTaxon.ValidToDate;
            taxon.IsMicrospecies = webTaxon.DataFields.GetBoolean(IS_MICROSPECIES);
        }

        /// <summary>
        /// Update TaxonChange object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonChange">TaxonChange.</param>
        /// <param name="webTaxonChange">WebTaxonChange.</param>
        public virtual void UpdateTaxonChange(IUserContext userContext,
                                              ITaxonChange taxonChange,
                                              WebTaxonChange webTaxonChange)
        {
            taxonChange.Category = CoreData.TaxonManager.GetTaxonCategory(userContext, webTaxonChange.TaxonCategoryId);
            taxonChange.EventTypeId = webTaxonChange.TaxonRevisionEventTypeId;
            taxonChange.ScientificName = webTaxonChange.ScientificName;
            taxonChange.TaxonId = webTaxonChange.TaxonId;
            taxonChange.TaxonIdAfter = webTaxonChange.TaxonIdAfter;
        }

        /// <summary>
        /// Update TaxonName object.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonName">taxonName.</param>
        /// <param name="webTaxonName">WebTaxonName.</param>
        private void UpdateTaxonName(IUserContext userContext,
                                     ITaxonName taxonName,
                                     WebTaxonName webTaxonName)
        {
            taxonName.Author = webTaxonName.Author;
            taxonName.Category = CoreData.TaxonManager.GetTaxonNameCategory(userContext,
                                                                                webTaxonName.CategoryId);
            if (webTaxonName.IsChangedInTaxonRevisionEventIdSpecified)
            {
                taxonName.ChangedInTaxonRevisionEventId = webTaxonName.ChangedInTaxonRevisionEventId;
            }
            else
            {
                taxonName.ChangedInTaxonRevisionEventId = null;
            }
            taxonName.CreatedBy = webTaxonName.CreatedBy;
            taxonName.CreatedDate = webTaxonName.CreatedDate;
            taxonName.DataContext = GetDataContext(userContext);
            taxonName.Description = webTaxonName.Description;
            taxonName.Guid = webTaxonName.Guid;
            taxonName.Id = webTaxonName.Id;
            taxonName.IsOkForSpeciesObservation = webTaxonName.IsOkForSpeciesObservation;
            taxonName.IsOriginalName = webTaxonName.IsOriginalName;
            taxonName.IsPublished = webTaxonName.IsPublished;
            taxonName.IsRecommended = webTaxonName.IsRecommended;
            taxonName.IsUnique = webTaxonName.IsUnique;
            taxonName.ModifiedBy = webTaxonName.ModifiedBy;
            taxonName.ModifiedByPerson = GetModifiedByPerson(webTaxonName);
            taxonName.ModifiedDate = webTaxonName.ModifiedDate;
            taxonName.Name = webTaxonName.Name;
            if (webTaxonName.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                taxonName.ReplacedInTaxonRevisionEventId = webTaxonName.ReplacedInTaxonRevisionEventId;
            }
            else
            {
                taxonName.ReplacedInTaxonRevisionEventId = null;
            }
            taxonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(userContext, webTaxonName.StatusId);
            taxonName.NameUsage = CoreData.TaxonManager.GetTaxonNameUsage(userContext, GetNameUsageId(webTaxonName));
            if (webTaxonName.Taxon.IsNotNull())
            {
                taxonName.Taxon = GetTaxon(userContext, webTaxonName.Taxon);
            }
            taxonName.ValidFromDate = webTaxonName.ValidFromDate;
            taxonName.ValidToDate = webTaxonName.ValidToDate;
            taxonName.Version = GetTaxonNameVersion(webTaxonName);
        }

        /// <summary>
        /// Update taxon properties.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonProperties">
        /// Information about the updated taxon properties.
        /// This object is updated with latest taxon properties information.
        /// </param>    
        public virtual void UpdateTaxonProperties(IUserContext userContext,
                                                  ITaxonProperties taxonProperties)
        {
            WebTaxonProperties webTaxonProperties;

            CheckTransaction(userContext);
            webTaxonProperties = WebServiceProxy.TaxonService.UpdateTaxonProperties(GetClientInformation(userContext),
                                                                                    GetTaxonProperties(taxonProperties));
            UpdateTaxonProperties(userContext, taxonProperties, webTaxonProperties);
        }

        /// <summary>
        /// Updates a TaxonProperties object.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonProperties">The TaxonProperties object.</param>
        /// <param name="webTaxonProperties">The WebTaxonProperties object.</param>
        private void UpdateTaxonProperties(IUserContext userContext,
                                           ITaxonProperties taxonProperties,
                                           WebTaxonProperties webTaxonProperties)
        {
            taxonProperties.AlertStatus = (TaxonAlertStatusId)(webTaxonProperties.AlertStatusId);
            if (webTaxonProperties.ChangedInTaxonRevisionEvent.IsNotNull())
            {
                taxonProperties.ChangedInTaxonRevisionEvent = new TaxonRevisionEvent { Id = webTaxonProperties.ChangedInTaxonRevisionEvent.Id };
            }
            taxonProperties.ConceptDefinition = webTaxonProperties.ConceptDefinition;
            taxonProperties.DataContext = GetDataContext(userContext);
            taxonProperties.Id = webTaxonProperties.Id;
            taxonProperties.IsPublished = webTaxonProperties.IsPublished;
            taxonProperties.IsValid = webTaxonProperties.IsValid;
            if (webTaxonProperties.ModifiedBy.IsNotNull())
            {
                taxonProperties.ModifiedBy = new User(userContext) { Id = webTaxonProperties.ModifiedBy.Id };
            }
            taxonProperties.ModifiedByPerson = GetModifiedByPerson(webTaxonProperties);
            taxonProperties.ModifiedDate = webTaxonProperties.ModifiedDate;
            taxonProperties.PartOfConceptDefinition = webTaxonProperties.PartOfConceptDefinition;
            if (webTaxonProperties.ReplacedInTaxonRevisionEvent.IsNotNull())
            {
                taxonProperties.ReplacedInTaxonRevisionEvent = new TaxonRevisionEvent { Id = webTaxonProperties.ReplacedInTaxonRevisionEvent.Id };
            }
            taxonProperties.Taxon = new Taxon { DataContext = GetDataContext(userContext), Id = webTaxonProperties.Taxon.Id };
            if (webTaxonProperties.TaxonCategory.IsNotNull())
            {
                taxonProperties.TaxonCategory = GetTaxonCategory(userContext, webTaxonProperties.TaxonCategory);
            }
            taxonProperties.ValidFromDate = webTaxonProperties.ValidFromDate;
            taxonProperties.ValidToDate = webTaxonProperties.ValidToDate;
            taxonProperties.IsMicrospecies = webTaxonProperties.DataFields.GetBoolean(IS_MICROSPECIES);
        }

        /// <summary>
        /// Update taxon relation.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRelation">
        /// Information about the updated taxon relation.
        /// This object is updated with latest taxon relation information.
        /// </param>    
        public virtual void UpdateTaxonRelation(IUserContext userContext,
                                                ITaxonRelation taxonRelation)
        {
            TaxonList taxa;
            WebTaxonRelation webTaxonRelation;

            CheckTransaction(userContext);
            taxa = new TaxonList();
            taxa.Add(taxonRelation.ChildTaxon);
            taxa.Add(taxonRelation.ParentTaxon);
            webTaxonRelation = WebServiceProxy.TaxonService.UpdateTaxonRelation(GetClientInformation(userContext),
                                                                                GetTaxonRelation(taxonRelation));
            UpdateTaxonRelation(userContext, taxonRelation, webTaxonRelation, taxa);
        }

        /// <summary>
        /// Update taxon relation
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRelation">The taxon relation.</param>
        /// <param name="webTaxonRelation">The web taxon relation.</param>
        /// <param name="taxa">Parent and child taxa.</param>
        /// <returns>
        /// </returns>
        private void UpdateTaxonRelation(IUserContext userContext,
                                         ITaxonRelation taxonRelation,
                                         WebTaxonRelation webTaxonRelation,
                                         TaxonList taxa)
        {
            if (webTaxonRelation.IsChangedInTaxonRevisionEventIdSpecified)
            {
                taxonRelation.ChangedInTaxonRevisionEventId = webTaxonRelation.ChangedInTaxonRevisionEventId;
            }
            else
            {
                taxonRelation.ChangedInTaxonRevisionEventId = null;
            }
            taxonRelation.ChildTaxon = taxa.Get(webTaxonRelation.ChildTaxonId);
            taxonRelation.CreatedBy = webTaxonRelation.CreatedBy;
            taxonRelation.CreatedDate = webTaxonRelation.CreatedDate;
            taxonRelation.DataContext = GetDataContext(userContext);
            taxonRelation.Guid = webTaxonRelation.Guid;
            taxonRelation.Id = webTaxonRelation.Id;
            taxonRelation.IsPublished = webTaxonRelation.IsPublished;
            taxonRelation.IsMainRelation = webTaxonRelation.IsMainRelation;
            taxonRelation.ModifiedBy = webTaxonRelation.ModifiedBy;
            taxonRelation.ModifiedByPerson = GetModifiedByPerson(webTaxonRelation);
            taxonRelation.ModifiedDate = webTaxonRelation.ModifiedDate;
            taxonRelation.ParentTaxon = taxa.Get(webTaxonRelation.ParentTaxonId);
            if (webTaxonRelation.IsReplacedInTaxonRevisionEventIdSpecified)
            {
                taxonRelation.ReplacedInTaxonRevisionEventId = webTaxonRelation.ReplacedInTaxonRevisionEventId;
            }
            else
            {
                taxonRelation.ReplacedInTaxonRevisionEventId = null;
            }
            taxonRelation.SortOrder = webTaxonRelation.SortOrder;
            taxonRelation.ValidToDate = webTaxonRelation.ValidToDate;
            taxonRelation.ValidFromDate = webTaxonRelation.ValidFromDate;
        }

        /// <summary>
        /// Copies values to revision from webRevision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The taxon revision.</param>
        /// <param name="webTaxonRevision">The web taxon revision.</param>
        private void UpdateTaxonRevision(
            IUserContext userContext,
            ITaxonRevision taxonRevision,
            WebTaxonRevision webTaxonRevision)
        {
            const string isSpeciesFactPublishedFieldName = "IsSpeciesFactPublished";
            const string isReferenceRelationsPublishedFieldName = "IsReferenceRelationsPublished";
            taxonRevision.CreatedBy = webTaxonRevision.CreatedBy;
            taxonRevision.CreatedDate = webTaxonRevision.CreatedDate;
            taxonRevision.DataContext = GetDataContext(userContext);
            taxonRevision.Description = webTaxonRevision.Description;
            taxonRevision.ExpectedEndDate = webTaxonRevision.ExpectedEndDate;
            taxonRevision.ExpectedStartDate = webTaxonRevision.ExpectedStartDate;
            taxonRevision.Guid = webTaxonRevision.Guid;
            taxonRevision.Id = webTaxonRevision.Id;
            taxonRevision.ModifiedBy = webTaxonRevision.ModifiedBy;
            taxonRevision.ModifiedDate = webTaxonRevision.ModifiedDate;
            if (webTaxonRevision.RootTaxon.IsNotNull())
            {
                taxonRevision.RootTaxon = GetTaxon(userContext, webTaxonRevision.RootTaxon);
            }
            taxonRevision.State = CoreData.TaxonManager.GetTaxonRevisionState(userContext, webTaxonRevision.StateId);

            if (webTaxonRevision.DataFields != null)
            {
                taxonRevision.IsSpeciesFactPublished = webTaxonRevision.DataFields.GetBoolean(isSpeciesFactPublishedFieldName);                
                if (webTaxonRevision.DataFields.IsDataFieldSpecified(isReferenceRelationsPublishedFieldName))
                {
                    taxonRevision.IsReferenceRelationsPublished = webTaxonRevision.DataFields.GetBoolean(isReferenceRelationsPublishedFieldName);
                }
            }
        }

        /// <summary>
        /// Update taxon revision event.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionEvent">
        /// Information about the updated taxon revision event.
        /// </param>    
        public virtual void UpdateTaxonRevisionEvent(IUserContext userContext,
                                                     ITaxonRevisionEvent taxonRevisionEvent)
        {
            CheckTransaction(userContext);
            WebServiceProxy.TaxonService.UpdateTaxonRevisionEvent(GetClientInformation(userContext),
                                                                  taxonRevisionEvent.Id);
        }

        /// <summary>
        /// Copies data from webobject to domainobject.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevisionEvent">
        /// The revision event.
        /// </param>
        /// <param name="webRevisionEvent">
        /// The web revision event.
        /// </param>
        private void UpdateTaxonRevisionEvent(IUserContext userContext,
                                              ITaxonRevisionEvent taxonRevisionEvent,
                                              WebTaxonRevisionEvent webRevisionEvent)
        {
            taxonRevisionEvent.AffectedTaxa = webRevisionEvent.AffectedTaxa;
            taxonRevisionEvent.CreatedBy = webRevisionEvent.CreatedBy;
            taxonRevisionEvent.CreatedDate = webRevisionEvent.CreatedDate;
            taxonRevisionEvent.DataContext = GetDataContext(userContext);
            taxonRevisionEvent.Id = webRevisionEvent.Id;
            taxonRevisionEvent.NewValue = webRevisionEvent.NewValue;
            taxonRevisionEvent.OldValue = webRevisionEvent.OldValue;
            taxonRevisionEvent.RevisionId = webRevisionEvent.RevisionId;
            taxonRevisionEvent.Type = CoreData.TaxonManager.GetTaxonRevisionEventType(userContext, webRevisionEvent.TypeId);
        }

        /// <summary>
        /// Internal sorting of all taxa w/ same parent taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonIdParent">Id of the parent taxon.</param>
        /// <param name="taxonIdChildren">Sorted list of taxa ids.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        /// <returns></returns>
        public virtual void UpdateTaxonTreeSortOrder(IUserContext userContext,
                                                     Int32 taxonIdParent,
                                                     List<Int32> taxonIdChildren,
                                                     ITaxonRevisionEvent taxonRevisionEvent)
        {
            WebServiceProxy.TaxonService.UpdateTaxonTreeSortOrder(GetClientInformation(userContext), taxonIdParent, taxonIdChildren, taxonRevisionEvent.Id);
            // Update the RevisionEvent
            if (taxonRevisionEvent.IsNotNull())
            {
                WebServiceProxy.TaxonService.UpdateTaxonRevisionEvent(GetClientInformation(userContext), taxonRevisionEvent.Id);
            }
        }
    }
}
