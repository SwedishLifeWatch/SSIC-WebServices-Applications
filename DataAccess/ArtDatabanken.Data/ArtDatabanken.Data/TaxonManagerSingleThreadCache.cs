using System;
using System.Collections;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of taxon related information.
    /// </summary>
    public class TaxonManagerSingleThreadCache : TaxonManager
    {
        /// <summary>
        /// Create a TaxonManagerSingleThreadCache instance.
        /// </summary>
        public TaxonManagerSingleThreadCache()
        {
            LumpSplitEventTypes = new Hashtable();
            Taxa = new Hashtable();
            TaxonAlertStatuses = new Hashtable();
            TaxonCategories = new Hashtable();
            TaxonChangeStatuses = new Hashtable();
            TaxonNameCategories = new Hashtable();
            TaxonNameCategoryTypes = new Hashtable();
            TaxonNames = new Hashtable();
            TaxonNameStatuses = new Hashtable();
            TaxonNameUsages = new Hashtable();
            TaxonChildQualityStatistics = new Hashtable();
            TaxonRevisionEventTypes = new Hashtable();
            TaxonRevisionStates = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Taxon cache.
        /// </summary>
        protected Hashtable LumpSplitEventTypes
        { get; private set; }

        /// <summary>
        /// Taxon cache.
        /// </summary>
        protected Hashtable Taxa
        { get; private set; }

        /// <summary>
        /// Taxon alert statuese.
        /// </summary>
        protected Hashtable TaxonAlertStatuses
        { get; private set; }

        /// <summary>
        /// Taxon category cache.
        /// </summary>
        protected Hashtable TaxonCategories
        { get; private set; }

        /// <summary>
        /// Taxon change statuese.
        /// </summary>
        protected Hashtable TaxonChangeStatuses
        { get; private set; }

        /// <summary>
        /// TaxonQualitySummaryList cache.
        /// </summary>
        protected Hashtable TaxonChildQualityStatistics
        { get; private set; }

        /// <summary>
        /// Taxon name category cache.
        /// </summary>
        protected Hashtable TaxonNameCategories
        { get; private set; }

        /// <summary>
        /// Taxon name catgory type cache.
        /// </summary>
        protected Hashtable TaxonNameCategoryTypes
        { get; private set; }

        /// <summary>
        /// Taxon name cache.
        /// </summary>
        protected Hashtable TaxonNames
        { get; private set; }

        /// <summary>
        /// Taxon name status cache.
        /// </summary>
        protected Hashtable TaxonNameStatuses
        { get; private set; }

        /// <summary>
        /// Taxon name usage cache.
        /// </summary>
        protected Hashtable TaxonNameUsages
        { get; private set; }

        /// <summary>
        /// Taxon reviosion event type cache.
        /// </summary>
        protected Hashtable TaxonRevisionEventTypes
        { get; private set; }

        /// <summary>
        /// Taxon reviosion state cache.
        /// </summary>
        protected Hashtable TaxonRevisionStates
        { get; private set; }

        /// <summary>
        /// Get lump split event types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Lump split event types for specified locale.</returns>
        protected virtual LumpSplitEventTypeList GetLumpSplitEventTypes(ILocale locale)
        {
            LumpSplitEventTypeList lumpSplitEventTypes = null;

            if (LumpSplitEventTypes.ContainsKey(locale.ISOCode))
            {
                lumpSplitEventTypes = (LumpSplitEventTypeList)(LumpSplitEventTypes[locale.ISOCode]);
            }
            return lumpSplitEventTypes;
        }

        /// <summary>
        /// Get all lump split event types.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All lump split event types.</returns>
        public override LumpSplitEventTypeList GetLumpSplitEventTypes(IUserContext userContext)
        {
            LumpSplitEventTypeList lumpSplitEventTypes;

            lumpSplitEventTypes = GetLumpSplitEventTypes(userContext.Locale);
            if (lumpSplitEventTypes.IsNull())
            {
                lumpSplitEventTypes = base.GetLumpSplitEventTypes(userContext);
                SetLumpSplitEventTypes(lumpSplitEventTypes, userContext.Locale);
            }
            return lumpSplitEventTypes;
        }

        /// <summary>
        /// Get taxa with specified ids.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonIds">Taxon ids</param>
        /// <returns>Taxa with specified ids.</returns>
        public override TaxonList GetTaxa(IUserContext userContext,
                                               List<Int32> taxonIds)
        {
            ITaxon taxon;
            List<Int32> notFoundTaxonIds;
            TaxonList foundTaxa, notFoundTaxa, taxa;

            if (userContext.CurrentRole.IsNotNull() &&
                userContext.CurrentRole.Identifier.IsNotEmpty() &&
                userContext.CurrentRole.Identifier.StartsWith(Settings.Default.DyntaxaRevisionRoleIdentifier))
            {
                // Do not cache taxa if user is in a revision.
                taxa = base.GetTaxa(userContext, taxonIds);
            }
            else
            {
                // Get cached taxa.
                foundTaxa = new TaxonList(true);
                notFoundTaxa = null;
                notFoundTaxonIds = new List<Int32>();
                foreach (Int32 taxonId in taxonIds)
                {
                    taxon = GetTaxon(taxonId, userContext.Locale);
                    if (taxon.IsNull())
                    {
                        notFoundTaxonIds.Add(taxonId);
                    }
                    else
                    {
                        foundTaxa.Add(taxon);
                    }
                }

                // Get taxa from data source.
                if (notFoundTaxonIds.IsNotEmpty())
                {
                    notFoundTaxa = base.GetTaxa(userContext, notFoundTaxonIds);
                    foundTaxa.Merge(notFoundTaxa);
                    taxa = new TaxonList(true);
                    foreach (Int32 taxonId in taxonIds)
                    {
                        taxa.Add(foundTaxa.Get(taxonId));
                    }
                }
                else
                {
                    taxa = foundTaxa;
                }

                // Save taxa in cache.
                if (notFoundTaxa.IsNotEmpty())
                {
                    foreach (ITaxon tempTaxon in notFoundTaxa)
                    {
                        SetTaxon(tempTaxon, userContext.Locale);
                    }
                }
            }

            return taxa;
        }

        /// <summary>
        /// Get taxon information for specified locale.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon information for specified locale.</returns>
        protected virtual ITaxon GetTaxon(Int32 taxonId, ILocale locale)
        {
            String cacheKey;
            ITaxon taxon = null;

            cacheKey = GetTaxonCacheKey(taxonId, locale);
            if (Taxa.ContainsKey(cacheKey))
            {
                taxon = (ITaxon)(Taxa[cacheKey]);
            }
            return taxon;
        }

        /// <summary>
        /// Get taxon alert statuses for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon alert statuses for specified locale.</returns>
        protected virtual TaxonAlertStatusList GetTaxonAlertStatuses(ILocale locale)
        {
            TaxonAlertStatusList taxonAlertStatuses = null;

            if (TaxonAlertStatuses.ContainsKey(locale.ISOCode))
            {
                taxonAlertStatuses = (TaxonAlertStatusList)(TaxonAlertStatuses[locale.ISOCode]);
            }
            return taxonAlertStatuses;
        }

        /// <summary>
        /// Get all taxon alert statuses.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All taxon alert statuses.</returns>
        public override TaxonAlertStatusList GetTaxonAlertStatuses(IUserContext userContext)
        {
            TaxonAlertStatusList taxonAlertStatuses;

            taxonAlertStatuses = GetTaxonAlertStatuses(userContext.Locale);
            if (taxonAlertStatuses.IsNull())
            {
                taxonAlertStatuses = base.GetTaxonAlertStatuses(userContext);
                SetTaxonAlertStatuses(taxonAlertStatuses, userContext.Locale);
            }
            return taxonAlertStatuses;
        }

        /// <summary>
        /// Get taxon by id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxonId">Taxon id.</param>
        /// <returns>Requested taxon.</returns>       
        public override ITaxon GetTaxon(IUserContext userContext,
                                            Int32 taxonId)
        {
            ITaxon taxon;

            if (userContext.CurrentRole.IsNotNull() &&
                userContext.CurrentRole.Identifier.IsNotEmpty() &&
                userContext.CurrentRole.Identifier.StartsWith(Settings.Default.DyntaxaRevisionRoleIdentifier))
            {
                // Do not cache taxon if user is in a revision.
                taxon = base.GetTaxon(userContext, taxonId);
            }
            else
            {
                taxon = GetTaxon(taxonId, userContext.Locale);
                if (taxon.IsNull())
                {
                    taxon = base.GetTaxon(userContext, taxonId);
                    if (taxon.IsNotNull())
                    {
                        SetTaxon(taxon, userContext.Locale);    
                    }
                }
            }
            return taxon;
        }

        /// <summary>
        /// Get cache key for taxon.
        /// </summary>
        /// <param name="taxonId">Taxon id.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Cache key for taxon.</returns>
        protected String GetTaxonCacheKey(Int32 taxonId, ILocale locale)
        {
            return taxonId.WebToString() + locale.ISOCode;
        }

        /// <summary>
        /// Get taxon categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon categories for specified locale.</returns>
        protected virtual TaxonCategoryList GetTaxonCategories(ILocale locale)
        {
            TaxonCategoryList taxonCategories = null;

            if (TaxonCategories.ContainsKey(locale.ISOCode))
            {
                taxonCategories = (TaxonCategoryList)(TaxonCategories[locale.ISOCode]);
            }
            return taxonCategories;
        }

        /// <summary>
        /// Get all taxon categories.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon categories.</returns>       
        public override TaxonCategoryList GetTaxonCategories(IUserContext userContext)
        {
            TaxonCategoryList taxonCategories;

            taxonCategories = GetTaxonCategories(userContext.Locale);
            if (taxonCategories.IsNull())
            {
                taxonCategories = base.GetTaxonCategories(userContext);
                SetTaxonCategories(taxonCategories, userContext.Locale);
            }
            return taxonCategories;
        }

        /// <summary>
        /// Get taxon change statuses for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon change statuses for specified locale.</returns>
        protected virtual TaxonChangeStatusList GetTaxonChangeStatuses(ILocale locale)
        {
            TaxonChangeStatusList taxonChangeStatuses = null;

            if (TaxonChangeStatuses.ContainsKey(locale.ISOCode))
            {
                taxonChangeStatuses = (TaxonChangeStatusList)(TaxonChangeStatuses[locale.ISOCode]);
            }
            return taxonChangeStatuses;
        }

        /// <summary>
        /// Get all taxon change statuses.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <returns>All taxon change statuses.</returns>
        public override TaxonChangeStatusList GetTaxonChangeStatuses(IUserContext userContext)
        {
            TaxonChangeStatusList taxonChangeStatuses;

            taxonChangeStatuses = GetTaxonChangeStatuses(userContext.Locale);
            if (taxonChangeStatuses.IsNull())
            {
                taxonChangeStatuses = base.GetTaxonChangeStatuses(userContext);
                SetTaxonChangeStatuses(taxonChangeStatuses, userContext.Locale);
            }
            return taxonChangeStatuses;
        }

        /// <summary>
        /// Get taxon quality summary for a specified taxon
        /// </summary>
        /// <param name="taxon">Taxon to get quality summary for</param>
        /// <returns>List of quality summary values.</returns>
        protected virtual TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(ITaxon taxon)
        {
            TaxonChildQualityStatisticsList taxonQualitySummaries = null;

            if (TaxonChildQualityStatistics.ContainsKey(taxon.Id))
            {
                taxonQualitySummaries = (TaxonChildQualityStatisticsList)(TaxonChildQualityStatistics[taxon.Id]);
            }
            return taxonQualitySummaries;
        }

        /// <summary>
        /// Get taxon quality summary for a taxon
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="taxon">Taxon to get quality summary for</param>
        /// <returns>List of quality summary values.</returns>
        public override TaxonChildQualityStatisticsList GetTaxonChildQualityStatistics(IUserContext userContext, ITaxon taxon)
        {
            TaxonChildQualityStatisticsList taxonQualitySummary;

            taxonQualitySummary = GetTaxonChildQualityStatistics(taxon);
            if (taxonQualitySummary.IsNull())
            {
                taxonQualitySummary = base.GetTaxonChildQualityStatistics(userContext, taxon);
                SetTaxonChildQualityStatistics(taxonQualitySummary, taxon);
            }
            return taxonQualitySummary;
        }

        /// <summary>
        /// Get cache key for taxon names.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Cache key for taxon names.</returns>
        protected String GetTaxonNameCacheKey(ITaxon taxon, ILocale locale)
        {
            return taxon.Id.WebToString() + locale.ISOCode;
        }

        /// <summary>
        /// Get taxon name categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name categories for specified locale.</returns>
        protected virtual TaxonNameCategoryList GetTaxonNameCategories(ILocale locale)
        {
            TaxonNameCategoryList taxonNameCategories = null;

            if (TaxonNameCategories.ContainsKey(locale.ISOCode))
            {
                taxonNameCategories = (TaxonNameCategoryList)(TaxonNameCategories[locale.ISOCode]);
            }
            return taxonNameCategories;
        }

        /// <summary>
        /// Get all taxon name categories.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon name categories.</returns>       
        public override TaxonNameCategoryList GetTaxonNameCategories(IUserContext userContext)
        {
            TaxonNameCategoryList taxonNameCategories;

            taxonNameCategories = GetTaxonNameCategories(userContext.Locale);
            if (taxonNameCategories.IsNull())
            {
                taxonNameCategories = base.GetTaxonNameCategories(userContext);
                SetTaxonNameCategories(taxonNameCategories, userContext.Locale);
            }
            return taxonNameCategories;
        }

        /// <summary>
        /// Get taxon name category types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name category types for specified locale.</returns>
        protected virtual TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(ILocale locale)
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes = null;

            if (TaxonNameCategoryTypes.ContainsKey(locale.ISOCode))
            {
                taxonNameCategoryTypes = (TaxonNameCategoryTypeList)(TaxonNameCategoryTypes[locale.ISOCode]);
            }
            return taxonNameCategoryTypes;
        }

        /// <summary>
        /// Get all taxon name category types.
        /// </summary>
        /// <param name="userContext">User context</param>
        /// <returns>All taxon name category types.</returns>       
        public override TaxonNameCategoryTypeList GetTaxonNameCategoryTypes(IUserContext userContext)
        {
            TaxonNameCategoryTypeList taxonNameCategoryTypes;

            taxonNameCategoryTypes = GetTaxonNameCategoryTypes(userContext.Locale);
            if (taxonNameCategoryTypes.IsNull())
            {
                taxonNameCategoryTypes = base.GetTaxonNameCategoryTypes(userContext);
                SetTaxonNameCategoryTypes(taxonNameCategoryTypes, userContext.Locale);
            }
            return taxonNameCategoryTypes;
        }

        /// <summary>
        /// Get taxon names for specified locale and taxon.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon names for specified locale.</returns>
        protected virtual TaxonNameList GetTaxonNames(ITaxon taxon, ILocale locale)
        {
            String cacheKey;
            TaxonNameList taxonNames = null;

            cacheKey = GetTaxonNameCacheKey(taxon, locale);
            if (TaxonNames.ContainsKey(cacheKey))
            {
                taxonNames = (TaxonNameList)(TaxonNames[cacheKey]);
            }
            return taxonNames;
        }

        /// <summary>
        /// Get all taxon names for specified taxa.
        /// The result is sorted in the same order as input taxa.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxa">Taxa.</param>
        /// <returns>Taxon names.</returns>
        public override List<TaxonNameList> GetTaxonNames(IUserContext userContext,
                                                                TaxonList taxa)
        {
            Hashtable allTaxonNamesUnsorted;
            Int32 index;
            List<TaxonNameList> allTaxonNamesSorted, allTaxonNamesNotCached;
            TaxonList notCachedTaxa;
            TaxonNameList taxonNames;

            if (userContext.CurrentRole.IsNotNull() &&
                userContext.CurrentRole.Identifier.IsNotEmpty() &&
                userContext.CurrentRole.Identifier.StartsWith(Settings.Default.DyntaxaRevisionRoleIdentifier))
            {
                // Do not cache taxon names if user is in a revision.
                allTaxonNamesSorted = base.GetTaxonNames(userContext, taxa);
            }
            else
            {
                allTaxonNamesUnsorted = new Hashtable();
                notCachedTaxa = new TaxonList();

                // Get already cached taxon names.
                foreach (ITaxon taxon in taxa)
                {
                    taxonNames = GetTaxonNames(taxon, userContext.Locale);
                    if (taxonNames.IsEmpty())
                    {
                        notCachedTaxa.Add(taxon);
                    }
                    else
                    {
                        allTaxonNamesUnsorted[taxon.Id] = taxonNames;
                    }
                }

                // Get not already cached taxon names.
                if (notCachedTaxa.IsNotEmpty())
                {
                    allTaxonNamesNotCached = base.GetTaxonNames(userContext,
                                                                      notCachedTaxa);
                    foreach (TaxonNameList tempTaxonNames in allTaxonNamesNotCached)
                    {
                        allTaxonNamesUnsorted[tempTaxonNames[0].Taxon.Id] = tempTaxonNames;

                        // Cache not already cached taxon names.
                        SetTaxonNames(tempTaxonNames,
                                      tempTaxonNames[0].Taxon,
                                      userContext.Locale);
                    }
                }

                // Create sorted output.
                allTaxonNamesSorted = new List<TaxonNameList>();
                for (index = 0; index < taxa.Count; index++)
                {
                    allTaxonNamesSorted.Add((TaxonNameList)(allTaxonNamesUnsorted[taxa[index].Id]));
                }
            }
            return allTaxonNamesSorted;
        }

        /// <summary>
        /// Get all taxon names for taxon.
        /// </summary>
        /// <param name="userContext">The user context.</param>
        /// <param name="taxon">The taxon.</param>
        /// <returns>All taxon names for taxon.</returns>
        public override TaxonNameList GetTaxonNames(IUserContext userContext,
                                                           ITaxon taxon)
        {
            TaxonNameList taxonNames;

            if (userContext.CurrentRole.IsNotNull() &&
                userContext.CurrentRole.Identifier.IsNotEmpty() &&
                userContext.CurrentRole.Identifier.StartsWith(Settings.Default.DyntaxaRevisionRoleIdentifier))
            {
                // Do not cache taxon names if user is in a revision.
                taxonNames = base.GetTaxonNames(userContext, taxon);
            }
            else
            {
                taxonNames = GetTaxonNames(taxon, userContext.Locale);
                if (taxonNames.IsNull())
                {
                    taxonNames = base.GetTaxonNames(userContext, taxon);
                    SetTaxonNames(taxonNames, taxon, userContext.Locale);
                }
            }
            return taxonNames;
        }

        /// <summary>
        /// Get taxon name status for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name status for specified locale.</returns>
        protected virtual TaxonNameStatusList GetTaxonNameStatus(ILocale locale)
        {
            TaxonNameStatusList taxonNameStatus = null;

            if (TaxonNameStatuses.ContainsKey(locale.ISOCode))
            {
                taxonNameStatus = (TaxonNameStatusList)(TaxonNameStatuses[locale.ISOCode]);
            }
            return taxonNameStatus;
        }

        /// <summary>
        /// Get all possible values for taxon name staus.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All possible values for taxon name staus.</returns>       
        public override TaxonNameStatusList GetTaxonNameStatuses(IUserContext userContext)
        {
            TaxonNameStatusList taxonNameStatus;

            taxonNameStatus = GetTaxonNameStatus(userContext.Locale);
            if (taxonNameStatus.IsNull())
            {
                taxonNameStatus = base.GetTaxonNameStatuses(userContext);
                SetTaxonNameStatus(taxonNameStatus, userContext.Locale);
            }
            return taxonNameStatus;
        }


        /// <summary>
        /// Get taxon name usage for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon name usage for specified locale.</returns>
        protected virtual TaxonNameUsageList GetTaxonNameUsage(ILocale locale)
        {
            TaxonNameUsageList taxonNameUsage = null;

            if (TaxonNameUsages.ContainsKey(locale.ISOCode))
            {
                taxonNameUsage = (TaxonNameUsageList)(TaxonNameUsages[locale.ISOCode]);
            }

            return taxonNameUsage;
        }

        /// <summary>
        /// Get all possible values for taxon name usage.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All possible values for taxon name usage.</returns>       
        public override TaxonNameUsageList GetTaxonNameUsages(IUserContext userContext)
        {
            TaxonNameUsageList taxonNameUsage;

            taxonNameUsage = GetTaxonNameUsage(userContext.Locale);
            if (taxonNameUsage.IsNull())
            {
                taxonNameUsage = base.GetTaxonNameUsages(userContext);
                SetTaxonNameUsage(taxonNameUsage, userContext.Locale);
            }
            return taxonNameUsage;
        }

        /// <summary>
        /// Get taxon revision event types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon revision event types for specified locale.</returns>
        protected virtual TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(ILocale locale)
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes = null;

            if (TaxonRevisionEventTypes.ContainsKey(locale.ISOCode))
            {
                taxonRevisionEventTypes = (TaxonRevisionEventTypeList)(TaxonRevisionEventTypes[locale.ISOCode]);
            }
            return taxonRevisionEventTypes;
        }

        /// <summary>
        /// Get all taxon revision event types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision event types.</returns>
        public override TaxonRevisionEventTypeList GetTaxonRevisionEventTypes(IUserContext userContext)
        {
            TaxonRevisionEventTypeList taxonRevisionEventTypes;

            taxonRevisionEventTypes = GetTaxonRevisionEventTypes(userContext.Locale);
            if (taxonRevisionEventTypes.IsNull())
            {
                taxonRevisionEventTypes = base.GetTaxonRevisionEventTypes(userContext);
                SetTaxonRevisionEventTypes(taxonRevisionEventTypes, userContext.Locale);
            }
            return taxonRevisionEventTypes;
        }

        /// <summary>
        /// Get taxon revision states for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Taxon revision states for specified locale.</returns>
        protected virtual TaxonRevisionStateList GetTaxonRevisionStates(ILocale locale)
        {
            TaxonRevisionStateList taxonRevisionStates = null;

            if (TaxonRevisionStates.ContainsKey(locale.ISOCode))
            {
                taxonRevisionStates = (TaxonRevisionStateList)(TaxonRevisionStates[locale.ISOCode]);
            }
            return taxonRevisionStates;
        }

        /// <summary>
        /// Get all taxon revision states.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All taxon revision states.</returns>
        public override TaxonRevisionStateList GetTaxonRevisionStates(IUserContext userContext)
        {
            TaxonRevisionStateList taxonRevisionStates;

            taxonRevisionStates = GetTaxonRevisionStates(userContext.Locale);
            if (taxonRevisionStates.IsNull())
            {
                taxonRevisionStates = base.GetTaxonRevisionStates(userContext);
                SetTaxonRevisionStates(taxonRevisionStates, userContext.Locale);
            }
            return taxonRevisionStates;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            LumpSplitEventTypes.Clear();
            Taxa.Clear();
            TaxonAlertStatuses.Clear();
            TaxonCategories.Clear();
            TaxonChangeStatuses.Clear();
            TaxonChildQualityStatistics.Clear();
            TaxonNameCategories.Clear();
            TaxonNameCategoryTypes.Clear();
            TaxonNames.Clear();
            TaxonNameStatuses.Clear();
            TaxonNameUsages.Clear();
            TaxonRevisionEventTypes.Clear();
            TaxonRevisionStates.Clear();
        }

        /// <summary>
        /// Set lump split event types for specified locale.
        /// </summary>
        /// <param name="lumpSplitEventTypes">Reference relation types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetLumpSplitEventTypes(LumpSplitEventTypeList lumpSplitEventTypes,
                                                      ILocale locale)
        {
            LumpSplitEventTypes[locale.ISOCode] = lumpSplitEventTypes;
        }

        /// <summary>
        /// Set taxon information for specified locale.
        /// </summary>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxon(ITaxon taxon, ILocale locale)
        {
            Taxa[GetTaxonCacheKey(taxon.Id, locale)] = taxon;
        }

        /// <summary>
        /// Set taxon alert statuses for specified locale.
        /// </summary>
        /// <param name="taxonAlertStatuses">Taxon alert statuses.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonAlertStatuses(TaxonAlertStatusList taxonAlertStatuses,
                                                     ILocale locale)
        {
            TaxonAlertStatuses[locale.ISOCode] = taxonAlertStatuses;
        }

        /// <summary>
        /// Set taxon categories for specified locale.
        /// </summary>
        /// <param name="taxonCategories">Taxon categories.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonCategories(TaxonCategoryList taxonCategories,
                                                  ILocale locale)
        {
            TaxonCategories[locale.ISOCode] = taxonCategories;
        }

        /// <summary>
        /// Set taxon change statuses for specified locale.
        /// </summary>
        /// <param name="taxonChangeStatuses">Taxon change statuses.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonChangeStatuses(TaxonChangeStatusList taxonChangeStatuses,
                                                      ILocale locale)
        {
            TaxonChangeStatuses[locale.ISOCode] = taxonChangeStatuses;
        }

        /// <summary>
        /// Set taxon quality summary for specified taxon
        /// </summary>
        /// <param name="taxonQualitySummary">Taxon quality summary list.</param>
        /// <param name="taxon">Taxon.</param>
        protected virtual void SetTaxonChildQualityStatistics(TaxonChildQualityStatisticsList taxonQualitySummary, ITaxon taxon)
        {
            TaxonChildQualityStatistics[taxon.Id] = taxonQualitySummary;
        }

        /// <summary>
        /// Set taxon name categories for specified locale.
        /// </summary>
        /// <param name="taxonNameCategories">Taxon name categories.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonNameCategories(TaxonNameCategoryList taxonNameCategories,
                                                      ILocale locale)
        {
            TaxonNameCategories[locale.ISOCode] = taxonNameCategories;
        }

        /// <summary>
        /// Set taxon name category types for specified locale.
        /// </summary>
        /// <param name="taxonNameCategoryTypes">Taxon name category types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonNameCategoryTypes(TaxonNameCategoryTypeList taxonNameCategoryTypes,
                                                         ILocale locale)
        {
            TaxonNameCategoryTypes[locale.ISOCode] = taxonNameCategoryTypes;
        }

        /// <summary>
        /// Set taxon names for specified locale and taxon.
        /// </summary>
        /// <param name="taxonNames">Taxon names.</param>
        /// <param name="taxon">Taxon.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonNames(TaxonNameList taxonNames,
                                             ITaxon taxon, 
                                             ILocale locale)
        {
            TaxonNames[GetTaxonNameCacheKey(taxon, locale)] = taxonNames;
        }

        /// <summary>
        /// Set taxon name status for specified locale.
        /// </summary>
        /// <param name="taxonNameStatus">Taxon name status.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonNameStatus(TaxonNameStatusList taxonNameStatus,
                                                  ILocale locale)
        {
            TaxonNameStatuses[locale.ISOCode] = taxonNameStatus;
        }

        /// <summary>
        /// Set taxon name usage for specified locale.
        /// </summary>
        /// <param name="taxonNameUsage">Taxon name usage.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonNameUsage(TaxonNameUsageList taxonNameUsage,
                                                  ILocale locale)
        {
            TaxonNameUsages[locale.ISOCode] = taxonNameUsage;
        }

        /// <summary>
        /// Set taxon revision event types for specified locale.
        /// </summary>
        /// <param name="taxonRevisionEventTypes">Taxon revision event types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonRevisionEventTypes(TaxonRevisionEventTypeList taxonRevisionEventTypes,
                                                          ILocale locale)
        {
            TaxonRevisionEventTypes[locale.ISOCode] = taxonRevisionEventTypes;
        }

        /// <summary>
        /// Set taxon revision states for specified locale.
        /// </summary>
        /// <param name="taxonRevisionStates">Taxon revision states.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetTaxonRevisionStates(TaxonRevisionStateList taxonRevisionStates,
                                                      ILocale locale)
        {
            TaxonRevisionStates[locale.ISOCode] = taxonRevisionStates;
        }
    }
}
