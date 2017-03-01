using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AreaDatasetCategory : WebData
    {
        /// <summary>
        /// Region id.
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