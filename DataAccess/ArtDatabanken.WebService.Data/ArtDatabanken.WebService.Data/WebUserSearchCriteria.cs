using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class handles search criterias that
    /// are used to find users.
    /// The operator 'AND' is used between the
    /// different search conditions.
    /// </summary>
    [DataContract]
    public class WebUserSearchCriteria : WebData
    {
        /// <summary>
        /// Find persons who have addresses
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String Address
        { get; set; }

        /// <summary>
        /// Find persons who have authorities related
        /// to the specified application action.
        /// </summary>
        [DataMember]
        public Int32 ApplicationActionId
        { get; set; }

        /// <summary>
        /// Find persons who have authorities related
        /// to the specified application.
        /// </summary>
        [DataMember]
        public Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// Find persons who lives in the specified city.
        /// No wildcard characters are used.
        /// </summary>
        [DataMember]
        public String City
        { get; set; }

        /// <summary>
        /// Find persons who lives in the specified country.
        /// No wildcard characters are used.
        /// </summary>
        [DataMember]
        public String CountryISOCode
        { get; set; }

        /// <summary>
        /// Find users who have an email address
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String EmailAddress
        { get; set; }

        /// <summary>
        /// Find persons with a first name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String FirstName
        { get; set; }

        /// <summary>
        /// Find persons with a full name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String FullName
        { get; set; }

        /// <summary>
        /// Specifies if application action id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsApplicationActionIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if application id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsApplicationIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if is valid should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsValidSpecified
        { get; set; }

        /// <summary>
        /// Specifies if organization category id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsOrganizationCategoryIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if organization id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsOrganizationIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if role id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsRoleIdSpecified
        { get; set; }

        /// <summary>
        /// Specifies if user type should be used.
        /// </summary>
        [DataMember]
        public Boolean IsUserTypeSpecified
        { get; set; }

        /// <summary>
        /// Restrict search to users with accounts that
        /// are valid today.
        /// </summary>
        [DataMember]
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// Find persons with a last name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String LastName
        { get; set; }

        /// <summary>
        /// Find persons with a middle name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String MiddleName
        { get; set; }

        /// <summary>
        /// Find persons who have authorities related
        /// to the specified organization category.
        /// </summary>
        [DataMember]
        public Int32 OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find persons who have authorities related
        /// to the specified organization.
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        { get; set; }

        /// <summary>
        /// Find persons with the specified phone number.
        /// No wildcard characters are used.
        /// </summary>
        [DataMember]
        public String PhoneNumber
        { get; set; }

        /// <summary>
        /// Find persons who have authorities related
        /// to the specified role.
        /// </summary>
        [DataMember]
        public Int32 RoleId
        { get; set; }

        /// <summary>
        /// Search for users of this type.
        /// </summary>
        [DataMember]
        public UserType UserType
        { get; set; }

        /// <summary>
        /// Find persons who lives in the specified postal area.
        /// No wildcard characters are used.
        /// </summary>
        [DataMember]
        public String ZipCode
        { get; set; }
    }
}
