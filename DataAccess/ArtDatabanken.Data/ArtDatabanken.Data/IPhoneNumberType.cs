using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  Enum that contains phone number type ids.
    /// </summary>
    public enum PhoneNumberTypeId
    {
        /// <summary>Home</summary>
        Home = 1,
        /// <summary>Work</summary>
        Work = 2,
        /// <summary>Mobil</summary>
        Mobil = 3
    }

    /// <summary>
    /// This interface handles information about a phone number type.
    /// </summary>
    public interface IPhoneNumberType : IDataId32
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Name.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Get StringId for the Name property.
        /// </summary>
        Int32 NameStringId
        { get; }
    }
}
