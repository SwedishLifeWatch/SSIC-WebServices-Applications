using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Information about name usage of taxon names,
    /// (Accepted, Synonym, Homotypic, Heterotypic, proParte synonym, Misapplied (auct. name)).
    /// </summary>
    public interface ITaxonNameUsage : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// Description of the taxon name usage.
        /// </summary>
        String Description
        { get; set; }

        /// <summary>
        /// Name of the taxon name usage.
        /// </summary>
        String Name
        { get; set; }
    }
}
