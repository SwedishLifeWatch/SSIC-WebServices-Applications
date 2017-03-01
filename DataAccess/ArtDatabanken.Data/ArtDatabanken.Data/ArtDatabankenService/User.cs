using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    ///  This class contains information about a user.
    /// </summary>
    public class User : DataId
    {
        private String _firstName;
        private String _lastName;
        private String _userName;
        private UserRoleList _roles;

        /// <summary>
        /// Create a User instance.
        /// </summary>
        /// <param name='id'>Id for user.</param>
        /// <param name='firstName'>The users first name.</param>
        /// <param name='lastName'>The users last name.</param>
        /// <param name='userName'>The users user name.</param>
        /// <param name='roles'>This users roles.</param>
        public User(Int32 id, String firstName, String lastName, String userName, UserRoleList roles)
            : base(id)
        {
            _firstName = firstName;
            _lastName = lastName;
            _userName = userName;
            _roles = roles;
        }

        /// <summary>
        /// Get first name for this user.
        /// </summary>
        public String FirstName
        {
            get { return _firstName; }
        }

        /// <summary>
        /// Get full name (first and last name) for this user.
        /// </summary>
        public String FullName
        {
            get { return this.FirstName + ' ' + this.LastName; }
        }

        /// <summary>
        /// Get last name for this user.
        /// </summary>
        public String LastName
        {
            get { return _lastName; }
        }

        /// <summary>
        /// Get roles that this user has.
        /// </summary>
        public UserRoleList Roles
        {
            get { return _roles; }
        }

        /// <summary>
        /// Get user name for this user.
        /// </summary>
        public String UserName
        {
            get { return _userName; }
        }
    }
}
