using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// This class holds factor tree filter information.
    /// </summary>
    [DataContract]
    public class WebFactorTreeSearchCriteria : WebData
    {
        /// <summary>
        /// Create a WebFactorTreeSearchCriteria instance.
        /// </summary>
        public WebFactorTreeSearchCriteria()
        {
            RestrictSearchToFactorIds = null;
        }

        /// <summary>
        /// Limit search to factors.
        /// </summary>
        [DataMember]
        public List<Int32> RestrictSearchToFactorIds
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
