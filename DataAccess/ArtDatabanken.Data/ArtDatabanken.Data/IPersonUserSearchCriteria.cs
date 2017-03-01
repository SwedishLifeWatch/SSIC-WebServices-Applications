using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching users of type person user based on information about both the person and its user object.
    /// </summary>
    public interface IPersonUserSearchCriteria
    {
        /// <summary>
        /// Find persons with authority
        /// handling specified applicationaction.
        /// </summary>
        Int32? ApplicationActionId
        { get; set; }

        /// <summary>
        /// Find persons with role
        /// connected to specified application.
        /// </summary>
        Int32? ApplicationId
        { get; set; }

        /// <summary>
        /// Find persons with a city
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String City
        { get; set; }

        /// <summary>
        /// Find persons with an emailaddress
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String EmailAddress
        { get; set; }

        /// <summary>
        /// Find persons with a first name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String FirstName
        { get; set; }

        /// <summary>
        /// Find persons with a full name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String FullName
        { get; set; }

        /// <summary>
        /// Find persons with a last name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        String LastName
        { get; set; }

        /// <summary>
        /// Find persons with role connected to 
        /// organization with specified organization category.
        /// </summary>
        Int32? OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find persons with role
        /// connected to specified organization.
        /// </summary>
        Int32? OrganizationId
        { get; set; }
    }
}
