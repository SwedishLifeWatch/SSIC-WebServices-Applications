using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.Caching;
using ArtDatabanken.Data;
using ArtDatabanken.Database;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SpeciesObservation.Data
{
    /// <summary>
    /// This class contains handling of taxon related objects.
    /// </summary>
    public class TaxonManager : ITaxonManager
    {
        /// <summary>
        /// The taxon related information.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon related information.</returns>
        public Dictionary<Int32, TaxonInformation> GetTaxonInformation(WebServiceContext context)
        {
            String cacheKey;
            Dictionary<Int32, TaxonInformation> taxonInformationCache;
            TaxonInformation taxonInformation;

            // Get cached information.
            cacheKey = Settings.Default.TaxonInformationCacheKey + ":" + context.Locale.ISOCode;
            taxonInformationCache = (Dictionary<Int32, TaxonInformation>)(context.GetCachedObject(cacheKey));

            if (taxonInformationCache.IsEmpty())
            {
                // Data not in cache. Get information from database.
                taxonInformationCache = new Dictionary<Int32, TaxonInformation>();
                using (DataReader dataReader = context.GetSpeciesObservationDatabase().GetTaxonInformation())
                {
                    while (dataReader.Read())
                    {
                        taxonInformation = new TaxonInformation();
                        taxonInformation.LoadData(dataReader);
                        taxonInformationCache.Add(taxonInformation.DyntaxaTaxonId, taxonInformation);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonInformationCache,
                                        DateTime.Now + new TimeSpan(0, 12, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonInformationCache;
        }

        /// <summary>
        /// Get ids for requested taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">The species observation search criteria.</param>
        /// <param name="includeChildTaxa">Include child taxa in returned taxa.</param>
        /// <returns>Ids for requested taxa.</returns>
        public List<Int32> GetTaxonIds(WebServiceContext context,
                                               WebSpeciesObservationSearchCriteria searchCriteria,
                                               Boolean includeChildTaxa)
        {
            List<Int32> redlistedTaxonIds, taxonIds;

            taxonIds = searchCriteria.TaxonIds;
            if (searchCriteria.IncludeRedlistedTaxa ||
                searchCriteria.IncludeRedListCategories.IsNotEmpty())
            {
                // Get redlisted taxon ids.
                redlistedTaxonIds = GetRedlistedTaxonIds(context,
                                                         searchCriteria.IncludeRedlistedTaxa,
                                                         searchCriteria.IncludeRedListCategories);
                if (redlistedTaxonIds.IsNotEmpty())
                {
                    if (taxonIds.IsEmpty())
                    {
                        taxonIds = redlistedTaxonIds;
                    }
                    else
                    {
                        foreach (Int32 taxonId in redlistedTaxonIds)
                        {
                            if (!taxonIds.Contains(taxonId))
                            {
                                taxonIds.Add(taxonId);
                            }
                        }
                    }
                }
            }

            if (includeChildTaxa)
            {
                taxonIds = WebServiceData.TaxonManager.GetChildTaxonIds(context, taxonIds);
            }

            return taxonIds;
        }

        /// <summary>
        /// Get taxon ids for red listed taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="includeRedlistedTaxa">If true all red listed taxa should be returned.</param>
        /// <param name="redlistCategories">Taxa belonging to specified red list categories should be returned.</param>
        /// <returns>Requested red listed taxa.</returns>
        public List<Int32> GetRedlistedTaxonIds(WebServiceContext context,
                                                        Boolean includeRedlistedTaxa,
                                                        List<RedListCategory> redlistCategories)
        {
            Hashtable redlistedTaxaInformation;
            List<Int32> redlistedTaxonIds;

            // Get cached information.
            redlistedTaxaInformation = GetRedlistedTaxa(context);

            if (includeRedlistedTaxa)
            {
                redlistedTaxonIds = (List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey()]);
            }
            else
            {
                redlistedTaxonIds = new List<Int32>();
                if (redlistCategories.IsNotEmpty())
                {
                    foreach (RedListCategory redListCategory in redlistCategories)
                    {
                        redlistedTaxonIds.AddRange((List<Int32>)(redlistedTaxaInformation[GetRedlistedTaxaCacheKey(redListCategory)]));
                    }
                }
            }

            return redlistedTaxonIds;
        }

        /// <summary>
        /// Get information about red listed taxa.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Information about red listed taxa.</returns>
        private static Hashtable GetRedlistedTaxa(WebServiceContext context)
        {
            Dictionary<RedListCategory, List<Int32>> redlistedTaxonIds;
            Hashtable redlistedTaxaInformation;
            List<Int32> allRedlistedTaxonIds;
            RedListCategory redListCategory;
            Dictionary<Int32, TaxonInformation> taxonInformationCache;

            // Get data from cache.
            redlistedTaxaInformation = (Hashtable)context.GetCachedObject(GetRedlistedTaxaCacheKey());

            if (redlistedTaxaInformation.IsNull())
            {
                // Data not in cache - store it in the cache.
                // Init data structures.
                allRedlistedTaxonIds = new List<Int32>();
                redlistedTaxaInformation = new Hashtable();
                redlistedTaxonIds = new Dictionary<RedListCategory, List<Int32>>();
                for (redListCategory = RedListCategory.DD; redListCategory <= RedListCategory.NT; redListCategory++)
                {
                    redlistedTaxonIds[redListCategory] = new List<Int32>();
                }

                // Extract red list information from taxon information.
                taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
                foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
                {
                    if (taxonInformation.RedlistCategory.IsNotEmpty())
                    {
                        allRedlistedTaxonIds.Add(taxonInformation.DyntaxaTaxonId);
                        switch (taxonInformation.RedlistCategory.Substring(0, 2).ToUpper())
                        {
                            case "CR":
                                redlistedTaxonIds[RedListCategory.CR].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                            case "DD":
                                redlistedTaxonIds[RedListCategory.DD].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                            case "EN":
                                redlistedTaxonIds[RedListCategory.EN].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                            case "NT":
                                redlistedTaxonIds[RedListCategory.NT].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                            case "RE":
                                redlistedTaxonIds[RedListCategory.RE].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                            case "VU":
                                redlistedTaxonIds[RedListCategory.VU].Add(taxonInformation.DyntaxaTaxonId);
                                break;
                        }
                    }
                }

                // Save red list information into hashtable.
                redlistedTaxaInformation[GetRedlistedTaxaCacheKey()] = allRedlistedTaxonIds;
                foreach (RedListCategory tempRedListCategory in redlistedTaxonIds.Keys)
                {
                    redlistedTaxaInformation[GetRedlistedTaxaCacheKey(tempRedListCategory)] = redlistedTaxonIds[tempRedListCategory];
                }

                // Store data in cache.
                context.AddCachedObject(GetRedlistedTaxaCacheKey(),
                                        redlistedTaxaInformation,
                                        DateTime.Now + new TimeSpan(1, 0, 0, 0),
                                        CacheItemPriority.High);
            }

            return redlistedTaxaInformation;
        }

        /// <summary>
        /// Get cache key for all red listed taxa.
        /// </summary>
        /// <returns>Cache key for all red listed taxa.</returns>
        private static String GetRedlistedTaxaCacheKey()
        {
            return Settings.Default.RedlistedTaxaCacheKey;
        }

        /// <summary>
        /// Get cache key for taxa that is red listed
        /// in specified red list category.
        /// </summary>
        /// <param name="redlistCategory">Cache key for taxa belonging to specified red list category should be returned.</param>
        /// <returns>Cache key for red listed taxa.</returns>
        private static String GetRedlistedTaxaCacheKey(RedListCategory redlistCategory)
        {
            return Settings.Default.RedlistedTaxaCacheKey +
                   WebService.Settings.Default.CacheKeyDelimiter +
                   redlistCategory;
        }


    }
}
