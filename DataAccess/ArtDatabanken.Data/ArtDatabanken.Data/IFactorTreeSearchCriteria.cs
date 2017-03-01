using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds factor tree filter information.
    /// </summary>
    public interface IFactorTreeSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Limit search to factors.
        /// </summary>
        List<Int32> FactorIds { get; set; }
    }
}