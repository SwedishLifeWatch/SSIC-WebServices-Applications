using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of region related information.
    /// In current version of Region there's no language handling for regions
    /// but Locale is used to secure future development.
    /// </summary>
    public class RegionManagerMultiThreadCache : RegionManagerSingleThreadCache
    {
        /// <summary>
        /// Create a RegionManagerMultiThreadCache instance.
        /// </summary>
        public RegionManagerMultiThreadCache(ICoordinateSystem coordinateSystem) : base(coordinateSystem)
        {

        }

        /// <summary>
        /// Get region categories for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Region types for specified locale.</returns>
        protected override RegionCategoryList GetRegionCategories(ILocale locale)
        {
            RegionCategoryList regionCategories = null;

            lock (RegionCategories)
            {
                if (RegionCategories.ContainsKey(locale))
                {
                    regionCategories = (RegionCategoryList)(RegionCategories[locale]);
                }
            }
            return regionCategories;
        }


        /// <summary>
        /// Get region types for specified locale.
        /// </summary>
        /// <param name="locale">Locale.</param>
        /// <returns>Region types for specified locale.</returns>
        protected override RegionTypeList GetRegionTypes(ILocale locale)
        {
            RegionTypeList regionTypes = null;

            lock (RegionTypes)
            {
                if (RegionTypes.ContainsKey(locale))
                {
                    regionTypes = (RegionTypeList)(RegionTypes[locale]);
                }
            }
            return regionTypes;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (RegionTypes)
            {
                RegionTypes.Clear();
            }
            lock (RegionCategories)
            {
                RegionCategories.Clear();
            }
        }

        /// <summary>
        /// Set region categories for specified locale.
        /// </summary>
        /// <param name="regionCategories">Region categories.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetRegionCategories(RegionCategoryList regionCategories,
                                                    ILocale locale)
        {
            lock (RegionCategories)
            {
                RegionCategories[locale.ISOCode] = regionCategories;
            }
        }

        /// <summary>
        /// Set region types for specified locale.
        /// </summary>
        /// <param name="regionTypes">Region types.</param>
        /// <param name="locale">Locale.</param>
        protected override void SetRegionTypes(RegionTypeList regionTypes,
                                               ILocale locale)
        {
            lock (RegionTypes)
            {
                RegionTypes[locale.ISOCode] = regionTypes;
            }
        }
    }
}
