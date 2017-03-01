using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds parameters used to search for taxa.
    /// </summary>
    [DataContract]
    public class WebTaxonSearchCriteria : WebData
    {
        /// <summary>
        /// Specifies if IsValidTaxon should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidTaxonSpecified
        { get; set; }

        /// <summary>
        /// Restrict search to valid or not valid taxa.
        /// Property IsIsValidTaxonSpecified must be set to true
        /// if property IsValidTaxon should be used in the search.
        /// </summary>
        [DataMember]
        public Boolean IsValidTaxon
        { get; set; }

        /// <summary>
        /// Taxon search scope.
        /// May be used to include parent taxa or child taxa in search.
        /// </summary>
        [DataMember]
        public TaxonSearchScope Scope
        { get; set; }

        /// <summary>
        /// Find taxa who have "Swedish immigration history" value
        /// set to one on the integer values in this list.
        /// </summary>
        [DataMember]
        public List<Int32> SwedishImmigrationHistory
        { get; set; }

        /// <summary>
        /// Find taxa who have "Swedish occurrence" value
        /// set to one on the integer values in this list.
        /// </summary>
        [DataMember]
        public List<Int32> SwedishOccurrence
        { get; set; }

        /// <summary>
        /// Find taxa related to taxa with specified taxon categories.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonCategoryIds
        { get; set; }

        /// <summary>
        /// Find taxa related to taxa with specified ids.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }

        /// <summary>
        /// Find taxa that matches taxon name search criteria.
        /// Compare operators in WebStringSearchCriteria are 
        /// currently ignored. Compare operator 'Like' is always used.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria TaxonNameSearchString
        { get; set; }
    }
}
