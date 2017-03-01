using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles information about a revision event type.
    /// </summary>
    public class TaxonRevisionEventType : ITaxonRevisionEventType
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Name of the revision event type.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Unique identification of a revision event type.
        /// Mandatory ie always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier of the revision event type.
        /// </summary>
        public String Identifier { get; set; }
    }
}
