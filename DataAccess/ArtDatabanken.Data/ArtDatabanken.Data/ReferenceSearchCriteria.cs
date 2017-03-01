using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Search criteria used when references are retrieved.
    /// </summary>
    public class ReferenceSearchCriteria : IReferenceSearchCriteria
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Specify how search criteria should be logically combined
        /// when more than one search criteria is specified.
        /// Only logical operator And and Or are supported.
        /// </summary>
        public LogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// Search references based on names of references.
        /// </summary>
        public IStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Search references based on titles on references.
        /// </summary>
        public IStringSearchCriteria TitleSearchString { get; set; }

        /// <summary>
        /// Search references based on years.
        /// </summary>
        public List<Int32> Years { get; set; }
    }
}
