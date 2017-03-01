using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles information about a lump split event type.
    /// </summary>
    public class LumpSplitEventType : ILumpSplitEventType
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the lump split event type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for the lump split event type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the lump split event type.
        /// </summary>
        public String Identifier { get; set; }
    }
}
