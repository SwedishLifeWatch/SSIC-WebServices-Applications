using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ArtDatabanken.Data;
using ArtDatabanken.WebApplication.Dyntaxa.Data.Extensions;
using Resources;

namespace ArtDatabanken.WebApplication.Dyntaxa.Data
{
    using System.Collections;

    /// <summary>
    /// Helper functions used to get information from ArtFakta.
    /// </summary>
    public static class SpeciesFactHelper
    {
        /// <summary>
        /// Initializes static members of the <see cref="SpeciesFactHelper"/> class.
        /// </summary>
        static SpeciesFactHelper()
        {
            CommonSpeciesFacts = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Species facts cache for common facts used by Dyntaxa.
        /// </summary>
        private static Hashtable CommonSpeciesFacts { get;  set; }

        /// <summary>
        /// The get factors value lists.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="factorIds">
        /// The factor ids.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<FactorId, IList<FactorFieldEnumValue>> GetFactorsValueLists(IUserContext userContext, IEnumerable<FactorId> factorIds)
        {
            var dic = new Dictionary<FactorId, IList<FactorFieldEnumValue>>();
            var intFactorIds = new List<int>(factorIds.Select(factorId => (int)factorId));
            FactorList factors = CoreData.FactorManager.GetFactors(userContext, intFactorIds);
            foreach (IFactor factor in factors)
            {
                dic.Add((FactorId)factor.Id, factor.DataType.MainField.Enum.Values.Cast<FactorFieldEnumValue>().ToList());
            }

            return dic;
        }

        private static bool IsInRevision()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null)
            {
                int? isInRevision = HttpContext.Current.Session["RevisionId"] as int?;
                return isInRevision.HasValue;
            }

            return false;            
        }

        /// <summary>
        /// Gets the common dyntaxa species facts. (SwedishOccurence, SwedishHistory, RedlistCategory)
        /// Get facts from cache (CommonSpeciesFacts) if they exists.
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<FactorId, SpeciesFact> GetCommonDyntaxaSpeciesFacts(
            IUserContext userContext, 
            ITaxon taxon)
        {
            SpeciesFactList speciesFactList;
            IEnumerable<FactorId> factorIds = new[] { FactorId.SwedishOccurrence, FactorId.SwedishHistory, FactorId.RedlistCategory };
            var speciesFactDictionary = GetCommonDyntaxaSpeciesFactsFromCache(taxon.Id);                      
            //if (!IsInRevision() && speciesFactDictionary.IsNotNull())
            if (speciesFactDictionary.IsNotNull())
            {                
                return speciesFactDictionary;
            }
            else
            {
                Dictionary<FactorId, SpeciesFact> dic = new Dictionary<FactorId, SpeciesFact>();
                dic = GetSpeciesFacts(userContext, taxon, factorIds, out speciesFactList);
                
                // Add species facts to cache.
                AddCommonDyntaxaSpeciesFactsToCache(taxon.Id, dic);                
                return dic;
            }
        }

