using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ArtDatabanken.Data.WebService;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  Enum that contains individual category ids for individual
    ///  categories that are specifically used by web service clients.
    /// </summary>
    public enum IndividualCategoryId
    {
        /// <summary>
        ///  Id for default individual category.
        /// </summary>
        Default = 0
    }

    /// <summary>
    ///  This class represents an individual category.
    /// </summary>
    [Serializable]
    public class IndividualCategory : DataSortOrder, IListableItem
    {
        private String _name;
        private String _definition;

        /// <summary>
        /// Create a IndividualCategory instance.
        /// </summary>
        /// <param name='id'>Id for the individual category.</param>
        /// <param name='name'>Name of the individual category.</param>
        /// <param name='definition'>Information for the individual category.</param>
        /// <param name='sortOrder'>Sortorder for the individual category.</param>
        public IndividualCategory(Int32 id, String name, String definition, Int32 sortOrder) : base(id, sortOrder)
        {
            _name = name;
            _definition = definition;
        }

        /// <summary>
        /// Get name for this individual category.
        /// </summary>
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Get definition for this individual category. 
        /// </summary>
        public String Definition
        {
            get { return _definition; }
            set { _definition = value; }
        }

        #region IListableItem Members
        
        /// <summary>
        /// A string usable as a display name
        /// </summary>
        public string Label
        {
            get { return _name; }
        }

        #endregion
    }
}
