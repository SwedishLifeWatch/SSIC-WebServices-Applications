using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class represents a picture relation type.
    /// </summary>
    public class PictureRelationType : IPictureRelationType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Picture relations of this type is related to this data type.
        /// </summary>
        public IPictureRelationDataType DataType { get; set; }

        /// <summary>
        /// Description for this picture relation type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this picture relation type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation type.
        /// </summary>
        public String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation type.
        /// </summary>
        public String Name { get; set; }
    }
}
