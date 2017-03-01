using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles information about an authority type.
    /// </summary>
    public interface IAuthorityDataType : IDataId32
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Identifier.
        /// </summary>
        String Identifier
        { get; set; }

    }
}
