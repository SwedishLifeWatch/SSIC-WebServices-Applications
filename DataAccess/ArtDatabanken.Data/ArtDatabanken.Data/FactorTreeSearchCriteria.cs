using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class holds factor tree filter information.
    /// </summary>
    public class FactorTreeSearchCriteria : IFactorTreeSearchCriteria
    {
        /// <summary>
        /// Data context with meta information about this object.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Limit search to factors.
        /// </summary>
        public List<Int32> FactorIds { get; set; }
    }
}