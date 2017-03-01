using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about a phone number.
    /// </summary>
    public interface IPhoneNumber : IDataId32
    {
        /// <summary>
        /// Country. Not null.
        /// </summary>
        ICountry Country
        { get; set; }

        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Phone number. Not empty.
        /// </summary>
        String Number
        { get; set; }

        /// <summary>
        /// Get country specific phone number prefix.
        /// Not NULL. Is read only.
        /// </summary>
        Int32 Prefix
        { get; }

        /// <summary>
        /// Type of phone number. Not null.
        /// </summary>
        IPhoneNumberType Type
        { get; set; }
    }
}
