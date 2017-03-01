using System;
using System.Runtime.Serialization;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// This class handles search criterias that
    /// are used to find roles.
    /// The operator 'AND' is used between the
    /// different search conditions.
    /// </summary>
    [DataContract]
    public class WebRoleSearchCriteria : WebData
    {
        /// <summary>
        /// Find roles who have authorities related
        /// to the specified application action.
        /// </summary>
        [DataMember]
        public Int32 ApplicationActionId
        { get; set; }

        /// <summary>
        /// Find roles who have authorities related
        /// to the specified application.
        /// </summary>
        [DataMember]
        public Int32 ApplicationId
        { get; set; }

        /// <summary>
        /// Find roles with an identifier
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String Identifier
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
        /// Specifies if role type should be used.
        /// </summary>
        [DataMember]
        public Boolean IsRoleTypeSpecified
        { get; set; }

        /// <summary>
        /// Specifies if user id should be used.
        /// </summary>
        [DataMember]
        public Boolean IsUserIdSpecified
        { get; set; }

        /// <summary>
        /// Restrict search to users with accounts that
        /// are valid today.
        /// </summary>
        [DataMember]
        public Boolean IsValid
        { get; set; }

        /// <summary>
        /// Find roles for organizations that is categorized 
        /// as the specified organization category.
        /// </summary>
        [DataMember]
        public Int32 OrganizationCategoryId
        { get; set; }

        /// <summary>
        /// Find roles for the the specified organization.
        /// </summary>
        [DataMember]
        public Int32 OrganizationId
        { get; set; }

        /// <summary>
        /// Find roles with a name
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String Name
        { get; set; }

        /// <summary>
        /// Search for roles of this type.
        /// </summary>
        [DataMember]
        public RoleType RoleType
        { get; set; }

        /// <summary>
        /// Find roles with a shortname
        /// similar to the specified value.
        /// Wildcard characters may be used.
        /// </summary>
        [DataMember]
        public String ShortName
        { get; set; }

        /// <summary>
        /// Find roles which are related to the specified user.
        /// </summary>
        [DataMember]
        public Int32 UserId
        { get; set; }
    }
}
