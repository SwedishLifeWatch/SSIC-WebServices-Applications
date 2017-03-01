using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents picture information.
    /// </summary>
    public class PictureInformation : IPictureInformation
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for this picture.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        public PictureMetaDataList MetaData { get; set; }

        /// <summary>
        /// The picture.
        /// </summary>
        public IPicture Picture { get; set; }

        /// <summary>
        /// Relations that this picture has to other objects.
        /// </summary>
        public PictureRelationList Relations { get; set; }
    }
}