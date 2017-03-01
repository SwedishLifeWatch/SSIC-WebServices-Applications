using System;
using System.Globalization;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This interface handles information about a locale.
    /// </summary>
    public interface ILocale : IDataId32
    {
        /// <summary>
        /// Get culture info which correspond to this locale.
        /// </summary>
        CultureInfo CultureInfo
        { get; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Get ISO code for this locale.
        /// This code is a combination of "ISO 639-1" (language code)
        /// and "ISO 3166-1 alpha-2" (country code).
        /// E.g. en-GB and sv-SE.
        /// </summary>
        String ISOCode
        { get; }

        /// <summary>
        /// Get english name of the locale.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Get native name or names of the locale.
        /// </summary>
        String NativeName
        { get; }
    }
}
