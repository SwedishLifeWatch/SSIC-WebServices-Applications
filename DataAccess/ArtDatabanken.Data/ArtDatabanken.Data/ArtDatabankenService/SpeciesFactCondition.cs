using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// Contains conditions on species facts that are selected
    /// in a data query.
    /// </summary>
    public class SpeciesFactCondition : DataQuery
    {
        /// <summary>
        /// Create a SpeciesFactCondition instance.
        /// </summary>
        public SpeciesFactCondition()
            : base(DataQueryType.SpeciesFactCondition)
        {
            Factors = new FactorList();
            Periods = new PeriodList();
            IndividualCategories = new IndividualCategoryList();
            SpeciesFactFieldConditions = new List<SpeciesFactFieldCondition>();
        }

        /// <summary>
        /// Limit condition to specified factors.
        /// </summary>
        public FactorList Factors
        { get; set; }

        /// <summary>
        /// Limit condition to specified periods.
        /// </summary>
        public PeriodList Periods
        { get; set; }

        /// <summary>
        /// Limit condition to specified individual categories.
        /// </summary>
        public IndividualCategoryList IndividualCategories
        { get; set; }

        /// <summary>
        /// Condition on the specified species fact fields.
        /// </summary>
        public List<SpeciesFactFieldCondition> SpeciesFactFieldConditions
        { get; set; }
    }
}
