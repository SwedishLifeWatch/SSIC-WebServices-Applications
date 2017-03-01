using System;
using ArtDatabanken.Data.DataSource;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Class that handles locale related information.
    /// </summary>
    public class LocaleManager : ILocaleManager
    {
        /// <summary>
        /// This interface is used to retrieve information.
        /// </summary>
        public ILocaleDataSource DataSource
        { get; set; }

        /// <summary>
        /// Get information about data source.
        /// </summary>
        /// <returns>Information about data source.</returns>
        public virtual IDataSourceInformation GetDataSourceInformation()
        {
            return DataSource.GetDataSourceInformation();
        }

        /// <summary>
        /// Get default locale.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>Default locale.</returns>
        public virtual ILocale GetDefaultLocale(IUserContext userContext)
        {
            switch (Configuration.CountryId)
            {
                case CountryId.Norway:
                    return GetLocale(userContext, LocaleId.nb_NO);
                case CountryId.Sweden:
                    return GetLocale(userContext, LocaleId.sv_SE);
                default:
                    throw new ApplicationException("Not handled country = " + Configuration.CountryId);
            }
        }

        /// <summary>
        /// Get locale with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Locale with specified id.</returns>
        public virtual ILocale GetLocale(IUserContext userContext,
                                         Int32 localeId)
        {
            return GetLocales(userContext).Get(localeId);
        }

        /// <summary>
        /// Get locale with specified id.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="localeId">Locale id.</param>
        /// <returns>Locale with specified id.</returns>
        public virtual ILocale GetLocale(IUserContext userContext,
                                         LocaleId localeId)
        {
            return GetLocale(userContext, (Int32)localeId);
        }

        /// <summary>
        /// Get a locale object by ISO code.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// Language codes are matched to first locale of the same language.
        /// For example "en" may be matched to "en-GB".
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <param name="ISOCode">ISO code for requested locale.</param>
        /// <returns>Requested locale object</returns>
        public virtual ILocale GetLocale(IUserContext userContext,
                                         String ISOCode)
        {
            return GetLocales(userContext).Get(ISOCode);
        }

        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        public virtual LocaleList GetLocales(IUserContext userContext)
        {
            return DataSource.GetLocales(userContext);
        }

        /// <summary>
        /// Get all locales that is used in this application.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        public virtual LocaleList GetUsedLocales(IUserContext userContext)
        {
            LocaleList usedLocales;

            usedLocales = new LocaleList();
            switch (Configuration.CountryId)
            {
                case CountryId.Norway:
                    usedLocales.Add(GetLocale(userContext, LocaleId.nb_NO));
                    usedLocales.Add(GetLocale(userContext, LocaleId.en_GB));
                    break;
                case CountryId.Sweden:
                    usedLocales.Add(GetLocale(userContext, LocaleId.sv_SE));
                    usedLocales.Add(GetLocale(userContext, LocaleId.en_GB));
                    break;
                default:
                    throw new ApplicationException("Not handled country = " + Configuration.CountryId);
            }
            return usedLocales;
        }
    }
}
