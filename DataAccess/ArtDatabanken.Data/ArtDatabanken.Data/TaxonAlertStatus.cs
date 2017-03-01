using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Contains definitions of taxon alert statuses.
    /// </summary>
    public class TaxonAlertStatus : ITaxonAlertStatus
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Unique identifier for this taxon alert status.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Id for this taxon alert status.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Unique identifier for this taxon alert status.
        /// </summary>
        public String Identifier { get; set; }
    }
}
