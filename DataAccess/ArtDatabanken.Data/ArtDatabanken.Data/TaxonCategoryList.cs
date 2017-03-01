using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonCategory interface.
    /// </summary>
    [Serializable]
    public class TaxonCategoryList : DataId32List<ITaxonCategory>
    {
        /// <summary>
        /// Constructor for the TaxonCategoryList class.
        /// </summary>
        public TaxonCategoryList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonCategoryList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonCategoryList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Sort taxon categories based on taxon category sort order.
        /// </summary>
        public new void Sort()
        {
            Sort(new TaxonCategoryComparer());
        }

        private class TaxonCategoryComparer : IComparer<ITaxonCategory>
        {
            public int Compare(ITaxonCategory x, ITaxonCategory y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.SortOrder;
                sortOrderY = y.SortOrder;
                if (sortOrderX < sortOrderY)
                {
                    return -1;
                }
                if (sortOrderX == sortOrderY)
                {
                    return 0;
                }
                // if (sortOrderX > sortOrderY)
                return 1;
            }
        }
    }
}

