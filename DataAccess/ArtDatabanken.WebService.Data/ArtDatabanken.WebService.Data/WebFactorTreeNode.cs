using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains information about a factor tree node.
    /// </summary>
    [DataContract]
    public class WebFactorTreeNode : WebData
    {
        /// <summary>
        /// Children to this factor tree node.
        /// </summary>
        [DataMember]
        public List<WebFactorTreeNode> Children { get; set; }

        /// <summary>
        /// Factor for this factor tree node.
        /// </summary>
        [DataMember]
        public WebFactor Factor { get; set; }

        /// <summary>
        /// Id for this factor tree node.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Parents of this factor tree node.
        /// This property is not included in the web service contract.
        /// </summary>
        public List<WebFactorTreeNode> Parents { get; set; }

        /// <summary>
        /// Children to this factor tree node that is represented
        /// with factor ids instead of factor object to avoid
        /// problems with circular trees.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> RecursiveChildrenIds { get; set; }
    }
}