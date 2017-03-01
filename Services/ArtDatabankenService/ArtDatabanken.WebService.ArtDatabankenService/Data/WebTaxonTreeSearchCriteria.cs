using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class holds taxon tree filter information.
    /// </summary>
    [DataContract]
    public class WebTaxonTreeSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebTaxonTreeSearchCriteria instance.
        /// </summary>
        public WebTaxonTreeSearchCriteria()
        {
            RestrictSearchToTaxonIds = null;
            RestrictSearchToTaxonTypeIds = null;
            TaxonInformationType = TaxonInformationType.Basic;
        }

        /// <summary>
        /// 	Limit search to taxa.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToTaxonIds
        { get; set; }

        /// <summary>
        /// 	Limit search to taxon types.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToTaxonTypeIds
        { get; set; }

        /// <summary>Type of taxon information that is returned.</summary>
        [DataMember]
        public TaxonInformationType TaxonInformationType
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
        }
    }
}
