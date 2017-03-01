using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class represents a factor origin.
    /// </summary>
    [Serializable]
    public class FactorOrigin : DataSortOrder
    {
        private String _name;
        private String _definition;

        /// <summary>
        /// Create a FactorOrigin instance.
        /// </summary>
        /// <param name='id'>Id for factor origin.</param>
        /// <param name='name'>Name of the factor origin.</param>
        /// <param name='definition'>Definition for the factor origin.</param>
        /// <param name='sortorder'>Sort order among factor origins.</param>
        public FactorOrigin(Int32 id, String name, String definition, Int32 sortorder)
            : base(id, sortorder)
        {
            _name = name;
            _definition = definition;
        }

        /// <summary>
        /// Get or set definition for this factor origin. 
        /// </summary>
        public String Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }
        
        /// <summary>
        /// Get name for this factor origin.
        /// </summary>
        public  String Name
        {
            get { return _name; }
        }
    }
}
