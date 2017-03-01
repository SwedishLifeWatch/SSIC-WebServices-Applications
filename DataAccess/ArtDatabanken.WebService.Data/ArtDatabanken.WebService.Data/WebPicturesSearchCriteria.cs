using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class represents a pictures search criteria.
    /// </summary>
    [DataContract]
    public class WebPicturesSearchCriteria : WebData
    {
        /// <summary>
        /// Search for pictures that belong to specified factors.
        /// </summary>
        [DataMember]
        public List<Int32> FactorIds { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified taxa.
        /// Taxon ids according to Dyntaxa.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds { get; set; }

        /// <summary>
        /// Search for pictures that belong to specified species fact identifiers.
        /// </summary>
        [DataMember]
        public List<String> SpeciesFactIdentifiers { get; set; }

            /// <summary>
        /// Search for pictures that contains one or more of the metadata in the list.
        /// </summary>
        [DataMember]
        public List<WebPictureMetaData> MetaData { get; set; }
    }
}