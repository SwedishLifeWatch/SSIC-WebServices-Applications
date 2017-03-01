using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    public class FactorViewModel : BaseViewModel
    {
        private readonly ModelLabels _labels = new ModelLabels();
      //  private DyntaxaFactorId factorEnumId = DyntaxaFactorId.NOT_SUPPORTED;
        private string mainHeader = string.Empty;
        private string subHeader = string.Empty;
        private string superiorHeader = string.Empty;
        private string factorName = string.Empty;
        private int individualCategoryId = 0;
        private int oldIndividualCategoryId = 0;
        private int factorFieldEnumValue = 0;
        private int factorFieldEnumValue2 = 0;
        private string factorFieldComment = string.Empty;
        private int qualityId = 0;
        private string factorSortOrder = string.Empty;
        private bool isMainHeader = false;
        private bool isSubHeader = false;
        private bool isSuperiorHeader = false;
        private bool isMarked = false;
        private bool useDifferentColor = false;
        private int useDifferentColorFromIndex = 1;
        private FactorFieldViewModel fieldValues = null;
        private FactorFieldViewModel fieldValues2 = null;
        private string factorFieldValueTableHeader = Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue;
        private string factorReferenceOld = "-";

        public int ReferenceId { get; set; }

        [Required]
        [LocalizedDisplayName("SpeciesFactSharedExistingEvaluations", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string ExistingEvaluations { get; set; }
        public string MainHeader
        {
            get { return mainHeader; }
            set { mainHeader = value; }
        }
        [Required]
        [LocalizedDisplayName("SpeciesFactSharedIndividualCategory", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int IndividualCategoryId
        {
            get { return individualCategoryId; }
            set { individualCategoryId = value; }
        }

         public int OldIndividualCategoryId
        {
            get { return oldIndividualCategoryId; }
            set { oldIndividualCategoryId = value; }
        }
        public string IndividualCategoryName { get; set; }

        /// <summary>
        /// List of all individual categories
        /// </summary>
        public IList<SpeciesFactDropDownModelHelper> IndividualCategoryList { get; set; }

        [Required]
        public int FactorFieldEnumValue
        {
            get { return factorFieldEnumValue; }
            set { factorFieldEnumValue = value; }
        }

        public int FactorFieldEnumValue2
        {
            get { return factorFieldEnumValue2; }
            set { factorFieldEnumValue2 = value; }
        }
        public IList<SpeciesFactDropDownModelHelper> FactorFieldEnumValueList { get; set; }
        public IList<SpeciesFactDropDownModelHelper> FactorFieldEnumValueList2 { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.Text)]
        [AllowHtml]
        [LocalizedDisplayName("SpeciesFactExcelListHeaderComment", NameResourceType = typeof(Resources.DyntaxaResource))]               
        public string FactorFieldComment
        {
            get { return factorFieldComment; }
            set { factorFieldComment = value; }
        }

        [Required]
        [LocalizedDisplayName("SpeciesFactExcelListHeaderQuality", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int QualityId
        {
            get { return qualityId; }
            set { qualityId = value; }
        }

        public IList<SpeciesFactDropDownModelHelper> QualityValueList { get; set; }

         public string FactorFieldEnumLabel { get; set; }
         public string FactorFieldEnumLabel2 { get; set; }

         public string HostTaxaHeader { get; set; }
         public string HostTaxaText { get; set; }

        //[LocalizedDisplayName("SharedReferenceLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        //public string ReferenceName { get; set; }

        //public int ReferenceId { get; set; }

        //public string FactorId
        //{
        //    get { return factorId; }
        //    set { factorId = value; }
        //}

        /// <summary>
        /// Selected faktor reference id
        /// </summary>
        [LocalizedDisplayName("SharedReferenceNewLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public int FactorReferenceId { get; set; }

        [LocalizedDisplayName("SharedReferenceOldLabel", NameResourceType = typeof(Resources.DyntaxaResource))]
        public string FactorReferenceOld
        {
            get { return factorReferenceOld; }
            set { factorReferenceOld = value; }
        }

        /// <summary>
        /// A list of  factor references.
        /// </summary>
        public IList<SpeciesFactDropDownModelHelper> FaktorReferenceList { get; set; }

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

        public FactorFieldViewModel FieldValues
        {
            get { return fieldValues; }
            set { fieldValues = value; }
        }

        public FactorFieldViewModel FieldValues2
        {
            get { return fieldValues2; }
            set { fieldValues2 = value; }
        }

        public string FactorFieldValueTableHeader
        {
            get { return factorFieldValueTableHeader; }
            set { factorFieldValueTableHeader = value; }
        }

        //public DyntaxaFactorEnumId FactorEnumId
        //{
        //    get { return factorEnumId; }
        //    set { factorEnumId = value; }
        //}

        //public  DyntaxaDataType DataType
        //{
        //    get { return dataType; }
        //    set { dataType = value; }
        //}

        //public DyntaxaFactorDataType FactorDataType
        //{
        //    get { return factorDataType; }
        //    set { factorDataType = value; }
        //}
        public int FactorDataTypeId { get; set; }
        public int TaxonId { get; set; }
        public int ChildFactorId { get; set; }
        public string PostAction { get; set; }
        public IList<SpeciesFactViewModelHeaderItem> SpeciesFactViewModelHeaderItemList { get; set; }

        public ModelLabels Labels
        {
            get { return _labels; }
        }

        /// <summary>
        /// Localized labels
        /// </summary>
        public class ModelLabels
        {
            public string TitleLabel { get { return Resources.DyntaxaResource.SpeciesFactExportTitle; } }
            public string DatabaseLabel { get { return Resources.DyntaxaResource.SpeciesFactDatabaseTitle; } }
            public string FactorLabel { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeader1; } }
            public string GetExcelFile { get { return Resources.DyntaxaResource.SpeciesFactExportGetExcelFile; } }
            public string GeneratingExcelFile { get { return Resources.DyntaxaResource.SpeciesFactExportGeneratingExcelFile; } }

            public string SpeciesFactMainHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeader1; } }
            public string SpeciesFactCategoryHeader { get { return Resources.DyntaxaResource.SpeciesFactListHeaderIndividualCategory; } }
            public string SpeciesFactFactorValueHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderValue; } }
            public string SpeciesFactCommentHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderComment; } }
            public string SpeciesFactQualityHeader { get { return Resources.DyntaxaResource.SpeciesFactExcelListHeaderQuality; } }
            public string SpeciesFactFactorId { get { return Resources.DyntaxaResource.SpeciesFactListHeaderFactorID; } }
            public string SpeciesFactSortOrder { get { return Resources.DyntaxaResource.SpeciesFactListHeaderSortOrder; } }
        }

        public class FactorFieldViewModel
        {
            private string fieldName = string.Empty;
            private int fieldValue = 0;
            IList<KeyValuePair<string, int>> factorFieldValues = new List<KeyValuePair<string, int>>();
            private DyntaxaFactorId factorEnumId = DyntaxaFactorId.NOT_SUPPORTED;

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

            public DyntaxaFactorId FactorEnumId
            {
                get { return factorEnumId; }
                set { factorEnumId = value; }
            }
        }

        public string UpdateUserData { get; set; }

        public int HostId { get; set; }

        public bool IsNewCategory { get; set; }

        public int DataTypeId { get; set; }

        public int MainParentFactorId { get; set; }
    }
}
