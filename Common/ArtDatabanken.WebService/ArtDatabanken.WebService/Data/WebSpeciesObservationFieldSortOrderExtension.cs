using System;
using ArtDatabanken.Data;

namespace ArtDatabanken.WebService.Data
{
    /// <summary>
    /// Contains extension to the WebSpeciesObservationFieldSortOrder class.
    /// </summary>
    public static class WebSpeciesObservationFieldSortOrderExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="sortOrder">Sort order.</param>
        public static void CheckData(this WebSpeciesObservationFieldSortOrder sortOrder)
        {
            if (sortOrder.IsNotNull())
            {
                sortOrder.Class.CheckNotNull("Class");
                sortOrder.Class.CheckData();
                sortOrder.Property.CheckNotNull("Property");
                sortOrder.Property.CheckData();
            }
        }

        /// <summary>
        /// Returns SpeciesObservationFieldSortOrder in sql sort order string.
        /// </summary>
        /// <param name="webSpeciesObservationFieldSortOrder">Species observation property.</param>
        /// <returns>Property as string.</returns>
        public static String GetSortOrder(this WebSpeciesObservationFieldSortOrder webSpeciesObservationFieldSortOrder)
        {
            String property = webSpeciesObservationFieldSortOrder.Property.Id.ToString();

            if (webSpeciesObservationFieldSortOrder.Property.Id == SpeciesObservationPropertyId.None)
                property = webSpeciesObservationFieldSortOrder.Property.Identifier;

            if (webSpeciesObservationFieldSortOrder.SortOrder == SortOrder.Ascending)
                return "[" + property + "]" + " asc";

            return "[" + property + "]" + " desc";
        }

        /// <summary>
        /// Get sort order in json format.
        /// </summary>
        /// <param name="sortOrder">Species observation sort order specification.</param>
        /// <returns>Sort order in json format.</returns>
        public static String GetSortOrderJson(this WebSpeciesObservationFieldSortOrder sortOrder)
        {
            String sort;

            if (sortOrder.IsNull())
            {
                return String.Empty;
            }
            else
            {
                switch (sortOrder.SortOrder)
                {
                    case SortOrder.Ascending:
                        sort = "asc";
                        break;
                    case SortOrder.Descending:
                        sort = "desc";
                        break;
                    default:
                        throw new Exception("Unknown sort order = " + sortOrder.SortOrder);
                }

                return "\"" + sortOrder.Class.GetClass() + "_" +
                       sortOrder.Property.GetProperty() +
                       "\": {\"order\": \"" + sort + "\"}";
            }
        }
    }
}
