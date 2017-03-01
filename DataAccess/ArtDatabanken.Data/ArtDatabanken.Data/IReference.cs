using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// A reference.
    /// </summary>
    public interface IReference : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Name of the person that made the last
        /// update of this reference.
        /// </summary>
        String ModifiedBy { get; set; }

        /// <summary>
        /// Date when the reference was last updated.
        /// </summary>
        DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Name of the reference.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Title for the reference.
        /// </summary>
        String Title { get; set; }

        /// <summary>
        /// Reference year.
        /// </summary>
        Int32? Year { get; set; }
    }
}
