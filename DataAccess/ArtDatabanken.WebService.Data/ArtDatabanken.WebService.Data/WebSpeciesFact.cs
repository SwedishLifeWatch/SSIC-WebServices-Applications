using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a species fact.
    /// </summary>
    [DataContract]
    public class WebSpeciesFact : WebData
    {
        /// <summary>
        /// Factor Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 FactorId { get; set; }

        /// <summary>
        /// Field value 1 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue1 { get; set; }

        /// <summary>
        /// Field value 2 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue2 { get; set; }

        /// <summary>
        /// Field value 3 of this species fact.
        /// </summary>
        [DataMember]
        public Double FieldValue3 { get; set; }

        /// <summary>
        /// Field value 4 of this species fact.
        /// </summary>
        [DataMember]
        public String FieldValue4 { get; set; }

        /// <summary>
        /// Field value 5 of this species fact.
        /// </summary>
        [DataMember]
        public String FieldValue5 { get; set; }

        /// <summary>
        /// Host id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 HostId { get; set; }

        /// <summary>
        /// Id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Individual category id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 IndividualCategoryId { get; set; }

        /// <summary>
        /// Indication about whether or not field 1 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue1Specified { get; set; }

        /// <summary>
        /// Indication about whether or not field 2 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue2Specified { get; set; }

        /// <summary>
        /// Indication about whether or not field 3 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue3Specified { get; set; }

        /// <summary>
        /// Indication about whether or not field 4 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue4Specified { get; set; }

        /// <summary>
        /// Indication about whether or not field 5 has a Value for this species fact.
        /// </summary>
        [DataMember]
        public Boolean IsFieldValue5Specified { get; set; }

        /// <summary>
        /// Indication about whether or not this species fact has a host value.
        /// </summary>
        [DataMember]
        public Boolean IsHostSpecified { get; set; }

        /// <summary>
        /// Indication about whether or not this species fact has a period value.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodSpecified { get; set; }

        /// <summary>
        /// Name of the person that made the last
        /// update of this species fact.
        /// </summary>
        [DataMember]
        public String ModifiedBy { get; set; }

        /// <summary>
        /// Date when the species fact was last updated.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Period id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 PeriodId { get; set; }

        /// <summary>
        /// Quality id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 QualityId { get; set; }

        /// <summary>
        /// Reference id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 ReferenceId { get; set; }

        /// <summary>
        /// Taxon id for this species fact.
        /// </summary>
        [DataMember]
        public Int32 TaxonId { get; set; }
    }
}