        /// <summary>
        /// The get species facts.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="factorIds">
        /// The factor ids.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<FactorId, SpeciesFact> GetSpeciesFacts(
            IUserContext userContext, 
            ITaxon taxon, 
            IEnumerable<FactorId> factorIds)
        {
            SpeciesFactList speciesFactList;
            return GetSpeciesFacts(userContext, taxon, factorIds, out speciesFactList);
        }

        /// <summary>
        /// The get species facts.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxon">
        /// The taxon.
        /// </param>
        /// <param name="factorIds">
        /// The factor ids.
        /// </param>
        /// <param name="speciesFactList">
        /// The species fact list.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<FactorId, SpeciesFact> GetSpeciesFacts(
            IUserContext userContext, 
            ITaxon taxon, 
            IEnumerable<FactorId> factorIds, 
            out SpeciesFactList speciesFactList)
        {            
            var dic = GetSpeciesFacts(userContext, new[] { taxon }, factorIds, out speciesFactList);
            if (dic.Values.Count == 1)
            {
                return dic.Values.First();
            }

            return new Dictionary<FactorId, SpeciesFact>();
        }

        /// <summary>
        /// The get species facts.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxa">
        /// The taxa.
        /// </param>
        /// <param name="factorIds">
        /// The factor ids.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<int, Dictionary<FactorId, SpeciesFact>> GetSpeciesFacts(
            IUserContext userContext, 
            IEnumerable<ITaxon> taxa, 
            IEnumerable<FactorId> factorIds)
        {
            SpeciesFactList speciesFactList;
            return GetSpeciesFacts(userContext, taxa, factorIds, out speciesFactList);
        }

        /// <summary>
        /// The get species facts.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <param name="taxa">
        /// The taxa.
        /// </param>
        /// <param name="factorIds">
        /// The factor ids.
        /// </param>
        /// <param name="speciesFactList">
        /// The species fact list.
        /// </param>
        /// <returns>
        /// The <see cref="Dictionary"/>.
        /// </returns>
        public static Dictionary<int, Dictionary<FactorId, SpeciesFact>> GetSpeciesFacts(
            IUserContext userContext, 
            IEnumerable<ITaxon> taxa, 
            IEnumerable<FactorId> factorIds, 
            out SpeciesFactList speciesFactList)
        {
            var intFactorIds = new List<int>(factorIds.Select(factorId => (int)factorId));
            FactorList factors = CoreData.FactorManager.GetFactors(userContext, intFactorIds);
            ISpeciesFactSearchCriteria speciesFactSearchCriteria = new SpeciesFactSearchCriteria();
            IPeriod period = CoreData.FactorManager.GetCurrentPublicPeriod(userContext);
            speciesFactSearchCriteria.EnsureNoListsAreNull();
            speciesFactSearchCriteria.IncludeNotValidHosts = true;
            speciesFactSearchCriteria.IncludeNotValidTaxa = true;
            speciesFactSearchCriteria.Add(period);
            speciesFactSearchCriteria.Add(CoreData.FactorManager.GetDefaultIndividualCategory(userContext));
            speciesFactSearchCriteria.Taxa = new TaxonList();            

            foreach (ITaxon taxon in taxa)
            {
                speciesFactSearchCriteria.Taxa.Add(taxon);                
            }

            speciesFactSearchCriteria.Factors = new FactorList();
            foreach (IFactor factor in factors)
            {
                speciesFactSearchCriteria.Factors.Add(factor);
            }

            speciesFactList = CoreData.SpeciesFactManager.GetDyntaxaSpeciesFacts(userContext, speciesFactSearchCriteria);
            var dic = new Dictionary<int, Dictionary<FactorId, SpeciesFact>>();
            foreach (SpeciesFact speciesFact in speciesFactList)
            {
                var factorId = (FactorId)speciesFact.Factor.Id;

                if (!dic.ContainsKey(speciesFact.Taxon.Id))
                {
                    dic.Add(speciesFact.Taxon.Id, new Dictionary<FactorId, SpeciesFact>());
                }

                if (!dic[speciesFact.Taxon.Id].ContainsKey(factorId))
                {
                    dic[speciesFact.Taxon.Id].Add(factorId, speciesFact);
                }
            }

            return dic;
        }

        /// <summary>
        /// Commit all changes in the DB.
        /// </summary>
        /// <param name="userContext">
        /// The user Context.
        /// </param>
        /// <param name="speciesFacts">
        /// The species Facts.
        /// </param>
        public static void UpdateDyntaxaSpeciesFacts(UserContext userContext, SpeciesFactList speciesFacts)
        {
            IReference referenceValue = GetDyntaxaReference(userContext);
            using (ITransaction transaction = userContext.StartTransaction(30))
            {
                CoreData.SpeciesFactManager.UpdateSpeciesFacts(userContext, speciesFacts, referenceValue);
                transaction.Commit();
            }
        }

        /// <summary>
        /// The get dyntaxa reference.
        /// </summary>
        /// <param name="userContext">
        /// The user context.
        /// </param>
        /// <returns>
        /// The <see cref="IReference"/>.
        /// </returns>
        private static IReference GetDyntaxaReference(IUserContext userContext)
        {
#if DEBUG
            int dyntaxaReferenceId = DyntaxaSettings.Default.DyntaxaDefaultReferenceIdMoneses;
#else
            int dyntaxaReferenceId = Resources.DyntaxaSettings.Default.DyntaxaDefaultReferenceId;        
#endif
            return CoreData.ReferenceManager.GetReference(userContext, dyntaxaReferenceId);
        }

        /// <summary>
        /// The get factor value.
        /// </summary>
        /// <param name="dicFactors">
        /// The dic factors.
        /// </param>
        /// <param name="factorId">
        /// The factor id.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetFactorValue(Dictionary<FactorId, SpeciesFact> dicFactors, FactorId factorId)
        {
            SpeciesFact speciesFact = null;
            dicFactors.TryGetValue(factorId, out speciesFact);
            if (speciesFact != null)
            {
                if (speciesFact.MainField != null)
                {
                    var factorField = speciesFact.MainField.Value as FactorFieldEnumValue;
                    if (factorField != null)
                    {
                        return factorField.OriginalLabel;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// The get factor value.
        /// </summary>
        /// <param name="speciesFact">
        /// The species Fact.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetFactorValue(SpeciesFact speciesFact)
        {                        
            if (speciesFact != null)
            {
                if (speciesFact.MainField != null)
                {
                    var factorField = speciesFact.MainField.Value as FactorFieldEnumValue;
                    if (factorField != null)
                    {
                        return factorField.OriginalLabel;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Get common species facts from cache.
        /// </summary>
        /// <param name="taxonId">
        /// Taxon id.
        /// </param>
        /// <returns>
        /// Taxon information for specified locale.
        /// </returns>
        private static Dictionary<FactorId, SpeciesFact> GetCommonDyntaxaSpeciesFactsFromCache(Int32 taxonId)
        {
            String cacheKey;
            cacheKey = GetTaxonCacheKey(taxonId);
            if (CommonSpeciesFacts.ContainsKey(cacheKey))
            {
                return (Dictionary<FactorId, SpeciesFact>)CommonSpeciesFacts[cacheKey];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// The add common dyntaxa species facts to cache.
        /// </summary>
        /// <param name="taxonId">
        /// The taxon id.
        /// </param>
        /// <param name="speciesFacts">
        /// The species facts.
        /// </param>
        private static void AddCommonDyntaxaSpeciesFactsToCache(Int32 taxonId, Dictionary<FactorId, SpeciesFact> speciesFacts)
        {
            CommonSpeciesFacts[GetTaxonCacheKey(taxonId)] = speciesFacts;
        }

        /// <summary>
        /// Get cache key for taxon.
        /// </summary>
        /// <param name="taxonId">
        /// Taxon id.
        /// </param>
        /// <returns>
        /// Cache key for taxon.
        /// </returns>
        private static String GetTaxonCacheKey(Int32 taxonId)
        {
            return taxonId.WebToString();
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">
        /// User context.
        /// </param>
        public static void RefreshCache(IUserContext userContext)
        {
            if (CommonSpeciesFacts != null)
            {
                CommonSpeciesFacts.Clear();                
            }
        }
    }
}
