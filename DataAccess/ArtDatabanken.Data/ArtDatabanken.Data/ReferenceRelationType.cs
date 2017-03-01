using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains definitions of reference relation types.
    /// </summary>
    public class ReferenceRelationType : IReferenceRelationType
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identifier for this reference relation type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this reference relation type.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this reference relation type.
        /// </summary>
        public String Identifier { get; set; }
    }
}
