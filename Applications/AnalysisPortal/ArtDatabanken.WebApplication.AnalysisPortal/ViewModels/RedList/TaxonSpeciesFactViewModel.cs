using System;
using System.Collections.Generic;
using System.Linq;
using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.Taxon;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// A view model representing a species fact taxon.
    /// </summary>
    [Serializable]
    public class TaxonSpeciesFactViewModel
    {
        /// <summary>
        /// Gets or sets the action plan.
        /// </summary>
        public string ActionPlan { get; set; }

        /// <summary>
        /// Gets or sets th author.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the biotopes which are important.
        /// </summary>
        public string BiotopesImportant { get; set; }

        /// <summary>
        /// Gets or sets the biotopes which are possible.
        /// </summary>
        public string BiotopesPossible { get; set; }

        /// <summary>
        /// Gets or sets the common name.
        /// </summary>
        public string CommonName { get; set; }

        /// <summary>
        /// Gets or sets the conventions.
        /// </summary>
        public string Conventions { get; set; }

        /// <summary>
        /// Gets or sets the global red list category.
        /// </summary>
        public string GlobalRedListCategory { get; set; }

        /// <summary>
        /// Gets or sets the hosts.
        /// </summary>
        public List<string> Hosts { get; set; }

        /// <summary>
        /// Indicates if we have a picture or not
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// Indicates if the taxon has a county map or not
        /// </summary>
        public bool HasCountyMap { get; set; }

        /// <summary>
        /// Indicates if the taxon has a observationamp or not
        /// </summary>
        public bool HasObservationMap { get; set; }

        /// <summary>
        /// Metadata information in image.
        /// </summary>
        public string ImageMetaData { get; set; }

        /// <summary>
        /// Gets or sets the impacy.
        /// </summary>
        public List<string> Impact { get; set; }

        /// <summary>
        /// Gets or sets if an exception to protected by law exists.
        /// </summary>
        public bool IsProtectedByLawException { get; set; }

        /// <summary>
        /// Gets or sets if it is protected by law nationwide.
        /// </summary>
        public bool IsProtectedByLawNationwide { get; set; }

        /// <summary>
        /// Gets or sets if the species is red listed.
        /// </summary>
        public bool IsRedListed { get; set; }

        /// <summary>
        /// Gets or sets if red list criteria are available.
        /// </summary>
        public bool IsRedListCriteriaAvailable { get; set; }

        /// <summary>
        /// Gets or sets if species information document is publishable.
        /// </summary>
        public bool IsSpeciesInformationDocumentPublishable { get; set; }

        /// <summary>
        /// Gets or sets if taxon county occurrence is available.
        /// </summary>
        public bool IsTaxonCountyOccurrencyAvailable { get; set; }

        /// <summary>
        /// Gets or sets the landscape types which are important.
        /// </summary>
        public string LandscapeTypeOccurrenceImportant { get; set; }

        /// <summary>
        /// Gets or sets the landscape types which are possible.
        /// </summary>
        public string LandscapeTypeOccurrencePossible { get; set; }

        /// <summary>
        /// Gets or sets the lifeforms.
        /// </summary>
        public string Lifeforms { get; set; }

        /// <summary>
        /// Gets or sets the map.
        /// </summary>
        public string Map { get; set; }

        /// <summary>
        /// Gets or sets the order name.
        /// </summary>
        //public string OrderName { get; set; }

        /// <summary>
        /// Gets or sets the organism group.
        /// </summary>
        public string OrganismGroup { get; set; }

        /// <summary>
        /// Gets or sets the organism group id.
        /// </summary>
        public int? OrganismGroupId { get; set; }

        /// <summary>
        /// Gets or sets the list of parent taxa (taxonomic hierarchy).
        /// </summary>
        public List<TaxonViewModel> ParentTaxa { get; set; }

        /// <summary>
        /// Gets or sets the red list period.
        /// </summary>
        public int Period { get; set; }

        /// <summary>
        /// Gets or sets the preservation status.
        /// </summary>
        public string PreservationStatus { get; set; }

        /// <summary>
        /// Gets or sets protected by law, factor name.
        /// </summary>
        public string ProtectedByLawFactorName { get; set; }

        /// <summary>
        /// Gets or sets protected by law paragraphs.
        /// </summary>
        public string ProtectedByLawParagraphs { get; set; }

        /// <summary>
        /// Gets or sets protected by law text/exception.
        /// </summary>
        public string ProtectedByLawTextException { get; set; }

        /// <summary>
        /// Gets or sets the red list category.
        /// </summary>
        public string RedListCategory { get; set; }

        /// <summary>
        /// Gets or sets the red list criteria.
        /// </summary>
        public string RedListCriteria { get; set; }

        /// <summary>
        /// Gets or sets the red list documentation quality.
        /// </summary>
        public int RedListDocumentationQuality { get; set; }

        /// <summary>
        /// Gets or sets the red list documentation text.
        /// </summary>
        public string RedListDocumentationText { get; set; }

        /// <summary>
        /// Gets or sets the scientific name.
        /// </summary>
        public string ScientificName { get; set; }

        /// <summary>
        /// Gets the scientific name, together with the author.
        /// </summary>
        public string ScientificNameAndAuthor
        {
            get
            {
                return string.IsNullOrEmpty(Author) ? ScientificName : ScientificName + " " + Author;
            }
        }

        /// <summary>
        /// Gets or sets the species information document author and year.
        /// </summary>
        public string SpeciesInformationDocumentAuthorAndYear { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document author and year.
        /// </summary>
        public string SpeciesInformationDocumentAuthorAndYearTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document description.
        /// </summary>
        public string SpeciesInformationDocumentDescription { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document description.
        /// </summary>
        public string SpeciesInformationDocumentDescriptionTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document distribution.
        /// </summary>
        public string SpeciesInformationDocumentDistribution { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document distribution.
        /// </summary>
        public string SpeciesInformationDocumentDistributionTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document ecology.
        /// </summary>
        public string SpeciesInformationDocumentEcology { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document ecology.
        /// </summary>
        public string SpeciesInformationDocumentEcologyTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document extra.
        /// </summary>
        public string SpeciesInformationDocumentExtra { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document extra.
        /// </summary>
        public string SpeciesInformationDocumentExtraTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document measures.
        /// </summary>
        public string SpeciesInformationDocumentMeasures { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document measures.
        /// </summary>
        public string SpeciesInformationDocumentMeasuresTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document preamble.
        /// </summary>
        public string SpeciesInformationDocumentPreamble { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document preamble.
        /// </summary>
        public string SpeciesInformationDocumentPreambleTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document references.
        /// </summary>
        public string SpeciesInformationDocumentReferences { get; set; }

        /// <summary>
        /// Gets or sets the title of the species information document references.
        /// </summary>
        public string SpeciesInformationDocumentReferencesTitle { get; set; }

        /// <summary>
        /// Gets or sets the species information document threats.
        /// </summary>
        public string SpeciesInformationDocumentThreats { get; set; }

        /// <summary>
        /// Gets or sets the titel of the species information document threats.
        /// </summary>
        public string SpeciesInformationDocumentThreatsTitle { get; set; }

        /// <summary>
        /// Gets or sets the substrates which are important.
        /// </summary>
        public string SubstrateImportant { get; set; }

        /// <summary>
        /// Gets or sets the substrates which are possible.
        /// </summary>
        public string SubstratePossible { get; set; }

        ///// <summary>
        ///// Gets or sets the substrates information.
        ///// </summary>
        //public Dictionary<string, IList<SubstrateInformation>> SubstrateInformationList { get; set; }

        /// <summary>
        /// Information about swedish occurrence for taxon.
        /// </summary>
        public string SwedishOccurrence { get; set; }

        /// <summary>
        /// Id for swedish occurrence applied to taxon.
        /// </summary>
        public int SwedishOccurrenceId { get; set; }

        /// <summary>
        /// Synonyms for this taxon
        /// </summary>
        public List<TaxonNameViewModel> Synonyms { get; set; }

        /// <summary>
        /// Information about older values for red list criteria for taxon.
        /// </summary>
        public Dictionary<string, string> PreviousRedListCriteria { get; set; }

        /// <summary>
        /// Gets or sets the taxon id.
        /// </summary>
        public int TaxonId { get; set; }

        /// <summary>
        /// Indicates whether or not the taxon has any speciesfacts.
        /// </summary>
        public bool HasSpeciesFacts { get; set; }

        /// <summary>
        /// Indicates whether or not the taxon is valid.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the category.
        /// </summary>        
        public string Category { get; set; }

        /// <summary>
        /// Category name for high level categories. Returns empty string for Art and lower
        /// </summary>
        public string CategoryNameHighLevel { get; set; }

        /// <summary>
        /// Number of taxa in each category
        /// </summary>
        public Dictionary<RedListCategory, List<int>> RedListCategoryTaxa { get; set; }

        /// <summary>
        /// Flag that indicates if this taxa is higer than species
        /// </summary>
        public bool IsHigherTaxa { get; set; }

        /// <summary>
        /// Create a new TaxonSpeciesFactViewModel, from a taxon.
        /// </summary>
        /// <param name="user">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        public TaxonSpeciesFactViewModel(IUserContext user, ITaxon taxon)
        {
            Author = taxon.Author ?? string.Empty;
            CommonName = taxon.GetCommonName(user).IsNotNull() ? taxon.GetCommonName(user).Name : string.Empty;
            ParentTaxa = GetParentTaxa(user, taxon);
            ScientificName = taxon.GetScientificName(user).IsNotNull() ? taxon.GetScientificName(user).Name : string.Empty;
            TaxonId = taxon.Id;
            Category = taxon.Category.Name;
            //Synonyms = taxon.GetSynonymsViewModel(user);
            RedListCategoryTaxa = new Dictionary<RedListCategory, List<int>>();
        }

        private string GetLinkPrefix()
        {
            string linkPrefix = "";
            switch (Environment.MachineName)
            {
                case "MONESES-DEV":
                    {
                        linkPrefix = "http://test-artfakta.artdatabanken.se/";
                        break;
                    }
                case "MONESES2-1":
                    {
                        linkPrefix = "http://testprod-artfakta.artdatabanken.se/";
                        break;
                    }
                case "LAMPETRA2-1":
                    {
                        linkPrefix = "http://artfakta.artdatabanken.se/";
                        break;
                    }
                case "LAMPETRA2-2":
                    {
                        linkPrefix = "http://artfakta.artdatabanken.se/";
                        break;
                    }
                default:
                    {
                        linkPrefix = "http://localhost:60269/";
                        break;
                    }
            }

            linkPrefix += "taxon/";

            return linkPrefix;
        }

        /// <summary>
        /// Gets the taxonomic hierarchy, for the specified taxon.
        /// </summary>
        /// <param name="user">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>An ordered list of parent taxa, forming the taxonomic hierarchy.</returns>
        private List<TaxonViewModel> GetParentTaxa(IUserContext user, ITaxon taxon)
        {
            var taxaRelations = taxon.GetAllParentTaxonRelations(user, null, false, false, true);
            var distinctParentTaxa = taxaRelations.GroupBy(x => x.ParentTaxon.Id).Select(x => x.First().ParentTaxon).Where(x => x.Id > 0).ToList();

            return distinctParentTaxa.Select(parentTaxon => new TaxonViewModel
            {
                Author = parentTaxon.Author,
                Category = parentTaxon.Category.Name,
                CommonName = parentTaxon.CommonName.IsNotEmpty() ? parentTaxon.CommonName : string.Empty,
                ScientificName = parentTaxon.ScientificName.IsNotEmpty() ? parentTaxon.ScientificName : string.Empty,
                //SortOrder = parentTaxon.SortOrder,
                TaxonId = parentTaxon.Id,
                //CategoryId = parentTaxon.Category.Id
            }).ToList();
        }
    }
}
