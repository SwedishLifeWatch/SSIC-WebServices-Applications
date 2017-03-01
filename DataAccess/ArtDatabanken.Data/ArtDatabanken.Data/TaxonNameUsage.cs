using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about name usage of taxon names,
    /// (Accepted, Synonym, Homotypic, Heterotypic, proParte synonym, Misapplied (auct. name)).
    /// </summary>
    public class TaxonNameUsage : ITaxonNameUsage
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Description of the taxon name usage.
        /// </summary>
        public String Description
        { get; set; }

        /// <summary>
        /// Id for this taxon name usage.
        /// </summary>
        public Int32 Id
        { get; set; }

        /// <summary>
        /// Name of the taxon name usage.
        /// </summary>
        public String Name
        { get; set; }
    }
}
