using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds factor tree filter information.
    /// </summary>
    [DataContract]
    public class WebFactorTreeSearchCriteria : WebData
    {
        /// <summary>
        /// Search for factor trees that is of one of
        /// these factor data types.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public List<Int32> FactorDataTypeIds { get; set; }

        /// <summary>
        /// Limit search to these factors.
        /// </summary>
        [DataMember]
        public List<Int32> FactorIds { get; set; }

        /// <summary>
        /// Indicates if only main factor relations
        /// should be included in returned trees.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsMainRelationRequired { get; set; }

        /// <summary>
        /// Indicates if only public factor relations and factors
        /// should be included in returned trees.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPublicRequired { get; set; }
    }
}