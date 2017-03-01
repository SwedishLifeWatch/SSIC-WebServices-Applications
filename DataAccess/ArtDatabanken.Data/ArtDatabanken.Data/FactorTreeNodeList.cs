using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IFactorTreeNode interface.
    /// </summary>
    [Serializable]
    public class FactorTreeNodeList : DataId32List<IFactorTreeNode>
    {
        /// <summary>
        /// Constructor for the FactorTreeNodeList class.
        /// </summary>
        public FactorTreeNodeList()
            : base(true)
        {
        }

        /// <summary>
        /// Sort items based on sort order.
        /// </summary>
        public new void Sort()
        {
            Sort(new SortOrderComparer());
        }

        /// <summary>
        /// Private class that compares two items based on sort order.
        /// </summary>
        private class SortOrderComparer : IComparer<IFactorTreeNode>
        {
            /// <summary>
            /// Compares two objects and returns a value
            /// indicating whether one is less than, equal
            /// to, or greater than the other.
            /// </summary>
            /// <param name='x'>The first object to compare.</param>
            /// <param name='y'>The second object to compare.</param>
            /// <returns>
            /// A signed integer that indicates the relative values of x and y.
            /// If x is less than y less than zero is returned.
            /// If x equals y zero is returned.
            /// If x is greater than y greater than zero is returned.
            /// </returns>
            public int Compare(IFactorTreeNode x, IFactorTreeNode y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.Factor.SortOrder;
                sortOrderY = y.Factor.SortOrder;
                if (sortOrderX < sortOrderY)
                {
                    return -1;
                }

                if (sortOrderX == sortOrderY)
                {
                    return 0;
                }

                // if (idX > idY)
                return 1;
            }
        }
    }
}