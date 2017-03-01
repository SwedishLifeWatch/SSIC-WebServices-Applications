namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents picture information.
    /// </summary>
    public interface IPictureInformation : IDataId64
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Meta data about the picture.
        /// </summary>
        PictureMetaDataList MetaData { get; set; }

        /// <summary>
        /// The picture.
        /// </summary>
        IPicture Picture { get; set; }

        /// <summary>
        /// Relations that this picture has to other objects.
        /// </summary>
        PictureRelationList Relations { get; set; }
    }
}