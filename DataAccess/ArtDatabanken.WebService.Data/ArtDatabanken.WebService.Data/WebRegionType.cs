using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an region type,
    ///  for example "Political boundary" or "Validate regions".
    /// </summary>
    [DataContract]
    public class WebRegionType : WebData
    {
        /// <summary>
        /// Region type id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Region type name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
