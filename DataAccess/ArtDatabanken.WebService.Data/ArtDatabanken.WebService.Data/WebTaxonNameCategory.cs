using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Category of taxon name. Scientific, Swedish etc.
    /// </summary>
    [DataContract]
    public class WebTaxonNameCategory : WebData
    {
        /// <summary>
        /// Id for taxon name category.
        /// </summary>
        [DataMember]
        public Int32 Id { get; set; }

        /// <summary>
        /// Indicates if property "LocaleId" has a value or not.
        /// </summary>
        [DataMember]
        public Boolean IsLocaleIdSpecified { get; set; }

        /// <summary>
        /// Locale id is used together with Type = CommonName.
        /// </summary>
        [DataMember]
        public Int32 LocaleId { get; set; }

        /// <summary>
        /// Name of taxon name category.
        /// </summary>
        [DataMember]
        public String Name { get; set; }

        /// <summary>
        /// Short verison of the name for the taxon name category.
        /// </summary>
        [DataMember]
        public String ShortName { get; set; }

        /// <summary>
        /// Sort order for the taxon name category.
        /// </summary>
        [DataMember]
        public Int32 SortOrder { get; set; }

        /// <summary>
        /// Id for the taxon name category type.
        /// </summary>
        [DataMember]
        public Int32 TypeId { get; set; }
    }
}
