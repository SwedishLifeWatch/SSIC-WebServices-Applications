using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// SpecificationId of sort order when
    /// species observations are retrieved.
    /// </summary>
    [DataContract]
    public class WebSpeciesObservationFieldSortOrder : WebData
    {
        /// <summary>
        /// Type of data that the sort order should be applied to.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationClass Class
        { get; set; }


        /// <summary>
        /// Sort order for species observations field
        /// where class may have multiple instances of data.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public SortOrder ClassSortOrder
        { get; set; }

        /// <summary>
        /// Specifies if property ClassSortOrder has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsClassSortOrderSpecified
        { get; set; }

        /// <summary>
        /// Specifies if property PropertySortOrder has a value or not.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public Boolean IsPropertySortOrderSpecified
        { get; set; }

        /// <summary>
        /// Information about which species observation property
        /// that the sort order should be applied to.
        /// </summary>
        [DataMember]
        public WebSpeciesObservationProperty Property
        { get; set; }

        /// <summary>
        /// Sort order for species observations field
        /// where property may have multiple instances of data.
        /// This property is currently not used.
        /// </summary>
        [DataMember]
        public SortOrder PropertySortOrder
        { get; set; }

        /// <summary>
        /// Sort order for species observations.
        /// </summary>
        [DataMember]
        public SortOrder SortOrder
        { get; set; }
    }
}
