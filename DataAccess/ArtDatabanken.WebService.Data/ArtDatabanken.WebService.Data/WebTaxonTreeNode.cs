using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information about a taxon tree node.
    /// No information about WebTaxonRelation is included except sort order
    /// and taxon relation that is reflected in the taxon tree structure.
    /// 
    /// The following dynamic properties exists: 
    /// ChildrenCircularTaxonIds (String)
    /// The string consists of a comma seperated list of circular child taxon ids.
    /// </summary>
    [DataContract]
    public class WebTaxonTreeNode : WebData
    {
        /// <summary>
        /// Children to this taxon tree node.
        /// </summary>
        [DataMember]
        public List<WebTaxonTreeNode> Children
        { get; set; }

        /// <summary>
        /// Taxon for this taxon tree node.
        /// </summary>
        [DataMember]
        public WebTaxon Taxon
        { get; set; }
    }
}
