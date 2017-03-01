using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// This class holds factor tree filter information.
    /// </summary>
    public class FactorTreeSearchCriteria
    {
        private List<Int32> _restrictSearchToFactorIds;

        /// <summary>
        /// Create a FactorTreeSearchCriteria instance.
        /// </summary>
        public FactorTreeSearchCriteria()
        {
            _restrictSearchToFactorIds = null;
        }

        /// <summary>
        /// Limit search to factors.
        /// </summary>
        public List<Int32> RestrictSearchToFactorIds
        {
            get { return _restrictSearchToFactorIds; }
            set { _restrictSearchToFactorIds = value; }
        }

        /// <summary>
        /// Check that data is valid.
        /// </summary>
        public void CheckData()
        {
            // Nothing to check yet.
        }
    }
}
