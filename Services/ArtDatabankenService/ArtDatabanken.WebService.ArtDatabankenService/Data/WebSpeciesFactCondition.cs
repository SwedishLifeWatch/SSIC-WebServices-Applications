using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.ArtDatabankenService.Data
{
    /// <summary>
    /// Contains a condition on species facts that are returned.
    /// This class is used in WebDataQuery handling.
    /// </summary>
    [DataContract]
    public class WebSpeciesFactCondition : WebData
    {
        /// <summary>
        /// Create a WebSpeciesFactCondition instance.
        /// </summary>
        public WebSpeciesFactCondition()
        {
            Factors = null;
            HostIds = null;
            IndividualCategories = null;
            SpeciesFactFieldConditions = null;
            Periods = null;
            TaxonIds = null;
        }

        /// <summary>
        /// Limit condition to specified factors.
        /// </summary>
        [DataMember]
        public WebFactor[] Factors
        { get; set; }

        /// <summary>
        /// Limit condition to specified hosts.
        /// </summary>
        [DataMember]
        public Int32[] HostIds
        { get; set; }

        /// <summary>
        /// Limit condition to specified individual categories.
        /// </summary>
        [DataMember]
        public WebIndividualCategory[] IndividualCategories
        { get; set; }

        /// <summary>
        /// Limit condition to specified periods.
        /// </summary>
        [DataMember]
        public WebPeriod[] Periods
        { get; set; }

        /// <summary>
        /// Condition on the specified fact fields.
        /// </summary>
        [DataMember]
        public WebSpeciesFactFieldCondition[] SpeciesFactFieldConditions
        { get; set; }

        /// <summary>
        /// Limit condition to specified taxa.
        /// </summary>
        [DataMember]
        public Int32[] TaxonIds
        { get; set; }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public override void CheckData()
        {
            base.CheckData();
            if (Factors.IsNotEmpty())
            {
                foreach (WebFactor factor in Factors)
                {
                    factor.CheckData();
                }
            }
            if (IndividualCategories.IsNotEmpty())
            {
                foreach (WebIndividualCategory individualCategory in IndividualCategories)
                {
                    individualCategory.CheckData();
                }
            }
            if (SpeciesFactFieldConditions.IsNotEmpty())
            {
                foreach (WebSpeciesFactFieldCondition speciesFactFieldCondition in SpeciesFactFieldConditions)
                {
                    speciesFactFieldCondition.CheckData();
                }
            }
            if (Periods.IsNotEmpty())
            {
                foreach (WebPeriod period in Periods)
                {
                    period.CheckData();
                }
            }
        }
    }
}
