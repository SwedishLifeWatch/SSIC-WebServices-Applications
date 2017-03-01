using System;
using System.Collections.Generic;
using System.Security.Policy;

using ArtDatabanken;
using ArtDatabanken.Data;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.WebApplication.AnalysisPortal.ViewModels.RedList
{
    /// <summary>
    /// Cached information about red list taxon category.
    /// Factor with id 743.
    /// Possible values are
    ///  0 DD, Data Deficient.
    ///  1 RE, Regionally Extinct.
    ///  2 CR, Critically Endagered.
    ///  3 EN, Endagered.
    ///  4 VU, Vulnerable.
    ///  5 NT, Near Threatened.
    ///  6 LC, Least Concern.
    ///  7 NA, Not Applicable
    ///  8 NE, Not Evaluated
    /// </summary>    
    [Serializable]
    public class RedListCategoryCache
    {
        private readonly Dictionary<RedListCategory, TaxonIdList> mCategories;
        private readonly Dictionary<int, RedListCategory> mTaxonIdCategories;

        /// <summary>
        /// Create a RedListCategoryCache instance.
        /// </summary>    
        public RedListCategoryCache()
        {
            mCategories = new Dictionary<RedListCategory, TaxonIdList>();
            mTaxonIdCategories = new Dictionary<int, RedListCategory>();

            var categories = RedListedHelper.GetAllRedListCategories();
            foreach (var cat in categories)
            {
                this.mCategories[cat] = new TaxonIdList();
            }
        }

        /// <summary>
        /// Get factors for the species facts that are cached.
        /// </summary>    
        /// <param name="userContext">The user context.</param>
        /// <returns>Factors for the species facts that are cached.</returns>
        public static FactorList GetFactors(IUserContext userContext)
        {
            FactorList factors;

            factors = new FactorList
                          {
                              CoreData.FactorManager.GetFactor(userContext, FactorId.RedlistCategory)
                          };
            return factors;
        }

        /// <summary>
        /// Get ids for taxa that matches search criteria.
        /// </summary>
        /// <param name="searchCriteria">Red list category search criteria.</param>
        /// <param name="taxonIds">If not null, limit search to these taxa.</param>
        /// <returns>Ids for taxa that matches search criteria.</returns>
        public TaxonIdList GetTaxonIds(List<int> searchCriteria, TaxonIdList taxonIds)
        {
            int index;
            TaxonIdList categoryTaxonIds;

            if (searchCriteria.IsEmpty() ||
                (taxonIds.IsNotNull() && taxonIds.IsEmpty()))
            {
                // No taxa matches search critera and taxa limitations.
                return new TaxonIdList();
            }

            // Get all taxon ids that matches search criteria.
            categoryTaxonIds = this.mCategories[(RedListCategory)searchCriteria[0]].Clone();
            for (index = 1; index < searchCriteria.Count; index++)
            {
                categoryTaxonIds.AddRange(this.mCategories[(RedListCategory)searchCriteria[index]]);
            }

            if (taxonIds.IsEmpty())
            {
                taxonIds = categoryTaxonIds;
            }
            else
            {
                // Limit returned taxa.
                taxonIds.Subset(categoryTaxonIds);
            }

            return taxonIds;
        }

        /// <summary>
        /// Gets the taxon redlistcategory
        /// </summary>
        /// <param name="taxonId"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public bool GetRedListCategoryForTaxonId(int taxonId, out RedListCategory category)
        {
            if (mTaxonIdCategories.ContainsKey(taxonId))
            {
                category = mTaxonIdCategories[taxonId];
                return true;
            }

            category = RedListCategory.NA;
            return false;
        }

        /// <summary>
        /// Initiate the cache with species fact information.
        /// </summary>    
        /// <param name="userContext">The user context.</param>
        public void Init(IUserContext userContext)
        {
            FactorList factors;
            ISpeciesFactDataSource speciesFactDataSource;
            ISpeciesFactSearchCriteria searchCriteria;
            SpeciesFactList speciesFacts;

            speciesFactDataSource = new RedListSpeciesFactDataSource();
            searchCriteria = new SpeciesFactSearchCriteria();
            searchCriteria.IndividualCategories = new IndividualCategoryList();
            searchCriteria.IndividualCategories.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
            searchCriteria.Periods = new PeriodList();
            searchCriteria.Periods.Add(CoreData.FactorManager.GetCurrentRedListPeriod(userContext));
            factors = GetFactors(userContext);
            searchCriteria.Factors = new FactorList();
            searchCriteria.Factors.AddRange(factors);
            speciesFacts = speciesFactDataSource.GetSpeciesFacts(userContext, searchCriteria);
            foreach (ISpeciesFact speciesFact in speciesFacts)
            {
                if (speciesFact.IsRedlistCategorySpecified())
                {
                    if (speciesFact.Field1.EnumValue.KeyText.IsEmpty())
                    {
                        var ex = new Exception("Must be an error in the database for speciesFact.Field1.EnumValue.KeyText taxon =" + speciesFact.Taxon.Id);
                        throw ex;
                    }
                    if (speciesFact.Field1.EnumValue.KeyInt != null)
                    {
                        mCategories[(RedListCategory)speciesFact.Field1.EnumValue.KeyInt.Value].Add(new TaxonIdImplementation(speciesFact.Taxon.Id));
                    }
                }
            }

            foreach (var category in mCategories.Keys)
            {
                foreach (var taxon in mCategories[category])
                {
                    if (mTaxonIdCategories.ContainsKey(taxon.Id))
                    {
                        throw new Exception();
                    }

                    mTaxonIdCategories.Add(taxon.Id, category);
                }
            }
        }
    }
}
