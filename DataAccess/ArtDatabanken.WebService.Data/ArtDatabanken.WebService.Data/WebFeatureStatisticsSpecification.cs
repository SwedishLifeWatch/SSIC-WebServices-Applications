using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about what statistics are requested
    /// from an web feature service and wich spatial feature type that is to be measured.
    /// </summary>
    [DataContract]
    public class WebFeatureStatisticsSpecification : WebData
    {
        
        /// <summary>
        ///  Type of feature that are to be measured.
        /// </summary>
        [DataMember]
        public FeatureType FeatureType { get; set; }
       
    }
}
