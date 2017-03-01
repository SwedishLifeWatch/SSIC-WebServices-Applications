using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an area type,
    ///  for example "Political boundary" or "Validate regions".
    /// </summary>
    [DataContract]
    public class WebAreaType : WebData
    {
        /// <summary>
        /// Area type id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Area type name.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
