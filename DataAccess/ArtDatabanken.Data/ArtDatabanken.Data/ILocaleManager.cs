using System;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Definition of the LocaleManager interface.
    /// </summary>
    public interface ILocaleManager : IManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        ILocaleDataSource DataSource
        { get; set; }

        /// <summary>
        /// Get default locale.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Default locale.</returns>
        ILocale GetDefaultLocale(IUserContext userContext);

        /// <summary>
        /// Get locale with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Locale with specified id.</returns>
        ILocale GetLocale(IUserContext userContext, Int32 localeId);

        /// <summary>
        /// Get locale with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Locale with specified id.</returns>
        ILocale GetLocale(IUserContext userContext, LocaleId localeId);

        /// <summary>
        /// Get a locale object by ISO code.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// Language codes are matched to first locale of the same language.
        /// For example "en" may be matched to "en-GB".
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="localeISOCode">ISO code for requested locale.</param>
        /// <returns>Requested locale object.</returns>
        ILocale GetLocale(IUserContext userContext, String localeISOCode);

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        LocaleList GetLocales(IUserContext userContext);

        /// <summary>
        /// Get all locales that is used in this application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        LocaleList GetUsedLocales(IUserContext userContext);
    }
}
