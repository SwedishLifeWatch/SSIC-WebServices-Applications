using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A picture relation data type contains information about which
    /// data type a picture is related to in a picture relation.
    /// </summary>
    public class PictureRelationDataType : IPictureRelationDataType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Description for this picture relation data type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this picture relation data type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this picture relation data type.
        /// </summary>
        public String Identifier { get; set; }

        /// <summary>
        /// Name for this picture relation data type.
        /// </summary>
        public String Name { get; set; }
    }
}
