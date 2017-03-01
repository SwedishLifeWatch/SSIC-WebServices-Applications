using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions
{
    /// <summary>
    /// Contains extension methods for the ISpeciesFactManager interface.
    /// </summary>
    public static class ISpeciesFactManagerExtension
    {
        /// <summary>
        /// Get information about species facts that correspond to all the combinations of parameters in the user parameter selection.
        /// In case data values exists in database values are provided otherwise values is set to defaults
        /// When user parameter selection is incomplete, i.e. some parameter lists is empty, the user parameter selection is complemented.
        /// This method is equivalent to SpeciesFactManager.GetSpeciesFacts(...) but adjusted for use in Dyntaxa Web Application.
        /// </summary>
        /// <param name="manager">The SpeciesFactManager.</param>
        /// <param name="userContext">
        /// Information about the user that makes this method call.
        /// </param>
        /// <param name="searchCriteria">Species fact search criteria.</param>
        /// <returns>Species facts that matches search criteria.</returns>
        public static SpeciesFactList GetDyntaxaSpeciesFacts(
            this ISpeciesFactManager manager, 
            IUserContext userContext,
            ISpeciesFactSearchCriteria searchCriteria)
        {
            SpeciesFactList speciesFactList = manager.GetSpeciesFacts(userContext, searchCriteria);
            if (speciesFactList.IsNull())
            {
                speciesFactList = new SpeciesFactList();
            }

            if (searchCriteria.Factors.IsNotEmpty())
            {
                ExpandSpeciesFactListWithEmptySpeciesFacts(userContext, searchCriteria, speciesFactList);
            }

            return speciesFactList;
        }

        /// <summary>
        /// Expands a Species Fact List with empty species facts so that every combination from the user parameter selection is represented.
        /// Factor Headers are excluded.
        /// Periodic factors are not expanded to individual categories other than the default. 
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="speciesFactSearchCriteria">
        /// Search criteria to be used as base for the species fact list. Needs to have factors and taxa. If it has no individual category, it will be given the default category.
        /// </param>
        /// <param name="speciesFacts">
        /// Species fact list to be expanded.
        /// </param>
        public static void ExpandSpeciesFactListWithEmptySpeciesFacts(IUserContext userContext, ISpeciesFactSearchCriteria speciesFactSearchCriteria, SpeciesFactList speciesFacts)
        {
            if (speciesFacts.IsNull())
            {
                speciesFacts = new SpeciesFactList();
            }

            // Add default host if necessary.
            if (speciesFactSearchCriteria.Hosts.IsEmpty())
            {
                if (speciesFactSearchCriteria.Hosts.IsNull())
                {
                    speciesFactSearchCriteria.Hosts = new TaxonList();
                }

                speciesFactSearchCriteria.Hosts.Add(CoreData.TaxonManager.GetTaxon(userContext, 0));
            }

            // Add default individual category if necessary.
            if (speciesFactSearchCriteria.IndividualCategories.IsEmpty())
            {
                if (speciesFactSearchCriteria.IndividualCategories.IsNull())
                {
                    speciesFactSearchCriteria.IndividualCategories = new IndividualCategoryList();
                }

                speciesFactSearchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetIndividualCategory(userContext, IndividualCategoryId.Default));
            }

            foreach (IFactor factor in speciesFactSearchCriteria.Factors)
            {                
                if (factor.UpdateMode.IsHeader)
                {
                    // Don't create SpeicesFacts for 'Headers'.
                    continue;
                }

                foreach (IIndividualCategory individualCategory in speciesFactSearchCriteria.IndividualCategories)
                {
                    if (factor.IsPeriodic && (individualCategory.Id != ((Int32)IndividualCategoryId.Default)))
                    {
                        // Periodic factors should only be combined
                        // with default IndividualCategory.
                        continue;
                    }

                    foreach (ITaxon taxon in speciesFactSearchCriteria.Taxa)
                    {
                        if (factor.IsPeriodic)
                        {
                            // Factor is periodic
                            foreach (IPeriod period in speciesFactSearchCriteria.Periods)
                            {
                                ExpandSpeciesFactListWithEmptySpeciesFact(userContext, taxon, individualCategory, factor, null, period, speciesFacts);

                                if (factor.IsTaxonomic)
                                {
                                    foreach (ITaxon host in speciesFactSearchCriteria.Hosts)
                                    {
                                        ExpandSpeciesFactListWithEmptySpeciesFact(userContext, taxon, individualCategory, factor, host, period, speciesFacts);
                                    }
                                }
                            }

                            // End factor is periodic
                        }
                        else
                        {
                            // Factor is not periodic
                            if (factor.IsTaxonomic)
                            {
                                foreach (ITaxon host in speciesFactSearchCriteria.Hosts)
                                {
                                    ExpandSpeciesFactListWithEmptySpeciesFact(userContext, taxon, individualCategory, factor, host, null, speciesFacts);
                                }
                            }
                            else
                            {
                                ExpandSpeciesFactListWithEmptySpeciesFact(userContext, taxon, individualCategory, factor, null, null, speciesFacts);
                            }

                            // End factor is not periodic
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Expands a Species Fact List with empty species facts so that every combination from the user parameter selection is represented.
        /// Factor Headers are excluded.
        /// Periodic factors are not expanded to individual categories other than the default. 
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">Taxon object of the species fact.</param>
        /// <param name="individualCategory">Individual category object of the species fact.</param>
        /// <param name="factor">Factor object of the species fact.</param>
        /// <param name="host">Host taxon object of the species fact.</param>
        /// <param name="period">Period object of the species fact.</param>
        /// <param name="speciesFacts">Species fact list to be expanded.</param>
        private static void ExpandSpeciesFactListWithEmptySpeciesFact(
            IUserContext userContext,
            ITaxon taxon,
            IIndividualCategory individualCategory,
            IFactor factor,
            ITaxon host,
            IPeriod period,
            SpeciesFactList speciesFacts)
        {            
            if (!speciesFacts.Exists(CoreData.SpeciesFactManager.GetSpeciesFactIdentifier(
                taxon,
                individualCategory,
                factor,
                host,
                period)))
            {                
                speciesFacts.Add(CoreData.SpeciesFactManager.GetSpeciesFact(
                    userContext, 
                    taxon,
                    individualCategory,
                    factor,
                    host,
                    period));
            }
        }
    }
}
