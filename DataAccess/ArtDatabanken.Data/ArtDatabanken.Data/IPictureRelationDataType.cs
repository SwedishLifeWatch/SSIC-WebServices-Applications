using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A picture relation data type contains information about which
    /// data type a picture is related to in a picture relation.
    /// </summary>
    public interface IPictureRelationDataType : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this picture relation data type.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation data type.
        /// </summary>
        String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation data type.
        /// </summary>
        String Name { get; set; }
    }
}
