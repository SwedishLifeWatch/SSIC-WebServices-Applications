using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class represents a taxon name use type.
    /// In TaxonService this information has the name "taxon name status".
    /// </summary>
    [DataContract]
    public class WebTaxonNameUseType : WebData
    {
        /// <summary>
        /// Id for this taxon name use type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this taxon name use type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
