using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonNameCategory interface.
    /// </summary>
    [Serializable()]
    public class TaxonNameCategoryList : DataId32List<ITaxonNameCategory>
    {
        /// <summary>
        /// Constructor for the TaxonNameCategoryList class.
        /// </summary>
        public TaxonNameCategoryList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonNameCategoryList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonNameCategoryList(Boolean optimize)
            : base(optimize)
        {
        }
    }
}

