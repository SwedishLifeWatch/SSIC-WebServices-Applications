using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IUser interface.
    /// </summary>
    [Serializable]
    public class UserList : DataId32List<IUser>
    {
        /// <summary>
        /// Constructor for the UserList class.
        /// </summary>
        public UserList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the UserList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public UserList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}
