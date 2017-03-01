using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a picture relation.
    /// </summary>
    public interface IPictureRelation : IDataId64
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Indicates if the picture that is referenced in
        /// this relation is the recommended picture for the
        /// related object.
        /// </summary>
        Boolean IsRecommended { get; set; }

        /// <summary>
        /// GUID for the object that is related to a picture.
        /// </summary>
        String ObjectGuid { get; set; }

        /// <summary>
        /// Id for the picture that the object is related to.
        /// </summary>
        Int64 PictureId { get; set; }

        /// <summary>
        /// Sort order among pictures that are
        /// related to the same object.
        /// </summary>
        Int64 SortOrder { get; set; }

        /// <summary>
        /// Type of picture relation.
        /// </summary>
        IPictureRelationType Type { get; set; }
    }
}
