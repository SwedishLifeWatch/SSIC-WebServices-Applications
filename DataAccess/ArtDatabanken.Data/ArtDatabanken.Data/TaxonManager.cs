using System;
using System.Linq;
using System.Collections.Generic;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles taxon related information.
    /// </summary>
    public class TaxonManager : ITaxonManager
    {
        /// <summary>
        /// This property is used to retrieve or update taxon information.
        /// </summary>
        public ITaxonDataSource DataSource { get; set; }

        /// <summary>
        /// This property is used to retrieve taxon information from PESI.
        /// </summary>
        public IPesiNameDataSource PesiNameDataSource { get; set; }

        /// <summary>Check in a revision.</summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision to check in.</param>
        public void CheckInTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            DataSource.CheckInTaxonRevision(userContext, taxonRevision);
        }

        /// <summary>
        /// Check out a revision.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void CheckOutTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            DataSource.CheckOutTaxonRevision(userContext, taxonRevision);      
        }

        private Boolean CheckTaxon(IUserContext userContext, ITaxon taxon)
        {
            if (taxon.Id != 0 && taxon.Id != Int32.MinValue)
            {
                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid)
                {
                    // Initialize parentTaxa
                    var numberOfMainRelations =
                        (from rel in taxon.GetCheckedOutChangesParentTaxa(userContext) where rel.IsMainRelation == true select rel).
                            Count();

                    if (numberOfMainRelations != 1)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Check data in list of taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="newTaxonNames">List of edited or new TaxonName objects.</param>
        /// <returns>true - if data in taxon name list is ok
        ///          false - if data in taxon name list is not valid
        /// </returns>
        private Boolean CheckTaxonNames(IUserContext userContext,
                                        TaxonNameList newTaxonNames)
        {
            ITaxonName changedTaxonName;
            foreach (TaxonName taxonName in newTaxonNames)
            {
                if (taxonName.IsRecommended)
                {
                    changedTaxonName = GetTaxonNameByVersion(userContext, taxonName.Version);
                    //                    changedTaxonName = DataSource.GetTaxonNameById(userContext, taxonName.Id);
                    // Check if TaxonName has changed category
                    if (!taxonName.Category.Id.Equals(changedTaxonName.Category.Id))
                    {
                        // Check if there is another recommended name in this name category.
                        TaxonNameSearchCriteria searchCriteria = new TaxonNameSearchCriteria();
                        searchCriteria.IsRecommended = true;
                        searchCriteria.Category = taxonName.Category;
                        List<Int32> taxonIds = new List<Int32>();
                        taxonIds.Add(taxonName.Taxon.Id);
                        searchCriteria.TaxonIds = taxonIds;
                        TaxonNameList taxonNames = GetTaxonNames(userContext, searchCriteria);
                        // if other name was found - return false
                        if (taxonNames.IsNotEmpty())
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Call to PESI WebService to get GUID for taxon.
        /// Match by taxon scientific name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        public void CreatePESIData(IUserContext userContext, ITaxon taxon, ITaxonRevisionEvent taxonRevisionEvent)
        {
            String pesiGUID = PesiNameDataSource.GetPesiGuidByScientificName(taxon.ScientificName);
            if (pesiGUID.IsNotEmpty())
            {
                var taxonNameCategory = GetTaxonNameCategory(userContext, 16);

                // create the PESI GUID as TaxonName identifier
                var newIdentifier = new TaxonName();
                newIdentifier.DataContext = new DataContext(userContext);
                newIdentifier.Name = pesiGUID;
                newIdentifier.Taxon = (taxon);
                newIdentifier.Description = "PESI identifier";
                newIdentifier.Category = (TaxonNameCategory)taxonNameCategory;
                newIdentifier.Status = GetTaxonNameStatus(userContext, TaxonNameStatusId.ApprovedNaming);
                newIdentifier.NameUsage = GetTaxonNameUsage(userContext, TaxonNameUsageId.Accepted);
                newIdentifier.IsOkForSpeciesObservation = true;
                newIdentifier.IsPublished = false;
                newIdentifier.IsRecommended = true;
                newIdentifier.IsUnique = true;
                newIdentifier.SetChangedInRevisionEvent(taxonRevisionEvent);
                newIdentifier.SetReferences(new List<IReferenceRelation>());

                // add a PESI reference to the GUID.
                IReferenceRelation referenceRelation = new ReferenceRelation();
                referenceRelation.Reference = null;
                referenceRelation.ReferenceId = Settings.Default.PESIReferenceId;
                referenceRelation.Type = CoreData.ReferenceManager.GetReferenceRelationType(userContext, ReferenceRelationTypeId.Source);
                
                TaxonNameList taxonNamesList = new TaxonNameList();
                taxonNamesList.Add(newIdentifier);
                DataSource.SaveTaxonNames(userContext, taxonNamesList, (TaxonRevisionEvent)taxonRevisionEvent);

                referenceRelation.RelatedObjectGuid = taxonNamesList[0].Guid;
                CoreData.ReferenceManager.CreateReferenceRelation(userContext, referenceRelation);
            }
        }

        /// <summary>
        /// Creates a taxon, with revisionevent
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonRevision"> The revision.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="scientificName">Scientific name.</param>
        /// <param name="commonName">Common name.</param>
        /// <param name="author">Author.</param>
        /// <param name="alertStatusId">Alert status id.</param>
        /// <param name="parentTaxon">Parent taxon.</param>
        /// <param name="taxonCategory">The taxon category.</param>
        /// <param name="conceptDefinition">Concept definition.</param>
        public void CreateTaxon(IUserContext userContext,
                                ITaxonRevision taxonRevision,
                                ITaxon taxon,
                                String scientificName,
                                String commonName,
                                String author,
                                TaxonAlertStatusId alertStatusId,
                                ITaxon parentTaxon,
                                ITaxonCategory taxonCategory,
                                String conceptDefinition)
        {
            ITaxonRevisionEvent taxonRevisionEvent;

            // Check revision
            if (taxonRevision.State.Id != (int)TaxonRevisionStateId.Ongoing)
            {
                throw new Exception("Revision is not checked out for edit");
            }

            // Check that parent taxon is in revision
            if (!parentTaxon.IsInRevision)
            {
                throw new ArgumentException("Parent taxon is not in revision.");
            }

            // check scientific name
            if (scientificName.IsEmpty() || scientificName.IsNull())
            {
                throw new ArgumentException("Taxon - Scientific name is mandatory.");
            }


                // Create revisionvent
                CreateTaxonRevisionEvent(userContext, taxonRevision, 1);

                // Create a new Taxon
                taxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();
                taxon.Category = taxonCategory;

                var newProp = new TaxonProperties()
                {
                    ChangedInTaxonRevisionEvent = taxonRevisionEvent,
                    DataContext = new DataContext(userContext),
                    Taxon = taxon,
                    IsPublished = false,
                    IsValid = true,
                    TaxonCategory = taxonCategory,
                    ModifiedBy = userContext.User,
                    ModifiedDate = DateTime.Now,
                    AlertStatus = alertStatusId,
                    PartOfConceptDefinition = conceptDefinition,
                    ValidFromDate = DateTime.Now,
                };
                taxon.GetTaxonProperties(userContext).Add(newProp);

                // Create one main taxonrelation
                var newRelation = new TaxonRelation();
                newRelation.CreatedBy = userContext.User.Id;
                newRelation.CreatedDate = DateTime.Now;
                newRelation.IsPublished = false;
                newRelation.ParentTaxon = parentTaxon;
                newRelation.ChildTaxon = taxon;
                newRelation.IsMainRelation = true;
                newRelation.SortOrder = 1;
                if (taxonRevisionEvent.IsNull())
                {
                    newRelation.ChangedInTaxonRevisionEventId = null;
                }
                else
                {
                    newRelation.ChangedInTaxonRevisionEventId = taxonRevisionEvent.Id;
                }
                newRelation.ValidFromDate = DateTime.Now;
                List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();
                taxonRelations.Add(newRelation);

                // Save the taxon.
                UpdateTaxon(userContext, taxon, taxonRevisionEvent, null, taxonRelations);
               
                // Create two new names               
                TaxonNameList taxonNames = new TaxonNameList();
                var newScientificName = new TaxonName();
                newScientificName.DataContext = new DataContext(userContext);
                newScientificName.Name = scientificName;
                newScientificName.Taxon = taxon;
                newScientificName.Description = String.Empty;
                newScientificName.Category = GetTaxonNameCategory(userContext, 0);
                newScientificName.Status = GetTaxonNameStatus(userContext, TaxonNameStatusId.ApprovedNaming);
                newScientificName.NameUsage = GetTaxonNameUsage(userContext, TaxonNameUsageId.Accepted);
                newScientificName.IsOkForSpeciesObservation = true;
                newScientificName.IsPublished = false;
                newScientificName.IsRecommended = true;
                newScientificName.IsUnique = true;
                newScientificName.ModifiedByPerson = author;
                newScientificName.SetChangedInRevisionEvent(null);
                newScientificName.SetChangedInRevisionEvent(taxonRevisionEvent);
                newScientificName.Author = author;
                newScientificName.ValidFromDate = DateTime.Now;
                taxonNames.Add(newScientificName);

                var taxonNameCategories = GetTaxonNameCategories(userContext);
                ITaxonNameCategory commonTaxonNameCategory = new TaxonNameCategory();
                commonTaxonNameCategory.Id = Int32.MinValue;
                foreach (ITaxonNameCategory taxonNameCategory in taxonNameCategories)
                {
                    if (taxonNameCategory.Locale.IsNotNull() &&
                        taxonNameCategory.Locale.Id == userContext.Locale.Id)
                    {
                        commonTaxonNameCategory = taxonNameCategory;
                        break;
                    }
                }
                
                if (commonName.IsNotNull() && commonName.IsNotEmpty())
                {
                    var newCommonName = new TaxonName();
                    newCommonName.DataContext = new DataContext(userContext);
                    newCommonName.Name = commonName;
                    newCommonName.Taxon = taxon;
                    newCommonName.Description = String.Empty;
                    newCommonName.Category = commonTaxonNameCategory;
                    newCommonName.Status = GetTaxonNameStatus(userContext, TaxonNameStatusId.ApprovedNaming);
                    newCommonName.NameUsage = GetTaxonNameUsage(userContext, TaxonNameUsageId.Accepted);
                    newCommonName.IsOkForSpeciesObservation = true;
                    newCommonName.IsPublished = false;
                    newCommonName.IsRecommended = true;
                    newCommonName.IsUnique = true;
                    newCommonName.SetChangedInRevisionEvent(null);
                    newCommonName.SetChangedInRevisionEvent(taxonRevisionEvent);
                    newCommonName.ValidFromDate = DateTime.Now;
                    taxonNames.Add(newCommonName);
                }

                DataSource.SaveTaxonNames(userContext, taxonNames, taxonRevisionEvent);
        }

        /// <summary>
        /// Create a taxon name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">Taxon name.</param>
        public void CreateTaxonName(IUserContext userContext, ITaxonRevision taxonRevision, ITaxonName taxonName)
        {
            /* BEHÖVS NÅGON VALIDERING HÄR ? GuNy 2012-04-25
            if (!taxonName.IsRecommended)
            {
                List<ITaxonName> addedTaxonName = new List<ITaxonName>();
                addedTaxonName.Add(taxonName);
                if (!CheckTaxonNames(userContext, addedTaxonName))
                {
                    throw new ArgumentException("One recommended scientific name is mandatory.");
                }
            }
             */
            SaveTaxonName(userContext, taxonRevision, taxonName, TaxonRevisionEventTypeId.AddTaxonName);
        }

        /// <summary>
        /// Creates the revision event.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="revisionEventTypeId">The revision event type id.</param>
        private void CreateTaxonRevisionEvent(IUserContext userContext, ITaxonRevision taxonRevision, int revisionEventTypeId)
        {
            // Check revision
            if (taxonRevision.State.Id != (int)TaxonRevisionStateId.Ongoing)
            {
                throw new Exception("Revision is not checked out for edit");
            }

            // Create revisionvent
            ITaxonRevisionEvent taxonRevisionEvent = new TaxonRevisionEvent()
            {
                Type = new TaxonRevisionEventType() { Id = revisionEventTypeId },
                CreatedDate = DateTime.Now,
                CreatedBy = userContext.User.Id,
                RevisionId = taxonRevision.Id
            };
            taxonRevision.GetRevisionEvents(userContext).Add(taxonRevisionEvent);

            // Save the revision event
            // DataSource.SaveRevision(userContext, revision);
            DataSource.CreateTaxonRevisionEvent(userContext, taxonRevisionEvent);
        }


        /// <summary>
        /// Removes the taxon by setting TaxonProperty.IsValid = false
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void DeleteTaxon(IUserContext userContext, ITaxon taxon, ITaxonRevision taxonRevision)
        {
            List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();
            // Create revisionvent
            CreateTaxonRevisionEvent(userContext, taxonRevision, 2);

            // Create a new TaxonProperty object
            // TODO Här måste vi ha med ConceptDefinitionStrings + GUID + ReferenceIds + AlertStatus
            var newProp = new TaxonProperties()
            {
                ChangedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last(),
                DataContext = new DataContext(userContext),
                Taxon = taxon,
                IsPublished = false,
                IsValid = false,
                TaxonCategory = taxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory,
                ModifiedBy = userContext.User,
                ModifiedDate = DateTime.Now,
                AlertStatus = TaxonAlertStatusId.Red,
                PartOfConceptDefinition = taxon.GetCheckedOutChangesTaxonProperties(userContext).PartOfConceptDefinition,
                ReplacedInTaxonRevisionEvent = null,
                ValidFromDate = DateTime.Now,
                ValidToDate = DateTime.Now.AddYears(40)
            };

            // Flag the currently valid TaxonProperty object so it will be invalidated on checkin
            taxon.GetCheckedOutChangesTaxonProperties(userContext).ReplacedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            // Add the new TaxonProperty object
            taxon.GetTaxonProperties(userContext).Add(newProp);

            // Expire all taxonrelations of the invalid taxon. They should now be treated as historical
            foreach (var parentRelation in taxon.GetCheckedOutChangesParentTaxa(userContext))
            {
                ITaxonRevisionEvent taxonRevisionEvent;

                taxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();
                if (taxonRevisionEvent.IsNull())
                {
                    parentRelation.ReplacedInTaxonRevisionEventId = null;
                }
                else
                {
                    parentRelation.ReplacedInTaxonRevisionEventId = taxonRevisionEvent.Id;
                }
                taxonRelations.Add(parentRelation);
            }

            // Save the taxon.
            this.UpdateTaxon(userContext, taxon, taxonRevision.GetRevisionEvents(userContext).Last(), null, taxonRelations);
        }

        /// <summary>
        /// Deletes a name of the taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">TaxonName object.</param>
        public void DeleteTaxonName(IUserContext userContext, ITaxonRevision taxonRevision, ITaxonName taxonName)
        {
            SaveTaxonName(userContext, taxonRevision, taxonName, TaxonRevisionEventTypeId.DeleteTaxonName);   
        }

        /// <summary>
        /// Delete a taxon revision.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">Taxon revision.</param>
        public void DeleteTaxonRevision(IUserContext userContext,
                                        ITaxonRevision taxonRevision)
        {
            if (taxonRevision.State.Id != (Int32)(TaxonRevisionStateId.Created))
            {
                throw new Exception("Revision was already started. Can't be deleted!");
            }

            DataSource.DeleteTaxonRevision(userContext, taxonRevision);
        }

        /// <summary>
        /// Rolls back all changes for one revisionevent
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevisionEvent">The revision event.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void DeleteTaxonRevisionEvent(IUserContext userContext, ITaxonRevisionEvent taxonRevisionEvent, ITaxonRevision taxonRevision)
        {
            // Check that revision is checked out
            if (taxonRevision.State.Id != (int)TaxonRevisionStateId.Ongoing)
            {
                throw new Exception("Revision is not checked out for edit");
            }

            // Check that no more recent events exists for this revision
            if (taxonRevision.GetRevisionEvents(userContext).Last().Id != taxonRevisionEvent.Id)
            {
                throw new Exception("Revisionevents can only be undone in reversed creation order");
            }
            // Delete the event, all related newly created rows and remove flag from rows that will be invalidated by the newly created rows. 
            // All updates happening on db level, updates REVISION read back.
            DataSource.DeleteTaxonRevisionEvent(userContext, taxonRevisionEvent, taxonRevision);
        }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get lumpsplitevent by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="lumpSplitEventGuid">GUID.</param>
        /// <returns>Requested object.</returns>       
        public ILumpSplitEvent GetLumpSplitEvent(IUserContext userContext, string lumpSplitEventGuid)
        {
            return DataSource.GetLumpSplitEvent(userContext, lumpSplitEventGuid);
        }

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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public LumpSplitEventList GetLumpSplitEventsByNewReplacingTaxon(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetLumpSplitEventsByNewReplacingTaxon(userContext, taxon);
        }

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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public LumpSplitEventList GetLumpSplitEventsByOldReplacedTaxon(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetLumpSplitEventsByOldReplacedTaxon(userContext, taxon);
        }

        /// <summary>
        /// Get specified lump split event type.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="lumpSplitEventTypeId">Id for lump split event type.</param>
        /// <returns>Specified lump split event type.</returns>
        public virtual ILumpSplitEventType GetLumpSplitEventType(IUserContext userContext,
                                                                 Int32 lumpSplitEventTypeId)
        {
            return GetLumpSplitEventTypes(userContext).Get(lumpSplitEventTypeId);
        }

        /// <summary>
        /// Get specified lump split event type.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="lumpSplitEventTypeId">Id for lump split event type.</param>
        /// <returns>Specified lump split event type.</returns>
        public virtual ILumpSplitEventType GetLumpSplitEventType(IUserContext userContext,
                                                                 LumpSplitEventTypeId lumpSplitEventTypeId)
        {
            return GetLumpSplitEventType(userContext, (Int32)lumpSplitEventTypeId);
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All lump split event types.</returns>
        public virtual LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext)
        {
            return DataSource.GetLumpSplitEventTypes(userContext);
        }

        /// <summary>
        /// t
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        public virtual TaxonList GetTaxa(IUserContext userContext, ITaxonSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxa(userContext, searchCriteria);
        }

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonIds">Taxon ids</param>
        /// <returns>Taxa with specified ids.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         List<Int32> taxonIds)
        {
            return DataSource.GetTaxa(userContext, taxonIds);
        }

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonIds">Taxon ids</param>
        /// <returns>Taxa with specified ids.</returns>
        public virtual TaxonList GetTaxa(IUserContext userContext,
                                         List<TaxonId> taxonIds)
        {
            List<Int32> ids;

            ids = new List<Int32>();
            if (taxonIds.IsNotEmpty())
            {
                foreach (TaxonId taxonId in taxonIds)
                {
                    ids.Add((Int32)taxonId);
                }
            }

            return GetTaxa(userContext, ids);
        }

        /// <summary>
        /// Get taxon by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonGuid">GUID.</param>
        /// <returns>Requested taxon.</returns>       
        public virtual ITaxon GetTaxon(IUserContext userContext, String taxonGuid)
        {
            return DataSource.GetTaxon(userContext, taxonGuid);
        }

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>       
        public virtual ITaxon GetTaxon(IUserContext userContext,
                                       Int32 taxonId)
        {
            return DataSource.GetTaxon(userContext, taxonId);
        }

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>       
        public virtual ITaxon GetTaxon(IUserContext userContext,
                                       TaxonId taxonId)
        {
            return GetTaxon(userContext, (Int32)taxonId);
        }

        /// <summary>
        /// Get taxon alert status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonAlertStatusId">Id for requested taxon alert status.</param>
        /// <returns>Taxon alert status with specified id.</returns>
        public virtual ITaxonAlertStatus GetTaxonAlertStatus(IUserContext userContext,
                                                             Int32 taxonAlertStatusId)
        {
            return GetTaxonAlertStatuses(userContext).Get(taxonAlertStatusId);
        }

        /// <summary>
        /// Get taxon alert status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonAlertStatusId">Id for requested taxon alert status.</param>
        /// <returns>Taxon alert status with specified id.</returns>
        public virtual ITaxonAlertStatus GetTaxonAlertStatus(IUserContext userContext,
                                                             TaxonAlertStatusId taxonAlertStatusId)
        {
            return GetTaxonAlertStatus(userContext, (Int32)taxonAlertStatusId);
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon alert statuses.</returns>
        public virtual TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext)
        {
            return DataSource.GetTaxonAlertStatuses(userContext);
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon categories.</returns>       
        public virtual TaxonCategoryList GetTaxonCategories(IUserContext userContext)
        {
            return DataSource.GetTaxonCategories(userContext);
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
        public TaxonCategoryList GetTaxonCategories(IUserContext userContext,
                                                    ITaxon taxon)
        {
            return DataSource.GetTaxonCategories(userContext, taxon);
        }

        /// <summary>
        /// Get taxon category with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonCategoryId">Id of taxon category to be retrived.</param>
        /// <returns>Taxon category with specified id.</returns>
        public ITaxonCategory GetTaxonCategory(IUserContext userContext,
                                               Int32 taxonCategoryId)
        {
            return GetTaxonCategories(userContext).Get(taxonCategoryId);
        }

        /// <summary>
        /// Get taxon category by specified name
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ITaxonCategory GetTaxonCategory(IUserContext userContext, string name)
        {
            return GetTaxonCategories(userContext).FirstOrDefault(x => x.Name == name);
        }

        /// <summary>
        /// Get taxon category with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonCategoryId">Id of taxon category to be retrived.</param>
        /// <returns>Taxon category with specified id.</returns>
        public ITaxonCategory GetTaxonCategory(IUserContext userContext,
                                               TaxonCategoryId taxonCategoryId)
        {
            return GetTaxonCategory(userContext, (Int32)taxonCategoryId);
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
        public TaxonChangeList GetTaxonChange(IUserContext userContext,
                                              ITaxon rootTaxon,
                                              DateTime dateFrom,
                                              DateTime dateTo)
        {
            return DataSource.GetTaxonChange(userContext, rootTaxon, dateFrom, dateTo);
        }

        /// <summary>
        /// Get taxon change status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonChangeStatusId">Id for requested taxon change status.</param>
        /// <returns>Taxon change status with specified id.</returns>
        public virtual ITaxonChangeStatus GetTaxonChangeStatus(IUserContext userContext,
                                                               Int32 taxonChangeStatusId)
        {
            return GetTaxonChangeStatuses(userContext).Get(taxonChangeStatusId);
        }

        /// <summary>
        /// Get taxon change status with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonChangeStatusId">Id for requested taxon change status.</param>
        /// <returns>Taxon change status with specified id.</returns>
        public virtual ITaxonChangeStatus GetTaxonChangeStatus(IUserContext userContext,
                                                               TaxonChangeStatusId taxonChangeStatusId)
        {
            return GetTaxonChangeStatus(userContext, (Int32)taxonChangeStatusId);
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon change statuses.</returns>
        public virtual TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext)
        {
            return DataSource.GetTaxonChangeStatuses(userContext);
        }

        /// <summary>
        /// Get a list of taxon quality summary. 
        /// </summary>
        /// <param name="userContext">UserContext</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>List of ITaxonQualitySummary with taxa quality summary.</returns>
        public virtual TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext,
                                                                                      ITaxon taxon)
        {
            return DataSource.GetTaxonChildQualityStatistics(userContext, taxon);
        }

        /// <summary>
        /// Get a list of taxon statistics. 
        /// </summary>
        /// <param name="userContext">UserContext</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>List of ITaxonStatistics with taxa statistics.</returns>
        public TaxonChildStatisticsList GetTaxonChildStatistics(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetTaxonChildStatistics(userContext, taxon);
        }

        /// <summary>
        /// Get concept definition string for specified taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxon">Taxon.</param>
        /// <returns>Concept definition string for specified taxon.</returns>
        public String GetTaxonConceptDefinition(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetTaxonConceptDefinition(userContext, taxon);
        }

        /// <summary>
        /// Get taxon name by Id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameId">Id for TaxonName.</param>
        /// <returns>Requested object.</returns>       
        public ITaxonName GetTaxonName(IUserContext userContext, Int32 taxonNameId)
        {
            return DataSource.GetTaxonName(userContext, taxonNameId);
        }

        /// <summary>
        /// Get taxonname by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameGuid">GUID.</param>
        /// <returns>Requested object.</returns>       
        public ITaxonName GetTaxonName(IUserContext userContext, String taxonNameGuid)
        {
            return DataSource.GetTaxonName(userContext, taxonNameGuid);
        }

        /// <summary>
        /// Get taxon name by version.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameVersion">Taxon name version.</param>
        /// <returns>Taxon name with specified version.</returns>
        private ITaxonName GetTaxonNameByVersion(IUserContext userContext,
                                                 Int32 taxonNameVersion)
        {
            String guid;

            guid = "urn:lsid:dyntaxa.se:TaxonName:-1:" + taxonNameVersion;
            return GetTaxonName(userContext, guid);
        }

        /// <summary>
        /// Get a list of all taxon name categories ie TaxonNameCategoryList class. 
        /// </summary>
        /// <param name="userContext">Usercontext</param>
        /// <returns>TaxonCategoryList with all taxon calegories.</returns>
        public virtual TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext)
        {
            return DataSource.GetTaxonNameCategories(userContext);
        }

        /// <summary>
        /// Get taxon name category with specified id.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonNameCategoryId">Id of taxon name category to be retrived.</param>
        /// <returns>Taxon name category with specified id.</returns>
        public ITaxonNameCategory GetTaxonNameCategory(IUserContext userContext,
                                                       Int32 taxonNameCategoryId)
        {
            return GetTaxonNameCategories(userContext).Get(taxonNameCategoryId);
        }

        /// <summary>
        /// Get taxon name category type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameCategoryTypeId">Id for requested taxon category type.</param>
        /// <returns>Taxon name category type with specified id.</returns>
        public virtual ITaxonNameCategoryType GetTaxonNameCategoryType(IUserContext userContext,
                                                                       Int32 taxonNameCategoryTypeId)
        {
            return GetTaxonNameCategoryTypes(userContext).Get(taxonNameCategoryTypeId);
        }

        /// <summary>
        /// Get taxon name category type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonNameCategoryTypeId">Id for requested taxon category type.</param>
        /// <returns>Taxon name category type with specified id.</returns>
        public virtual ITaxonNameCategoryType GetTaxonNameCategoryType(IUserContext userContext,
                                                                       TaxonNameCategoryTypeId taxonNameCategoryTypeId)
        {
            return GetTaxonNameCategoryType(userContext, (Int32)taxonNameCategoryTypeId);
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon name category types.</returns>
        public virtual TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext)
        {
            return DataSource.GetTaxonNameCategoryTypes(userContext);
        }

        /// <summary>
        /// Search taxonnames by search critera
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="searchCriteria">The TaxonNameSearchCriteria object</param>
        /// <returns>
        /// List of taxonnames.
        /// </returns>
        public TaxonNameList GetTaxonNames(IUserContext userContext,
                                           ITaxonNameSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxonNames(userContext, searchCriteria);
        }

        /// <summary>
        /// Get all taxon names for taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>All taxon names for taxon.</returns>
        public virtual TaxonNameList GetTaxonNames(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetTaxonNames(userContext, taxon);
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
            return DataSource.GetTaxonNames(userContext, taxa);
        }

        /// <summary>
        /// Get taxon name status with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameStatusId">Taxon name status id.</param>
        /// <returns>Taxon name status with specified id.</returns>
        public virtual ITaxonNameStatus GetTaxonNameStatus(IUserContext userContext,
                                                           Int32 taxonNameStatusId)
        {
            return GetTaxonNameStatuses(userContext).Get(taxonNameStatusId);
        }

        /// <summary>
        /// Get taxon name status with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameStatusId">Taxon name status id.</param>
        /// <returns>Taxon name status with specified id.</returns>
        public virtual ITaxonNameStatus GetTaxonNameStatus(IUserContext userContext,
                                                           TaxonNameStatusId taxonNameStatusId)
        {
            return GetTaxonNameStatus(userContext, (Int32)taxonNameStatusId);
        }

        /// <summary>
        /// Get information about possbile status for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>Information about possbile status for taxon names.</returns>
        public virtual TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext)
        {
            return DataSource.GetTaxonNameStatuses(userContext);
        }

        /// <summary>
        /// Get taxon name usage with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameUsageId">Taxon name usage id.</param>
        /// <returns>
        /// Taxon name status with specified id.
        /// </returns>        
        public virtual ITaxonNameUsage GetTaxonNameUsage(IUserContext userContext, int taxonNameUsageId)
        {
            return GetTaxonNameUsages(userContext).Get(taxonNameUsageId);
        }

        /// <summary>
        /// Get taxon name usage with specified id.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonNameUsageId">Taxon name usage id.</param>
        /// <returns>
        /// Taxon name usage with specified id.
        /// </returns>        
        public virtual ITaxonNameUsage GetTaxonNameUsage(IUserContext userContext, TaxonNameUsageId taxonNameUsageId)
        {
            return GetTaxonNameUsage(userContext, (Int32)taxonNameUsageId);
        }

        /// <summary>
        /// Get information about possible usage for taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>
        /// Information about possible usage for taxon names.
        /// </returns>
        public virtual TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext)
        {
            return DataSource.GetTaxonNameUsages(userContext);
        }

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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public TaxonPropertiesList GetTaxonProperties(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetTaxonProperties(userContext, taxon);
        }

        /// <summary>
        /// Get taxon relations that matches search criteria.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">Search criteria.</param>
        /// <returns>Taxon relations that matches search criteria.</returns>
        public TaxonRelationList GetTaxonRelations(IUserContext userContext,
                                                                   ITaxonRelationSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxonRelations(userContext, searchCriteria);
        }

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
        public ITaxonRevision GetTaxonRevision(IUserContext userContext, Int32 taxonRevisionId)
        {
            return DataSource.GetTaxonRevision(userContext, taxonRevisionId);
        }

        /// <summary>
        /// Get revision by GUID.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionGuid">GUID.</param>
        /// <returns>Requested object.</returns>       
        public ITaxonRevision GetTaxonRevision(IUserContext userContext, String taxonRevisionGuid)
        {
            return DataSource.GetTaxonRevision(userContext, taxonRevisionGuid);
        }

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
        /// <exception cref="NotImplementedException">
        /// </exception>
        public ITaxonRevisionEvent GetTaxonRevisionEvent(IUserContext userContext, int taxonRevisionEventId)
        {
            return DataSource.GetTaxonRevisionEvent(userContext, taxonRevisionEventId);
        }

        /// <summary>
        /// Get revision event selected ny revision id.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevisionId">Revision id</param>
        /// <returns>Revision event list selected by revision.</returns>
        public virtual TaxonRevisionEventList GetTaxonRevisionEvents(IUserContext userContext, Int32 taxonRevisionId)
        {
            return DataSource.GetTaxonRevisionEvents(userContext, taxonRevisionId);
        }

        /// <summary>
        /// Get taxon revision event type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionEventTypeId">Id for taxon revision event type.</param>
        /// <returns>Requested taxon revision event type.</returns>
        public virtual ITaxonRevisionEventType GetTaxonRevisionEventType(IUserContext userContext,
                                                                         Int32 taxonRevisionEventTypeId)
        {
            return GetTaxonRevisionEventTypes(userContext).Get(taxonRevisionEventTypeId);
        }

        /// <summary>
        /// Get taxon revision event type with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionEventTypeId">Id for taxon revision event type.</param>
        /// <returns>Requested taxon revision event type.</returns>
        public virtual ITaxonRevisionEventType GetTaxonRevisionEventType(IUserContext userContext,
                                                                         TaxonRevisionEventTypeId taxonRevisionEventTypeId)
        {
            return GetTaxonRevisionEventType(userContext, (Int32)taxonRevisionEventTypeId);
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision event types.</returns>
        public virtual TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext)
        {
            return DataSource.GetTaxonRevisionEventTypes(userContext);
        }

        /// <summary>
        /// Get revisons by search criteria
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="searchCriteria">Serack criteria to search revisions for</param>
        /// <returns></returns>
        public TaxonRevisionList GetTaxonRevisions(IUserContext userContext, ITaxonRevisionSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxonRevisions(userContext, searchCriteria);
        }

        /// <summary>
        /// Get all revisions that affected a taxon or its childtaxa.
        /// </summary>
        /// <param name="userContext"> The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>A list of revisions.</returns>
        public TaxonRevisionList GetTaxonRevisions(IUserContext userContext, ITaxon taxon)
        {
            return DataSource.GetTaxonRevisions(userContext, taxon);
        }

        /// <summary>
        /// Get taxon revision state with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionStateId">Id for taxon revision state.</param>
        /// <returns>Requested taxon revision state.</returns>
        public virtual ITaxonRevisionState GetTaxonRevisionState(IUserContext userContext,
                                                                 Int32 taxonRevisionStateId)
        {
            return GetTaxonRevisionStates(userContext).Get(taxonRevisionStateId);
        }

        /// <summary>
        /// Get taxon revision state with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonRevisionStateId">Id for taxon revision state.</param>
        /// <returns>Requested taxon revision state.</returns>
        public virtual ITaxonRevisionState GetTaxonRevisionState(IUserContext userContext,
                                                                 TaxonRevisionStateId taxonRevisionStateId)
        {
            return GetTaxonRevisionState(userContext, (Int32)taxonRevisionStateId);
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision states.</returns>
        public virtual TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext)
        {
            return DataSource.GetTaxonRevisionStates(userContext);
        }

        /// <summary>
        /// Get information about taxon trees that matches the search criteria.
        /// All taxon tree nodes without parents are returned
        /// if no taxon ids are specified.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="searchCriteria">The taxon tree search criteria.</param>
        /// <returns>Taxon tree information.</returns>
        public TaxonTreeNodeList GetTaxonTrees(IUserContext userContext,
                                               ITaxonTreeSearchCriteria searchCriteria)
        {
            return DataSource.GetTaxonTrees(userContext, searchCriteria);
        }

        /// <summary>
        /// Test if it is ok to lump taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxa">Taxa.</param>
        /// <param name="taxonAfter">Taxon after.</param>
        /// <returns>True, if it is ok to lump taxon.</returns>
        public Boolean IsOkToLumpTaxa(IUserContext userContext,
                                      TaxonList taxa,
                                      ITaxon taxonAfter)
        {
            var category = taxa[0].GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;

            foreach (ITaxon taxon in taxa)
            {
                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != category.Id)
                {
                    return false;
                }

                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                {
                    return false;
                }

                if (!taxon.IsInRevision)
                {
                    return false;
                }

                if (taxon.GetNearestChildTaxonRelations(userContext).Count > 0)
                {
                    return false;
                }
            }

            if (taxonAfter.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Test if it is ok to split taxon.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonBefore">Taxon before.</param>
        /// <param name="taxa">Taxa.</param>
        /// <returns>True, if it is ok to split taxon.</returns>
        public Boolean IsOkToSplitTaxon(IUserContext userContext,
                                        ITaxon taxonBefore,
                                        TaxonList taxa)
        {

            var category = taxa[0].GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;

            foreach (ITaxon taxon in taxa)
            {
                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.Id != category.Id)
                {
                    return false;
                }

                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
                {
                    return false;
                }

                if (!taxon.IsInRevision)
                {
                    return false;
                }
            }

            if (taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).IsValid == false)
            {
                return false;
            }

            return true;

        }

        /// <summary>
        /// Checks if values of data (that can be set) in taxonName object are changed.
        /// </summary>
        /// <param name="changedTaxonName">The edited taxonName object.</param>
        /// <param name="originalTaxonName">The original taxonName object.</param>
        /// <returns>true - if values are not the same in changed vs. orginal 
        ///          false - if values in changed and original are the same
        /// </returns>
        private Boolean IsTaxonNameChanged(ITaxonName changedTaxonName, ITaxonName originalTaxonName)
        {
            if (String.Compare(changedTaxonName.Author, originalTaxonName.Author, true) != 0) { return true; }
            if (String.Compare(changedTaxonName.Description, originalTaxonName.Description, true) != 0) { return true; }
            if (!changedTaxonName.IsOkForSpeciesObservation.Equals(originalTaxonName.IsOkForSpeciesObservation)) { return true; }
            if (!changedTaxonName.IsOriginalName.Equals(originalTaxonName.IsOriginalName)) { return true; }
            if (!changedTaxonName.IsRecommended.Equals(originalTaxonName.IsRecommended)) { return true; }
            if (!changedTaxonName.IsUnique.Equals(originalTaxonName.IsUnique)) { return true; }
            if (!changedTaxonName.Name.Equals(originalTaxonName.Name)) { return true; }
            if (!changedTaxonName.Category.Id.Equals(originalTaxonName.Category.Id)) { return true; }
            if (!changedTaxonName.Status.Id.Equals(originalTaxonName.Status.Id)) { return true; }
            if (!changedTaxonName.NameUsage.Id.Equals(originalTaxonName.NameUsage.Id)) { return true; }
            // nothing changes
            return false;
        }

        /// <summary>
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxaBefore">The taxon before.</param>
        /// <param name="taxonAfter">The taxon after.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void LumpTaxon(
            IUserContext userContext,
            TaxonList taxaBefore,
            ITaxon taxonAfter,
            ITaxonRevision taxonRevision)
        {
            var referenceRelationsKeyValuePair = new List<KeyValuePair<ITaxonName, IReferenceRelation>>();
            ILumpSplitEvent lumpSplitEvent = null;
            LumpSplitEventList lumpSplitEvents;

            lumpSplitEvents = new LumpSplitEventList();

            TaxonNameList taxonAfterSynonyms = new TaxonNameList();

            CreateTaxonRevisionEvent(userContext, taxonRevision, 8);
            var revisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();
            List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();
            List<ITaxonRelation> taxonBeforeRelations = new List<ITaxonRelation>();

            foreach (ITaxon taxonBefore in taxaBefore)
            {
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ReplacedInTaxonRevisionEvent = revisionEvent;
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ModifiedBy = userContext.User;
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ModifiedDate = DateTime.Now;

                var newTaxonProperties = new TaxonProperties();
                newTaxonProperties.ConceptDefinition = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ConceptDefinition;
                newTaxonProperties.DataContext = new DataContext(userContext); 
                newTaxonProperties.PartOfConceptDefinition = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).PartOfConceptDefinition;
                newTaxonProperties.IsValid = false;
                newTaxonProperties.AlertStatus = TaxonAlertStatusId.Red;
                newTaxonProperties.Taxon = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).Taxon;
                newTaxonProperties.TaxonCategory = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;
                newTaxonProperties.IsPublished = false;
                newTaxonProperties.ChangedInTaxonRevisionEvent = revisionEvent;
                taxonBefore.GetTaxonProperties(userContext).Add(newTaxonProperties);

                // Handle relations for invalidated object
                foreach (ITaxonRelation parentTaxon in taxonBefore.GetNearestParentTaxonRelations(userContext))
                {
                    if (revisionEvent.IsNull())
                    {
                        parentTaxon.ReplacedInTaxonRevisionEventId = null;
                    }
                    else
                    {
                        parentTaxon.ReplacedInTaxonRevisionEventId = revisionEvent.Id;
                    }
                    taxonBeforeRelations.Add(parentTaxon);
                }

                // Create LumpSplit
                if (taxonBefore.Id != taxonAfter.Id) // No self referencing rows
                {
                    lumpSplitEvent = new LumpSplitEvent();
                    if (revisionEvent.IsNull())
                    {
                        lumpSplitEvent.ChangedInTaxonRevisionEventId = null;
                    }
                    else
                    {
                        lumpSplitEvent.ChangedInTaxonRevisionEventId = revisionEvent.Id;
                    }
                    lumpSplitEvent.Type = GetLumpSplitEventType(userContext, LumpSplitEventTypeId.Lump);
                    lumpSplitEvent.TaxonBefore = taxonBefore;
                    lumpSplitEvent.TaxonAfter = taxonAfter;
                    lumpSplitEvents.Add(lumpSplitEvent);
                }

                // Copy ChildTaxa
                foreach (ITaxonRelation childTaxonRelation in taxonBefore.GetNearestChildTaxonRelations(userContext))
                {
                    // Relation to old parent marked as ChangedInRevisionEvent
                    var alreadyChildOfTaxonAfter = false;
                    foreach (
                        ITaxonRelation taxonRelation in
                            childTaxonRelation.ChildTaxon.GetNearestParentTaxonRelations(userContext))
                    {
                        if (taxonRelation.ParentTaxon.Id == taxonBefore.Id)
                        {
                            if (revisionEvent.IsNull())
                            {
                                taxonRelation.ReplacedInTaxonRevisionEventId = null;
                            }
                            else
                            {
                                taxonRelation.ReplacedInTaxonRevisionEventId = revisionEvent.Id;
                            }
                        }
                        taxonRelations.Add(taxonRelation);

                        if (taxonRelation.ParentTaxon.Id == taxonAfter.Id)
                        {
                            alreadyChildOfTaxonAfter = true;
                        }
                    }

                    // If taxon isn't child of taxonAfter we create a new relation as well.
                    if (!alreadyChildOfTaxonAfter)
                    {
                        var newTaxonRelation = new TaxonRelation();
                        newTaxonRelation.ChildTaxon = childTaxonRelation.ChildTaxon;
                        newTaxonRelation.ParentTaxon = taxonAfter;
                        if (revisionEvent.IsNull())
                        {
                            newTaxonRelation.ChangedInTaxonRevisionEventId = null;
                        }
                        else
                        {
                            newTaxonRelation.ChangedInTaxonRevisionEventId = revisionEvent.Id;
                        }
                        newTaxonRelation.IsPublished = false;
                        newTaxonRelation.SortOrder =
                            (from taxonRelation in
                                childTaxonRelation.ChildTaxon.GetNearestParentTaxonRelations(userContext)
                                select taxonRelation.SortOrder).DefaultIfEmpty().Max();
                        newTaxonRelation.IsMainRelation = true;
                        taxonRelations.Add(newTaxonRelation);
                    }

                    // save the childtaxon
                    UpdateTaxon(userContext, childTaxonRelation.ChildTaxon, revisionEvent, null, taxonRelations);
                }

                // Names of taxonBefore to be added to taxonAfter as synonyms (if they don't exist already)
                foreach (var name in taxonBefore.GetCheckedOutChangesTaxonName(userContext))
                {
                    var identicalItemsCount =
                        (from existingName in taxonAfter.GetCheckedOutChangesTaxonName(userContext)
                            where existingName.Name == name.Name && existingName.Author == name.Author
                            select existingName).Count();

                    if (identicalItemsCount == 0)
                    {
                        // Copy name to taxonAfter
                        ITaxonName synonym = new TaxonName();
                        synonym.DataContext = new DataContext(userContext);
                        synonym.IsRecommended = false;
                        synonym.Author = name.Author;
                        synonym.Description = name.Description;
                        synonym.IsOkForSpeciesObservation = name.IsOkForSpeciesObservation;
                        synonym.IsOriginalName = name.IsOriginalName;
                        synonym.IsPublished = false;
                        synonym.IsUnique = name.IsUnique;
                        synonym.Category = name.Category;
                        synonym.Status = name.Status;
                        synonym.NameUsage = name.NameUsage;
                        synonym.Name = name.Name;
                        synonym.SetReferences(name.GetReferences(userContext));
                        synonym.SetChangedInRevisionEvent(name.GetChangedInRevisionEvent(userContext));
                        synonym.ChangedInTaxonRevisionEventId = name.ChangedInTaxonRevisionEventId;
                        synonym.Id = 0;
                        synonym.ValidFromDate = name.ValidFromDate;
                        synonym.ValidToDate = name.ValidToDate;

                        //taxonAfter.TaxonNames.Add(synonym);
                        taxonAfterSynonyms.Add(synonym);

                        foreach (var referenceRelation in name.GetReferences(userContext))
                        {
                            var referenceRelationCopy = new ReferenceRelation();
                            referenceRelationCopy.Reference = referenceRelation.Reference;
                            referenceRelationCopy.ReferenceId = referenceRelation.ReferenceId;
                            referenceRelationCopy.Type = referenceRelation.Type;

                            referenceRelationsKeyValuePair.Add(
                                new KeyValuePair<ITaxonName, IReferenceRelation>(synonym, referenceRelationCopy));
                        }
                    }
                }

                UpdateTaxon(userContext, taxonBefore, revisionEvent, null, taxonBeforeRelations);
            }

            UpdateTaxon(userContext, taxonAfter, revisionEvent, lumpSplitEvents, null);

            // Save the synonyms for the taxonAfter
            foreach (TaxonName taxonAfterSynonym in taxonAfterSynonyms)
            {
                taxonAfterSynonym.Taxon = taxonAfter;
                taxonAfterSynonym.SetChangedInRevisionEvent(revisionEvent);
            }
            DataSource.SaveTaxonNames(userContext, taxonAfterSynonyms, null);

            var referenceRelationsToAdd = new ReferenceRelationList();
            // Copy references for taxonnames
            foreach (var keyValuePair in referenceRelationsKeyValuePair)
            {
                // First update the reference objects with the correct guid...
                IReferenceRelation newReference = keyValuePair.Value;
                newReference.RelatedObjectGuid = keyValuePair.Key.Guid;
                referenceRelationsToAdd.Add(newReference);
            }

            //... then save
            CoreData.ReferenceManager.CreateReferenceRelations(userContext, referenceRelationsToAdd);
        }

        /// <summary>
        /// Moves a list of taxa from one parent to another. The new relation will be sorted last.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxa">List of taxa that will be mmoved.</param>
        /// <param name="previousParent">The previous parent.</param>
        /// <param name="newParent">The new parent.</param>
        /// <param name="taxonRevision">The revision. </param>
        public void MoveTaxa(IUserContext userContext,
                             TaxonList taxa,
                             ITaxon previousParent,
                             ITaxon newParent,
                             ITaxonRevision taxonRevision)
        {
            // TODO -- Make validation

            int eventType = (int)TaxonRevisionEventTypeId.ChangeTaxonParent;

            // Create the Event
            CreateTaxonRevisionEvent(userContext, taxonRevision, eventType);
            var revisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            foreach (ITaxon taxon in taxa)
            {
                MoveTaxon(userContext, taxon, previousParent, newParent, revisionEvent);
            }
        }

        /// <summary>
        /// Moves taxon from one parent to another. If previosParent is null a new parent is created. The new relation will be sorted last.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="previousParent">The previous parent.</param>
        /// <param name="newParent">The new parent.</param>
        /// <param name="taxonRevision">The revision. </param>
        public void MoveTaxon(IUserContext userContext, ITaxon taxon, ITaxon previousParent, ITaxon newParent, ITaxonRevision taxonRevision)
        {
            if (newParent.IsNotNull())
            {
                // Check category based rules to see if the move is allowed

                ITaxonCategory newTaxonCategory = newParent.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory;

                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder < newTaxonCategory.SortOrder)
                {
                    throw new ArgumentException(
                        "Illegal move. For this taxon it is not possible to add/change to selected parent.");
                }


                var genusCategory = this.GetTaxonCategory(userContext, 14);
                // Check if new category is taxonomic 
                // GuNy 2013-01-15
                if (taxon.GetCheckedOutChangesTaxonProperties(userContext).TaxonCategory.SortOrder > genusCategory.SortOrder &&
                    newTaxonCategory.SortOrder < genusCategory.SortOrder &&
                    newTaxonCategory.IsTaxonomic)
                {
                    throw new ArgumentException(
                        "For this taxon it is not possible to add parent above category for genus.");
                }
            }

            // TODO: Fler regler
            // TODO: Regler ska påverka namnbyte vid släkte, kategori ändring etc.

            int eventType;

            if (newParent.IsNull())
            {
                eventType = (int)TaxonRevisionEventTypeId.RemoveTaxonParent;
            }
            else if (previousParent.IsNull())
            {
                eventType = (int)TaxonRevisionEventTypeId.AddTaxonParent;
            }
            else
            {
                eventType = (int)TaxonRevisionEventTypeId.ChangeTaxonParent;
            }

            // Create the Event
            CreateTaxonRevisionEvent(userContext, taxonRevision, eventType);
            var revisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            MoveTaxon(userContext, taxon, previousParent, newParent, revisionEvent);   
        }

        private void MoveTaxon(IUserContext userContext, ITaxon taxon, ITaxon previousParent, ITaxon newParent, ITaxonRevisionEvent taxonRevisionEvent)
        {
            // Transaction shuold be handled in the calling method
            ITaxonRelation oldTaxonRelation = null;
            List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();

            // If previousParent is not null set ChangedInRevisionEventId on this TaxonRelation
            if (previousParent != null)
            {
                oldTaxonRelation =
                    (from taxonRelation in taxon.GetNearestParentTaxonRelations(userContext)
                     where taxonRelation.ParentTaxon.Id == previousParent.Id
                     select taxonRelation).First();
                if (taxonRevisionEvent.IsNull())
                {
                    oldTaxonRelation.ReplacedInTaxonRevisionEventId = null;
                }
                else
                {
                    oldTaxonRelation.ReplacedInTaxonRevisionEventId = taxonRevisionEvent.Id;
                }
                taxonRelations.Add(oldTaxonRelation);
            }

            if (newParent != null)
            {
                // Create the new relation
                var newRelation = new TaxonRelation();
                newRelation.CreatedBy = userContext.User.Id;
                newRelation.CreatedDate = DateTime.Now;
                newRelation.IsPublished = false;
                newRelation.ParentTaxon = newParent;
                newRelation.ChildTaxon = taxon;
                if (oldTaxonRelation != null)
                {
                    newRelation.IsMainRelation = oldTaxonRelation.IsMainRelation;
                }
                else
                {
                    var isMainRelationCount = (from taxonRelation in taxon.GetCheckedOutChangesParentTaxa(userContext)
                                               where taxonRelation.IsMainRelation == true
                                               select taxonRelation).Count();

                    if (isMainRelationCount > 0)
                    {
                        newRelation.IsMainRelation = false;
                    }
                    else
                    {
                        newRelation.IsMainRelation = true;
                    }

                }

                newRelation.SortOrder =
                    (from taxonRelation in taxon.GetNearestParentTaxonRelations(userContext) select taxonRelation.SortOrder).DefaultIfEmpty().Max();
                if (taxonRevisionEvent.IsNull())
                {
                    newRelation.ChangedInTaxonRevisionEventId = null;
                }
                else
                {
                    newRelation.ChangedInTaxonRevisionEventId = taxonRevisionEvent.Id;
                }
                //newRelation.ValidFromDate = DateTime.Now;
                //newRelation.ValidToDate = DateTime.Now.AddYears(40);
                //taxon.GetNearestParentTaxonRelations(userContext).Add(newRelation);
                taxonRelations.Add(newRelation);
            }

            // Save taxon
            UpdateTaxon(userContext, taxon, taxonRevisionEvent, null, taxonRelations);
        }

        /// <summary>
        /// Split one taxon into several other taxa
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonBefore">The taxon before split is done.</param>
        /// <param name="taxaAfter">List of taxa after split is done.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void SplitTaxon(IUserContext userContext,
                               ITaxon taxonBefore,
                               TaxonList taxaAfter,
                               ITaxonRevision taxonRevision)
        {
            var referenceRelationsKeyValuePair = new List<KeyValuePair<ITaxonName, IReferenceRelation>>();
            ILumpSplitEvent newLumpSplitEvent = null;
            LumpSplitEventList lumpSplitEvents;
            List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();
            
            TaxonNameList taxonNamesToBeAddAsSynonymsOnEachTaxon = new TaxonNameList();
            TaxonNameList taxonAfterSynonyms = new TaxonNameList();

            CreateTaxonRevisionEvent(userContext, taxonRevision, 9);
            var revisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            if ((from taxon in taxaAfter where taxon.Id == taxonBefore.Id select taxon).Count() == 0)
            {
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ReplacedInTaxonRevisionEvent = revisionEvent;
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ModifiedBy = userContext.User;
                taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ModifiedDate = DateTime.Now;

                var newTaxonProperties = new TaxonProperties();
                newTaxonProperties.AlertStatus = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).AlertStatus;
                newTaxonProperties.ConceptDefinition =
                    taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).ConceptDefinition;
                newTaxonProperties.DataContext = new DataContext(userContext);
                newTaxonProperties.PartOfConceptDefinition =
                    taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).PartOfConceptDefinition;
                newTaxonProperties.IsValid = true;
                newTaxonProperties.Taxon = taxonBefore.GetCheckedOutChangesTaxonProperties(userContext).Taxon;
                newTaxonProperties.TaxonCategory = GetTaxonCategory(userContext, 27); // Kollektivtaxon
                newTaxonProperties.IsPublished = false;
                newTaxonProperties.ChangedInTaxonRevisionEvent = revisionEvent;
                newTaxonProperties.ModifiedBy = userContext.User;
                newTaxonProperties.ModifiedDate = DateTime.Now;
                newTaxonProperties.ValidFromDate = DateTime.Now;
                newTaxonProperties.ValidToDate = DateTime.Now.AddYears(100);
                taxonBefore.GetTaxonProperties(userContext).Add(newTaxonProperties);

                // Save ALL taxonnames in a list 
                // GuNy 2012-03-27 ska vi verkligen slänga över ALLA namn till splittade taxon ?
                taxonNamesToBeAddAsSynonymsOnEachTaxon = taxonBefore.GetTaxonNames(userContext);

                UpdateTaxon(userContext, taxonBefore, revisionEvent, null, null);
            }

            foreach (ITaxon taxonAfter in taxaAfter)
            {
                lumpSplitEvents = new LumpSplitEventList();
                // Create LumpSplit
                if (taxonBefore.Id != taxonAfter.Id) // No selreferencing rows
                {
                    newLumpSplitEvent = new LumpSplitEvent();
                    if (revisionEvent.IsNull())
                    {
                        newLumpSplitEvent.ChangedInTaxonRevisionEventId = null;
                    }
                    else
                    {
                        newLumpSplitEvent.ChangedInTaxonRevisionEventId = revisionEvent.Id;
                    }
                    newLumpSplitEvent.Type = GetLumpSplitEventType(userContext, LumpSplitEventTypeId.Split);
                    newLumpSplitEvent.TaxonBefore = taxonBefore;
                    newLumpSplitEvent.TaxonAfter = taxonAfter;
                    lumpSplitEvents.Add(newLumpSplitEvent);
                }

                // Create relations to taxonBefore if not already there!
                var existingRelation = (from taxonRelation in taxonAfter.GetNearestParentTaxonRelations(userContext)
                                        where taxonRelation.ParentTaxon.Id == taxonBefore.Id
                                        select taxonRelation).Count();

                if (existingRelation == 0)
                {
                    var newRelation = new TaxonRelation();
                    newRelation.IsPublished = false;
                    newRelation.ParentTaxon = taxonBefore;
                    if (revisionEvent.IsNull())
                    {
                        newRelation.ChangedInTaxonRevisionEventId = null;
                    }
                    else
                    {
                        newRelation.ChangedInTaxonRevisionEventId = revisionEvent.Id;
                    }
                    newRelation.SortOrder = taxonAfter.GetNearestParentTaxonRelations(userContext).Last().SortOrder + 1;
                    newRelation.ChildTaxon = taxonAfter;
                    newRelation.IsMainRelation = false;
                    //newRelation.ValidFromDate = DateTime.Now;
                    //newRelation.ValidToDate = DateTime.Now.AddYears(40);
                    //taxonAfter.GetNearestParentTaxonRelations(userContext).Add(newRelation);
                    taxonRelations.Add(newRelation);
                }

                UpdateTaxon(userContext, taxonAfter, revisionEvent, lumpSplitEvents, taxonRelations);

                // fix the list of taxon names in taxonAfterSynonyms
                foreach (TaxonName taxonName in taxonNamesToBeAddAsSynonymsOnEachTaxon)
                {
                    ITaxonName synonym = new TaxonName();
                    synonym.DataContext = new DataContext(userContext);
                    synonym.IsRecommended = false;
                    synonym.Author = taxonName.Author;
                    synonym.Description = taxonName.Description;
                    synonym.IsOkForSpeciesObservation = taxonName.IsOkForSpeciesObservation;
                    synonym.IsOriginalName = taxonName.IsOriginalName;
                    synonym.IsPublished = false;
                    synonym.IsUnique = taxonName.IsUnique;
                    synonym.Category = taxonName.Category;
                    synonym.Status = taxonName.Status;
                    synonym.NameUsage = taxonName.NameUsage;
                    synonym.Name = taxonName.Name;
                    synonym.SetReferences(taxonName.GetReferences(userContext));
                    synonym.SetChangedInRevisionEvent(taxonName.GetChangedInRevisionEvent(userContext));
                    synonym.ChangedInTaxonRevisionEventId = taxonName.ChangedInTaxonRevisionEventId;
                    synonym.Id = 0;
                    synonym.ValidFromDate = taxonName.ValidFromDate;
                    synonym.ValidToDate = taxonName.ValidToDate;
                    synonym.Taxon = taxonAfter;

                    taxonAfterSynonyms.Add(synonym);

                    foreach (var referenceRelation in taxonName.GetReferences(userContext))
                    {
                        var referenceRelationCopy = new ReferenceRelation();
                        referenceRelationCopy.Reference = referenceRelation.Reference;
                        referenceRelationCopy.ReferenceId = referenceRelation.ReferenceId;
                        referenceRelationCopy.Type = referenceRelation.Type;

                        referenceRelationsKeyValuePair.Add(new KeyValuePair<ITaxonName, IReferenceRelation>(synonym, referenceRelationCopy));
                    }
                }

                // save the taxon names
                DataSource.SaveTaxonNames(userContext, taxonAfterSynonyms, null);
            }

            var referenceRelationsToAdd = new ReferenceRelationList();
            // Copy references for taxonnames
            foreach (var keyValuePair in referenceRelationsKeyValuePair)
            {
                // First update the reference objects with the correct guid...
                IReferenceRelation newReference = keyValuePair.Value;
                newReference.RelatedObjectGuid = keyValuePair.Key.Guid;
                referenceRelationsToAdd.Add(newReference);
            }

            //... then save
            CoreData.ReferenceManager.CreateReferenceRelations(userContext, referenceRelationsToAdd);
        }

        /// <summary>
        /// Saves name of the taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">TaxonName object.</param>
        /// <param name="action">TaxonEventType - Add, Edit or Delete</param>
        private void SaveTaxonName(IUserContext userContext, ITaxonRevision taxonRevision, ITaxonName taxonName, TaxonRevisionEventTypeId action)
        {
            int eventTypeId = (int)action;

            ITaxonName changedTaxonName = null;
            TaxonNameList taxonNamesToBeSaved = new TaxonNameList();

            // if edit name - check if data is the same - and return if it is.
            if (action.Equals(TaxonRevisionEventTypeId.EditTaxonName))
            {
                // Get the taxon name that is changing
                changedTaxonName = GetTaxonNameByVersion(userContext, taxonName.Version);

                if (!IsTaxonNameChanged(changedTaxonName, taxonName))
                {
                    return;
                    //throw new ArgumentException("Not saved! Taxon name is not changed.");
                }
            }

            // Create revisionvent
            CreateTaxonRevisionEvent(userContext, taxonRevision, eventTypeId);

            taxonName.SetChangedInRevisionEvent(taxonRevision.GetRevisionEvents(userContext).Last());
            taxonName.IsPublished = false;


            if (action.Equals(TaxonRevisionEventTypeId.DeleteTaxonName))
            {
                taxonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(userContext, TaxonNameStatusId.Removed);

                // Get the taxon name that is changing
                changedTaxonName = GetTaxonNameByVersion(userContext, taxonName.Version);

                // Set ChangedInRevisionEvent
                changedTaxonName.SetReplacedInRevisionEvent(taxonRevision.GetRevisionEvents(userContext).Last());

                // Add the changed taxon name object to the list of TaxonName objects to be saved
                taxonNamesToBeSaved.Add(changedTaxonName);

                // Add the invalidated taxonname
                taxonNamesToBeSaved.Add(taxonName);
            }

            else if (action.Equals(TaxonRevisionEventTypeId.EditTaxonName))
            {
                // Set ChangedInRevisionEvent
                changedTaxonName.SetReplacedInRevisionEvent(taxonRevision.GetRevisionEvents(userContext).Last());

                // Check if the taxonname changes from NOT recommeded to recommended
                if (taxonName.IsRecommended && !changedTaxonName.IsRecommended)
                {
                    // still in the same namecategory ?
                    if (changedTaxonName.Category.Id == taxonName.Category.Id)
                    {
                        IList<TaxonName> currentRecommendedNames = UpdateCurrentRecommendedName(userContext, taxonName.Taxon, taxonName.Category.Id, taxonRevision.GetRevisionEvents(userContext).Last());
                        if (currentRecommendedNames.IsNotNull())
                        {
                            foreach (TaxonName currentRecommendedName in currentRecommendedNames)
                            {
                                taxonNamesToBeSaved.Add(currentRecommendedName);
                            }
                        }
                    }
                }

                // Add the changed taxon name object to the list of TaxonName objects in Taxon
                taxonNamesToBeSaved.Add(changedTaxonName);


                // Add the edited taxon name object to the list of TaxonName objects in Taxon
                taxonNamesToBeSaved.Add(taxonName);
            }

            else if (action.Equals(TaxonRevisionEventTypeId.AddTaxonName))
            {
                // if the new taxon name is the recommended name in the category ... 
                if (taxonName.IsRecommended)
                {
                    IList<TaxonName> currentRecommendedNames = UpdateCurrentRecommendedName(userContext, taxonName.Taxon, taxonName.Category.Id, taxonRevision.GetRevisionEvents(userContext).Last());
                    if (currentRecommendedNames.IsNotNull())
                    {
                        foreach (TaxonName currentRecommendedName in currentRecommendedNames)
                        {
                            taxonNamesToBeSaved.Add(currentRecommendedName);
                        }
                    }
                }
                // Add the new TaxonName object
                taxonNamesToBeSaved.Add(taxonName);
            }

            // Set taxonName.Version = 0 to be able to do correct steps in SaveTaxonName method.
            taxonName.Version = 0;

            // Save the taxon.
            DataSource.SaveTaxonNames(userContext, taxonNamesToBeSaved, (TaxonRevisionEvent)taxonRevision.GetRevisionEvents(userContext).Last());
        }

        /// <summary>
        /// Change the current recommended name in a specific namecategory 
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <param name="nameCategoryId">Check names in this namecategory.</param>
        /// <param name="taxon">Taxon object.</param>
        /// <param name="lastTaxonRevisionEvent">The revision event.</param>
        /// <returns>TaxonName object set as recommended name</returns>
        private IList<TaxonName> UpdateCurrentRecommendedName(IUserContext userContext, ITaxon taxon, Int32 nameCategoryId, ITaxonRevisionEvent lastTaxonRevisionEvent)
        {
            IList<TaxonName> taxonNamesToBeChanged = new List<TaxonName>();

            TaxonName taxonNameCloneUpdate = null;
            TaxonName taxonNameCloneInsert = null;
            // Get the list of taxon names from database.
            TaxonNameList taxonNames = GetTaxonNames(userContext, taxon);
            foreach (TaxonName taxonName in taxonNames)
            {
                if (taxonName.IsRecommended &&
                    (taxonName.Status.Id == (Int32)(TaxonNameStatusId.ApprovedNaming)) &&
                    taxonName.ValidToDate > DateTime.Now)
                {
                    if (taxonName.Category.Id == nameCategoryId)
                    {
                        // make the current recommended name invalid by setting the ChangedInRevision
                        taxonNameCloneUpdate = taxonName.Clone(userContext);
                        taxonNameCloneUpdate.SetReplacedInRevisionEvent(lastTaxonRevisionEvent);
                        taxonNamesToBeChanged.Add(taxonNameCloneUpdate);
                        // create copy of the TaxonName object
                        taxonNameCloneInsert = taxonName.Clone(userContext);
                        taxonNameCloneInsert.SetChangedInRevisionEvent(lastTaxonRevisionEvent);
                        taxonNameCloneInsert.IsPublished = false;
                        taxonNameCloneInsert.IsRecommended = false;
                        // Set Version = 0 -- GuNy 2013-01-14
                        taxonNameCloneInsert.Version = 0;
                        // taxonNameCloneInsert.Id = 0;
                        // do not add references - they are already connected to this name by GUID
                        taxonNameCloneInsert.SetReferences(null);
                        taxonNamesToBeChanged.Add(taxonNameCloneInsert);
                        break;
                    }
                }
            }
            return taxonNamesToBeChanged;
        }

        /// <summary>
        /// Update taxon category for taxon.
        /// Creates all related changes in the revision.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="taxonRevision">Revision.</param>
        /// <param name="category">The category.</param>
        public void UpdateTaxon(IUserContext userContext,
                                ITaxon taxon,
                                ITaxonRevision taxonRevision,
                                ITaxonCategory category)
        {
            // Check revision
            if (taxonRevision.State.Id != (int)TaxonRevisionStateId.Ongoing)
            {
                throw new Exception("Revision is not checked out for edit");
            }
            
            // Save the revision event
            CreateTaxonRevisionEvent(userContext, taxonRevision, (int)TaxonRevisionEventTypeId.ChangeTaxonCategory);

            // Create a new TaxonProperty object
            var newProp = new TaxonProperties()
            {
                ChangedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last(),
                DataContext = new DataContext(userContext),
                Taxon = taxon,
                IsPublished = false,
                IsValid = true,
                TaxonCategory = category,
                ModifiedBy = userContext.User,
                ModifiedDate = DateTime.Now,
                ValidFromDate = DateTime.Now,
                ValidToDate = DateTime.Now.AddYears(40)
            };

            // Flag the currently valid TaxonProperty object so it will be invalidated on checkin
            taxon.GetCheckedOutChangesTaxonProperties(userContext).ReplacedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            // Add the new TaxonProperty object
            taxon.GetTaxonProperties(userContext).Add(newProp);

            // Save the taxon.
            UpdateTaxon(userContext, taxon, taxonRevision.GetRevisionEvents(userContext).Last(), null, null);
        }

        /// <summary>
        /// Create new taxon.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">
        /// Information about the new taxon.
        /// This object is updated with information 
        /// about the created taxon.
        /// </param>
        /// <param name="taxonRevisionEvent">Revision event.</param>
        /// <param name="lumpSplitEvents">Creat lump split events if this parameter is not null.</param>
        /// <param name="taxonRelations">Create taxonrelations if this parameter is not null.</param>
        public virtual void UpdateTaxon(IUserContext userContext,
                                        ITaxon taxon,
                                        ITaxonRevisionEvent taxonRevisionEvent,
                                        LumpSplitEventList lumpSplitEvents,
                                        IList<ITaxonRelation> taxonRelations)
        {
            if (!CheckTaxon(userContext,taxon))
            {
                throw new Exception("Validation of taxon failed!");
            }

            // Taxon.
            if (taxon.Id <= 0)
            {
                DataSource.CreateTaxon(userContext, taxon, taxonRevisionEvent);
            }

            // TaxonProperties.
            foreach (ITaxonProperties taxonProperties in taxon.GetTaxonProperties(userContext))
            {
                if (taxonProperties.Id <= 0)
                {
                    taxonProperties.Taxon = taxon;
                    DataSource.CreateTaxonProperties(userContext, taxonProperties);
                }
                else if ((taxonProperties.Id) > 0 &&
                         taxonProperties.ReplacedInTaxonRevisionEvent.IsNotNull() &&
                         (taxonProperties.ReplacedInTaxonRevisionEvent.Id == taxonRevisionEvent.Id))
                {
                    DataSource.UpdateTaxonProperties(userContext, taxonProperties);
                }
            }

            // TaxonRelation.
            // foreach (ITaxonRelation taxonRelation in taxon.GetNearestParentTaxonRelations(userContext))
            if (taxonRelations.IsNotNull() && taxonRelations.Count() > 0)
            {
                foreach (ITaxonRelation taxonRelation in taxonRelations)
                {
                    if (taxonRelation.Id <= 0)
                    {
                        taxonRelation.ChildTaxon = taxon;
                        this.DataSource.CreateTaxonRelation(userContext, taxonRelation);
                    }
                    else if ((taxonRelation.Id > 0) && taxonRelation.ReplacedInTaxonRevisionEventId.HasValue
                             && (taxonRelation.ReplacedInTaxonRevisionEventId.Value == taxonRevisionEvent.Id))
                    {
                        this.DataSource.UpdateTaxonRelation(userContext, taxonRelation);
                    }
                }
            }
            else
            {
            }

            // LumpSplit.
            if (lumpSplitEvents.IsNotEmpty())
            {
                foreach (ILumpSplitEvent lumpSplitEvent in lumpSplitEvents)
                {
                    DataSource.CreateLumpSplitEvent(userContext, lumpSplitEvent);
                }
            }

            // Update the taxon revision event.
            if (taxonRevisionEvent.IsNotNull())
            {
                DataSource.UpdateTaxonRevisionEvent(userContext, taxonRevisionEvent);
            }
        }

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
        public void UpdateTaxon(IUserContext userContext,
            ITaxon taxon,
            ITaxonRevision taxonRevision,
            String conceptDefinitionPartString,
            ITaxonCategory category,
            TaxonAlertStatusId alertStatus,
            Boolean isMicrospecies)
        {
            if (taxon.PartOfConceptDefinition != conceptDefinitionPartString
                || (taxon.Category.Id != category.Id) 
                || (taxon.AlertStatus.Id != (Int32)alertStatus) 
                || taxon.IsMicrospecies != isMicrospecies)
            {
                // Check revision
                if (taxonRevision.State.Id != (int) TaxonRevisionStateId.Ongoing)
                {
                    throw new Exception("Revision is not checked out for edit");
                }

                // Save the revision event
                CreateTaxonRevisionEvent(userContext, taxonRevision, (int) TaxonRevisionEventTypeId.EditTaxon );

                // Create a new TaxonProperty object
                var newProp = new TaxonProperties()
                                    {
                                        Id = 0,
                                        ChangedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last(),
                                        DataContext = new DataContext(userContext),
                                        Taxon = taxon,
                                        IsPublished = false,
                                        IsValid = true,
                                        TaxonCategory = category,
                                        AlertStatus = alertStatus,
                                        IsMicrospecies = isMicrospecies,
                                        PartOfConceptDefinition = conceptDefinitionPartString,
                                        ModifiedBy = userContext.User,
                                        ModifiedDate = DateTime.Now,
                                        ValidFromDate = DateTime.Now,
                                        ValidToDate = DateTime.Now.AddYears(40)
                };

                // Flag the currently valid TaxonProperty object so it will be invalidated on checkin
                taxon.GetCheckedOutChangesTaxonProperties(userContext).ReplacedInTaxonRevisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

                // Add the new TaxonProperty object
                taxon.GetTaxonProperties(userContext).Add(newProp);

                // Save the taxon.
                UpdateTaxon(userContext, taxon, taxonRevision.GetRevisionEvents(userContext).Last(), null, null);                
            }
        }

        /// <summary>
        /// Change a taxon name.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonName">TaxonName object with name to be changed.</param>
        public void UpdateTaxonName(IUserContext userContext,
                                    ITaxonRevision taxonRevision,
                                    ITaxonName taxonName)
        {
            TaxonNameList taxonNames = new TaxonNameList();
            taxonNames.Add(taxonName);

            UpdateTaxonNames(userContext, taxonRevision, taxonNames);
        }

        /// <summary>
        /// Change taxon names.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonRevision">The revision.</param>
        /// <param name="taxonNames">List of TaxonName objects.</param>
        public void UpdateTaxonNames(IUserContext userContext,
                                     ITaxonRevision taxonRevision,
                                     TaxonNameList taxonNames)
        {
            if (!CheckTaxonNames(userContext, taxonNames))
            {
                throw new ArgumentException("Only one recommended name is allowed in a category.");
            }
            
            foreach (ITaxonName taxonName in taxonNames)
            {
                SaveTaxonName(userContext, taxonRevision, taxonName, TaxonRevisionEventTypeId.EditTaxonName);
            }
        }

        /// <summary>Save revision.</summary>
        /// <param name="userContext">User context</param>
        /// <param name="taxonRevision">The revision.</param>
        public void UpdateTaxonRevision(IUserContext userContext, ITaxonRevision taxonRevision)
        {
            DataSource.SaveTaxonRevision(userContext, taxonRevision);
        }

        /// <summary>Changes sortorder for a complete level.</summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxonIds">The taxa ids sorted in new sort order.</param>
        /// <param name="parentTaxonId">The parent taxon id.</param>
        /// <param name="taxonRevision">The revision.</param>
        public void UpdateTaxonTreeSortOrder(IUserContext userContext, List<int> taxonIds, int parentTaxonId, ITaxonRevision taxonRevision)
        {
            CreateTaxonRevisionEvent(userContext, taxonRevision, (int)TaxonRevisionEventTypeId.ChangeTaxonSortOrder);
           // var revisionEvent = taxonRevision.GetRevisionEvents(userContext).Last();

            DataSource.UpdateTaxonTreeSortOrder(userContext, parentTaxonId, taxonIds, (TaxonRevisionEvent)taxonRevision.GetRevisionEvents(userContext).Last());
        }
    }
}   