using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IReference interface.
    /// </summary>
    [Serializable]
    public class ReferenceList : DataId32List<IReference>
    {
        /// <summary>
        /// Constructor for the ReferenceList class.
        /// </summary>
        public ReferenceList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the ReferenceList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public ReferenceList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}