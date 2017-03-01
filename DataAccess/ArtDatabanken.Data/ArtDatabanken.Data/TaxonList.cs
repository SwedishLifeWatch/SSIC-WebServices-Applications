using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxon interface.
    /// </summary>
    [Serializable]
    public class TaxonList : DataId32List<ITaxon>
    {
        /// <summary>
        /// Constructor for the TaxonList class.
        /// </summary>
        public TaxonList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonList(Boolean optimize)
            : base(optimize)
        {
            
        }

        /// <summary>
        /// Get a copy of this taxon list.
        /// </summary>
        /// <returns>A copy of this taxon list</returns>
        public TaxonList Clone()
        {
            TaxonList taxa;

            taxa = new TaxonList(Optimize);
            taxa.AddRange(this);
            return taxa;
        }

        /// <summary>
        /// Sort taxa based on taxon sort order.
        /// </summary>
        public new void Sort()
        {
            Sort(new TaxonComparer());
        }

        /// <summary>
        /// Sort taxon based on taxon category sort order.
        /// </summary>
        public void SortTaxonCategory()
        {
            Sort(new TaxonCategoryComparer());
        }

        private class TaxonComparer : IComparer<ITaxon>
        {
            public int Compare(ITaxon x, ITaxon y)
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

        private class TaxonCategoryComparer : IComparer<ITaxon>
        {
            public int Compare(ITaxon x, ITaxon y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.Category.SortOrder;
                sortOrderY = y.Category.SortOrder;
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

