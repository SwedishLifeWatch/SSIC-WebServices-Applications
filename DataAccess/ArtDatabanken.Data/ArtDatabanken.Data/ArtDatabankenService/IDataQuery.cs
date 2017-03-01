using System;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains all query types.
    /// </summary>
    public enum DataQueryType
    {
        /// <summary>AndCondition</summary>
        AndCondition,
        /// <summary>NotCondition</summary>
        NotCondition,
        /// <summary>OrCondition</summary>
        OrCondition,
        /// <summary>SpeciesFactCondition</summary>
        SpeciesFactCondition
    }

    /// <summary>
    /// Generic interface for classes that implements data query
    /// related functionality.
    /// </summary>
    public interface IDataQuery
    {
        /// <summary>
        /// Get type of data query element.
        /// </summary>
        DataQueryType Type { get; }
    }
}
