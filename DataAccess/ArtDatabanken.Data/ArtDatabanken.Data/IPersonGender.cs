using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  Enum that contains person gender ids.
    /// </summary>
    public enum PersonGenderId
    {
        /// <summary>Man</summary>
        Man = 1,
        /// <summary>Woman</summary>
        Woman = 2,
        /// <summary>Unspecified</summary>
        Unspecified = 3
    }

    /// <summary>
    /// This interface handles information about an person gender.
    /// </summary>
    public interface IPersonGender : IDataId32
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
        /// Get string id for the Name property.
        /// </summary>
        Int32 NameStringId
        { get; }
    }
}

