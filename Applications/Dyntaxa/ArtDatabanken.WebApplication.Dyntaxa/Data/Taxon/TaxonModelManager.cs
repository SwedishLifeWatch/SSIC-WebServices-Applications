using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Taxon;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

// ReSharper disable CheckNamespace

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonModelManager
    {
        //    private SpeciesFactModelManager speciesModel = null;

        //SpeciesFactModelManager SpeciesModel
        //{
        //    get { return speciesModel; }
        //}

        public TaxonAddViewModel GetAddTaxonViewModel(
            IUserContext userContext,
            ITaxon parentTaxon,
            ITaxonRevision taxonRevision,
            IList<ITaxonCategory> possibleTaxonCategories)
        {
            TaxonAddViewModel model = new TaxonAddViewModel();
            IUserContext loggedInUser = userContext;
            int defaultCategoryId = 0;
            if (loggedInUser.IsNotNull())
            {
                if (parentTaxon.IsNotNull() && parentTaxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = parentTaxon.Id.ToString();
                    if (taxonRevision.IsNotNull())
                    {
                        model.ParentTaxonId = parentTaxon.Id.ToString();
                        model.TaxonGuid = null;

                        model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
                        model.TaxonCategoryList.Add(new TaxonDropDownModelHelper(0,
                            Resources.DyntaxaResource.SharedTaxonSelecteCategoryDropDownListText));

                        foreach (var possibleTaxonCategory in possibleTaxonCategories)
                        {
                            model.TaxonCategoryList.Add(new TaxonDropDownModelHelper(possibleTaxonCategory.Id,
                                possibleTaxonCategory.Name));
                            // find out which category that should be selected as default 
                            if (defaultCategoryId == 0 &&
                                possibleTaxonCategory.SortOrder > parentTaxon.Category.SortOrder)
                            {
                                defaultCategoryId = possibleTaxonCategory.Id;
                            }
                        }
                        model.TaxonCategoryId = defaultCategoryId;
                        model.Author = string.Empty;
                        model.CommonName = string.Empty;
                        model.ScientificName = string.Empty;
                        model.Description = string.Empty;
                        model.TaxonIsProblematic = false;
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        //public ITaxon InitNewTaxon(IUserContext loggedInUser, TaxonAddViewModel model, ITaxon newTaxon, ITaxon parentTaxon, 
        //                                   ITaxonCategory category, ITaxonName scentificName, ITaxonName commonName)
        //{
        //    int alert;
        //    newTaxon.Category = category;

        //    // Add main parent
        //    ITaxonRelation taxonRelation = new TaxonRelation();
        //    List<ITaxonRelation> taxonRelations = new List<ITaxonRelation>();
        //    taxonRelation.IsMainRelation = true;
        //    taxonRelation.RelatedTaxon = parentTaxon;
        //    taxonRelations.Add(taxonRelation);

        //    //Add names
        //    IList<ITaxonName> taxonNames = new List<ITaxonName>();
        //    // Scentific name
        //    ITaxonName taxonScentificName = scentificName;
        //    taxonScentificName.Name = model.ScientificName;
        //    taxonScentificName.Author = model.Author.IsNotNull() ? model.Author : string.Empty;
        //    taxonScentificName.NameCategory =
        //        DyntaxaCachedSettings.Instance.GetTaxonNameCategoryById(
        //            TaxonNameCategoryIds.SCIENTIFIC_NAME);

        //    taxonScentificName.Status = CoreData.TaxonManager.GetTaxonNameStatus(CoreData.UserManager.GetCurrentUser(), TaxonNameStatusId.Removed);
        //    taxonScentificName.IsRecommended = true;
        //    taxonScentificName.Description = string.Empty;
        //    //taxonScentificName.TaxonId = model.TaxonId;
        //    taxonScentificName.IsOkForObsSystems = true;

        //    taxonNames.Add(taxonScentificName);
        //    //Common name if not nulll
        //    if (model.CommonName.IsNotNull())
        //    {
        //        ITaxonName taxonCommonName = commonName;
        //        taxonCommonName.Name = model.CommonName;
        //        taxonCommonName.Author = model.Author;
        //        taxonCommonName.NameCategory =
        //            DyntaxaCachedSettings.Instance.GetTaxonNameCategoryById(
        //                Resources.DyntaxaSettings.Default.CommonNameCategoryId);
        //        taxonCommonName.Status = CoreData.TaxonManager.GetTaxonNameStatus(loggedInUser, TaxonNameStatusId.Removed);
        //        taxonCommonName.IsRecommended = true;
        //        taxonCommonName.Description = string.Empty;
        //        //taxonScentificName.TaxonId = model.TaxonId;
        //        taxonCommonName.IsOkForObsSystems = true;
        //        taxonNames.Add(taxonCommonName);
        //    }

        //    newTaxon.TaxonNames = taxonNames;

        //    newTaxon.ConceptDefinitionPartString = model.Description;
        //    // Update alert status
        //    if (model.TaxonIsProblematic)
        //    {
        //        alert = (int)TaxonAlertLevel.Yellow;
        //    }
        //    else
        //    {
        //        alert = (int)TaxonAlertLevel.Green;
        //    }
        //    newTaxon.AlertStatus = alert;
        //    return newTaxon;
        //}

        public TaxonEditViewModel GetEditTaxonViewModel(
            IUserContext userContext,
            ITaxon taxon,
            ITaxonRevision taxonRevision,
            ITaxonCategory category,
            TaxonCategoryList taxonCategories,
            bool isTaxonNew)
        {
            var model = new TaxonEditViewModel();
            model.IsTaxonJustCreated = isTaxonNew;
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (taxonRevision.IsNotNull())
                    {
                        model.TaxonId = taxon.Id.ToString();
                        model.TaxonGuid = taxon.Guid;
                        model.TaxonReferencesList = new List<IReference>();
                        model.TaxonCategoryId = category.Id;
                        model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();

                        foreach (ITaxonCategory taxonCategory in taxonCategories)
                        {
                            model.TaxonCategoryList.Add(
                                new TaxonDropDownModelHelper(taxonCategory.Id, taxonCategory.Name));
                        }

                        model.IsMicrospecies = taxon.IsMicrospecies;
                        model.NoOfTaxonReferences = 0;
                        foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(userContext))
                        {
                            model.NoOfTaxonReferences++;
                        }

                        model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
                        model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();
                        model.SwedishImmigrationHistoryReferenceList.Add(
                            new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
                        foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(userContext))
                        {
                            IReference reference = referenceRelation.GetReference(userContext);
                            Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                            model.SwedishOccurrenceReferenceList.Add(
                                new TaxonDropDownModelHelper(
                                    reference.Id,
                                    reference.Name + " " + year));
                            model.SwedishImmigrationHistoryReferenceList.Add(
                                new TaxonDropDownModelHelper(
                                    reference.Id,
                                    reference.Name + " " + year));
                        }

                        // Set taxon properties 
                        model.Author = taxon.Author.IsNotEmpty() ? taxon.Author : string.Empty;
                        model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                        model.ScientificName = taxon.ScientificName;
                        model.Description = taxon.PartOfConceptDefinition.IsNotNull()
                            ? taxon.PartOfConceptDefinition
                            : string.Empty;
                        model.EnableTaxonIsProblematic = true;
                        model.TaxonIsProblematic = false;
                        model.SpeciesFactError = false;

                        // In alert status disable possibility to change alert status since it is at red/high level
                        if (taxon.AlertStatus.Id == (Int32) TaxonAlertStatusId.Red)
                        {
                            model.EnableTaxonIsProblematic = false;
                            model.TaxonIsProblematic = true;
                        }
                        else if (taxon.AlertStatus.Id == (Int32) TaxonAlertStatusId.Yellow)
                        {
                            model.TaxonIsProblematic = true;
                        }

                        // Set default values
                        model.BlockedForReporting = false;
                        model.ExcludeFromReportingSystem = false;

                        // Check if taxon has sortorder higher than "Art"
                        model.EnableSpeciesFact = false;

                        // Get specices fact information
                        try
                        {
                            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
                            try
                            {
                                model.BlockedForReporting = speciesFactModel.BanndedForReporting;
                                model.ExcludeFromReportingSystem = speciesFactModel.ExcludeFromReportingSystem;
                                if (speciesFactModel.QualityStatus.IsNotNull())
                                {
                                    model.TaxonQualityId = speciesFactModel.QualityStatus.Id;
                                }
                                else if (speciesFactModel.QualityStatusList.IsNotNull()
                                         && speciesFactModel.QualityStatusList.Count > 1)
                                {
                                    model.TaxonQualityId = speciesFactModel.QualityStatusList.ElementAt(0).Id;
                                }
                                else
                                {
                                    model.TaxonQualityId = 0;
                                }

                                model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
                                foreach (var status in speciesFactModel.QualityStatusList)
                                {
                                    model.TaxonQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                                }

                                model.TaxonQualityDescription = speciesFactModel.QualityDescription;
                            }
                            catch (Exception)
                            {
                                // Show data with an error message
                                model.SpeciesFactError = true;
                                model.EnableSpeciesFact = false;
                                model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                            }

                            if (taxon.Category.SortOrder >= Resources.DyntaxaSettings.Default.SpeciesCategorySortOrder)
                            {
                                model.EnableSpeciesFact = true;
                                try
                                {
                                    GetSpeciesFact(userContext, taxonRevision.Id, taxon.Id, model, isTaxonNew,
                                        speciesFactModel);
                                }
                                catch (Exception)
                                {
                                    // Show data with an error message
                                    model.SpeciesFactError = true;
                                    model.EnableSpeciesFact = false;
                                    model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.SpeciesFactError = true;
                            model.EnableSpeciesFact = false;
                            model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                        }
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        public void UpdateSpeciesFact(
            IUserContext loggedInUser,
            TaxonSwedishOccuranceBaseViewModel model,
            ITaxon taxon,
            int? revisionId,
            bool updateSwedishOccurance = true,
            bool updateQuality = true,
            bool updateAdditionalQuality = true)
        {
            try
            {
                // Check logged in user
                if (loggedInUser.IsNull())
                {
                    return;
                }

                // Update quality species facts directly to Artfakta.
                if (updateQuality || updateAdditionalQuality)
                {
                    SaveQualitySpeciesFact(loggedInUser, model, taxon, updateQuality, updateAdditionalQuality);
                }

                // Save DyntaxaRevisionSpeciesFacts to Taxon database. Commit to Artfakta later when the Revision is published.
                if (updateSwedishOccurance &&
                    taxon.Category.Id >= Resources.DyntaxaSettings.Default.GenusTaxonCategoryId)
                {
                    SaveSwedishOccurrenceAndHistoryToTaxonDatabase(loggedInUser, model, taxon, revisionId);
                }
            }
            catch (Exception e)
            {
                Exception ex = new Exception(Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError, e);
                throw ex;
            }
        }

        public void SaveSwedishOccurrenceAndHistoryToTaxonDatabase(
            IUserContext loggedInUser,
            TaxonSwedishOccuranceBaseViewModel model,
            ITaxon taxon,
            int? revisionId)
        {
            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);

            // Create swedish ocurrance values
            if (model.SwedishOccurrenceStatusId != 0)
            {
                speciesFactModel.SwedishOccurrenceId = model.SwedishOccurrenceStatusId;
            }

            if (model.SwedishOccurrenceQualityId != 0)
            {
                speciesFactModel.SwedishOccurrenceQualityId = model.SwedishOccurrenceQualityId;
            }

            if (model.SwedishOccurrenceReferenceId != 0)
            {
                speciesFactModel.SwedishOccurrenceReferenceId = model.SwedishOccurrenceReferenceId;
            }

            if (model.SwedishOccurrenceDescription.IsNotNull())
            {
                speciesFactModel.SwedishOccurrenceDescription = model.SwedishOccurrenceDescription;
            }

            // Create immigration history values. If SwedishImmigrationHistoryStatusId is not set
            // the rest of the values are not applicable.
            if (model.SwedishImmigrationHistoryStatusId != 0)
            {
                speciesFactModel.SwedishHistoryId = model.SwedishImmigrationHistoryStatusId;
                speciesFactModel.SwedishHistoryQualityId = model.SwedishImmigrationHistoryQualityId;
                speciesFactModel.SwedishHistoryReferenceId = model.SwedishImmigrationHistoryReferenceId;

                if (model.SwedishImmigrationHistoryDescription.IsNotNull())
                {
                    speciesFactModel.SwedishHistoryDescription = model.SwedishImmigrationHistoryDescription;
                }
            }
            else
            {
                speciesFactModel.SwedishHistoryDescription = null;
                speciesFactModel.SwedishHistoryId = null;
                speciesFactModel.SwedishHistoryQualityId = null;
                speciesFactModel.SwedishHistoryReferenceId = null;
            }

            UpdateDyntaxaRevisionSpeciesFact(loggedInUser, model, speciesFactModel, taxon, revisionId.Value);
        }

        public void SaveQualitySpeciesFact(
            IUserContext loggedInUser,
            TaxonSwedishOccuranceBaseViewModel model,
            ITaxon taxon,
            bool updateQuality,
            bool updateAdditionalQuality,
            QualityApplyMode applyMode)
        {
            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
            List<SpeciesFactModelManager> modifiedSpeciesFactModelList = new List<SpeciesFactModelManager>();

            if (updateQuality)
            {
                // Create quality values
                speciesFactModel.QualityStatusId = model.TaxonQualityId;
                speciesFactModel.QualityDescription = model.TaxonQualityDescription;
            }

            if (updateAdditionalQuality)
            {
                //Additional spieces fact settings
                speciesFactModel.BanndedForReporting = model.BlockedForReporting;
                speciesFactModel.ExcludeFromReportingSystem = model.ExcludeFromReportingSystem;
            }

            if (applyMode > QualityApplyMode.OnlySelected)
            {
                modifiedSpeciesFactModelList = GetQualitySpeciesFactTree(taxon, loggedInUser, model, applyMode);
            }

            using (var transaction = loggedInUser.StartTransaction(180))
            {
                //Update current taxon
                speciesFactModel.UpdateDyntaxaSpeciesFactsWithoutTransaction();

                //Update child taxa
                if (applyMode > QualityApplyMode.OnlySelected)
                {
                    foreach (var childSpeciesFactModel in modifiedSpeciesFactModelList)
                    {
                        childSpeciesFactModel.UpdateDyntaxaSpeciesFactsWithoutTransaction();
                    }
                }

                transaction.Commit();
            }
        }

        public void SaveQualitySpeciesFact(
            IUserContext loggedInUser,
            TaxonSwedishOccuranceBaseViewModel model,
            ITaxon taxon,
            bool updateQuality,
            bool updateAdditionalQuality)
        {
            SaveQualitySpeciesFact(loggedInUser, model, taxon, updateQuality, updateAdditionalQuality, QualityApplyMode.OnlySelected);
        }

        private List<SpeciesFactModelManager> GetQualitySpeciesFactTree(ITaxon taxon, IUserContext loggedInUser, TaxonSwedishOccuranceBaseViewModel model, QualityApplyMode applyMode)
        {
            // Get all child taxa, including start node taxon
            ITaxonSearchCriteria taxonSearchCriteria = new TaxonSearchCriteria();
            taxonSearchCriteria.TaxonIds = new List<int> {taxon.Id};
            taxonSearchCriteria.Scope = TaxonSearchScope.AllChildTaxa;
            var taxonList = CoreData.TaxonManager.GetTaxa(loggedInUser, taxonSearchCriteria);
            //Remove root taxon from list
            taxonList.RemoveAt(0);

            //Create list with original values
            var speciesFactModelList =
                taxonList.Select(childTaxon => new SpeciesFactModelManager(childTaxon, loggedInUser)).ToList();

            //Update list with new QualityStatus value
            var modifiedSpeciesFactModelList = new List<SpeciesFactModelManager>();
            foreach (var sfm in speciesFactModelList)
            {
                if (applyMode == QualityApplyMode.AddToAllUnderlyingTaxa ||
                    (applyMode == QualityApplyMode.AddToUnderlyingTaxaExceptWhereAlreadyDeclared &&
                     (sfm.QualityStatus.IsNull() || sfm.QualityStatus.Id == (int) DataQuality.NotEvaluated)) ||
                    (applyMode == QualityApplyMode.AddToUnderlyingTaxaExceptWhereAlreadyDeclaredHigher &&
                     (sfm.QualityStatus.IsNull() || sfm.QualityStatus.Id < model.TaxonQualityId)))
                {
                    //Update item with the new value
                    sfm.QualityStatusId = model.TaxonQualityId;

                    modifiedSpeciesFactModelList.Add(sfm);
                }
            }

            return modifiedSpeciesFactModelList;
        }

        /// <summary>
        /// Species fact state enum.
        /// </summary>
        private enum SpeciesFactStateEnum
        {
            Unchanged,
            Changed,
            Created,
            Deleted
        }

        /// <summary>
        /// Gets the state of the species fact.
        /// </summary>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>The state.</returns>
        private SpeciesFactStateEnum GetSpeciesFactState(ISpeciesFact speciesFact)
        {            
            if (speciesFact.AllowUpdate && speciesFact.HasChanged)
            {
                if (!speciesFact.HasId && speciesFact.ShouldBeSaved && !speciesFact.ShouldBeDeleted)
                {
                    return SpeciesFactStateEnum.Created;
                }

                if (speciesFact.HasId && speciesFact.ShouldBeDeleted)
                {
                    return SpeciesFactStateEnum.Deleted;                    
                }

                if (speciesFact.HasId && speciesFact.ShouldBeSaved && !speciesFact.ShouldBeDeleted)
                {
                    return SpeciesFactStateEnum.Changed;                    
                }
            }
          
            return SpeciesFactStateEnum.Unchanged;
        }

        /// <summary>
        /// Creates a dyntaxa revision species fact change unit.
        /// The change unit consists of the Revision event and the Dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="originalSpeciesFact">Original </param>
        /// <param name="oldSpeciesFact">The old species fact.</param>
        /// <param name="newSpeciesFact">The new species fact.</param>
        /// <param name="revisionEventType">Type of the revision event.</param>
        /// <returns></returns>        
        private DyntaxaRevisionSpeciesFactChangeUnit CreateDyntaxaRevisionSpeciesFactChangeUnit(IUserContext userContext, int revisionId, ITaxon taxon, SpeciesFact originalSpeciesFact, SpeciesFact oldSpeciesFact, SpeciesFact newSpeciesFact, TaxonRevisionEventTypeId revisionEventType)
        {
            TaxonRevisionEvent revisionEvent = GetDyntaxaRevisionSpeciesFactRevisionEvent(userContext, revisionId, taxon, oldSpeciesFact, newSpeciesFact, revisionEventType);
            DyntaxaRevisionSpeciesFact dyntaxaRevisionSpeciesFact = new DyntaxaRevisionSpeciesFact();
            switch (revisionEventType)
            {
                case TaxonRevisionEventTypeId.ChangeSwedishOccurrence:
                    dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishOccurrence;
                    break;
                case TaxonRevisionEventTypeId.ChangeSwedishImmigrationHistory:
                    dyntaxaRevisionSpeciesFact.FactorId = (Int32)FactorId.SwedishHistory;
                    break;
                default:
                    throw new ArgumentException(string.Format("RevisionEventType {0} not handled in CreateDyntaxaRevisionSpeciesFactChangeUnit(...)", revisionEventType));
            }
        
            dyntaxaRevisionSpeciesFact.TaxonId = taxon.Id;
            dyntaxaRevisionSpeciesFact.RevisionId = revisionId;
            dyntaxaRevisionSpeciesFact.StatusId = newSpeciesFact.GetStatusId();
            // If StatusId is null, then set the other values to null.
            // StatusId=null means that the species fact should be deleted.
            if (dyntaxaRevisionSpeciesFact.StatusId == null) 
            {
                dyntaxaRevisionSpeciesFact.QualityId = null;
                dyntaxaRevisionSpeciesFact.ReferenceId = null;
                dyntaxaRevisionSpeciesFact.Description = null;
            }
            else
            {
                dyntaxaRevisionSpeciesFact.QualityId = newSpeciesFact.GetQualityId();
                dyntaxaRevisionSpeciesFact.ReferenceId = newSpeciesFact.GetReferenceId();
                dyntaxaRevisionSpeciesFact.Description = newSpeciesFact.GetDescription();
            }            
            dyntaxaRevisionSpeciesFact.RevisionEventId = revisionEvent.Id;
            dyntaxaRevisionSpeciesFact.CreatedBy = userContext.User.Id;
            dyntaxaRevisionSpeciesFact.CreatedDate = DateTime.Now;
            dyntaxaRevisionSpeciesFact.ChangedInRevisionEventId = null;
            dyntaxaRevisionSpeciesFact.IsPublished = false;

            if (originalSpeciesFact.GetStatusId().HasValue)
            {
                dyntaxaRevisionSpeciesFact.SpeciesFactExists = true;
                dyntaxaRevisionSpeciesFact.OriginalStatusId = originalSpeciesFact.GetStatusId();
                dyntaxaRevisionSpeciesFact.OriginalQualityId = originalSpeciesFact.GetQualityId();
                dyntaxaRevisionSpeciesFact.OriginalReferenceId = originalSpeciesFact.GetReferenceId();
                dyntaxaRevisionSpeciesFact.OriginalDescription = originalSpeciesFact.GetDescription();
            }
            else
            {
                dyntaxaRevisionSpeciesFact.SpeciesFactExists = false;
            }

            DyntaxaRevisionSpeciesFactChangeUnit changeUnit = new DyntaxaRevisionSpeciesFactChangeUnit
            {
                TaxonRevisionEvent = revisionEvent,
                DyntaxaRevisionSpeciesFact = dyntaxaRevisionSpeciesFact
            };

            return changeUnit;
        }

        /// <summary>
        /// Creates a revision event.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="oldSpeciesFact">The old species fact.</param>
        /// <param name="newSpeciesFact">The new species fact.</param>
        /// <param name="revisionEventType">Type of the revision event.</param>
        /// <returns></returns>
        private TaxonRevisionEvent GetDyntaxaRevisionSpeciesFactRevisionEvent(IUserContext userContext, int revisionId, ITaxon taxon, ISpeciesFact oldSpeciesFact, ISpeciesFact newSpeciesFact, TaxonRevisionEventTypeId revisionEventType)
        {
            DyntaxaSpeciesFactChangeDescription swedishOccurrenceChangeDescription = GetDyntaxaSpeciesFactChangeDescription(oldSpeciesFact, newSpeciesFact);

            string affectedTaxa = string.Format("{0} [{1}]", taxon.ScientificName, taxon.Id);
            // Create revisionvent
            TaxonRevisionEvent taxonRevisionEvent = new TaxonRevisionEvent()
            {
                Type = new TaxonRevisionEventType() { Id = (int)revisionEventType },
                CreatedDate = DateTime.Now,
                CreatedBy = userContext.User.Id,
                RevisionId = revisionId,
                AffectedTaxa = affectedTaxa,
                OldValue = swedishOccurrenceChangeDescription.OldValues,
                NewValue = swedishOccurrenceChangeDescription.NewValues
            };

            return taxonRevisionEvent;
        }

        /// <summary>
        /// Determines whether the species fact is changed.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="taxonId">The taxon identifier.</param>
        /// <param name="speciesFact">The species fact.</param>
        /// <returns>True if changed; otherwise false.</returns>
        private DyntaxaRevisionSpeciesFactChangeStatus GetSpeciesFactChangeStatus(IUserContext userContext, int revisionId, int taxonId, ISpeciesFact speciesFact, out DyntaxaRevisionSpeciesFact revisionSpeciesFact)
        {            
            // Check if species fact change is stored in Taxon database in this revision.
            DyntaxaInternalTaxonServiceManager internalTaxonServiceManager = new DyntaxaInternalTaxonServiceManager();
            revisionSpeciesFact = internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(userContext, speciesFact.Factor.Id, taxonId, revisionId);

            // Value is deleted and a previous change in this revision exist
            if (!speciesFact.GetStatusId().HasValue && revisionSpeciesFact != null && revisionSpeciesFact.StatusId.HasValue) // Previous changes in this revision.
            {
                return DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact;                
            }

            if (speciesFact != null)
            {
                SpeciesFactStateEnum state = GetSpeciesFactState(speciesFact);
                if (state == SpeciesFactStateEnum.Deleted)
                {
                    // Check if already deleted in this revision.
                    if (revisionSpeciesFact != null && !revisionSpeciesFact.StatusId.HasValue)
                    {
                        return DyntaxaRevisionSpeciesFactChangeStatus.NoChanges;
                    }

                    return DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromOriginalSpeciesFact;
                }

                if (state == SpeciesFactStateEnum.Changed || state == SpeciesFactStateEnum.Created)
                {
                    //// Check if species fact change is stored in Taxon database in this revision.
                    //DyntaxaInternalTaxonServiceManager internalTaxonServiceManager = new DyntaxaInternalTaxonServiceManager();
                    //revisionSpeciesFact = internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(userContext, speciesFact.Factor.Id, taxonId, revisionId);

                    if (revisionSpeciesFact == null) // No previous changes in this revision. But this is a change.
                    {
                        return DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromOriginalSpeciesFact;
                    }

                    // Check if any changes.                    
                    if (revisionSpeciesFact.StatusId != speciesFact.GetStatusId() ||
                        revisionSpeciesFact.QualityId != speciesFact.GetQualityId() ||
                        revisionSpeciesFact.ReferenceId != speciesFact.GetReferenceId() ||
                        revisionSpeciesFact.Description != speciesFact.GetDescription())
                    {
                        return DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact;                        
                    }
                    else // Nothing is changed. Don't update!
                    {
                        return DyntaxaRevisionSpeciesFactChangeStatus.NoChanges;
                    }
                }
                else
                {
                    return DyntaxaRevisionSpeciesFactChangeStatus.NoChanges;
                }
            }
            else
            {
                return DyntaxaRevisionSpeciesFactChangeStatus.NoChanges;
            }
        }

        /// <summary>
        /// Updates the dyntaxa revision species fact.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="model">The model.</param>
        /// <param name="speciesFactModel">The species fact model.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <param name="updateSwedishOccurance">if set to <c>true</c> [update swedish occurance].</param>
        /// <param name="updateQuality">if set to <c>true</c> [update quality].</param>
        /// <param name="updateAdditionalQaulity">if set to <c>true</c> [update additional qaulity].</param>
        private void UpdateDyntaxaRevisionSpeciesFact(IUserContext userContext, TaxonSwedishOccuranceBaseViewModel model, SpeciesFactModelManager speciesFactModel, ITaxon taxon, int revisionId)
        {            
            SpeciesFactModelManager oldSpeciesFactModel = new SpeciesFactModelManager(taxon, userContext);
            SpeciesFactModelManager originalSpeciesFactModel = new SpeciesFactModelManager(taxon, userContext);
            ISpeciesFact swedishOccurrenceSpeciesFact = speciesFactModel.SpeciesFact.FirstOrDefault(speciesFact => speciesFact.Factor.Id == (int)FactorId.SwedishOccurrence);
            ISpeciesFact swedishHistorySpeciesFact = speciesFactModel.SpeciesFact.FirstOrDefault(speciesFact => speciesFact.Factor.Id == (int)FactorId.SwedishHistory);
            DyntaxaInternalTaxonServiceManager internalTaxonServiceManager = new DyntaxaInternalTaxonServiceManager();                        
            List<DyntaxaRevisionSpeciesFactChangeUnit> dyntaxaRevisionSpeciesFactUnits = new List<DyntaxaRevisionSpeciesFactChangeUnit>();
            DyntaxaRevisionSpeciesFact revisionSpeciesFactSwedishOccurrence, revisionSpeciesFactSwedishHistory;

            // Swedish occurrence
            var swedishOccurrenceChangeStatus = GetSpeciesFactChangeStatus(userContext, revisionId, taxon.Id, swedishOccurrenceSpeciesFact, out revisionSpeciesFactSwedishOccurrence);
            if (swedishOccurrenceChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact ||
                swedishOccurrenceChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromOriginalSpeciesFact)
            {
                if (swedishOccurrenceChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact)
                {
                    // Update OldSpeciesFact model so that the description in the revision event will be correct.
                    UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(userContext, oldSpeciesFactModel.SwedishOccurrenceSpeciesFact, revisionSpeciesFactSwedishOccurrence);
                }                

                DyntaxaRevisionSpeciesFactChangeUnit swedishOccurrenceRevisionSpeciesFactChangeUnit = CreateDyntaxaRevisionSpeciesFactChangeUnit(userContext, revisionId, taxon, originalSpeciesFactModel.SwedishOccurrenceSpeciesFact, oldSpeciesFactModel.SwedishOccurrenceSpeciesFact, speciesFactModel.SwedishOccurrenceSpeciesFact, TaxonRevisionEventTypeId.ChangeSwedishOccurrence);
                dyntaxaRevisionSpeciesFactUnits.Add(swedishOccurrenceRevisionSpeciesFactChangeUnit);
            }
            
            // Swedish immigration history
            var swedishHistoryChangeStatus = GetSpeciesFactChangeStatus(userContext, revisionId, taxon.Id, swedishHistorySpeciesFact, out revisionSpeciesFactSwedishHistory);
            if (swedishHistoryChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact ||
                swedishHistoryChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromOriginalSpeciesFact)
            {
                if (swedishHistoryChangeStatus == DyntaxaRevisionSpeciesFactChangeStatus.ChangedFromDyntaxaRevisionSpeciesFact)
                {
                    // Update OldSpeciesFact model so that the description in the revision event will be correct.
                    UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(userContext, oldSpeciesFactModel.SwedishHistorySpeciesFact, revisionSpeciesFactSwedishHistory);
                }

                DyntaxaRevisionSpeciesFactChangeUnit swedishHistoryRevisionSpeciesFactChangeUnit = CreateDyntaxaRevisionSpeciesFactChangeUnit(userContext, revisionId, taxon, originalSpeciesFactModel.SwedishHistorySpeciesFact, oldSpeciesFactModel.SwedishHistorySpeciesFact, speciesFactModel.SwedishHistorySpeciesFact, TaxonRevisionEventTypeId.ChangeSwedishImmigrationHistory);
                dyntaxaRevisionSpeciesFactUnits.Add(swedishHistoryRevisionSpeciesFactChangeUnit);
            }

            if (dyntaxaRevisionSpeciesFactUnits.Count > 0)
            {
                try
                {
                    using (ITransaction transaction = userContext.StartTransaction())
                    {
                        foreach (var changeUnit in dyntaxaRevisionSpeciesFactUnits)
                        {
                            var createdEvent = internalTaxonServiceManager.CreateCompleteRevisionEvent(userContext, changeUnit.TaxonRevisionEvent);
                            changeUnit.TaxonRevisionEvent.Id = createdEvent.Id;
                            changeUnit.DyntaxaRevisionSpeciesFact.RevisionEventId = createdEvent.Id;                            
                            internalTaxonServiceManager.CreateDyntaxaRevisionSpeciesFact(userContext, changeUnit.DyntaxaRevisionSpeciesFact);    
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception e)
                {
                    Exception ex = new Exception(Resources.DyntaxaResource.SharedNotPossibleToUpdateSpeciesFactError, e);
                    throw ex;
                }
            }
        }

        public static void UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(IUserContext userContext, SpeciesFact oldSpeciesFact, DyntaxaRevisionSpeciesFact revisionSpeciesFact)
        {
            if (oldSpeciesFact == null || revisionSpeciesFact == null)
            {
                return;
            }

            oldSpeciesFact.SetStatus(revisionSpeciesFact.StatusId);
            oldSpeciesFact.SetQuality(userContext, revisionSpeciesFact.QualityId);
            oldSpeciesFact.SetReference(userContext, revisionSpeciesFact.ReferenceId);
            oldSpeciesFact.SetDescription(revisionSpeciesFact.Description);
        }

        /// <summary>
        /// Gets the dyntaxa species fact change description.
        /// </summary>
        /// <param name="oldSpeciesFact">The old species fact.</param>
        /// <param name="newSpeciesFact">The new species fact.</param>
        /// <returns>Change description.</returns>
        public static DyntaxaSpeciesFactChangeDescription GetDyntaxaSpeciesFactChangeDescription(ISpeciesFact oldSpeciesFact, ISpeciesFact newSpeciesFact)
        {            
            List<string> oldValuesChangeList = new List<string>();
            List<string> newValuesChangeList = new List<string>();
            
            if (oldSpeciesFact.GetStatusId() != newSpeciesFact.GetStatusId())
            {                
                oldValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeStatusLabel, oldSpeciesFact.GetStatusLabel()));
                newValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeStatusLabel, newSpeciesFact.GetStatusLabel()));
            }

            if (oldSpeciesFact.GetQualityId() != newSpeciesFact.GetQualityId())
            {
                oldValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeQualityLabel, oldSpeciesFact.GetQualityName()));
                newValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeQualityLabel, newSpeciesFact.GetQualityName()));
            }

            if (oldSpeciesFact.GetReferenceId() != newSpeciesFact.GetReferenceId())
            {
                oldValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeReferenceLabel, oldSpeciesFact.GetReferenceName()));
                newValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeReferenceLabel, newSpeciesFact.GetReferenceName()));
            }

            if (oldSpeciesFact.GetDescription() != newSpeciesFact.GetDescription())
            {
                oldValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeCommentLabel, oldSpeciesFact.GetDescription()));
                newValuesChangeList.Add(string.Format(new NullFormat(), "{0}={1}", Resources.DyntaxaResource.DyntaxaSpeciesFactChangeCommentLabel, newSpeciesFact.GetDescription()));
            }

            DyntaxaSpeciesFactChangeDescription changeDescription = new DyntaxaSpeciesFactChangeDescription();
            if (oldSpeciesFact.GetStatusId() != null)
            {
                // If there exists previous values.
                changeDescription.OldValues = string.Join(", ", oldValuesChangeList);
            }
            else
            {
                // If there is no previous values
                changeDescription.OldValues = Resources.DyntaxaResource.DyntaxaSpeciesFactChangeDoNotExist;
            }

            if (newSpeciesFact.GetStatusId() != null)
            {
                // A new value is set.
                changeDescription.NewValues = string.Join(", ", newValuesChangeList);
            }
            else
            {
                // The value is deleted
                changeDescription.NewValues = Resources.DyntaxaResource.DyntaxaSpeciesFactDelete;
            }
            
            return changeDescription;
        }

        /// <summary>
        /// Gets the edit swedish occurance view model.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="revisionId">The revision identifier.</param>
        /// <returns></returns>
        public TaxonSwedishOccuranceEditViewModel GetEditSwedishOccuranceViewModel(IUserContext userContext, ITaxon taxon, int? revisionId)
        {
            TaxonSwedishOccuranceEditViewModel model = new TaxonSwedishOccuranceEditViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                        
                    model.TaxonId = taxon.Id.ToString();
                    model.TaxonGuid = taxon.Guid;
                    model.TaxonReferencesList = new List<IReference>();

                    model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
                    model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();
                    model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
                    foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(userContext))
                    {
                        IReference reference = referenceRelation.GetReference(userContext);
                        Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                        model.SwedishOccurrenceReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                        model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                    }
                            
                    model.SpeciesFactError = false;
                         
                    // Check if taxon has sort order higher than "Art".
                    model.EnableSpeciesFact = false;
                    if (taxon.Category.SortOrder >= Resources.DyntaxaSettings.Default.SpeciesCategorySortOrder)
                    {
                        model.EnableSpeciesFact = true;

                        // Get specices fact information
                        try
                        {
                            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);                            
                            GetSpeciesFact(userContext, revisionId.Value, taxon.Id, model, false, speciesFactModel);
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.SpeciesFactError = true;
                            model.EnableSpeciesFact = false;
                            model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;

                            return model;
                        }
                    }
                    else
                    {
                        model.SpeciesFactError = true;
                        model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }

            return model;
        }

        /// <summary>
        /// Reloads the edit swedish occurance view model.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public TaxonSwedishOccuranceEditViewModel ReloadEditSwedishOccuranceViewModel(IUserContext userContext, int? revisionId, ITaxon taxon, TaxonSwedishOccuranceEditViewModel model)
        {
             IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    model.TaxonReferencesList = new List<IReference>();
                    model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
                    model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();                    
                    model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));

                    foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(userContext))
                    {
                        IReference reference = referenceRelation.GetReference(userContext);
                        Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                        model.SwedishOccurrenceReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                        model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                    }

                    model.SpeciesFactError = false;
                    // Check if taxon has sortorder higher than "art"
                    model.EnableSpeciesFact = false;

                    if (taxon.Category.SortOrder >= Resources.DyntaxaSettings.Default.SpeciesCategorySortOrder)
                    {
                        model.EnableSpeciesFact = true;

                        // Get specices fact information
                        try
                        {
                            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
                            ReloadSpeciesFact(loggedInUser, revisionId.Value, taxon.Id, model, speciesFactModel);
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.SpeciesFactError = true;
                            model.EnableSpeciesFact = false;
                            model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;
                        }
                    }
                    else
                    {
                        model.SpeciesFactError = true;
                        model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }

            return model;
        }

        public TaxonEditQualityViewModel GetTaxonEditQualityViewModel(IUserContext userContext, ITaxon taxon)
        {
            TaxonEditQualityViewModel model = new TaxonEditQualityViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();

                    model.TaxonId = taxon.Id.ToString();
                    model.TaxonGuid = taxon.Guid;
                  
                     // Get quality information
                    try
                    {
                        SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
                        try
                        {
                            if (speciesFactModel.QualityStatus.IsNotNull())
                            {
                                model.TaxonQualityId = speciesFactModel.QualityStatus.Id;
                            }
                            else if (speciesFactModel.QualityStatusList.IsNotNull() && speciesFactModel.QualityStatusList.Count > 1)
                            {
                                model.TaxonQualityId = speciesFactModel.QualityStatusList.ElementAt(0).Id;
                            }
                            else
                            {
                                model.TaxonQualityId = 0;
                            }

                            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
                            foreach (var status in speciesFactModel.QualityStatusList)
                            {
                                model.TaxonQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                            }

                            model.TaxonQualityDescription = speciesFactModel.QualityDescription;
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                        }
                    }
                    catch (Exception)
                    {
                        // Show data with an error message
                        model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                    }                
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }

            return model;
        }

        public TaxonEditQualityViewModel ReloadTaxonEditQualityViewModel(IUserContext userContext, ITaxon taxon, TaxonEditQualityViewModel model)
        {
            IUserContext loggedInUser = userContext;
            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                  
                    // Get specices fact information
                    try
                    {
                        SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
                        try
                        {
                            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
                            foreach (var status in speciesFactModel.QualityStatusList)
                            {
                                model.TaxonQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                            }

                            model.TaxonQualityDescription = speciesFactModel.QualityDescription;
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                        }
                    }
                    catch (Exception)
                    {
                        // Show data with an error message
                        model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        public TaxonEditViewModel ReloadTaxonEditViewModel(IUserContext userContext, int? revisionId, ITaxon taxon, TaxonEditViewModel model, ITaxonCategory category, TaxonCategoryList taxonCategories)
        {
            IUserContext loggedInUser = userContext;
            model.SwedishImmigrationHistoryQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryStatusList = new List<TaxonDropDownModelHelper>();
            model.SwedishOccurrenceQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
            model.SwedishOccurrenceStatusList = new List<TaxonDropDownModelHelper>();
            model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();

            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();

                    model.TaxonReferencesList = new List<IReference>();
                   
                    model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();

                    foreach (ITaxonCategory taxonCategory in taxonCategories)
                    {
                        if (category.IsNotNull() || category.Id.IsNotNull())
                        {
                            model.TaxonCategoryList.Add(new TaxonDropDownModelHelper(taxonCategory.Id, taxonCategory.Name));
                        } 
                    }

                    model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
                    model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();
                    model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
                    foreach (IReferenceRelation referenceRelation in taxon.GetReferenceRelations(userContext))
                    {
                        IReference reference = referenceRelation.GetReference(userContext);
                        Int32 year = reference.Year.HasValue ? reference.Year.Value : -1;
                        model.SwedishOccurrenceReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                        model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(reference.Id, reference.Name + " " + year));
                    }

                    model.TaxonReferencesList = new List<IReference>();

                    //model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
                    //model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();

                    //foreach (ReferenceRelation referenceRelation in taxon.References)
                    //{
                    //    referenceRelation.Init();
                    //    model.SwedishOccurrenceReferenceList.Add(new TaxonDropDownModelHelper(referenceRelation.Reference.Id, referenceRelation.Reference.Name + " " + referenceRelation.Reference.Year));
                    //    model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(referenceRelation.Reference.Id, referenceRelation.Reference.Name + " " + referenceRelation.Reference.Year));
                    //}

                    model.SpeciesFactError = false;
                    // Check if taxon has sortorder higher "art"
                    model.EnableSpeciesFact = false;
                    
                    // Get specices fact information
                    try
                    {
                        SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, loggedInUser);
                        try
                        {
                            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
                            foreach (var status in speciesFactModel.QualityStatusList)
                            {
                                model.TaxonQualityList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
                            }

                            model.TaxonQualityDescription = speciesFactModel.QualityDescription;
                        }
                        catch (Exception)
                        {
                            // Show data with an error message
                            model.SpeciesFactError = true;
                            model.EnableSpeciesFact = false;
                            model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                        }
                              
                        if (taxon.Category.SortOrder >= Resources.DyntaxaSettings.Default.SpeciesCategorySortOrder)
                        {
                            model.EnableSpeciesFact = true;
                            try
                            {
                                ReloadSpeciesFact(userContext, revisionId.Value, taxon.Id, model, speciesFactModel);
                            }
                            catch (Exception)
                            {
                                // Show data with an error message
                                model.SpeciesFactError = true;
                                model.EnableSpeciesFact = false;
                                model.ErrorMessage = Resources.DyntaxaResource.SharedSpeciesFactError;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        // Show data with an error message
                        model.SpeciesFactError = true;
                        model.EnableSpeciesFact = false;
                        model.ErrorMessage = Resources.DyntaxaResource.SharedNotPossibleToReadSpeciesFactError;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        public TaxonDropParentViewModel GetDropParentViewModel(IUserContext userContext, ITaxon taxon, ITaxonRevision taxonRevision, bool isReloaded)
        {
            TaxonDropParentViewModel model = new TaxonDropParentViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (taxonRevision.IsNotNull())
                    {
                        model.RevisionErrorId = taxonRevision.Id.ToString();                        
                        model.RevisionId = taxonRevision.Id.ToString();
                        model.TaxonId = taxon.Id.ToString();
                        model.EnableSaveDeleteParentTaxonButton = false;
                        model.IsReloaded = isReloaded;
                        //ITaxonCategory category = CoreData.TaxonManager.GetTaxonCategoryById(loggedInUser, taxon.Category);
                        model.TaxonList = new List<TaxonParentViewModelHelper>();

                        if (taxon.IsNotNull() && taxonRevision.IsNotNull())
                        {
                            IList<ITaxonRelation> parentRelations = taxon.GetCheckedOutChangesParentTaxa(userContext);
                            foreach (ITaxonRelation relation in parentRelations)
                            {
                                ITaxon parentTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, relation.ParentTaxon.Id);
                                TaxonParentViewModelHelper parentTaxonModel = new TaxonParentViewModelHelper();
                                parentTaxonModel.Category = parentTaxon.Category.Name;
                                parentTaxonModel.SortOrder = parentTaxon.Category.SortOrder;
                                parentTaxonModel.CommonName = parentTaxon.CommonName.IsNotEmpty()
                                                                  ? parentTaxon.CommonName
                                                                  : string.Empty;
                                parentTaxonModel.ScientificName = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : string.Empty;
                                parentTaxonModel.TaxonId = parentTaxon.Id.ToString();
                                parentTaxonModel.IsMain = relation.IsMainRelation;
                                model.TaxonList.Add(parentTaxonModel);
                            }
                            if (model.TaxonList.Count > 1)
                            {
                                model.EnableSaveDeleteParentTaxonButton = true;
                            }
                        }

                        return model;
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        public IList<TaxonParentViewModelHelper> GetParents(IUserContext userContext, ITaxon taxon)
        {
            IList<TaxonParentViewModelHelper> parents = new List<TaxonParentViewModelHelper>();
            if (taxon.IsNotNull())
            {
                IList<ITaxonRelation> parentRelations = taxon.GetCheckedOutChangesParentTaxa(userContext);
                IList<ITaxon> taxa = new List<ITaxon>();
                foreach (ITaxonRelation relation in parentRelations)
                {
                    taxa.Add(CoreData.TaxonManager.GetTaxon(userContext, relation.ParentTaxon.Id));
                }
                if (taxa.IsNotNull())
                {
                    foreach (var parentTaxon in taxa)
                    {
                        TaxonParentViewModelHelper parentTaxonModel = new TaxonParentViewModelHelper();
                        parentTaxonModel.Category = parentTaxon.Category.Name;
                        parentTaxonModel.SortOrder = parentTaxon.Category.SortOrder;
                        parentTaxonModel.CommonName = parentTaxon.CommonName.IsNotEmpty()
                                                            ? parentTaxon.CommonName
                                                            : string.Empty;
                        parentTaxonModel.ScientificName = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : string.Empty;
                        parentTaxonModel.TaxonId = parentTaxon.Id.ToString();
                        parents.Add(parentTaxonModel);
                    }
                }
            }
            return parents;
        }

        public TaxonAddParentViewModel GetAddParentViewModel(IUserContext userContext, ITaxon taxon, ITaxonRevision taxonRevision, bool isOkToAdd = true)
        {
            TaxonAddParentViewModel model = new TaxonAddParentViewModel();
            model.TaxonCategories = new Dictionary<int, TaxonCategoryViewModel>();
            model.TaxonDictionary = new Dictionary<int, List<TaxonParentViewModelHelper>>();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (taxonRevision.IsNotNull())
                    {
                        model.RevisionErrorId = taxonRevision.Id.ToString();

                        model.AvailableParents = new List<TaxonParentViewModelHelper>();
                        // get existing parent for taxon
                        List<ITaxonRelation> parentTaxa = taxon.GetCheckedOutChangesParentTaxa(userContext);
                        foreach (var taxonRelation in parentTaxa)
                        {
                            ITaxon parentTaxon = CoreData.TaxonManager.GetTaxon(loggedInUser, taxonRelation.ParentTaxon.Id);
                            TaxonParentViewModelHelper existingParent = new TaxonParentViewModelHelper();
                            existingParent.Category = parentTaxon.Category.Name;
                            existingParent.CommonName = parentTaxon.CommonName.IsNotEmpty() ? parentTaxon.CommonName : string.Empty;
                            existingParent.ScientificName = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : string.Empty;
                            existingParent.SortOrder = taxonRelation.SortOrder;
                            existingParent.TaxonId = parentTaxon.Id.ToString();
                            model.AvailableParents.Add(existingParent);
                        }                        

                        model.RevisionId = taxonRevision.Id.ToString();
                        model.TaxonId = taxon.Id.ToString();
                        model.EnableSaveNewParentTaxonButton = false;
                        model.IsOkToAdd = isOkToAdd;
                        //  ITaxonCategory category = CoreData.TaxonManager.GetTaxonCategoryById(loggedInUser, taxon.Category);
                        model.TaxonList = new List<TaxonParentViewModelHelper>();

                        if (taxon.IsNotNull() && taxonRevision.IsNotNull())
                        {
                            TaxonList taxa = taxon.GetTaxaPossibleParents(loggedInUser, taxonRevision);
                            if (taxa.IsNotNull() && taxa.Count > 0)
                            {                                
                                foreach (ITaxon parentTaxon in taxa)
                                {
                                    TaxonParentViewModelHelper parentTaxonModel = new TaxonParentViewModelHelper();
                                    parentTaxonModel.Category = parentTaxon.Category.Name;
                                    parentTaxonModel.SortOrder = parentTaxon.Category.SortOrder;
                                    parentTaxonModel.CommonName = parentTaxon.CommonName.IsNotEmpty() ? parentTaxon.CommonName : string.Empty;
                                    parentTaxonModel.ScientificName = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : string.Empty;
                                    parentTaxonModel.TaxonId = parentTaxon.Id.ToString();
                                    model.TaxonList.Add(parentTaxonModel);

                                    // Add taxon category
                                    if (!model.TaxonCategories.ContainsKey(parentTaxon.Category.Id))
                                    {
                                        TaxonCategoryViewModel taxonCategory = new TaxonCategoryViewModel(parentTaxon.Category.Id, parentTaxon.Category.Name);
                                        model.TaxonCategories.Add(taxonCategory.Id, taxonCategory);
                                    }

                                    // Add taxon model to dictionary
                                    if (!model.TaxonDictionary.ContainsKey(parentTaxon.Category.Id))
                                    {
                                        model.TaxonDictionary.Add(parentTaxon.Category.Id, new List<TaxonParentViewModelHelper>());
                                    }
                                    model.TaxonDictionary[parentTaxon.Category.Id].Add(parentTaxonModel);
                                }
                                if (model.TaxonList.Count > 0)
                                {
                                    model.EnableSaveNewParentTaxonButton = true;
                                }

                                // sort taxon dictionary
                                foreach (KeyValuePair<int, List<TaxonParentViewModelHelper>> pair in model.TaxonDictionary)
                                {
                                    pair.Value.Sort(CompareTaxonParentByName);
                                }
                            }
                        }
                        model.DialogTextPopUpText = Resources.DyntaxaResource.TaxonAddParentErrorText;
                        model.DialogTitlePopUpText = Resources.DyntaxaResource.TaxonAddParentHeaderText;
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        private int CompareTaxonParentByName(TaxonParentViewModelHelper x, TaxonParentViewModelHelper y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                return -1;
            }
            if (y == null)
            {
                return 1;
            }
            int retval = System.String.Compare(x.ScientificName, y.ScientificName, System.StringComparison.Ordinal);
            if (retval != 0)
            {
                return retval;
            }
            return System.String.Compare(x.CommonName, y.CommonName, System.StringComparison.Ordinal);
        }

        public TaxonDeleteViewModel GetDeleteTaxonViewModel(IUserContext userContext, ITaxon taxon, int revisionId)
        {
            TaxonDeleteViewModel model = new TaxonDeleteViewModel();
            IUserContext loggedInUser = userContext;
            if (loggedInUser.IsNotNull())
            {
                if (taxon.IsNotNull() && taxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = taxon.Id.ToString();
                    if (revisionId.IsNotNull())
                    {
                        // 1. first we check selected taxon if it has any children
                        model.IsSelectedTaxonChildless = true;
                        if (taxon.GetNearestChildTaxonRelations(userContext).IsNotEmpty())
                        {
                            model.IsSelectedTaxonChildless = false;
                        }

                        model.Category = taxon.Category.Name;
                        model.SortOrder = taxon.Category.SortOrder;
                        model.CommonName = taxon.CommonName.IsNotEmpty() ? taxon.CommonName : string.Empty;
                        model.ScientificName = taxon.ScientificName.IsNotEmpty() ? taxon.ScientificName : string.Empty;
                        model.TaxonId = taxon.Id.ToString();
                        model.TaxonErrorId = taxon.Id.ToString();
                        model.RevisionId = revisionId.ToString();
                        model.RevisionErrorId = revisionId.ToString();
                        model.DialogTextPopUpText = Resources.DyntaxaResource.TaxonDeleteInfoText;
                        model.DialogTitlePopUpText = Resources.DyntaxaResource.TaxonDeleteInfoHeaderText;
                    }
                    else
                    {
                        model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidRevision;
                    }
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }

        #region ModelManger helper methods

        /// <summary>
        /// Get the species fact data
        /// </summary>
        /// <param name="model"></param>
        private void GetSpeciesFact(IUserContext userContext, int revisionId, int taxonId, TaxonSwedishOccuranceBaseViewModel model, bool isTaxonNew, SpeciesFactModelManager speciesFactModel, bool overwriteWithDyntaxaRevisionSpeciesFactIfExists = true)
        {            
            model.SwedishOccurrenceDescription = speciesFactModel.SwedishOccurrenceDescription;

            // Swedish occurance
            if (speciesFactModel.SwedishOccurrence.IsNotNull())
            {
                model.SwedishOccurrenceStatusId = speciesFactModel.SwedishOccurrence.Id;
            }
            //else if (speciesModel.SwedishOccurrenceList.IsNotNull() && speciesModel.SwedishOccurrenceList.Count > 5)
            //{
            //   model.SwedishOccurrenceStatusId = speciesModel.SwedishOccurrenceList.ElementAt(5).Id;
            //}
            else if (speciesFactModel.SwedishOccurrenceList.IsNotNull() && speciesFactModel.SwedishOccurrenceList.Count > 0)
            {
                model.SwedishOccurrenceStatusId = speciesFactModel.SwedishOccurrenceList.ElementAt(0).Id;
            }
            else
            {
                model.SwedishOccurrenceStatusId = 0;
            }
            model.SwedishOccurrenceStatusList = new List<TaxonDropDownModelHelper>();
            foreach (var status in speciesFactModel.SwedishOccurrenceList)
            {
                model.SwedishOccurrenceStatusList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
            }

            // Swedish  quality occurance use of isTaxonNew to fix bug/Set of initial value in Artfakat setting default quality value to 3 ie "Godtagbar"
            if (speciesFactModel.SwedishOccurrenceQuality.IsNotNull() && !isTaxonNew)
            {
                model.SwedishOccurrenceQualityId = speciesFactModel.SwedishOccurrenceQuality.Id;
            }
            else if (speciesFactModel.SwedishOccurrenceQualityList.IsNotNull() && speciesFactModel.SwedishOccurrenceQualityList.Count > 0)
            {
                model.SwedishOccurrenceQualityId = speciesFactModel.SwedishOccurrenceQualityList.ElementAt(0).Id;
            }
            else
            {
                model.SwedishOccurrenceQualityId = 0;
            }
            model.SwedishOccurrenceQualityList = new List<TaxonDropDownModelHelper>();
            foreach (var quality in speciesFactModel.SwedishOccurrenceQualityList)
            {
                model.SwedishOccurrenceQualityList.Add(new TaxonDropDownModelHelper(quality.Id, quality.Name));
            }

            // Swedish  quality occurance reference
            if (speciesFactModel.SwedishOccurrenceReference.IsNotNull())
            {
                model.SwedishOccurrenceReferenceId = speciesFactModel.SwedishOccurrenceReference.Id;
                bool referenceExists = false;
                foreach (TaxonDropDownModelHelper reference in model.SwedishOccurrenceReferenceList)
                {
                    if (reference.Id == speciesFactModel.SwedishOccurrenceReference.Id)
                    {
                        referenceExists = true;
                        break;
                    }
                }
                if (!referenceExists)
                {
                    model.SwedishOccurrenceReferenceList.Add(
                        new TaxonDropDownModelHelper(
                            speciesFactModel.SwedishOccurrenceReference.Id,
                            speciesFactModel.SwedishOccurrenceReference.Name + " " + speciesFactModel.SwedishOccurrenceReference.Year));
                }
            }

            // Swedish history
            bool historyExists = true;
            model.SwedishImmigrationHistoryDescription = speciesFactModel.SwedishHistoryDescription;
            
            // Swedish occurance
            if (speciesFactModel.SwedishHistory.IsNotNull() && !isTaxonNew)
            {
                model.SwedishImmigrationHistoryStatusId = speciesFactModel.SwedishHistory.Id;
            }
            else if (speciesFactModel.SwedishHistory.IsNull() && !isTaxonNew)
            {
                // Set status to not avaliable..
                model.SwedishImmigrationHistoryStatusId = 0;
                historyExists = false;
            }
            else if (speciesFactModel.SwedishHistoryList.IsNotNull() && speciesFactModel.SwedishHistoryList.Count > 1)
            {
                model.SwedishImmigrationHistoryStatusId = speciesFactModel.SwedishHistoryList.ElementAt(1).Id;
            }
            else
            {
                // Set status to not avaliable..
                model.SwedishImmigrationHistoryStatusId = 0;
                historyExists = false;
            }

            model.SwedishImmigrationHistoryStatusList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryStatusList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            foreach (var status in speciesFactModel.SwedishHistoryList)
            {
                model.SwedishImmigrationHistoryStatusList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
            }

            if (speciesFactModel.SwedishHistoryQuality.IsNotNull() && !isTaxonNew && historyExists)
            {
                model.SwedishImmigrationHistoryQualityId = speciesFactModel.SwedishHistoryQuality.Id;
            }
            // Set initial value to "Ej angivet"
            else if (isTaxonNew || speciesFactModel.SwedishHistoryQuality.IsNull() || !historyExists)
            {
                model.SwedishImmigrationHistoryQualityId = 1; // 1 = Status Id for high quality
            }
            // Swedish  history quality use of isTaxonNew to fix bug/Set of initial value in Artfakat setting default quality value to 3 ie "Godtagbar" No used any more...
            else if (speciesFactModel.SwedishOccurrenceQualityList.IsNotNull() && speciesFactModel.SwedishHistoryQualityList.Count > 0)
            {
                model.SwedishImmigrationHistoryQualityId = speciesFactModel.SwedishHistoryQualityList.ElementAt(0).Id;
            }
            else
            {
                model.SwedishImmigrationHistoryQualityId = 0; 
            }
            model.SwedishImmigrationHistoryQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryQualityList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            foreach (var quality in speciesFactModel.SwedishHistoryQualityList)
            {
                model.SwedishImmigrationHistoryQualityList.Add(new TaxonDropDownModelHelper(quality.Id, quality.Name));
            }

            // Swedish  history quality reference
            if (speciesFactModel.SwedishHistoryReference.IsNotNull() && historyExists)
            {
                model.SwedishImmigrationHistoryReferenceId = speciesFactModel.SwedishHistoryReference.Id;
                 bool referenceExists = false;
                foreach (TaxonDropDownModelHelper reference in model.SwedishImmigrationHistoryReferenceList)
                {
                    if (reference.Id == speciesFactModel.SwedishHistoryReference.Id)
                    {
                        referenceExists = true;
                        break;
                    }
                }
                if (!referenceExists)
                {
                    model.SwedishImmigrationHistoryReferenceList.Add(
                        new TaxonDropDownModelHelper(
                            speciesFactModel.SwedishHistoryReference.Id,
                            speciesFactModel.SwedishHistoryReference.Name + " " + speciesFactModel.SwedishHistoryReference.Year));
                }
            }
            else
            {
                model.SwedishImmigrationHistoryReferenceId = 0;
            }
            if (isTaxonNew && model.SwedishImmigrationHistoryReferenceId == 0)
            {
                if (model.SwedishImmigrationHistoryReferenceList.Count >= 2)
                {
                    model.SwedishImmigrationHistoryReferenceId = model.SwedishImmigrationHistoryReferenceList[1].Id;
                }
            }

            // Set default text if no data exist in reference lists
            if (model.SwedishOccurrenceReferenceList.IsNull() && model.SwedishOccurrenceReferenceList.Count == 0)
            {
                model.SwedishOccurrenceReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedTaxonSelecteReferenceDropDownListText));
                model.SwedishOccurrenceReferenceId = 0;
            }
            if (model.SwedishImmigrationHistoryReferenceList.IsNull() && model.SwedishImmigrationHistoryReferenceList.Count == 0)
            {
                model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedTaxonSelecteReferenceDropDownListText));
                model.SwedishImmigrationHistoryReferenceId = 0;
            }

            if (overwriteWithDyntaxaRevisionSpeciesFactIfExists)
            {
                // Set species fact from revision data if any changes is made.
                OverwriteSpeciesFactDataWithDyntaxaRevisionSpeciesFact(userContext, revisionId, taxonId, model);
            }
        }

        private void OverwriteSpeciesFactDataWithDyntaxaRevisionSpeciesFact(IUserContext userContext, int revisionId, int taxonId, TaxonSwedishOccuranceBaseViewModel model)
        {
            // Set species fact from revision data if any changes is made.

            // Try get revision species fact information.
            bool isInRevision = DyntaxaHelper.IsInRevision(userContext, revisionId);
            bool isTaxonInRevision = CoreData.TaxonManager.GetTaxon(userContext, taxonId).IsInRevision;
            // Todo Om isTaxonInRevision=true så måste vi ha reda på vilken RevisionId som den är med i. Skicka med som extra data från tjänsten eller ny funktion i tjänsten som returnerar RevisionId utifrån TaxonId.                            

            DyntaxaInternalTaxonServiceManager internalTaxonServiceManager = new DyntaxaInternalTaxonServiceManager();

            // Check if Swedish occurrence is stored in Taxon database in this revision.
            DyntaxaRevisionSpeciesFact swedishOccurrenceRevisionSpeciesFact =
                internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                    userContext,
                    (Int32)FactorId.SwedishOccurrence, 
                    taxonId, 
                    revisionId);
            if (swedishOccurrenceRevisionSpeciesFact != null)
            {
                //speciesFactModel.SwedishOccurrenceSpeciesFact.SetStatus(swedishOccurrenceRevisionSpeciesFact.StatusId);
                if (swedishOccurrenceRevisionSpeciesFact.StatusId.HasValue)
                {
                    model.SwedishOccurrenceStatusId = swedishOccurrenceRevisionSpeciesFact.StatusId.Value;
                    model.SwedishOccurrenceQualityId = swedishOccurrenceRevisionSpeciesFact.QualityId.Value;
                    model.SwedishOccurrenceReferenceId = swedishOccurrenceRevisionSpeciesFact.ReferenceId.Value;
                    model.SwedishOccurrenceDescription = swedishOccurrenceRevisionSpeciesFact.Description;
                }                
            }

            // Check if Swedish immigration history is stored in Taxon database in this revision.                            
            DyntaxaRevisionSpeciesFact swedishHistoryRevisionSpeciesFact =
                internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                    userContext,
                    (Int32)FactorId.SwedishHistory, 
                    taxonId, 
                    revisionId);
            if (swedishHistoryRevisionSpeciesFact != null)
            {
                //speciesFactModel.SwedishHistorySpeciesFact.SetStatus(swedishHistoryRevisionSpeciesFact.StatusId);
                if (swedishHistoryRevisionSpeciesFact.StatusId.HasValue)
                {
                    model.SwedishImmigrationHistoryStatusId = swedishHistoryRevisionSpeciesFact.StatusId.Value;
                    model.SwedishImmigrationHistoryQualityId = swedishHistoryRevisionSpeciesFact.QualityId.Value;
                    model.SwedishImmigrationHistoryReferenceId = swedishHistoryRevisionSpeciesFact.ReferenceId.Value;
                    model.SwedishImmigrationHistoryDescription = swedishHistoryRevisionSpeciesFact.Description;
                }
                else // Swedish History Fact is set to be deleted
                {
                    model.SwedishImmigrationHistoryStatusId = 0;
                    model.SwedishImmigrationHistoryQualityId = 0;
                    model.SwedishImmigrationHistoryReferenceId = 0;
                    model.SwedishImmigrationHistoryDescription = null;
                }
            }
        }

        /// <summary>
        /// Get the species fact data
        /// </summary>
        /// <param name="model"></param>
        private void ReloadSpeciesFact(IUserContext userContext, int revisionId, int taxonId, TaxonSwedishOccuranceBaseViewModel model, SpeciesFactModelManager speciesFactModel, bool overwriteWithDyntaxaRevisionSpeciesFactIfExists = true)
        {           
            model.SwedishOccurrenceStatusList = new List<TaxonDropDownModelHelper>();
            foreach (var status in speciesFactModel.SwedishOccurrenceList)
            {
                model.SwedishOccurrenceStatusList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
            }
            
            model.SwedishOccurrenceQualityList = new List<TaxonDropDownModelHelper>();
            foreach (var quality in speciesFactModel.SwedishOccurrenceQualityList)
            {
                model.SwedishOccurrenceQualityList.Add(new TaxonDropDownModelHelper(quality.Id, quality.Name));
            }

            // Swedish  quality occurance reference
            if (speciesFactModel.SwedishOccurrenceReference.IsNotNull())
            {
                 model.SwedishOccurrenceReferenceList.Add(
                    new TaxonDropDownModelHelper(
                        speciesFactModel.SwedishOccurrenceReference.Id,
                        speciesFactModel.SwedishOccurrenceReference.Name + " " + speciesFactModel.SwedishOccurrenceReference.Year));
            }

            // Swedish history
             model.SwedishImmigrationHistoryStatusList = new List<TaxonDropDownModelHelper>();
             model.SwedishImmigrationHistoryStatusList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
             foreach (var status in speciesFactModel.SwedishHistoryList)
            {
                model.SwedishImmigrationHistoryStatusList.Add(new TaxonDropDownModelHelper(status.Id, status.Label));
            }

            // Swedish  history quality             
            model.SwedishImmigrationHistoryQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryQualityList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            foreach (var quality in speciesFactModel.SwedishHistoryQualityList)
            {
                model.SwedishImmigrationHistoryQualityList.Add(new TaxonDropDownModelHelper(quality.Id, quality.Name));
            }

            // Swedish  history quality reference
            if (speciesFactModel.SwedishHistoryReference.IsNotNull())
            {
                model.SwedishImmigrationHistoryReferenceList.Add(
                    new TaxonDropDownModelHelper(
                        speciesFactModel.SwedishHistoryReference.Id,
                        speciesFactModel.SwedishHistoryReference.Name + " " + speciesFactModel.SwedishHistoryReference.Year));
            }

            if (overwriteWithDyntaxaRevisionSpeciesFactIfExists)
            {
                // Set species fact from revision data if any changes is made.
                OverwriteSpeciesFactDataWithDyntaxaRevisionSpeciesFact(userContext, revisionId, taxonId, model);
            }            
        }

        public void InitQualityAndCategoryDropDown(TaxonEditViewModel model)
        {
            model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
        }

        public void InitQualityDropDown(TaxonEditQualityViewModel model)
        {
            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
        }

        public void InitSpeciesDropDown(TaxonSwedishOccuranceBaseViewModel model)
        {
            model.SwedishImmigrationHistoryQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryQualityList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            model.SwedishImmigrationHistoryReferenceList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryReferenceList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            model.SwedishImmigrationHistoryStatusList = new List<TaxonDropDownModelHelper>();
            model.SwedishImmigrationHistoryStatusList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedNotSpecifiedText));
            model.SwedishOccurrenceQualityList = new List<TaxonDropDownModelHelper>();
            model.SwedishOccurrenceReferenceList = new List<TaxonDropDownModelHelper>();
            model.SwedishOccurrenceStatusList = new List<TaxonDropDownModelHelper>();
            model.TaxonQualityList = new List<TaxonDropDownModelHelper>();
        }
        #endregion

        public TaxonAddViewModel ReloadTaxonAddViewModel(IUserContext userContext, ITaxon parentTaxon, TaxonAddViewModel model, ITaxonCategory category, IList<ITaxonCategory> possibleTaxonCategories)
        {
            IUserContext loggedInUser = userContext;
            model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();
            if (loggedInUser.IsNotNull())
            {
                if (parentTaxon.IsNotNull() && parentTaxon.Id.IsNotNull())
                {
                    model.TaxonErrorId = parentTaxon.Id.ToString();
                    model.TaxonCategoryList = new List<TaxonDropDownModelHelper>();

                    if (category.IsNotNull())
                    {
                        model.TaxonCategoryId = category.Id;
                    }
                    else
                    {
                        model.TaxonCategoryList.Add(new TaxonDropDownModelHelper(0, Resources.DyntaxaResource.SharedTaxonSelecteCategoryDropDownListText));
                    }  
                    foreach (var possibleTaxonCategory in possibleTaxonCategories)
                    {
                        model.TaxonCategoryList.Add(new TaxonDropDownModelHelper(possibleTaxonCategory.Id, possibleTaxonCategory.Name));
                    }             
                }
                else
                {
                    model.ErrorMessage = Resources.DyntaxaResource.TaxonSharedInvalidTaxon;
                }
            }
            else
            {
                model.ErrorMessage = Resources.DyntaxaResource.SharedInvalidUserContext;
            }
            return model;
        }
    }
}
