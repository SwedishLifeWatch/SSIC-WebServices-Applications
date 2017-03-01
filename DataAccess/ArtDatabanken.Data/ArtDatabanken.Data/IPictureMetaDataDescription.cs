using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents picture metadata description.
    /// </summary>
    public interface IPictureMetaDataDescription : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for picture metadata description.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Exif code (hexadecimal format) for picture meta data description.
        /// </summary>
        String Exif { get; set; }

        /// <summary>
        /// Name for picture metadata description.
        /// </summary>
        String Name { get; set; }
    }
}