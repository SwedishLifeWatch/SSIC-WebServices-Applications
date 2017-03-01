using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.TaxonName;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Shared;
using DataType = System.ComponentModel.DataAnnotations.DataType;

// ReSharper disable CheckNamespace
namespace ArtDatabanken.WebApplication.Dyntaxa.Data 
// ReSharper restore CheckNamespace
{
    public class TaxonAddViewModel : BaseViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();

        /// <summary>
        /// Gets and sets the parent taxon Id
        /// </summary>
        public string ParentTaxonId { get; set; }

        /// <summary>
        /// Gets and sets taxon guid
        /// </summary>
        [AllowHtml]
        public string TaxonGuid { get; set; }

        /// <summary>
        /// Indicates if it is ok to create taxon with set values
        /// </summary>
        public bool IsOkToCreateTaxon { get; set; }

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
        [Required]
        [StringLength(250, ErrorMessageResourceType = typeof(Resources.DyntaxaResource), ErrorMessageResourceName = "SharedErrorStringToLong250")]
        [DataType(DataType.Text)]
        [LocalizedDisplayName("TaxonSharedScientificName", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string ScientificName { get; set; }

        /// <summary>
        /// Taxon Author
        /// </summary>
        //[Required]
        [LocalizedDisplayName("TaxonSharedAuthor", NameResourceType = typeof(Resources.DyntaxaResource))]
        [AllowHtml]
        public string Author { get; set; }

        /// <summary>
        /// Recommended common name of the taxon.
        /// </summary>
        //[Required]
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

            public string IsNotOkToCreateTaxonErrorText
            {
                get { return Resources.DyntaxaResource.TaxonAddIsNotOkToCreateTaxonErrorText; }
            }

            public string ConfirmButtonText
            {
                get { return Resources.DyntaxaResource.SharedOkButtonText; }
            }

            public string ResetButtonLabel
            {
                get { return Resources.DyntaxaResource.SharedResetButtonText; }
            }

            public string GetSelectedSave
            {
                get { return "GetSelectedSave"; }
            }

            public string AddTaxonPageTitle { get { return Resources.DyntaxaResource.TaxonAddPageTitle; } }
            public string AddTaxonPageHeader { get { return Resources.DyntaxaResource.TaxonAddPageHeader; } }
            public string TaxonAddBoxHeader { get { return Resources.DyntaxaResource.TaxonAddBoxHeader; } }

            public string DialogAddTaxonInfoText
            {
                get { return Resources.DyntaxaResource.TaxonAddCreateTaxonText; }
            }

            public string CreatingLabel
            {
                get { return Resources.DyntaxaResource.SharedCreating; }
            }

            public string SavingLabel
            {
                get { return Resources.DyntaxaResource.SharedSaving; }
            }
        }

        #endregion

    }
}
