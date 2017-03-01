using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonRelation interface.
    /// </summary>
    public class TaxonRelationList : DataId32List<ITaxonRelation>
    {
        /// <summary>
        /// Sort taxon relations based on sort order.
        /// </summary>
        public new void Sort()
        {
            Sort(new TaxonRelationComparer());
        }

        /// <summary>
        /// Sort taxon relations based on
        /// parent taxon category sort order.
        /// </summary>
        public void SortTaxonCategory()
        {
            Sort(new TaxonCategoryComparer());
        }

        /// <summary>
        /// Sort taxon relations based on
        /// child taxon category sort order.
        /// </summary>
        public void SortChildTaxon()
        {
            Sort(new ChildTaxonComparer());
        }

        /// <summary>
        /// Sort taxon relations based on
        /// parent taxon sort order.
        /// </summary>
        public void SortParentTaxon()
        {
            Sort(new ParentTaxonComparer());
        }

        private class ChildTaxonComparer : IComparer<ITaxonRelation>
        {
            public int Compare(ITaxonRelation x, ITaxonRelation y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.ChildTaxon.SortOrder;
                sortOrderY = y.ChildTaxon.SortOrder;
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

        private class ParentTaxonComparer : IComparer<ITaxonRelation>
        {
            public int Compare(ITaxonRelation x, ITaxonRelation y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.ParentTaxon.SortOrder;
                sortOrderY = y.ParentTaxon.SortOrder;
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

        private class TaxonCategoryComparer : IComparer<ITaxonRelation>
        {
            public int Compare(ITaxonRelation x, ITaxonRelation y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.ParentTaxon.Category.SortOrder;
                sortOrderY = y.ParentTaxon.Category.SortOrder;
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

        private class TaxonRelationComparer : IComparer<ITaxonRelation>
        {
            public int Compare(ITaxonRelation x, ITaxonRelation y)
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
