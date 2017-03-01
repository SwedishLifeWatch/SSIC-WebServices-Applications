using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information related to a country.
    /// </summary>
    public interface ICountry : IDataId32
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Get two character code representing the country
        /// according to ISO-3166. Not Null. Is read only.
        /// </summary>
        String ISOCode
        { get; }

        /// <summary>
        /// Get ISO name of the country. Not NULL. Is read only.
        /// </summary>
        String ISOName
        { get; }

        /// <summary>
        /// Get english name of the country. Not NULL. Is read only.
        /// </summary>
        String Name
        { get; }

        /// <summary>
        /// Get native name or names of the country. Not Null. Is read only.
        /// </summary>
        String NativeName
        { get; }

        /// <summary>
        /// Get phone number prefix. Not NULL. Is read only.
        /// </summary>
        Int32 PhoneNumberPrefix
        { get; }
    }
}
