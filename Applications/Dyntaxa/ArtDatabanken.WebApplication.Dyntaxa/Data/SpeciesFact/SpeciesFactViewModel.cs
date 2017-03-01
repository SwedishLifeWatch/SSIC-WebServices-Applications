using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Helpers;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class SpeciesFactViewModel : BaseViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();
        private DyntaxaFactorId factorEnumId = DyntaxaFactorId.NOT_SUPPORTED;
        private DyntaxaDataType dataType = DyntaxaDataType.NOT_SUPPORTED;
        private DyntaxaFactorDataType factorDataType = DyntaxaFactorDataType.NOT_SUPPORTED;
        private int dropDownFactorId = 0;
        private int dropDownAllFactorId = 0;
        //private int dropDownTaxonId = 0;
       
        private string factorFieldValueTableHeader = Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue;
        private string factorFieldValue2TableHeader = Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue;

        public string FactorFieldValueTableHeader
        {
            get { return factorFieldValueTableHeader; }
            set { factorFieldValueTableHeader = value; }
        }

        public string FactorFieldValue2TableHeader
        {
            get { return factorFieldValue2TableHeader; }
            set { factorFieldValue2TableHeader = value; }
        }

        public DyntaxaFactorId MainParentFactorId
        {
            get { return factorEnumId; }
            set { factorEnumId = value; }
        }

        public DyntaxaDataType DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public DyntaxaFactorDataType FactorDataType
        {
            get { return factorDataType; }
            set { factorDataType = value; }
        }

        public int TaxonId { get; set; }
        public string PostAction { get; set; }
        public string ReferenceId { get; set; }
        public IList<SpeciesFactViewModelHeaderItem> SpeciesFactViewModelHeaderItemList { get; set; }

        [Required]
        [LocalizedDisplayName("SpeciesFactEditFactorsAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int DropDownFactorId
        {
            get { return dropDownFactorId; }
            set { dropDownFactorId = value; }
        }

        public IList<SpeciesFactDropDownModelHelper> AllAvaliableFactors { get; set; }

        [Required]
        [LocalizedDisplayName("SpeciesFactEditAllFactorsAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int DropDownAllFactorId
        {
            get { return dropDownAllFactorId; }
            set { dropDownAllFactorId = value; }
        }

        //[Required]
        //[LocalizedDisplayName("SpeciesFactEditTaxonAddDropDownName", NameResourceType = typeof(Resources.DyntaxaResource))]
        //public int DropDownTaxonId
        //{
        //    get { return dropDownTaxonId; }
        //    set { dropDownTaxonId = value; }
        //}

        ///// <summary>
        ///// Set the selected individual category for selected data
        ///// </summary>
        //public DyntaxaIndividualCategory IndividualCategory { get; set; }

        /// <summary>
        /// List of all individual categories
        /// </summary>
        public IList<SpeciesFactDropDownModelHelper> FactorList { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorsHeaderText; } }
            public string GetExcelFile { get { return Resources.DyntaxaResource.SpeciesFactExportGetExcelFile; } }
            public string GeneratingExcelFile { get { return Resources.DyntaxaResource.SpeciesFactExportGeneratingExcelFile; } }
            public string SaveNewValuesLabel { get { return "Spara nya värden"; } }
            public string SaveLabel { get { return Resources.DyntaxaResource.SharedSaveButtonText; } }
            public string ResetLabel { get { return "Återställ till senast sparade"; } }
            public string CancelLabel { get { return Resources.DyntaxaResource.SharedCancelButtonText; } }
            public string LoadingLabel { get { return Resources.DyntaxaResource.SharedLoading; } }
            public string AddLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorsAddFactorLabel; } }
            public string AddButtonLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorsAddButton; } }
            public string YesLabel { get { return Resources.DyntaxaResource.SharedYesButtonText; } }
            public string NoLabel { get { return Resources.DyntaxaResource.SharedNoButtonText; } }

            public string SpeciesFactMainHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeader1; } }
            public string SpeciesFactCategoryHeader { get { return Resources.DyntaxaResource.SpeciesFactListHeaderIndividualCategory; } }
            public string SpeciesFactFactorValueHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue; } }
            public string SpeciesFactCommentHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderComment; } }
            public string SpeciesFactQualityHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderQuality; } }
            public string SpeciesFactFactorId { get { return Resources.DyntaxaResource.SpeciesFactListHeaderFactorID; } }
            public string SpeciesFactSortOrder { get { return Resources.DyntaxaResource.SpeciesFactListHeaderSortOrder; } }
            public string FactorPopUpLabel { get { return Resources.DyntaxaResource.SpeciesFactEditFactorItemHeader; } }

            public object SpeciesFactAddFactorContinueWithoutSaveDialogHeader { get { return Resources.DyntaxaResource.SpeciesFactAddFactorContinueWithoutSaveDialogHeader; } }
            public object SpeciesFactAddFactorContinueWithoutSaveDialogText { get { return Resources.DyntaxaResource.SpeciesFactAddFactorContinueWithoutSaveDialogText; } }
        }
    }

    public class SpeciesFactViewModelHeaderItem
    {
        private SpeciesFactViewModelItem speciesFactViewModelItem = new SpeciesFactViewModelItem();
        private IList<SpeciesFactViewModelSubHeaderItem> speciecFactViewModelSubHeaderItemList = new List<SpeciesFactViewModelSubHeaderItem>();

        public SpeciesFactViewModelItem SpeciesFactViewModelItem
        {
            get { return speciesFactViewModelItem; }
            set { speciesFactViewModelItem = value; }
        }

        public IList<SpeciesFactViewModelSubHeaderItem> SpeciecFactViewModelSubHeaderItemList
        {
            get { return speciecFactViewModelSubHeaderItemList; }
            set { speciecFactViewModelSubHeaderItemList = value; }
        }
    }

    public class SpeciesFactViewModelSubHeaderItem
    {
        private SpeciesFactViewModelItem speciesFactViewModelItem = new SpeciesFactViewModelItem();
        private IList<SpeciesFactViewModelItem> speciecFactViewModelItemList = new List<SpeciesFactViewModelItem>();

        public SpeciesFactViewModelItem SpeciesFactViewModelItem
        {
            get { return speciesFactViewModelItem; }
            set { speciesFactViewModelItem = value; }
        }

        public IList<SpeciesFactViewModelItem> SpeciesFactViewModelItemList
        {
            get { return speciecFactViewModelItemList; }
            set { speciecFactViewModelItemList = value; }
        }

        public bool IsShortList { get; set; }
    }

    public class SpeciesFactViewModelItem
    {
        private string mainHeader = string.Empty;
        private string subHeader = string.Empty;
        private string superiorHeader = string.Empty;
        private string factorName = string.Empty;
        private string individualCategory = string.Empty;
        private int individualCategoryId = 0;
        private string factorFieldValue = string.Empty;
        private string factorFieldValue2 = string.Empty;
        private string factorFieldComment = string.Empty;
        private string quality = string.Empty;
        private int qualityId = 0;
        private int referenceId = 0;
        private string factorId = string.Empty;
        private string factorSortOrder = string.Empty;
        private bool isMainHeader = false;
        private bool isSubHeader = false;
        private bool isSuperiorHeader = false;
        private bool isMarked = false;
        private bool useDifferentColor = false;
        private int useDifferentColorFromIndex = 1;
        private SpeciesFactViewModelItemFieldValues fieldValues = null;
        private SpeciesFactViewModelItemFieldValues fieldValues2 = null;
        private SpeciesFactViewModelItemFieldValues qualityValues = null;
        private string taxonId = string.Empty;
        private bool isHost = false;
        private int hostId = 0;
        private int mainParentFactorId = 0;
        private bool isOkToUpdate = true;

        public bool IsOkToUpdate
        {
            get { return isOkToUpdate; }
            set { isOkToUpdate = value; }
        }

        public string MainHeader
        {
            get { return mainHeader; }
            set { mainHeader = value; }
        }

        public int IndividualCategoryId
        {
            get { return individualCategoryId; }
            set { individualCategoryId = value; }
        }

        public string IndividualCategoryName
        {
            get { return individualCategory; }
            set { individualCategory = value; }
        }

        public string FactorFieldValue
        {
            get { return factorFieldValue; }
            set { factorFieldValue = value; }
        }

        public string FactorFieldValue2
        {
            get { return factorFieldValue2; }
            set { factorFieldValue2 = value; }
        }

        public string FactorFieldComment
        {
            get { return factorFieldComment; }
            set { factorFieldComment = value; }
        }

        public string Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        public int QualityId
        {
            get { return qualityId; }
            set { qualityId = value; }
        }

        public int ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        public string FactorId
        {
            get { return factorId; }
            set { factorId = value; }
        }

        public string FactorSortOrder
        {
            get { return factorSortOrder; }
            set { factorSortOrder = value; }
        }

        public bool IsMainHeader
        {
            get { return isMainHeader; }
            set { isMainHeader = value; }
        }

        public string SubHeader
        {
            get { return subHeader; }
            set { subHeader = value; }
        }

        public string SuperiorHeader
        {
            get { return superiorHeader; }
            set { superiorHeader = value; }
        }

        public string FactorName
        {
            get { return factorName; }
            set { factorName = value; }
        }

        public bool IsMarked
        {
            get { return isMarked; }
            set { isMarked = value; }
        }

        public bool IsSubHeader
        {
            get { return isSubHeader; }
            set { isSubHeader = value; }
        }

        public bool IsSuperiorHeader
        {
            get { return isSuperiorHeader; }
            set { isSuperiorHeader = value; }
        }

        public bool UseDifferentColor
        {
            get { return useDifferentColor; }
            set { useDifferentColor = value; }
        }

        public int UseDifferentColorFromIndex
        {
            get { return useDifferentColorFromIndex; }
            set { useDifferentColorFromIndex = value; }
        }

        public SpeciesFactViewModelItemFieldValues FieldValues 
        {
            get { return fieldValues; }
            set { fieldValues = value; } 
        }

        public SpeciesFactViewModelItemFieldValues FieldValues2
        {
            get { return fieldValues2; }

            set { fieldValues2 = value; }
        }

        public SpeciesFactViewModelItemFieldValues QualityValues
        {
            get { return qualityValues; }
            set { qualityValues = value; }
        }
        public string TaxonId
        {
            get { return taxonId; }
            set { taxonId = value; }
        }

        public bool IsHost
        {
            get { return isHost; }
            set { isHost = value; }
        }

         public int HostId
        {
            get { return hostId; }
            set { hostId = value; }
        }
        public int MainParentFactorId
         {
             get { return mainParentFactorId; }
             set { mainParentFactorId = value; }
         }

       // public bool HasHost { get; set; }

        public bool IsShortList { get; set; }

        public bool IsHeader { get; set; }
    }

    public class SpeciesFactViewModelItemFieldValues
    {
        private string fieldName = string.Empty;
        private int fieldValue = 0;
        IList<KeyValuePair<string, int>> factorFieldValues = new List<KeyValuePair<string, int>>();
        private DyntaxaFactorId factorEnumId = DyntaxaFactorId.NOT_SUPPORTED;
        IList<KeyValuePair<int, string>> qualityValues = new List<KeyValuePair<int, string>>();

        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        public int FieldValue
        {
            get { return fieldValue; }
            set { fieldValue = value; }
        }

        public IList<KeyValuePair<string, int>> FactorFieldValues
        {
            get { return factorFieldValues; }
            set { factorFieldValues = value; }
        }

        public DyntaxaFactorId MainParentFactorId
        {
            get { return factorEnumId; }
            set { factorEnumId = value; }
        }

        public IList<KeyValuePair<int, string>> QualityValues
        {
            get { return qualityValues; }
            set { qualityValues = value; }
        }

        public int FieldKey { get; set; }
    } 
}
