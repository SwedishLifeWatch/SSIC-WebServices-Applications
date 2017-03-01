using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IIndividualCategory interface.
    /// </summary>
    [Serializable]
    public class IndividualCategoryList : DataId32List<IIndividualCategory>
    {
        /// <summary>
        /// Constructor for the IndividualCategoryList class.
        /// </summary>
        public IndividualCategoryList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the IndividualCategoryList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public IndividualCategoryList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}