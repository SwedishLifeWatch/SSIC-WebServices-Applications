using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface for data types that has 32 bits integer id.
    /// </summary>
    public interface IDataId32
    {
        /// <summary>
        /// Id for this data.
        /// </summary>
        Int32 Id { get; set; }
    }
}
