using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for data types that has 64 bits integer id.
    /// </summary>
    public interface IDataId64
    {
        /// <summary>
        /// Id for this data.
        /// </summary>
        Int64 Id
        { get; set; }
    }
}
