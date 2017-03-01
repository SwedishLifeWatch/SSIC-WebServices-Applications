using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactHostViewModel 
    {
        private readonly ModelLabels _labels = new ModelLabels();
          private int dropDownTaxonId = -1000;
          private int dropDownHostFactorId = -1000;

          [LocalizedDisplayName("SpeciesFactEditHostFactorsForSubstrateTaxonAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
          public int TaxonId
          { get; set; }

          [LocalizedDisplayName("SpeciesFactEditHostFactorsForSubstrateFactorAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]  
          public int FactorId
          { get; set; }
          public string PostAction { get; set; }
          public string ReferenceId { get; set; }

          [Required]
          [LocalizedDisplayName("SpeciesFactEditTaxonAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
          public int DropDownTaxonId
          {
              get { return dropDownTaxonId; }
              set { dropDownTaxonId = value; }
          }

          [Required]
          [LocalizedDisplayName("SpeciesFactEditFactorsAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
          public int DropDownHostFactorId
          {
              get { return dropDownHostFactorId; }
              set { dropDownHostFactorId = value; }
          }

          public SpeciesFactViewModel SpeciesFactViewModel { get; set; }

          /// <summary>
          /// List of all factors
          /// </summary>
          public IList<SpeciesFactHostViewModelItem> HostFactorList { get; set; }

        /// <summary>
        /// List of all taxa
        /// </summary>
          public IList<SpeciesFactHostViewModelItem> HostTaxonList { get; set; }

          /// <summary>
          /// List of all selectable taxa possible to add  as hosts
          /// </summary>
          public IList<SpeciesFactDropDownModelHelper> AddTaxonToHostList { get; set; }

          /// <summary>
          /// List of all selectable taxa possible to add  as hosts
          /// </summary>
          public IList<SpeciesFactDropDownModelHelper> AddFactorToHostList { get; set; }

        /// <summary>
          /// List of all selectable individualcategories
          /// </summary>
          public IList<SpeciesFactDropDownModelHelper> IndividualCategoryList { get; set; }

        /// <summary>
        /// Set the selected individual category for selected data
        /// </summary>
        public int IndividualCategoryId { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels 
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.SpeciesFactEditHostFactorsForSubstrateHeaderText; } }
            public string FastTitleLabel { get { return "Snabbredigera värdtaxa"; } }
            public string ExistingDataLabel { get { return Resources.DyntaxaResource.SpeciesFactEditHostFactorsForSubstrateExistingDataLabelText; } }
            public string ContinueLabel { get { return Resources.DyntaxaResource.SharedOkButtonText; } }
            public string SaveLabel { get { return Resources.DyntaxaResource.SharedSaveButtonText; } }
            public string ResetLabel { get { return "Ta bort gjorda val"; } }//Resources.DyntaxaResource.SharedResetButtonText; } }
            public string ResetLabelFast { get { return "Återställ till senast sparade"; } }
            public string CancelLabel { get { return Resources.DyntaxaResource.SharedCancelButtonText; } }
            public string LoadingLabel { get { return Resources.DyntaxaResource.SharedLoading; } }
            public string AddLabel { get { return Resources.DyntaxaResource.SharedAddButtonText; } }
            public string AddButtonLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorsAddButton; } }
            public string YesLabel { get { return Resources.DyntaxaResource.SharedYesButtonText; } }
            public string NoLabel { get { return Resources.DyntaxaResource.SharedNoButtonText; } }
            public string SearchTaxon { get { return Resources.DyntaxaResource.SpeciesFactEditHostFactorsForSubstrateSearchTaxonButtonText; } }
            // SpeciesFactEditHostFactorsForSubstrate
            public string SpeciesFactRemoveItemText { get { return Resources.DyntaxaResource.SharedDeleteButtonText; } }
            public string SubstrateChangeMany { get { return Resources.DyntaxaResource.SpeciesFactEditHostForSubstrateChangeManyButtonText; } }
            public string SpeciesFactInvalidSelectionDialogText { get { return Resources.DyntaxaResource.SpeciesFactInvalidSelectionDialogText; } }
            public string SpeciesFactInvalidSelectionDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactInvalidSelectionDialogHeader; } }
            public string SpeciesFactSaveSettings { get { return "Spara gjorda val"; } }//Resources.DyntaxaResource.SpeciesFactEditHostFactorsForSubstrateSaveSettingsButtonText; } }
            public string SpeciesFactNoItemsSelectedDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactNoItemsSelectedDialogHeader; } }
            public string SpeciesFactNoItemsSelectedDialogText { get { return Resources.DyntaxaResource.SpeciesFactNoItemsSelectedDialogText; } }
            public string FactorPopUpLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorItemHeader; } }
            public string HostPopUpLabel { get { return Resources.DyntaxaResource.SpeciesFactEditHostItemHeader; } }

            public string SpeciesFactDeleteFactorItemDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactDeleteFactorItemDialogHeader; } }
            public string SpeciesFactDeleteFactorItemDialogText { get { return Resources.DyntaxaResource.SpeciesFactDeleteFactorItemDialogText; } }
            public string SpeciesFactDeleteTaxonItemDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactDeleteTaxonItemDialogHeader; } }
            public string SpeciesFactDeleteTaxonItemDialogText { get { return Resources.DyntaxaResource.SpeciesFactDeleteTaxonItemDialogText; } }

            public string SpeciesFactNoHostItemsSelectedDialogText { get { return Resources.DyntaxaResource.SpeciesFactNoHostsItemsSelectedDialogText; } }

            public string SpeciesFactNoHostsItemsSelectedDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactNoHostsItemsSelectedDialogHeader; } }

            public string SpeciesFactNotSavedDialogText { get { return Resources.DyntaxaResource.SpeciesFactNotSavedDialogText; } }

            public string SpeciesFactNotSavedDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactNotSavedDialogHeader; } }

            public object SpeciesFactQualityHeader { get; set; }
        }

        public int MainParentFactorId { get; set; }

        public DyntaxaFactorDataType FactorDataType { get; set; }

        public DyntaxaDataType DataType { get; set; }
    }

    public class SpeciesFactHostViewModelItem
    {
        private string name = string.Empty;
        private string type = "taxon";
        private int factorId = 0;
        private bool isChecked = false;
        private string isHostToFactor = string.Empty;
        private string categoryName = string.Empty;
        private int taxonId = 0;
        private int isHostToFactorId = 0;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        public int FactorId
        {
            get { return factorId; }
            set { factorId = value; }
        }

        public int TaxonId
        {
            get { return taxonId; }
            set { taxonId = value; }
        }

        public bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        public string IsHostToFactor
        {
            get { return isHostToFactor; }
            set { isHostToFactor = value; }
        }

        public int IsHostToFactorId
        {
            get { return isHostToFactorId; }
            set { isHostToFactorId = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        public bool IsCategorySet { get; set; }

        public int CategoryId { get; set; }

        public bool IsShortList { get; set; }
    } 
}
