using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about an address.
    /// </summary>
    public interface ISpeciesActivityCategory : IDataId32
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext
        { get; set; }

        /// <summary>
        /// GUID for this species activity.
        /// </summary>
        String Guid { get; set; }

        /// <summary>
        /// Identifier for this species activity.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Name for this species activity category.
        /// </summary>
        String Name
        { get; set; }
    }
}
