using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Information about usage of taxon names,
    /// (Accepted, Synonym, Homotypic, Heterotypic, proParte synonym, Misapplied (auct. name)).
    /// </summary>
    [DataContract]
    public class WebTaxonNameUsage : WebData
    {
        /// <summary>
        /// Description of the taxon name usage.
        /// </summary>
        [DataMember]
        public String Description { get; set; }

        /// <summary>
        /// Id of taxon name usage.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Name of the taxon name usage.
        /// </summary>
        [DataMember]
        public String Name { get; set; }
    }
}
