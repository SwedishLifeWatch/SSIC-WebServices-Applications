using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class contains information about a city.
    /// </summary>
    [DataContract]
    public class WebCityInformation : WebData
    {
        /// <summary>
        /// Gets or sets the value of the X coordinate
        /// </summary>
        [DataMember]
        public double CoordinateX { get; set; }

        /// <summary>
        /// Gets or sets the value of the Y coordinate
        /// </summary>
        [DataMember]
        public double CoordinateY { get; set; }

        /// <summary>
        /// Gets or sets the County name
        /// </summary>
        [DataMember]
        public string County { get; set; }

        /// <summary>
        /// Gets or sets the Municipality name
        /// </summary>
        [DataMember]
        public string Municipality { get; set; }

        /// <summary>
        /// Gets or sets the name of the City
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Parish name
        /// </summary>
        [DataMember]
        public string Parish { get; set; }

        /// <summary>
        /// Gets or sets the province name
        /// This property is currently not used
        /// </summary>
        [DataMember]
        public string Province { get; set; }
    }
}
