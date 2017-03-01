using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a factor field enumeration value.
    /// </summary>
    [DataContract]
    public class WebFactorFieldEnumValue : WebData
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
        /// Id for the factor field enumeration which this
        /// factor field enumeration value belongs to.
        /// </summary>
        [DataMember]
        public Int32 EnumId { get; set; }

        /// <summary>
        /// Id for this factor field enumeration value.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Information and definition text for
        /// this field enumeration value.
        /// </summary>
        [DataMember]
        public String Information { get; set; }

        /// <summary>
        /// Indication about whether or not this
        /// field enumeration has a integer key.
        /// </summary>
        [DataMember]
        public Boolean IsKeyIntegerSpecified { get; set; }

        /// <summary>
        /// Key integer value for this field enumeration value.
        /// </summary>
        [DataMember]
        public Int32 KeyInteger { get; set; }

        /// <summary>
        /// Key text value for this field enumeration value.
        /// </summary>
        [DataMember]
        public String KeyText { get; set; }

        /// <summary>
        /// Label for this field enumeration value.
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
        /// Indication about whether or not this
        /// field enumeration value should be saved to Database.
        /// </summary>
        [DataMember]
        public Boolean ShouldBeSaved { get; set; }

        /// <summary>
        /// Sort order for this field enumeration value.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }
    }
}