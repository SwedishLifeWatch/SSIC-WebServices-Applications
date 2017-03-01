using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    ///  This class represents an areadataset.
    /// </summary>
    [DataContract]
    public class WebAreaDataset : WebData
    {
        /// <summary>
        /// AreaDataset id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }
    }
}
