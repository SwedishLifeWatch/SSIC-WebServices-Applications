using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the ITaxonSpeciesObservationCount interface.
    /// </summary>
    [Serializable]
    public class TaxonSpeciesObservationCountList : DataId32List<ITaxonSpeciesObservationCount>
    {
        /// <summary>
        /// Constructor for the TaxonSpeciesObservationCountList class.
        /// </summary>
        public TaxonSpeciesObservationCountList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the TaxonSpeciesObservationCountList class.
        /// </summary>
        /// <param name="optimize">
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public TaxonSpeciesObservationCountList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Sort taxa based on taxon sort order.
        /// </summary>
        public new void Sort()
        {
            Sort(new TaxonSpeciesObservationCountComparer());
        }

        private class TaxonSpeciesObservationCountComparer : IComparer<ITaxonSpeciesObservationCount>
        {
            public int Compare(ITaxonSpeciesObservationCount x, ITaxonSpeciesObservationCount y)
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
                return 1;
            }
        }
    }
}