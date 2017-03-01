using System;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.GeoReferenceService.Data
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AreaDataset : WebData
    {
        /// <summary>
        /// Region id.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

               /// <summary>
        /// CountryIsoCode.
        /// </summary>
        [DataMember]
        public Int32 CountryIsoCode
        { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// IsRequired
        /// </summary>
        [DataMember]
        public Boolean IsRequired
        { get; set; }

        /// <summary>
        /// IsIndirect
        /// </summary>
        [DataMember]
        public Boolean IsIndirect
        { get; set; }

        /// <summary>
        /// AllowOverrideIndirectType
        /// </summary>
        [DataMember]
        public Boolean AllowOverrideIndirectType
        { get; set; }

        /// <summary>
        /// AreaDatasetCategoryId
        /// </summary>
        [DataMember]
        public Int32 AreaDatasetCategoryId
        { get; set; }

        /// <summary>
        /// SortOrder
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; } 

        /// <summary>
        /// AttribuitesToHtmlXslt
        /// </summary>
        [DataMember]
        public String AttributesToHtmlXslt
        { get; set; }

        /// <summary>
        /// AreaLevel
        /// </summary>
        [DataMember]
        public Int32 AreaLevel
        { get; set; }

        /// <summary>
        /// AllowOverrideIndirectType
        /// </summary>
        [DataMember]
        public Boolean HasStatistics
        { get; set; }
        
        /// <summary>
        /// AllowOverrideIndirectType
        /// </summary>
        [DataMember]
        public Boolean IsValidationArea
        { get; set; }
    }
}