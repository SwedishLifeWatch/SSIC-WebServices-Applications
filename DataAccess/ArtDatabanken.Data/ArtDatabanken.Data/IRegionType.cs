using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    ///  This interface represents an region type,
    ///  for example "Political boundary" or "Validate regions".
    /// </summary>
    public interface IRegionType : IDataId32
    {
        /// <summary>
        /// Get data context.
        /// </summary>
        IDataContext DataContext
        { get; }

        /// <summary>
        /// Region type name.
        /// </summary>
        String Name
        { get; }
    }
}
