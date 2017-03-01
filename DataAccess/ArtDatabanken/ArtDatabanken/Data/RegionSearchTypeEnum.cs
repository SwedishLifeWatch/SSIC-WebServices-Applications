using System.Runtime.Serialization;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Enum for type of search when there are regions 
    /// of type counties and provinces
    /// </summary>
    [DataContract]
    public enum CountyProvinceRegionSearchType
    {
        /// <summary>
        /// The default type of search. The ByCoordinate type is an optimized search. 
        /// When the region is a county or a province precalculated Id's of the observation are used
        /// for filtering the search result. The Id's are calculated using the coordinates of the observation.
        /// </summary>
        [EnumMember]
        ByCoordinate = 0,

        /// <summary>
        /// Search counties and provinces primary by name and secondary by coordinates
        /// An optimized search like the ByCoordinate RegionSearchType but the 
        /// precalculated Id's are calculated by matching by the region names.
        /// </summary>
        [EnumMember]
        ByName = 1
    }
}
