using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils;
using ArtDatabanken.WebApplication.AnalysisPortal.Utils.Extensions;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon
{
    public class TaxonSummaryViewModel
    {
        /// <summary>
        /// Id of the taxon.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// LSID of the taxon.
        /// "urn:lsid:dyntaxa.artdata.slu.se:taxon:[TaxonId]"
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// Taxon category of the taxon, e.g. Species, Genus or Family.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// The sort order for the taxon category
        /// Decides if the scientific name should be rendered italic or not
        /// </summary>
        public int CategorySortOrder { get; set; }

        /// <summary>
        /// Taxon concept description
        /// </summary>
        public string ConceptDefinition { get; set; }

        public TaxonAlertStatusId AlertStatus { get; set; }

        /// <summary>
        /// Url to the alert image
        /// </summary>
        public string AlertImageUrl { get; set; }

        public string UpdateInformation { get; set; }

        public string ValidToInformation { get; set; }

        public string CreatedInformation { get; set; }

        public string Validity { get; set; }

        /// <summary>
        /// Taxon scientific name
        /// </summary>
        public TaxonNameAuthorViewModel ScientificName { get; set; }

        /// <summary>
        /// Taxon common name
        /// </summary>
        public TaxonNameAuthorViewModel CommonName { get; set; }

        /// <summary>
        /// All taxon synonyms
        /// </summary>
        public List<TaxonNameViewModel> Synonyms { get; set; }

        /// <summary>
        /// All taxon other names
        /// </summary>
        public List<TaxonNameViewModel> OtherNames { get; set; }

        /// <summary>
        /// All taxon other valid names
        /// </summary>
        public List<string> OtherValidCommonNames { get; set; }

        /// <summary>
        /// List of parent categories and their names
        /// </summary>
        public List<RelatedTaxonViewModel> Classification { get; set; }

        public string SwedishOccurrence { get; set; }

        public string SwedishHistory { get; set; }

        public static TaxonSummaryViewModel Create(ITaxon taxon, IUserContext user)
        {
            var model = new TaxonSummaryViewModel();
            IPerson person;
            // string person;
            model.Id = taxon.Id.ToString();
            model.Guid = taxon.Guid ?? "";
            model.Category = taxon.Category != null ? taxon.Category.Name : "";
            model.CategorySortOrder = taxon.Category != null ? taxon.Category.SortOrder : 0;
            model.ConceptDefinition = taxon.GetConceptDefinition(user) ?? "-";
            model.AlertStatus = (TaxonAlertStatusId)taxon.AlertStatus.Id;
            model.AlertImageUrl = GetAlertImageUrl(model.AlertStatus);
            person = taxon.GetModifiedByPerson(user);
            // taxon.GetModifiedByPersonFullname(user); Replace with this when Dyntaxa is updated
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
                model.CommonName = new TaxonNameAuthorViewModel(taxon.CommonName, "");
            }

            // Synonyms
            model.Synonyms = new List<TaxonNameViewModel>();
            var synonyms = taxon.GetSynonyms(user);
            if (synonyms != null)
            {
                foreach (ITaxonName taxonName in synonyms)
                {
                    model.Synonyms.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            // Other names
            model.OtherNames = new List<TaxonNameViewModel>();
            var otherNames = taxon.GetOtherNames(user);
            if (otherNames != null)
            {
                foreach (ITaxonName taxonName in otherNames)
                {
                    model.OtherNames.Add(new TaxonNameViewModel(taxonName, taxon));
                }
            }

            // Other valid common names
            // todo - change implementation?
            model.OtherValidCommonNames = new List<string>();
            if (!taxon.CommonName.IsEmpty())
            {
                int commonNameCategoryId = taxon.GetCommonName(user).Category.Id;
                model.OtherValidCommonNames.AddRange(
                    from taxonName in taxon.GetTaxonNames(user)
                    where
                        taxonName.Category.Id == commonNameCategoryId &&
                        taxonName.Version != taxon.GetCommonName(user).Version
                    select taxonName.Name);
                // todo - även ha med att namnet är gilitigt. Hur ser man det???         
            }

            // Classification
            var allParentTaxa = taxon.GetAllParentTaxonRelations(user, null, false, false, true);
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
                //Dictionary<Data.ArtDatabankenService.FactorId, Data.ArtDatabankenService.SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(taxon, new[] { Data.ArtDatabankenService.FactorId.SwedishOccurrence, Data.ArtDatabankenService.FactorId.SwedishHistory });
                //model.SwedishHistory = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, Data.ArtDatabankenService.FactorId.SwedishHistory);
                //model.SwedishOccurrence = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, Data.ArtDatabankenService.FactorId.SwedishOccurrence);
                // Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetSpeciesFacts(taxon, new [] {FactorId.SwedishOccurence, FactorId.SwedishHistory});
                Dictionary<FactorId, SpeciesFact> dicSpeciesFacts = SpeciesFactHelper.GetCommonDyntaxaSpeciesFacts(user, taxon);
                model.SwedishHistory = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, FactorId.SwedishHistory);
                model.SwedishOccurrence = SpeciesFactHelper.GetFactorValue(dicSpeciesFacts, FactorId.SwedishOccurrence);
            }
            catch (Exception)
            {
                // the taxon did not exist in Artfakta
            }

            return model;
        }

        public static TaxonSummaryViewModel Create(int taxonId, IUserContext user)
        {
            ITaxon taxon = CoreData.TaxonManager.GetTaxon(user, taxonId);
            return Create(taxon, user);
        }

        private static string GetAlertImageUrl(TaxonAlertStatusId alertStatus)
        {
            switch (alertStatus)
            {
                case TaxonAlertStatusId.Green:
                    return "~/Content/Images/taxon-alert-status-green.png";
                case TaxonAlertStatusId.Yellow:
                    return "~/Content/Images/taxon-alert-status-yellow.png";
                case TaxonAlertStatusId.Red:
                    return "~/Content/Images/taxon-alert-status-red.png";

                //case TaxonAlertStatusId.Green:
                //    return "~/Content/Images/taxon-alert-status-green-right.png";
                //case TaxonAlertStatusId.Yellow:
                //    return "~/Content/Images/taxon-alert-status-yellow-right.png";
                //case TaxonAlertStatusId.Red:
                //    return "~/Content/Images/taxon-alert-status-red-right.png";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static string GetValidityDescription(ITaxon taxon)
        {
            string text;
            if (taxon.IsValid)
            {
                if (taxon.Category.IsTaxonomic)
                {
                    text = Resources.Resource.TaxonSummaryValitityValueAccepted;
                }
                else
                {
                    text = Resources.Resource.TaxonSummaryValitityValuePragmatic;
                }
            }
            else
            {
                text = Resources.Resource.TaxonSummaryValitityValueNotValid;
                var validToInformation = string.Format("{0} ({1})", taxon.ValidToDate.ToShortDateString(), taxon.ModifiedByPerson);
                text = text + ". " + Resources.Resource.TaxonSummaryValidToLabel + " " + validToInformation;
            }
            return text;
        }

        public ModelLabels Labels
        {
            get { return _labels; }
        }
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string SwedishHistoryLabel
            {
                get { return Resources.Resource.TaxonSummarySwedishHistoryLabel; }
            }

            public string SwedishOccurrenceLabel
            {
                get { return Resources.Resource.TaxonSummarySwedishOccurrenceLabel; }
            }

            public string ClassificationLabel
            {
                get { return Resources.Resource.TaxonSummaryClassificationLabel; }
            }

            public string CommonNamesLabel
            {
                get { return Resources.Resource.TaxonSummaryCommonNamesLabel; }
            }

            public string SynonymsLabel
            {
                get { return Resources.Resource.TaxonSummarySynonymsLabel; }
            }

            public string ScientificNameLabel
            {
                get { return Resources.Resource.TaxonSummaryScientificNameLabel; }
            }

            public string CategoryLabel
            {
                get { return Resources.Resource.TaxonSummaryCategoryLabel; }
            }

            public string ConceptDefinitionLabel
            {
                get { return Resources.Resource.TaxonSummaryConceptDefinitionLabel; }
            }

            public string ValidityLabel
            {
                get { return Resources.Resource.TaxonSummaryValidityLabel; }
            }

            public string GuidLabel
            {
                get { return Resources.Resource.TaxonSummaryGuidLabel; }
            }

            public string IdLabel
            {
                get { return Resources.Resource.TaxonSummaryIdLabel; }
            }

            public string UpdateInformationLabel
            {
                get { return Resources.Resource.TaxonSummaryUpdateInformationLabel; }
            }

            public string CreateInformationLabel
            {
                get { return Resources.Resource.TaxonSummaryCreatedLabel; }
            }

            public string ValidToInformationLabel
            {
                get { return Resources.Resource.TaxonSummaryValidToLabel; }
            }
        }
    }
}
