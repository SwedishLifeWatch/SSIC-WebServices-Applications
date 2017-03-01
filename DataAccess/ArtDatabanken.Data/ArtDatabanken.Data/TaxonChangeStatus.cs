using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains definitions of taxon change statuses.
    /// </summary>
    public class TaxonChangeStatus : ITaxonChangeStatus
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identifier for this taxon change status.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this taxon change status.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this taxon change status.
        /// </summary>
        public String Identifier { get; set; }
    }
}
