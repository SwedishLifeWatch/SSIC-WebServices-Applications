using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about status of taxon names,
    /// (approved, tentatively, invalid, etc.).
    /// </summary>
    public class TaxonNameStatus : ITaxonNameStatus
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Description of the taxon name status.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// Id for this taxon name status.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name of the taxon name status.
        /// </summary>
        public String Name
        { get; set; }
    }
}
