using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching regions.
    /// </summary>
    public interface IRegionSearchCriteria
    {
        /// <summary>
        /// Search regions that belongs to specified categories.
        /// </summary>
        RegionCategoryList Categories
        { get; set; }

        /// <summary>
        /// Search regions that belongs to the specified countries.
        /// </summary>
        List<Int32> CountryIsoCodes
        { get; set; }

        /// <summary>
        /// Search for regions with matching names.
        /// </summary>
        StringSearchCriteria NameSearchString
        { get; set; }

        /// <summary>
        /// Search regions of specified type.
        /// </summary>
        IRegionType Type
        { get; set; }
    }
}
