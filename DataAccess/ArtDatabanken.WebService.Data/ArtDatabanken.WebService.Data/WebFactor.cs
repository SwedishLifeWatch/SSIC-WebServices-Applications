using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents a factor.
    /// </summary>
    [DataContract]
    public class WebFactor : WebData
    {
        /// <summary>
        /// Factor is of this category.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 CategoryId { get; set; }

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
        /// Id of the factor data type of this factor.
        /// Property IsDataTypeIdSpecified indicates if
        /// this property has a value or not.
        /// </summary>
        [DataMember]
        public Int32 DataTypeId { get; set; }

        /// <summary>
        /// Taxon id for parent taxon for all
        /// potential host taxa associated with this factor.
        /// This property is used only if the property
        /// IsTaxonomic is true.
        /// </summary>
        [DataMember]
        public Int32 DefaultHostParentTaxonId { get; set; }

        /// <summary>
        /// Host label for this factor.
        /// This property is used only if the property
        /// IsTaxonomic is true.
        /// </summary>
        [DataMember]
        public String HostLabel { get; set; }

        /// <summary>
        /// Id for this factor.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Information about this factor.
        /// </summary>
        [DataMember]
        public String Information { get; set; }

        /// <summary>
        /// Indicates if this factor has a data type or not.
        /// </summary>
        [DataMember]
        public Boolean IsDataTypeIdSpecified { get; set; }

        /// <summary>
        /// Indicates if this factor is a leaf in the factor tree.
        /// </summary>
        [DataMember]
        public Boolean IsLeaf { get; set; }

        /// <summary>
        /// Indication about whether or not this factor is periodic.
        /// </summary>
        [DataMember]
        public Boolean IsPeriodic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor
        /// should be available for public use.
        /// </summary>
        [DataMember]
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// Indication about whether or not this factor
        /// can be associated with a host taxon.
        /// </summary>
        [DataMember]
        public Boolean IsTaxonomic { get; set; }

        /// <summary>
        /// Label for this factor.
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
        /// Name for this factor.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Id of the factor origin of this factor.
        /// </summary>
        [DataMember]
        public Int32 OriginId { get; set; }

        /// <summary>
        /// Sort order for this factor.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Id of the factor type of this factor.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }

        /// <summary>
        /// Id of the factor update mode of this factor.
        /// </summary>
        [DataMember]
        public Int32 UpdateModeId { get; set; }
    }
}