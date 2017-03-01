using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class holds parameters used to
    /// search for taxon revisions.
    /// </summary>
    [DataContract]
    public class WebTaxonRevisionSearchCriteria : WebData
    {
        /// <summary>
        /// Find revisions who have taxon revision state ids
        /// of specified values.
        /// </summary>
        [DataMember]
        public List<Int32> StateIds
        { get; set; }

        /// <summary>
        /// Find revisions who have taxon ids related
        /// to the specified taxon.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
        { get; set; }
    }
}
