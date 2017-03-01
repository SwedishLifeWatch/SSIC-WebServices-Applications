using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArtDatabanken.Data.ArtDatabankenService
{
    /// <summary>
    /// List class for the UserRole class.
    /// </summary>
    public class UserRoleList : DataIdList
    {
        /// <summary>
        /// Get UserRole with specified id.
        /// </summary>
        /// <param name='id'>Id of requested user role.</param>
        /// <returns>Requested user role.</returns>
        /// <exception cref="ArgumentException">Thrown if no data has the requested id.</exception>
        public UserRole Get(Int32 id)
        {
            return (UserRole)(GetById(id));
        }

        /// <summary>
        /// Get/set UserRole by list index.
        /// </summary>
        public new UserRole this[Int32 index]
        {
            get
            {
                return (UserRole)(base[index]);
            }
            set
            {
                base[index] = value;
            }
        }
    }
}
