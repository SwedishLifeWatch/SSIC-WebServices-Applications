using System;
using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// List class for the IPeriod interface.
    /// </summary>
    [Serializable]
    public class PeriodList : DataId32List<IPeriod>
    {
        /// <summary>
        /// Constructor for the PeriodList class.
        /// </summary>
        public PeriodList()
            : this(false)
        {
        }

        /// <summary>
        /// Constructor for the PeriodList class.
        /// </summary>
        /// <param name='optimize'>
        /// Indicates if speed optimization should be turned on.
        /// This optimization assumes that each id only occurs once
        /// in the list.
        /// </param>
        public PeriodList(Boolean optimize)
            : base(optimize)
        {
        }

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        public PeriodList GetPeriods(Int32 periodTypeId)
        {
            PeriodList periods;

            periods = new PeriodList();
            foreach (IPeriod period in this)
            {
                if (period.Type.Id == periodTypeId)
                {
                    periods.Add(period);
                }
            }

            return periods;
        }

        /// <summary>
        /// Get all periods of a certain period type.
        /// </summary>
        /// <param name="periodTypeId">Period type id.</param>
        /// <returns>All periods of a certain period type.</returns>
        public PeriodList GetPeriods(PeriodTypeId periodTypeId)
        {
            return GetPeriods((Int32)periodTypeId);
        }

        /// <summary>
        /// Sort periods based on year.
        /// </summary>
        public new void Sort()
        {
            Sort(new PeriodComparer());
        }

        private class PeriodComparer : IComparer<IPeriod>
        {
            public int Compare(IPeriod x, IPeriod y)
            {
                Int32 sortOrderX, sortOrderY;

                sortOrderX = x.Year;
                sortOrderY = y.Year;
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