using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents picture metadata description.
    /// </summary>
    public class PictureMetaDataDescription : IPictureMetaDataDescription
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for picture metadata description.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Exif code (hexadecimal format) for picture meta data description.
        /// </summary>
        public String Exif { get; set; }

        /// <summary>
        /// Id for picture metadata description.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Name for picture metadata description.
        /// </summary>
        public String Name { get; set; }
    }
}