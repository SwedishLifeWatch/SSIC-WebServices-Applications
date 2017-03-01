using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface represents a species fact quality.
    /// </summary>
    public interface ISpeciesFactQuality : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this species fact quality.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Name for this species fact quality.
        /// </summary>
        String Name { get; set; }
    }
}