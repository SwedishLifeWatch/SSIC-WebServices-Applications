using System;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This interface holds parameters used to
    /// search for species observations.
    /// </summary>
    public interface ISpeciesObservationPageSpecification
    {
        /// <summary>
        /// Data context.
        /// </summary>
        IDataContext DataContext { get; set; }

        /// <summary>
        /// Defines how species observations should be sorted
        /// before paging is applied to the result set.
        /// </summary>
        SpeciesObservationFieldSortOrderList SortOrder
        { get; set; }

        /// <summary>
        /// Page size, i.e. how many species observations to be
        /// returned in each call to the web service
        /// Max page size is 10000 species observations.
        /// </summary>
        Int64 Size
        { get; set; }

        /// <summary>
        /// Page start in the result set.
        /// </summary>
        Int64 Start
        { get; set; }
    }
}
