using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a factor field.
    /// </summary>
    [DataContract]
    public class WebFactorField : WebData
    {
        /// <summary>
        /// User id that created the record.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// This property is currently not used.
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date record was created.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Name for this factor field in database.
        /// </summary>
        [DataMember]
        public String DatabaseFieldName { get; set; }

        /// <summary>
        /// Id for this factor data type.
        /// </summary>
        [DataMember]
        public Int32 FactorDataTypeId { get; set; }

        /// <summary>
        /// Factor field enumeration id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 EnumId { get; set; }

        /// <summary>
        /// Id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Information for this factor field.
        /// </summary>
        [DataMember]
        public String Information { get; set; }

        /// <summary>
        /// Test if this factor field is of type enumeration.
        /// </summary>
        [DataMember]
        public Boolean IsEnumField { get; set; }

        /// <summary>
        /// Indicator of whether or not this factor field is the main field.
        /// </summary>
        [DataMember]
        public Boolean IsMain { get; set; }

        /// <summary>
        /// Indicator of whether or not this factor field is a substantial field.
        /// </summary>
        [DataMember]
        public Boolean IsSubstantial { get; set; }

        /// <summary>
        /// Label for this factor field.
        /// </summary>
        [DataMember]
        public String Label { get; set; }

        /// <summary>
        /// Id of the user that made the latest update of the record.
        /// Set by database when record is modified.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Date when the latest update of the record was done.
        /// Set by database when record is modified.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Maximum length of this factor field if it is of type String.
        /// </summary>
        [DataMember]
        public Int32 Size { get; set; }

        /// <summary>
        /// Type id for this factor field.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }

        /// <summary>
        /// Unit label for this factor field.
        /// </summary>
        [DataMember]
        public String Unit { get; set; }
    }
}