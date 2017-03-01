using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  Enum that contains message type ids.
    /// </summary>
    public enum MessageTypeId
    {
        /// <summary>No message</summary>
        NoMail = 1,
        /// <summary>Information</summary>
        Information = 2,
        /// <summary>Agreement </summary>
        Agreement = 3
    }

    /// <summary>
    /// This interface handles information about a message type.
    /// </summary>
    public interface IMessageType : IDataId32
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
