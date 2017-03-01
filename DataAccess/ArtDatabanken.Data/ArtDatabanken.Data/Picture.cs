using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a picture.
    /// </summary>
    public class Picture : IPicture
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// The picture is represented in this format.
        /// E.g. 'jpg', 'bmp' or 'png'.
        /// </summary>
        public String Format { get; set; }

        /// <summary>
        /// GUID for this picture.
        /// </summary>
        public String Guid { get; set; }

        /// <summary>
        /// Id for this picture.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// The image as binary (unsigned byte) data
        /// that has been Base64 encoded.
        /// </summary>
        public String Image { get; set; }

        /// <summary>
        /// Note if picture is archived or not.
        /// </summary>
        public Boolean IsArchived { get; set; }

        /// <summary>
        /// Note if picture is public or not.
        /// </summary>
        public Boolean IsPublic { get; set; }

        /// <summary>
        /// When picture was last modified.
        /// </summary>
        public DateTime LastModified { get; set; }

        /// <summary>
        /// When information about a picture in the database was last changed.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Original size (in bytes) of picture in web service.
        /// </summary>
        public Int64 OriginalSize { get; set; }

        /// <summary>
        /// String identifier for this picture.
        /// </summary>
        public Int64 PictureStringId { get; set; }

        /// <summary>
        /// Size (in bytes) of picture.
        /// </summary>
        public Int64 Size { get; set; }

        /// <summary>
        /// This picture is related to this taxon.
        /// Not all pictures are related to taxon.
        /// </summary>
        public ITaxon Taxon { get; set; }

        /// <summary>
        /// User who last updated the picture. (PictureModifiedBy)
        /// </summary>
        public String UpdatedBy { get; set; }

        /// <summary>
        /// Version id for picture.
        /// </summary>
        public Int64 VersionId { get; set; }
    }
}