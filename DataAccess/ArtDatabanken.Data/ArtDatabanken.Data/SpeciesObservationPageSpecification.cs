using System;


namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains parameters used to
    /// search for species observations.
    /// </summary>
    public class SpeciesObservationPageSpecification: ISpeciesObservationPageSpecification
    {
        /// <summary>
        /// Data context.
        /// </summary>
        public IDataContext DataContext { get; set; }

        /// <summary>
        /// Defines how species observations should be sorted
        /// before paging is applied to the result set.
        /// </summary>
        public SpeciesObservationFieldSortOrderList SortOrder
        { get; set; }

        private Int64 _size;

        /// <summary>
        /// Page size, i.e. how many species observations to be
        /// returned in each call to the web service
        /// Max page size is 10000 species observations.
        /// </summary>
        public Int64 Size
        {
            get { return _size; }
            set
            {
                if (!IsValidSize(value))
                    throw new ArgumentOutOfRangeException("Size", "Please enter a positive Size less than or equal to " + Settings.Default.SpeciesObservationPageMaxSize.WebToString());
                else
                    _size = value;
            }
        }

        private bool IsValidSize(Int64 size)
        {
            return (size > 0 && size <= Settings.Default.SpeciesObservationPageMaxSize);
        }

        /// <summary>
        /// Page start in the result set.
        /// </summary>
        public Int64 Start
        { get; set; }
    }
}
