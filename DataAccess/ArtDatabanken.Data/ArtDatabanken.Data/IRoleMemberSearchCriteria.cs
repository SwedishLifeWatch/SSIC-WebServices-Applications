using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// Search criterias used when retrieving role members.
    /// </summary>
    public interface IRoleMemberSearchCriteria
    {
        /// <summary>
        /// Find roles that are activated - value true
        /// Find roles that are not activated - value false
        /// </summary>
        Boolean? IsActivated { get; set; }

        /// <summary>
        /// List of role id - restrict search with this list 
        /// </summary>
        List<Int32> RoleIdList { get; set; }

        /// <summary>
        /// List of user id - restrict search with this list 
        /// </summary>
        List<Int32> UserIdList { get; set; }
    }
}