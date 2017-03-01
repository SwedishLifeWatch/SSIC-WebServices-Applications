using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// SpecificationId of paging information when
    /// species observations are retrieved.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationPageSpecification : WebData
    {
        /// <summary>
        /// Page size, i.e. how many species observations to be
        /// returned in each call to the web service
        /// Min page size is 1
        /// Max page size is 10000.
        /// </summary>
        [DataMember]
        public Int64 Size
        { get; set; }


        /// <summary>
        /// Defines how species observations should be sorted
        /// before paging is applied to the result set.
        /// </summary>
        [DataMember]
        public List<WebSpeciesObservationFieldSortOrder> SortOrder
        { get; set; }

        /// <summary>
        /// Start position for the page. 
        /// Min Start position is 1.
        /// If you write a to large position no rows will be returned.
        /// </summary>
        [DataMember]
        public Int64 Start 
        { get; set; }
    }
}
