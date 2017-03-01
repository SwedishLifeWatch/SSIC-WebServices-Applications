using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPerson interface.
    /// </summary>
    [Serializable]
    public class PersonList : DataId32List<IPerson>
    {
        /// <summary>
        /// Constructor for the PersonList class.
        /// </summary>
        public PersonList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the PersonList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public PersonList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}
