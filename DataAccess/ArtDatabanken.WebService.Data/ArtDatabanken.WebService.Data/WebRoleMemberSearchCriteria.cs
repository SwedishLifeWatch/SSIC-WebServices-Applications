using System;
using System.Runtime.Serialization;

namespace ArtDatabanken.WebService.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// This class handles search criterias that
    /// are used to find members (users) of roles.
    /// The operator 'AND' is used between the
    /// different search conditions.
    /// </summary>
    [DataContract]
    public class WebRoleMemberSearchCriteria : WebData
    {
        /// <summary>
        /// List of authority guids. 
        /// THIS CRITERIA IS NOT YET IMPLEMENTED.
        /// </summary>
        [DataMember]
        public List<String> AuthorityGuids
        { get; set; }

        /// <summary>
        /// Restrict search to rolemembers that are activated.
        /// </summary>
        [DataMember]
        public Boolean IsActivated
        { get; set; }

        /// <summary>
        /// Specifies if IsActivated should be used.
        /// </summary>
        [DataMember]
        public Boolean IsIsActivatedSpecified
        { get; set; }

        /// <summary>
        /// List of organization ids. 
        /// THIS CRITERIA IS NOT YET IMPLEMENTED.
        /// </summary>
        [DataMember]
        public List<Int32> OrganizationIds
        { get; set; }

        /// <summary>
        /// List of role ids. 
        /// </summary>
        [DataMember]
        public List<Int32> RoleIds
        { get; set; }

        /// <summary>
        /// List of user ids. 
        /// </summary>
        [DataMember]
        public List<Int32> UserIds
        { get; set; }
    }
}
