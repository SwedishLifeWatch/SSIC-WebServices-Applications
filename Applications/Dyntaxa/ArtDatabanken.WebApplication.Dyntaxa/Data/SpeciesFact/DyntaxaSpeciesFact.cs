using System;
using System.Collections.Generic;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    /// <summary>
    /// This class represents a DyntaxaSpeciesFact for a taxon.
    /// </summary>
    public class DyntaxaSpeciesFact : ArtDatabanken.WebApplication.Dyntaxa.DyntaxaFactor
    {
        private string information = string.Empty;
        private string hostLabel = string.Empty;
        private DyntaxaFactorQuality quality = null;
        private int referenceId = int.MaxValue;
        private string referenceName = "-";
        private IReferenceRelation dyntaxaFactorReference = null;
        private string updateUserFullName = null;
        private DateTime updateDate = new DateTime(DateTime.Now.Ticks);
        private int hostId = int.MaxValue;
        private int factorId = 0;

        private IList<DyntaxaFactorField> fields = new List<DyntaxaFactorField>();
        private IList<DyntaxaPeriod> periodList = new List<DyntaxaPeriod>(); 
        private IList<DyntaxaHost> hostList = new List<DyntaxaHost>();
        private IList<DyntaxaIndividualCategory> individualCategoriesList = new List<DyntaxaIndividualCategory>();
        private IList<KeyValuePair<int, string>> factorEnumValueList = new List<KeyValuePair<int, string>>();
        private IList<KeyValuePair<int, string>> factorEnumValueList2 = new List<KeyValuePair<int, string>>();

        private string identifier = string.Empty;

        /// <summary>
        /// Creates DyntaxaSpeciesFact.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="information"></param>
        /// <param name="label"></param>
        /// <param name="isLeaf"></param>
        /// <param name="isPeriodic"></param>
        /// <param name="sortOrder"></param>
        /// <param name="isPublic"></param>
        /// <param name="isTaxonomic"></param>
        /// <param name="hostLabel"></param>
        /// <param name="quality"></param>
        /// <param name="fields"></param>
        /// <param name="factorOrigin"></param>
        /// <param name="factorUpdateMode"></param>
        /// <param name="identifier"></param>
        public DyntaxaSpeciesFact(
            string id, 
            string information, 
            string label, 
            bool isLeaf, 
            bool isPeriodic, 
            int sortOrder, 
            bool isPublic, 
            bool isTaxonomic,
            string hostLabel, 
            int hostId, 
            DyntaxaFactorQuality quality, 
            int referenceId, 
            IList<DyntaxaFactorField> fields, 
            DyntaxaFactorOrigin factorOrigin, 
            DyntaxaFactorUpdateMode factorUpdateMode,
            string identifier, 
            DateTime updateDate, 
            DyntaxaIndividualCategory individualCategory, 
            int factorId, 
            string referenceName, 
            IReferenceRelation dyntaxaFactorReference = null, 
            IList<KeyValuePair<int, string>> factorEnumValueList = null, 
            IList<KeyValuePair<int, string>> factorEnumValueList2 = null, 
            string updateUserFullName = "")
            : base(id, label, isLeaf, isPeriodic, sortOrder, isPublic, isTaxonomic, factorOrigin, factorUpdateMode, factorId)
        {
            this.information = information;
            this.hostLabel = hostLabel;
            this.hostId = hostId;
            this.quality = quality;
            this.referenceId = referenceId;
            this.fields = fields;
            this.identifier = identifier; 
          
            this.dyntaxaFactorReference = dyntaxaFactorReference;
            this.factorEnumValueList = factorEnumValueList;
            this.factorEnumValueList2 = factorEnumValueList2;
            this.updateUserFullName = updateUserFullName;
            this.updateDate = updateDate;
            if (individualCategory.IsNotNull())
            {
                this.IndividualCatgory = individualCategory;
            }

            this.factorId = factorId;
            this.referenceName = referenceName;
        }

        public DyntaxaSpeciesFact()
        {
        }

        public DyntaxaFactor Factor
        {
            get
            {
                return this;
            }
        }
        public string Information
        {
            get
            {
                return information;
            }
            set
            {
                information = value;
            }
        }

        public string HostLabel
        {
            get { return hostLabel; }
            set { hostLabel = value; }
        }

        public new int HostId
        {
            get { return hostId; }
            set { hostId = value; }
        }

        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        public IList<DyntaxaFactorField> Fields
        {
             get { return fields; }
        }

        public IList<DyntaxaPeriod> PeriodList
        {
            get { return periodList; }
            set { periodList = value; }
        }

        public IList<DyntaxaIndividualCategory> IndividualCategoryList
        {
            get { return individualCategoriesList; }
            set { individualCategoriesList = value; }
        }

        public IList<DyntaxaHost> HostList
        {
            get { return hostList; }
            set { hostList = value; }
        }
        public ArtDatabanken.WebApplication.Dyntaxa.DyntaxaFactorQuality Quality
        {
            get { return quality; }
            set { quality = value; }
        }

        public int ReferenceId
        {
            get { return referenceId; }
            set { referenceId = value; }
        }

        public string ReferenceName
        {
            get { return referenceName; }
            set { referenceName = value; }
        }

        public DyntaxaIndividualCategory IndividualCatgory { get; set; }

        public string ExistingEvaluations { get; set; }

        public string Comments { get; set; }

        public int FactorEnumValue { get; set; }
        public int FactorEnumValue2 { get; set; }

        public string FactorEnumLabel { get; set; }
        public string FactorEnumLabel2 { get; set; }

        public IReferenceRelation DyntaxaFactorReference
        {
            get { return dyntaxaFactorReference; }
            set { dyntaxaFactorReference = value; }
        }

         public IList<KeyValuePair<int, string>> FactorEnumValueList
         {
             get { return factorEnumValueList; }
             set { factorEnumValueList = value; }
         }

         public IList<KeyValuePair<int, string>> FactorEnumValueList2
         {
             get { return factorEnumValueList2; }
             set { factorEnumValueList2 = value; }
         }

         public string UpdateUserFullName
         {
             get { return updateUserFullName; }
             set { updateUserFullName = value; }
         }

         public DateTime UpdateDate
         {
             get { return updateDate; }
             set { updateDate = value; }
         }

         public int FactorIdForHosts
         {
             get { return factorId; }
         }

         public string HostName { get; set; }
    }
}