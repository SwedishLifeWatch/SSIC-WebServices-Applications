using System;
using System.Collections;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles cache of locale related information.
    /// </summary>
    public class LocaleManagerMultiThreadCache : LocaleManagerSingleThreadCache
    {
        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <returns>All active locales.</returns>
        protected override LocaleList GetLocales()
        {
            LocaleList locales;

            lock (this)
            {
                locales = Locales;
            }

            return locales;
        }

        /// <summary>
        /// Refresh cached data.
        /// </summary>
        /// <param name="userContext">User context.</param>
        protected override void RefreshCache(IUserContext userContext)
        {
            lock (this)
            {
                Locales = null;
            }
        }

        /// <summary>
        /// Set active locales.
        /// </summary>
        /// <param name="locales">Active locales.</param>
        protected override void SetLocales(LocaleList locales)
        {
            lock (this)
            {
                Locales = locales;
            }
        }
    }
}
