using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of locale related information.
    /// </summary>
    public class LocaleManagerSingleThreadCache : LocaleManager
    {
        /// <summary>
        /// Create a LocaleManagerSingleThreadCache instance.
        /// </summary>
        public LocaleManagerSingleThreadCache()
        {
            Locales = null;
            CacheManager.RefreshCacheEvent += RefreshCache;
        }

        /// <summary>
        /// Locales cache.
        /// </summary>
        protected LocaleList Locales
        { get; set; }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        public override LocaleList GetLocales(IUserContext userContext)
        {
            LocaleList locales;

            locales = GetLocales();
            if (locales.IsNull())
            {
                locales = base.GetLocales(userContext);
                SetLocales(locales);
            }
            return locales;
        }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <returns>All active locales.</returns>
        protected virtual LocaleList GetLocales()
        {
            return Locales;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected virtual void RefreshCache(IUserContext userContext)
        {
            Locales = null;
        }

        /// <summary>
        /// Set active locales.
        /// </summary>
        /// <param name="locales">Active locales.</param>
        protected virtual void SetLocales(LocaleList locales)
        {
            Locales = locales;
        }
    }
}
