using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching organizations.
    /// </summary>
    public class OrganizationSearchCriteria : IOrganizationSearchCriteria
    {
        /// <summary>
        /// Find organizations that are
        /// owners of a spieces collection
        /// </summary>
        public Boolean? HasSpiecesCollection
        { get; set; }

        /// <summary>
        /// Find organizations with category id
        /// equals to the specified id value.
        /// </summary>
        public Int32? OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find organizations with a name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String Name
        { get; set; }

        /// <summary>
        /// Find organizations with a short name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String ShortName
        { get; set; }

    }
}
