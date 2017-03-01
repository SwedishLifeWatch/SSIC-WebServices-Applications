using System;
using System.Collections.Generic;
using ArtDatabanken.WebService.Data;

namespace ArtDatabanken.WebService.SwedishSpeciesObservationService.Data
{
    /// <summary>
    /// Contains extension methods to the
    /// WebSpeciesObservationPageSpecification class.
    /// </summary>
    public static class WebSpeciesObservationPageSpecificationExtension
    {
        /// <summary>
        /// Check that data is valid.
        /// </summary>
        /// <param name="webSpeciesObservationPageSpecification">Page specification.</param>
        public static void CheckData(this WebSpeciesObservationPageSpecification webSpeciesObservationPageSpecification)
        {
            if (!IsValidSize(webSpeciesObservationPageSpecification.Size))
                throw new ArgumentOutOfRangeException("Size", "Please enter a positive Size less than or equal to " + ArtDatabanken.Settings.Default.SpeciesObservationPageMaxSize.WebToString());

            if (webSpeciesObservationPageSpecification.Start <= 0)
                throw new ArgumentOutOfRangeException("Start", "Please enter a positive Start position.");

            if (webSpeciesObservationPageSpecification.SortOrder.IsNotNull())
            {
                foreach (WebSpeciesObservationFieldSortOrder webSpeciesObservationFieldSortOrder in webSpeciesObservationPageSpecification.SortOrder)
                {
                    if (webSpeciesObservationFieldSortOrder.Class.IsNull())
                        throw new ArgumentOutOfRangeException("Class", "Class (and Id) must be specified for sorting");
                    if (webSpeciesObservationFieldSortOrder.Class.Id.IsNull())
                        throw new ArgumentOutOfRangeException("Id", "Class Id must be specified for sorting");
                    if (webSpeciesObservationFieldSortOrder.Property.IsNull())
                        throw new ArgumentOutOfRangeException("Property", "Property (and ID) must be specified for sorting");
                    if (webSpeciesObservationFieldSortOrder.Property.Id.IsNull())
                        throw new ArgumentOutOfRangeException("Id", "Property Id must be specified for sorting");
                }

            }
        }

        private static bool IsValidSize(Int64 size)
        {
            return (size > 0 && size <= ArtDatabanken.Settings.Default.SpeciesObservationPageMaxSize);
        }

        /// <summary>
        /// Get sort order in SQL format.
        /// </summary>
        /// <param name="pageSpecification">Species observation page specification.</param>
        /// <param name="context">Web service request context.</param>
        /// <param name="searchCriteria">Species observation search criteria.</param>
        /// <returns>Sort order in SQL format.</returns>
        public static String GetSqlSortOrder(this WebSpeciesObservationPageSpecification pageSpecification,
                                             WebServiceContext context,
                                             WebSpeciesObservationSearchCriteria searchCriteria)
        {
            return pageSpecification.SortOrder.GetSqlSortOrder(context, searchCriteria, true);
        }

        /// <summary>
        /// The get end row.
        /// </summary>
        /// <param name="webSpeciesObservationPageSpecification">
        /// The web species observation page specification.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static Int64 GetEndRow(this WebSpeciesObservationPageSpecification webSpeciesObservationPageSpecification)
        {
            return webSpeciesObservationPageSpecification.Start + webSpeciesObservationPageSpecification.Size - 1;
        }
    }
}