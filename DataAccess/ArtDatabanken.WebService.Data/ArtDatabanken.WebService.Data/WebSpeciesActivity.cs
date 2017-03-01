using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about a species activity.
    /// For example "Carrying food for young"
    /// </summary>
    [DataContract]
    public class WebSpeciesActivity : WebData
    {
        /// <summary>
        /// Id for the species activity category that
        /// this species activity belongs to.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Int32 CategoryId
        { get; set; }
        
        /// <summary>
        /// GUID for this species activity.
        /// </summary>
        [DataMember]
        public String Guid { get; set; }

        /// <summary>
        /// Id for this species activity.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Identifier for this species activity.
        /// </summary>
        [DataMember]
        public String Identifier
        { get; set; }

        /// <summary>
        /// Name for this species activity.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// This species activity may be used together
        /// with these specified taxa and their child taxa.
        /// If property TaxonIds is empty this means that
        /// this activity may be used for all taxa.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }
    }
}
