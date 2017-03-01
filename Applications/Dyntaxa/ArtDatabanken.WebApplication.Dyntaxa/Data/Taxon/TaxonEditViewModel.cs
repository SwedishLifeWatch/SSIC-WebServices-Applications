using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using DataType = System.ComponentModel.DataAnnotations.DataType;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data
// ReSharper restore CheckNamespace
{
    public class TaxonEditViewModel : TaxonSwedishOccuranceBaseViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Gets and sets the taxon Id
        /// </summary>
        public string TaxonId { get; set; }

        /// <summary>
        /// Gets and sets taxon guid
        /// </summary>
        public string TaxonGuid { get; set; }

        /// <summary>
        /// Taxon category the taxon, e.g. Species, Genus or Family. 
        /// </summary>
        [Required]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedTaxonPickCategoryText")]       
        [LocalizedDisplayName("TaxonSharedTaxonCategory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int TaxonCategoryId { get; set; }

        /// <summary>
        /// List of all categories
        /// </summary>
        public IList<TaxonDropDownModelHelper> TaxonCategoryList { get; set; }        

        /// <summary>
        /// Recommended Scientific Name of the taxon.
        /// </summary>
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("TaxonSharedScientificName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string ScientificName { get; set; }

        /// <summary>
        /// Taxon Author
        /// </summary>
        [LocalizedDisplayName("TaxonSharedAuthor", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string Author { get; set; }

        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("TaxonSharedCommonName", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string CommonName { get; set; }

        /// <summary>
        /// An automatically generated description of the status of this taxon concept.
        /// This description should include change history information such as deleted, split and lump history, changes in content and taxon category. 
        /// It could also have information on confusing relationships with other taxa, due to name useage.
        /// </summary>
        //[Required]
        [LocalizedDisplayName("TaxonSharedDescription", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string Description { get; set; }

        [LocalizedDisplayName("TaxonAddIsProblematic", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool TaxonIsProblematic { get; set; }

        /// <summary>
        /// Indicates if it is possible to change taxon Alert status
        /// </summary>
        public bool EnableTaxonIsProblematic { get; set; }

        /// <summary>
        /// Indication of number of taxon references must be at least one.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedPickReferenceText")]
        [LocalizedDisplayName("SharedReferences", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int NoOfTaxonReferences { get; set; }

        [LocalizedDisplayName("SharedIsMicrospeciesLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public bool IsMicrospecies { get; set; }

        /// <summary>
        /// Indicates if it is possible to Species fact ie if Taxon has sortorder higher and  included for "släkte".
        /// </summary>
        public bool EnableSpeciesFact { get; set; }

        /// <summary>
        /// Indicates Species fact not possible to read.
        /// </summary>
        public bool SpeciesFactError { get; set; }

        /// <summary>
        /// Get and sets the internal error message.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sets the id of failing taxon
        /// </summary>
        public string TaxonErrorId { get; set; }

        #region Labels

        /// <summary>
        /// All localized labels
        /// </summary>
        public ModelLabels Labels
        {
            get { return _labels; }
        }

        public bool IsTaxonJustCreated { get; set; }

        /// <summary>
        /// Localized labels used in edit revision view
        /// </summary>
        public class ModelLabels
        {
            public string SaveButtonText
            {
                get { return Resources.DyntaxaResource.SharedSaveButtonText; }
            }

            public string CancelButtonText
            {
                get { return Resources.DyntaxaResource.SharedCancelButtonText; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            } 

            public string EditTaxonLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditHeaderLabel; }
            }

            public string SwedishHistoryLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditSwedishImmigrationHistoryLabel; }
            }
            public string SwedishOccurrenceLabel
            {
                get { return Resources.DyntaxaResource.TaxonEditSwedishOccurrenceLabel; }
            }

            public string TaxonEditQualityHeaderText
            {
                get { return Resources.DyntaxaResource.TaxonEditQualityHeaderText; }
            }

            public string SpeciesFactErrorText
            {
                get { return Resources.DyntaxaResource.SharedSpeciesFactError; }
            }

            public string ResetButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedResetButtonText; }
            }

            public string NoReferencesAvaliableText
            {
                get { return Resources.DyntaxaResource.SharedNoValidReferenceErrorText; }
           }
        }

        #endregion

    }
}