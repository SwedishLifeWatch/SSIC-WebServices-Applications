using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Meta information about a data object.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Information about the data source.
        /// </summary>
        IDataSourceInformation DataSource
        { get; set; }

        /// <summary>
        /// Indicates if data in the related data object has been changed.
        /// </summary>
        Boolean IsChanged
        { get; set; }

        /// <summary>
        /// The related data object was retrieved with this locale set.
        /// Can be null if the data handling is independent of locale.
        /// </summary>
        ILocale Locale
        { get; }
    }
}
