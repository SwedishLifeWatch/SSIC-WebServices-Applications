using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a factor update mode.
    /// </summary>
    [DataContract]
    public class WebFactorUpdateMode : WebData
    {
        /// <summary>
        /// Definition for this factor update mode.
        /// </summary>
        [DataMember]
        public String Definition { get; set; }

        /// <summary>
        /// Id for this factor update mode.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if factor may be updated.
        /// </summary>
        [DataMember]
        public Boolean IsUpdateAllowed { get; set; }

        /// <summary>
        /// Name for this factor update mode.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Type for this factor update mode.
        /// </summary>
        [DataMember]
        public FactorUpdateModeType Type { get; set; }
    }
}
