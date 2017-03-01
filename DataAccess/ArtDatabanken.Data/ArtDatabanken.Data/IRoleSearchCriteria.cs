using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching roles.
    /// </summary>
    public interface IRoleSearchCriteria
    {

        /// <summary>
        /// Find roles with an Identifier
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String Identifier
        { get; set; }

        /// <summary>
        /// Find roles for organizations with id
        /// equals to the specified id value.
        /// </summary>
        Int32? OrganizationId
        { get; set; }

        /// <summary>
        /// Find roles with a role name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String Name
        { get; set; }

        /// <summary>
        /// Find roles with a short name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String ShortName
        { get; set; }

    }
}
