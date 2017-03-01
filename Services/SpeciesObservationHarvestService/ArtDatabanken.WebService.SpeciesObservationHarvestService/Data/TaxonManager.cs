using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using ArtDatabanken.WebService.Data;
using ArtDatabanken.WebService.SpeciesObservation.Data;

namespace ArtDatabanken.WebService.SpeciesObservationHarvestService.Data
{
    /// <summary>
    /// Manger of taxon related information.
    /// </summary>
    public static class TaxonManager
    {
        /// <summary>
        /// Get current taxon id.
        /// For taxa that is no longer accepted this is not the same as the old taxon id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="oldTaxonId">Old taxon id.</param>
        /// <returns>Current taxon id.</returns>
        public static Int32 GetCurrentTaxonId(WebServiceContext context, Int32 oldTaxonId)
        {
            Int32 currentTaxonId;

            try
            {
                List<WebLumpSplitEvent> webLumpSplitEvents = WebServiceData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(context, oldTaxonId);

                // No event, nothing happend.
                if (webLumpSplitEvents.Count == 0)
                {
                    currentTaxonId = oldTaxonId;
                }
                else
                {
                    // Split due to the fact that its more than 1 event
                    // if some problem its written in taxonRemark.
                    if (webLumpSplitEvents.Count > 1)
                    {
                        currentTaxonId = -2;
                    }
                    else
                    {
                        currentTaxonId = GetCurrentTaxonId(context, webLumpSplitEvents[0].TaxonIdAfter);
                    }
                }
            }
            catch (Exception exception)
            {
                WebServiceData.LogManager.Log(context, "Failed to retrieve current taxon for old taxon id = " + oldTaxonId, LogType.Error, exception.Message);
                throw;
            }

            return currentTaxonId;
        }

        /// <summary>
        /// Get new taxon id.
        /// For taxa that is no longer accepted this is not the same as the old taxon id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <param name="oldTaxonId">Old taxon id.</param>
        /// <returns>New taxon id.</returns>
        public static Int32 GetNewTaxonId(WebServiceContext context, Int32 oldTaxonId)
        {
            List<WebLumpSplitEvent> webLumpSplitEvents = WebServiceData.TaxonManager.GetLumpSplitEventsByOldReplacedTaxon(context, oldTaxonId);

            // no event, nothing happend
            if (webLumpSplitEvents.Count == 0)
            {
                return -1;
            }

            // Split due to the fact that its more than 1 event
            // if some problem its written in taxonRemark
            if (webLumpSplitEvents.Count > 1)
            {
                return -2;
            }

            return webLumpSplitEvents[0].TaxonIdAfter;
        }

        /// <summary>
        /// Get taxon name dictionaries.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon name dictionaries.</returns>
        public static TaxonNameDictionaries GetTaxonNameDictionaries(WebServiceContext context)
        {
            String cacheKey;
            Dictionary<Int32, TaxonInformation> taxonInformationCache;
            TaxonNameDictionaries taxonNameDictionaries;

            // Get cached information.
            cacheKey = Settings.Default.TaxonNameDictionariesCacheKey;
            taxonNameDictionaries = (TaxonNameDictionaries)(context.GetCachedObject(cacheKey));

            if (taxonNameDictionaries.IsNull())
            {
                taxonNameDictionaries = new TaxonNameDictionaries();
                taxonInformationCache = WebSpeciesObservationServiceData.TaxonManager.GetTaxonInformation(context);
                foreach (TaxonInformation taxonInformation in taxonInformationCache.Values)
                {
                    if (taxonInformation.Genus.IsNotEmpty())
                    {
                        taxonNameDictionaries.Genus.Add(taxonInformation.Genus.ToLower(), taxonInformation);
                    }

                    if (taxonInformation.ScientificName.IsNotEmpty())
                    {
                        taxonNameDictionaries.ScientificNames.Add(taxonInformation.ScientificName.ToLower(), taxonInformation);
                    }

                    if (taxonInformation.ScientificName.IsNotEmpty() && taxonInformation.ScientificNameAuthorship.IsNotEmpty())
                    {
                        taxonNameDictionaries.ScientificNameAndAuthor.Add(taxonInformation.ScientificName.ToLower() + " " + taxonInformation.ScientificNameAuthorship.ToLower(), taxonInformation);
                    }
                }

                // Add information to cache.
                context.AddCachedObject(cacheKey,
                                        taxonNameDictionaries,
                                        DateTime.Now + new TimeSpan(0, 12, 0, 0),
                                        CacheItemPriority.High);
            }

            return taxonNameDictionaries;
        }

        /// <summary>
        /// Get taxon remarks for recently changed taxa.
        /// Key in dictionary is taxon id.
        /// </summary>
        /// <param name="context">Web service request context.</param>
        /// <returns>Taxon remarks for recently changed taxa.</returns>
        public static Dictionary<Int32, String> GetTaxonRemarks(WebServiceContext context)
        {
            Dictionary<Int32, String> taxonRemarkDictionary = new Dictionary<Int32, String>();

            // Get date for latest update from database.
            DateTime latestTaxonUpdateDate = context.GetSpeciesObservationDatabase().GetLatestTaxonUpdateDate();

            // Get changes from web service related to update date.
            List<WebTaxonChange> webTaxonChangeList = WebServiceData.TaxonManager.GetTaxonChange(context, latestTaxonUpdateDate, DateTime.Now);
            List<WebTaxon> webTaxons = WebServiceData.TaxonManager.GetTaxaByIds(context, webTaxonChangeList.Select(webTaxonChange => webTaxonChange.TaxonId).Distinct().ToList());

            foreach (WebTaxon webTaxon in webTaxons)
            {
                // TODO: this test should not be necessary but there are currently problems with the data.
                // TODO:  2017-01-11.
                if (webTaxon.IsNotNull())
                {
                    String taxonRemark = WebServiceData.TaxonManager.GetTaxonConceptDefinition(context, webTaxon);
                    taxonRemarkDictionary.Add(webTaxon.Id, taxonRemark);
                }
            }

            return taxonRemarkDictionary;
        }
    }
}
