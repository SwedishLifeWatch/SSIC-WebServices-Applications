using System;


namespace ArtDatabanken.Data
{

    /// <summary>
    /// RoleMember
    /// </summary>
    public class RoleMember 
    {
        /// <summary>
        /// Role object 
        /// </summary>
        public IRole Role { get; set; }

        /// <summary>
        /// User who is member of this role
        /// </summary>
        public IUser User { get; set; }

        /// <summary>
        /// Is the rule activated for this user?
        /// </summary>
        public Boolean IsActivated{ get; set; }
    }
}
