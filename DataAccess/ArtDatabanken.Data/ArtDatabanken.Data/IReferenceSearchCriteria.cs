using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Search criteria used when references are retrieved.
    /// </summary>
    public interface IReferenceSearchCriteria
    {
        /// <summary>
        /// Meta information about this data.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Specify how search criteria should be logically combined
        /// when more than one search criteria is specified.
        /// Only logical operator And and Or are supported.
        /// </summary>
        LogicalOperator LogicalOperator { get; set; }

        /// <summary>
        /// Search references based on names of references.
        /// </summary>
        IStringSearchCriteria NameSearchString { get; set; }

        /// <summary>
        /// Search references based on titles on references.
        /// </summary>
        IStringSearchCriteria TitleSearchString { get; set; }

        /// <summary>
        /// Search references based on years.
        /// </summary>
        List<Int32> Years { get; set; }
    }
}
