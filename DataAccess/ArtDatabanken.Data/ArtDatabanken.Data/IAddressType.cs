using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  Enum that contains address type ids.
    /// </summary>
    public enum AddressTypeId
    {
        /// <summary>Home</summary>
        Home = 1,
        /// <summary>Work</summary>
        Work = 2,
        /// <summary>Billing</summary>
        Billing = 3
    }

    /// <summary>
    /// This interface handles information about an address type.
    /// </summary>
    public interface IAddressType : IDataId32
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
