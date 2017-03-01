using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    /// <summary>
    /// Contains extension methods for the ISpeciesFactSearchCriteriaExtension interface.
    /// </summary>
    public static class ISpeciesFactSearchCriteriaExtension
    {
        /// <summary>
        /// Ensures that no lists in <paramref name="speciesFactSearchCriteria"/> are null.
        /// </summary>
        /// <param name="speciesFactSearchCriteria">
        /// The species fact search criteria.
        /// </param>
        public static void EnsureNoListsAreNull(this ISpeciesFactSearchCriteria speciesFactSearchCriteria)
        {
            if (speciesFactSearchCriteria.FactorDataTypes == null)
            {
                speciesFactSearchCriteria.FactorDataTypes = new FactorDataTypeList();
            }

            if (speciesFactSearchCriteria.Factors == null)
            {
                speciesFactSearchCriteria.Factors = new FactorList();
            }

            if (speciesFactSearchCriteria.FieldSearchCriteria == null)
            {
                speciesFactSearchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();
            }

            if (speciesFactSearchCriteria.Hosts == null)
            {
                speciesFactSearchCriteria.Hosts = new TaxonList();
            }

            if (speciesFactSearchCriteria.IndividualCategories == null)
            {
                speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
            }

            if (speciesFactSearchCriteria.Periods == null)
            {
                speciesFactSearchCriteria.Periods = new PeriodList();
            }

            if (speciesFactSearchCriteria.Taxa == null)
            {
                speciesFactSearchCriteria.Taxa = new TaxonList();
            }
        }
    }
}
