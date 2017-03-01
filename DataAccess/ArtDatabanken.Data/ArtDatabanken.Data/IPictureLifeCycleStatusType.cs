using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a picture life cycle status type.
    /// </summary>
    public interface IPictureLifeCycleStatusType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identifier for this picture life cycle status type.
        /// </summary>
        String Identifier { get; set; }

        /// <summary>
        /// Name for this picture life cycle status type.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Description for this picture life cycle status type.
        /// </summary>
        String Description { get; set; }
    }
}
