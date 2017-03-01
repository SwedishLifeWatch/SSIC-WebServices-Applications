using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles information about a revision event type.
    /// </summary>
    public class TaxonRevisionState : ITaxonRevisionState
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Information about the taxon revision state.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Unique identification of a revision state.
        /// Mandatory ie always required.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for the revision state.
        /// </summary>
        public String Identifier { get; set; }
    }
}
