using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Handles a relation between a reference and another object,
    /// for example Taxon or TaxonName.
    /// </summary>
    [DataContract]
    public class WebReferenceRelation : WebData
    {
        /// <summary>
        /// Id of the reference relation.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Id of the reference.
        /// </summary>
        [DataMember]
        public Int32 ReferenceId { get; set; }

        /// <summary>
        /// Guid for the object that the reference is related to.
        /// </summary>
        [DataMember]
        public String RelatedObjectGuid { get; set; }

        /// <summary>
        /// Id for the reference relation type.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }
    }
}
