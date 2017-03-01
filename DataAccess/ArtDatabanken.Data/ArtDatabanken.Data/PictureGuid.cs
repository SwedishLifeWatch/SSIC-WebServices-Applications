using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class holds information on what GUID is connected to corresponding picture.
    /// </summary>
    public class PictureGuid : IPictureGuid
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// GUID for the object that is related to a picture.
        /// </summary>
        public String ObjectGuid { get; set; }

        /// <summary>
        /// Id for the picture that the object guid is related to.
        /// </summary>
        public Int64 Id { get; set; }
    }
}
