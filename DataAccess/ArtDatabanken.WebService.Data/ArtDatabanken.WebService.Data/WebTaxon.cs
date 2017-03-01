using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a taxon.
    /// </summary>
    [DataContract]
    public class WebTaxon : WebData
    {
        /// <summary>
        /// Alert status for taxon.
        /// </summary>
        [DataMember]
        public Int32 AlertStatusId { get; set; }

        /// <summary>
        /// Author of the recommended scientific name.
        /// </summary>
        [DataMember]
        public String Author { get; set; }

        /// <summary>
        /// Category that this taxon belongs to.
        /// </summary>
        [DataMember]
        public Int32 CategoryId { get; set; }

        /// <summary>
        /// Indicates changes related to this taxon concept.
        /// </summary>
        [DataMember]
        public Int32 ChangeStatusId { get; set; }

        /// <summary>
        /// Recommended common name.
        /// Not all taxa has a recommended common name.
        /// </summary>
        [DataMember]
        public String CommonName { get; set; }

        /// <summary>
        /// Concept definition for a taxon. 
        /// Not required ie could be null.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public String ConceptDefinition { get; set; }

        /// <summary>
        /// UserId that created the record.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// </summary> 
        [DataMember]
        public Int32 CreatedBy { get; set; }

        /// <summary>
        /// Date record was created.
        /// Set by database when inserted.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this object.
        /// It is a LSID, which is unique for each version of the record holding the information included in this object. 
        /// It is updated automatically by database each time information is saved.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// TaxonId unique identification of a taxon.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if this taxon has been graded or not.
        /// Grading is when a taxon changes taxon category.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsGraded { get; set; }

        /// <summary>
        /// Indicates if this taxon is in a checked out revision
        /// and may be updated.
        /// </summary>
        [DataMember]
        public Boolean IsInRevision { get; set; }

        /// <summary>
        /// Gets or sets IsPublished
        /// </summary>
        [DataMember]
        public Boolean IsPublished { get; set; }

        /// <summary>
        /// IsValid
        /// true - taxon is valid.
        /// false - taxon is NOT valid.
        /// </summary>
        [DataMember]
        public Boolean IsValid { get; set; }

        /// <summary>
        /// Id of the User that modified the record.
        /// Set by database when record is modified.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy { get; set; }

        /// <summary>
        /// Date taxon was modified
        /// Set by database when revision with taxon is checked in.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Part of concept definition for a taxon. 
        /// Not required ie could be null.
        /// </summary>
        [DataMember]
        public String PartOfConceptDefinition { get; set; }

        /// <summary>
        /// Recommended scientific name.
        /// </summary>
        [DataMember]
        public String ScientificName { get; set; }

        /// <summary>
        /// SortOrder
        /// Sorting order for this taxon.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Date user is valid from. Not Null. Is set to date created by default.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public DateTime ValidFromDate { get; set; }

        /// <summary>
        /// Date user is valid to. Not Null. Is set to date created + 100 years by default.
        /// Mandatory ie always required.
        /// </summary>
        [DataMember]
        public DateTime ValidToDate { get; set; }
    }
}
