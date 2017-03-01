using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ArtDatabanken.WebService.ArtDatabankenService;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Possible scope types when searching for taxa.
    /// </summary>
    [DataContract]
    public enum TaxonSearchScope
    {
        /// <summary>
        /// No special handling.
        /// </summary>
        [EnumMember]
        NoScope,
        /// <summary>
        /// Include all child taxa.
        /// </summary>
        [EnumMember]
        AllChildTaxa,
        /// <summary>
        /// Include all parent taxa.
        /// </summary>
        [EnumMember]
        AllParentTaxa
    }

    /// <summary>
    /// This class holds taxa filter information.
    /// </summary>
    [DataContract]
    public class WebTaxonSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebTaxonSearchCriteria instance.
        /// </summary>
        public WebTaxonSearchCriteria()
        {
            RestrictReturnToScope = TaxonSearchScope.NoScope;
            RestrictReturnToTaxonTypeIds = null;
            RestrictReturnToSwedishSpecies = false;
            RestrictSearchToSwedishSpecies = false;
            RestrictSearchToTaxonIds = null;
            RestrictSearchToTaxonTypeIds = null;
            TaxonInformationType = TaxonInformationType.Basic;
            TaxonNameSearchString = null;
        }

        /// <summary>Scope for taxa that is returned.</summary>
        [DataMember]
        public TaxonSearchScope RestrictReturnToScope
        { get; set; }

        /// <summary>
        /// 	<c>true</c> if [exclude none swedish species]; otherwise, <c>false</c>.
        /// </summary>
        [DataMember]
        public Boolean RestrictReturnToSwedishSpecies
        { get; set; }

        /// <summary>The types of taxon that is to be returned.</summary>
        [DataMember]
        public List<Int32> RestrictReturnToTaxonTypeIds
        { get; set; }

        /// <summary>
        /// 	<c>true</c> if [exclude none swedish species]; otherwise, <c>false</c>.
        /// </summary>
        [DataMember]
        public Boolean RestrictSearchToSwedishSpecies
        { get; set; }

        /// <summary>
        /// 	Limit taxon search (not taxon return) to taxa.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToTaxonIds
        { get; set; }

        /// <summary>The types of taxon that is to be searched.</summary>
        [DataMember]
        public List<Int32> RestrictSearchToTaxonTypeIds
        { get; set; }

        /// <summary>Type of taxon information to return.</summary>
        [DataMember]
        public TaxonInformationType TaxonInformationType
        { get; set; }

        /// <summary>The taxon name search string.</summary>
        [DataMember]
        public String TaxonNameSearchString
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            TaxonNameSearchString = TaxonNameSearchString.CheckSqlInjection();
        }
    }
}
