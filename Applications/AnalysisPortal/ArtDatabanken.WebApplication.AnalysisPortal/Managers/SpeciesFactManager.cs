using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebApplication.AnalysisPortal.Managers
{
    /// <summary>
    /// Handle fact information for taxon.
    /// </summary>
    public class SpeciesFactManager
    {
        private readonly IUserContext _user;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeciesFactManager"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        public SpeciesFactManager(IUserContext user)
        {
            _user = user;
        }

        /// <summary>
        /// Get list of protected taxons. List is cached.
        /// </summary>
        /// <returns>Protected taxons.</returns>
        public IEnumerable<ITaxon> GetProtectedTaxons()
        {
            const string CachedProtectedTaxons = "protectedTaxons";

            IEnumerable<ITaxon> protectedTaxons = (IEnumerable<ITaxon>)AspNetCache.GetCachedObject(CachedProtectedTaxons);

            if (protectedTaxons.IsNull())
            {
                ISpeciesFactSearchCriteria searchCriteria = new SpeciesFactSearchCriteria();

                searchCriteria.IncludeNotValidHosts = false;
                searchCriteria.IncludeNotValidTaxa = false;
                searchCriteria.Factors = new FactorList();

                IFactor factor = CoreData.FactorManager.GetFactor(_user, FactorId.ProtectionLevel);

                searchCriteria.Factors.Add(factor);

                searchCriteria.FieldSearchCriteria = new SpeciesFactFieldSearchCriteriaList();

                ISpeciesFactFieldSearchCriteria fieldSearchCriteria = new SpeciesFactFieldSearchCriteria();
                fieldSearchCriteria.FactorField = factor.DataType.Field1;
                fieldSearchCriteria.Operator = CompareOperator.Greater;
                fieldSearchCriteria.AddValue(1);

                searchCriteria.FieldSearchCriteria.Add(fieldSearchCriteria);

                // Get species facts related to protected taxons
                SpeciesFactList speciesFactList = CoreData.SpeciesFactManager.GetSpeciesFacts(_user, searchCriteria);

                // Get distinct list of taxons from list of species facts
                protectedTaxons = speciesFactList.Select(sf => sf.Taxon).Distinct();

                AspNetCache.AddCachedObject(
                    CachedProtectedTaxons,
                    protectedTaxons,
                    DateTime.Now.AddHours(6),
                    System.Web.Caching.CacheItemPriority.Normal);
            }

            return protectedTaxons;
        }
    }
}
