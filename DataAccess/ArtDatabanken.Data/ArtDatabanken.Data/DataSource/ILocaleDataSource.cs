using System;

namespace ArtDatabanken.Data.DataSource
{
    /// <summary>
    /// Definition of the LocaleDataSource interface.
    /// This interface is used to retrieve locale related information.
    /// </summary>
    public interface ILocaleDataSource : IDataSource
    {
        /// <summary>
        /// Get all active locales.
        /// </summary>
        /// <param name="userContext">User context.</param>
        /// <returns>All active locales.</returns>
        LocaleList GetLocales(IUserContext userContext);
    }
}
