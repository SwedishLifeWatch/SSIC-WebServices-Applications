using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Search criterias when taxon names are retrieved.
    /// </summary>
    [DataContract]
    public class WebTaxonNameSearchCriteria : WebData
    {
        /// <summary>
        /// Author search string.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria AuthorSearchString
        { get; set; }

        /// <summary>
        /// Search for taxon names with this taxon name category.
        /// </summary>
        [DataMember]
        public Int32 CategoryId
        { get; set; }

        /// <summary>
        /// Is author included in NameSearchString.
        /// </summary>
        [DataMember]
        public Boolean IsAuthorIncludedInNameSearchString  
        { get; set; }

        /// <summary>
        /// Indicates if property CategoryId has been set.
        /// </summary>
        [DataMember]
        public Boolean IsCategoryIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsOkForSpeciesObservation should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsOkForSpeciesObservationSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsOriginalName should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsOriginalNameSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsRecommended should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsRecommendedSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsUnique should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsUniqueSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsValidTaxon should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidTaxonSpecified
        { get; set; }

        /// <summary>
        /// Specifies if IsValidTaxonName should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidTaxonNameSpecified
        { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are ok 
        /// for species observation systems or not.
        /// </summary>
        [DataMember]
        public Boolean IsOkForSpeciesObservation
        { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are orginal name.
        /// </summary>
        [DataMember]
        public Boolean IsOriginalName
        { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are recommended or not.
        /// </summary>
        [DataMember]
        public Boolean IsRecommended
        { get; set; }

        /// <summary>
        /// Indicates if property StatusId has been set.
        /// </summary>
        [DataMember]
        public Boolean IsStatusIdSpecified
        { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are unique or not.
        /// </summary>
        [DataMember]
        public Boolean IsUnique
        { get; set; }

        /// <summary>
        /// Restrict search to taxon names for
        /// taxa with specified valid status.
        /// </summary>
        [DataMember]
        public Boolean IsValidTaxon
        { get; set; }

        /// <summary>
        /// Filter taxon names based on if they are valid or not.
        /// </summary>
        [DataMember]
        public Boolean IsValidTaxonName
        { get; set; }

        /// <summary>
        /// The name search string.
        /// </summary>
        [DataMember]
        public WebStringSearchCriteria NameSearchString
        { get; set; }

        /// <summary>
        /// Search for taxon names with this taxon name status.
        /// </summary>
        [DataMember]
        public Int32 StatusId
        { get; set; }

        /// <summary>
        /// Restrict search with this list of parent taxa.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }
    }
}
