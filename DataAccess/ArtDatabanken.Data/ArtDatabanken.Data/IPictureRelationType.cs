using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a picture relation type.
    /// </summary>
    public interface IPictureRelationType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Picture relations of this type is related to this data type.
        /// </summary>
        IPictureRelationDataType DataType { get; set; }

        /// <summary>
        /// Description for this picture relation type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation type.
        /// </summary>
        String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation type.
        /// </summary>
        String Name { get; set; }
    }
}
