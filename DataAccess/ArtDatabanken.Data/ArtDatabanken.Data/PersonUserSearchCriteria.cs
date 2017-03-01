using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface handles search criteria used when 
    /// searching users of type person user based on information about both the person and its user object.
    /// </summary>
    public class PersonUserSearchCriteria : IPersonUserSearchCriteria
    {

        /// <summary>
        /// Find persons with authority
        /// handling specified applicationaction.
        /// </summary>
        public Int32? ApplicationActionId
        { get; set; }

        /// <summary>
        /// Find persons with role
        /// connected to specified application.
        /// </summary>
        public Int32? ApplicationId
        { get; set; }

        /// <summary>
        /// Find persons with a city
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String City
        { get; set; }

        /// <summary>
        /// Find persons with an emailaddress
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String EmailAddress
        { get; set; }

        /// <summary>
        /// Find persons with a first name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String FirstName
        { get; set; }

        /// <summary>
        /// Find persons with a full name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String FullName
        { get; set; }

        /// <summary>
        /// Find persons with a last name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        public String LastName
        { get; set; }

        /// <summary>
        /// Find persons with role connected to 
        /// organization with specified organization category.
        /// </summary>
        public Int32? OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find persons with role
        /// connected to specified organization.
        /// </summary>
        public Int32? OrganizationId
        { get; set; }

    }
}
