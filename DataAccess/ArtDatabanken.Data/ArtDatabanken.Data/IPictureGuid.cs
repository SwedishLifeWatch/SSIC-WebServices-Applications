using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds information on what GUID is connected to corresponding picture.
    /// </summary>
    public interface IPictureGuid : IDataId64
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID for the object that is related to a picture.
        /// </summary>
        String ObjectGuid { get; set; }

    }
}
