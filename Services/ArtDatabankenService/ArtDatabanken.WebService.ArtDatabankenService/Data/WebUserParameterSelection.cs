using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class holds user selected parameter lists.
    /// </summary>
    [DataContract]
    public class WebUserParameterSelection : WebData
    {
        /// <summary>
        /// Create a WebUserParameterSelection instance.
        /// </summary>
        public WebUserParameterSelection()
        {
            FactorIds = null;
            TaxonIds = null;
            IndividualCategoryIds = null;
            HostIds = null;
            PeriodIds = null;
            ReferenceIds = null;
        }

        /// <summary>
        /// User selected factors.
        /// </summary>
        [DataMember]
        public List<Int32> FactorIds
        { get; set; }

        /// <summary>
        /// User selected hosts.
        /// </summary>
        [DataMember]
        public List<Int32> HostIds
        { get; set; }

        /// <summary>
        /// User selected individual categories.
        /// </summary>
        [DataMember]
        public List<Int32> IndividualCategoryIds
        { get; set; }

        /// <summary>
        /// User selected periods.
        /// </summary>
        [DataMember]
        public List<Int32> PeriodIds
        { get; set; }

        /// <summary>
        /// User selected references.
        /// </summary>
        [DataMember]
        public List<Int32> ReferenceIds
        { get; set; }

        /// <summary>
        /// User selected taxa.
        /// </summary>
        [DataMember]
        public List<Int32> TaxonIds
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
