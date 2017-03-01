using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Interface that handles factor update mode.
    /// </summary>
    public interface IFactorUpdateMode : IDataId32
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Definition for this factor update mode.
        /// </summary>
        String Definition { get; set; }

        /// <summary>
        /// Indicates if factor is of type header.
        /// </summary>
        Boolean IsHeader { get; }

        /// <summary>
        /// Name for this factor update mode.
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// Type for this factor update mode.
        /// </summary>
        FactorUpdateModeType Type { get; set; }

        /// <summary>
        /// Manual update allowed for this factor update mode.
        /// </summary>
        Boolean AllowManualUpdate { get; }

        /// <summary>
        /// Automatic update allowed for this factor update mode.
        /// </summary>
        Boolean AllowAutomaticUpdate { get; }

        /// <summary>
        /// Update allowed for this factor update mode.
        /// </summary>
        Boolean AllowUpdate { get; set;  }
    }
}