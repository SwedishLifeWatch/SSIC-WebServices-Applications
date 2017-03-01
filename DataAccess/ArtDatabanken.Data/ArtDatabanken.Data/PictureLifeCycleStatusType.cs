using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a picture life cycle status type.
    /// </summary>
    public class PictureLifeCycleStatusType : IPictureLifeCycleStatusType
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Id for this picture life cycle status type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this picture life cycle status type.
        /// </summary>
        public String Identifier { get; set; }

        /// <summary>
        /// Name for this picture life cycle status type.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Description for this picture life cycle status type.
        /// </summary>
        public String Description { get; set; }
    }
}
