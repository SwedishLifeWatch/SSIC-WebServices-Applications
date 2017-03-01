using System.Collections.Generic;

namespace ArtDatabanken.Data
{
    /// <summary>
    /// This class contains extension methods for ITaxonNameStatus.
    /// </summary>
    public static class ITaxonNameStatusExtension
    {
        /// <summary>
        /// This dictionary holds the sort order for taxon status Id values.
        /// </summary>
        private static Dictionary<int, int> _dicSortOrder;

        /// <summary>
        /// Gets the sort order.
        /// </summary>
        /// <param name="taxonNameStatus">
        /// The taxon Name Status.
        /// </param>
        /// <returns>
        /// The sort order.
        /// </returns>
        public static int SortOrder(this ITaxonNameStatus taxonNameStatus)
        {
            int sortOrder;
            if (_dicSortOrder.TryGetValue(taxonNameStatus.Id, out sortOrder))
            {
                return sortOrder;
            }

            return 100;
        }


        /// <summary>
        /// Initializes the <see cref="ITaxonNameStatusExtension"/> class.
        /// Creates the sort order table where the keys are taxon status id.
        /// </summary>
        static ITaxonNameStatusExtension()
        {
            _dicSortOrder = new Dictionary<int, int>();
            _dicSortOrder.Add((int)TaxonNameStatusId.ApprovedNaming, 0);
            _dicSortOrder.Add((int)TaxonNameStatusId.Provisional, 1);
            _dicSortOrder.Add((int)TaxonNameStatusId.PreliminarySuggestion, 2);
            _dicSortOrder.Add((int)TaxonNameStatusId.Informal, 3);
            _dicSortOrder.Add((int)TaxonNameStatusId.Obsrek, 4);
            _dicSortOrder.Add((int)TaxonNameStatusId.Suppressed, 5);
            _dicSortOrder.Add((int)TaxonNameStatusId.IncorrectCitation, 6);
            _dicSortOrder.Add((int)TaxonNameStatusId.Misspelled, 7);
            _dicSortOrder.Add((int)TaxonNameStatusId.Unneccessary, 8);
            _dicSortOrder.Add((int)TaxonNameStatusId.Undescribed, 9);
            _dicSortOrder.Add((int)TaxonNameStatusId.Unpublished, 10);
            _dicSortOrder.Add((int)TaxonNameStatusId.InvalidNaming, 11);
            _dicSortOrder.Add((int)TaxonNameStatusId.Removed, 12);
        }
    }
}