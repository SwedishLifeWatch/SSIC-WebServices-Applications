using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of user related information.
    /// </summary>
    public class TaxonManagerMultiThreadCache : TaxonManagerSingleThreadCache
    {
        /// <summary>
        /// Get lump split event types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Lump split event types for specified locale.</returns>
        protected override LumpSplitEventTypeList GetLumpSplitEventTypes(ILocale locale)
        {
            LumpSplitEventTypeList lumpSplitEventTypes = null;

            lock (LumpSplitEventTypes)
            {
                if (LumpSplitEventTypes.ContainsKey(locale.ISOCode))
                {
                    lumpSplitEventTypes = (LumpSplitEventTypeList)(LumpSplitEventTypes[locale.ISOCode]);
                }
            }
            return lumpSplitEventTypes;
        }

        /// <summary>
        /// Get taxon information for specified locale.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon information for specified locale.</returns>
        protected override ITaxon GetTaxon(Int32 taxonId, ILocale locale)
        {
            String cacheKey;
            ITaxon taxon = null;

            lock (Taxa)
            {
                cacheKey = GetTaxonCacheKey(taxonId, locale);
                if (Taxa.ContainsKey(cacheKey))
                {
                    taxon = (ITaxon)(Taxa[cacheKey]);
                }
            }
            return taxon;
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All taxon alert statuses.</returns>
        protected override TaxonAlertStatusList GetTaxonAlertStatuses(ILocale locale)
        {
            TaxonAlertStatusList taxonAlertStatuses = null;

            lock (TaxonAlertStatuses)
            {
                if (TaxonAlertStatuses.ContainsKey(locale.ISOCode))
                {
                    taxonAlertStatuses = (TaxonAlertStatusList)(TaxonAlertStatuses[locale.ISOCode]);
                }
            }
            return taxonAlertStatuses;
        }

        /// <summary>
        /// Get taxon categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon categories for specified locale.</returns>
        protected override TaxonCategoryList GetTaxonCategories(ILocale locale)
        {
            TaxonCategoryList taxonCategories = null;

            lock (TaxonCategories)
            {
                if (TaxonCategories.ContainsKey(locale.ISOCode))
                {
                    taxonCategories = (TaxonCategoryList)(TaxonCategories[locale.ISOCode]);
                }
            }
            return taxonCategories;
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>All taxon change statuses.</returns>
        protected override TaxonChangeStatusList GetTaxonChangeStatuses(ILocale locale)
        {
            TaxonChangeStatusList taxonChangeStatuses = null;

            lock (TaxonChangeStatuses)
            {
                if (TaxonChangeStatuses.ContainsKey(locale.ISOCode))
                {
                    taxonChangeStatuses = (TaxonChangeStatusList)(TaxonChangeStatuses[locale.ISOCode]);
                }
            }
            return taxonChangeStatuses;
        }

        /// <summary>
        /// Get taxon quality summary for a specified taxon
        /// </summary>
        /// <param name="taxon">Taxon to get quality summary for</param>
        /// <returns>List of quality summary values.</returns>
        protected override TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(ITaxon taxon)
        {
            TaxonChildQualityStatisticsList taxonQualitySummaries = null;

            lock (TaxonChildQualityStatistics)
            {
                if (TaxonChildQualityStatistics.ContainsKey(taxon.Id))
                {
                    taxonQualitySummaries = (TaxonChildQualityStatisticsList)(TaxonChildQualityStatistics[taxon.Id]);
                }
            }
            return taxonQualitySummaries;
        }

        /// <summary>
        /// Get taxon name categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name categories for specified locale.</returns>
        protected override TaxonNameCategoryList GetTaxonNameCategories(ILocale locale)
        {
            TaxonNameCategoryList taxonNameCategories = null;

            lock (TaxonNameCategories)
            {
                if (TaxonNameCategories.ContainsKey(locale.ISOCode))
                {
                    taxonNameCategories = (TaxonNameCategoryList)(TaxonNameCategories[locale.ISOCode]);
                }
            }
            return taxonNameCategories;
        }

        /// <summary>
        /// Get taxon name categoriy types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name category types for specified locale.</returns>
        protected override TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(ILocale locale)
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes = null;

            lock (TaxonNameCategoryTypes)
            {
                if (TaxonNameCategoryTypes.ContainsKey(locale.ISOCode))
                {
                    taxonNameCategoryTypes = (TaxonNameCategoryTypeList)(TaxonNameCategoryTypes[locale.ISOCode]);
                }
            }
            return taxonNameCategoryTypes;
        }

        /// <summary>
        /// Get taxon names for specified locale and taxon.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon names for specified locale.</returns>
        protected override TaxonNameList GetTaxonNames(ITaxon taxon, ILocale locale)
        {
            String cacheKey;
            TaxonNameList taxonNames = null;

            cacheKey = GetTaxonNameCacheKey(taxon, locale);
            lock (TaxonNames)
            {
                if (TaxonNames.ContainsKey(cacheKey))
                {
                    taxonNames = (TaxonNameList)(TaxonNames[cacheKey]);
                }
            }
            return taxonNames;
        }

        /// <summary>
        /// Get taxon name status for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name status for specified locale.</returns>
        protected override TaxonNameStatusList GetTaxonNameStatus(ILocale locale)
        {
            TaxonNameStatusList taxonNameStatus = null;

            lock (TaxonNameStatuses)
            {
                if (TaxonNameStatuses.ContainsKey(locale.ISOCode))
                {
                    taxonNameStatus = (TaxonNameStatusList)(TaxonNameStatuses[locale.ISOCode]);
                }
            }
            return taxonNameStatus;
        }

        /// <summary>
        /// Get taxon name usage for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name usage for specified locale.</returns>
        protected override TaxonNameUsageList GetTaxonNameUsage(ILocale locale)
        {
            TaxonNameUsageList taxonNameUsage = null;

            lock (TaxonNameUsages)
            {
                if (TaxonNameUsages.ContainsKey(locale.ISOCode))
                {
                    taxonNameUsage = (TaxonNameUsageList)(TaxonNameUsages[locale.ISOCode]);
                }
            }

            return taxonNameUsage;
        }


        /// <summary>
        /// Get taxon revision event types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon revision event types for specified locale.</returns>
        protected override TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(ILocale locale)
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes = null;

            lock (TaxonRevisionEventTypes)
            {
                if (TaxonRevisionEventTypes.ContainsKey(locale.ISOCode))
                {
                    taxonRevisionEventTypes = (TaxonRevisionEventTypeList)(TaxonRevisionEventTypes[locale.ISOCode]);
                }
            }
            return taxonRevisionEventTypes;
        }

        /// <summary>
        /// Get taxon revision states for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon revision states for specified locale.</returns>
        protected override TaxonRevisionStateList GetTaxonRevisionStates(ILocale locale)
        {
            TaxonRevisionStateList taxonRevisionStates = null;

            lock (TaxonRevisionStates)
            {
                if (TaxonRevisionStates.ContainsKey(locale.ISOCode))
                {
                    taxonRevisionStates = (TaxonRevisionStateList)(TaxonRevisionStates[locale.ISOCode]);
                }
            }
            return taxonRevisionStates;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (LumpSplitEventTypes)
            {
                LumpSplitEventTypes.Clear();
            }
            lock (Taxa)
            {
                Taxa.Clear();
            }
            lock (TaxonAlertStatuses)
            {
                TaxonAlertStatuses.Clear();
            }
            lock (TaxonCategories)
            {
                TaxonCategories.Clear();
            }
            lock (TaxonChangeStatuses)
            {
                TaxonChangeStatuses.Clear();
            }
            lock (TaxonChildQualityStatistics)
            {
                TaxonChildQualityStatistics.Clear();
            }
            lock (TaxonNameCategories)
            {
                TaxonNameCategories.Clear();
            }
            lock (TaxonNameCategoryTypes)
            {
                TaxonNameCategoryTypes.Clear();
            }
            lock (TaxonNames)
            {
                TaxonNames.Clear();
            }
            lock (TaxonNameStatuses)
            {
                TaxonNameStatuses.Clear();
            }
            lock (TaxonNameUsages)
            {
                TaxonNameUsages.Clear();
            }
            lock (TaxonRevisionEventTypes)
            {
                TaxonRevisionEventTypes.Clear();
            }
            lock (TaxonRevisionStates)
            {
                TaxonRevisionStates.Clear();
            }
        }

        /// <summary>
        /// Set lump split event types for specified locale.
        /// </summary>
        /// <param name="lumpSplitEventTypes">Reference relation types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetLumpSplitEventTypes(LumpSplitEventTypeList lumpSplitEventTypes,
                                                       ILocale locale)
        {
            lock (LumpSplitEventTypes)
            {
                LumpSplitEventTypes[locale.ISOCode] = lumpSplitEventTypes;
            }
        }

        /// <summary>
        /// Set taxon information for specified locale.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxon(ITaxon taxon, ILocale locale)
        {
            lock (Taxa)
            {
                Taxa[GetTaxonCacheKey(taxon.Id, locale)] = taxon;
            }
        }

        /// <summary>
        /// Set taxon alert statuses for specified locale.
        /// </summary>
        /// <param name="taxonAlertStatuses">Taxon alert statuses.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonAlertStatuses(TaxonAlertStatusList taxonAlertStatuses,
                                                      ILocale locale)
        {
            lock (TaxonAlertStatuses)
            {
                TaxonAlertStatuses[locale.ISOCode] = taxonAlertStatuses;
            }
        }

        /// <summary>
        /// Set taxon categories for specified locale.
        /// </summary>
        /// <param name="taxonCategories">Taxon categories.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonCategories(TaxonCategoryList taxonCategories,
                                                   ILocale locale)
        {
            lock (TaxonCategories)
            {
                TaxonCategories[locale.ISOCode] = taxonCategories;
            }
        }

        /// <summary>
        /// Set taxon change statuses for specified locale.
        /// </summary>
        /// <param name="taxonChangeStatuses">Taxon change statuses.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonChangeStatuses(TaxonChangeStatusList taxonChangeStatuses,
                                                       ILocale locale)
        {
            lock (TaxonChangeStatuses)
            {
                TaxonChangeStatuses[locale.ISOCode] = taxonChangeStatuses;
            }
        }

        /// <summary>
        /// Set taxon quality summary for specified taxon
        /// </summary>
        /// <param name="taxonQualitySummary">Taxon quality summary list.</param>
        /// <param name="taxon">Taxon.</param>
        protected override void SetTaxonChildQualityStatistics(TaxonChildQualityStatisticsList taxonQualitySummary, ITaxon taxon)
        {
            lock (TaxonChildQualityStatistics)
            {
                TaxonChildQualityStatistics[taxon.Id] = taxonQualitySummary;
            }
        }

        /// <summary>
        /// Set taxon name categories for specified locale.
        /// </summary>
        /// <param name="taxonNameCategories">Taxon name categories.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonNameCategories(TaxonNameCategoryList taxonNameCategories,
                                                       ILocale locale)
        {
            lock (TaxonNameCategories)
            {
                TaxonNameCategories[locale.ISOCode] = taxonNameCategories;
            }
        }

        /// <summary>
        /// Set taxon name category types for specified locale.
        /// </summary>
        /// <param name="taxonNameCategoryTypes">Taxon name category types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonNameCategoryTypes(TaxonNameCategoryTypeList taxonNameCategoryTypes,
                                                          ILocale locale)
        {
            lock (TaxonNameCategoryTypes)
            {
                TaxonNameCategoryTypes[locale.ISOCode] = taxonNameCategoryTypes;
            }
        }

        /// <summary>
        /// Set taxon names for specified locale and taxon.
        /// </summary>
        /// <param name="taxonNames">Taxon names.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonNames(TaxonNameList taxonNames,
                                              ITaxon taxon,
                                              ILocale locale)
        {
            lock (TaxonNames)
            {
                TaxonNames[GetTaxonNameCacheKey(taxon, locale)] = taxonNames;
            }
        }

        /// <summary>
        /// Set taxon name status for specified locale.
        /// </summary>
        /// <param name="taxonNameStatus">Taxon name status.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonNameStatus(TaxonNameStatusList taxonNameStatus,
                                                   ILocale locale)
        {
            lock (TaxonNameStatuses)
            {
                TaxonNameStatuses[locale.ISOCode] = taxonNameStatus;
            }
        }

        /// <summary>
        /// Set taxon name usage for specified locale.
        /// </summary>
        /// <param name="taxonNameUsage">Taxon name usage.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonNameUsage(TaxonNameUsageList taxonNameUsage,
                                                   ILocale locale)
        {
            lock (TaxonNameUsages)
            {
                TaxonNameUsages[locale.ISOCode] = taxonNameUsage;
            }
        }

        /// <summary>
        /// Set taxon revision event types for specified locale.
        /// </summary>
        /// <param name="taxonRevisionEventTypes">Taxon revision event types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonRevisionEventTypes(TaxonRevisionEventTypeList taxonRevisionEventTypes,
                                                           ILocale locale)
        {
            lock (TaxonRevisionEventTypes)
            {
                TaxonRevisionEventTypes[locale.ISOCode] = taxonRevisionEventTypes;
            }
        }

        /// <summary>
        /// Set taxon revision states for specified locale.
        /// </summary>
        /// <param name="taxonRevisionStates">Taxon revision states.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetTaxonRevisionStates(TaxonRevisionStateList taxonRevisionStates,
                                                       ILocale locale)
        {
            lock (TaxonRevisionStates)
            {
                TaxonRevisionStates[locale.ISOCode] = taxonRevisionStates;
            }
        }
    }
}
