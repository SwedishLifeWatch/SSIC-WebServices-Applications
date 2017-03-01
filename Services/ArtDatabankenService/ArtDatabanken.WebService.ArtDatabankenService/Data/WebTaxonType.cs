using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Holds information about how user selected
    /// taxon types should be used.
    /// </summary>
    public enum UserSelectedTaxonTypeUsage
    {
        /// <summary>
        /// User selected taxa is used as input to stored procedure.
        /// </summary>
        Input,
        /// <summary>
        /// User selected taxa is used when the stored procedure
        /// generates its output.
        /// </summary>
        Output
    }

    /// <summary>
    /// This class represents a taxon type.
    /// In TaxonService this information has the name "taxon category".
    /// </summary>
    [DataContract]
    public class WebTaxonType : WebData
    {
        /// <summary>
        /// Id for this taxon type.
        /// </summary>
        [DataMember]
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name for this taxon type.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Sort order for this taxon type.
        /// </summary>
        [DataMember]
        public Int32 SortOrder
        { get; set; }
    }
}
