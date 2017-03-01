using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a picture relation.
    /// </summary>
    public class PictureRelation : IPictureRelation
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for this picture relation.
        /// </summary>
        public Int64 Id { get; set; }

        /// <summary>
        /// Indicates if the picture that is referenced in
        /// this relation is the recommended picture for the
        /// related object.
        /// </summary>
        public Boolean IsRecommended { get; set; }

        /// <summary>
        /// GUID for the object that is related to a picture.
        /// </summary>
        public String ObjectGuid { get; set; }

        /// <summary>
        /// Id for the picture that the object is related to.
        /// </summary>
        public Int64 PictureId { get; set; }

        /// <summary>
        /// Sort order among pictures that are
        /// related to the same object.
        /// </summary>
        public Int64 SortOrder { get; set; }

        /// <summary>
        /// Type of picture relation.
        /// </summary>
        public IPictureRelationType Type { get; set; }
    }
}
