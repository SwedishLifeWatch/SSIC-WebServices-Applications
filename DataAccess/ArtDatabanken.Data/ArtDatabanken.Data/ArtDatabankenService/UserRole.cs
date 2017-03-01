using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class contains information on role that a user has.
    /// </summary>
    public class UserRole : DataId
    {
        private String _description;
        private String _name;

        /// <summary>
        /// Create a UserRole instance.
        /// </summary>
        /// <param name='id'>Id for user role.</param>
        /// <param name='name'>Name of the user role.</param>
        /// <param name='description'>Description of the user role.</param>
        public UserRole(Int32 id, String name, String description)
            : base(id)
        {
            _name = name;
            _description = description;
        }

        /// <summary>
        /// Get description for this user role.
        /// </summary>
        public String Description
        {
            get { return _description; }
        }

        /// <summary>
        /// Get name for this user role.
        /// </summary>
        public String Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Use name as default string.
        /// </summary>
        public override String ToString()
        {
            return Name;
        }
    }
}
