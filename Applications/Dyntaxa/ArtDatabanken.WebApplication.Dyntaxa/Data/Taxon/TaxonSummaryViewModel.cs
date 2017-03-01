using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.DyntaxaInternalService;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers.Extensions;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// View model for Taxon summary.    
    /// </summary>
    public class TaxonSummaryViewModel
    {
        /// <summary>
        /// Id of the taxon.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// LSID of the taxon.
        /// "urn:lsid:dyntaxa.artdata.slu.se:taxon:[TaxonId]".
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Taxon category Id 
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Taxon category of the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The sort order for the taxon category
        /// Decides if the scientific name should be rendered italic or not.
        /// </summary>
        public int CategorySortOrder { get; set; }

        /// <summary>
        /// Taxon concept description.
        /// </summary>
        public string ConceptDefinition { get; set; }

        /// <summary>
        /// Gets or sets the alert status.
        /// </summary>
        public TaxonAlertStatusId AlertStatus { get; set; }

        /// <summary>
        /// Url to the alert image.
        /// </summary>
        public string AlertImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the update information.
        /// </summary>
        public string UpdateInformation { get; set; }

        /// <summary>
        /// Gets or sets the valid to information.
        /// </summary>
        public string ValidToInformation { get; set; }

        /// <summary>
        /// Gets or sets the created information.
        /// </summary>
        public string CreatedInformation { get; set; }

        /// <summary>
        /// Gets or sets the validity.
        /// </summary>
        public string Validity { get; set; }

        /// <summary>
        /// Taxon scientific name.
        /// </summary>
        public TaxonNameAuthorViewModel ScientificName { get; set; }

        /// <summary>
        /// Taxon common name.
        /// </summary>
        public TaxonNameAuthorViewModel CommonName { get; set; }

        /// <summary>
        /// Taxon synonyms.
        /// </summary>
        public List<TaxonNameViewModel> Synonyms { get; set; }

        /// <summary>
        /// proParte synonyms.
        /// </summary>
        public List<TaxonNameViewModel> ProParteSynonyms { get; set; }

        /// <summary>
        /// Misapplied names.
        /// </summary>
        public List<TaxonNameViewModel> MisappliedNames { get; set; }

        /// <summary>
        /// All taxon other valid names.
        /// </summary>
        public List<TaxonNameViewModel> OtherValidCommonNames { get; set; }

        /// <summary>
        /// List of parent categories and their names.
        /// </summary>
        public List<RelatedTaxonViewModel> Classification { get; set; }

        /// <summary>
        /// Gets or sets the swedish occurrence.
        /// </summary>
        public string SwedishOccurrence { get; set; }

        /// <summary>
        /// Gets or sets the swedish history.
        /// </summary>
        public string SwedishHistory { get; set; }

        /// <summary>
        /// True if it's micro specie
        /// </summary>
        public bool IsMicrospecies { get; set; }

        //private static IPerson GetCreatedByPerson(IUserContext userContext, int createdBy)
        //{
        //    IPerson person;
        //    IUser user;

        //    try
        //    {
        //        user = CoreData.UserManager.GetUser(userContext, createdBy);
        //    }
        //    catch (Exception)
        //    {
        //        // user does not exist in UserAdmin system
        //        user = null;
        //    }

        //    if ((user.IsNotNull()) &&
        //        (user.Type == UserType.Person) &&
        //        (user.PersonId.HasValue))
        //    {
        //        person = CoreData.UserManager.GetPerson(userContext, user.PersonId.Value);
        //    }
        //    else
        //    {
        //        person = null;
        //    }
        //    return person;
        //}

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <returns>
        /// The <see cref="TaxonSummaryViewModel"/>.
        /// </returns>
        public static TaxonSummaryViewModel Create(IUserContext userContext, ITaxon taxon, int? revisionId)
        {
            var model = new TaxonSummaryViewModel();
            IPerson person;
            bool isInRevision = DyntaxaHelper.IsInRevision(userContext, revisionId);
            bool isUserTaxonEditor = userContext.IsTaxonEditor();

            model.Id = taxon.Id.ToString();
            model.Guid = taxon.Guid ?? string.Empty;
            model.CategoryId = taxon.Category != null ? taxon.Category.Id : 0;
            model.Category = taxon.Category != null ? taxon.Category.Name : string.Empty;
            model.IsMicrospecies = taxon.IsMicrospecies;
            model.CategorySortOrder = taxon.Category != null ? taxon.Category.SortOrder : 0;
            model.ConceptDefinition = taxon.GetConceptDefinition(userContext) ?? "-";
            model.AlertStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id;
            model.AlertImageUrl = GetAlertImageUrl(model.AlertStatus);
            person = taxon.GetModifiedByPerson(userContext);
            //IPerson createdByPerson = GetCreatedByPerson(userContext, taxon.CreatedBy);
            //string createdFullName = createdByPerson.FullName;
            if (person.IsNull())
            {
                model.UpdateInformation = string.Format("{0} ({1})", taxon.ModifiedDate.ToShortDateString(), String.Empty);
            }
            else
            {
                model.UpdateInformation = string.Format("{0} ({1})", taxon.ModifiedDate.ToShortDateString(), person.FullName);
            }

            model.ValidToInformation = string.Format("{0} ({1})", taxon.ValidToDate.ToShortDateString(), taxon.ModifiedByPerson);
            model.CreatedInformation = string.Format("{0} ({1})", taxon.CreatedDate.ToShortDateString(), taxon.ModifiedByPerson);
            model.Validity = GetValidityDescription(taxon);

            if (taxon.ScientificName.IsNotEmpty())
            {                        
                model.ScientificName = new TaxonNameAuthorViewModel(taxon.ScientificName, taxon.Author);
            }

            if (taxon.CommonName.IsNotEmpty())
            {
                model.CommonName = new TaxonNameAuthorViewModel(taxon.CommonName, string.Empty);
            }

            // Synonyms
            //model.Synonyms = new List<TaxonNameViewModel>();
            //var synonyms = taxon.GetSynonyms(userContext, true);
            //if (synonyms != null)
            //{
            //    foreach (ITaxonName taxonName in synonyms)
            //    {                            
            //        model.Synonyms.Add(new TaxonNameViewModel(taxonName, taxon));
            //    }
            //}

            model.Synonyms = taxon.GetSynonymsViewModel(isInRevision, isUserTaxonEditor, false);
            model.ProParteSynonyms = taxon.GetProParteSynonymsViewModel(isInRevision, isUserTaxonEditor);
            model.MisappliedNames = taxon.GetMisappliedNamesViewModel(isInRevision, isUserTaxonEditor);

            // Other valid common names
            // todo - change implementation?
            //model.OtherValidCommonNames = new List<string>();
            //if (!taxon.CommonName.IsEmpty())
            //{                
            //    model.OtherValidCommonNames.AddRange(
            //        from taxonName in taxon.GetTaxonNames(userContext)
            //        where
            //            taxonName.Category.Id == (int)TaxonNameCategoryId.SwedishName &&
            //            taxonName.Version != taxon.GetCommonName(userContext).Version
            //        select taxonName.Name);

            //    // todo - även ha med att namnet är gilitigt. Hur ser man det???         
            //}

            model.OtherValidCommonNames = taxon.GetNotRecommendedSwedishNamesViewModel(isInRevision, isUserTaxonEditor);

            // Remove other valid common names from synonyms
            if (model.OtherValidCommonNames.IsNotEmpty())
            {
                List<TaxonNameViewModel> newSynonymList = new List<TaxonNameViewModel>();

                foreach (TaxonNameViewModel synonym in model.Synonyms)
                {
                    if (model.OtherValidCommonNames.All(x => x.Id != synonym.Id))
                    {
                        newSynonymList.Add(synonym);
                    }
                }

                model.Synonyms = newSynonymList;
            }

            // Classification
            var allParentTaxa = taxon.GetAllParentTaxonRelations(userContext, null, isInRevision, false, true);
            var distinctParentTaxa = allParentTaxa.GroupBy(x => x.ParentTaxon.Id).Select(x => x.First().ParentTaxon).ToList();
            model.Classification = new List<RelatedTaxonViewModel>();
            foreach (ITaxon relatedTaxon in distinctParentTaxa)
            {
                if (relatedTaxon.Category.IsTaxonomic)
                {
                    model.Classification.Add(new RelatedTaxonViewModel(relatedTaxon, relatedTaxon.Category, null));
                }
            }

            // Species fact
            try
            {                
                // Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(taxon, new [] {FactorId.SwedishOccurence, FactorId.SwedishHistory});
                Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetCommonDyntaxaSpeciesFacts(userContext, taxon);
                model.SwedishHistory = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, FactorId.SwedishHistory);
                model.SwedishOccurrence = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, FactorId.SwedishOccurrence);

                // If swedish occurrence or swedish history is changed in the current revision, then show those values instead.
                if (DyntaxaHelper.IsInRevision(userContext, revisionId))
                {
                    DyntaxaInternalTaxonServiceManager internalTaxonServiceManager =
                        new DyntaxaInternalTaxonServiceManager();

                    // Check if Swedish occurrence is stored in Taxon database in this revision.
                    DyntaxaRevisionSpeciesFact swedishOccurrenceRevisionSpeciesFact =
                        internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                            userContext,
                            (Int32)FactorId.SwedishOccurrence, 
                            taxon.Id, 
                            revisionId.Value);
                    if (swedishOccurrenceRevisionSpeciesFact != null)
                    {
                        SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, userContext);
                        TaxonModelManager.UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(userContext, speciesFactModel.SwedishOccurrenceSpeciesFact, swedishOccurrenceRevisionSpeciesFact);
                        model.SwedishOccurrence = speciesFactModel.SwedishOccurrenceSpeciesFact.GetStatusOriginalLabel();                        
                    }

                    // Check if Swedish history is stored in Taxon database in this revision.
                    DyntaxaRevisionSpeciesFact swedishHistoryRevisionSpeciesFact =
                        internalTaxonServiceManager.GetDyntaxaRevisionSpeciesFact(
                            userContext,
                            (Int32)FactorId.SwedishHistory, 
                            taxon.Id, 
                            revisionId.Value);
                    if (swedishHistoryRevisionSpeciesFact != null)
                    {
                        if (swedishHistoryRevisionSpeciesFact.StatusId.HasValue)
                        {
                            SpeciesFactModelManager speciesFactModel = new SpeciesFactModelManager(taxon, userContext);
                            TaxonModelManager.UpdateOldSpeciesFactModelWithDyntaxaRevisionSpeciesFactValues(userContext, speciesFactModel.SwedishHistorySpeciesFact, swedishHistoryRevisionSpeciesFact);
                            model.SwedishHistory = speciesFactModel.SwedishHistorySpeciesFact.GetStatusOriginalLabel();
                        }
                        else // swedish history is deleted in this revision
                        {
                            model.SwedishHistory = "";
                        }                        
                    }
                }
            }
            catch (Exception)
            {
                // the taxon did not exist in Artfakta
            }
            
            return model;
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="revisionId">
        /// The revision id.
        /// </param>
        /// <returns>
        /// The <see cref="TaxonSummaryViewModel"/>.
        /// </returns>
        public static TaxonSummaryViewModel Create(IUserContext userContext, int taxonId, int? revisionId)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(userContext, taxonId);
            return Create(userContext, taxon, revisionId);
        }

        /// <summary>
        /// The get alert image url.
        /// </summary>
        /// <param name="alertStatus">
        /// The alert status.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// </exception>
        private static string GetAlertImageUrl(TaxonAlertStatusId alertStatus)
        {
            switch (alertStatus)
            {
                case TaxonAlertStatusId.Green:
                    return "~/Images/Icons/info_right_green.png";
                case TaxonAlertStatusId.Yellow:
                    return "~/Images/Icons/info_right_yellow.png";
                case TaxonAlertStatusId.Red:
                    return "~/Images/Icons/info_right_red.png";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// The get validity description.
        /// </summary>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private static string GetValidityDescription(ITaxon taxon)
        {
            string text;
            if (taxon.IsValid)
            {
                if (taxon.Category.IsTaxonomic)
                {
                    text = DyntaxaResource.TaxonSummaryValitityValueAccepted;
                }
                else
                {
                    text = DyntaxaResource.TaxonSummaryValitityValuePragmatic;
                }
            }
            else
            {
                text = DyntaxaResource.TaxonSummaryValitityValueNotValid;
                var validToInformation = string.Format("{0} ({1})", taxon.ValidToDate.ToShortDateString(), taxon.ModifiedByPerson);
                text = text + ". " + DyntaxaResource.TaxonSummaryValidToLabel + " " + validToInformation;
            }

            return text;                    
        }

        /// <summary>
        /// Gets the labels.
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// The _labels.
        /// </summary>
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels.
        /// </summary>
        public class ModelLabels
        {
            /// <summary>
            /// Gets the swedish history label.
            /// </summary>
            public string SwedishHistoryLabel
            {
                get { return DyntaxaResource.TaxonSummarySwedishHistoryLabel; }
            }

            /// <summary>
            /// Gets the swedish occurrence label.
            /// </summary>
            public string SwedishOccurrenceLabel
            {
                get { return DyntaxaResource.TaxonSummarySwedishOccurrenceLabel; }
            }

            /// <summary>
            /// Gets the classification label.
            /// </summary>
            public string ClassificationLabel
            {
                get { return DyntaxaResource.TaxonSummaryClassificationLabel; }
            }

            /// <summary>
            /// Gets the common names label.
            /// </summary>
            public string CommonNamesLabel
            {
                get { return DyntaxaResource.TaxonSummaryCommonNamesLabel; }
            }

            /// <summary>
            /// Gets the synonyms label.
            /// </summary>
            public string SynonymsLabel
            {
                get { return DyntaxaResource.TaxonSummarySynonymsLabel; }
            }

            /// <summary>
            /// Gets the scientific name label.
            /// </summary>
            public string ScientificNameLabel
            {
                get { return DyntaxaResource.TaxonSummaryScientificNameLabel; }
            }

            /// <summary>
            /// Gets the category label.
            /// </summary>
            public string CategoryLabel
            {
                get { return DyntaxaResource.TaxonSummaryCategoryLabel; }
            }

            /// <summary>
            /// Gets the concept definition label.
            /// </summary>
            public string ConceptDefinitionLabel
            {
                get { return DyntaxaResource.TaxonSummaryConceptDefinitionLabel; }
            }            

            /// <summary>
            /// Gets the taxon status label.
            /// </summary>            
            public string TaxonStatusLabel
            {
                get { return DyntaxaResource.TaxonSummaryTaxonStatusLabel; }
            }

            /// <summary>
            /// Gets the guid label.
            /// </summary>
            public string GuidLabel
            {
                get { return DyntaxaResource.TaxonSummaryGuidLabel; }
            }

            /// <summary>
            /// Gets the id label.
            /// </summary>
            public string IdLabel
            {
                get { return DyntaxaResource.TaxonSummaryIdLabel; }
            }

            /// <summary>
            /// Gets the update information label.
            /// </summary>
            public string UpdateInformationLabel
            {
                get { return DyntaxaResource.TaxonSummaryUpdateInformationLabel; }
            }

            /// <summary>
            /// Gets the create information label.
            /// </summary>
            public string CreateInformationLabel
            {
                get { return DyntaxaResource.TaxonSummaryCreatedLabel; }
            }

            /// <summary>
            /// Gets the valid to information label.
            /// </summary>
            public string ValidToInformationLabel
            {
                get { return DyntaxaResource.TaxonSummaryValidToLabel; }
            }

            /// <summary>
            /// Label for microspecies
            /// </summary>
            public string IsMicrospeciesLabel
            {
                get { return DyntaxaResource.SharedIsMicrospeciesLabel; }
            }

            public string True
            {
                get { return DyntaxaResource.SharedBoolTrueText; }
            }

            public string False
            {
                get { return DyntaxaResource.SharedBoolFalseText; }
            }
        }
    }
}