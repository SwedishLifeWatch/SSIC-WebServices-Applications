using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching organizations.
    /// </summary>
    public interface IOrganizationSearchCriteria
    {

        /// <summary>
        /// Find organizations that are
        /// owners of a spieces collection
        /// </summary>
        Boolean? HasSpiecesCollection
        { get; set; }

        /// <summary>
        /// Find organizations with category id
        /// equals to the specified id value.
        /// </summary>
        Int32? OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find organizations with a name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Find organizations with a short name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String ShortName
        { get; set; }

    }
}
