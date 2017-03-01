using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a picture.
    /// </summary>
    public interface IPicture : IDataId64
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// The picture is represented in this format.
        /// E.g. 'jpg', 'bmp' or 'png'.
        /// </summary>
        String Format { get; set; }

        /// <summary>
        /// GUID for this picture.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// The image as binary (unsigned byte) data
        /// that has been Base64 encoded.
        /// </summary>
        String Image { get; set; }

        /// <summary>
        /// Note if picture is archived or not.
        /// </summary>
        Boolean IsArchived { get; set; }

        /// <summary>
        /// Note if picture is public or not.
        /// </summary>
        Boolean IsPublic { get; set; }

        /// <summary>
        /// When picture was last modified.
        /// </summary>
        DateTime LastModified { get; set; }

        /// <summary>
        /// Last updated date of picture information.
        /// </summary>
        DateTime LastUpdated { get; set; }

        /// <summary>
        /// Original size (in bytes) of picture in web service.
        /// </summary>
        Int64 OriginalSize { get; set; }

        /// <summary>
        /// String identifier for this picture.
        /// </summary>
        Int64 PictureStringId { get; set; }

        /// <summary>
        /// Size (in bytes) of picture.
        /// </summary>
        Int64 Size { get; set; }

        /// <summary>
        /// This picture is related to this taxon.
        /// Not all pictures are related to taxon.
        /// </summary>
        ITaxon Taxon { get; set; }

        /// <summary>
        /// User who last updated the picture.
        /// </summary>
        String UpdatedBy { get; set; }

        /// <summary>
        /// Version id for picture.
        /// </summary>
        Int64 VersionId { get; set; }
    }
}