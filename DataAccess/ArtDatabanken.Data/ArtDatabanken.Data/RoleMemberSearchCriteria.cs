using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class handles search criteria used when 
    /// searching rolemembers.
    /// </summary>
    public class RoleMemberSearchCriteria : IRoleMemberSearchCriteria
    {
        /// <summary>
        /// Find roles that are activated - value true
        /// Find roles that are not activated - value false
        /// </summary>
        public Boolean? IsActivated
        { get; set; }

        /// <summary>
        /// List of role id - restrict search with this list 
        /// </summary>
        public List<Int32> RoleIdList
        { get; set; }

        /// <summary>
        /// List of user id - restrict search with this list 
        /// </summary>
        public List<Int32> UserIdList
        { get; set; }

    }
}
