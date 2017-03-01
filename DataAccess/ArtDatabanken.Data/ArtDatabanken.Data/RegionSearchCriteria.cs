using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching regions.
    /// </summary>
    public class RegionSearchCriteria : IRegionSearchCriteria
    {
        /// <summary>
        /// Search regions that belongs to specified categories.
        /// </summary>
        public RegionCategoryList Categories
        { get; set; }

        /// <summary>
        /// Search regions that belongs to the specified countries.
        /// </summary>
        public List<Int32> CountryIsoCodes
        { get; set; }

        /// <summary>
        /// Search for regions with matching names.
        /// </summary>
        public StringSearchCriteria NameSearchString
        { get; set; }

        /// <summary>
        /// Search regions of specified type.
        /// </summary>
        public IRegionType Type
        { get; set; }
    }
}
