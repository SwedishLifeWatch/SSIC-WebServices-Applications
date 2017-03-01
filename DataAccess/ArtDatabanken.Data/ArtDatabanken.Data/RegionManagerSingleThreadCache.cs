using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of region related information.
    /// In current version of Region there's no language handling for regions
    /// but Locale is used to secure future development.
    /// </summary>
    public class RegionManagerSingleThreadCache : RegionManager
    {

        /// <summary>
        /// Create a RegionManagerSingleThreadCache instance.
        /// </summary>
        public RegionManagerSingleThreadCache(ICoordinateSystem coordinateSystem) : base(coordinateSystem)
        {
            RegionTypes = new Hashtable();
            RegionCategories = new Hashtable();
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Get region categories.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="countryIsoCode">
        /// Get region categories related to this country.
        /// Country iso codes are specified in standard ISO-3166.
        /// </param>
        /// <returns>All region categories.</returns>
        public override RegionCategoryList GetRegionCategories(IUserContext userContext,
                                                               Int32? countryIsoCode)
        {
            RegionCategoryList regionCategories;

            regionCategories = GetRegionCategories(userContext.Locale);
            if (regionCategories.IsNull())
            {
                // get all region categories
                Int32? noCountryIsoCode = new Int32?();
                regionCategories = base.GetRegionCategories(userContext, noCountryIsoCode);
                SetRegionCategories(regionCategories, userContext.Locale);
            }
            if (countryIsoCode.HasValue && (regionCategories.IsNotEmpty()))
            {
                // make a copy of regionCategories list object
                RegionCategoryList regionCategoriesClone = new RegionCategoryList();
                regionCategoriesClone = regionCategories;
                regionCategories = new RegionCategoryList();
                // add the region categories where countrycode matches the requested one.
                foreach (RegionCategory regionCategory in regionCategoriesClone)
                {
                    if (regionCategory.CountryIsoCode.Equals(countryIsoCode))
                    {
                        regionCategories.Add(regionCategory);
                    }
                }
            }
            return regionCategories;
        }

        /// <summary>
        /// Get region categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Region types for specified locale.</returns>
        protected virtual RegionCategoryList GetRegionCategories(ILocale locale)
        {
            RegionCategoryList regionCategories = null;

            if (RegionCategories.ContainsKey(locale))
            {
                regionCategories = (RegionCategoryList)(RegionCategories[locale]);
            }
            return regionCategories;
        }

        /// <summary>
        /// Get all region types.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All region types.</returns>
        public override RegionTypeList GetRegionTypes(IUserContext userContext)
        {
            RegionTypeList regionTypes;

            regionTypes = GetRegionTypes(userContext.Locale);
            if (regionTypes.IsNull())
            {
                regionTypes = base.GetRegionTypes(userContext);
                SetRegionTypes(regionTypes, userContext.Locale);
            }
            return regionTypes;
        }

        /// <summary>
        /// Get region types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Region types for specified locale.</returns>
        protected virtual RegionTypeList GetRegionTypes(ILocale locale)
        {
            RegionTypeList regionTypes = null;

            if (RegionTypes.ContainsKey(locale))
            {
                regionTypes = (RegionTypeList)(RegionTypes[locale]);
            }
            return regionTypes;
        }

        /// <summary>
        /// Set region categories for specified locale.
        /// </summary>
        /// <param name="regionCategories">Region categories.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetRegionCategories(RegionCategoryList regionCategories,
                                                   ILocale locale)
        {
            RegionCategories[locale.ISOCode] = regionCategories;
        }

        /// <summary>
        /// Set region types for specified locale.
        /// </summary>
        /// <param name="regionTypes">Region types.</param>
        /// <param name="locale">Locale.</param>
        protected virtual void SetRegionTypes(RegionTypeList regionTypes,
                                              ILocale locale)
        {
            RegionTypes[locale.ISOCode] = regionTypes;
        }

        /// <summary>
        /// Region categories cache.
        /// </summary>
        protected Hashtable RegionCategories
        { get; private set; }

        /// <summary>
        /// Region types cache.
        /// </summary>
        protected Hashtable RegionTypes
        { get; private set; }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            RegionTypes.Clear();
            RegionCategories.Clear();
        }
    }
}
