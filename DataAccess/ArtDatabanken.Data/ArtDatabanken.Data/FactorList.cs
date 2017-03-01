using System;
using System.Collections.Generic;
using System.Linq;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IFactor interface.
    /// </summary>
    [Serializable]
    public class FactorList : DataId32List<IFactor>
    {
        /// <summary>
        /// Constructor for the FactorList class.
        /// </summary>
        public FactorList()
            : this(true)
        {
        }

        /// <summary>
        /// Constructor for the FactorList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public FactorList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get factor with specified id.
        /// </summary>
        /// <param name="factorId">factor id.</param>
        /// <returns>Factor with specified id.</returns>
        public IFactor Get(FactorId factorId)
        {
            return Get((Int32)factorId);
        }

        /// <summary>
        /// Get factors that are manually editable.
        /// </summary>
        /// <returns>Factors that are manually editable.</returns>
        public FactorList GetEditableFactors()
        {
            FactorList editableFactors;

            editableFactors = null;
            if (this.IsNotEmpty())
            {
                editableFactors = new FactorList();
                foreach (IFactor factor in this)
                {
                    if (factor.UpdateMode.AllowManualUpdate)
                    {
                        editableFactors.Add(factor);
                    }
                }
            }

            return editableFactors;
        }

        /// <summary>
        /// Get a subset of this factor list by search string.
        /// </summary>
        /// <param name="searchString">Search string.</param>
        /// <param name="comparisonType">Type of string comparison.</param>
        /// <returns>A factor list.</returns>
        public FactorList GetFactorsBySearchString(String searchString,
                                                   StringComparison comparisonType)
        {
            FactorList factors = new FactorList();
            var subset = from IFactor factor in this
                         where factor.Label.StartsWith(searchString, comparisonType)
                         orderby factor.Label ascending
                         select factor;
            factors.AddRange(subset.ToArray());
            return factors;
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
        private class SortOrderComparer : IComparer<IFactor>
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
            public int Compare(IFactor x, IFactor y)
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

                // if (idX > idY)
                return 1;
            }
        }
    }
}