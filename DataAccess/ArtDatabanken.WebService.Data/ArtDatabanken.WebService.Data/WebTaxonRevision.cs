using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Class with information related to a taxon revision.
    /// </summary>
    [DataContract]
    public class WebTaxonRevision : WebData
    {
        /// <summary>
        /// Id of user that created the taxon revision.
        /// Not null. Must be set first time the object is saved.
        /// </summary>
        [DataMember]
        public Int32 CreatedBy
        { get; set; }

        /// <summary>
        /// Date and time when the taxon revision was created.
        /// Not null. Is set by the database.
        /// </summary>
        [DataMember]
        public DateTime CreatedDate
        { get; set; }

        /// <summary>
        /// Description of the revision.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Expected taxon revision end date.
        /// </summary>
        [DataMember]
        public DateTime ExpectedEndDate { get; set; }

        /// <summary>
        /// Expected taxon revision start date.
        /// </summary>
        [DataMember]
        public DateTime ExpectedStartDate { get; set; }

        /// <summary>
        /// GUID (Globally Unique Identifier) for this taxon revision.
        /// It is a LSID, which is unique for each version of the record
        /// holding the information included in this taxon revision. 
        /// It is updated automatically by database each time information is saved.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id for this taxon revision.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Id of user that modified the taxon revision.
        /// Not null. Must be set each time the object is saved.
        /// </summary>
        [DataMember]
        public Int32 ModifiedBy
        { get; set; }

        /// <summary>
        /// Date and time when the taxon revision was last modified.
        /// Not null. Is set by the database.
        /// </summary>
        [DataMember]
        public DateTime ModifiedDate
        { get; set; }

        /// <summary>
        /// Root taxon for this revision.
        /// The root taxon defines the scoop of the revision.
        /// This must not be the same taxon as the
        /// root taxon for the whole taxon tree.
        /// </summary>
        [DataMember]
        public WebTaxon RootTaxon { get; set; }

        /// <summary>
        /// Id for the current state of the revision.
        /// </summary>
        [DataMember]
        public Int32 StateId { get; set; }
    }
}
